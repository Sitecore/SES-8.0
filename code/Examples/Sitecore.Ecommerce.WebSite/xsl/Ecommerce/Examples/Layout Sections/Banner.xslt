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
  exclude-result-prefixes="dot sc ec">

  <!-- output directives -->
  <xsl:output method="xml" indent="no" encoding="UTF-8" omit-xml-declaration="yes" />

  <!-- parameters -->
  <xsl:param name="lang" />
  <xsl:param name="id" />
  <xsl:param name="sc_item"/>
  <xsl:param name="sc_currentitem"/>
  <xsl:param name="itemID"/>

  <!-- variables-->
  <xsl:variable name="OneColWidth" select="ec:GetDesignSetting('One Column Width')"/>
  <xsl:variable name="bgColor" select="ec:GetDesignSetting('Product Image Background Color')"/>

  <!--==============================================================-->
  <!-- main                                                         -->
  <!--==============================================================-->
  <xsl:template match="*">
    <xsl:choose>
      <xsl:when test ="$itemID">
        <xsl:apply-templates mode="layout-section" select="sc:item($itemID,.)" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates mode ="layout-section" select="." />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!--=========================================
      Column template - Banner
  =============================================-->
  <xsl:template match="*[@template='banner']" mode="layout-section">
    <div class="content">
      <div class="colMargin8NoTopMargin">
        <xsl:choose>
          <xsl:when test="sc:fld('link',.,'url')">
            <sc:link field="Link">
              <sc:image field="image" w="{$OneColWidth}" as="1" bc="{$bgColor}"/>
            </sc:link>
          </xsl:when>
          <xsl:otherwise>
            <sc:image field="image" w="{$OneColWidth}" as="1" bc="{$bgColor}"/>
          </xsl:otherwise>
        </xsl:choose>
      </div>
      <div class="clear"></div>
    </div>
  </xsl:template>
</xsl:stylesheet>