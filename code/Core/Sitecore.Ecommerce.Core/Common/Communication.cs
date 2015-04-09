// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Communication.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the Communication class.
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

namespace Sitecore.Ecommerce.Common
{
  public class Communication : IEntity
  {
    /// <summary>
    /// Code for the communicationtype
    /// </summary>
    public virtual string ChannelCode { get; set; }

    /// <summary>
    /// The communicationtype as text
    /// </summary>
    public virtual string Channel { get; set; }

    /// <summary>
    /// The value of the communicationType
    /// </summary>
    public virtual string Value { get; set; }

    public virtual long Alias { get; protected set; }
  }
}
