<%--=======================================================================================
Copyright 2015 Sitecore Corporation A/S
Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
except in compliance with the License. You may obtain a copy of the License at
    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the 
License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
either express or implied. See the License for the specific language governing permissions 
and limitations under the License.
======================================================================================--%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SitecoreSimpleFormAscx.ascx.cs" Inherits="Sitecore.Ecommerce.Form.Web.UI.Controls.SitecoreSimpleFormAscx" %>
<%@ Register Namespace="Sitecore.Form.Web.UI.Controls" Assembly="Sitecore.Forms.Core" TagPrefix="wfm" %>

<wfm:FormTitle ID="title" runat="server"/>
<wfm:FormIntroduction ID="intro" runat="server"/>
<asp:ValidationSummary ID="summary" runat="server" ValidationGroup="submit" CssClass="scfValidationSummary"/>
<wfm:SubmitSummary ID="submitSummary" runat="server" CssClass="scfSubmitSummary"/>
<asp:Panel ID="fieldContainer" runat="server"/>
<wfm:FormFooter ID="footer" runat="server"/>
<wfm:FormSubmit ID="submit" runat="server" Class="scfSubmitButtonBorder"/>
 
