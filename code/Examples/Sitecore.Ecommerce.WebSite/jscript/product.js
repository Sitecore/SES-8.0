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
$(function() {
    $('a[rel*=lightbox]').lightBox({
        overlayBgColor: '#000',
        overlayOpacity: 0.7,
        imageLoading: '/images/lightbox/lightbox-ico-loading.gif',
        imageBtnClose: '/images/lightbox/lightbox-btn-close.gif',
        imageBtnPrev: '/images/lightbox/lightbox-btn-prev.gif',
        imageBtnNext: '/images/lightbox/lightbox-btn-next.gif',
        imageBlank: '/images/lightbox/lightbox-blank.gif',
        containerResizeSpeed: 350
    });


    $(".tab").click(function() {
        var TabName = $(this).attr("id");
        LoadRendering("Ecommerce/Examples/ProductTab" + TabName, $(".tabContent"));
        SetCurrentTab(TabName);
        $("#li_Specifications,#li_Accessories,#li_Resources,#li_Reviews").removeClass("current");
        $("#li_" + TabName).addClass("current");
        return false;
    });
});

function rating(id) {
    rate = parseInt(id.charAt(4));
    document.getElementById("review_rate").value = rate;
    for (i = 1; i <= rate; i++) {
        document.getElementById("rate" + i).className = "scoreSelected";
    }
    for (i = rate + 1; i <= 5; i++) {
        document.getElementById("rate" + i).className = "score";
    }
}