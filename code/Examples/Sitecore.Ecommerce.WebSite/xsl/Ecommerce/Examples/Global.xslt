<?xml version="1.0" encoding="UTF-8"?>

<!--======================================================================================
Copyright 2015 Sitecore Corporation A/S
Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
except in compliance with the License. You may obtain a copy of the License at
    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the 
License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
either express or implied. See the License for the specific language governing permissions 
and limitations under the License.
======================================================================================-->


<!--=============================================================
    File: Global.xslt                                                   
    Created by: sitecore\admin                                       
    Created: 12.05.2008 16:59:55                                               
==============================================================-->

<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:sc="http://www.sitecore.net/sc"
  xmlns:dot="http://www.sitecore.net/dot"
  xmlns:ec="http://www.sitecore.net/ec"
  xmlns:ecan="http://www.sitecore.net/ecommerceanalytics"
  exclude-result-prefixes="dot sc ec ecan">
  
  <!--=========================================
      Products In Stock
  =============================================-->
  <xsl:template name="products-in-stock">
    <xsl:param name="itm" />
    
    <xsl:variable name="code" select="sc:fld('Product Code',$itm)"/>
    <xsl:variable name="stock" select="ec:GetStock($code)"/>
    <xsl:choose>
      <xsl:when test="$stock > 0">
        <span class="header">
          <xsl:value-of select="ec:Translate('Stock')"/>:
        </span>&#160;<xsl:value-of select="$stock"/>
      </xsl:when>
      <xsl:otherwise>
        <span class="header">
          <xsl:value-of select="ec:Translate('Temporarily out of stock')"/>
        </span>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!--=========================================
      Products Detail in product list
  =============================================-->
  <xsl:template name="product-detail">
    <xsl:param name="itm" />
    <xsl:param name="list" />
    <li>
      <div class="colImage">
        <a href='{ecan:GetVirtualProductUrlWithAnaliticsQueryString($sc_currentitem, $itm)}'>
          <xsl:variable name="mediaProductItem" select="ec:GetFirstImageItem('Images', $itm)"/>
          <img id="product_shot" class="fix" alt="{sc:fld('alt',$mediaProductItem)}" title="{sc:fld('alt',$mediaProductItem)}" src="{sc:GetSignedMediaUrl($mediaProductItem, true, '?w=100&amp;h=100&amp;as=1&amp;bc=white', false)}" />
        </a>
      </div>
      <!-- image -->
      <div class="colText">
        <h2>
          <a href='{ecan:GetVirtualProductUrlWithAnaliticsQueryString($sc_currentitem, $itm)}'>
            <sc:text field="Title" select="$itm"/>
          </a>
        </h2>        
        <sc:html field="Short Description" select="$itm"/>

        <p class="inStock">
          <xsl:call-template name="products-in-stock" >
            <xsl:with-param name="itm" select="$itm"/>
          </xsl:call-template>
        </p>
      </div>
      <!-- text -->

      <div class="clear"></div>


      <div class="rightAlignedContainer">
        <div class="colControlButtons">
          <input type="button" class="btnSmall" value="{ec:Translate('BUY')}">
            <xsl:attribute name="onclick">AddToShoppingCart('<xsl:value-of select="sc:fld('Product Code',$itm)"/>', 1);return false;</xsl:attribute>            
          </input>
        </div>
        <!-- controls -->

        <div class="colPrice">
          <xsl:choose>
            <xsl:when test="sc:IsLoggedIn()">
              <div class="priceMain">
                <xsl:value-of select="ec:GetCustomerPrice($itm)"/>
              </div>
              <div class="priceOriginal">
                <xsl:value-of select="ec:GetListPrice($itm)"/>
              </div>
            </xsl:when>
            <xsl:otherwise>
              <div class="priceMain">
                <xsl:value-of select="ec:GetListPrice($itm)"/>
              </div>
            </xsl:otherwise>
          </xsl:choose>          
        </div>
        <!-- priceCurrent -->

      </div>
    </li>
  </xsl:template>
  
  
</xsl:stylesheet>
