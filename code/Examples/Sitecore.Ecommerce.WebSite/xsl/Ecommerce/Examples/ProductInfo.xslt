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
    File: ProductInfo.xslt                                                   
    Created by: sitecore\admin                                       
    Created: 12.05.2008 12:22:42                                               
==============================================================-->

<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:sc="http://www.sitecore.net/sc"
  xmlns:dot="http://www.sitecore.net/dot"
  xmlns:ec="http://www.sitecore.net/ec"
  exclude-result-prefixes="dot sc">

  <!-- output directives -->
  <xsl:output method="html" indent="no" encoding="UTF-8" />

  <!-- parameters -->
  <xsl:param name="lang" select="'en'"/>
  <xsl:param name="id" select="''"/>
  <xsl:param name="sc_item"/>
  <xsl:param name="sc_currentitem"/>
  <xsl:param name="ImageFieldName" select="'Image'"/>

  <!-- variables -->
  <xsl:variable name="home" select="$sc_item/ancestor-or-self::item[@template='site root']" />
  <xsl:variable name="MaxHeight" select="ec:GetDesignSetting('Product Image Max Width')"/>
  <xsl:variable name="MaxWidth" select="ec:GetDesignSetting('Product Image Thumbnail Heigth')"/>
  <xsl:variable name="ImageWidth" select="ec:GetDesignSetting('Product Info Image Width')"/>
  <xsl:variable name="ThumbnailImageWidth" select="ec:GetDesignSetting('Product Info Thumbnail Image Width')"/>

  <xsl:include href="ProductVariantSelector.xslt"/>

  <!-- entry point -->
  <xsl:template match="*">
    <xsl:apply-templates select="$sc_item" mode="main"/>
  </xsl:template>

  <!--==============================================================-->
  <!-- main                                                         -->
  <!--==============================================================-->
  <xsl:template match="*" mode="main">
    <!-- *************************************
                 **  Product
                 ************************************* -->
    <div class="colMargin8NoTopMargin">

      <div class="pictureContainer">
        <xsl:variable name="images" select="ec:VariantOrProductItems('images',.)" />
        <xsl:variable name="anchorParameters" select="concat('mw=', $MaxWidth, '&amp;mh=', $MaxHeight, '&amp;as=1')" />
        <xsl:variable name="imageParameters" select="concat('w=', $ImageWidth, '&amp;as=1')" />
        <xsl:variable name="thumbnailParameters" select="concat('w=', $ThumbnailImageWidth, '&amp;as=1')" />

        <xsl:for-each select="$images">
          <xsl:if test="position() = 1">
            <a href="{sc:GetSignedMediaUrl(., true, $anchorParameters, false)}" rel="lightbox">
              <img id="product_shot" class="fix" alt="{sc:fld('alt',.)}" title="{sc:fld('alt',.)}" src="{sc:GetSignedMediaUrl(., true, $imageParameters, false)}" />
            </a>
            <div class="clear"></div>
          </xsl:if>
        </xsl:for-each>

        <xsl:if test="count($images) > 1">
          <div class="tbnContainer">
            <xsl:for-each select="$images">
              <a href="{sc:GetSignedMediaUrl(., true, $anchorParameters, false)}" rel="lightbox">
                <img alt="{sc:fld('alt',.)}" title="{sc:fld('alt',.)}" src="{sc:GetSignedMediaUrl(., true, $thumbnailParameters, false)}" />
              </a>
            </xsl:for-each>
          </div>
        </xsl:if>
      </div>
      <!-- pictureArea -->

      <div class="descriptionContainer">
        <h1>
          <sc:text field="Title" select="ec:GetVariantOrProduct('Title',.)" />
        </h1>

        <p class="teaser">
          <sc:text field="Short Description" select="ec:GetVariantOrProduct('Short Description',.)" />
        </p>

        <div>
          <xsl:apply-templates mode="product-variant-selector" select="." />
        </div>

        <div class="priceAndBuyContainer">
          <div class="priceContainer">
            <xsl:choose>
              <xsl:when test="sc:IsLoggedIn()">
                <div class="priceMain">
                  <xsl:value-of select="ec:GetCustomerPrice(.)"/>
                </div>
                <div class="priceOriginal">
                  <xsl:value-of select="ec:GetListPrice(.)"/>
                </div>
              </xsl:when>
              <xsl:otherwise>
                <div class="priceMain">
                  <xsl:value-of select="ec:GetListPrice(.)"/>
                </div>
              </xsl:otherwise>
            </xsl:choose>
          </div>

          <div class="colCount">
            <input type="text" value="1" id="quantity" name="quantity"/>
          </div>
                   
          <!-- amount -->          
          <div class="btnContainer">            
            <input type="submit" class="btnMedium" name="btn_add_{sc:fld('Product Code',ec:GetVariantOrProduct('Product Code', .))}" id="btn{@id}" value="{ec:Translate('BUY')}">
              <xsl:attribute name="onclick">
                javascript:AddToShoppingCart('<xsl:value-of select="sc:fld('Product Code',ec:GetVariantOrProduct('Product Code', .))"/>', $('#quantity').val(),
              '<xsl:value-of select="sc:fld('Unit Of Measures',.)" />');return false;
              </xsl:attribute>
            </input>
          </div>
          <!-- button -->

          <div class="colStockAndDelivery">
            <xsl:variable name="code" select="sc:fld('Product Code', ec:GetVariantOrProduct('Product Code', .))"/>
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
          </div>
          <!-- Stock -->
        </div>

        <!-- details container -->
        <div class="clear"></div>

        <div>
          <h2>
            <xsl:value-of select="ec:Translate('Description')"/>
          </h2>
          <sc:html field="Description" select="ec:GetVariantOrProduct('Description',.)" />
        </div>
        <!-- description -->

        <div class="clear"></div>
      </div>
      <!-- description container -->
      <div class="clear"></div>
    </div>
    <!-- procuct_container -->
  </xsl:template>
</xsl:stylesheet>
