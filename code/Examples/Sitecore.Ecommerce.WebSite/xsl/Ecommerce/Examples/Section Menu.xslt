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
  <xsl:output method="html" indent="no" encoding="UTF-8" />

  <!-- parameters -->
  <xsl:param name="lang" select="'en'"/>
  <xsl:param name="id" select="''"/>
  <xsl:param name="sc_item"/>
  <xsl:param name="sc_currentitem"/>

  <!-- If MenuRootPath is not empty it will be root for menu -->
  <!-- Example: /sitecore/content/Ecommerce/Products -->
  <xsl:param name="MenuRootPath" select="'/sitecore/content/E-Commerce Examples/Home'"/>
  <xsl:param name="IncludeTemplates" select="'!site section!product site section!'"/>
  <xsl:param name="MenuItemFieldName" select="'Menu title'"/>
  <xsl:param name="IsHideFieldName" select="'Hide Menu'"/>
  <xsl:param name="CSSClassOpenItem" select="'current'"/>
  <xsl:param name="CSSClassCloseItem" select="''"/>
  <xsl:param name="CSSClassSelectedItem" select="'current'"/>
  <xsl:param name="DeepLevel" select="1"/>
  <xsl:param name="ExcludeTemplates" select="'!newsitem!slr!p and s!lenses!other accessories!flash!navigation links!layout section!'"/>

  <!-- entry point -->
  <xsl:template match="*">
    <xsl:apply-templates select="$sc_item" mode="main"/>
  </xsl:template>

  <!--==============================================================-->
  <!-- main                                                         -->
  <!--==============================================================-->
  <xsl:template match="*" mode="main">
    <xsl:choose >
        <xsl:when test="$MenuRootPath!=''">
          <xsl:call-template name="rootmenu">
            <xsl:with-param name="root" select="sc:item($MenuRootPath,.)"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="rootmenu">
            <xsl:with-param name="root" select="ec:GetClosestActualItem()/ancestor-or-self::item[contains($IncludeTemplates, concat('!',@template,'!'))]"/>
          </xsl:call-template>
        </xsl:otherwise>
      </xsl:choose>
  </xsl:template>

  <!--==============================================================-->
  <!-- Section menu                                                 -->
  <!--==============================================================-->
  <xsl:template name="rootmenu" >
    <xsl:param name="root" select="''"/>
<!--
    <h2>
      <xsl:call-template name="showtitle">
        <xsl:with-param name="root" select="$root"/>
      </xsl:call-template>
    </h2>
-->
    <ul id="lc_menu" class="clearfix">
<xsl:choose>
  <xsl:when test="$sc_currentitem/@name='Home'">
    <li class="current"><a href="/">Home</a></li>
  </xsl:when>
  <xsl:otherwise>
    <li><a href="/">Home</a></li>
  </xsl:otherwise>
</xsl:choose>
      <xsl:call-template name="sectionmenu">
        <xsl:with-param name="root" select="$root"/>
        <xsl:with-param name="level" select="0"/>
      </xsl:call-template>
    </ul>
  </xsl:template>


  <!--==============================================================-->
  <!-- Section menu                                                 -->
  <!--==============================================================-->
  <xsl:template name="sectionmenu" >
    <xsl:param name="root" select="''"/>
    <xsl:param name="level" select="0"/>


    <xsl:for-each select="$root/item[not(contains($ExcludeTemplates, concat('!',@template,'!')))]">
      <xsl:sort select="@sortorder" data-type="number"/>
      <xsl:variable name="IsHaveChild" select="boolean(./item)" />
      <xsl:variable name="IsSelected" select="boolean(./descendant-or-self::item[@id=ec:GetClosestActualItem()/@id])" />
      <xsl:variable name="IsShow" select="boolean(sc:fld($IsHideFieldName,.)!=1)" />
      <xsl:variable name="IsCurrent" select="boolean(@id=$sc_currentitem/@id)" />
      <sc:sec/>
        <xsl:if test="$IsShow and (contains(@template, 'product search group') or contains(@template, 'page'))">
        <li>
          <xsl:choose>
            <xsl:when test="$level=0">
              <xsl:choose>
                <xsl:when test="$IsSelected">
                  <xsl:attribute name="class">
                    <xsl:value-of select="$CSSClassOpenItem"/>
                  </xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="class">
                    <xsl:value-of select="$CSSClassCloseItem"/>
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
              
            </xsl:when>
            <xsl:otherwise>
              
              <xsl:choose>
                <xsl:when test="$IsSelected">
                  <xsl:attribute name="class">
                    <xsl:value-of select="$CSSClassSelectedItem"/>
                  </xsl:attribute>
                </xsl:when>
              </xsl:choose>
              
            </xsl:otherwise>
          </xsl:choose>
          <sc:link>
            <xsl:call-template name="showtitle">
              <xsl:with-param name="root" select="."/>
            </xsl:call-template>
          </sc:link>
          <xsl:if test="$level &lt; $DeepLevel and $IsSelected and $IsHaveChild" >
            <ul>
              <xsl:call-template name="sectionmenu">
                <xsl:with-param name="root" select="."/>
                <xsl:with-param name="level" select="$level+1"/>
              </xsl:call-template>
            </ul>
          </xsl:if>
        </li>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

  <!--==============================================================-->
  <!-- Menu title                                                   -->
  <!--==============================================================-->
  <xsl:template name="showtitle">
    <xsl:param name="root"/>

    <xsl:choose>
      <xsl:when test="sc:fld($MenuItemFieldName,$root)!=''">
        <xsl:value-of select="sc:field($MenuItemFieldName,$root)" disable-output-escaping="yes"/>
      </xsl:when>
      <xsl:otherwise>
<xsl:choose>
  <xsl:when test="sc:fld('title', $root)=''">
        <xsl:value-of select="$root/@name" />
  </xsl:when>
  <xsl:otherwise>
        <xsl:value-of select="sc:fld('title', $root)" />
  </xsl:otherwise>
</xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
    
  </xsl:template>

</xsl:stylesheet>
