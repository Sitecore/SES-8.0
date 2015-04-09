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
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SampleLayoutPrint.aspx.cs" Inherits="Sitecore.Ecommerce.Examples.SampleLayoutPrint" Debug="true" %>

<%@ Import Namespace="Sitecore.Ecommerce.Classes" %>
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
  <link href="/styles/ecommerce_common.css" rel="stylesheet" />
  <style type="text/css">
    body {
      background: #fff;
    }
    #body_inner {
      width: auto;
    }
    .printhide {
      display: none;
    }
  </style>
  <link href="~/layouts/ecommerce/Examples/Check Out Process/css/print.css" rel="stylesheet" />
</head>
<body id="index">
  <form id="mainform" runat="server">
  <div id="body_inner">
    <div id="pageContainer" runat="server">
      <div id="page_body" class="clearfix">
        <div class="pb_mc">
          <sc:Placeholder runat="server" Key="phContent" ID="placeholder2" />
        </div>
      </div>
    </div>
  </div>
  </form>
</body>
</html>
