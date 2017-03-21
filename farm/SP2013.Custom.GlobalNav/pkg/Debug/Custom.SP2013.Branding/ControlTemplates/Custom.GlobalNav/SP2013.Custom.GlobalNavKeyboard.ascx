<%@ Assembly Name="SP2013.Custom.Branding, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0eb8b5d2de4a9079" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SP2013.Custom.GlobalNavKeyboard.ascx.cs" Inherits="Custom.SP2013.Branding.ControlTemplates.Custom.SP2013.Branding.Custom" %>
<asp:Label ID="Custom_SP2013_GlobalNavContainer_Keyboard" Style="height:101%; overflow:scroll" TabIndex="0" runat="server" ViewStateMode="Disabled" CssClass="CustomHQAMGlobalNavContainer" ></asp:Label>
