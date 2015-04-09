// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StateUnlocalizer.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Contains the definition of StateInlocalizer class.
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

namespace Sitecore.Ecommerce.Apps.OrderManagement
{
  using System.Collections.Generic;
  using System.Linq;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using Globalization;
  using Merchant.OrderManagement;
  using Sitecore.Data.Managers;

  /// <summary>
  /// Defines the state unlocalizer class.
  /// </summary>
  public class StateUnlocalizer
  {
    /// <summary>
    /// Stores reference to instance of merchant order state configuration.
    /// </summary>
    private MerchantOrderStateConfiguration merchantOrderStateConfiguration;

    /// <summary>
    /// Stores reference to the language used to get state and substate names.
    /// </summary>
    private Language language;

    /// <summary>
    /// Gets or sets the merchant order state configuration.
    /// </summary>
    /// <value>The merchant order state configuration.</value>
    [NotNull]
    public MerchantOrderStateConfiguration MerchantOrderStateConfiguration 
    {
      get
      {
        if (this.merchantOrderStateConfiguration == null)
        {
          this.merchantOrderStateConfiguration = Context.Entity.Resolve<MerchantOrderStateConfiguration>();
        }

        return this.merchantOrderStateConfiguration;
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.merchantOrderStateConfiguration = value;
      }
    }

    /// <summary>
    /// Gets or sets the language.
    /// </summary>
    /// <value>The language.</value>
    [NotNull]
    public Language Language
    {
      get
      {
        if (this.language == null)
        {
          this.language = LanguageManager.DefaultLanguage;
        }

        return this.language;
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.language = value;
      }
    }

    /// <summary>
    /// Unlocalizes the state.
    /// </summary>
    /// <param name="state">The original state.</param>
    /// <returns>The unlocalized state.</returns>
    public State UnlocalizeState(State state)
    {
      using (new LanguageSwitcher(this.Language))
      {
        State result = this.MerchantOrderStateConfiguration.GetStates().SingleOrDefault(s => s.Code == state.Code);
        IDictionary<string, bool> isSubstateActive = state.Substates.ToDictionary(s => s.Code, s => s.Active);

        foreach (Substate substate in result.Substates)
        {
          substate.Active = isSubstateActive[substate.Code];
        }

        return result;
      }
    }
  }
}
