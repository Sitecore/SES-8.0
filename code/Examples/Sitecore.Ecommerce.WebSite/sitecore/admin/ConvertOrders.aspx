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
<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="Sitecore.Ecommerce" %>
<%@ Import Namespace="Sitecore.Ecommerce.Update" %>
<%@ Import Namespace="Sitecore.Ecommerce.Orders" %>
<%@ Import Namespace="Sitecore.Ecommerce.Data" %>
<%@ Import Namespace="Sitecore.Ecommerce.Visitor.OrderManagement.Transient" %>
<%@ Import Namespace="Sitecore.Pipelines" %>
<%@ Import Namespace="Sitecore.Sites" %>
<%@ Import Namespace="Sitecore.Configuration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Sitecore E-Commerce Services</title>
  <script language="C#" runat="server">
  /// <summary>
  ///   Runs the conversion
  /// </summary>
  /// <param name="sender">The sender.</param>
  /// <param name="e">The event args</param>
  protected void ConvertRun_Click(object sender, EventArgs e)
  {
    // Site name in the following using statement must be changed to the actual webshop site name.
    using (new SiteContextSwitcher(Factory.GetSite("example")))
    {
      ConvertToV2Orders orderConvertor = new ConvertToV2Orders
      {
        ShopContext = Sitecore.Ecommerce.Context.Entity.Resolve<ShopContext>(),
        OrderManager = Sitecore.Ecommerce.Context.Entity.Resolve<OrderManager<Sitecore.Ecommerce.DomainModel.Orders.Order>>(),
        OrderRepository = Sitecore.Ecommerce.Context.Entity.Resolve<Repository<Sitecore.Ecommerce.OrderManagement.Orders.Order>>(),
        TransientOrderConverter = Sitecore.Ecommerce.Context.Entity.Resolve<TransientOrderConverter>()
      };

      try
      {
        orderConvertor.Process(new PipelineArgs());

        HttpContext.Current.Response.Write(Sitecore.Globalization.Translate.Text(Texts.AllOrdersHaveBeenSuccessfullyConvertedToTheNewFormat));
      }
      catch (Exception exception)
      {
        HttpContext.Current.Response.Write(Sitecore.Globalization.Translate.Text(Texts.AnUnexpectedErrorOccurredDuringOrderConversion, exception.Message));
      }

      HttpContext.Current.Response.End();
    }
  }
  </script>
</head>
<body>
  <form id="form1" runat="server">
    <div>
      <asp:Button ID="ConvertRun" runat="server" Text="Convert orders to new format" OnClick="ConvertRun_Click" />
    </div>
  </form>
</body>
</html>
