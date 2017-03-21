using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;

namespace AEP.HQAMC.Branding.ControlTemplates.AEP.HQAMC.GlobalNav
{
    public partial class AEPHQAMCKeyboardMenu2 : UserControl
    {
        public static string html { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite thisSite = new SPSite(SPContext.Current.Site.WebApplication.AlternateUrls[0].Uri.AbsoluteUri))
                    {
                        html = "";
                        TaxonomySession session = new TaxonomySession(thisSite);
                        TermStoreCollection store = session.TermStores;

                        try
                        {
                            foreach (TermStore termStore in session.TermStores)
                            {

                                var string1 = termStore.Name.ToString();
                                Group navGroup = termStore.Groups["AEP HQAMC Navigation"];
                                foreach (TermSet topSet in navGroup.TermSets)
                                {
                                    html += writeTerms(topSet.Terms);

                                }
                            }
                        }
                        catch
                        {
                        }

                        finally
                        {
                            AEP_HQAMC_GlobalNavContainer2.Text = "";
                            AEP_HQAMC_GlobalNavContainer2.Text = html;
                        }
                    }
                });
            }
        }

        public string writeTerms(TermCollection terms)
        {
            var tabInt = 0; 
            var stringMenu = "<span title = \"Settings\" class=\"ms-siteactions-normal ms-siteactions-hover\" id=\"zz12_SiteActionsMenu_t\" onmouseover=\"MMU_PopMenuIfShowing(this);MMU_EcbTableMouseOverOut(this, true)\" onclick =\" CoreInvoke('MMU_Open',byid('AEPNav'), MMU_GetMenuFromClientId('zz12_SiteActionsMenu'),event,true, null, 0); return false;\" oncontextmenu =\"ClkElmt(this); return false;\" foa=\"MMU_GetMenuFromClientId('zz12_SiteActionsMenu')\" hoverinactive=\"ms-siteactions-normal\" hoveractive=\"ms-siteactions-normal ms-siteactions-hover\"><a title = \"Settings\" class=\"ms-core-menu-root\" id = \"zz12_SiteActionsMenu\" accesskey=\"/\" onkeydown=\"MMU_EcbLinkOnKeyDown(byid('zz5_SiteActionsMenuMain'), MMU_GetMenuFromClientId('zz12_SiteActionsMenu'));\" href=\"javascript:;\" serverclientid=\"zz12_SiteActionsMenu\" menutokenvalues=\"MENUCLIENTID=zz12_SiteActionsMenu,TEMPLATECLIENTID=AEPNav           \"><span class=\"ms-siteactions-imgspan\"><img title = \"Settings\" class=\"ms-core-menu-buttonIcon\" alt=\"Settings\" src=\"/sites/test1/_catalogs/theme/Themed/AEBDBD80/spcommon-B35BB0A9.themedpng?ctag=224\"></span><span class=\"ms-accessible\">Use SHIFT+ENTER to open the menu(new window).</span></a></span>";
            if (terms.Count > 0)
            {
                //html += "\n<ul class=\"AEPHQAMCGlobalNav\">\n";
                html += "\n<span><menu id=\"AEPNav\" type=\"ServerMenu\" hideicons=\"true\">";

                foreach (Term subTerm in terms)
                {


                    try
                    {

                        //html += "<li class=\"\"><a  tabindex=\"" + tabInt + "\" href=\"" + subTerm.LocalCustomProperties["_Sys_Nav_SimpleLinkUrl"] + "\">" + subTerm.Name + "</a>";
                        html += "<ie:menuitem  tabindex=\"" + tabInt + "\" id=\"ct" + subTerm.Id + "\" type=\"option\" menuGroupId=\"200\" description=\"" + "description" + "\" onMenuClick=\"window.location =\'" + subTerm.LocalCustomProperties["_Sys_Nav_SimpleLinkUrl"] + "\" text=\"" + subTerm.Name + "></ie:menuitem>";
                        writeTerms(subTerm.Terms);
                        html += "\n";

                    }

                    catch
                    {
                        //html += "<li class=\"\"><a   tabindex=\"" + tabInt + "\" href=\"#\">" + subTerm.Name + "</a>";
                        html += "<ie:menuitem  tabindex=\"" + tabInt + "\" id=\"ct" + subTerm.Id + "\" type=\"option\" menuGroupId=\"200\" description=\"" + "description" + "\" onMenuClick=\"window.location =\'" + subTerm.LocalCustomProperties["_Sys_Nav_SimpleLinkUrl"] + "\" text=\"" + subTerm.Name + "></ie:menuitem>";
                        writeTerms(subTerm.Terms);
                        html += "\n";

                    }

                    //tabInt++;

                }

                html += "</menu></span>\n" + stringMenu             ;
            }
            return html;

        }
    }
}

