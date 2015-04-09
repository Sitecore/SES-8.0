// ------------------------------------------------------------------------------------------
// Copyright 2015 Sitecore Corporation A/S
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
// except in compliance with the License. You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the 
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
// either express or implied. See the License for the specific language governing permissions 
// and limitations under the License.
// -------------------------------------------------------------------------------------------
jQuery(function ($) {

  /* Remove this code after we would have decision about Credit Card payment methods */
  $("input[id*='btnConfirm']").live("click", function (event) {
    var paymentCode = $("select[id*='ddlPaymentMethods'] option:selected").val();
    if (paymentCode == "nonSelected" || paymentCode == "Visa" ||
        paymentCode == "MasterCard" || paymentCode == "AmericanExpress") {
      event.preventDefault();
      return false;
    }
    else {
      return true;
    }
  });


  var checkoutTitle;

  $("input[name*='termsOfConditions']").click(function () {
    var checked = $(this).attr("checked");
    var elem = $(".proceedToCheckout");
    if (checked) {
      elem.removeAttr('disabled');
      elem.parent().attr("class", "btnContainer");
      checkoutTitle = $(".proceedToCheckout").attr('title');
      $(".proceedToCheckout").attr('title', '');
    }
    else {
      elem.attr('disabled', 'disabled');
      elem.parent().attr("class", "btnContainer disabled");
      $(".proceedToCheckout").attr('title', checkoutTitle);
    }
  });


  $(".proceedToCheckout").live("click", function (event) {
    if ($("input[name*='termsOfConditions']").attr("checked")) {
      return true;
    }
    else {
      event.preventDefault();
    }
  });

  $("#btnCreateAccount").live("click", function () {
    var container = $(this).parent();
    $.ajax({
      type: "POST",
      url: "/layouts/ecommerce/Examples/ajax.asmx/CreateCustomerAccount",
      data: "{}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (msg) {
        LoadSublayout("Ecommerce/Examples/Check Out Process/CreateAccount", null, function (msg) {
          container.animate({ opacity: 0.3 }, 200).replaceWith(msg.d);
          container.css({ opacity: 0.3 }).animate({ opacity: 1 }, 600);
        });
        LoadSublayout("Ecommerce/Examples/Check Out Process/LoginPanel", null, function (msg) {
          $("#ph_login").animate({ opacity: 0.3 }, 200).replaceWith(msg.d);
          $("#ph_login").css({ opacity: 0.3 }).animate({ opacity: 1 }, 600);
        });
      }
    });
  });

  $("input[name*='152BA8ACD8704019A7D01281B549761B']").blur(function () {

    $("input[name$='19848439AFDB4649BCD2ACAF7085B322']").val($(this).val());
  });

  // Create Account Page
  $("input[name*='C30A6C21E4EA44FE9B40CD70D290293B']").blur(function () {
    $("input[name$='D121B8508B6A47349B0DBA2099FF2D98']").val($(this).val());
  });

  $(document).ready(function () {
    $("input[name*='19848439AFDB4649BCD2ACAF7085B322']").val($("input[name*='152BA8ACD8704019A7D01281B549761B']").val());
    $("input[name$='D121B8508B6A47349B0DBA2099FF2D98']").val($("input[name*='C30A6C21E4EA44FE9B40CD70D290293B']").val());
  });

  $("a[id*='btnLogOut']").live("click", function () {
    $.ajax({
      type: "POST",
      url: "/layouts/ecommerce/Examples/ajax.asmx/LogOutCurrentUser",
      data: "{}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (msg) {
        window.location.href = window.location.href;
        return false;
      }
    });
  });

  $("a[id*='btnLogIn']").live("click", function () {
    $.ajax({
      type: "POST",
      url: "/layouts/ecommerce/Examples/ajax.asmx/LoginUser",
      data: "{}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (msg) {
        return true;
      }
    });
  });

  $("a[id*='changeDestination']").click(function () {
    var maxWidth = 350;
    var maxHeight = 300;

    var w = ($(window).width() < maxWidth) ? $(window).width() * 0.7 : maxWidth;
    var h = ($(window).height() < maxHeight) ? $(window).height() * 0.7 : maxHeight;
    var lPos = (($(document).width() - w) / 2); // +$(document).scrollLeft();
    var tPos = (($(window).height() - h) / 2) + $(document).scrollTop();

    $("#ShippingAdressForm-overlay").css({ 'position': 'absolute', 'top': '0px', 'left': '0px', 'z-index': '0', 'background': '#000' });
    $("#ShippingAdressForm-overlay").css({ width: $(document).width() + 'px', height: $(document).height() + 'px', opacity: 0.7 }).show(); //.fadeIn();

    $("#ShippingAdressForm").css({ position: 'absolute', top: '200px', left: '200px', display: 'none', width: '600px', 'z-index': '1', 'background-color': 'Transparent', overflow: 'auto' });
    $("#ShippingAdressForm").css({ 'left': lPos + 'px', 'width': w + 'px', 'top': tPos + 'px', 'height': h + 'px', 'background-color': 'Transparent' }).fadeIn();

    $("select[id*='ddlShippingCountries']").change(function () {
      var countryCode = $(this).val();
      $.ajax({
        type: "POST",
        url: "/layouts/ecommerce/Examples/ajax.asmx/GetCountryStates",
        data: "{countryCode:'" + countryCode + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
          $("select[id*='ddlShippingStates']").html(msg.d);
        }
      });
    });

    $("input[id*='confirmShippingForm']").click(function () {
      var countryCode = $("select[id*='ddlShippingCountries']").val();
      var state = $("select[id*='ddlShippingStates']").val();

      if (countryCode == 'NotSelected') {
        alert("Please select country!");
        return false;
      }

      if (state == null) {
        state = "";
      }

      $.ajax({
        type: "POST",
        url: "/layouts/ecommerce/Examples/ajax.asmx/SaveShippingAddress",
        data: "{countryCode:'" + countryCode + "',state:'" + state + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
          window.location.href = window.location.href;
        }
      });
    });

    $("input[id*='cancelShippingForm']").click(function () {
      $("#ShippingAdressForm-overlay").fadeOut();
      $("#ShippingAdressForm").fadeOut();
    });

    return false;
  });
});
