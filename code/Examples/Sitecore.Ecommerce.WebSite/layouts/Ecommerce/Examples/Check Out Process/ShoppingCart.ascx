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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShoppingCart.ascx.cs" Inherits="Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess.ShoppingCart" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>
<%@ Register Src="~/layouts/Ecommerce/Examples/Check Out Process/ProductsListView.ascx" TagName="ProductsListView" TagPrefix="uc" %>
<%@ Register TagPrefix="uc" Src="~/layouts/Ecommerce/Examples/Check Out Process/ShoppingCartSummary.ascx" TagName="Summary" %>
<%@ Register Src="~/layouts/Ecommerce/Examples/Check Out Process/Delivery.ascx" TagName="Delivery" TagPrefix="uc" %>

<div id="pb_shopping_cart_container" runat="server">
   <div class="content">
      <div id="pb_header_shaddow">
         <h1>
            <sc:Text ID="text1" Field="Title" runat="server" />
         </h1>
      </div>
      <asp:Literal ID="litStatus" runat="server" />
      <div class="clear">
      </div>
   </div>
   <div class="content">
      <ul class="ulProductList" id="ShoppingCart_product_list">
      <uc:ProductsListView ID="UcProductsListView" DisplayMode="ShoppingCart"
         runat="server" />
      <li>         
         <uc:Delivery ID="delivery" runat="server" />
      </li>
      <uc:Summary ID="summary" runat="server" DisplayTitle="false" DisplayVAT="true" DisplayPriceIncVat="true" DispayPriceExlVat="true" />
      </ul>
      <div class="bottomNavigation">
        <div class="bottomNavigationLeft">
          <div id="btnEmptyContainer" runat="server" class="btnContainer"><asp:LinkButton ID="btnEmptyShoppingCart" runat="server" OnClick="btnEmptyShoppingCart_Click" /></div>
          <div id="btnUpdateContainer" runat="server" class="btnContainer"><asp:LinkButton ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" /></div>
          <div class="btnContainer"><asp:LinkButton ID="btnContinueShopping" runat="server" OnClick="btnContinueShopping_Click" /></div>
        </div>
      </div>
      <div style="display: inline; float: left; margin: 8px 0pt; padding: 0pt; width: 100%;">
         <div style="float: left; padding-left: 8px; text-align: left; width: 65%;" id="tocArea"
            runat="server">
            <asp:CheckBox type="checkbox" ID="termsOfConditions" runat="server" CssClass="termsOfCond"
               Text="I agree with the" />
            <a class='modalinfo' href='/en/Company/Terms and Conditions.aspx'>terms and conditions</a>
         </div>
         <div class="bottomNavigationRight" style="text-align: right; margin-top: -7px; margin-right: 15px;">
           <div id="btnProceedToCheckoutContainer" runat="server" class="btnContainer disabled">
             <asp:LinkButton runat="server" ID="btnProceedToCheckout" CssClass="proceedToCheckout"  OnClick= "ProceedToCheckoutClicked"/>
           </div>
         </div>
      </div>
      <!-- bottom navigation -->
   </div>
</div>
<!-- shopping_cart_container -->
<div class="clear">
</div>
