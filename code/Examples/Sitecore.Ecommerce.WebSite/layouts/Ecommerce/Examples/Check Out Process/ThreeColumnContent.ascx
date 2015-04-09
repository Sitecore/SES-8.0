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
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess.ThreeColumnContent" Codebehind="ThreeColumnContent.ascx.cs" %>
<div id="page_body" class="clearfix">
    <div class="pb_lc">
        <%----------------------------------------
        --               left side menu
        ----------------------------------------%>
        <sc:Placeholder runat="server" Key="phleft" ID="phLeft"></sc:Placeholder>
        <div class="clear">
        </div>
    </div>
    <%----------------------------------------
        --               content
        ----------------------------------------%>
    <div class="checkout_mc">               
        <sc:Placeholder runat="server" Key="phcenter" ID="phCenter"></sc:Placeholder>        
    </div>
    <%----------------------------------------
        --               right side content
        ----------------------------------------%>
    <div id="pb_menu_secondary" class="pb_rc">        
        <sc:Placeholder runat="server" Key="phright" ID="phRight"></sc:Placeholder>
    </div>
    <div class="clear">
    </div>
</div>
