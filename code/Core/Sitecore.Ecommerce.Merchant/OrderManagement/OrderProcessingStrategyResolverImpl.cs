// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderProcessingStrategyResolverImpl.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Strategy resolver implementation.
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

namespace Sitecore.Ecommerce.Merchant.OrderManagement
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text.RegularExpressions;
  using Diagnostics;
  using Ecommerce.OrderManagement;
  using Ecommerce.OrderManagement.Orders;
  using Utils;

  /// <summary>
  /// Strategy resolver implementation.
  /// </summary>
  public class OrderProcessingStrategyResolverImpl : OrderProcessingStrategyResolver
  {
    /// <summary>
    /// List of order processing strategies.
    /// </summary>
    private readonly LinkedList<OrderProcessingStrategy> strategies = new LinkedList<OrderProcessingStrategy>();

    /// <summary>
    /// Gets or sets the order security.
    /// </summary>
    /// <value>The order security.</value>
    public MerchantOrderSecurity OrderSecurity { get; set; }

    /// <summary>
    /// Gets or sets the cancel order processing strategy.
    /// </summary>
    /// <value>The cancel order processing strategy.</value>
    [CanBeNull]
    public OrderProcessingStrategy CancelOrderProcessingStrategy { get; set; }

    /// <summary>
    /// Gets or sets the capture order processing strategy.
    /// </summary>
    /// <value>The capture order processing strategy.</value>
    [CanBeNull]
    public OrderProcessingStrategy CaptureOrderProcessingStrategy { get; set; }

    /// <summary>
    /// Gets or sets the pack order processing strategy.
    /// </summary>
    /// <value>
    /// The pack order processing strategy.
    /// </value>
    [CanBeNull]
    public OrderProcessingStrategy PackOrderProcessingStrategy { get; set; }

    /// <summary>
    /// Gets or sets the ship order processing strategy.
    /// </summary>
    /// <value>
    /// The ship order processing strategy.
    /// </value>
    [CanBeNull]
    public OrderProcessingStrategy ShipOrderProcessingStrategy { get; set; }

    /// <summary>
    /// Gets or sets the suspicious product quantity order processing strategy.
    /// </summary>
    /// <value>
    /// The suspicious product quantity order processing strategy.
    /// </value>
    [CanBeNull]
    public OrderProcessingStrategy SuspiciousProductQuantityOrderProcessingStrategy { get; set; }

    /// <summary>
    /// Gets or sets the pack order processing strategy.
    /// </summary>
    /// <value>
    /// The pack order processing strategy.
    /// </value>
    [CanBeNull]
    public OrderProcessingStrategy OrderStateProcessingStrategy { get; set; }

    /// <summary>
    /// Gets the strategy.
    /// </summary>
    /// <param name="oldOrder">The old order.</param>
    /// <param name="newOrder">The new order.</param>
    /// <returns>
    /// The strategy.
    /// </returns>
    [NotNull]
    public override IEnumerable<OrderProcessingStrategy> GetStrategies([CanBeNull] Order oldOrder, [NotNull] Order newOrder)
    {
      Assert.ArgumentNotNull(newOrder, "newOrder");

      this.strategies.Clear();

      if (oldOrder != null)
      {
        IEnumerable<string> addedEntityFields, removedEntityFields, changedEntityFields;

        this.CalculateOrderDiff(oldOrder, newOrder, out addedEntityFields, out removedEntityFields, out changedEntityFields);

        Assert.IsNotNull(addedEntityFields, "addedEntityFields");
        Assert.IsNotNull(removedEntityFields, "removedEntityFields");
        Assert.IsNotNull(changedEntityFields, "changedEntityFields");

        this.ProcessChanges(oldOrder, newOrder, addedEntityFields, removedEntityFields, changedEntityFields);
      }

      if (this.strategies.Count == 0)
      {
        this.strategies.AddLast(OrderProcessingStrategy.Empty);
      }

      return this.strategies;
    }

    /// <summary>
    /// Calculates the order diff.
    /// </summary>
    /// <param name="oldOrder">The old order.</param>
    /// <param name="newOrder">The new order.</param>
    /// <param name="addedFields">The added fields.</param>
    /// <param name="removedFields">The removed fields.</param>
    /// <param name="changedFields">The changed fields.</param>
    protected virtual void CalculateOrderDiff([NotNull] Order oldOrder, [NotNull] Order newOrder, [CanBeNull] out IEnumerable<string> addedFields, [CanBeNull] out IEnumerable<string> removedFields, [CanBeNull] out IEnumerable<string> changedFields)
    {
      Assert.ArgumentNotNull(oldOrder, "oldOrder");
      Assert.ArgumentNotNull(newOrder, "newOrder");

      var overrides = new KeyValuePair<KeyValuePair<Type, Type>, Func<object, object, IEnumerable<string>>>(new KeyValuePair<Type, Type>(typeof(DateTime), typeof(DateTime)), this.DateTimeComparator);

      IEnumerable<string> differentFields = PropertyUtil.GetDifferentFields(oldOrder, newOrder, true, overrides);
      addedFields = this.GetAddedEntityFields(oldOrder, newOrder, differentFields);
      var diffForRemoved = differentFields.Except(addedFields);

      removedFields = this.GetAddedEntityFields(newOrder, oldOrder, diffForRemoved);

      var diffForChanged = diffForRemoved.Except(removedFields).Where(f => !f.EndsWith(".Alias"));

      changedFields = this.FilterChangedFields(oldOrder, newOrder, diffForChanged);
    }

    /// <summary>
    /// Processes the changes.
    /// </summary>
    /// <param name="oldOrder">The old order.</param>
    /// <param name="newOrder">The new order.</param>
    /// <param name="addedFields">The added fields.</param>
    /// <param name="removedFields">The removed fields.</param>
    /// <param name="changedFields">The changed fields.</param>
    protected virtual void ProcessChanges([NotNull] Order oldOrder, [NotNull] Order newOrder, [NotNull] IEnumerable<string> addedFields, [NotNull] IEnumerable<string> removedFields, [NotNull] IEnumerable<string> changedFields)
    {
      Assert.ArgumentNotNull(oldOrder, "oldOrder");
      Assert.ArgumentNotNull(newOrder, "newOrder");
      Assert.ArgumentNotNull(addedFields, "addedFields");
      Assert.ArgumentNotNull(removedFields, "removedFields");
      Assert.ArgumentNotNull(changedFields, "changedFields");

      changedFields = this.ProcessStateChanges(newOrder, oldOrder, changedFields);
      changedFields = this.ProcessAllowanceChargeChanges(newOrder, oldOrder, changedFields);
      changedFields = this.ProcessAnticipatedMonetaryTotalChanges(newOrder, oldOrder, changedFields);
      changedFields = this.ProcessTaxTotalChanges(newOrder, oldOrder, changedFields);
      changedFields = this.ProcessOrderLineChanges(newOrder, oldOrder, changedFields);

      this.ProcessOrderChanges(newOrder, oldOrder, changedFields);
    }

    /// <summary>
    /// Processes the state changes.
    /// </summary>
    /// <param name="newOrder">The new order.</param>
    /// <param name="oldOrder">The old order.</param>
    /// <param name="changedEntityFields">The changed entity fields.</param>
    /// <returns>
    /// The state changes.
    /// </returns>
    [NotNull]
    private IEnumerable<string> ProcessStateChanges([NotNull] Order newOrder, [NotNull] Order oldOrder, [NotNull] IEnumerable<string> changedEntityFields)
    {
      Assert.ArgumentNotNull(newOrder, "newOrder");
      Assert.ArgumentNotNull(oldOrder, "oldOrder");
      Assert.ArgumentNotNull(changedEntityFields, "changedEntityFields");

      Assert.IsNotNull(this.OrderSecurity, Texts.UnableToSaveTheOrdersOrderSecurityCannotBeNull);

      if (changedEntityFields.Any(propertyName => propertyName.StartsWith("State.") || (propertyName == "State")))
      {
        Assert.IsNotNull(this.OrderStateProcessingStrategy, "Unable to get strategies. OrderStateProcessingStrategy cannot be null.");

        Assert.IsTrue(((oldOrder.State.Code != OrderStateCode.Closed) && (oldOrder.State.Code != OrderStateCode.Cancelled)) || this.OrderSecurity.CanReopen(oldOrder), Texts.YouDoNotHaveTheNecessaryPermissionsToReopenTheOrder);
        Assert.IsTrue((oldOrder.State.Code == OrderStateCode.Cancelled) || (newOrder.State.Code != OrderStateCode.Cancelled) || this.OrderSecurity.CanCancel(oldOrder), Texts.YouDoNotHaveTheNecessaryPermissionsToCancelTheOrder);

        this.CheckSubstates(newOrder, oldOrder);
        if (!this.strategies.Contains(this.CaptureOrderProcessingStrategy))
        {
          this.strategies.AddLast(this.OrderStateProcessingStrategy);
        }

        changedEntityFields = changedEntityFields.Where(propertyName => !(propertyName.StartsWith("State.") || (propertyName == "State")));
      }

      return changedEntityFields;
    }

    /// <summary>
    /// Processes the order line changes.
    /// </summary>
    /// <param name="newOrder">The new order.</param>
    /// <param name="oldOrder">The old order.</param>
    /// <param name="changedEntityFields">The changed entity fields.</param>
    /// <returns>The order line changes.</returns>
    private IEnumerable<string> ProcessOrderLineChanges([NotNull] Order newOrder, [NotNull] Order oldOrder, [NotNull] IEnumerable<string> changedEntityFields)
    {
      Assert.ArgumentNotNull(newOrder, "newOrder");
      Assert.ArgumentNotNull(oldOrder, "oldOrder");
      Assert.ArgumentNotNull(changedEntityFields, "changedEntityFields");

      return changedEntityFields.Where(field => !Regex.IsMatch(field, @"^OrderLines\[\d{1,}\]"));
    }

    /// <summary>
    /// Processes the tax total changes.
    /// </summary>
    /// <param name="newOrder">The new order.</param>
    /// <param name="oldOrder">The old order.</param>
    /// <param name="changedEntityFields">The changed entity fields.</param>
    /// <returns>The tax subtotal changes.</returns>
    private IEnumerable<string> ProcessTaxTotalChanges([NotNull] Order newOrder, [NotNull] Order oldOrder, [NotNull] IEnumerable<string> changedEntityFields)
    {
      Assert.ArgumentNotNull(newOrder, "newOrder");
      Assert.ArgumentNotNull(oldOrder, "oldOrder");
      Assert.ArgumentNotNull(changedEntityFields, "changedEntityFields");

      return changedEntityFields.Where(field => !field.StartsWith("TaxTotal"));
    }


    /// <summary>
    /// Processes the anticipated monetary total changes.
    /// </summary>
    /// <param name="newOrder">The new order.</param>
    /// <param name="oldOrder">The old order.</param>
    /// <param name="changedEntityFields">The changed entity fields.</param>
    /// <returns>The tax subtotal changes.</returns>
    private IEnumerable<string> ProcessAnticipatedMonetaryTotalChanges([NotNull] Order newOrder, [NotNull] Order oldOrder, [NotNull] IEnumerable<string> changedEntityFields)
    {
      Assert.ArgumentNotNull(newOrder, "newOrder");
      Assert.ArgumentNotNull(oldOrder, "oldOrder");
      Assert.ArgumentNotNull(changedEntityFields, "changedEntityFields");

      return changedEntityFields.Where(field => !field.StartsWith("AnticipatedMonetaryTotal"));
    }

    /// <summary>
    /// Processes the allowance charge changes.
    /// </summary>
    /// <param name="newOrder">The new order.</param>
    /// <param name="oldOrder">The old order.</param>
    /// <param name="changedEntityFields">The changed entity fields.</param>
    /// <returns>The allowance charge changes.</returns>
    private IEnumerable<string> ProcessAllowanceChargeChanges([NotNull] Order newOrder, [NotNull] Order oldOrder, [NotNull] IEnumerable<string> changedEntityFields)
    {
      Assert.ArgumentNotNull(newOrder, "newOrder");
      Assert.ArgumentNotNull(oldOrder, "oldOrder");
      Assert.ArgumentNotNull(changedEntityFields, "changedEntityFields");

      var changedChargesValues = changedEntityFields.Where(f => Regex.IsMatch(f, @"^AllowanceCharge\[\d{1,}\].Amount"));
      if (changedChargesValues.Any())
      {
        changedEntityFields = changedEntityFields.Where(f => !changedChargesValues.Contains(f));
        this.strategies.AddLast(new ChangeAllowanceChargeAmountProcessingStrategy(oldOrder, changedChargesValues.ToDictionary(key => key, key => PropertyUtil.GetPropertyValue(oldOrder, key))));
      }

      return changedEntityFields;
    }

    /// <summary>
    /// Processes the order changes.
    /// </summary>
    /// <param name="newOrder">The new order.</param>
    /// <param name="oldOrder">The old order.</param>
    /// <param name="changedEntityFields">The changed entity fields.</param>
    /// <returns>The order changes.</returns>
    private IEnumerable<string> ProcessOrderChanges([NotNull] Order newOrder, [NotNull] Order oldOrder, [NotNull] IEnumerable<string> changedEntityFields)
    {
      Assert.ArgumentNotNull(newOrder, "newOrder");
      Assert.ArgumentNotNull(oldOrder, "oldOrder");
      Assert.ArgumentNotNull(changedEntityFields, "changedEntityFields");

      Assert.IsNotNull(this.OrderSecurity, Texts.UnableToSaveTheOrdersOrderSecurityCannotBeNull);

      if (changedEntityFields.Any())
      {
        Assert.IsTrue(this.OrderSecurity.CanProcess(oldOrder), Texts.YouDoNotHaveTheNecessaryPermissionsToEditTheOrder);

        this.strategies.AddLast(new OrderFieldProcessingStrategy(changedEntityFields.ToDictionary(key => key, key => PropertyUtil.GetPropertyValue(oldOrder, key))));
      }

      return Enumerable.Empty<string>();
    }

    /// <summary>
    /// Checks the in process substates.
    /// </summary>
    /// <param name="newOrder">The new order.</param>
    /// <param name="oldOrder">The old order.</param>
    private void CheckSubstates([NotNull] Order newOrder, [NotNull] Order oldOrder)
    {
      Assert.ArgumentNotNull(newOrder, "newOrder");
      Assert.ArgumentNotNull(oldOrder, "oldOrder");

      Assert.IsNotNull(this.CaptureOrderProcessingStrategy, "Unable to get strategies. CaptureOrderProcessingStrategy cannot be null.");
      this.CheckSubstate(newOrder, oldOrder, this.CaptureOrderProcessingStrategy, OrderStateCode.InProcess, OrderStateCode.InProcessCapturedInFull);

      Assert.IsNotNull(this.PackOrderProcessingStrategy, "Unable to get strategies. PackOrderProcessingStrategy cannot be null.");
      this.CheckSubstate(newOrder, oldOrder, this.PackOrderProcessingStrategy, OrderStateCode.InProcess, OrderStateCode.InProcessPackedInFull);

      Assert.IsNotNull(this.ShipOrderProcessingStrategy, "Unable to get strategies. ShipOrderProcessingStrategy cannot be null.");
      this.CheckSubstate(newOrder, oldOrder, this.ShipOrderProcessingStrategy, OrderStateCode.InProcess, OrderStateCode.InProcessShippedInFull);

      Assert.IsNotNull(this.SuspiciousProductQuantityOrderProcessingStrategy, "Unable to get strategies. SuspiciousProductQuantityOrderProcessingStrategy cannot be null.");
      this.CheckSubstate(newOrder, oldOrder, this.SuspiciousProductQuantityOrderProcessingStrategy, OrderStateCode.Suspicious, OrderStateCode.SuspiciousProductQuantity);
    }

    /// <summary>
    /// Checks the in process substate.
    /// </summary>
    /// <param name="newOrder">The new order.</param>
    /// <param name="oldOrder">The old order.</param>
    /// <param name="strategy">The strategy.</param>
    /// <param name="stateCode">The state code.</param>
    /// <param name="substateCode">The substate code.</param>
    private void CheckSubstate([NotNull] Order newOrder, [NotNull] Order oldOrder, [NotNull] OrderProcessingStrategy strategy, [NotNull] string stateCode, [NotNull] string substateCode)
    {
      Assert.ArgumentNotNull(newOrder, "newOrder");
      Assert.ArgumentNotNull(oldOrder, "oldOrder");
      Assert.ArgumentNotNull(strategy, "strategy");
      Assert.ArgumentNotNull(stateCode, "stateCode");

      if (this.CheckSubstateChanged(oldOrder, newOrder, stateCode, substateCode))
      {
        this.strategies.AddLast(strategy);
      }
    }

    /// <summary>
    /// Checks if capture in full.
    /// </summary>
    /// <param name="oldOrder">The old order.</param>
    /// <param name="newOrder">The new order.</param>
    /// <param name="stateCode">The state code.</param>
    /// <param name="substateCode">The substate code.</param>
    /// <returns>
    /// Boolean value.
    /// </returns>
    internal bool CheckSubstateChanged([CanBeNull] Order oldOrder, [NotNull] Order newOrder, [NotNull] string stateCode, [NotNull] string substateCode)
    {
      Assert.ArgumentNotNull(newOrder, "newOrder");
      Assert.ArgumentNotNull(substateCode, "substateCode");

      Assert.IsNotNull(newOrder.State, "State cannot be null.");
      Assert.IsNotNull(newOrder.State.Substates, "State.Substates cannot be null.");

      bool oldFlag = true;
      bool newFlag = false;

      Substate substateNew = newOrder.State.Substates.FirstOrDefault(ss => ss.Code == substateCode);
      if ((newOrder.State.Code == stateCode) && (substateNew != null))
      {
        if (substateNew.Active)
        {
          newFlag = true;
        }
      }

      if (oldOrder != null)
      {
        Assert.IsNotNull(oldOrder.State, "State cannot be null.");
        Assert.IsNotNull(oldOrder.State.Substates, "State.Substates cannot be null.");

        Substate substateOld = oldOrder.State.Substates.FirstOrDefault(ss => ss.Code == substateCode);
        if ((oldOrder.State.Code == stateCode) && (substateOld != null))
        {
          if (substateOld.Active)
          {
            oldFlag = false;
          }
        }
      }

      return oldFlag && newFlag;
    }

    /// <summary>
    /// Gets the added entity fields.
    /// </summary>
    /// <param name="oldOrder">The old order.</param>
    /// <param name="newOrder">The new order.</param>
    /// <param name="changedFieldList">The changed field list.</param>
    /// <returns>The added entity fields.</returns>
    [NotNull]
    private IEnumerable<string> GetAddedEntityFields([NotNull] Order oldOrder, [NotNull] Order newOrder, [NotNull] IEnumerable<string> changedFieldList)
    {
      Assert.ArgumentNotNull(oldOrder, "oldOrder");
      Assert.ArgumentNotNull(newOrder, "newOrder");
      Assert.ArgumentNotNull(changedFieldList, "changedFieldList");

      LinkedList<string> result = new LinkedList<string>();

      foreach (string field in changedFieldList)
      {
        bool added = false;
        bool removed = false;

        try
        {
          PropertyUtil.GetPropertyValue(oldOrder, field);
        }
        catch (ArgumentOutOfRangeException)
        {
          added = true;
        }

        try
        {
          PropertyUtil.GetPropertyValue(newOrder, field);
        }
        catch (ArgumentOutOfRangeException)
        {
          removed = true;
        }

        if (added && (!removed))
        {
          result.AddLast(field);
        }
      }

      return result;
    }

    /// <summary>
    /// Filters the changed fields.
    /// </summary>
    /// <param name="oldOrder">The old order.</param>
    /// <param name="newOrder">The new order.</param>
    /// <param name="changedFieldList">The changed field list.</param>
    /// <returns>The changed fields.</returns>
    [NotNull]
    private IEnumerable<string> FilterChangedFields([NotNull] Order oldOrder, [NotNull] Order newOrder, [NotNull] IEnumerable<string> changedFieldList)
    {
      Assert.ArgumentNotNull(oldOrder, "oldOrder");
      Assert.ArgumentNotNull(newOrder, "newOrder");
      Assert.ArgumentNotNull(changedFieldList, "changedFieldList");

      LinkedList<string> result = new LinkedList<string>();

      foreach (string field in changedFieldList)
      {
        object oldValue = PropertyUtil.GetPropertyValue(oldOrder, field);
        object newValue = PropertyUtil.GetPropertyValue(newOrder, field);

        if ((oldValue is IEnumerable) ^ (newValue is IEnumerable))
        {
          IEnumerable oldEnumerable = oldValue as IEnumerable;
          IEnumerable newEnumerable = newValue as IEnumerable;

          if (((oldEnumerable == null) || (oldEnumerable.Cast<object>().Count() > 0) || (newValue != null)) &&
            ((newEnumerable == null) || (newEnumerable.Cast<object>().Count() > 0) || (oldValue != null)))
          {
            result.AddLast(field);
          }
        }
        else
        {
          result.AddLast(field);
        }
      }

      return result;
    }

    /// <summary>
    /// Times the span comparator.
    /// </summary>
    /// <param name="obj1">The obj1.</param>
    /// <param name="obj2">The obj2.</param>
    /// <returns>The span comparator.</returns>
    [NotNull]
    private IEnumerable<string> DateTimeComparator([NotNull] object obj1, [NotNull] object obj2)
    {
      Assert.ArgumentNotNull(obj1, "obj1");
      Assert.ArgumentNotNull(obj2, "obj2");

      DateTime dateTime1 = (DateTime)obj1;
      DateTime dateTime2 = (DateTime)obj2;

      if (Math.Abs((dateTime1 - dateTime2).TotalMilliseconds) < 1000)
      {
        return Enumerable.Empty<string>();
      }

      return new[] { string.Empty };
    }
  }
}