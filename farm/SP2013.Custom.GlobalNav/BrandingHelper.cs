using Microsoft.SharePoint;
using System;
using System.Linq;
using Microsoft.SharePoint.Utilities;
using System.IO;
using System.Globalization;
using SP2013.Custom.Branding;

namespace Custom.SP2013.Branding
{
    public class BrandingHelper
    {
        public static string WebAppPath(SPWeb currentWeb, SPSite siteCollection)
        {
            var rootWeb = siteCollection.RootWeb;
            var webAppRelativePath = rootWeb.ServerRelativeUrl;
           // webAppRelativePath = webAppRelativePath == "/" ? rootWeb.Url : rootWeb.ServerRelativeUrl;

            if (!webAppRelativePath.EndsWith("/"))
            {
                webAppRelativePath += "/";
            }
            return webAppRelativePath;

        }
        public static SPTheme ApplyThemeToRootWeb(SPWeb topWeb, String webAppRelativePath, string customLookCustom, SPFieldUrlValue themeUrl, SPFieldUrlValue fontUrl)
        {
            //order of this method important
            //3. apply comp look 4. update current in comp look list
            var themeFile = topWeb.GetFile(themeUrl.Url);
            var fontSchemeFile = topWeb.GetFile(fontUrl.Url);
            var theme = SPTheme.Open(customLookCustom, themeFile, fontSchemeFile);
            theme.ApplyTo(topWeb, true);
            return theme;


        }
        public static void ApplyMasterPageToRootWeb(SPWeb rootWeb, String webAppRelativePath, String catalogMasterPage, String masterPageName)
        {
            var masterUrl = new SPFieldUrlValue();
            masterUrl.Url = webAppRelativePath + catalogMasterPage + masterPageName;
            rootWeb.MasterUrl = webAppRelativePath + catalogMasterPage + masterPageName;
            rootWeb.CustomMasterUrl = webAppRelativePath + catalogMasterPage + masterPageName;
            rootWeb.SiteLogoUrl = webAppRelativePath + catalogMasterPage + CustomBranding.ProvisionSiteThemeEventReceiver_FeatureActivated_Custom_SP2013_LOGO_png;
            rootWeb.Update();
        }
        public static void ActivateProvisionWebThemeFeature(SPSite site, String guid)
        {

           
                if (site.Features[new Guid(guid)] == null)
                {

                    site.Features.Add(new Guid(guid), true);

                }
           

        }
        public static void DeactivateProvisionWebThemeFeature(SPSite site, String guid)
        {

           
                if (site.Features[new Guid(guid)] != null)
                {

                    site.Features.Remove(new Guid(guid), true);

                }
           



        }
        public static void RemoveMasterPage(SPWeb topWeb, SPFile customMasterFile)
        {
            if (customMasterFile.Exists && customMasterFile.CustomizedPageStatus == SPCustomizedPageStatus.Uncustomized)
            {
                //invalid port 1-28
                var themeFile = topWeb.GetFile(topWeb.Url + CustomBranding.BrandingHelper_RemoveMasterPage___catalogs_theme_15_Custom_spcolor);
                var customPreview = topWeb.GetFile(topWeb.Url + CustomBranding.BrandingHelper_RemoveMasterPage___catalogs_masterpage_Custom_SP2013_preview);
                var customCssFile = topWeb.GetFile(topWeb.Url + CustomBranding.BrandingHelper_RemoveMasterPage___catalogs_masterpage_Custom_SP2013_Custom_css);
                var CustomHeaderImage = topWeb.GetFile(topWeb.Url + CustomBranding.BrandingHelper_RemoveMasterPage___catalogs_masterpage_Custom_SP2013_CustomHeader_png);
                var CustomSiteLogo = topWeb.GetFile(topWeb.Url + CustomBranding.BrandingHelper_RemoveMasterPage___catalogs_masterpage_Custom_SP2013_logo_png);
                if (themeFile.Exists)
                {
                    themeFile.Delete();
                }
                themeFile.Update();

                if (customPreview.Exists)
                {
                    customPreview.Delete();
                }
                customPreview.Update();
                if (customCssFile.Exists)
                {
                    customCssFile.Delete();
                }
                customCssFile.Update();
                if (customMasterFile.Name.ToString() == CustomBranding.BrandingHelper_RemoveMasterPage_Custom_SP2013_master)
                {
                    var customCssFileHideNav = topWeb.GetFile(topWeb.Url + CustomBranding.BrandingHelper_RemoveMasterPage___catalogs_masterpage_Custom_SP2013_CustomHideNav_css);
                    if (customCssFileHideNav.Exists)
                    {
                        customCssFileHideNav.Delete();
                    }
                    customCssFileHideNav.Update();
                }
                if (customMasterFile.Name.ToString() == CustomBranding.BrandingHelper_RemoveMasterPage_Custom_SP2013CustomSiteNav_master)
                {
                    var customPreviewSiteNav = topWeb.GetFile(topWeb.Url + CustomBranding.BrandingHelper_RemoveMasterPage___catalogs_masterpage_Custom_SP2013SiteNav_preview);
                    if (customPreviewSiteNav.Exists)
                    {
                        customPreviewSiteNav.Delete();
                    }
                    customPreviewSiteNav.Update();
                }
                if (CustomHeaderImage.Exists)
                {
                    CustomHeaderImage.Delete();
                }
                CustomHeaderImage.Update();
                if (CustomSiteLogo.Exists)
                {
                    CustomSiteLogo.Delete();
                }
                CustomSiteLogo.Update();

            }
        }

