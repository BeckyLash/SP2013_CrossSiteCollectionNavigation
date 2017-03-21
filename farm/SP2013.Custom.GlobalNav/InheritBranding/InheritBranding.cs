using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;


namespace Custom.SP2013.Branding.InheritBranding
{
    /// <summary>
    /// Web Events
    /// </summary>
    public class InheritBranding : SPWebEventReceiver
    {
        /// <summary>
        /// A site was provisioned.
        /// </summary>
        public override void WebProvisioned(SPWebEventProperties properties)
        {
            SPDiagnosticsService diagSvc = SPDiagnosticsService.Local;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    base.WebProvisioned(properties);
                    var rootWeb = properties.Web.Site.RootWeb;
                    var currentWeb = properties.Web;
                    var siteCollection = currentWeb.Site;

                    using (currentWeb)
                    {

                        var webAppRelativePath = BrandingHelper.WebAppPath(currentWeb, siteCollection);
                        BrandingHelper.InheritTopSiteBranding(currentWeb, rootWeb, siteCollection, webAppRelativePath);


                    }
                }

           
               

                catch (Exception ex)
                {
                    diagSvc.WriteTrace(0, new SPDiagnosticsCategory("Error Info", TraceSeverity.Monitorable, EventSeverity.Error), TraceSeverity.Monitorable, "Houston, we have a problem in WebProvisioned event: {0}---{1}", ex.InnerException, ex.StackTrace);
                }
        });

        }

       
    }
}