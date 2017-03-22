using System;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;
using System.Reflection;
using System.Resources;
using Microsoft.SharePoint.WebControls;

namespace Custom.SP2013.GlobalNav.ControlTemplates.Custom.SP2013.GlobalNav
{
    [MdsCompliant(true)]
    public partial class Custom : UserControl
    {

        public static string html { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            //if (!Page.IsPostBack)
            //{
                SPSecurity.RunWithElevatedPrivileges(delegate ()

                {
                    ResourceManager rm = new ResourceManager("SP2013.Custom.Branding.CustomBranding", Assembly.GetExecutingAssembly());
                    string error = rm.GetString("TermError");
                    string errorServiceApp = rm.GetString("TermServiceAppError");

                    try
                    {
                       
                        SPSite thisSite = SPContext.Current.Site;

                      
                        if (thisSite != null)
                        {
                           
                                html = "";
                                TaxonomySession session = new TaxonomySession(thisSite);

                      //          TermStoreCollection store = session.TermStores;


                                try
                                {
                                    foreach (TermStore termStore in session.TermStores)
                                    {

                                        
                                       
                                        var termStoreName = Attributes["CustomTermStoreName"];
                                        var navGroup = termStore.Groups[termStoreName];

                                        foreach (TermSet topSet in navGroup.TermSets)
                                        {
                                            html += WriteTerms(topSet.Terms);

                                        }
                                    }
                                }
                                catch
                                {

                                    html = error;

                                }

                                finally
                                {
                                    Custom_SP2013_GlobalNavContainer.Text = "";
                                    Custom_SP2013_GlobalNavContainer.Text = html;
                                }


                            }


                        
                    }
                    catch
                    {
                        html = errorServiceApp;
                         Custom_SP2013_GlobalNavContainer.Text = html; 

                    }

                    finally
                    {
                       
                        Custom_SP2013_GlobalNavContainer.Text = html;

                    }
                });


            //}
        }

        public string WriteTerms(TermCollection terms)
        {
            var tabInt = 0;
            if (terms.Count > 0)
            {
                html += "\n<ul class=\"CustomSP2013GlobalNav\">\n";
               
                foreach (Term subTerm in terms)
                {
                   
                    
                    try
                    {
                        
                        html += "<li class=\"\"><a class=\"dynamic\" tabindex=\"" + tabInt + "\" href=\"" + subTerm.LocalCustomProperties["_Sys_Nav_SimpleLinkUrl"] + "\">" + subTerm.Name + "</a>";
                        WriteTerms(subTerm.Terms);
                        html += "</li>\n";
                        
                    }
                     
                    catch
                    {
                        html += "<li class=\"\"><a  class=\"dynamic\" tabindex=\"" + tabInt + "\" href=\"#\">" + subTerm.Name + "</a>";
                       WriteTerms(subTerm.Terms);
                        html += "</li>\n";
                        
                    }

                  

                }

                html += "</ul>\n";
            }
            return html;

        }
    }
}
