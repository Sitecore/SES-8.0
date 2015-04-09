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
  <xsl:param name="lang" select="'en'"/>
  <xsl:param name="id" select="''"/>
  <xsl:param name="sc_item"/>
  <xsl:param name="sc_currentitem"/>
  <xsl:param name="itemID"/>

  <!-- include fiels-->
  <xsl:include href="Rotator Spot.xslt"/>

  <!-- variables-->
  <xsl:variable name="OneColWidth" select="ec:GetDesignSetting('One Column Width')"/>

  <!--==============================================================-->
  <!-- main                                                         -->
  <!--==============================================================-->
  <xsl:template match="*" >
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
      Column template - Rotating Banner
  =============================================-->
  <xsl:template match="*[@template='rotating banner']" mode="layout-section">
    <xsl:variable name="timeOutFldName" select="'Rotate Frequency'"/>
    <xsl:variable name="identifier" select="translate(@name,' ','_')"/>

    <div class="content">
      <div class="colMargin8NoTopMargin">

        <xsl:if test="ec:IsEditingMode()">
          <div class="editMode">
            <span>
              <xsl:value-of select="ec:FieldTitle(@id,$timeOutFldName)"/>:
              <span class="editText">
                <xsl:value-of select="sc:field($timeOutFldName,.)" disable-output-escaping="yes"/>
              </span >
            </span>
          </div>
        </xsl:if>

        <xsl:for-each select="child::item">
          <div  style="display:none">
            <xsl:attribute name="id">
              <xsl:value-of select="concat($identifier,'_', position()-1)"/>
            </xsl:attribute>

            <xsl:choose>
              <xsl:when test="sc:fld('Link',.,'url')">
                <sc:link field="Link">
                  <sc:image field="image" w="{$OneColWidth}" as="true" bc="{$bgColor}" h="{ec:GetMaxImgHeighFromChildren(parent::item,'Image', $OneColWidth)}" />
                </sc:link>
              </xsl:when>
              <xsl:otherwise>
                <sc:image field="image" w="{$OneColWidth}" as="true" bc="{$bgColor}" h="{ec:GetMaxImgHeighFromChildren(parent::item,'Image', $OneColWidth)}" />
              </xsl:otherwise>
            </xsl:choose>
          </div>
        </xsl:for-each>

        <xsl:call-template name="RotateScript">
          <xsl:with-param name="count" select="count(child::item)"/>
          <xsl:with-param name="timeout" select="sc:field($timeOutFldName,.)"/>
          <xsl:with-param name="identifier"  select="$identifier"/>
        </xsl:call-template>

      </div>
      <div class="clear"></div>
    </div>
  </xsl:template>

</xsl:stylesheet>