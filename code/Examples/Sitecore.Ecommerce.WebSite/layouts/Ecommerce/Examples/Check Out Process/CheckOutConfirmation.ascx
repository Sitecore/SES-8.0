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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckOutConfirmation.ascx.cs" Inherits="Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess.CheckOutConfirmation" %>
<%@ Register Src="~/layouts/Ecommerce/Examples/Check Out Process/ShoppingCartAndOrderView.ascx" TagName="ShoppingCartAndOrderView" TagPrefix="uc" %>
<%@ Register Src="~/layouts/Ecommerce/Examples/Check Out Process/CreateAccount.ascx" TagName="CreateAccount" TagPrefix="uc" %>
<link href="../../../../sitecore%20modules/Shell/Web%20Forms%20for%20Marketers/Themes/Default.css" rel="stylesheet" type="text/css" />
<style type="text/css">
      
  .pb_mc .container, .pb_mc .content, .pb_mc .content2, .pb_mc .content3, .pb_mc .content4 {
    display: block;
    float: none;
  }
  .pb_mc .content .col, .pb_mc .content2 .col, .pb_mc .content2 .colRotator, .pb_mc .content2 .colDynamicHight, .pb_mc .content3 .col, .pb_mc .content4 .col {
    border: none;
  }
                   
  div.boxShaddow1 dl dd {
    padding: 16px 24px 24px;
  }
  
  #dlProducts dd .ulProductList {
    border-top: none;
    margin-bottom: 20px;
  }
  
  .ulProductList li {
    border: none;
    display: block;
    float: none;
    width: auto;
  }
  
</style>
<div class="boxShaddow1">
  <dl>
    <dd>
      <div style="position: relative;">
        <div class="scfTitleBorder">
          <asp:Literal ID="litTitle" runat="server" />
        </div>
        <div class="scfIntroBorder">
          <asp:Literal ID="litStatusMessage" runat="server" />
        </div>
        <div style="position: absolute; top: 0; right: 0;">
          <uc:CreateAccount ID="CreateAcccount" runat="server" />
        </div>
      </div>
    </dd>
    <dd>
      <uc:ShoppingCartAndOrderView ID="ShoppingCartAndOrderView" CurrentOrderDisplayMode="OrderConfirmation" runat="server" />
    </dd>
    <dd class="bottom">
      <div>
        &nbsp;</div>
    </dd>
  </dl>
  <div class="bottomNavigation">
    <div class="bottomNavigationLeft">
      <input ID="btnPrintOrder" type="button" runat="server" onclick="printVersion()"  />
      <asp:Button ID="btnGotoShop" runat="server" OnClick="BtnGotoShop_Click" />
    </div>
  </div>
</div>
