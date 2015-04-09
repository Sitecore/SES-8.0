// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityAttribute.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The entity attribute.
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

namespace Sitecore.Ecommerce.Data
{
  using System;

  /// <summary>
  /// The entity attribute.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
  public class EntityAttribute : Attribute
  {
    /// <summary>
    /// Gets or sets the template id.
    /// </summary>
    /// <value>The template id.</value>
    public virtual string TemplateId { get; set; }

    /// <summary>
    /// Gets or sets the name of the field.
    /// </summary>
    /// <value>The name of the field.</value>
    public virtual string FieldName { get; set; }

    /// <summary>
    /// Gets or sets the field rule.
    /// </summary>
    /// <value>The field rule.</value>
    public virtual FieldMappingRule Rule { get; set; }

    /// <summary>
    /// Gets or sets the name of the mapping rule.
    /// The mapping rule name used to resolve entity field 
    /// mapping rule from Unity IoC container.
    /// </summary>
    /// <value>
    /// The name of the mapping rule.
    /// </value>
    public string MemberConverter { get; set; }
  }
}