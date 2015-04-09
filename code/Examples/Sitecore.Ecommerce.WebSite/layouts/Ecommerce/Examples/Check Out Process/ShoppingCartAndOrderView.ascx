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
<%@ Import Namespace="Sitecore.Ecommerce.Utils" %>
<%@ Control Language="C#" AutoEventWireup="true" Debug="true" CodeBehind="ShoppingCartAndOrderView.ascx.cs"
   Inherits="Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess.ShoppingCartAndOrderView" %>
<%@ Import Namespace="Sitecore.Globalization" %>
<%@ Import Namespace="Sitecore.Ecommerce.Examples" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>
<%@ Register Src="ProductsListView.ascx" TagName="ProductsListView" TagPrefix="uc" %>
<%@ Register Src="~/layouts/Ecommerce/Examples/Check Out Process/Delivery.ascx" TagName="Delivery"
   TagPrefix="uc" %>
<%@ Register TagPrefix="uc" Src="~/layouts/Ecommerce/Examples/Check Out Process/ShoppingCartSummary.ascx" TagName="Summary" %>
<div id="lblOrderNumber" runat="server" class="OrderNumberCaption">
</div>
<div id="divRow1" runat="server">
   <div class="col" id="divSection1" visible="false" runat="server">
      <dl class="dlDeleveryAddress">
         <dt>
            <asp:Literal runat="server" ID="litTitleSection1" /></dt>
         <dd id="ddSection1" runat="server">
            <div class="colMargin8">
               <asp:Repeater ID="repeaterOrderSection1" runat="server" OnItemDataBound="RepeaterOrderSection_ItemDataBound">
                  <ItemTemplate>
                     <div class="title">
                        <%# DataBinder.Eval(Container, "DataItem.Label")%>
                     </div>
                     <div class="value">
                        <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container, "DataItem.Value"))%>
                     </div>
                  </ItemTemplate>
               </asp:Repeater>
            </div>
            <div class="clear">
            </div>
         </dd>
      </dl>
   </div>
   <div class="col" id="divSection2" visible="false" runat="server">
      <dl class="dlDeleveryAddress">
         <dt>
            <asp:Literal runat="server" ID="litTitleSection2" /></dt>
         <dd id="ddSection2" runat="server">
            <div class="colMargin8">
               <asp:Repeater ID="repeaterOrderSection2" runat="server" OnItemDataBound="RepeaterOrderSection_ItemDataBound">
                  <ItemTemplate>
                     <div class="title">
                        <%# DataBinder.Eval(Container, "DataItem.Label")%>
                     </div>
                     <div class="value">
                        <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container, "DataItem.Value"))%>
                     </div>
                  </ItemTemplate>
               </asp:Repeater>
            </div>
            <div class="clear">
            </div>
         </dd>
      </dl>
   </div>
   <div class="col" id="divSection3" visible="false" runat="server">
      <dl class="dlDeleveryAddress">
         <dt>
            <asp:Literal runat="server" ID="litTitleSection3" /></dt>
         <dd id="ddSection3" runat="server">
            <div class="colMargin8">
               <asp:Repeater ID="repeaterOrderSection3" runat="server" OnItemDataBound="RepeaterOrderSection_ItemDataBound">
                  <ItemTemplate>
                     <div class="title">
                        <%# DataBinder.Eval(Container, "DataItem.Label")%>
                     </div>
                     <div class="value">
                        <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container, "DataItem.Value"))%>
                     </div>
                  </ItemTemplate>
               </asp:Repeater>
            </div>
            <div class="clear">
            </div>
         </dd>
      </dl>
   </div>
   <div class="clear">
   </div>
</div>
<div id="lblCardType" runat="server" class="CardTypeCaption">
</div>
<div class="content" id="pnlShoppingCart" runat="server">
   <div class="col">
      <dl id="dlProducts">
         <dt>
            <% = Translate.Text(Texts.Products) %></dt>
         <dd>
            <ul class="ulProductList" id="ShoppingCart_product_list">
               <uc:ProductsListView ID="ProductsListView" DisplayMode="OrderConfirmation" runat="server" />
               <li>
                  <asp:Label ID="lblShippingMethods" runat="server" />
                  <asp:Label ID="lblShippingMethod" runat="server" Visible="false" />
                  <asp:Label ID="shippingCost" Style="float: right;" runat="server" />
                  <uc:Delivery ID="delivery" runat="server" DisplayMode="OrderConfirmation" />
               </li>
               <uc:Summary ID="summary" runat="server" DisplayTitle="false" DisplayVAT="true" DispayPriceExlVat="true" />
            </ul>
         </dd>
      </dl>
   </div>
</div>
<div id="divRow2" runat="server">
   <div class="col" id="divSection4" visible="false" runat="server">
      <dl class="dlDeleveryAddress">
         <dt>
            <asp:Literal runat="server" ID="litTitleSection4" /></dt>
         <dd id="ddSection4" runat="server">
            <div class="colMargin8">
               <asp:Repeater ID="repeaterOrderSection4" runat="server" OnItemDataBound="RepeaterOrderSection_ItemDataBound">
                  <ItemTemplate>
                     <div class="title">
                        <%# DataBinder.Eval(Container, "DataItem.Label")%>
                     </div>
                     <div class="value">
                        <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container, "DataItem.Value"))%>
                     </div>
                  </ItemTemplate>
               </asp:Repeater>
            </div>
            <div class="clear">
            </div>
         </dd>
      </dl>
   </div>
   <div class="col" id="divSection5" visible="false" runat="server">
      <dl class="dlDeleveryAddress">
         <dt>
            <asp:Literal runat="server" ID="litTitleSetion5" /></dt>
         <dd id="ddSection5" runat="server">
            <div class="colMargin8">
               <asp:Repeater ID="repeaterOrderSection5" runat="server" OnItemDataBound="RepeaterOrderSection_ItemDataBound">
                  <ItemTemplate>
                     <div class="title">
                        <%# DataBinder.Eval(Container, "DataItem.Label")%>
                     </div>
                     <div class="value">
                        <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container, "DataItem.Value"))%>
                     </div>
                  </ItemTemplate>
               </asp:Repeater>
            </div>
            <div class="clear">
            </div>
         </dd>
      </dl>
   </div>
   <div class="col" id="divSection6" visible="false" runat="server">
      <dl class="dlDeleveryAddress">
         <dt>
            <asp:Literal runat="server" ID="litTitleSection6" /></dt>
         <dd id="ddSection6" runat="server">
            <div class="colMargin8">
               <asp:Repeater ID="repeaterOrderSection6" runat="server" OnItemDataBound="RepeaterOrderSection_ItemDataBound">
                  <ItemTemplate>
                     <div class="title">
                        <%# DataBinder.Eval(Container, "DataItem.Label")%>
                     </div>
                     <div class="value">
                        <%# HttpUtility.HtmlEncode(DataBinder.Eval(Container, "DataItem.Value"))%>
                     </div>
                  </ItemTemplate>
               </asp:Repeater>
            </div>
            <div class="clear">
            </div>
         </dd>
      </dl>
   </div>
   <div class="clear">
   </div>
</div>
