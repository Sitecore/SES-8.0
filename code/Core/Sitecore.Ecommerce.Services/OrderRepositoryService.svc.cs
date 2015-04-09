// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderRepositoryService.svc.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderRepositoryService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
// Copyright 2015 Sitecore Corporation A/S
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
// except in compliance with the License. You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the 
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
// either express or implied. See the License for the specific language governing permissions 
// and limitations under the License.
// -------------------------------------------------------------------------------------------

namespace Sitecore.Ecommerce.Services
{
  using System.ServiceModel;
  using System.ServiceModel.Activation;
  using Diagnostics;
  using Ecommerce.Utils;
  using Newtonsoft.Json;
  using Newtonsoft.Json.Serialization;
  using OrderManagement;
  using OrderManagement.Orders;
  using Sitecore.Pipelines;
  using Sites;

  /// <summary>
  ///  Defines the OrderRepositoryService type.
  /// </summary>
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  public class OrderRepositoryService : IOrderRepositoryService
  {
    /// <summary>
    /// The predefined ExpressionSerializer.
    /// </summary>
    private ExpressionSerializer expressionSerializer = new ExpressionSerializer();

    /// <summary>
    /// Gets or sets the ExpressionSerializer.
    /// </summary>
    /// <value>The ExpressionSerializer.</value>
    [NotNull]
    public ExpressionSerializer ExpressionSerializer
    {
      get
      {
        return this.expressionSerializer;
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.expressionSerializer = value;
      }
    }

    /// <summary>
    /// Creates the specified order.
    /// </summary>
    /// <param name="orderSerialization">The order serialization.</param>
    /// <param name="args">The arguments.</param>
    public void Create(string orderSerialization, ServiceClientArgs args)
    {
      SiteContext site = Utils.GetExistingSiteContext(args);
      using (new SiteContextSwitcher(site))
      {
        VisitorOrderManager orderRepository = Context.Entity.Resolve<VisitorOrderManager>();
        var settings = new JsonSerializerSettings
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
          MissingMemberHandling = MissingMemberHandling.Ignore,
          PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };
        settings.Error += this.DeserializationErrorHandler;

        Order order = JsonConvert.DeserializeObject<Order>(orderSerialization, settings);

        orderRepository.Create(order);

        PipelineArgs pipelineArgs = new PipelineArgs();
        pipelineArgs.CustomData.Add("orderNumber", order.OrderId);

        CorePipeline.Run("orderCreated", pipelineArgs);
      }
    }

    /// <summary>
    /// Gets all orders.
    /// </summary>
    /// <param name="filterExpression">The ExpressionSerializer expression.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>
    /// The all orders.
    /// </returns>
    public string GetAll(string filterExpression, ServiceClientArgs args)
    {
      SiteContext site = Utils.GetExistingSiteContext(args);
      using (new SiteContextSwitcher(site))
      {
        VisitorOrderManager orderRepository = Context.Entity.Resolve<VisitorOrderManager>();
        Utils.SetCustomerId(args, orderRepository);

        var tempResult = orderRepository.GetAll();
        object rawResult = this.ExpressionSerializer.ApplyExpressionAsFilter(tempResult, filterExpression);
        var settings = new JsonSerializerSettings
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
          MissingMemberHandling = MissingMemberHandling.Ignore,
          PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };
        return JsonConvert.SerializeObject(rawResult, Formatting.None, settings);
      }
    }

    /// <summary>
    /// Deserializations the error handler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="Newtonsoft.Json.Serialization.ErrorEventArgs"/> instance containing the event data.</param>
    private void DeserializationErrorHandler(object sender, ErrorEventArgs e)
    {
      e.ErrorContext.Handled = true;
    }
  }
}
