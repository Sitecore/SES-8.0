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
    File: ProductTabs.xslt                                                   
    Created by: sitecore\admin                                       
    Created: 12.05.2008 16:59:55                                               
==============================================================-->

<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:sc="http://www.sitecore.net/sc"
  xmlns:dot="http://www.sitecore.net/dot"
  xmlns:ec="http://www.sitecore.net/ec"
  exclude-result-prefixes="dot sc ec">

   <!-- output directives -->
   <xsl:output method="html" indent="no" encoding="UTF-8" />

   <!-- parameters -->
   <xsl:param name="lang" select="'en'"/>
   <xsl:param name="id" select="''"/>
   <xsl:param name="sc_item"/>
   <xsl:param name="sc_currentitem"/>

   <xsl:param name="SelectedTab"/>

   <!-- variables -->
   <xsl:variable name="home" select="$sc_item/ancestor-or-self::item[@template='site root']" />


   <!-- include fiels-->
   <xsl:include href="Global.xslt"/>

  <!-- entry point -->
  <xsl:template match="*">
    <xsl:choose>
      <xsl:when test="contains($sc_item/@template, 'variant')">
        <xsl:apply-templates select="$sc_item/parent::item" mode="main"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="$sc_item" mode="main"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!--=========================================
                 ProductAccessories
  =============================================-->
   <xsl:template match="*" mode="main">
      <xsl:variable name="ids" select="concat(sc:fld('Accessories',.),'|')"/>

     <div id="tab_content_accessories">
       <ul class="ulProductList">
              <xsl:call-template name="AccessoriesItem">
                 <xsl:with-param name="ids" select="$ids"/>
              </xsl:call-template>
           </ul>
        <div class="clear"></div>
      </div>
   </xsl:template>


   <xsl:template name="AccessoriesItem">
      <xsl:param name="ids" select="''" />

      <xsl:if test="$ids">
         <xsl:variable name="itm_id" select="substring-before($ids, '|')"/>
         <xsl:if test="$itm_id">
            <xsl:variable name="itm" select="sc:item($itm_id,.)"/>
           <!-- Start: Single Product -->
           <xsl:call-template name="product-detail">
             <xsl:with-param name="itm" select="$itm"/>
             <xsl:with-param name="list" select="'Accessories'"/>
           </xsl:call-template>
           <!-- End: Single Product -->
         </xsl:if>
         <xsl:call-template name="AccessoriesItem">
            <xsl:with-param name="ids" select="substring-after($ids, '|')"/>
         </xsl:call-template>
      </xsl:if>
   </xsl:template>

 
</xsl:stylesheet>
