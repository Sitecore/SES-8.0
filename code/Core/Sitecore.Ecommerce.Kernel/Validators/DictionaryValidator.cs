// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryValidator.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the DictionaryValidator class.
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

namespace Sitecore.Ecommerce.Validators
{
  using System.Runtime.Serialization;
  using Sitecore.Data.Items;
  using Sitecore.Data.Validators;

  /// <summary>
  /// </summary>
  public class DictionaryValidator : StandardValidator
  {
    // Methods
    /// <summary>
    /// </summary>
    public DictionaryValidator()
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="info">
    /// </param>
    /// <param name="context">
    /// </param>
    public DictionaryValidator(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    /// <summary>
    /// </summary>
    public override string Name
    {
      get
      {
        return "Dictionary";
      }
    }

    /// <summary>
    /// </summary>
    /// <returns>
    /// </returns>
    protected override ValidatorResult Evaluate()
    {
      Item item = this.GetItem();

      if (!item["Key"].StartsWith(item.Name))
      {
        this.Text =
          "Item name does not match the Key field. These values should match in order to simplify debugging.";
        return ValidatorResult.Warning;
      }

// check if dictionary entry is in correct folder
      string folderLetter = item.Parent.Name;
      string itemFirstLetter = item.Name.Substring(0, 1);

      if (folderLetter.Length == 1 && folderLetter.ToLower() != itemFirstLetter.ToLower())
      {
        this.Text = string.Format("Item name does not start with {0}. You should move this item to the folder{1}", folderLetter, itemFirstLetter.ToUpper());
        return ValidatorResult.Warning;
      }

      return ValidatorResult.Valid;
    }

    /// <summary>
    /// </summary>
    /// <returns>
    /// </returns>
    protected override ValidatorResult GetMaxValidatorResult()
    {
      return this.GetFailedResult(ValidatorResult.Warning);
    }
  }
}