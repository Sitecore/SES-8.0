// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CoreOrderStateConfiguration.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Reads order state configuration from web-shop Business Catalog.
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

namespace Sitecore.Ecommerce.OrderManagement
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Linq.Expressions;
  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.Ecommerce.DomainModel.Configurations;
  using Sitecore.Ecommerce.OrderManagement.Orders;

  /// <summary>
  /// Reads order state configuration from web-shop Business Catalog.
  /// </summary>
  public class CoreOrderStateConfiguration
  {
    /// <summary>
    /// Represents the name of sub-state container.
    /// </summary>
    private const string SubstateContainerName = "Substates";

    /// <summary>
    /// Represents the name of container for following statuses.
    /// </summary>
    private const string FollowingStatusesContainerName = "FollowingStates";

    /// <summary>
    /// Represents the name of container for sub-state combinations.
    /// </summary>
    private const string SubstateCombinationsContainer = "SubstateCombinations";

    /// <summary>
    /// Represents the name of code field.
    /// </summary>
    private const string CodeFieldName = "Code";

    /// <summary>
    /// Represents the name of name field.
    /// </summary>
    private const string NameFieldName = "Name";

    /// <summary>
    /// The abbreviation field name.
    /// </summary>
    private const string AbbreviationFieldName = "Abbreviation";

    /// <summary>
    /// Represents the name of following state field.
    /// </summary>
    private const string FollowingStateFieldName = "Following State";

    /// <summary>
    /// Represents the name of sub-states field.
    /// </summary>
    private const string SubstatesFieldName = "Substates";

    /// <summary>
    /// Template ID of state item.
    /// </summary>
    private static readonly TemplateID FollowingStateLinkTemplateId = new TemplateID(ID.Parse("{AB83D7CA-E135-40BB-931F-8EB9E19714B6}"));

    /// <summary>
    /// Gets or sets the shop context.
    /// </summary>
    /// <value>
    /// The shop context.
    /// </value>
    [CanBeNull]
    public ShopContext ShopContext { get; set; }

    /// <summary>
    /// Gets the states.
    /// </summary>
    /// <returns>The states.</returns>
    [NotNull]
    protected internal virtual IQueryable<State> GetStates()
    {
      return this.GetAll(state => true);
    }

    /// <summary>
    /// Gets the specified expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>
    /// The collection of the states..
    /// </returns>
    [NotNull]
    protected internal virtual IQueryable<State> GetAll([NotNull] Expression<Func<State, bool>> expression)
    {
      Debug.ArgumentNotNull(expression, "expression");

      return this.GetRootItem().Children.Select(this.CreateStateFromItem).AsQueryable().Where(expression);
    }

    /// <summary>
    /// Gets the following states.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <returns>
    /// The following states.
    /// </returns>
    [NotNull]
    protected internal virtual IQueryable<State> GetFollowingStates([NotNull] State state)
    {
      Debug.ArgumentNotNull(state, "state");

      ICollection<Tuple<string, SubstateCombinationSet>> stateTransitionInfo = new LinkedList<Tuple<string, SubstateCombinationSet>>();
      Dictionary<string, bool> substateCombinationPrototype = state.Substates.ToDictionary(substate => substate.Code, substate => false);
      Sitecore.Data.Items.Item stateItem = this.GeStateItem(state);

      if (stateItem != null)
      {
        Sitecore.Data.Items.Item[] transitionItems = stateItem.Axes.SelectItems(string.Format("./{0}/*", FollowingStatusesContainerName));

        if (transitionItems != null)
        {
          foreach (Sitecore.Data.Items.Item transitionItem in transitionItems)
          {
            if (((Sitecore.Data.Fields.ReferenceField)transitionItem.Fields[FollowingStateFieldName]).TargetItem != null)
            {
              string stateCode = ((Sitecore.Data.Fields.ReferenceField)transitionItem.Fields[FollowingStateFieldName]).TargetItem.Fields[CodeFieldName].Value;
              stateTransitionInfo.Add(Tuple.Create(stateCode, this.CreateSubstateCombinationSetFromItem(transitionItem, substateCombinationPrototype)));
            }
          }
        }
      }

      LinkedList<string> resultingStateCodes = new LinkedList<string>();

      foreach (Tuple<string, SubstateCombinationSet> transition in stateTransitionInfo)
      {
        if (this.IsSubstateCombinationApplicable(state, transition.Item2))
        {
          resultingStateCodes.AddLast(transition.Item1);
        }
      }

      return this.GetAll(s => resultingStateCodes.Contains(s.Code));
    }

    /// <summary>
    /// Gets the previous states.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <returns>
    /// The previous states.
    /// </returns>
    [NotNull]
    protected internal virtual IQueryable<State> GetPreviousStates([NotNull] State state)
    {
      Debug.ArgumentNotNull(state, "state");

      Sitecore.Data.Items.Item stateItem = this.GeStateItem(state);

      IEnumerable<string> previousStateCodes = Globals.LinkDatabase.GetReferrers(stateItem)
        .Select(itemLink => itemLink.GetSourceItem())
        .Where(sourceItem => sourceItem != null && sourceItem.TemplateID == FollowingStateLinkTemplateId)
        .Select(stateLink => stateLink.Parent.Parent)
        .Distinct()
        .Select(si => si.Fields[CodeFieldName].Value);

      return this.GetAll(s => previousStateCodes.Contains(s.Code));
    }

    /// <summary>
    /// Determines whether the specified state is valid.
    /// </summary>
    /// <param name="state">
    /// The state.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified state is valid; otherwise, <c>false</c>.
    /// </returns>
    protected internal virtual bool IsValid([NotNull] State state)
    {
      Debug.ArgumentNotNull(state, "state");

      return this.IsSubstateCombinationApplicable(state, this.GetSubstateCombinations(state));
    }

    /// <summary>
    /// Gets the sub-state combinations.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <returns>
    /// The sub-state combinations.
    /// </returns>
    [NotNull]
    protected internal virtual SubstateCombinationSet GetSubstateCombinations([NotNull] State state)
    {
      Debug.ArgumentNotNull(state, "state");

      Sitecore.Data.Items.Item stateItem = this.GeStateItem(state);
      Assert.IsNotNull(stateItem, "Unable to get substate combination set. State item not found.");

      return this.CreateSubstateCombinationSetFromItem(stateItem.Children[SubstateCombinationsContainer], state.Substates != null ? state.Substates.ToDictionary(substate => substate.Code, substate => false) : new Dictionary<string, bool>());
    }

    /// <summary>
    /// Determines whether sub-state combination is applicable for the specified state.
    /// </summary>
    /// <param name="state">
    /// The state.
    /// </param>
    /// <param name="substateCombinationSet">
    /// The sub-state Combination Set.
    /// </param>
    /// <returns>
    /// <c>true</c> if sub-state combination is applicable for the specified state; otherwise, <c>false</c>.
    /// </returns>
    private bool IsSubstateCombinationApplicable([NotNull] State state, [NotNull] SubstateCombinationSet substateCombinationSet)
    {
      Debug.ArgumentNotNull(state, "state");
      Debug.ArgumentNotNull(substateCombinationSet, "substateCombinationSet");

      return substateCombinationSet.SubstateCombinations.Aggregate(!substateCombinationSet.SubstateCombinations.Any(), (val, combination) => val || (state.Substates.Aggregate(true, (v, e) => v && (e.Active == combination[e.Code])) && (state.Substates.Count > 0)));
    }

    /// <summary>
    /// Creates the sub-state combination set from item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="substateCombinationPrototype">The sub-state combination prototype.</param>
    /// <returns>
    /// The sub-state combination set from item.
    /// </returns>
    [NotNull]
    private SubstateCombinationSet CreateSubstateCombinationSetFromItem([NotNull] Sitecore.Data.Items.Item item, [NotNull] IDictionary<string, bool> substateCombinationPrototype)
    {
      Debug.ArgumentNotNull(item, "item");
      Debug.ArgumentNotNull(substateCombinationPrototype, "substateCombinationPrototype");

      SubstateCombinationSet result = new SubstateCombinationSet();
      List<IDictionary<string, bool>> substateCombinationList = new List<IDictionary<string, bool>>();

      result.SubstateCombinations = substateCombinationList;

      foreach (Sitecore.Data.Items.Item substateCombinationItem in item.Children)
      {
        IDictionary<string, bool> substateCombination = new Dictionary<string, bool>(substateCombinationPrototype);

        foreach (Sitecore.Data.Items.Item substateItem in ((Sitecore.Data.Fields.MultilistField)substateCombinationItem.Fields[SubstatesFieldName]).GetItems())
        {
          substateCombination[substateItem.Fields[CodeFieldName].Value] = !substateCombination[substateItem.Fields[CodeFieldName].Value];
        }

        substateCombinationList.Add(substateCombination);
      }

      return result;
    }

    /// <summary>
    /// Gets the item.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <returns>The item.</returns>
    private Sitecore.Data.Items.Item GeStateItem([NotNull] State state)
    {
      Debug.ArgumentNotNull(state, "state");

      return this.GetRootItem().Axes.SelectSingleItem(string.Format("./*[@{0}='{1}']", CodeFieldName, state.Code));
    }

    /// <summary>
    /// Creates the state from item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>The state from item.</returns>
    [NotNull]
    private State CreateStateFromItem([NotNull] Sitecore.Data.Items.Item item)
    {
      Debug.ArgumentNotNull(item, "item");

      State result = new State();

      this.Map(item, result);

      return result;
    }

    /// <summary>
    /// Gets the root item.
    /// </summary>
    /// <returns>The root item.</returns>
    [NotNull]
    private Sitecore.Data.Items.Item GetRootItem()
    {
      Assert.IsNotNull(this.ShopContext, "Unable to get order states. ShopContext cannot be null.");

      BusinessCatalogSettings settings = this.ShopContext.BusinessCatalogSettings;
      Assert.IsNotNull(settings, "Unable to get order states. BusinessCatalogSettings cannot be null.");

      string orderStatusesLink = settings.OrderStatusesLink;
      Assert.IsNotNull(orderStatusesLink, "Unable to get order states. OrderStatusesLink cannot be null.");

      Database database = this.ShopContext.InnerSite.ContentDatabase;
      Assert.IsNotNull(database, "Unable to get order states. Database cannot be null.");

      Sitecore.Data.Items.Item root = database.GetItem(settings.OrderStatusesLink);
      Assert.IsNotNull(root, "Unable to get order state. RootItem cannot be null.");

      return root;
    }

    /// <summary>
    /// Maps the specified source.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    private void Map([NotNull] Sitecore.Data.Items.Item source, [NotNull] State destination)
    {
      Debug.ArgumentNotNull(source, "source");
      Debug.ArgumentNotNull(destination, "destination");

      destination.Code = source.Fields[CodeFieldName].Value;
      destination.Name = source.Fields[NameFieldName].Value;

      destination.Substates.Clear();

      Sitecore.Data.Items.Item[] substateItems = source.Axes.SelectItems(string.Format("./{0}/*", SubstateContainerName));

      if (substateItems != null)
      {
        foreach (Sitecore.Data.Items.Item item in substateItems)
        {
          Substate substate = new Substate();
          this.Map(item, substate);
          destination.Substates.Add(substate);
        }
      }
    }

    /// <summary>
    /// Maps the specified source.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    private void Map([NotNull] Sitecore.Data.Items.Item source, [NotNull] Substate destination)
    {
      Debug.ArgumentNotNull(source, "source");
      Debug.ArgumentNotNull(destination, "destination");

      destination.Code = source.Fields[CodeFieldName].Value;
      destination.Name = source.Fields[NameFieldName].Value;
      destination.Abbreviation = source.Fields[AbbreviationFieldName].Value;
    }
  }
}