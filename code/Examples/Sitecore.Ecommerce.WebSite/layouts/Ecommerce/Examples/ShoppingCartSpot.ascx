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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShoppingCartSpot.ascx.cs"
   Inherits="Sitecore.Ecommerce.layouts.Ecommerce.UserControls.ShoppingCartSpot" %>
<%@ Register Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.WebControls" TagPrefix="sc" %>
<%@ Import Namespace="Sitecore.Globalization" %>
<%@ Import Namespace="Sitecore.Ecommerce.Utils" %>
<%@ Import Namespace="Sitecore.Ecommerce.Examples" %>
<div id="movingContainer" style="position: relative;">
   <dl id="small_ShoppingCart_container" class="shoppingShoppingCartSmall">
      <dt><a id="shoppingCartHeader" href="<%= ItemUtil.GetItemUrl(this.Settings.CheckOutLink, false)%>"
         visible="<%# IsItemsInShoppingCart %>">
         <%=Translate.Text(Texts.ShoppingCart) %>
      </a></dt>
      <dd>
         <ul class="plain">
            <asp:Repeater ID="repShoppingCartList" runat="server" Visible="<%# this.ShowShoppingCartItems %>">
               <ItemTemplate>
                  <li class="clearfix">
                     <div>
                        <a href='<%# ShoppingCartLineFriendlyUrl(Container.DataItem) %>'><span runat="server"
                           visible="<%# Settings.ShowImage %>">
                           <img alt="product" height="36" width="36" border="0" src="<%# Eval("ImageUrl") %>?w=36&h=36&thn=1" />
                        </span><span>
                           <%# Eval("Product.Title") %>
                        </span></a>
                     </div>
                     <div class="colPriceContainer">
                        <div class="countInfoText">
                           <%# Eval("Quantity") %>
                           x
                           <%# this.ShoppingCartLineItemPrice(Container.DataItem) %>
                        </div>
                        <div runat="server" class="deleteRow" visible="<%# Settings.ShowDeleteOption %>">
                           <input type="image" title="<%#Translate.Text(Texts.Delete) %>" src="/images/ecommerce/delete3.png"
                              name="<%# "btn_del_" + Eval("Product.Code") %>" onclick="<%# "javascript:DeleteFromShoppingCart('"+ Eval("Product.Code") + "');return false;" %>" />
                        </div>
                        <div class="priceMain">
                           <%# this.ShoppingCartLineTotalPrice(Container.DataItem) %>
                        </div>
                     </div>
                  </li>
               </ItemTemplate>
            </asp:Repeater>
            <li class="summary">
               <div class="amountInShoppingCartStatus" visible="<%# this.ShowAmountInShoppingCartStatusLine %>"
                  runat="server">
                  <asp:Literal ID="litAmountInShoppingCartStatusLine" Visible="<%# this.ShowAmountInShoppingCartStatusLine %>"
                     runat="server" Text="<%# AmountInShoppingCartStatusText %>" />
               </div>
               <div runat="server" class="colPriceContainer" visible="<%# this.ShowTotalSumInShoppingCart %>">
                  <div class="title">
                     <%= Translate.Text(Texts.Total) %>
                  </div>
                  <div class="priceTotal">
                     <asp:Literal ID="litTotalSum" runat="server" Text="<%# this.TotalSum %>" />
                  </div>
                  <br />
               </div>
               <div class="showPriceInfo" runat="server" visible="<%#  Settings.ShowPriceInfo %>">
                  <asp:Literal ID="litShowPriceInfo" runat="server" Text="<%# this.Settings.PriceInfo %>"
                     Visible="<%#  Settings.ShowPriceInfo %>" />
                  <%# this.TotalPriceIncVat %>
               </div>
               <div class="editShoppingCart" visible="<%# IsItemsInShoppingCart %>" runat="server">
                  <a id="goToCart" href="<%= ItemUtil.GetItemUrl(this.Settings.CheckOutLink, true)%>">
                     <%= Translate.Text(Texts.EditShoppingCart)%>
                  </a>
               </div>
               <div runat="server" class="btnContainer" visible="<%# this.IsItemsInShoppingCart %>">
                  <a id="goToCheckout" href='<%= ItemUtil.GetItemUrl(this.Settings.CheckOutLink, false)%>'>
                     <%= Translate.Text(Texts.ProceedToCheckout) %>
                  </a>
               </div>
            </li>
         </ul>
      </dd>
   </dl>
</div>
