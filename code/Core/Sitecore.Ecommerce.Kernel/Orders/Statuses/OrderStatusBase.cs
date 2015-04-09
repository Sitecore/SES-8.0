// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderStatusBase.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The order status base class.
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

namespace Sitecore.Ecommerce.Orders.Statuses
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Linq;
  using System.Runtime.Serialization;
  using Diagnostics;
  using DomainModel.Data;
  using DomainModel.Orders;
  using Microsoft.Practices.Unity;
  using Sitecore.Data.Fields;
  using Sitecore.Ecommerce.Data;
  
  /// <summary>
  /// The order status base class.
  /// </summary>
  [Obsolete]
  [DataContract]
  [KnownType("GetKnownTypes")]
  public abstract class OrderStatusBase : OrderStatus, IEntity
  {
    /// <summary>
    /// The available following statuses list.
    /// </summary>
    private IEnumerable<OrderStatus> availableFollowingStatuses;

    #region Implementation of IEntity

    /// <summary>
    /// Gets or sets the alias.
    /// </summary>
    /// <value>The alias.</value>
    [DataMember]
    public virtual string Alias { get; [NotNull]set; }

    #endregion

    /// <summary>
    /// Gets or sets code for the order status
    /// </summary>
    [Entity(FieldName = "Code")]
    public override string Code 
    { 
      get
      {
        return base.Code;
      } 
      
      set
      {
        base.Code = value;
      }
    }

    /// <summary>
    /// Gets or sets title for the order status
    /// </summary>
    [Entity(FieldName = "Title")]
    public override string Title
    {
      get
      {
        return base.Title;
      }
      
      set
      {
        base.Title = value;
      }
    }

    /// <summary>
    /// Gets the available following statuses.
    /// </summary>
    /// <returns>Collection of the available following statuses</returns>
    public override IEnumerable<OrderStatus> GetAvailableFollowingStatuses()
    {
      if (this.availableFollowingStatuses != null)
      {
        return this.availableFollowingStatuses;
      }

      IList<OrderStatus> followingStatuses = new List<OrderStatus>();

      using (new SiteIndependentDatabaseSwitcher(Sitecore.Context.ContentDatabase))
      {
        var item = Sitecore.Context.Database.GetItem(this.Alias);

        Assert.IsNotNull(item, "item");

        var availableStateCodes = (new MultilistField(item.Fields["Available List"])).GetItems().Select(stateItem => stateItem["Code"]);

        foreach (var registration in Context.Entity.Registrations.Where(r => availableStateCodes.Contains(r.Name)))
        {
          if (registration.RegisteredType != typeof(OrderStatus))
          {
            continue;
          }

          var entityProvider = Context.Entity.GetType().GetMethod("Resolve").Invoke(Context.Entity, new object[] { typeof(IEntityProvider<>).MakeGenericType(registration.MappedToType), string.Empty, new ResolverOverride[0] });
          var config = new NameValueCollection
                         {
                           { "description", "Order Status Provider" },
                           { "setting name", "Order Statuses Link" },
                           { "default container name", "Default Order Status" },
                           { "containers item template Id", "{3F593780-BA47-4AA9-B413-597291DDE655}" }
                         };
          ((System.Configuration.Provider.ProviderBase)entityProvider).Initialize(config["description"], config);

          followingStatuses.Add((OrderStatus)entityProvider.GetType().GetMethod("Get").Invoke(entityProvider, new object[] { registration.Name }));
        }
      }

      this.availableFollowingStatuses = followingStatuses;

      return this.availableFollowingStatuses;
    }
  }
}