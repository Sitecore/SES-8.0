<%--=======================================================================================
Copyright 2015 Sitecore Corporation A/S
Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
except in compliance with the License. You may obtain a copy of the License at
    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the 
License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
either express or implied. See the License for the specific language governing permissions 
and limitations under the License.
======================================================================================--%>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageMainMail.aspx.cs" Inherits="Sitecore.Ecommerce.layouts.Ecommerce.PageMainMail"
    Debug="true" %>
<%@ Import Namespace="Sitecore.Ecommerce.Classes"%>

<%@ OutputCache Location="None" VaryByParam="none" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head" runat="server">
    <title>
        <%=NicamHelper.GetTitle()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="CODE_LANGUAGE" content="C#" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <meta name="scID" id="scID" content="" runat="server" />
</head>
<body id="index">
    
<style type="text/css">
/*
	--------------------------------------------
	Global
	--------------------------------------------
*/
	html, body{ width: 100%; }

	img               { margin: 0; }
	p   	          { voice-family: "\"}\""; voice-family:inherit; }
	input	          { voice-family: "\"}\""; voice-family:inherit; }
	select            { voice-family: "\"}\""; voice-family:inherit; }
	option            { voice-family: "\"}\""; voice-family:inherit; }
	textarea          { voice-family: "\"}\""; voice-family:inherit; }
	th	              { voice-family: "\"}\""; voice-family:inherit; }
	tb	              { voice-family: "\"}\""; voice-family:inherit; }
	div               { voice-family: "\"}\""; voice-family:inherit; }
	h1,h2,h3,h4,h5,h6 { voice-family: "\"}\""; voice-family:inherit; }

	p, div, span, label, ul, ol, li, dd, dt, dl{ margin: 0; padding: 0;}


	/*	Headers 
		-------------------------------------------- */

	h1,h2,h3,h4,h5,h6,h1 a,h2 a,h3 a,h4 a,h5 a,h6 a{
		font-weight: 100;
		margin: 0;
		padding: 0; }

	h1 {
		font-size: 30px;
		font-weight: bold;
		margin-bottom: 15px;
		width: 555px;}

	h2 {
		font-size: 20px;
		font-weight: bold;}

	h3 {
		font-size: 14px;
		font-weight: bold; }
		
		
	/*	Links
		--------------------------------------------*/	
	a, a:focus, a:hover, a:active, a:visited{ outline: none; }
	a{ 
		color: #cc0606;
		text-decoration: none; }
		
	a:hover{}
	a img{ border: none; }	/* Disable image borders on linked images */

/*
	--------------------------------------------
	Main Page
	--------------------------------------------
*/

	body {
		color: #444444;
		font-family: Arial, Helvetica, sans-serif;
		font-size: 12px;
		margin: 0;
		padding:0;
		width: 520px;
		}

	#page_container{
		margin: 0;
		padding: 0;	}

	#page_body {
		clear: both;
		padding: 8px;
		margin: 0;
		background: #fff; }
		