        public static void RemoveAssocationDeviceChannelMapping(String masterFileName, String deviceChannelMappings, SPWeb web2)
        { 
              var dcmFile = web2.GetFile(deviceChannelMappings);
                    if (dcmFile.Exists)
                    {
                        var dcmFileStream = dcmFile.OpenBinaryStream();
        var dcmFileWrite = new MemoryStream();

                        using (var sw = new StreamWriter(dcmFileWrite))
                        using (var sr = new StreamReader(dcmFileStream))
                        {
                            string line;
    var foundCorrection = false;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (line.Contains(masterFileName))
                                {
                                    foundCorrection = true;
                                    line = line.Replace(masterFileName, "seattle.master");
                                }
                            sw.WriteLine(line);
                            }
                            sw.Flush();
                            if (foundCorrection)
                            {
                                if (dcmFile.CheckOutType != SPFile.SPCheckOutType.None)
                                    dcmFile.UndoCheckOut();
                                if (dcmFile.RequiresCheckout)
                                {
                                    dcmFile.CheckOut();
                                    dcmFile.SaveBinary(dcmFileWrite);
                                    dcmFile.CheckIn("Updated master page references to default.");
                                }
                                else
                                {
                                    dcmFile.SaveBinary(dcmFileWrite);
                                }
                                if (dcmFile.Level == SPFileLevel.Draft)
                                {
                                    dcmFile.Publish("Updated master page references to default.");
                                }
                                dcmFile.Update();
                            }
                        }


                    }


        }

