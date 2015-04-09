// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationPipelineArgs.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the arguments for GetConfiguration pipeline.
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

namespace Sitecore.Ecommerce.Pipelines.GetConfiguration
{
  using System;
  using System.Runtime.Serialization;
  using System.Security.Permissions;

  using Sitecore.Pipelines;

  /// <summary>
  /// Defines the arguments for GetConfiguration pipeline.
  /// </summary>
  [Serializable]
  public class ConfigurationPipelineArgs : PipelineArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationPipelineArgs"/> class.
    /// </summary>
    /// <param name="configurationItemType">Type of the configuration item.</param>
    public ConfigurationPipelineArgs(Type configurationItemType)
    {
      this.ConfigurationItemType = configurationItemType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationPipelineArgs"/> class.
    /// </summary>
    /// <param name="info">The serialized data.</param>
    /// <param name="context">The context.</param>
    protected ConfigurationPipelineArgs([NotNull] SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.ConfigurationItem = (object)info.GetValue("ConfigurationPipelineArgs.ConfigurationItem", typeof(object));
      this.ConfigurationItemType = (Type)info.GetValue("ConfigurationPipelineArgs.ConfigurationItemType", typeof(Type));
      this.EntityName = (string)info.GetValue("ConfigurationPipelineArgs.EntityName", typeof(string));
    }

    /// <summary>
    /// Gets or sets the configuration item.
    /// </summary>
    /// <value>The configuration item.</value>
    public object ConfigurationItem { get; set; }

    /// <summary>
    /// Gets the type of the configuration item.
    /// </summary>
    /// <value>The type of the configuration item.</value>
    public Type ConfigurationItemType { get; private set; }

    /// <summary>
    /// Gets or sets the name of the entity.
    /// </summary>
    /// <value>The name of the entity.</value>
    public string EntityName { get; set; }


    /// <summary>
    /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> with the data needed to serialize the target object.
    /// </summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> to populate with data.</param>
    /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"></see>) for this serialization.</param>
    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("ConfigurationPipelineArgs.ConfigurationItem", this.ConfigurationItem, typeof(object));
      info.AddValue("ConfigurationPipelineArgs.ConfigurationItemType", this.ConfigurationItemType, typeof(Type));
      info.AddValue("ConfigurationPipelineArgs.EntityName", this.EntityName, typeof(string));
    }
  }
}