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
<%@ Import Namespace="Sitecore.Ecommerce.Utils" %>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="Sitecore.Ecommerce.layouts.Ecommerce.Search" Codebehind="Search.ascx.cs" %>

<script language="JavaScript" type="text/javascript">
    function DoClick(nameButton) {
        document.getElementById(nameButton).value = nameButton;
        document.forms[0].submit();
        return false;
    }
</script>

<div id="ph_search">
    <table id="tbl_search" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td id="td_search_frase">
                <input runat="server" id="headerSearch" name="headerSearch" type="text" class="searchFrase" />
            </td>
            <td id="td_search_btn">
                <input type="submit" value="Search" class="searchBtn" onclick="javascript: DoClick('SearchButton')" />
            </td>
        </tr>
    </table>
</div>
<input id="SearchButton" name="SearchButton" type="hidden" />