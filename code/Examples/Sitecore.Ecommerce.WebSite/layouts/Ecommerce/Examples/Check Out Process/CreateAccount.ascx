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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateAccount.ascx.cs"
   Inherits="Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess.CreateAccount" %>
<%@ Import Namespace="Sitecore.Globalization" %>
<%@ Import Namespace="Sitecore.Ecommerce.Examples" %>
<div id="createAccountPanel" class="createAccountContainer">
   <div class="colMargin8">
   </div>
   <div>
      <%# !this.Visibility ? GetPromtMessage() : GetConfiramtionMessage()%>
   </div>
   <div class="colMargin8">
   </div>
   <input type="button" id="btnCreateAccount" value='<%= Translate.Text(Texts.CreateAccount) %>'
      style='visibility: <%# !this.Visibility ? "visible":"hidden" %>' />
</div>
