// -------------------------------------------------------------------------------------------
// <copyright file="QueryNode.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Search
{
  using System.Runtime.Serialization;

  /// <summary>
  /// Query node class
  /// </summary>
  [DataContract(IsReference = true)]
  public class QueryNode
  {
    /// <summary>
    /// Next element of Query
    /// </summary>
    private QueryNode nextNode;

    /// <summary>
    /// Previous element of Query
    /// </summary>
    private QueryNode previousNode;

    /// <summary>
    ///  Element of query
    /// </summary>
    private QueryElement element;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryNode"/> class.
    /// </summary>
    public QueryNode()
    {      
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryNode"/> class.
    /// </summary>
    /// <param name="elem">The element of query</param>
    public QueryNode(QueryElement elem)
    {
      this.element = elem;
    }

    /// <summary>
    /// Gets or sets the next element.
    /// </summary>
    /// <value>The next element.</value>
    [DataMember]
    public QueryNode NextNode
    {
      get { return this.nextNode; }
      set { this.nextNode = value; }
    }

    /// <summary>
    /// Gets or sets the previous element.
    /// </summary>
    /// <value>The previous element.</value>
    [DataMember]
    public QueryNode PreviousNode
    {
      get { return this.previousNode; }
      set { this.previousNode = value; }
    }

    /// <summary>
    /// Gets or sets the element.
    /// </summary>
    /// <value>The element.</value>
    [DataMember]
    public QueryElement Element
    {
      get { return this.element; }
      set { this.element = value; }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is first.
    /// </summary>
    /// <value><c>true</c> if this instance is first; otherwise, <c>false</c>.</value>
    [DataMember]
    public bool IsFirst
    {
      get { return this.PreviousNode == null; }
      set { if (!value) { this.PreviousNode = null; } }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is last.
    /// </summary>
    /// <value><c>true</c> if this instance is last; otherwise, <c>false</c>.</value>
    [DataMember]
    public bool IsLast
    {
      get { return this.NextNode == null; }
      set { if (!value) { this.NextNode = null; } }
    }

    /// <summary>
    /// Determines whether [is sub query].
    /// </summary>
    /// <returns>
    /// <c>true</c> if [is sub query]; otherwise, <c>false</c>.
    /// </returns>
    public bool IsQuery()
    {
      return this.Element is Query; 
    }
  }
}