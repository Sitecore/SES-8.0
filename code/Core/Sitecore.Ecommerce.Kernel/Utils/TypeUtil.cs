// -------------------------------------------------------------------------------------------
// <copyright file="TypeUtil.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Utils
{
  using System;
  using System.ComponentModel;
  using System.Globalization;
  using Diagnostics;
  using DomainModel.Data;
 
  /// <summary>
  /// The type util helper class.
  /// </summary>
  public static class TypeUtil
  {
    /// <summary>
    /// Tries the parse.
    /// </summary>
    /// <typeparam name="T">The output type.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The typed specified value.</returns>
    public static T TryParse<T>(object value, T defaultValue)
    {
      try
      {
        return Parse<T>(value);
      }
      catch (Exception exception)
      {
        Log.Warn(exception.Message, exception);
        return defaultValue;
      }
    }

    /// <summary>
    /// Parses the specified value.
    /// </summary>
    /// <typeparam name="T">The output type.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns>The typed specified value.</returns>
    public static T Parse<T>(object value)
    {
      return (T)Parse(value, typeof(T));
    }

    /// <summary>
    /// Parses the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="type">The output type.</param>
    /// <returns>The typed specified value.</returns>
    public static object Parse(object value, Type type)
    {
      if (value == null)
      {
        if (type == typeof(string))
        {
          return null;
        }

        try
        {
          return Activator.CreateInstance(type);
        }
        catch
        {
          return null;
        }
      }

      if (value is IEntity)
      {
        return ((IEntity)value).Alias;
      }

      if (value.GetType().IsSubclassOf(type) || value.GetType() == type)
      {
        return value;
      }

      if (type.Name == "Nullable`1")
      {
        type = Nullable.GetUnderlyingType(type);
      }

      if (value is string)
      {
        if (string.IsNullOrEmpty((string)value) && type.IsSubclassOf(typeof(ValueType)))
        {
          return Activator.CreateInstance(type);
        }
      }

      TypeConverter typeConverter = TypeDescriptor.GetConverter(type);
      if (typeConverter != null)
      {
        if (typeConverter.CanConvertFrom(value.GetType()) && typeConverter.CanConvertTo(type))
        {
          return typeConverter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
        }
      }

      if (type.BaseType != null && type.BaseType.Name == "Enum")
      {
        return Enum.Parse(type, value.ToString().Replace(" ", string.Empty));
      }

      if (type.Name == "Guid")
      {
        return new Guid(value.ToString());
      }

      if (value is IConvertible)
      {
        return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
      }

      return null;
    }
  }
}