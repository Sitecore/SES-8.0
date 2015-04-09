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



  <!--==============================================================-->
  <!-- pager                                                        -->
  <!--==============================================================-->
  <xsl:template name="pager">
    <xsl:param name="count" />
    <xsl:param name="pagesize" />
    <xsl:param name="groupsize" select="10" />
    <xsl:param name="currentpage" />
    <xsl:param name="sc_item" />
    <xsl:param name="showSorting"/>

    <!-- get an XML structure representing the pager -->
    <xsl:param name="pager" select="ec:GetPagerXml($pagesize, $groupsize, $count, $currentpage)/root"/>


    <xsl:if test="count($pager/page)!=1 and $count != 0">
      <div class="pagerSortingContainer">
        <div class="pager">


          <!-- link to previous group -->
          <xsl:if test="$pager/page[@previousgroup]">
            <div class="pagerItemPreviousGroup">
              <a class="pagerLink" name="{$pager/page[@previousgroup]}" href="{ec:GetPagerLink($sc_item , $pager/page[@previousgroup])}" title="{ec:Translate('Pager Previous Group Title')}">
                <xsl:value-of select="ec:Translate('Pager Previous Group Link')"/>
              </a>
            </div>
          </xsl:if>

          <!-- link to previous page -->
          <xsl:if test="$pager/@first='false'">
            <div class="pagerItemPrevious">
              <a class="pagerLink" name="{$pager/page[@previous]}" href="{ec:GetPagerLink($sc_item, $pager/page[@previous])}" title="{ec:Translate('Pager Previous Title')}">
                <xsl:value-of select="ec:Translate('Pager Previous Link')"/>
              </a>
            </div>
          </xsl:if>



          <xsl:for-each select="$pager/page">
            <xsl:if test="@visible">
              <div class="pagerItem">
                <xsl:choose>
                  <xsl:when test="@current">
                    <b>
                      <xsl:value-of select="."/>
                    </b>
                  </xsl:when>
                  <xsl:otherwise>
                    <a class="pagerLink" name="{.}" href="{ec:GetPagerLink($sc_item, .)}">
                      <xsl:value-of select="."/>
                    </a>
                  </xsl:otherwise>
                </xsl:choose>
              </div>
            </xsl:if>
          </xsl:for-each>

          <!-- link to next page -->
          <xsl:if test="$pager/@last='false'">
            <div class="pagerItemNext">
              <a class="pagerLink" name="{$pager/page[@next]}" href="{ec:GetPagerLink($sc_item, $pager/page[@next])}" title="{ec:Translate('Pager Next Title')}">
                <xsl:value-of select="ec:Translate('Pager Next Link')"/>
              </a>
            </div>
          </xsl:if>

          <!-- link to next group -->
          <xsl:if test="$pager/page[@nextgroup]">
            <div class="pagerItemNextGroup">
              <a class="pagerLink" name="{$pager/page[@nextgroup]}" href="{ec:GetPagerLink($sc_item, $pager/page[@nextgroup])}" title="{ec:Translate('Pager Next Group Title')}">
                <xsl:value-of select="ec:Translate('Pager Next Group Link')"/>
              </a>
            </div>
          </xsl:if>

        </div>
        <!-- pager -->

        <xsl:if test="$showSorting">
          <xsl:variable name="pageSizes" select="sc:item('/sitecore/system/Modules/Ecommerce/System/Pager Paging Size',.)"/>
          
          <div class="sorting">
            <select id="pagesize" onchange="location.href='{ec:GetPageSizeLink($sc_item, $pager/page[@current])}&amp;pagesize='+ $(this).val()">
              <xsl:for-each select="$pageSizes/child::item">
                <option value="{sc:fld('pageSize',.)}">
                  <xsl:value-of select="sc:fld('Title',.)"/>
                </option>
              </xsl:for-each>
            </select>
          </div>
          <!-- sorting options -->
        </xsl:if>
      </div>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>