// -------------------------------------------------------------------------------------------
// <copyright file="ListNodeIterator.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Xsl
{
  using System.Collections.Generic;
  using System.Xml.XPath;
  using Sitecore.Configuration;
  using Sitecore.Data.Items;
  using Xml.XPath;

  /// <summary>
  /// The list node iterator.
  /// </summary>
  internal class ListNodeIterator : XPathNodeIterator
  {
    /// <summary>
    /// List of Iterators.
    /// </summary>
    private readonly List<XPathNavigator> list;

    /// <summary>
    /// </summary>
    private readonly bool reverseOrder;

    /// <summary>
    /// </summary>
    private int currentIndex;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListNodeIterator"/> class.
    /// </summary>
    public ListNodeIterator()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListNodeIterator"/> class.
    /// </summary>
    /// <param name="items">The items.</param>
    public ListNodeIterator(List<Item> items) : this(items, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListNodeIterator"/> class.
    /// </summary>
    /// <param name="items">The items collection.</param>
    public ListNodeIterator(Item[] items) : this(items, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListNodeIterator"/> class.
    /// </summary>
    /// <param name="list">The iterator list.</param>
    /// <param name="reverseOrder">if set to <c>true</c> [reverse order].</param>
    public ListNodeIterator(List<XPathNavigator> list, bool reverseOrder)
    {
      this.list = list;
      this.reverseOrder = reverseOrder;
    }

    /// <summary>
    /// Gets the index of the last node in the selected set of nodes.
    /// </summary>
    /// <value></value>
    /// <returns>
    /// The int index of the last node in the selected set of nodes, or 0 if there are no selected nodes.
    /// </returns>
    public override int Count
    {
      get
      {
        return this.list.Count;
      }
    }

    /// <summary>
    /// When overridden in a derived class, returns the <see cref="T:System.Xml.XPath.XPathNavigator"/> object for this <see cref="T:System.Xml.XPath.XPathNodeIterator"/>, positioned on the current context node.
    /// </summary>
    /// <value></value>
    /// <returns>
    /// An <see cref="T:System.Xml.XPath.XPathNavigator"/> object positioned on the context node from which the node set was selected. The <see cref="M:System.Xml.XPath.XPathNodeIterator.MoveNext"/> method must be called to move the <see cref="T:System.Xml.XPath.XPathNodeIterator"/> to the first node in the selected set.
    /// </returns>
    public override XPathNavigator Current
    {
      get
      {
        if ((this.currentIndex <= 0) || (this.currentIndex > this.list.Count))
        {
          return null;
        }

        if (this.reverseOrder)
        {
          return this.list[this.list.Count - this.currentIndex];
        }

        return this.list[this.currentIndex - 1];
      }
    }

    /// <summary>
    /// When overridden in a derived class, gets the index of the current position in the selected set of nodes.
    /// </summary>
    /// <value></value>
    /// <returns>
    /// The int index of the current position.
    /// </returns>
    public override int CurrentPosition
    {
      get
      {
        return this.currentIndex;
      }
    }

    /// <summary>
    /// When overridden in a derived class, returns a clone of this <see cref="T:System.Xml.XPath.XPathNodeIterator"/> object.
    /// </summary>
    /// <returns>
    /// A new <see cref="T:System.Xml.XPath.XPathNodeIterator"/> object clone of this <see cref="T:System.Xml.XPath.XPathNodeIterator"/> object.
    /// </returns>
    public override XPathNodeIterator Clone()
    {
      return new ListNodeIterator(this.list, this.reverseOrder);
    }

    /// <summary>
    /// When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator"/> object returned by the <see cref="P:System.Xml.XPath.XPathNodeIterator.Current"/> property to the next node in the selected node set.
    /// </summary>
    /// <returns>
    /// true if the <see cref="T:System.Xml.XPath.XPathNavigator"/> object moved to the next node; false if there are no more selected nodes.
    /// </returns>
    public override bool MoveNext()
    {
      if (this.list == null)
      {
        return false;
      }

      this.currentIndex++;
      return this.currentIndex <= this.list.Count;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListNodeIterator"/> class.
    /// </summary>
    /// <param name="items">The items collection.</param>
    /// <param name="reverseOrder">if set to <c>true</c> [reverse order].</param>
    private ListNodeIterator(Item[] items, bool reverseOrder)
    {
      this.reverseOrder = reverseOrder;
      this.list = new List<XPathNavigator>();
      foreach (Item item in items)
      {
        if (item == null)
        {
          continue;
        }

        ItemNavigator nav = Factory.CreateItemNavigator(item);
        this.list.Add(nav.CreateNavigator());
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListNodeIterator"/> class.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="reverseOrder">if set to <c>true</c> [reverse order].</param>
    private ListNodeIterator(List<Item> items, bool reverseOrder)
    {
      this.reverseOrder = reverseOrder;
      this.list = new List<XPathNavigator>();
      foreach (Item item in items)
      {
        if (item == null)
        {
          continue;
        }

        ItemNavigator nav = Factory.CreateItemNavigator(item);
        this.list.Add(nav.CreateNavigator());
      }
    }
  }
}