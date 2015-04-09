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
  <xsl:output method="xml" indent="yes" encoding="utf-8" omit-xml-declaration="yes"/>

  <!-- parameters -->
  <xsl:param name="lang" select="'en'"/>
  <xsl:param name="id" select="''"/>
  <xsl:param name="sc_item"/>
  <xsl:param name="sc_currentitem"/>

  <!-- variables -->
  <xsl:variable name="home" select="ec:GetSiteStartItem()" />
  <xsl:variable name="seperator" select="'/'"/>

  <!-- entry point -->
  <xsl:template match="*">
    <xsl:apply-templates select="$sc_item" mode="main"/>
  </xsl:template>

  <!--==============================================================-->
  <!-- main                                                         -->
  <!--==============================================================-->
  <xsl:template match="*" mode="main">
    <div class="content">
      <div id="pb_header_shaddow">
        <h1>
          <sc:text field="Title" />
        </h1>
        <div class="clear"></div>
      </div>
      <div class="clear"></div>
    </div>    
  </xsl:template>
</xsl:stylesheet>