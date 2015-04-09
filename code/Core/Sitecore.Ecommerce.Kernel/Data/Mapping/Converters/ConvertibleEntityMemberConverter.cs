// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertibleEntityMemberConverter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the field to convertible mapping rule class.
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

namespace Sitecore.Ecommerce.Data.Mapping.Converters
{
  using System;
  using System.Globalization;
  using Diagnostics;
  using DomainModel.Data;
  using Sitecore.Data.Fields;
  using Validators.Interception;

  /// <summary>
  /// Defines the field to convertible mapping rule class.
  /// </summary>
  public class ConvertibleEntityMemberConverter : EntityMemberConverter<IConvertible, Field>, IRequiresEntityMemberType, IRequiresStorageObject<Field>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ConvertibleEntityMemberConverter"/> class.
    /// </summary>
    public ConvertibleEntityMemberConverter()
    {
      this.MemberType = typeof(string);
    }

    /// <summary>
    /// Gets or sets the type of the member.
    /// </summary>
    /// <value>
    /// The type of the member.
    /// </value>
    [NotNull]
    public Type MemberType { get; [NotNullValue]set; }

    /// <summary>
    /// Gets or sets the storage object.
    /// </summary>
    /// <value>
    /// The storage object.
    /// </value>
    public Field StorageObject { get; [NotNullValue]set; }

    /// <summary>
    /// Converts from.
    /// </summary>
    /// <param name="storage">The storage.</param>
    /// <returns>
    /// The from.
    /// </returns>
    [NotNull]
    public override IConvertible ConvertFrom([NotNull] Field storage)
    {
      Assert.ArgumentNotNull(storage, "storage");

      return (IConvertible)Convert.ChangeType(storage.Value, this.MemberType, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converts the <see cref="ConvertibleEntityMemberConverter"/> to a <see cref="Field"/>.
    /// </summary>
    /// <param name="entityMember">The entity member.</param>
    /// <returns>
    /// The <see cref="Field"/>.
    /// </returns>
    [NotNull]
    public override Field ToStorage([CanBeNull] IConvertible entityMember)
    {
      this.StorageObject.Value = entityMember == null ? string.Empty : entityMember.ToString(CultureInfo.InvariantCulture);

      return this.StorageObject;
    }
  }
}