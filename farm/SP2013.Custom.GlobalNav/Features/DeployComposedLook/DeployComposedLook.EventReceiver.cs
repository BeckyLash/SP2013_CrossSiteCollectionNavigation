
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Microsoft.SharePoint;
using SP2013.Custom.Branding;

namespace Custom.SP2013.Branding.Features.DeployComposedLook
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("036f72ed-1877-46a8-9723-4af5c2b977c4")]
    public class DeployComposedLookEventReceiver : SPFeatureReceiver
    {
        //guid for stapling template feature - activate only on site collection
        //guid provision web theme feature - activate only on site collection
        string guidProvisionWebTheme = CustomBranding.DeployComposedLookSiteNavEventReceiver_guidProvisionWebTheme_a32b6912_77e8_459d_ad50_e387c465320f;
        string customLookCustom = CustomBranding.DeployComposedLookSiteNavEventReceiver_customLookCustom_Custom_SP2013_ComposedLook;
        string catalogMasterPage = CustomBranding.DeployComposedLookSiteNavEventReceiver_catalogMasterPage__catalogs_masterpage_;


        // Uncomment the method below to handle the event raised after a feature has been activated.
     
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            //order of events important: 
            //Add the Composed Look Files  - declaratively optional
            //Add the Master Page � Add a custom master page
            //Create a new Composed Look item 
            //Apply the Master Page � 
            //Apply the Composed Look(aka.Theme) 
            //Update the Current Composed Look Item
            //NOte that subsites get spfont and spcolor from composed look, but not master page; must explicitly set properties on SPWeb to get master page from parent without publishing feature 

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {

                var site = properties.Feature.Parent as SPSite;
                var masterPageName = properties.Definition.Properties["MasterPage"].Value;


                if (site != null)
                {
                    var topWeb = site.RootWeb;
                    var masterUrl = new SPFieldUrlValue();
                    var webAppRelativePath = BrandingHelper.WebAppPath(topWeb, site);
                    //apply everything to TopWeb first to get handle on master page for feature activation
                    masterUrl.Url = webAppRelativePath + catalogMasterPage + masterPageName;
                    var themeUrl = new SPFieldUrlValue();
                    var fontUrl = new SPFieldUrlValue();

                    themeUrl.Url = webAppRelativePath + CustomBranding.DeployComposedLookSiteNavEventReceiver_FeatureActivated__catalogs_theme_15_CustomLOGO_spcolor;
                    fontUrl.Url = webAppRelativePath + CustomBranding.DeployComposedLookSiteNavEventReceiver_FeatureActivated__catalogs_theme_15_fontscheme002_spfont;

                    //important: update master page after creating item for composed look in list
                    BrandingHelper.CreateComposedLookListItem(topWeb, customLookCustom, masterUrl, themeUrl, fontUrl);
                    topWeb.Update();
                    //Open web theme only on rootWeb
                    BrandingHelper.ApplyMasterPageToRootWeb(topWeb, webAppRelativePath, catalogMasterPage, masterPageName);
                    var theme = BrandingHelper.ApplyThemeToRootWeb(topWeb, webAppRelativePath, customLookCustom, themeUrl, fontUrl);
                    BrandingHelper.UpdateCurrentComposedLookListItem(topWeb, customLookCustom, masterUrl, themeUrl, fontUrl);
                    //activate ProvisionWebTheme              
                    //BrandingHelper.ActivateProvisionWebThemeFeature(site, guidCustom);
                    BrandingHelper.ActivateProvisionWebThemeFeature(site, guidProvisionWebTheme);
                    topWeb.Update();
                    // Enumerate through each site and apply branding.

                    foreach (SPWeb currentWeb in site.AllWebs)
                    {
                        if (currentWeb.Url != topWeb.Url)
                        {
                            //must apply master page in this order-after create composed look list item
                            currentWeb.MasterUrl = webAppRelativePath + catalogMasterPage + masterPageName;
                            currentWeb.CustomMasterUrl = webAppRelativePath + catalogMasterPage + masterPageName;
                            currentWeb.SiteLogoUrl = webAppRelativePath + catalogMasterPage + CustomBranding.ProvisionSiteThemeEventReceiver_FeatureActivated_Custom_SP2013_LOGO_png;
                            currentWeb.Update();
                            using (new CustomPerformanceTimerLogger("DeployedComposedLooktoSubsites", currentWeb.Url.ToString()))
                            { 
                                BrandingHelper.ApplyComposedLook(theme, currentWeb);
                                
                            }
                            //good practice in this type of loop with ApplyTo method
                            currentWeb.Dispose();
                        }
                    }

                    topWeb.AllowUnsafeUpdates = false;

                }
            });
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {

                var siteCollection = properties.Feature.Parent as SPSite;



                if (siteCollection != null)
                {
                    var topWeb = siteCollection.RootWeb;

                    var webAppRelativePath = BrandingHelper.WebAppPath(topWeb, siteCollection);
                    var masterPageName = properties.Definition.Properties["MasterPage"].Value;

                    topWeb.AllowUnsafeUpdates = true;
                    //check if top web has master page custom.master applied
                    if (topWeb.MasterUrl.Replace(webAppRelativePath + catalogMasterPage, "") == masterPageName)
                    {

                        // Calculate relative path to site from Web Application root.
                        var designGallery = topWeb.GetCatalog(SPListTemplateType.DesignCatalog);
                        if (null == designGallery)
                        {
                            return;
                        }

                        SPQuery query = new SPQuery();
                        query.RowLimit = 1;
                        query.Query = "<Where><Eq><FieldRef Name='Name'/><Value Type='Text'>" + customLookCustom + "</Value></Eq></Where>";
                        query.ViewFields = "<FieldRef Name='Name'/>";
                        query.ViewFieldsOnly = true;
                        var currentItems = designGallery.GetItems(query);

                        if (currentItems.Count == 1)
                        {
                            currentItems[0].Delete();
                            designGallery.Update();
                        }

                        BrandingHelper.DeactivateProvisionWebThemeFeature(siteCollection, guidProvisionWebTheme);
                        BrandingHelper.RemoveComposedLookTopSite(topWeb, siteCollection, webAppRelativePath);
                        //update current look in composed look list
                        //SPFile customMasterFile = topLevelSite.GetFile(topLevelSite + "_catalogs/masterpage/Custom.master");
                        var customMasterFile = topWeb.GetFile(topWeb.Url + catalogMasterPage + masterPageName);
                        //check if Custom.master is applied to the top web to run rest of feature
                        //if not customized, remove masterpage and associated files                    
                        BrandingHelper.RemoveMasterPage(topWeb, customMasterFile);
                        topWeb.Update();

                        //    //remove dependency on devicechannel
                        foreach (SPWeb currentWeb in siteCollection.AllWebs)
                        {

                            //if mobile feature turned on, must loop through each web and remove master page association with devicechannelmappings.aspx
                            var deviceChannelMappings = currentWeb.Url + CustomBranding.ProvisionSiteThemeEventReceiver_FeatureDeactivating___catalogs_masterpage___devicechannelmappings_aspx;
                            BrandingHelper.RemoveAssocationDeviceChannelMapping(masterPageName, deviceChannelMappings, currentWeb);
                            //dispose in this kind of loop in feature activation good practice 
                            currentWeb.Dispose();
                        }
                        //now we can delete master page
                        if (customMasterFile.Exists)
                        {
                            customMasterFile.Delete();
                        }
                        customMasterFile.Update();

                        topWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }




        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
