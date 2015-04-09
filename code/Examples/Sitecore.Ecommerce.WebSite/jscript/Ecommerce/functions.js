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

function LoadRendering(rendering, placeholder) {
  var id = $("#scID").attr("content");

  $.ajax({
    type: "POST",
    url: "/layouts/ecommerce/Examples/Ajax.asmx/LoadRendering",
    data: "{rendering:'" + rendering + "', id : '" + id + "'}",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function(msg) {
      // Replace the div's content with the page method's return.
      placeholder.html(msg.d);
    }
  });
}

function LoadSublayout(sublayout, placeholder, callback) {
  var id = $("#scID").attr("content");

  $.ajax({
    type: "POST",
    url: "/layouts/ecommerce/Examples/Ajax.asmx/LoadSublayout",
    data: "{sublayout:'" + sublayout + "', id : '" + id + "'}",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: (callback) ? callback : function(msg) {
      placeholder.html(msg.d);
    }
  });
}

function ExecuteAjaxMethod(methodName, methodArgs) {
  $.ajax({
    type: "POST",
    url: "/layouts/ecommerce/Examples/Ajax.asmx/" + methodName,
    data: "{" + methodArgs + "}",
    contentType: "application/json; charset=utf-8",
    dataType: "json"
  });
}

$("#goToCheckout").live("click", function() {
  ExecuteAjaxMethod("ShoppingCartSpotCheckout", "");
});

$("#goToCheckout").live("keypess", function() {
  ExecuteAjaxMethod("ShoppingCartSpotCheckout", "");
});

$("#goToCart,#shoppingCartHeader").live("click", function() {
  ExecuteAjaxMethod("EditShopingCartClicked", "");
});

$("#goToCart,#shoppingCartHeader").live("keypess", function() {
  ExecuteAjaxMethod("EditShopingCartClicked", "");
});

function SetCurrentTab(tab) {
  $.ajax({
    type: "POST",
    url: "/layouts/ecommerce/Examples/ajax.asmx/SetCurrentTab",
    data: "{tab:'" + tab + "'}",
    contentType: "application/json; charset=utf-8",
    dataType: "json"
  });
}


function AddReview(title, text, rate) {
  var id = $("#scID").attr("content");
  $.ajax({
    type: "POST",
    url: "/layouts/ecommerce/Examples/ajax.asmx/AddReview",
    data: "{title:'" + title + "', text :'" + text + "', rate:'" + rate + "', id : '" + id + "'}",
    contentType: "application/json; charset=utf-8",
    dataType: "json"
  });
}

var addToShoppingCartIsProcessing = false;

function AddToShoppingCart(productCode, quantity) {
  if (!addToShoppingCartIsProcessing) {
    addToShoppingCartIsProcessing = true;
    setTimeout(function() {
      addToShoppingCartIsProcessing = false;
    }, 500);

    $.ajax({
      type: "POST",
      url: "/layouts/ecommerce/Examples/ajax.asmx/AddToShoppingCart",
      data: "{productCode:'" + productCode + "', quantity:'" + quantity + "'}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function(msg) {
        // Replace the div's content with the page method's return.
        LoadSublayout("Ecommerce/Examples/ShoppingCartSpot", null, function(msg) {
          $("#small_ShoppingCart_container").animate({ opacity: 0.3 }, 200).replaceWith(msg.d);
          $("#small_ShoppingCart_container").css({ opacity: 0.3 }).animate({ opacity: 1 }, 600);
        });
      }
    });
  }
}

function DeleteFromShoppingCart(productCode) {
  $.ajax({
    type: "POST",
    url: "/layouts/ecommerce/Examples/ajax.asmx/DeleteFromShoppingCart",
    data: "{productCode:'" + productCode + "'}",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function(msg) {
      // Replace the div's content with the page method's return.
      LoadSublayout("Ecommerce/Examples/ShoppingCartSpot", null, function(msg) {
        $("#small_ShoppingCart_container").animate({ opacity: 0.3 }, 200).replaceWith(msg.d);
        $("#small_ShoppingCart_container").css({ opacity: 0.3 }).animate({ opacity: 1 }, 600);
      });
    }
  });
}

function UpdateShoppingCart(productCode, quantity) {
  $.ajax({
    type: "POST",
    url: "/layouts/ecommerce/Examples/ajax.asmx/UpdateShoppingCart",
    data: "{productCode:'" + productCode + "' quantity:'" + quantity + "'}",
    contentType: "application/json; charset=utf-8",
    dataType: "json"
  });
}