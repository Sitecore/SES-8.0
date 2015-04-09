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

<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:sc="http://www.sitecore.net/sc"
  xmlns:dot="http://www.sitecore.net/dot"
  xmlns:ec="http://www.sitecore.net/ec"
  xmlns:ecan="http://www.sitecore.net/ecommerceanalytics"
  exclude-result-prefixes="dot sc ec">

  <xsl:import href="Global Variables.xslt" />

  <!-- output directives -->
  <xsl:output method="xml" indent="no" encoding="UTF-8" omit-xml-declaration="yes" />

  <!--==============================================================-->
  <!-- Column                                                       -->
  <!--==============================================================-->
  <xsl:template name="column">
    <xsl:param name="idx"/>
    <xsl:param name="columns"/>
    <xsl:param name="item"/>

    <xsl:variable name="mw" xml:space="default">
      <xsl:choose>
        <xsl:when test="$columns=2">
          <xsl:value-of select="$TwoColWidth"/>
        </xsl:when>
        <xsl:when test="$columns=3">
          <xsl:value-of select="$ThreeColWidth"/>
        </xsl:when>
        <xsl:when test="$columns=4">
          <xsl:value-of select="$FourColWidth"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="mh" xml:space="default">
      <xsl:choose>
        <xsl:when test="$columns=2">
          <xsl:value-of select="$TwoColHeight"/>
        </xsl:when>
        <xsl:when test="$columns=3">
          <xsl:value-of select="$ThreeColHeight"/>
        </xsl:when>
        <xsl:when test="$columns=4">
          <xsl:value-of select="$FourColHeight"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <div class="col">
      <div class="colMargin10">
        <div class="textContainer">
          <h2>
            <xsl:choose>
              <xsl:when test="sc:fld(concat('Title', $idx),.)">
                <xsl:variable name="field"  select="concat('Title', $idx)"/>
                <a href='{ecan:GetVirtualProductUrlWithAnaliticsQueryString($sc_currentitem, $item)}'>
                  <sc:text field="$field"/>
                </a>
              </xsl:when>
              <xsl:otherwise>
                <a href='{ecan:GetVirtualProductUrlWithAnaliticsQueryString($sc_currentitem, $item)}'>
                  <sc:text field="Title" select="$item"/>
                </a>
              </xsl:otherwise>
            </xsl:choose>
          </h2>
          <xsl:if test="$columns!=4">
            <p>
              <xsl:choose>
                <xsl:when test="sc:fld(concat('Description',$idx),.)">
                  <xsl:variable name="field"  select="concat('Description', $idx)"/>
                  <sc:text field="$field"/>
                </xsl:when>
                <xsl:otherwise>
                  <sc:text field="Short Description" select="$item"/>
                </xsl:otherwise>
              </xsl:choose>
            </p>
          </xsl:if>
        </div>

        <xsl:choose>
          <xsl:when test="sc:fld(concat('image',$idx),.,'src')">
            <xsl:variable name="field"  select="concat('Image', $idx)"/>
            <a href='{ecan:GetVirtualProductUrlWithAnaliticsQueryString($sc_currentitem, $item)}'>
              <sc:image field="$field" w="{$mw}" h="{$mh}" as="true" bc="{$bgColor}"/>
            </a>
          </xsl:when>
          <xsl:otherwise>
            <xsl:variable name="mediaProductItem" select="ec:GetFirstImageItem('images', $item)"/>
            <xsl:variable name="mediaParameters" select="concat('w=', $mw, '&amp;h=', $mh, '&amp;as=1&amp;bc=', $bgColor)"/>
            <a href='{ecan:GetVirtualProductUrlWithAnaliticsQueryString($sc_currentitem, $item)}'>
              <img alt="{sc:fld('alt',$mediaProductItem)}" title="{sc:fld('alt',$mediaProductItem)}" src="{sc:GetSignedMediaUrl($mediaProductItem, true, $mediaParameters, false)}" />
            </a>
          </xsl:otherwise>
        </xsl:choose>

        <xsl:if test="$item">
          <!-- controls -->
          <div class="leftAlignedContainer">
            <div class="colControlButtons">
              <input type="button" class="btnSmall" value="{ec:Translate('BUY')}">
                <xsl:attribute name="onclick">
                  AddToShoppingCart('<xsl:value-of select="sc:fld('Product Code',$item)"/>', 1);return false;
                </xsl:attribute>
              </input>
            </div>

            <!-- Prices -->
            <div class="colPrice">
              <xsl:choose>
                <xsl:when test="sc:IsLoggedIn()">
                  <div class="priceMain">
                    <xsl:value-of select="ec:GetCustomerPrice($item)"/>
                  </div>
                  <div class="priceOriginal">
                    <xsl:value-of select="ec:GetListPrice($item)"/>
                  </div>
                </xsl:when>
                <xsl:otherwise>
                  <div class="priceMain">
                    <xsl:value-of select="ec:GetListPrice($item)"/>
                  </div>
                </xsl:otherwise>
              </xsl:choose>
            </div>
          </div>
        </xsl:if>
      </div>
      <div class="clear"></div>
    </div>
  </xsl:template>
</xsl:stylesheet>