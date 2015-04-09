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
<%@ Import Namespace="Sitecore.Ecommerce.Examples" %>
<%@ Import Namespace="Sitecore.Ecommerce.Utils" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderHistory.ascx.cs"
  Inherits="Sitecore.Ecommerce.layouts.Ecommerce.OrderHistory" %>
<%@ Import Namespace="Sitecore.Globalization" %>
<%@ Import Namespace="Sitecore.Ecommerce.DomainModel.Configurations" %>
<%@ Import Namespace="Sitecore.Ecommerce.OrderManagement.Orders" %>
<asp:DataGrid ID="orderList" runat="server" AutoGenerateColumns="False" GridLines="None"
  ShowFooter="True" Width="100%">
  <Columns>
    <asp:TemplateColumn>
      <HeaderStyle CssClass="line-bottom" Font-Bold="True" HorizontalAlign="Center" />
      <ItemStyle CssClass="line-bottom" HorizontalAlign="Center" />
      <HeaderTemplate>
        <%= Translate.Text(Texts.OrderNumber)%>
      </HeaderTemplate>
      <ItemTemplate>
        <%# Eval("OrderId") %>
      </ItemTemplate>
    </asp:TemplateColumn>
    <asp:TemplateColumn>
      <HeaderStyle CssClass="line-bottom" Font-Bold="True" HorizontalAlign="Center" />
      <ItemStyle CssClass="line-bottom" HorizontalAlign="Center" />
      <HeaderTemplate>
        <%= Translate.Text(Texts.OrderDate)%>
      </HeaderTemplate>
      <ItemTemplate>
        <%# Eval("IssueDate", "{0:dd.MM.yyyy}") %>
      </ItemTemplate>
    </asp:TemplateColumn>
    <asp:TemplateColumn>
      <HeaderStyle CssClass="line-bottom" Font-Bold="True" HorizontalAlign="Center" />
      <ItemStyle CssClass="line-bottom" HorizontalAlign="Center" />
      <HeaderTemplate>
        <%= Translate.Text(Texts.Items)%>
      </HeaderTemplate>
      <ItemTemplate>
        <%# GetQuantity(Container.DataItem)%>
      </ItemTemplate>
    </asp:TemplateColumn>
    <asp:TemplateColumn>
      <HeaderStyle CssClass="line-bottom" Font-Bold="True" HorizontalAlign="Right" />
      <ItemStyle CssClass="line-bottom" HorizontalAlign="Right" />
      <HeaderTemplate>
        <%= Translate.Text(Texts.TotalPrice)+":"%>
      </HeaderTemplate>
      <ItemTemplate>
        <%# Eval("AnticipatedMonetaryTotal.TaxInclusiveAmount.Value", Sitecore.Ecommerce.Context.Entity.GetConfiguration<GeneralSettings>().PriceFormatString)%>
      </ItemTemplate>
    </asp:TemplateColumn>
    <asp:TemplateColumn>
      <HeaderStyle CssClass="line-bottom" Font-Bold="True" HorizontalAlign="Center" />
      <ItemStyle CssClass="line-bottom" HorizontalAlign="Right" />
      <HeaderTemplate>
      </HeaderTemplate>
      <ItemTemplate>
        <a href='<%= ItemUtil.GetNavigationLinkPath("View Details") %>?orderid=<%# Eval("OrderId") %>'>
          <%= Translate.Text(Texts.Details)%>
        </a>
      </ItemTemplate>
    </asp:TemplateColumn>
    <asp:TemplateColumn>
      <HeaderStyle CssClass="line-bottom" Font-Bold="True" HorizontalAlign="Center" />
      <ItemStyle CssClass="line-bottom" HorizontalAlign="Right" />
      <HeaderTemplate>
      </HeaderTemplate>
      <ItemTemplate>
        <asp:LinkButton ID="CancelOrderLink" runat="server" CommandArgument='<%# ((Order)Container.DataItem).OrderId%>' 
          OnClick="CancelOrderLink_Click" OnDataBinding="CancelOrderLink_DataBind" />
      </ItemTemplate>
    </asp:TemplateColumn>
  </Columns>
</asp:DataGrid>
