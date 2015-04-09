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

  <!-- variables -->
  <xsl:variable name="bgColor" select="ec:GetDesignSetting('Product Image Background Color')"/>
  
  <xsl:template name="RotatorSpot">
    <xsl:param name="ids"/>
    <xsl:param name="idx"/>
    <xsl:param name="identifier"/>
    <xsl:param name="width"/>
    <xsl:param name="timeout"/>

    <xsl:choose>
      <xsl:when test="$ids">
        <xsl:variable name="itm_id" select="substring-before($ids, '|')"/>
        <xsl:if test="$itm_id">
          <xsl:variable name="spotItem" select="sc:item($itm_id,.)"/>
          <xsl:if test="$spotItem">
            <div  style="display:none">
              <xsl:attribute name="id">
                <xsl:value-of select="concat($identifier,'_', $idx)"/>
              </xsl:attribute>
              <xsl:choose>
                <xsl:when test="sc:fld('Link',$spotItem,'url')">
                  <sc:link field="Link" select="$spotItem">
                    <sc:image field="image" select="$spotItem" w="{$width}" as="true" bc="{$bgColor}" h="{ec:GetMaxImgHeighFromChildren($spotItem/parent::item,'Image', $width)}" />
                  </sc:link>
                </xsl:when>
                <xsl:otherwise>
                  <sc:image field="image" select="$spotItem" w="{$width}" as="true" bc="{$bgColor}" h="{ec:GetMaxImgHeighFromChildren($spotItem/parent::item,'Image', $width)}" />
                </xsl:otherwise>
              </xsl:choose>
            </div>
          </xsl:if>
        </xsl:if>


        <!-- Commented out by Kasper because it wouldn't build -->
        <xsl:call-template name="RotatorSpot">
          <xsl:with-param name="ids" select="substring-after($ids, '|')"/>
          <xsl:with-param name="idx" select="$idx + 1"/>
          <xsl:with-param name="identifier" select="$identifier"/>
          <xsl:with-param name="width" select="$width"/>
          <xsl:with-param name="timeout" select="$timeout"/>
        </xsl:call-template>

      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="RotateScript">
          <xsl:with-param name="count" select="$idx"/>
          <xsl:with-param name="timeout" select="$timeout"/>
          <xsl:with-param name="identifier"  select="$identifier"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="RotateScript">
    <xsl:param name="count" select="''"/>
    <xsl:param name="timeout" select="''"/>
    <xsl:param name="identifier" select="''"/>

    <xsl:if test="$count > 0 and not(ec:IsEditingMode())">
      <script type="text/javascript">

        var <xsl:value-of select="$identifier"/>_running = true;
        var <xsl:value-of select="$identifier"/>_i =0;

        function <xsl:value-of select="$identifier"/>()
        {
        var spotsCount = <xsl:value-of select="$count"/>;
        var currentNomber = <xsl:value-of select="$identifier"/>_i % spotsCount;
        var prevNomber = (<xsl:value-of select="$identifier"/>_i + spotsCount - 1) % spotsCount;


        jQuery("#<xsl:value-of select="$identifier"/>_" + prevNomber).css("display", "none");
        jQuery("#<xsl:value-of select="$identifier"/>_" + currentNomber).css("display", "inline");

        <xsl:value-of select="$identifier"/>_i++;
        <xsl:value-of select="$identifier"/>_timerID = setTimeout(<xsl:value-of select="$identifier"/>,<xsl:value-of select="$timeout"/>);
        }

        var <xsl:value-of select="$identifier"/>_timerID = setTimeout(<xsl:value-of select="$identifier"/>,1);
      </script>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>
