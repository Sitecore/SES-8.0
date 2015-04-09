// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeEntityMemberConverter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the date time mapping rule class.
//   Maps an entity DateTime field value to an item ISO Date format.
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
  using Diagnostics;
  using Sitecore.Data.Fields;

  /// <summary>
  /// Defines the date time mapping rule class.
  /// Maps an entity DateTime field value to an item ISO Date format.
  /// </summary>
  public class DateTimeEntityMemberConverter : FieldBasedEntityMemberConverter<DateTime>
  {
    /// <summary>
    /// Converts the storage object to <see cref="DateTime"/>
    /// </summary>
    /// <param name="storage">The storage object.</param>
    /// <returns>
    /// The entity member value.
    /// </returns>
    public override DateTime ConvertFrom([NotNull] Field storage)
    {
      Assert.ArgumentNotNull(storage, "storage");

      DateField dateField = new DateField(storage);

      return dateField.DateTime;
    }

    /// <summary>
    /// Gets the value from DateTime and converts it to item format.
    /// </summary>
    /// <param name="entityMember">The entity member.</param>
    /// <returns>
    /// The value to save in the item.
    /// </returns>
    /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
    [NotNull]
    public override Field ToStorage(DateTime entityMember)
    {
      this.StorageObject.Value = DateUtil.ToIsoDate(entityMember);

      return this.StorageObject;
    }
  }
}