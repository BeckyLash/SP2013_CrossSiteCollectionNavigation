<%@ Assembly Name="AEP.HQAMC.Branding, Version=1.0.0.0, Culture=neutral, PublicKeyToken=53213fcea87a81fe" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="GlobalNav" TagName="GlobalNav" Src="~/_controltemplates/15/AEP.HQAMC.GlobalNav/AEP.HQAMC.GlobalNavKeyboard.ascx" %>

<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>


<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AEPHQAMCMenu.aspx.cs" Inherits="AEP.HQAMC.Branding.Layouts.AEP.HQAMC.Branding.AEPHQAMCMenu" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
 <%-- custom AEP global nav control with new div tag --%>
                        
                                
<GlobalNav:GlobalNav  aepTermStoreName="AEP HQAMC Navigation" id="GlobalNav1" EnableViewState="false" runat="server"  />
                         
                              
                                    <%--End custom AEP tags--%>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
