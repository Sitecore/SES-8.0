// -------------------------------------------------------------------------------------------
// <copyright file="Validators.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.PriceMatrix
{
  using System;
  using System.Runtime.Serialization;
  using System.Security.Permissions;
  using System.Text.RegularExpressions;
  using Sitecore.Data.Items;
  using Sitecore.Data.Validators;

  /// <summary>
  /// The Price matrix validator.
  /// </summary>
  [Serializable]
  public class PriceMatrixValidator : StandardValidator
  {
    /// <summary>
    /// The message
    /// </summary>
    private string message = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="PriceMatrixValidator"/> class.
    /// </summary>
    public PriceMatrixValidator()
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PriceMatrixValidator"/> class.
    /// </summary>
    /// <param name="info">The Serialization info.</param>
    /// <param name="context">The context.</param>
    protected PriceMatrixValidator(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.message = (string)info.GetValue("PriceMatrixValidator.message", typeof(string));
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>
    /// The validator name.
    /// </value>
    public override string Name
    {
      get
      {
        return "Required";
      }
    }

    /// <summary>
    /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" />
    /// with the data needed to serialize the target object.
    /// </summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
    /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("PriceMatrixValidator.message", this.message, typeof(string));
    }
    
    /// <summary>
    /// When overridden in a derived class, this method contains the code
    /// to determine whether the value in the input control is valid.
    /// </summary>
    /// <returns>
    /// The result of the evaluation.
    /// </returns>
    protected override ValidatorResult Evaluate()
    {
      var priceMatrixItem = PriceMatrix.Load(this.ControlValidationValue);
      var item = Sitecore.Context.Database.SelectSingleItem("/*/system/Modules/*[@@templatekey='configuration']").Children["PriceMatrix"];

      if (priceMatrixItem != null)
      {
        this.IterateRecursiveToLoad(priceMatrixItem.MainCategory, item);
      }

      if (string.IsNullOrEmpty(this.message))
      {
        return ValidatorResult.Valid;
      }

      this.Text = string.Format("Field \"{0}\" has some invalid fields.\n", this.GetFieldDisplayName()) +
                  this.message;
      return this.GetFailedResult(ValidatorResult.CriticalError);
    }
    
    /// <summary>
    /// Iterates the recursive to load.
    /// </summary>
    /// <param name="priceMatrixItem">The price matrix item.</param>
    /// <param name="item">The item.</param>
    protected virtual void IterateRecursiveToLoad(IPriceMatrixItem priceMatrixItem, Item item)
    {
      var key = item.Template.Key;
      if (key.Equals("pricematrix settings"))
      {
        var children = item.Children;
        foreach (Item child in children)
        {
          IPriceMatrixItem categoryChild = null;
          var matrixItem = priceMatrixItem as Category;
          if (matrixItem != null)
          {
            var category = matrixItem;
            categoryChild = category.GetElement(child.Name);
          }

          this.IterateRecursiveToLoad(categoryChild, child);
        }
      }
      else if (key.Equals("pricematrix site"))
      {
        foreach (Item child in item.Children)
        {
          IPriceMatrixItem categoryChild = null;
          var matrixItem = priceMatrixItem as Category;
          if (matrixItem != null)
          {
            var category = matrixItem;
            categoryChild = category.GetElement(child.Name);
          }

          this.IterateRecursiveToLoad(categoryChild, child);
        }
      }
      else if (key.Equals("pricematrix price"))
      {
        var matrixItem = priceMatrixItem as CategoryItem;
        var categoryItem = matrixItem;
        if (categoryItem == null)
        {
          return;
        }

        var value = categoryItem.Amount ?? string.Empty;
        var expression = this.Parameters["Pattern"];

        var regex = new Regex(expression);
        if (string.IsNullOrEmpty(value))
        {
          return;
        }

        if (!regex.IsMatch(value))
        {
          this.message += string.Format("Price field \"{0}\" has wrong format: \"{1}\".\n", new object[] { this.GetAncestorPath(item), value });
        }
      }
    }

    /// <summary>
    /// Gets the ancestor path.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>the ancestor path.</returns>
    protected virtual string GetAncestorPath(Item item)
    {
      var path = string.Empty;
      while (item != null && !item.Template.Key.Equals("pricematrix settings"))
      {
        var title = item["title"];
        title = string.IsNullOrEmpty(title) ? item.Name : title.Replace(":", string.Empty);

        if (string.IsNullOrEmpty(path))
        {
          path = title;
        }
        else
        {
          path = title + "/" + path;
        }

        item = item.Parent;
      }

      return path;
    }

    /// <summary>
    /// Gets the max validator result.
    /// </summary>
    /// <returns>
    /// The max validator result.
    /// </returns>
    /// <remarks>
    /// This is used when saving and the validator uses a thread. If the Max Validator Result
    /// is Error or below, the validator does not have to be evaluated before saving.
    /// If the Max Validator Result is CriticalError or FatalError, the validator must have
    /// been evaluated before saving.
    /// </remarks>
    protected override ValidatorResult GetMaxValidatorResult()
    {
      return this.GetFailedResult(ValidatorResult.CriticalError);
    }
  }
}