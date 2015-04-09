// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultPaymentProviderFactory.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the default payment provider factory class.
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

namespace Sitecore.Ecommerce.Payments
{
  using Diagnostics;
  using DomainModel.Data;
  using DomainModel.Payments;

  /// <summary>
  /// Instantiates and configures payment providers using payment provider code.
  /// </summary>
  public class DefaultPaymentProviderFactory : PaymentProviderFactory
  {
    /// <summary>
    /// The entity provider.
    /// </summary>
    [NotNull]
    private readonly IEntityProvider<DomainModel.Payments.PaymentSystem> entityProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultPaymentProviderFactory" /> class.
    /// </summary>
    /// <param name="entityProvider">The entity provider.</param>
    public DefaultPaymentProviderFactory(IEntityProvider<DomainModel.Payments.PaymentSystem> entityProvider)
    {
      Assert.ArgumentNotNull(entityProvider, "entityProvider");

      this.entityProvider = entityProvider;
    }

    /// <summary>
    /// Gets the payment provider by code.
    /// </summary>
    /// <param name="code">The payment provider code.</param>
    /// <returns>The payment provider.</returns>
    [NotNull]
    public override PaymentProvider GetProvider([NotNull] string code)
    {
      Assert.ArgumentNotNull(code, "code");

      PaymentProvider paymentProvider = Context.Entity.Resolve<PaymentProvider>(code);
      paymentProvider.PaymentOption = this.entityProvider.Get(code);

      return paymentProvider;
    }
  }
}