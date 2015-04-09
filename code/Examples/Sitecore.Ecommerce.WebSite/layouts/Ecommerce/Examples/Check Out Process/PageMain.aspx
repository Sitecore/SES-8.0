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

<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="PageMain.aspx.cs" Inherits="Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess.PageMain"
    Debug="true" %>
<%@ Import Namespace="Sitecore.Globalization"%>
<%@ Import Namespace="Sitecore.Ecommerce.Utils"%>
<%@ Import Namespace="Sitecore.Ecommerce.Classes"%>

<%@ OutputCache Location="None" VaryByParam="none" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head" runat="server">
    <title>
        <%=NicamHelper.GetTitle()%></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="CODE_LANGUAGE" content="C#" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <meta name="scID" id="scID" content="" runat="server" />
    <link href="/styles/ecommerce_common.css" rel="stylesheet" />
    <link href="/styles/jquery.lightbox-0.5.css" rel="stylesheet" type="text/css" />
        
    <script src="/jscript/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="/jscript/jquery.lightbox-0.5.min.js" type="text/javascript"></script>
    <script src="/jscript/swfobject.js" type="text/javascript"></script>
    <script src="/jscript/Ecommerce/ecommerce.js" type="text/javascript" language="javascript"></script>
    <script src="/jscript/Ecommerce/functions.js" type="text/javascript" language="javascript"></script>
    <script src="/layouts/ecommerce/Examples/Check Out Process/jscripts/CheckOut.js" type="text/javascript" language="javascript"></script>

</head>
<body id="index">
    <form id="mainform" runat="server">
    <div id="body_inner">
        <div id="pageContainer" runat="server">
            <!-- *************************************
                 **  Header
                 ************************************* -->
            <div id="page_header">
                <sc:Sublayout runat="server" RenderingID="" path="/layouts/Ecommerce/Examples/Check Out Process/LoginPanel.ascx" ID="LoginPanel" />
                <!-- login and language selection -->
                                
                <div class="clear">
                </div>
            </div>
            <%----------------------------------------
                --               Content
                ----------------------------------------%>
            <sc:Placeholder runat="server" Key="phContent" ID="placeholder2" />            
        </div>
    </div>
    </form>
</body>
</html>
