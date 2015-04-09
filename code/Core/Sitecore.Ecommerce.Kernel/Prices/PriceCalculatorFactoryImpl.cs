// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PriceCalculatorFactoryImpl.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the price calculator factory impl class.
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

namespace Sitecore.Ecommerce.Prices
{
  using Diagnostics;
  using DomainModel.Prices;
  using Security;
  using Sitecore.Security.Accounts;

  /// <summary>
  /// Defines the price calculator factory impl class.
  /// </summary>
  public class PriceCalculatorFactoryImpl : PriceCalculatorFactory
  {
    /// <summary>
    /// The member price.
    /// </summary>
    private const string MemberPrice = "MemberPrice";

    /// <summary>
    /// The normal price.
    /// </summary>
    private const string NormalPrice = "NormalPrice";

    /// <summary>
    /// The customer membership.
    /// </summary>
    private readonly CustomerMembership membership;

    /// <summary>
    /// Defines totals factory.
    /// </summary>
    private readonly TotalsFactory totalsFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="PriceCalculatorFactoryImpl" /> class.
    /// </summary>
    /// <param name="membership">The membership.</param>
    /// <param name="totalsFactory">The totals factory.</param>
    public PriceCalculatorFactoryImpl([NotNull]CustomerMembership membership, [NotNull] TotalsFactory totalsFactory)
    {
      Assert.ArgumentNotNull(membership, "membership");
      Assert.ArgumentNotNull(totalsFactory, "totalsFactory");

      this.membership = membership;
      this.totalsFactory = totalsFactory;
    }

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    /// <value>The user.</value>
    [CanBeNull]
    public User User { get; set; }

    /// <summary>
    /// Gets or sets the membership.
    /// </summary>
    /// <value>
    /// The membership.
    /// </value>
    [NotNull]
    protected CustomerMembership Membership
    {
      get { return this.membership; }
    }

    /// <summary>
    /// Gets the totals factory.
    /// </summary>
    [NotNull]
    protected TotalsFactory TotalsFactory
    {
      get { return this.totalsFactory; }
    }

    /// <summary>
    /// Creates the calculator.
    /// </summary>
    /// <returns>
    /// The calculator.
    /// </returns>
    [NotNull]
    public override PriceCalculator CreateCalculator()
    {
      Assert.IsNotNull(this.User, "User cannot be null.");

      string priceKey = this.membership.IsCustomer(this.User) ? MemberPrice : NormalPrice;
      PriceCalculator calculator = new DefaultPriceCalculator(priceKey, this.totalsFactory);

      return calculator;
    }
  }
}