        public static void CreateComposedLookListItem(SPWeb web, String customLook, SPFieldUrlValue masterUrl, SPFieldUrlValue themeUrl, SPFieldUrlValue fontUrl)
        {
            var match = false;
            var list = web.Lists["Composed Looks"];
            var currentLooks = list.GetItems();

            foreach (SPListItem itemUpdate in currentLooks.Cast<SPListItem>().Where(itemUpdate => itemUpdate.Name == customLook))
            {
                match = true;

                itemUpdate["MasterPageUrl"] = masterUrl.Url;
                itemUpdate["ThemeUrl"] = themeUrl.Url;
                itemUpdate["FontSchemeUrl"] = fontUrl.Url;
                itemUpdate["DisplayOrder"] = 5;
                itemUpdate["ImageUrl"] = null;
                itemUpdate.Update();
            }
            if (match) return;
            var newItem = list.AddItem();
            newItem["Title"] = customLook;
            newItem["Name"] = customLook;
            newItem["MasterPageUrl"] = masterUrl.Url;
            newItem["ThemeUrl"] = themeUrl.Url;
            newItem["FontSchemeUrl"] = fontUrl.Url;
            newItem["DisplayOrder"] = 5;
            //newItem["ImageUrl"] = bgImageURL;

            newItem.Update();
        }
        public static void ApplyComposedLook(SPTheme theme, SPWeb currentWebSite)
        {
            //old SPWeb topWeb, SPWeb currentWebSite, String customLook, SPFieldUrlValue masterURL, SPFieldUrlValue themeURL, SPFieldUrlValue fontURL


            //SPFile themeFile = topWeb.GetFile(themeURL.Url);
            //SPFile fontSchemeFile = topWeb.GetFile(fontURL.Url);
            //SPTheme theme = SPTheme.Open(customLook, themeFile, fontSchemeFile);
            theme.ApplyTo(currentWebSite, true);
            currentWebSite.Update();
            ////redundant
            // currentWebSite.ApplyTheme(themeURL.Url.ToString(), fontURL.Url.ToString(), null, true);


        }
        public static void RemoveComposedLookTopSite(SPWeb topWeb, SPSite site, String webAppRelativePath)
        {

            var themeUrl = new SPFieldUrlValue();
            var fontUrl = new SPFieldUrlValue();
            var logoUrl = new SPFieldUrlValue();
            //SPFieldUrlValue bgImageURL = new SPFieldUrlValue();
            var masterUrl = new SPFieldUrlValue();
            //if paths incorrect, files don't delete correct theme is not applied 
            masterUrl.Url = webAppRelativePath + CustomBranding.BrandingHelper_RemoveComposedLookTopSite__catalogs_masterpage_seattle_master;
            themeUrl.Url = webAppRelativePath + CustomBranding.BrandingHelper_RemoveComposedLookTopSite__catalogs_theme_15_palette001_spcolor;
            fontUrl.Url = webAppRelativePath + CustomBranding.BrandingHelper_RemoveComposedLookTopSite__catalogs_theme_15_fontscheme002_spfont;
            logoUrl.Url = webAppRelativePath + CustomBranding.BrandingHelper_RemoveComposedLookTopSite__layouts_15_images_siteicon_png;
            //these paths must be web relative with serverrelative url 1-28 changed for root
            topWeb.CustomMasterUrl = webAppRelativePath + CustomBranding.BrandingHelper_RemoveComposedLookTopSite__catalogs_masterpage_seattle_master;
            topWeb.MasterUrl = webAppRelativePath + CustomBranding.BrandingHelper_RemoveComposedLookTopSite__catalogs_masterpage_seattle_master;
            topWeb.SiteLogoUrl = webAppRelativePath + CustomBranding.BrandingHelper_RemoveComposedLookTopSite__layouts_15_images_siteicon_png;
            topWeb.Update();

            var themeFile = topWeb.GetFile(themeUrl.Url);

            var fontSchemeFile = topWeb.GetFile(fontUrl.Url);

            var theme = SPTheme.Open("Office", themeFile, fontSchemeFile);
            theme.ApplyTo(topWeb, true);
            //next two lines possibly redundant 
            //topWeb.ApplyTheme(themeURL.Url.ToString(), fontURL.Url.ToString(), null, true);
            //topWeb.Update();

            // Enumerate through each site and apply out of box branding.
            foreach (SPWeb web in site.AllWebs)
            {
                if (web.Url == topWeb.Url) continue;
                web.CustomMasterUrl = masterUrl.Url;
                web.MasterUrl = masterUrl.Url;
                web.SiteLogoUrl = logoUrl.Url;
                theme.ApplyTo(web, true);
                web.Update();
                //next two lines possibly redundant               
                // web.ApplyTheme(themeURL.Url.ToString(), fontURL.Url.ToString(), null, true);
            }

            var list = topWeb.Lists["Composed Looks"];
            var currentLooks = list.GetItems();

            foreach (SPListItem itemUpdate in currentLooks.Cast<SPListItem>().Where(itemUpdate => itemUpdate.Name == "Current"))
            {
                itemUpdate["MasterPageUrl"] = masterUrl;
                itemUpdate["ThemeUrl"] = themeUrl;
                itemUpdate["FontSchemeUrl"] = fontUrl;
                itemUpdate["ImageUrl"] = null;
                itemUpdate.Update();
            }
        }
        public static void UpdateCurrentComposedLookListItem(SPWeb currentWebSite, String customLook, SPFieldUrlValue masterUrl, SPFieldUrlValue themeUrl, SPFieldUrlValue fontUrl)
        {
            var designGallery = currentWebSite.GetCatalog(SPListTemplateType.DesignCatalog);
            if (null == designGallery)
            {

                return;
            }

            SPQuery query = new SPQuery();
            query.RowLimit = 1;
            query.Query = "<Where><Eq><FieldRef Name='DisplayOrder'/><Value Type='Number'>0</Value></Eq></Where>";
            query.ViewFields = "<FieldRef Name='DisplayOrder'/>";
            query.ViewFieldsOnly = true;

            var currentItems = designGallery.GetItems(query);

            if (currentItems.Count == 1)
            {
                // Remove the old Current item.
                currentItems[0].Delete();
            }

            var currentItem = designGallery.AddItem();

            currentItem["Name"] = SPResource.GetString(CultureInfo.CurrentUICulture, Strings.DesignGalleryCurrentItemName);
            currentItem["Title"] = SPResource.GetString(CultureInfo.CurrentUICulture, Strings.DesignGalleryCurrentItemName);

            // Change this line if you want to specify a different master page.
            currentItem["MasterPageUrl"] = masterUrl;
            currentItem["ThemeUrl"] = themeUrl;


            currentItem["FontSchemeUrl"] = fontUrl;
            currentItem["DisplayOrder"] = 0;
            currentItem.Update();

        }

