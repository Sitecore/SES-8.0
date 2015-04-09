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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginPanel.ascx.cs" Inherits="Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess.LoginPanel" %>
<%@ Import Namespace="Sitecore.Ecommerce.DomainModel.Configurations" %>
<%@ Import Namespace="Sitecore.Globalization" %>
<%@ Import Namespace="Sitecore.Ecommerce.Utils" %>
<%@ Import Namespace="Sitecore.Ecommerce.Examples" %>
<div runat="server" id="loginContainer">
   <ul id="ph_login">
      <li id="liMypage" runat="server"><a href='<%= ItemUtil.GetItemUrl(Sitecore.Ecommerce.Context.Entity.GetConfiguration<GeneralSettings>().MyPageLink, true) %>'>
         <%= Translate.Text(Texts.MyPage) %>
      </a></li>
      <li id="liStatusLoggedIn" runat="server"><a href='<%= ItemUtil.GetItemUrl(Sitecore.Ecommerce.Context.Entity.GetConfiguration<GeneralSettings>().MyPageLink, true) %>'>
         <asp:Label runat="server" ID="lblLogedInAs" />
      </a></li>
      <li id="liStatusNotLoggedIn" runat="server">
         <%= Translate.Text(Texts.YouAreNotLoggedIn)%>
      </li>
      <li class="phLogin" id="liBtnLogInOut"><a id="btnLogIn" runat="server"><span>
         <%= Translate.Text(Texts.Login) %>
      </span></a><a id="btnLogOut" runat="server"><span>
         <%= Translate.Text(Texts.Logout) %>
      </span></a></li>
      <li class="phDanish"><a id="lnkDdanish" href='<%= MainUtil.GetLanguageLink("da")  %>'>
         <%= Translate.Text(Texts.Danish) %>
      </a></li>
      <li class="phEnglish"><a id="lnkEnglish" href='<%= MainUtil.GetLanguageLink("en")  %>'>
         <%= Translate.Text(Texts.English) %>
      </a></li>
   </ul>
</div>
