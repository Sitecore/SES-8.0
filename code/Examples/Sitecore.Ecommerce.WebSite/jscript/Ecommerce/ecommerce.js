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
  function countryChangedHandler() {
    var countryCode = $(this).val();
    var container = $(this).parent().parent().parent();
    var checkBox = container.find("[class*='name.Hide'] input");
    var checked = true;

    if (checkBox.length > 0) {
      checked = checkBox.attr("checked");
    }

    if ((countryCode == "") && checked) {
      var control = container.find("[class$='State'] select.scfDropList");

      control.parent().parent().hide();
      control.siblings("input[id$='_ecstate']").val("disabled");
      control.val("");
    } else {
      $.ajax({
        type: "POST",
        url: "/layouts/ecommerce/Examples/Ajax.asmx/GetCountryStates",
        data: "{countryCode:'" + countryCode + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
          if (checked) {
            var control = container.find("[class$='State'] select.scfDropList");

            if (msg.d == "") {
              control.parent().parent().hide();
              control.siblings("input[id$='_ecstate']").val("disabled");
              control.val("");
            } else {
              control.parent().parent().show();
              control.siblings("input[id$='_ecstate']").val("");
            }
          }
        }
      });
    }
  }

  $("[class$='Country'] select.scfDropList").each(countryChangedHandler);
  $("[class$='Country'] select.scfDropList").change(countryChangedHandler);

  $("input[value='disabled']").each(function () {
    $(this).parent().parent().hide();
  });

  $("[class*='name.Hide'] :checkbox:not(:checked)").map(function () {
    $(this).parent().parent().parent().nextAll("div").each(function () {
      $(this).find("input[id$='_ecstate']").each(function () {
        $(this).parent().parent().hide();
        $(this).val("disabled");
      });
    });
  });



  $("[class*='name.Hide'] input").click(function () {
    var checked = $(this).attr("checked");
    $(this).parent().parent().parent().nextAll("div").each(function () {
      $(this).find("input[id$='_ecstate']").each(function () {
        if (!checked) {
          $(this).parent().parent().hide();
          $(this).val("disabled");
        } else {
          $(this).parent().parent().show();
          $(this).val("");
          var shippingCountry = $(this).find("[class$='ShippingCountry'] select.scfDropList").val();
          if (shippingCountry != null && shippingCountry != 'undefined') {
            var shippingState = $("[class$='ShippingState'] select.scfDropList");
            if (shippingCountry != "United States") {
              shippingState.parent().parent().hide();
              shippingState.siblings("input[id$='_ecstate']").val("disabled");
              shippingState.val("");
            }
            else {
              shippingState.parent().parent().show();
              shippingState.siblings("input[id$='_ecstate']").val("");
            }
          }
        }
      });
    });
  });


  $("input").keypress(function (e) {
    var isFirstmatch = true;
    if (e.keyCode == 13) {
      $(this).parents().each(function () {
        if (isFirstmatch) {
          $(this).find("input[type='submit'],input[type='image']").map(function () {
            isFirstmatch = false;
            $(this).trigger('click');
            return false;
          });
        }
      });
      return false;
    }
  });

  $(".modalinfo").click(function () {
    var maxWidth = 800;
    var maxHeight = 800;

    if ($("#modalinfo-overlay").html() == null) {
      $(this).after("<div id='modalinfo-overlay'></div><div id='modalinfo-content'></div><div id='modalinfoClose'><img src='/images/lightbox/lightbox-btn-close.gif' /></div>");
    }

    var w = ($(window).width() < maxWidth) ? $(window).width() * 0.7 : maxWidth;
    var h = ($(window).height() < maxHeight) ? $(window).height() * 0.7 : maxHeight;
    var lPos = (($(document).width() - w) / 2); // +$(document).scrollLeft();
    var tPos = (($(window).height() - h) / 2) + $(document).scrollTop();

    $("#modalinfo-overlay").css({ 'position': 'absolute', 'top': '0px', 'left': '0px', 'z-index': '0', 'background': '#000' });
    $("#modalinfo-overlay").css({ width: $(document).width() + 'px', height: $(document).height() + 'px', opacity: 0.7 }).show(); //.fadeIn();

    $("#modalinfo-content").css({ position: 'absolute', top: '200px', left: '200px', display: 'none', width: '600px', 'z-index': '1', background: '#fff', padding: '10px', overflow: 'auto' });
    $("#modalinfo-content").css({ 'left': lPos + 'px', 'width': w + 'px', 'top': tPos + 'px', 'height': h + 'px', 'background': '#fff' }).fadeIn();

    var href = $(this).attr("href").replace(/\s/g, '%20');
    $("#modalinfo-content").load(href + ' .content');

    $("#modalinfoClose").css({ 'position': 'absolute', 'top': '200px', display: 'none', 'z-index': '200' });
    $("#modalinfoClose").css({ 'left': lPos + w - $("#modalinfoClose").outerWidth() + 'px', 'top': tPos + 10 + 'px', 'height': '20' + 'px' }).fadeIn();

    $("#modalinfoClose").click(function () {
      $(this).fadeOut();
      $("#modalinfo-overlay").fadeOut();
      $("#modalinfo-content").fadeOut();
    });


    return false;
  });

});

jQuery(document).ready(function() {

    /* function used to calculate the window offset in every browser */
    function getScrollXY() {
        var scrOfY = 0;

        if (typeof (window.pageYOffset) == 'number') {
            //Netscape compliant
            scrOfY = window.pageYOffset;
        } else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
            //DOM compliant
            scrOfY = document.body.scrollTop;
        } else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
            //IE6 standards compliant mode
            scrOfY = document.documentElement.scrollTop;
        }

        return [scrOfY];
    }

    jQuery(window).scroll(function() {

        jQuery("#movingContainer").stop();
        var windowOffset = getScrollXY();
        jQuery("#movingContainer").animate({ top: windowOffset }, 1000);

    });
});

function openWin(url) {
    window.open(url, "print", "resizable=1,scrollbars=1");
}

function printVersion() {
    qs = window.location.search.substring(1);
    qs += "&amp;p=1";
    url = "?" + qs;
    openWin(url);
}


