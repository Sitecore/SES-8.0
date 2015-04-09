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


  <xsl:variable name="OneColWidth" select="ec:GetDesignSetting('One Column Width')"/>
  <xsl:variable name="TwoColWidth" select="ec:GetDesignSetting('Two Column Width')"/>
  <xsl:variable name="ThreeColWidth" select="ec:GetDesignSetting('Three Column Width')"/>
  <xsl:variable name="FourColWidth" select="ec:GetDesignSetting('Four Column Width')"/>


  <xsl:variable name="TwoColHeight" select="ec:GetDesignSetting('Two Column Height')"/>
  <xsl:variable name="ThreeColHeight" select="ec:GetDesignSetting('Three Column Height')"/>
  <xsl:variable name="FourColHeight" select="ec:GetDesignSetting('Four Column Height')"/>

  <xsl:variable name="bgColor" select="ec:GetDesignSetting('Product Image Background Color')"/>

  <!--=========================================
      Layout secton - Rich text
  =============================================-->
  <xsl:template match="*[@template='rich text']" mode="layout-section">
    <div class="content">
      <div class="colRichText">
        <h2>
          <sc:text field="title"/>
        </h2>
        <sc:text field="text"/>
      </div>
      <div class="clear"></div>
    </div>
  </xsl:template>


  <!--=========================================
      Layout secton - Two column
  =============================================-->
  <xsl:template match="*[@template='two column']" mode="layout-section">
    <xsl:variable name="item2" select="ec:Item(sc:fld('Product2',.)) "/>
    <div class="content2">
      <xsl:call-template name="column">
        <xsl:with-param name="idx" select="1"/>
        <xsl:with-param name="columns" select="2"/>
        <xsl:with-param name="item" select="ec:Item(sc:fld('Product1',.))"/>
      </xsl:call-template>
      <!-- col -->
      <xsl:call-template name="column">
        <xsl:with-param name="idx" select="2"/>
        <xsl:with-param name="columns" select="2"/>
        <xsl:with-param name="item" select="ec:Item(sc:fld('Product2',.))"/>
      </xsl:call-template>
      <!-- col -->
      <div class="clear"></div>
    </div>
  </xsl:template>



  <!--=========================================
      Layout secton - Three column
  =============================================-->
  <xsl:template match="*[@template='three column']" mode="layout-section">
    <xsl:variable name="item2" select="ec:Item(sc:fld('Product2',.)) "/>
    <div class="content3">
      <xsl:call-template name="column">
        <xsl:with-param name="idx" select="1"/>
        <xsl:with-param name="columns" select="3"/>
        <xsl:with-param name="item" select="ec:Item(sc:fld('Product1',.))"/>
      </xsl:call-template>
       <!--col--> 
      <xsl:call-template name="column">
        <xsl:with-param name="idx" select="2"/>
        <xsl:with-param name="columns" select="3"/>
        <xsl:with-param name="item" select="ec:Item(sc:fld('Product2',.))"/>
      </xsl:call-template>
       <!--col--> 
      <xsl:call-template name="column">
        <xsl:with-param name="idx" select="3"/>
        <xsl:with-param name="columns" select="3"/>
        <xsl:with-param name="item" select="ec:Item(sc:fld('Product3',.))"/>
      </xsl:call-template>
       <!--col--> 
      <div class="clear"></div>
    </div>
  </xsl:template>

  <xsl:template match="*[sc:fld('attr',.)='Nynorsk']" mode="layout-section">
    <div class="content">
      <div class="col">
        Dette er standard rederingen
      </div>
    </div>
  </xsl:template>
  

  <!--=========================================
      Layout secton - Four column
  =============================================-->
  <xsl:template match="*[@template='four column']" mode="layout-section">
    <xsl:variable name="item2" select="ec:Item(sc:fld('Product2',.)) "/>
    <div class="content4">
      <xsl:call-template name="column">
        <xsl:with-param name="idx" select="1"/>
        <xsl:with-param name="columns" select="4"/>
        <xsl:with-param name="item" select="ec:Item(sc:fld('Product1',.))"/>
      </xsl:call-template>
      <!-- col -->
      <xsl:call-template name="column">
        <xsl:with-param name="idx" select="2"/>
        <xsl:with-param name="columns" select="4"/>
        <xsl:with-param name="item" select="ec:Item(sc:fld('Product2',.))"/>
      </xsl:call-template>
      <!-- col -->
      <xsl:call-template name="column">
        <xsl:with-param name="idx" select="3"/>
        <xsl:with-param name="columns" select="4"/>
        <xsl:with-param name="item" select="ec:Item(sc:fld('Product3',.))"/>
      </xsl:call-template>
      <!-- col -->
      <xsl:call-template name="column">
        <xsl:with-param name="idx" select="4"/>
        <xsl:with-param name="columns" select="4"/>
        <xsl:with-param name="item" select="ec:Item(sc:fld('Product4',.))"/>
      </xsl:call-template>
      <!-- col -->

      <div class="clear"></div>
    </div>
  </xsl:template>



  
  
  <!--=========================================
      Column template
  =============================================-->
  <xsl:template name="column">
    <xsl:param name="idx"/>
    <xsl:param name="columns"/>
    <xsl:param name="item"/>

    <xsl:variable name="mw" xml:space="default">
      <xsl:choose>
        <xsl:when test="$columns=2">
          <xsl:value-of select="$TwoColWidth"/>
        </xsl:when>
        <xsl:when test="$columns=3">
          <xsl:value-of select="$ThreeColWidth"/>
        </xsl:when>
        <xsl:when test="$columns=4">
          <xsl:value-of select="$FourColWidth"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="mh" xml:space="default">
      <xsl:choose>
        <xsl:when test="$columns=2">
          <xsl:value-of select="$TwoColHeight"/>
        </xsl:when>
        <xsl:when test="$columns=3">
          <xsl:value-of select="$ThreeColHeight"/>
        </xsl:when>
        <xsl:when test="$columns=4">
          <xsl:value-of select="$FourColHeight"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>


    
    <div class="col">
      <div class="colMargin10">
        <div class="textContainer">
          <h2>
            <xsl:choose>
              <xsl:when test="sc:fld(concat('Title', $idx),.)">
                <xsl:variable name="field"  select="concat('Title', $idx)"/>
                <sc:link select="$item">
                  <sc:text field="$field"/>
                </sc:link>
              </xsl:when>
              <xsl:otherwise>
                <sc:link select="$item">
                  <sc:text field="Title" select="$item"/>
                </sc:link>
              </xsl:otherwise>
            </xsl:choose>
          </h2>
          <xsl:if test="$columns!=4">
            <p>
              <xsl:choose>
                <xsl:when test="sc:fld(concat('Description',$idx),.)">
                  <xsl:variable name="field"  select="concat('Description', $idx)"/>
                  <sc:text field="$field"/>
                </xsl:when>
                <xsl:otherwise>
                  <sc:text field="Short Description" select="$item"/>
                </xsl:otherwise>
              </xsl:choose>
            </p>
          </xsl:if>
        </div>
        <xsl:choose>
          <xsl:when test="sc:fld(concat('image',$idx),.,'src')">
            <xsl:variable name="field"  select="concat('Image', $idx)"/>
            <sc:link select="$item">
              <sc:image field="$field" w="{$mw}" h="{$mh}" as="true" bc="{$bgColor}"/>
            </sc:link>
          </xsl:when>
          <xsl:otherwise>
            <xsl:variable name="mediaProductItem" select="ec:GetFirstImageItem('images', $item)"/>
            <xsl:variable name="mediaProductUrl" select="concat(sc:GetMediaUrl($mediaProductItem), '?w=', $mw, '&amp;h=', $mh, '&amp;as=1&amp;bc=', $bgColor)"/>
            <sc:link select="$item">
              <img alt="{sc:fld('alt',$mediaProductItem)}" title="{sc:fld('alt',$mediaProductItem)}" src="{sc:SignMediaUrl($mediaProductUrl)}" />
            </sc:link>
          </xsl:otherwise>
        </xsl:choose>

        <xsl:if test="$item">
          <div class="colPriceContainer">
            <div class="priceMain">
              <xsl:value-of select="ec:GetSinglePrice($item)"/>
            </div>
            <div class="btnContainer">
              <input type="button" class="btnSmall" name="btn_add_{sc:fld('Product Code',$item)}" value="{ec:Translate('BUY')}">
                <xsl:attribute name="onclick">javascript:AddToShoppingCart('<xsl:value-of select="sc:fld('Product Code',$item)"/>', 1);return false;</xsl:attribute>
              </input>
            </div>
          </div>
        </xsl:if>
      </div>
      <div class="clear"></div>
    </div>

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


  <!--=========================================
      Column template - Flash Banner
  =============================================-->
  <xsl:template match="*[@template='flash banner']" mode="layout-section">
    <xsl:variable name="bannerItem" select="sc:item(sc:fld('flash',.,'mediaid'),.)"/>
    <xsl:variable name="identifier" select="translate(@name,' ','_')"/>

    <div class="content">
      <div class="colMargin8NoTopMargin">
          <xsl:if test="sc:fld('extension',$bannerItem)='swf'">
            <script type="text/javascript">
              swfobject.embedSWF('/<xsl:value-of select="sc:GetSignedMediaUrl($bannerItem)"/>', "<xsl:value-of select="$identifier"/>",
              "<xsl:value-of select="$OneColWidth"/>", "204", "8.0.0", '/jscript/expressInstall.swf', {}, {wmode : 'transparent'});
            </script>
            <div id="{$identifier}">
              <a href="http://www.adobe.com/go/getflashplayer">
                <img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif" alt="Get Adobe Flash player" />
              </a>
            </div>
          </xsl:if>         
      </div>
      <div class="clear"></div>
    </div>
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



  <!--=========================================
      Column template - Two column Rotator
  =============================================-->
  <xsl:template match="*[@template='two column rotator']" mode="layout-section">
    <xsl:variable name="timeOutFldName1" select="'Rotate Frequency1'"/>
    <xsl:variable name="timeOutFldName2" select="'Rotate Frequency2'"/>
    <xsl:variable name="colWidth" select="310"/>
    

    <xsl:variable name="identifier" select="translate(@name,' ','_')"/>

    <div class="content2">
      <div class="colRotator">
          <xsl:if test="ec:IsEditingMode()">
            <div class="editMode">
              <span>
                <xsl:value-of select="ec:FieldTitle(@id,$timeOutFldName1)"/>:
                <span class="editText">
                  <xsl:value-of select="sc:field($timeOutFldName1,.)" disable-output-escaping="yes"/>
                </span >
              </span>
            </div>
          </xsl:if>


        <xsl:call-template name="RotatorSpot" >
          <xsl:with-param name="ids" select="concat(sc:fld('Items1',.),'|')"/>
          <xsl:with-param name="idx" select="0"/>
          <xsl:with-param name="identifier" select="concat($identifier, '1')"/>
          <xsl:with-param name="width" select="$colWidth"/>
          <xsl:with-param name="timeout" select="sc:field($timeOutFldName1,.)"/>
        </xsl:call-template>
      </div>


      <div class="colRotator">
        <xsl:if test="ec:IsEditingMode()">
          <div class="editMode">
            <span>
              <xsl:value-of select="ec:FieldTitle(@id,$timeOutFldName2)"/>:
              <span class="editText">
                <xsl:value-of select="sc:field($timeOutFldName2,.)" disable-output-escaping="yes"/>
              </span >
            </span>
          </div>
        </xsl:if>


        <xsl:call-template name="RotatorSpot">
          <xsl:with-param name="ids" select="concat(sc:fld('Items2',.),'|')"/>
          <xsl:with-param name="idx" select="0"/>
          <xsl:with-param name="identifier" select="concat($identifier, '2')"/>
          <xsl:with-param name="width" select="$colWidth"/>
          <xsl:with-param name="timeout" select="sc:field($timeOutFldName2,.)"/>
        </xsl:call-template>

      </div>
      <div class="clear"></div>
    </div>
  </xsl:template>



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

  
  

  <!--=========================================
      Column template - Rotating Flash Banner
  =============================================-->
  <xsl:template match="*[@template='rotating flash banner']" mode="layout-section">
    <xsl:variable name="mediaItem" select="sc:item(sc:fld('flash', ., 'mediaid'),.)" />
    <xsl:variable name="identifier" select="translate(@name,' ','_')"/>
    
    <xsl:choose>
      <xsl:when test="not(ec:IsEditingMode())">
        <div class="content">
          <div class="colMargin8NoTopMargin">
            <!-- Get field values -->
            <xsl:variable name="src" select="sc:GetSignedMediaUrl($mediaItem)" />
            <xsl:variable name="width" select="sc:fld('Width',$mediaItem)" />
            <xsl:variable name="height" select="sc:fld('Height',$mediaItem)" />
            <xsl:variable name="mimetype" select="sc:fld('Mime Type',$mediaItem)" />
            
            <script type="text/javascript">
              swfobject.embedSWF('/<xsl:value-of select="$src"/>', "<xsl:value-of select="$identifier"/>",
              "<xsl:value-of select="$OneColWidth"/>", "<xsl:value-of select="$height"/>", "8.0.0", '/jscript/expressInstall.swf', {xmlPath : '<xsl:value-of select="sc:path(.)"/>'},
              {wmode : 'transparent', bgcolor : '#ffffff'});
            </script>
            
            <div id="{$identifier}">
              <sc:dot/>
                <a href="http://www.adobe.com/go/getflashplayer">
                  <img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif" alt="Get Adobe Flash player" />
                </a>
            </div>
          </div>
        </div>
      </xsl:when>
      <xsl:otherwise>

        <!-- Show the first image of the flash module -->
        <xsl:variable name="moduleItem" select="sc:item(sc:fld('FlashModule',.),.)" />

        <div class="content">
          <div class="colMargin8NoTopMargin">
            <div id="flashContainer" class="editMode">
              <div id="flashImage">
                <sc:image field="image1" />
              </div>
            </div>
          </div>
        </div>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>



  
</xsl:stylesheet>
