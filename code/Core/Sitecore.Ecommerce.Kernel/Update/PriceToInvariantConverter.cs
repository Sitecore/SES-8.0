// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PriceToInvariantConverter.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the PriceToInvariantConverter class.
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

namespace Sitecore.Ecommerce.Update
{
  using System.Globalization;

  public class PriceToInvariantConverter
  {
    public bool Convert(ref string value)
    {
      if (value.Split('.').Length == 2 && value.IndexOf(',') == -1)
      {
        return false;
      }

      decimal result;
      bool succeeded = decimal.TryParse(value, NumberStyles.Any, NumberFormatInfo.CurrentInfo, out result);

      if (succeeded)
      {
        value = result.ToString(NumberFormatInfo.InvariantInfo);
      }

      return succeeded;
    }
  }
}
