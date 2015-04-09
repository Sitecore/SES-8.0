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
                 Product Specification
  =============================================-->
  <xsl:template match="*" mode="main">
      <xsl:variable name="specFldNames" select="ec:Specifications(./@id)" />

      <div id="tab_content_specifications">
         <table>
             <xsl:call-template name="ProductSpecificationRow">
                <xsl:with-param name="specFldNames" select="$specFldNames"/>
                <xsl:with-param name="row" select="0"/>
             </xsl:call-template>
         </table>
        <div class="clear"></div>
      </div>
   </xsl:template>

   <!--=========================================
                 Product Specification Row
  =============================================-->
   <xsl:template name="ProductSpecificationRow">
      <xsl:param name="specFldNames" select="''"/>
      <xsl:param name="row" select="''"/>
      <xsl:if test="$specFldNames">
         <xsl:variable name="fldName" select="substring-before($specFldNames, '|')"/>
         <xsl:if test="$fldName  ">
            <tr>
               <xsl:choose>
                  <xsl:when test="$row mod 2 = 0">
                     <xsl:attribute name="class">
                        odd
                     </xsl:attribute>
                  </xsl:when>
                  <xsl:otherwise>
                     <xsl:attribute name="class">
                        even
                     </xsl:attribute>
                  </xsl:otherwise>
               </xsl:choose>

               <th class="feature"><xsl:value-of select="$fldName"/></th>
               <td><xsl:value-of select="sc:field($fldName,.)" disable-output-escaping="yes"/></td>
              
            </tr>
         </xsl:if>
         <xsl:call-template name="ProductSpecificationRow">
            <xsl:with-param name="specFldNames" select="substring-after($specFldNames, '|')"/>
            <xsl:with-param name="row" select="$row + 1"/>
         </xsl:call-template>
      </xsl:if>
   </xsl:template>

</xsl:stylesheet>
