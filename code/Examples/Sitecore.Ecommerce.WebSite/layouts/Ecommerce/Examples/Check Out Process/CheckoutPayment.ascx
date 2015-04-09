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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckoutPayment.ascx.cs" Inherits="Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess.CheckoutPayment" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess"
  Assembly="Sitecore.Ecommerce.WebSite" %>
<style type="text/css">
  .scfTitleBorder
  {
    color: #000000;
    font-family: Arial,Tahoma,Helvetica,sans-serif;
    font-size: 1.8em;
    font-weight: bold;
    line-height: 1.1em;
    margin: 5px 0pt;
    text-decoration: none;
  }
</style>
<script type="text/javascript">
  $(document).ready(function () {
    var currentValue;

    var selectByCode = function (paymentCode) {
      $("li[id*='paymentMethodContainer']").hide();
      $("input[id*='paymentMethod'][value=" + paymentCode + "]").parent().removeAttr('style');


      var confirmClicked = false;

      var payButton = $("input[id*='btnConfirm']");
      payButton.click(function () {
        if (!confirmClicked) {
          $(this).attr('class', 'btnContinueAndPayDisabled');
          confirmClicked = true;
        }
        else {
          return false;
        }
      });

      if (paymentCode != "nonSelected") {
        payButton.parent().attr('class', 'scfSubmitButtonBorder');
        payButton.removeAttr('disabled');
      }
      else {
        payButton.parent().attr('class', 'scfSubmitButtonBorder disabled');
        payButton.attr('disabled', 'disabled');
      }

      $.ajax({
        type: "POST",
        url: "/layouts/ecommerce/Examples/ajax.asmx/PaymentMethodChanged",
        data: "{paymentMethodCode:'" + paymentCode + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json"
      });
    }

    var dropDownListOnChangeCallback = function () {
      var val = $(this).val();
      if (currentValue != val) {
        currentValue = val;
        selectByCode(val);
      }
    }

    var ddlPaymentMethods = $("select[id*='ddlPaymentMethods']");

    ddlPaymentMethods.blur(dropDownListOnChangeCallback);
    ddlPaymentMethods.change(dropDownListOnChangeCallback);

    selectByCode(ddlPaymentMethods.val());
  });
</script>
<div class="paymentForm">
  <asp:Label ID="lblFormTitle" CssClass="scfTitleBorder" runat="server" />
  <h4>
    <asp:Label ID="lblFormDescription" runat="server" />
  </h4>
  <br />
  <asp:Label ID="lblpaymentMethods" runat="server" />
  <sc:DropDownList ID="ddlPaymentMethods" runat="server" />
  <div id="paymentsContainer">
    <ul>
      <asp:Repeater ID="repeaterPaymentMethods" runat="server" OnItemDataBound="repeaterPaymentMethods_ItemDataBound">
        <ItemTemplate>
          <li id="paymentMethodContainer" class="paymentsMethodContainer" runat="server">
            <input type="hidden" runat="server" id="paymentMethod" />
            <table class="ie7_table_fix">
              <tr>
                <td colspan="2">
                  <span class="paymentTitle">Payment type:
                    <%# Eval("Title") %></span>
                </td>
              </tr>
              <tr>
                <td>
                  <div class="colimage">
                    <img src='<%# Eval("logourl", "{0}?mh=48&mw=64&thn=1") %>' alt="<%# Eval("title") %>" />
                  </div>
                </td>
                <td class="paymentDescription">
                  <%# Eval("Description") %>
                </td>
              </tr>
            </table>
          </li>
        </ItemTemplate>
      </asp:Repeater>
    </ul>
  </div>
  <div class="scfSubmitButtonBorder disabled">
    <asp:Button ID="btnConfirm" runat="server" OnClick="ConfirmButton_Click" disabled="disabled" />
  </div>
</div>
