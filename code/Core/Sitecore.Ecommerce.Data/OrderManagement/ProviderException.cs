// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProviderException.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the provider exception class.
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

namespace Sitecore.Ecommerce.Data.OrderManagement
{
  using System;
  using System.Collections.Generic;
  using System.Data.Entity.Validation;
  using System.Linq;
  using Sitecore.Diagnostics;

  /// <summary>
  /// Defines the provider exception class.
  /// </summary>
  [Serializable]
  public class ProviderException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ProviderException"/> class.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public ProviderException([CanBeNull]DbEntityValidationException exception)
      : base("Entity validation failed.", exception)
    {
      Assert.ArgumentNotNull(exception, "exception");
    }

    /// <summary>
    /// Gets the validation errors.
    /// </summary>
    [NotNull]
    public virtual IDictionary<string, string> ValidationErrors
    {
      get
      {
        DbEntityValidationException entityValidationException = this.InnerException as DbEntityValidationException;

        if (entityValidationException != null)
        {
          return entityValidationException.EntityValidationErrors
            .SelectMany(validationErrorContainer => validationErrorContainer.ValidationErrors, (validationErrorContainer, validationError) => validationError)
            .ToDictionary(validationError => validationError.PropertyName, validationError => validationError.ErrorMessage);
        }

        return new Dictionary<string, string>();
      }
    }
  }
}