/*
	--------------------------------------------
	Body Specific Styles
	--------------------------------------------
*/

	/* Prices
		--------------------------------------------*/
	.priceMain,
	.priceTotal,
	.priceVat,
	.priceSale,
	.priceCurrent,
	.priceCurrentTotal,
	.priceOriginal{ white-space: nowrap;}




	/*	Middle Column (pb_mc)
		--------------------------------------------*/

	#pb_home_container{}
	#pb_shopping_cart_container{ }
	#pb_product_container{}
	#pb_product_list_container{}



	/* Text container */
	.pb_mc .colMargin10 .textContainer { 
		height: 80px;
		overflow: auto; } /* Container to secure fixed height for header and ingress */
		
		.pb_mc .col  .textContainer h2,
		.pb_mc .col1 .textContainer h2, 
		.pb_mc .col2 .textContainer h2, 
		.pb_mc .col3 .textContainer h2, 
		.pb_mc .col4 .textContainer h2,
		.pb_mc .col  .textContainer h2 a,
		.pb_mc .col1 .textContainer h2 a, 
		.pb_mc .col2 .textContainer h2 a, 
		.pb_mc .col3 .textContainer h2 a, 
		.pb_mc .col4 .textContainer h2 a{ 
			color: #000; 
			font-weight: 700; }

		.pb_mc .col4 .textContainer h2 a,
		.pb_mc .content4 .textContainer h2 a{ font-size: 14px; }

		
		.pb_mc .col  .textContainer p,
		.pb_mc .col1 .textContainer p, 
		.pb_mc .col2 .textContainer p, 
		.pb_mc .col3 .textContainer p, 
		.pb_mc .col4 .textContainer p  { color: #000; }
		
		.pb_mc .col  .textContainer img,
		.pb_mc .col1 .textContainer img, 
		.pb_mc .col2 .textContainer img, 
		.pb_mc .col3 .textContainer img, 
		.pb_mc .col4 .textContainer img { 
			display: block;
			border: 0;
			margin: 0;}
		
		
		


	/*	Lists 
		--------------------------------------------*/
	.pb_mc ul.ulProductList{ 
		display: inline;
		float: left;
		margin: 0 8px 8px 8px;
		padding: 0;
		list-style: none; }

	.content .pb_mc ul.ulProductList{ margin: 0px;}
	.content1 .pb_mc ul.ulProductList{ margin: 0px;}
	.content2 .pb_mc ul.ulProductList{ margin: 0px;}
	.content3 .pb_mc ul.ulProductList{ margin: 0px;}
	.content4 .pb_mc ul.ulProductList{ margin: 0px;}
	.colMargin8 .pb_mc ul.ulProductList{ margin: 0px;}
	.colMargin10 .pb_mc ul.ulProductList{ margin: 0px;}
	
		
	
	.colProductNumber{overflow: hidden;}
	.colImage{overflow: hidden;}
	.colText{overflow: hidden;}
	.colImageText{overflow: hidden;}
	.colCount{overflow: hidden;} /* Amount */
	.colPrice{overflow: hidden;}
	.colPriceCurrent{overflow: hidden;}
	.colCtrlButtons{overflow: hidden;}


	div.bottomNavigation{
		display: inline;
		float: left;
		width: 100%;
		margin: 8px 0;
		padding:0;}

			div.bottomNavigation .bottomNavigationFullWidth{ width: 100%;}
			
			div.bottomNavigation .bottomNavigationLeft{ 
				float: left;
				text-align:left;
				width: 65%;
				padding-left: 8px;}
				
			div.bottomNavigation .bottomNavigationRight{ 
				float: right;
				text-align: right;
				width: 30%;
				padding-right: 8px;}
				
				div.bottomNavigation input{ padding-right: 8px;}

			

		
	/* Product list */
	.ulProductList li{
		display: inline;
		float: left;
		clear: both;
		/*width: 612px;*/
		padding: 16px 8px;
		margin: 0;
		border-bottom: 1px solid #e3e3e3;} 
		
	.ulProductList li.ulHeader{ padding: 0 8px; }
	/*.ulProductList li.ulHeader div{ text-align: left;}*/
		
		
	/* All columns */		
	.ulProductList .colProductNumber{
		width: 108px;}
	
	.ulProductList .colImage { 
		width: 108px; }

	.ulProductList .colText{ 
		width: 342px; }

	.ulProductList .colImageText{ 
		width: 450px; }
		
		.colText h2 a,
		.colImageText h2 a { 
			font-size: 14px;
			font-weight: 700;}
			
		.colText .inStock,
		.colImageText .inStock{
			color: #c2c2c2;}

	.ulProductList .colCount{ 
		width: 50px;
		text-align: center;}
		
		.ulProductList .colCount input{ 
			text-align: center;
			width: 30px;
			border: 1px solid #dedede;}

	.ulProductList .colPriceCurrent{ 
		float: left;
		width: 0px;
		text-align: right;}
		
		.ulProductList .priceMain{
			font-weight: 700;
			font-size: 16px;}
			
		.ulProductList .priceOriginal{ 
			color: #dedede;
			text-decoration: line-through;
			font-weight: 100;}

	.ulProductList .colPriceCurrentTotal{
		text-align: right;}
		 	
	.ulProductList .colControlButtons{ 
		width: 60px;
		text-align: right;}

	
	#pb_product_list_container .rightAlignedContainer{ 
		display: inline;
		width: 130px;
		margin-top: -40px;} /* Container with price and buy button */
		
	#pb_product_list_container .ulHeader .rightAlignedContainer{ 
		text-align: left; 
		margin: 0; }
		
				.rightAlignedContainer .colPrice{ 
					width: 70px;
					text-align: right;
					padding-right: 8px;}

				.rightAlignedContainer .colControlButtons{ 
					width: 45px;
					text-align: right;}
					
				.rightAlignedContainer .colControlButtons{ float: right;}
				

	/* Product */
	#pb_product_container .pictureContainer{ 
		width: 250px;
		text-align: center;
		margin-right: 32px;}
		
		#pb_product_container .pictureContainer img{}
		
		#pb_product_container .pictureContainer .tbnContainer{ margin: 8px auto;}
			
			#pb_product_container .pictureContainer .tbnContainer img{
				border: 1px solid #dedede;
				margin: 1px 0px 0px 1px;}

	/* Description container */
	#pb_product_container .descriptionContainer{
		width: 340px;
		overflow: hidden;}

		#pb_product_container h1{ 
			margin: 0;
			color: #cc0606; }
			

		#pb_product_container h2{ 
			font-size: 12px;
			font-weight: 700;}		
		
		
		/* Price container */
		#pb_product_container .priceContainer{
			display: inline;
			width: 340px;
			margin: 16px 0;
			padding: 8px 0;
			border-top: 1px solid #dedede;
			border-bottom: 1px solid #dedede; }

			#pb_product_container .priceMain{ 
				float: left;
				width: 150px;
				margin-right: 16px;
				font-size: 24px;
				font-weight: 700;}
				
			#pb_product_container .colCount{ 
				padding-top: 2px;
				width: 60px;
				text-align: right; }
				
				#pb_product_container .colCount input{
					width: 30px;
					padding: 4px;
					border: 1px solid #dedede;
					text-align: center; }

			#pb_product_container .btnContainer{
				text-align: right;
				padding-right: 16px;}
					
		
		/* Details container */
		#pb_product_container .detailsContainer{ 
			margin: 16px 0;}
		
		#pb_product_container .title{ 
			width: 120px;}
		
		#pb_product_container .value{
			text-align: left; }
				
				
	/* check out */
	#pb_check_out_container .teaser{ margin: 0 10px;}

	#pb_check_out_container dl,
	#pb_check_out_container dt,
	#pb_check_out_container dd{
		display: inline;
		float: left;
		width: 100%;}



		#pb_check_out_container dt{
			padding: 4px 8px 8px 8px;
			/*background: url(../images/ecommerce/bg_pager.gif) 0 bottom repeat-x;*/
			background-color: #EAE9E5;
			font-weight: 700; }
			
		#pb_check_out_container dd{ }

		#pb_check_out_container input{ clear: both;}

			#pb_check_out_container .title{ 
				display: inline;
				font-weight: 700;
				white-space: nowrap;
				margin: 2px 0px; }
				
			#pb_check_out_container .value{ 
				display: inline;			
				font-weight: 100;
				margin: 2px 0px;
				/*white-space: nowrap; */}
			
				
            #pb_check_out_container .content .col,
			#pb_check_out_container .content2 .col,
			#pb_check_out_container .content3 .col
			{
				border:solid 1px silver;
				overflow: hidden; }

			

			/* Just in case */
			#pb_check_out_container .noLabels .title{ display: none; }

			
			/* 1 column */
			#pb_check_out_container .content .noLabels .value{ 
				float: left;
				clear: both;
				width: 100%;
				font-weight: 700;}
				
			#pb_check_out_container .content .labels .title{
				float: left;
				width: 150px; }
				
			#pb_check_out_container .content .labels .value{
				float: left;
				width: 455px;}

			
			
			/* 2 column */
			#pb_check_out_container .content2 .title{
				float: left;
				width: 100px; }
				
			#pb_check_out_container .content2 .col .value{
				float: left;
				width: 190px;}	
				
					
			
			/* 3 column */
			#pb_check_out_container .content3 .col .title,
			#pb_check_out_container .col3 .title{ 
				float: left;
				clear: left;
				width: 80px;}
				
			#pb_check_out_container .content3 .col .value,
			#pb_check_out_container .col3 .value{ 
				float: left;
				width: 100px; }


	#pb_check_out_container ul{ 
		display: inline;
		width: 100%;
		float: left;}
			
		#pb_check_out_container li.shippingOption{
			display: inline;
			float: left;
			width: 100%;
			margin-bottom: 8px;}
			
			li.shippingOption .colRadio{
				float: left;
				width: 25px;}

			li.shippingOption .colImage {
				float: left;
				width: 60px;}
			
			li.shippingOption .colText{
				float: left;
				width: 152px;}
				
			li.shippingOption .colImageText{
				float: left;
				width: 212px;}
				
			li.shippingOption .colImageTextPrice{
				float: left;
				width: 267px;}

				li.shippingOption .colText p,
				li.shippingOption .colImageText p,
				li.shippingOption .colImageTextPrice p{ line-height: 1.2em;}
			
			li.shippingOption .colPrice{
				float: right;
				clear: right;
				width: 52px;
				font-weight: 700;}


			/* 1 col */
			.content li.shippingOption .colText {			width: 470px; }
			.content li.shippingOption .colImageText{		width: 530px; }
			.content li.shippingOption .colImageTextPrice{	width: 585px; }


			/* 3 col */
			.content3 li.shippingOption .colText {			width: 70px;  }
			.content3 li.shippingOption .colImageText{		width: 126px; }
			.content3 li.shippingOption .colImageTextPrice{	width: 160px; }
			.content3 li.shippingOption .colPrice{			width: 30px;  }
		
		#pb_check_out_container li.trackingOption{
			display: inline;
			float: left;
			width: 100%;
			margin-bottom: 4px;	}
			
			li.trackingOption .colRadio{
				float: left;
				width: 25px;}
				
			li.trackingOption .colText{
				float: left;
				width: 45px;}
				
				
			li.trackingOption .colTextInput{ 
				float: left;
				width: 100px;
				text-align: left;}
				
				.content li.trackingOption .colTextInput{ width: 200px; }
				.content2 li.trackingOption .colTextInput{ width: 180px; }
				
				.colTextInput input{ 
					width: 80%;
					padding: 4px;
					margin-right: 4px;
					text-align: left;
					border: 1px solid #e3e3e3;}	



/*
	--------------------------------------------
	Eye Droppers
	--------------------------------------------
*/

	.textsmall{	font-size: 11px; }
	.textmedium{ font-size: 12px; }

	.errorMessageRed{ color: red; }
	.errorMessageWhite{	color: white; }

	div.center{ text-align: center; }
	div.left{ text-align: left; }
	div.right{ text-align: right; }

	.twoColoredSpacerLeft{	background: url(../images/ecommerce/two_colored_spacer.gif) left 0px repeat-y;}
	.twoColoredSpacerRight{	background: url(../images/ecommerce/two_colored_spacer.gif) right 0px repeat-y;}

	div.colMargin8{ display: block; margin: 8px;}
	div.colMargin8NoTopMargin{ display: block; float: none; margin: 0 8px 8px 8px;}

	div.colMargin10{ display: block; float: none; margin: 10px;}


/*
	--------------------------------------------
	Clear Class
	--------------------------------------------
*/

	div.clear {
		clear: both;
		height: 1px;
		line-height: 1px;
		font-size: 1px;
		overflow: hidden; }
		

	.clearfix:after { 
	  content: "."; 
	  display: block; 
	  clear: both; 
	  visibility: hidden; 
	  line-height: 0; 
	  height: 0;}
	  
	.clearfix {	display: inline-block;	}
	html[xmlns] .clearfix { display: block; }
	* html .clearfix { height: 1%; }


/*
    -----------------------------
    Tables
    -----------------------------
*/
    table.va-top td{    vertical-align: top;}
    table.va-bottom td{ vertical-align: bottom;}
    table.va-middle td{ vertical-align: middle;}

    table td.va-top{    vertical-align: top;}
    table td.va-bottom{ vertical-align: bottom;}
    table td.va-middle{ vertical-align: middle;}

    table.ha-right td { text-align: right; }
    table.ha-left td{ text-align: left; }
    table.ha-center td{ text-align:center; }

    table td.ha-right{ text-align:right;}
    table td.ha-left{ text-align:left;}
    table td.ha-center{ text-align:center;}

   
    .ulProductList td
    {
    	padding: 2px;
    }
/*
	--------------------------------------------
	IE Image Fix
	--------------------------------------------
*/

	img.fix{ 
		display: block; 
		margin: 0;}
</style>


    <form id="mainform" runat="server">
    <div id="body_inner">
        <div id="pageContainer" runat="server">
            <!-- *************************************
                 **  Header
                 ************************************* -->
            <%----------------------------------------
                --               Content
                ----------------------------------------%>
            <sc:Placeholder runat="server" Key="phCenter" ID="placeholder2" />
            <%----------------------------------------
                --               Footer
                ----------------------------------------%>
        </div>
    </div>
    </form>
</body>
</html>
