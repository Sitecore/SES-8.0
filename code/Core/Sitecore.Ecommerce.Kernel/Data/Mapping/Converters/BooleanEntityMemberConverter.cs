// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanEntityMemberConverter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the boolean entity member converter class.
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
  using Diagnostics;
  using Sitecore.Data.Fields;

  /// <summary>
  /// Defines the boolean entity member converter class.
  /// </summary>
  public class BooleanEntityMemberConverter : FieldBasedEntityMemberConverter<bool>
  {
    /// <summary>
    /// Converts from.
    /// </summary>
    /// <param name="storage">The storage.</param>
    /// <returns>
    /// The from.
    /// </returns>
    public override bool ConvertFrom([NotNull] Field storage)
    {
      Assert.ArgumentNotNull(storage, "storage");

      CheckboxField cb = new CheckboxField(storage);

      return cb.Checked;
    }

    /// <summary>
    /// Converts the <see cref="BooleanEntityMemberConverter"/> to a <see cref="string"/>.
    /// </summary>
    /// <param name="entityMember">if set to <c>true</c> [entity member].</param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    [NotNull]
    public override Field ToStorage(bool entityMember)
    {
      CheckboxField cb = new CheckboxField(this.StorageObject) { Checked = entityMember };

      return cb.InnerField;
    }
  }
}