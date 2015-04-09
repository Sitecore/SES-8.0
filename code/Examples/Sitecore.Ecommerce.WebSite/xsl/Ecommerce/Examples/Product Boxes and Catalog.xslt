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
    File: Product Catalog.xslt                                                   
    Created by: Alexander Tsvirchkov 
    Created: 07.05.2008 15:35:34                                               
==============================================================-->

<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:sc="http://www.sitecore.net/sc"
  xmlns:dot="http://www.sitecore.net/dot"
  xmlns:ec="http://www.sitecore.net/ec"
  exclude-result-prefixes="dot sc">

  <!-- output directives -->
  <xsl:output method="xml" indent="no" encoding="UTF-8" omit-xml-declaration="yes" />

  <!-- parameters -->
  <xsl:param name="lang" select="'en'"/>
  <xsl:param name="id" select="''"/>
  <xsl:param name="sc_item"/>
  <xsl:param name="sc_currentitem"/>
  <xsl:param name="SiteRootTemplateName" select="'sample item'"/>


  <!-- variables -->
  <xsl:variable name="currentpage" select="ec:RequestVariable('pg', '1')" />
  <xsl:variable name="pagesize" select="ec:RequestVariable('pagesize', '10')" />

  <!-- include fiels-->
  <xsl:include href="pager.xslt"/>
  <xsl:include href="LayoutSections.xslt"/>
  <xsl:include href="Global.xslt"/>

  <!-- entry point -->
  <xsl:template match="*">
    <xsl:apply-templates select="$sc_item" mode="main"/>
  </xsl:template>

  <!--==============================================================-->
  <!-- main                                                         -->
  <!--==============================================================-->
  <xsl:template match="*" mode="main">
    <xsl:param name="items" select="ec:GetProductsForCatalog(.)"/>
    <xsl:param name="cat" select="." />

    <xsl:variable name="sortItemId" select="ec:GetFirstAscendantOrSelfWithValue('sorting',.)"/>
    <xsl:variable name="sortItem" select="ec:Item($sortItemId)"/>
    <xsl:variable name="sortexpr" select="ec:RequestVariable('sortexpr')" />
    <xsl:variable name="products" select="ec:GetItemsFiltered(ec:RequestVariable('filter1'), ec:RequestVariable('filter2'), ec:RequestVariable('filter3'), $items)"/>
    <xsl:variable name="field" select="ec:GetSortField($sortexpr, 'forpris')"/>
    <xsl:variable name="dir" select="ec:GetSortDirection($sortexpr)"/>
    <xsl:variable name="sortType" select="ec:GetSortType($sortexpr)"/>
    <xsl:variable name="makeProductAppearUnderCategory" select="ec:GetFirstAscendantOrSelfWithValue('Make product appear under category',.)"/>

    <!-- layout sections, templates are located in files prefixed with LayoutSection -->
    <xsl:variable name="layoutSection" select="child::item[@template='layout section']"/>

    <div id="pb_home_container">
      <xsl:for-each select="$layoutSection/child::item">
        <xsl:apply-templates mode="layout-section" select="." />
      </xsl:for-each>
    </div>


    <div id="pb_product_list_container">
      <div class="content">
        <div id="pb_header_shaddow">
          <h1>
            <sc:text field="Title" />
          </h1>
        </div>
        <div class="clear"></div>
      </div>


      <!-- pager at top of page -->
      <xsl:call-template name="pager">
        <xsl:with-param name="count" select="count($products)" />
        <xsl:with-param name="pagesize" select="$pagesize" />
        <xsl:with-param name="currentpage" select="$currentpage" />
        <xsl:with-param name="showSorting" select="1"/>
      </xsl:call-template>

      <xsl:choose>
        <xsl:when test="count($products)=0">
          <ul class="ulProductList">
            <li>
            <h2>
              <xsl:value-of select="ec:Translate('No items found')"/>
            </h2>
            </li>
          </ul>
        </xsl:when>
        <xsl:otherwise>
          <ul class="ulProductList">

            <!-- Start: Header -->
            <!-- This is optional -->
            <li class="ulHeader">
              <div class="colImage">&#160;</div>
              <!-- image -->
              <div class="colText">
                <xsl:value-of select="ec:Translate('Description')"/>
              </div>
              <!-- text -->

              <div class="rightAlignedContainer">
                <div class="colControlButtons">&#160;</div>
                <!-- controls -->

                <div class="colPrice">
                  <xsl:value-of select="ec:Translate('Price')"/>
                </div>
                <!-- priceCurrent -->

              </div>
            </li>
            <!-- End: Header -->

            <xsl:choose>
              <xsl:when test="sc:fld($field,.)=''">
                <!-- Output products in the same order as in 'Products' field. -->
                <xsl:for-each select="sc:Split('Products', $cat)">
                  <xsl:variable name="productId" select="." />
                  <xsl:for-each select="$products[@id=$productId]">
                    <xsl:apply-templates mode="pageProductPresentation" select="." />
                  </xsl:for-each>
                </xsl:for-each>

                <!-- Output the rest of the products in arbitary order. -->
                <xsl:for-each select="$products[contains(sc:fld('Products',$cat),@id)=0]">
                  <xsl:apply-templates mode="pageProductPresentation" select="." />
                </xsl:for-each>
              </xsl:when>
              <xsl:otherwise>
                <xsl:for-each select="$products">
                  <xsl:sort select="sc:fld($field,.)" order="{$dir}" data-type="{$sortType}"/>
                  <xsl:apply-templates mode="pageProductPresentation" select="." />
                </xsl:for-each>
              </xsl:otherwise>
            </xsl:choose>
          </ul>
        </xsl:otherwise>
      </xsl:choose>

      <!-- pager at bottom of page -->
      <xsl:call-template name="pager">
        <xsl:with-param name="count" select="count($products)" />
        <xsl:with-param name="pagesize" select="$pagesize" />
        <xsl:with-param name="currentpage" select="$currentpage" />
        <xsl:with-param name="showSorting" select="1"/>
      </xsl:call-template>

      <div class="clear"></div>
    </div>
  </xsl:template>

  <xsl:template match="*" mode="pageProductPresentation">
    <xsl:if test="sc:qs('p')=1 or (position()  &gt; (($currentpage - 1) * $pagesize)) and (position() &lt; ($pagesize + 1 + (($currentpage - 1) * $pagesize)))">
      <xsl:variable name="indexOnPage" select="position() - (($currentpage - 1) * $pagesize)" />

      <!-- Start: Single Product -->
      <xsl:call-template name="product-detail">
        <xsl:with-param name="itm" select="."/>
        <xsl:with-param name="list" select="'Product Catalog'"/>
      </xsl:call-template>
      <!-- End: Single Product -->

    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
