// -------------------------------------------------------------------------------------------
// <copyright file="CheckOut.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// -------------------------------------------------------------------------------------------
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

namespace Sitecore.Ecommerce.CheckOuts
{
  using System;
  using DomainModel.Carts;
  using DomainModel.CheckOuts;
  using Unity;
  using Utils;

  /// <summary>
  /// The check out class.
  /// </summary>
  [Serializable]
  public class CheckOut : ICheckOut
  {
    /// <summary>
    /// Determines whether other shipping address has been checked or not.
    /// </summary>
    private bool hasOtherShippingAddressBeenChecked;

    /// <summary>
    /// Gets a value indicating whether [begin check out].
    /// </summary>
    /// <value><c>true</c> if [begin check out]; otherwise, <c>false</c>.</value>
    public virtual bool BeginCheckOut
    {
      get
      {
        ShoppingCart shoppingCart = Context.Entity.GetInstance<ShoppingCart>();
        return !shoppingCart.ShoppingCartLines.IsNullOrEmpty();
      }
    }

    /// UI session variable that remembers if the "Other shipping address" checkbox was checked.
    /// <summary>
    /// Gets or sets a value indicating whether this instance has other shipping address been checked.
    /// </summary>
    /// <value>
    /// <c>true</c> If this instance has other shipping address been checked; otherwise, <c>false</c>.
    /// </value>
    public virtual bool HasOtherShippingAddressBeenChecked 
    { 
      get
      {
        return this.hasOtherShippingAddressBeenChecked;
      }

      set
      {
        this.hasOtherShippingAddressBeenChecked = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether [name and address done].
    /// </summary>
    /// <value><c>true</c> if [name and address done]; otherwise, <c>false</c>.</value>
    public virtual bool NameAndAddressDone
    {
      get
      {
        ShoppingCart shoppingCart = Context.Entity.GetInstance<ShoppingCart>();
        if (shoppingCart.CustomerInfo == null)
        {
          return false;
        }

        int validCount = 0;
        int requiredCount = 0;
        return validCount == requiredCount;
      }
    }

    /// <summary>
    /// Gets a value indicating whether [delivery done].
    /// </summary>
    /// <value><c>true</c> if [delivery done]; otherwise, <c>false</c>.</value>
    public virtual bool DeliveryDone
    {
      get
      {
        return !string.IsNullOrEmpty(Context.Entity.GetInstance<ShoppingCart>().ShippingProvider.Code);
      }
    }

    /// <summary>
    /// Resets the check out.
    /// </summary>
    public virtual void ResetCheckOut()
    {
      ICheckOut checkOut = Context.Entity.Resolve<ICheckOut>();
      if (checkOut is CheckOut)
      {
        ((CheckOut)checkOut).HasOtherShippingAddressBeenChecked = false;
      }

      Context.Entity.SetInstance(checkOut);
      Context.Entity.SetInstance(Context.Entity.Resolve<ShoppingCart>());
    }
  }
}