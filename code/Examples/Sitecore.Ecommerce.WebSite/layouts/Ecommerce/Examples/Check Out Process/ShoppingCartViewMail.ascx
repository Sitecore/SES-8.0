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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShoppingCartView.ascx.cs"
    Inherits="Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess.ProductsListView" %>
<%@ Import Namespace="Sitecore.Ecommerce.DomainModel.Carts"%>
<%@ Import Namespace="Sitecore.Globalization"%>
<%@ Import Namespace="Sitecore.Ecommerce.Examples" %>
<%@ Import Namespace="Sitecore.Ecommerce.DomainModel.Configurations" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>

<%--<ul class="ulProductList" id="ShoppingCart_product_list"> --%>
<table class="ulProductList" style="border-collapse: collapse;" width="100%" cellpadding="0" cellspacing="0">
    <asp:Repeater ID="repProductsList" runat="server" OnItemDataBound="repShoppingCartList_ItemDataBound">
        <HeaderTemplate>
            <%-- <li class="ulHeader" id="liHeader" runat="server">--%>
            <tr visible="false" runat="server">
                <td>
                </td>
                <td>
                </td>
                <td>
                    <%--<div class="colProductNumber" id="tdNumber" runat="server">
                        <% = ItemUtil.Translate("Number") %></div>--%>
                    <div class="colImageText" runat="server"><% = Translate.Text(Texts.Description) %></div>
                </td>
                <td>
                    <div class="colCount"><% = Translate.Text(Texts.Amount) %></div>
                </td>
                <td>
                    <div class="colControlButtons">&nbsp;</div>
                </td>
             
                <td>
                    <div class="colPriceCurrent"><% = Translate.Text(Texts.Price) %></div>                        
                </td>
                <td>
                    <div class="colPriceCurrentTotal"><% = Translate.Text(Texts.TotalPrice) %></div><!-- floating right -->
                </td>
            </tr>
            <%-- </li> --%>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
            <%-- <li id="trShoppingCartLine" runat="server"> --%>
                <td>
                    <div class="colProductNumber" id="divNumber" visible="false" runat="server">
                        <!-- Number column - start -->
                        <%# DataBinder.Eval(Container, "DataItem.Id")%>
                        <!-- Number column - end -->
                    </div>
                    <!-- product number -->
                </td>
                <td>
                    <div class="colImage" id="divImage" runat="server">
                        <!-- Image column - start -->
                        <asp:HyperLink ID="lnkBilde" runat="server" NavigateUrl='<%# ShoppingCartLineFriendlyUrl(Container.DataItem) %>'>
                            <img class="float-left margin-right10" alt="" src='<%# Eval("ImageUrl") %>?w=100&h=60&bc=white' />
                        </asp:HyperLink>
                        <!-- Image column - end -->
                    </div>
                    <!-- image -->
                </td>
                <td>
                    <div class="colText" id="divText" runat="server">
                        <h2>
                            <asp:HyperLink ID="lnkDescription" runat="server" NavigateUrl='<%# ShoppingCartLineFriendlyUrl(Container.DataItem) %>'>
                              <%# Eval("Product.Description") %>
                            </asp:HyperLink>
                        </h2>
                        
                        <p><asp:Literal ID="litDescription" Visible="false" runat="server" Text='<%# Eval("Product.Description") %>' /></p>
                        
                        <asp:Label ID="divListPrice" runat="server">
                            <span class="float-left">
                                <p>
                                    <%= Translate.Text(Texts.ListPrice, true)%>:
                                    <asp:Literal ID="litListPrice" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PriceIncVat", "{0:# ### ##0.00}") %>' />
                                </p>
                            </span>
                        </asp:Label>
                    </div>           
                    <!-- text -->
                </td>
                <td>
                    <div class="colCount" id="divCountEdit" runat="server"><asp:TextBox ID="txtQuantity" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Quantity") %>' /></div>
                    <div class="colCount" id="divCountDisplay" runat="server"><asp:Literal ID="litQuantity" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Quantity") %>' /></div>
                    <!-- count -->
                </td>
                <td>
                    <div class="colControlButtons" id="tdCommands" runat="server">
                        <asp:LinkButton ID="btnDelete" CommandName="Delete" runat="server">
                            <img src="/images/ecommerce/btn_delete.gif" alt='<% = Translate.Text(Texts.Delete) %>' />
                        </asp:LinkButton>
                    </div>
                </td>
                <td style="text-align:right">               
                    <div class="colPriceCurrent"><asp:Literal ID="litPrice" runat="server" /></div>
                </td>
                <td style="text-align:right">
                    <div class="colPriceCurrentTotal"><asp:Literal ID="litTotalPrice" runat="server" /></div>
                    <!-- priceCurrentTotal -->
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <hr />
                </td>
            </tr>
            <%-- </li>--%>
        </ItemTemplate>
    </asp:Repeater>

    <tr>
    <%-- <li class="summary">--%>
        <td colspan="3"></td>
        <td colspan="4" align="right">
            <div class="colPriceContainer">
                <table>
                    <tr>
                        <td>
                            <div class="title"><%= Translate.Text(Texts.TotalPriceExclVat)%></div>
                        </td>
                        <td style="text-align:right; font-weight:bold">
                            <div class="priceTotal"><%# MainUtil.FormatPrice(Sitecore.Ecommerce.Context.Entity.GetInstance<ShoppingCart>().Totals.PriceExVat, Sitecore.Ecommerce.Context.Entity.GetConfiguration<GeneralSettings>().DisplayCurrencyOnPrices, Sitecore.Ecommerce.Context.Entity.GetConfiguration<ShoppingCartSettings>().PriceFormatString)%></div>
                            <%-- <div class="clear"></div>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="vat"><%= Translate.Text(Texts.TotalVat)%></div>
                        </td>
                        <td style="text-align:right;">
                            <div class="priceVat"><%# MainUtil.FormatPrice(Sitecore.Ecommerce.Context.Entity.GetInstance<ShoppingCart>().Totals.VAT, Sitecore.Ecommerce.Context.Entity.GetConfiguration<GeneralSettings>().DisplayCurrencyOnPrices, Sitecore.Ecommerce.Context.Entity.GetConfiguration<ShoppingCartSettings>().PriceFormatString)%></div>
                            <%-- <div class="clear"></div>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="title"><%= Translate.Text(Texts.TotalPriceInclVat)%></div>
                        </td>
                        <td style="text-align:right; font-weight:bold">
                            <div class="priceTotal"><%# MainUtil.FormatPrice(Sitecore.Ecommerce.Context.Entity.GetInstance<ShoppingCart>().Totals.PriceIncVat, Sitecore.Ecommerce.Context.Entity.GetConfiguration<GeneralSettings>().DisplayCurrencyOnPrices, Sitecore.Ecommerce.Context.Entity.GetConfiguration<ShoppingCartSettings>().PriceFormatString)%></div>
                        </td>
                    </tr>
                </table>
            </div>            
        </td>
    <%-- </li>--%>
    </tr>
</table>
<%-- </ul>--%>
