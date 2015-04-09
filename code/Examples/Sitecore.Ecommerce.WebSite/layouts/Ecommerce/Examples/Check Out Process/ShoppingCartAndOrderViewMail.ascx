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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShoppingCartAndOrderViewMail.ascx.cs"
    Inherits="Sitecore.Ecommerce.layouts.Ecommerce.CheckOutProcess.ShoppingCartAndOrderViewMail" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>
<%@ Register Src="ShoppingCartViewMail.ascx" TagName="ShoppingCartView" TagPrefix="uc" %>
<table class="va-top" cellpadding="0" cellspacing="0">
    <tr>
        <td id="divRow1" runat="server">
            <table width="100%" cellpadding="0" cellspacing="2">
                <tr>
                    <td class="col" id="divSection1" visible="false" runat="server">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                <td style="background-color: #EAE9E5; padding: 4px 8px 4px 8px; font-weight: bold;"
                  height="20px">
                                    <asp:Literal runat="server" ID="litTitleSection1" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                <div id="ddSection1" runat="server">
                                    <div class="colMargin8">
                                        <asp:Repeater ID="repeaterOrderSection1" runat="server" OnItemDataBound="RepeaterOrderSection_ItemDataBound">
                                            <ItemTemplate>
                                                <div class="title">
                                                    <%# DataBinder.Eval(Container, "DataItem.Label")%>
                                                </div>
                                                <div class="value">
                                                    <%# DataBinder.Eval(Container, "DataItem.Value")%>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                <asp:PlaceHolder ID="phSection1" runat="server"></asp:PlaceHolder>
                    <td class="col" id="divSection2" visible="false" runat="server">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                <td style="background-color: #EAE9E5; padding: 4px 8px 4px 8px; font-weight: bold;"
                  height="20px">
                                    <asp:Literal runat="server" ID="litTitleSection2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="ddSection2" runat="server">
                                        <div class="colMargin8">
                                            <table cellpadding="0" cellspacing="0">
                                                <asp:Repeater ID="repeaterOrderSection2" runat="server" OnItemDataBound="RepeaterOrderSection_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>                                                    
                                                                <div class="title">
                                                                    <%# DataBinder.Eval(Container, "DataItem.Label")%>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="value">
                                                                    <%# DataBinder.Eval(Container, "DataItem.Value")%>
                                                                </div>
                                                            </td>
                                                        </tr>                                                        
                                                    </ItemTemplate>
                                                </asp:Repeater>    
                                            </table>                                                
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                <asp:PlaceHolder ID="phSection2" runat="server"></asp:PlaceHolder>
                    <td class="col" id="divSection3" visible="false" runat="server">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                <td style="background-color: #EAE9E5; padding: 4px 8px 4px 8px; font-weight: bold;"
                  height="20px">
                                    <asp:Literal runat="server" ID="litTitleSection3" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="ddSection3" runat="server">
                                        <div class="colMargin8">
                                            <table>
                                                <asp:Repeater ID="repeaterOrderSection3" runat="server" OnItemDataBound="RepeaterOrderSection_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <div class="title">
                                                                    <%# DataBinder.Eval(Container, "DataItem.Label")%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div class="value">
                                                                    <%# DataBinder.Eval(Container, "DataItem.Value")%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td id="divRow2" runat="server">
            <table width="100%" cellpadding="0" cellspacing="2">
                <tr>
                    <td class="col" id="divSection4" visible="false" runat="server">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                <td style="background-color: #EAE9E5; padding: 4px 8px 4px 8px; font-weight: bold;"
                  height="20px">
                                    <asp:Literal runat="server" ID="litTitleSection4" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="ddSection4" runat="server">
                                        <div class="colMargin8">
                                            <table cellpadding="0" cellspacing="0">
                                                <asp:Repeater ID="repeaterOrderSection4" runat="server" OnItemDataBound="RepeaterOrderSection_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <div class="title">
                                                                    <%# DataBinder.Eval(Container, "DataItem.Label")%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div class="value">
                                                                    <%# DataBinder.Eval(Container, "DataItem.Value")%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                <asp:PlaceHolder ID="phSection4" runat="server"></asp:PlaceHolder>
                    <td class="col" id="divSection5" visible="false" runat="server">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                <td style="background-color: #EAE9E5; padding: 4px 8px 4px 8px; font-weight: bold;"
                  height="20px">
                                    <asp:Literal runat="server" ID="litTitleSetion5" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="ddSection5" runat="server">
                                        <div class="colMargin8">
                                            <table cellpadding="0" cellspacing="0">
                                                <asp:Repeater ID="repeaterOrderSection5" runat="server" OnItemDataBound="RepeaterOrderSection_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <div class="title">
                                                                    <%# DataBinder.Eval(Container, "DataItem.Label")%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>                                                                    
                                                                <div class="value">
                                                                    <%# DataBinder.Eval(Container, "DataItem.Value")%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                <asp:PlaceHolder ID="phSection5" runat="server"></asp:PlaceHolder>
                    <td class="col" id="divSection6" visible="false" runat="server">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                <td style="background-color: #EAE9E5; padding: 4px 8px 4px 8px; font-weight: bold;"
                  height="20px">
                                    <asp:Literal runat="server" ID="litTitleSection6" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="ddSection6" runat="server">
                                        <div class="colMargin8">
                                            <table cellpadding="0" cellspacing="0">
                                                <asp:Repeater ID="repeaterOrderSection6" runat="server" OnItemDataBound="RepeaterOrderSection_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <div class="title">
                                                                    <%# DataBinder.Eval(Container, "DataItem.Label")%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>   
                                                                <div class="value">
                                                                    <%# DataBinder.Eval(Container, "DataItem.Value")%>
                                                                </div>
                                                            </td>
                                                        </tr>                                                                    
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td style="padding: 1px 3px 0 3px">
            <table style="border:solid 1px silver;" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="background-color: #EAE9E5; padding: 4px 8px 4px 8px; font-weight:bold">
                        Products
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc:ShoppingCartView ID="ProductsListView" runat="server" />
                    </td>       
                </tr>
            </table>
        </td>
    </tr>
</table>
