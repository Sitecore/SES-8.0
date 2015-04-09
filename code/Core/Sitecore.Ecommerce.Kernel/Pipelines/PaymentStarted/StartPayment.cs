// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartPayment.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The start payment processor.
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

namespace Sitecore.Ecommerce.Pipelines.PaymentStarted
{
  using System.Text;
  using Diagnostics;
  using DomainModel.Carts;
  using DomainModel.Payments;
  using Sitecore.Ecommerce.Payments;
  using Sitecore.Pipelines;

  /// <summary>
  /// The start payment processor.
  /// </summary>
  public class StartPayment
  {
    /// <summary>
    /// Processes the specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void Process(PipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      ShoppingCart shoppingCart = Context.Entity.GetInstance<ShoppingCart>();
      PaymentProvider paymentProvider = Context.Entity.Resolve<PaymentProvider>(shoppingCart.PaymentSystem.Code);

      PaymentUrlResolver paymentUrlResolver = new PaymentUrlResolver();
      PaymentArgs paymentArgs = new PaymentArgs
      {
        ShoppingCart = shoppingCart,
        PaymentUrls = paymentUrlResolver.Resolve(),
      };

      StringBuilder description = new StringBuilder();
      foreach (ShoppingCartLine shoppingCartLine in shoppingCart.ShoppingCartLines)
      {
        description.Append(shoppingCartLine.Product.Title);
        description.Append(", ");
      }

      paymentArgs.Description = description.ToString().Trim().TrimEnd(',');

      paymentProvider.Invoke(shoppingCart.PaymentSystem, paymentArgs);

      args.AbortPipeline();
    }
  }
}