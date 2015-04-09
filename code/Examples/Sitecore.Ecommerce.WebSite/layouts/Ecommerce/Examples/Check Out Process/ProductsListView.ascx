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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductsListView.ascx.cs" Inherits="Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess.ProductsListView" %>
<%@ Import Namespace="Sitecore.Ecommerce.Utils" %>
<%@ Import Namespace="Sitecore.Globalization" %>
<%@ Import Namespace="Sitecore.Ecommerce.Examples" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>
<asp:Repeater ID="repProductsList" runat="server" OnItemDataBound="repShoppingCartList_ItemDataBound">
   <HeaderTemplate>
      <li class="ulHeader" id="liHeader" runat="server">
         <%--<div class="colProductNumber" id="tdNumber" runat="server">
                <% = ItemUtil.Translate("Number") %></div>--%>
         <div class="colImageText" runat="server">
            <% = Translate.Text(Texts.Description) %></div>
         <div class="colCount">
            <% = Translate.Text(Texts.Amount) %></div>
         <div class="colControlButtons">
            &nbsp;</div>
         <div class="colPriceCurrent">
            <% = Translate.Text(Texts.Price) %></div>
         <div class="colPriceCurrentTotal">
            <% = Translate.Text(Texts.TotalPrice) %></div>
         <!-- floating right -->
      </li>
   </HeaderTemplate>
   <ItemTemplate>
      <li id="trShoppingCartLine" runat="server">
         <div class="colProductNumber" id="divNumber" visible="false" runat="server">
            <!-- Number column - start -->
            <%# Eval("Product.Code")%>
            <!-- Number column - end -->
         </div>
         <!-- product number -->
         <div class="colImage" id="divImage" runat="server">
            <!-- Image column - start -->
            <asp:HyperLink ID="lnkBilde" runat="server" NavigateUrl="<%# ShoppingCartLineFriendlyUrl(Container.DataItem) %>">
            <img class="float-left margin-right10" width="100" height="60" alt="product" src='<%# Eval("ImageUrl") %>?mw=100&mh=60&thn=1' />
            </asp:HyperLink>
            <!-- Image column - end -->
         </div>
         <!-- image -->
         <div class="colText" id="divText" runat="server">
            <h2>
               <asp:HyperLink ID="lnkDescription" runat="server" NavigateUrl='<%# ShoppingCartLineFriendlyUrl(Container.DataItem) %>'>
               <%# Eval("Product.Title") %>
               </asp:HyperLink>
            </h2>
            <p>
               <asp:Literal ID="litDescription" Visible="false" runat="server" Text='<%# Eval("Product.Description") %>' /></p>
            <asp:Label ID="divListPrice" runat="server"><span class="float-left">
               <p>
                  <%= Translate.Text(Texts.ListPrice, true)%>:
                  <asp:Literal ID="litListPrice" runat="server" Text='<%# this.FormatPrice(Eval("Totals.PriceIncVat"))%>' />
               </p>
            </span></asp:Label>
         </div>
         <!-- text -->
         <div class="colCount" id="divCountEdit" runat="server">
            <asp:TextBox ID="txtQuantity" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Quantity") %>' /></div>
         <div class="colCount" id="divCountDisplay" runat="server">
            <asp:Literal ID="litQuantity" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Quantity") %>' /></div>
         <!-- count -->
         <div class="colControlButtons" id="tdCommands" runat="server">
            <asp:LinkButton ID="btnDelete" CommandName="Delete" runat="server">
                    <img src="/images/ecommerce/delete2.png" title="<% = Translate.Text(Texts.RemoveItemFromShoppingCart) %>" alt="<% = Translate.Text(Texts.RemoveItemFromShoppingCart) %>" />
            </asp:LinkButton>
         </div>
         <div class="colPriceCurrent">
            <asp:Literal ID="litPrice" runat="server" /></div>
         <div class="colPriceCurrentTotal">
            <asp:Literal ID="litTotalPrice" runat="server" /></div>
         <!-- priceCurrentTotal -->
      </li>
   </ItemTemplate>
</asp:Repeater>