        public static void InheritTopSiteBranding(SPWeb currentWeb, SPWeb rootWeb, SPSite site, string webAppRelativePath)
        {

            var catalogMasterPage = CustomBranding.DeployComposedLookSiteNavEventReceiver_catalogMasterPage__catalogs_masterpage_;
            var masterPageName = CustomBranding.BrandingHelper_RemoveMasterPage_Custom_SP2013_master;
            var masterPageNameSiteNav = CustomBranding.BrandingHelper_RemoveMasterPage_Custom_SP2013CustomSiteNav_master;

            //if master page is applied change current spweb
            //need to check because must apply theme .spcolor from file path; need consistent results for end user
            if (rootWeb.MasterUrl == webAppRelativePath + catalogMasterPage + masterPageName || rootWeb.MasterUrl == webAppRelativePath + catalogMasterPage + masterPageNameSiteNav)
            {
                var themeUrl = new SPFieldUrlValue();
                var fontUrl = new SPFieldUrlValue();
                currentWeb.MasterUrl = rootWeb.MasterUrl;
                currentWeb.CustomMasterUrl = rootWeb.MasterUrl;
                themeUrl.Url = webAppRelativePath + CustomBranding.InheritBranding_WebProvisioned__catalogs_theme_15_Custom_spcolor;
                fontUrl.Url = webAppRelativePath + CustomBranding.InheritBranding_WebProvisioned__catalogs_theme_15_fontscheme002_spfont;
                currentWeb.SiteLogoUrl = webAppRelativePath + CustomBranding.InheritBranding_WebProvisioned__catalogs_masterpage_Custom_SP2013_LOGO_png;
                currentWeb.Update();
                //apply font and color to current SPWeb
                var themeFile = rootWeb.GetFile(themeUrl.Url);
                var fontSchemeFile = rootWeb.GetFile(fontUrl.Url);
                var customLook = CustomBranding.InheritBranding_WebProvisioned_Custom_SP2013_ComposedLook;
                var theme = SPTheme.Open(customLook, themeFile, fontSchemeFile);
                theme.ApplyTo(currentWeb, true);

            }
        }


    }
}
