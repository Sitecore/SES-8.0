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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Product.ascx.cs" Inherits="Sitecore.Ecommerce.layouts.Ecommerce.Product" %>
<%@ Import Namespace="Sitecore.Globalization" %>
<%@ Import Namespace="Sitecore.Ecommerce.Examples" %>
<link rel="Stylesheet" href="/styles/product_page.css" />

<script type="text/javascript" src="/jscript/product.js"></script>

<div id="pb_product_container">
   <%--Product Info--%>
   <sc:XslFile runat="server" RenderingID="{DDBA971E-10D2-4847-ADBA-1B2939E83094}" Path="/xsl/Ecommerce/Examples/ProductInfo.xslt"
      Cacheable="true" VaryByData="true" VaryByLogin="true" ID="xslFile1" />
   <div class="clear">
   </div>
   <input id="tab_name" type="hidden" name="TabName" value="Specifications" />
   <div id="tab_area">
      <ul id="tab_controls">
         <li id="li_Specifications" class='<%= GetProductTabCSSClass("Specifications") %>'
            style="cursor: pointer"><a href="#" class="tab" id="Specifications">
               <%= Translate.Text(Texts.Specifications)%>
            </a></li>
         <li id="li_Accessories" class='<%= GetProductTabCSSClass("Accessories") %>' style="cursor: pointer">
            <a href="#" class="tab" id="Accessories">
               <%= Translate.Text(Texts.Accessories)%>
            </a></li>
      </ul>
      <div class="tabContentContainer">
         <div class="clear">
         </div>
         <div id="tabContent" class="tabContent" runat="server">
         </div>
         <div class="clear">
         </div>
      </div>
      <div class="clear">
      </div>
   </div>
</div>
