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
    File: SimpleText.xslt                                                   
    Created by: Sitecore\admin                                       
    Created: 13-05-2008 12:05:12                                               
==============================================================-->

<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
  xmlns:sc="http://www.sitecore.net/sc" 
  xmlns:dot="http://www.sitecore.net/dot"
  exclude-result-prefixes="dot sc">

<!-- output directives -->
<xsl:output method="html" indent="no" encoding="UTF-8" />

<!-- parameters -->
<xsl:param name="lang" select="'en'"/>
<xsl:param name="id" select="''"/>
<xsl:param name="sc_item"/>
<xsl:param name="sc_currentitem"/>
<xsl:param name="Title" select="'Title'"/>
<xsl:param name="Text" select="'Text'"/>

  <!-- entry point -->
<xsl:template match="*">
  <xsl:apply-templates select="$sc_item" mode="main"/>
</xsl:template>

<!--==============================================================-->
<!-- main                                                         -->
<!--==============================================================-->
<xsl:template match="*" mode="main">
  
  <div class="content">
    <div class="colMargin8NoTopMargin">
      <h1>
        <xsl:value-of select="sc:field($Title,.)" disable-output-escaping="yes" />
      </h1>
      <div class="textContainer">
        <xsl:value-of select="sc:field($Text,.)" disable-output-escaping="yes" />
      </div>
    </div>
  </div>
</xsl:template>

</xsl:stylesheet>
