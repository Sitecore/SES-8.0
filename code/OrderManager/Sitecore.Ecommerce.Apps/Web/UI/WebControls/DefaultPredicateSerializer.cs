// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultPredicateSerializer.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the default predicate serializer class.
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

namespace Sitecore.Ecommerce.Apps.Web.UI.WebControls
{
  using Sitecore.Configuration;
  using Sitecore.Data.Items;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// Defines the default predicate serializer class.
  /// </summary>
  public class DefaultPredicateSerializer : SitecorePredicateSerializer
  {
    /// <summary>
    /// The operators root item id.
    /// </summary>
    private const string RootItemId = "{94D66266-AE3D-4731-9038-F2A1E758E3C6}";

    /// <summary>
    /// The operators root item
    /// </summary>
    private Item operatorsRootItem;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultPredicateSerializer"/> class.
    /// </summary>
    public DefaultPredicateSerializer()
    {
      this.operatorsRootItem = Factory.GetDatabase("master").GetItem(RootItemId);
    }

    /// <summary>
    /// Gets the type of the lang.
    /// </summary>
    /// <value>The type of the lang.</value>
    [NotNull]
    public override string LangType
    {
      get
      {
        return "Default";
      }
    }

    /// <summary>
    /// Gets or sets the operators root.
    /// </summary>
    /// <value>
    /// The operators root.
    /// </value>
    public override Item OperatorsRoot
    {
      get
      {
        return this.operatorsRootItem;
      }

      set
      {
        this.operatorsRootItem = value;
      }
    }
  }
}
