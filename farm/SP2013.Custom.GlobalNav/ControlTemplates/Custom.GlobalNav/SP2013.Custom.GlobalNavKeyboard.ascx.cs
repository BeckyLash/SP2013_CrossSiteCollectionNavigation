using System;
using System.Web.UI;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;
using System.Resources;
using System.Reflection;


namespace Custom.SP2013.Branding.ControlTemplates.Custom.SP2013.Branding
{
    public partial class Custom : UserControl
    {
       
        public static string html { get; set; }

       protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()

                {
                    ResourceManager rm = new ResourceManager("SP2013.Custom.Branding.CustomBranding", Assembly.GetExecutingAssembly());
                    string error = rm.GetString("TermError");
                    string errorServiceApp = rm.GetString("TermServiceAppError");
                    try
                    {
                        SPSite thisSite = new SPSite(SPContext.Current.Site.WebApplication.AlternateUrls[0].Uri.AbsoluteUri);

                        if (thisSite != null)
                        {
                            using (thisSite)
                            {
                                html = "";
                                TaxonomySession session = new TaxonomySession(thisSite);
                        TermStoreCollection store = session.TermStores;
                       

                        try
                        {
                            foreach (TermStore termStore in session.TermStores)
                            {

                                //var string1 = termStore.Name.ToString();
                                //Group navGroup = termStore.Groups["Custom SP2013 Navigation"];
                                var string1 = termStore.Name.ToString();
                                var termStoreName = Attributes["CustomTermStoreName"].ToString();
                                Group navGroup = termStore.Groups[termStoreName];

                                foreach (TermSet topSet in navGroup.TermSets)
                                {
                                    html += writeTerms(topSet.Terms);

                                }
                            }
                        }
                        catch
                        {
                            html = error; ;
                        }

                                finally
                                {
                                    Custom_SP2013_GlobalNavContainer_Keyboard.Text = "";
                                    Custom_SP2013_GlobalNavContainer_Keyboard.Text = html;
                                }


                            }


                        }
                    }
                    catch
                    {
                        html = errorServiceApp;
                        Custom_SP2013_GlobalNavContainer_Keyboard.Text = html;

                    }

                    finally
                    {

                        Custom_SP2013_GlobalNavContainer_Keyboard.Text = html;

                    }
                });


            }
        }

        public string writeTerms(TermCollection terms)
        {
            var tabInt = 0;
            if (terms.Count > 0)
            {
                //html += "\n<ul class=\"CustomSP2013GlobalNav\">\n";
                html += "\n<ul class=\"\">\n";

                foreach (Term subTerm in terms)
                {

                  
                    try
                    {
                        

                        html += "<li class=\"\"><a  tabindex=\"" + tabInt + "\" onclick=\"window.opener.location.href = this.href; return false;\" href=\"" + subTerm.LocalCustomProperties["_Sys_Nav_SimpleLinkUrl"] + "\">" + subTerm.Name + "</a>";
                        writeTerms(subTerm.Terms);
                        html += "</li>\n";

                    }

                    catch
                    {
                        html += "<li class=\"\"><a   tabindex=\"" + tabInt + "\" onclick=\"window.opener.location.href = this.href; return false;\" href=\"#\">" + subTerm.Name + "</a>";
                        writeTerms(subTerm.Terms);
                        html += "</li>\n";

                    }

                    //tabInt++;

                }

                html += "</ul>\n";
            }
            return html;

        }
    }
}
