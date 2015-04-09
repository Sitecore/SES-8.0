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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShoppingCartSummary.ascx.cs"  Inherits="Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess.ShoppingCartSummary" %>
<%@ Import Namespace="Sitecore.Globalization" %>
<%@ Import Namespace="Sitecore.Ecommerce.Examples" %>
<div>
  <asp:Label ID="lblFormTitle" CssClass="scfTitleBorder" runat="server" />
  <ul>
    <li class="summary">
      <div class="colPriceContainer">
        <div id="priceExclVAT" runat="server">
          <div class="title">
            <%= Translate.Text(Texts.TotalPriceExclVat)%>:</div>
          <div class="priceTotal">
            <%# this.FormatPrice(this.Totals.PriceExVat) %></div>
          <div class="clear">
          </div>
        </div>
        <div id="VAT" runat="server">
          <div class="vat">
            <%= Translate.Text(Texts.TotalVat)%>:</div>
          <div class="priceVat">
            <%# this.FormatPrice(this.Totals.TotalVat) %></div>
          <div class="clear">
          </div>
        </div>
        <div id="priceInclVat" runat="server">
          <div class="title">
            <%= Translate.Text(Texts.TotalPriceInclVat)%>:</div>
          <div class="priceTotal">
            <%# this.FormatPrice(this.Totals.PriceIncVat) %></div>
        </div>
      </div>
    </li>
  </ul>
</div>
