using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;

namespace AEP.HQAMC.Branding.Features.DeploySiteLog
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("7570fdc6-3d18-4476-84e9-15b6bcab4022")]
    public class DeploySiteLogEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {

            SPSite site = properties.Feature.Parent as SPSite;
            site.AllowUnsafeUpdates = true;
            if (site != null)          
            {
                SPWebCollection subSites = site.AllWebs;
                site.RootWeb.SiteLogoUrl = site.ServerRelativeUrl + "/_layouts/15/images/AEP.HQAMC.Branding.SIPR/HQAMC.png";
                site.RootWeb.CustomMasterUrl = site.ServerRelativeUrl + "/_catalogs/masterpage/AEP_HQAMC.master";
                site.RootWeb.Update();
                site.AllowUnsafeUpdates = true;
                foreach (SPWeb subSite in subSites)
                {
                    subSite.AllowUnsafeUpdates = true;
                    subSite.SiteLogoUrl = site.RootWeb.SiteLogoUrl;
                    //1/6 apply master page to root site and subsite might not work on subsite needs further testing

                    //string sharePointServerPublishing = "f6924d36-2fa8-4f0b-b16d-06b7250180fa";
                    //Guid sharePointServerPublishingGuid = new Guid(sharePointServerPublishing);
                    //subSite.Features.Add(sharePointServerPublishingGuid, true);

                    // site.UIVersion = 4;


                    subSite.CustomMasterUrl = site.RootWeb.CustomMasterUrl;
                    
                    
                    //end apply master page to site 1/6
                    subSite.Update();
                    subSite.Dispose();

                    subSite.AllowUnsafeUpdates = false;

                }


                
            }
            site.AllowUnsafeUpdates = false;
            site.Dispose();
        }
        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


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
