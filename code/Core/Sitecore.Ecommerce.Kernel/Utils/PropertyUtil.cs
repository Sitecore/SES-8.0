// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyUtil.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the PropertyUtil type.
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

namespace Sitecore.Ecommerce.Utils
{
  using System;
  using System.Collections.Generic;
  using System.Data.SqlTypes;
  using System.Linq;
  using System.Reflection;
  using System.Web.UI;
  using Diagnostics;

  /// <summary>
  /// Definition of the PropertyUtil class.
  /// </summary>
  public static class PropertyUtil
  {
    /// <summary>
    /// List of dictionary types
    /// </summary>
    private static readonly Type[] DictionaryTypes = new[] { typeof(System.Collections.IDictionary), typeof(IDictionary<,>) };

    /// <summary>
    /// Gets the property value.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>The property value.</returns>
    /// <exception cref="ArgumentException">PropertyName argument is malformed.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><c>Index is out of range.</c> is out of range.</exception>
    [CanBeNull]
    public static object GetPropertyValue([NotNull] object obj, [NotNull] string propertyName)
    {
      Assert.ArgumentNotNull(obj, "obj");
      Assert.ArgumentNotNull(propertyName, "propertyName");

      if (propertyName == string.Empty)
      {
        return obj;
      }

      int startIndex = propertyName.IndexOf('[');
      if (startIndex >= 0)
      {
        int endIndex = propertyName.IndexOf(']', startIndex + 1);
        
        if (endIndex >= 0)
        {
          object valueObject = startIndex == 0 ? obj : DataBinder.Eval(obj, propertyName.Substring(0, startIndex));

          if (valueObject is System.Collections.IEnumerable)
          {
            bool flag = true;

            if (GetMethod(valueObject.GetType(), "get_Item") == null)
            {
              long index;

              if (long.TryParse(propertyName.Substring(startIndex + 1, endIndex - startIndex - 1), out index))
              {
                System.Collections.IEnumerator enumerator = ((System.Collections.IEnumerable)valueObject).GetEnumerator();

                while (index-- >= 0)
                {
                  enumerator.MoveNext();
                }

                try
                {
                  valueObject = enumerator.Current;
                }
                catch (InvalidOperationException e)
                {
                  throw new ArgumentOutOfRangeException("Index is out of range.", e);
                }
              }
              else
              {
                flag = false;
              }
            }
            else
            {
              valueObject = DataBinder.Eval(valueObject, propertyName.Substring(startIndex, endIndex - startIndex + 1));
            }

            if (flag)
            {
              string continuation = string.Empty;

              if (endIndex + 1 < propertyName.Length)
              {
                int offset = Convert.ToInt32(propertyName[endIndex + 1] == '.');
                continuation = propertyName.Substring(endIndex + offset + 1, propertyName.Length - endIndex - offset - 1);
              }

              return GetPropertyValue(valueObject, continuation);
            }
          }
        }
      }

      return DataBinder.Eval(obj, propertyName);
    }

    /// <summary>
    /// Ensures the property accessibility.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="propertyName">Name of the property.</param>
    public static void EnsurePropertyAccessibility([NotNull] object obj, [NotNull] string propertyName)
    {
      Assert.IsNotNull(obj, "obj");
      Assert.IsNotNullOrEmpty(propertyName, "propertyName");

      string[] propertyList = propertyName.Split('.');

      for (int index = 0; index < propertyList.Length - 1; ++index)
      {
        PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyList[index]);

        object val = propertyInfo.GetValue(obj, null);

        if (val == null)
        {
          if (!propertyInfo.CanWrite)
          {
            break;
          }

          val = CreateObjectDeep(propertyInfo.PropertyType);
          propertyInfo.SetValue(obj, val, null);
        }

        obj = val;
      }
    }

    /// <summary>
    /// Gets the different fields.
    /// </summary>
    /// <param name="obj1">The obj1.</param>
    /// <param name="obj2">The obj2.</param>
    /// <param name="overrides">The overrides.</param>
    /// <returns>The different fields.</returns>
    public static IEnumerable<string> GetDifferentFields(object obj1, object obj2, params KeyValuePair<KeyValuePair<Type, Type>, Func<object, object, IEnumerable<string>>>[] overrides)
    {
      return GetDifferentFields(obj1, obj2, false, overrides);
    }

    /// <summary>
    /// Gets list of names of different fields.
    /// </summary>
    /// <param name="obj1">First object to compare.</param>
    /// <param name="obj2">Second object to compare.</param>
    /// <param name="compareMatchingPropertiesOnly">Compares matching properties only if set to <c>true</c>.</param>
    /// <param name="overrides">Comparers that can be used for overriding comparison logic.</param>
    /// <returns>List of names of different fields.</returns>
    public static IEnumerable<string> GetDifferentFields(object obj1, object obj2, bool compareMatchingPropertiesOnly, params KeyValuePair<KeyValuePair<Type, Type>, Func<object, object, IEnumerable<string>>>[] overrides)
    {
      return GetDifferentFieldsInternal(obj1, obj2, compareMatchingPropertiesOnly, new HashSet<KeyValuePair<object, object>>(), overrides);
    }

    /// <summary>
    /// Creates the object deep.
    /// </summary>
    /// <param name="type">The type.</param>
    private static object CreateObjectDeep(Type type)
    {
      if (type.IsAbstract)
      {
        return null;
      }

      object result = Activator.CreateInstance(type);

      foreach (PropertyInfo propertyInfo in type.GetProperties())
      {
        if (propertyInfo.PropertyType.Assembly == type.Assembly)
        {
          object val = propertyInfo.GetValue(result);

          if (val == null)
          {
            if (!propertyInfo.CanWrite)
            {
              break;
            }

            val = CreateObjectDeep(propertyInfo.PropertyType);
            propertyInfo.SetValue(result, val);
          }
        }
        else if (propertyInfo.PropertyType == typeof(DateTime))
        {
          propertyInfo.SetValue(result, SqlDateTime.MinValue.Value);
        }
      }

      return result;
    }

    /// <summary>
    /// Gets the different fields internal.
    /// </summary>
    /// <param name="obj1">The obj1.</param>
    /// <param name="obj2">The obj2.</param>
    /// <param name="compareMatchingPropertiesOnly">Compares matching properties only if set to <c>true</c>.</param>
    /// <param name="visits">The visits.</param>
    /// <param name="overrides">The overrides.</param>
    /// <returns>The different fields internal.</returns>
    private static IEnumerable<string> GetDifferentFieldsInternal(object obj1, object obj2, bool compareMatchingPropertiesOnly, HashSet<KeyValuePair<object, object>> visits, params KeyValuePair<KeyValuePair<Type, Type>, Func<object, object, IEnumerable<string>>>[] overrides)
    {
      if (ReferenceEquals(obj1, obj2))
      {
        return Enumerable.Empty<string>();
      }

      if ((obj1 == null) || (obj2 == null))
      {
        return new[] { string.Empty };
      }

      Func<object, object, IEnumerable<string>> customComparer = overrides.Where(pair => pair.Key.Key.IsAssignableFrom(obj1.GetType()) && pair.Key.Value.IsAssignableFrom(obj2.GetType())).Select(pair => pair.Value).FirstOrDefault();

      if (customComparer != null)
      {
        IEnumerable<string> customComparerResult = customComparer(obj1, obj2);
        if (customComparerResult != null)
        {
          return customComparerResult;
        }
      }

      MethodBase equalsMethod = obj1.GetType().GetMethod("Equals", new[] { typeof(object) });

      Assert.IsNotNull(equalsMethod, "No equals method");

      if (obj1.GetType().IsValueType || (equalsMethod.DeclaringType == equalsMethod.ReflectedType))
      {
        return obj1.Equals(obj2) ? Enumerable.Empty<string>() : new[] { string.Empty };
      }

      KeyValuePair<object, object> objectPair = new KeyValuePair<object, object>(obj1, obj2);

      if (visits.Contains(objectPair))
      {
        return Enumerable.Empty<string>();
      }

      visits.Add(objectPair);

      if (ImplementsInterfaces(obj1, DictionaryTypes) && ImplementsInterfaces(obj2, DictionaryTypes))
      {
        return GetDifferentDictionaryElements(obj1, obj2, compareMatchingPropertiesOnly, visits, overrides);
      }

      if (ImplementsInterfaces(obj1, DictionaryTypes) ^ ImplementsInterfaces(obj2, DictionaryTypes))
      {
        return new[] { string.Empty };
      }

      if ((obj1 is System.Collections.IEnumerable) && (obj2 is System.Collections.IEnumerable))
      {
        return GetDifferentSequenceElements((System.Collections.IEnumerable)obj1, (System.Collections.IEnumerable)obj2, compareMatchingPropertiesOnly, visits, overrides);
      }

      if ((obj1 is System.Collections.IEnumerable) ^ (obj2 is System.Collections.IEnumerable))
      {
        return new[] { string.Empty };
      }

      return GetDifferentMemberFields(obj1, obj2, compareMatchingPropertiesOnly, visits, overrides);
    }

    /// <summary>
    /// Tests whether the object is dictionary.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <param name="interfaces">List of the interfaces.</param>
    /// <returns>True if the object is dictionary otherwise false.</returns>
    private static bool ImplementsInterfaces(object obj, IEnumerable<Type> interfaces)
    {
      Type objType = obj.GetType();

      return interfaces.Any(type => objType.GetInterface(type.FullName) != null);
    }

    /// <summary>
    /// Gets type method.
    /// </summary>
    /// <param name="objType">The type of the object.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <returns>The method with the given name</returns>
    private static MethodBase GetMethod(Type objType, string methodName)
    {
      MethodBase result = objType.GetMethod(methodName);

      if (result != null)
      {
        return result;
      }

      Type[] implementedInterfaces = objType.GetInterfaces();

      foreach (Type type in implementedInterfaces)
      {
        if (!(objType.IsArray && type.IsGenericType))
        {
          string methodNameToFind = string.Format("{0}.{1}", type.FullName, methodName);

          foreach (MethodInfo method in objType.GetInterfaceMap(type).TargetMethods)
          {
            if (method.Name == methodNameToFind)
            {
              return method;
            }
          }
        }
      }

      return null;
    }

    /// <summary>
    /// Appends nested fields information to the existing information about field.
    /// </summary>
    /// <param name="initialInfo">Initial field information.</param>
    /// <param name="infoToAppend">Information to append.</param>
    /// <returns>The new field information.</returns>
    private static string AppendFieldInformation(string initialInfo, string infoToAppend)
    {
      if (string.IsNullOrEmpty(infoToAppend))
      {
        return initialInfo;
      }

      if (infoToAppend.StartsWith("["))
      {
        return initialInfo + infoToAppend;
      }
        
      return string.Format("{0}.{1}", initialInfo, infoToAppend);
    }

    /// <summary>
    /// Keys to indexer string.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>The to indexer string.</returns>
    private static string KeyToIndexerString(object key)
    {
      if (key is string)
      {
        return string.Format("[\"{0}\"]", key);
      }

      return string.Format("[{0}]", key);
    }

    /// <summary>
    /// Compares two dictionary objects
    /// </summary>
    /// <param name="obj1">The first dictionary</param>
    /// <param name="obj2">The second dictionary</param>
    /// <param name="compareMatchingPropertiesOnly">Compares matching properties only if set to <c>true</c>.</param>
    /// <param name="visits">The visits.</param>
    /// <param name="overrides">The overrides.</param>
    /// <returns>List of indices</returns>
    private static IEnumerable<string> GetDifferentDictionaryElements(object obj1, object obj2, bool compareMatchingPropertiesOnly, HashSet<KeyValuePair<object, object>> visits, KeyValuePair<KeyValuePair<Type, Type>, Func<object, object, IEnumerable<string>>>[] overrides)
    {
      HashSet<string> result = new HashSet<string>();

      IEnumerable<object> keys1 = ((System.Collections.IEnumerable)GetMethod(obj1.GetType(), "get_Keys").Invoke(obj1, null)).Cast<object>();
      IEnumerable<object> keys2 = ((System.Collections.IEnumerable)GetMethod(obj2.GetType(), "get_Keys").Invoke(obj2, null)).Cast<object>();

      foreach (object key in keys1)
      {
        string val = KeyToIndexerString(key);

        Assert.IsFalse(result.Contains(val), "keys are not unique");

        result.Add(val);
      }

      foreach (object key in keys2)
      {
        string val = KeyToIndexerString(key);
        
        if (!result.Contains(val))
        {
          result.Add(val);
        }
      }

      MethodBase indexer1 = GetMethod(obj1.GetType(), "get_Item");
      MethodBase indexer2 = GetMethod(obj2.GetType(), "get_Item");

      foreach (var pair in keys1.Join(keys2, key => key, key => key, (key1, key2) => new { First = key1, Second = key2 }))
      {
        object val1 = indexer1.Invoke(obj1, new[] { pair.First });
        object val2 = indexer2.Invoke(obj2, new[] { pair.Second });
        string currentKeyRepresentation = KeyToIndexerString(pair.First);

        result.Remove(currentKeyRepresentation);

        foreach (string field in GetDifferentFieldsInternal(val1, val2, compareMatchingPropertiesOnly, visits, overrides))
        {
          result.Add(AppendFieldInformation(currentKeyRepresentation, field));
        }
      }

      return result;
    }

    /// <summary>
    /// Compares two list objects
    /// </summary>
    /// <param name="obj1">The first list</param>
    /// <param name="obj2">The second list</param>
    /// <param name="compareMatchingPropertiesOnly">Compares matching properties only if set to <c>true</c>.</param>
    /// <param name="visits">The visits.</param>
    /// <param name="overrides">The overrides.</param>
    /// <returns>List of indices</returns>
    private static IEnumerable<string> GetDifferentSequenceElements(System.Collections.IEnumerable obj1, System.Collections.IEnumerable obj2, bool compareMatchingPropertiesOnly, HashSet<KeyValuePair<object, object>> visits, KeyValuePair<KeyValuePair<Type, Type>, Func<object, object, IEnumerable<string>>>[] overrides)
    {
      HashSet<string> result = new HashSet<string>();

      System.Collections.IEnumerator enumerator1 = obj1.GetEnumerator();
      System.Collections.IEnumerator enumerator2 = obj2.GetEnumerator();
      bool flag1 = enumerator1.MoveNext();
      bool flag2 = enumerator2.MoveNext();
      int index = 0;

      while (flag1 && flag2)
      {
        foreach (string field in GetDifferentFieldsInternal(enumerator1.Current, enumerator2.Current, compareMatchingPropertiesOnly, visits, overrides))
        {
          result.Add(AppendFieldInformation(KeyToIndexerString(index), field));
        }

        flag1 = enumerator1.MoveNext();
        flag2 = enumerator2.MoveNext();
        ++index;
      }

      if (flag1 || flag2)
      {
        do
        {
          result.Add(KeyToIndexerString(index++));
        }
        while ((flag1 && enumerator1.MoveNext()) || (flag2 && enumerator2.MoveNext()));
      }

      return result;
    }

    /// <summary>
    /// Compares objects memberwise
    /// </summary>
    /// <param name="obj1">First object to compare</param>
    /// <param name="obj2">Second object to compare</param>
    /// <param name="compareMatchingPropertiesOnly">Compares matching properties only if set to <c>true</c>.</param>
    /// <param name="visits">The visits.</param>
    /// <param name="overrides">The overrides.</param>
    /// <returns>List of names of different fields.</returns>
    private static IEnumerable<string> GetDifferentMemberFields(object obj1, object obj2, bool compareMatchingPropertiesOnly, HashSet<KeyValuePair<object, object>> visits, KeyValuePair<KeyValuePair<Type, Type>, Func<object, object, IEnumerable<string>>>[] overrides)
    {
      HashSet<string> result = new HashSet<string>();

      IEnumerable<PropertyInfo> obj1Properties = obj1.GetType().GetProperties().Where(property => (property.GetIndexParameters().Length == 0) && property.CanRead && property.CanWrite);
      IEnumerable<PropertyInfo> obj2Properties = obj2.GetType().GetProperties().Where(property => (property.GetIndexParameters().Length == 0) && property.CanRead && property.CanWrite);

      if (!compareMatchingPropertiesOnly)
      {
        foreach (PropertyInfo property in obj1Properties)
        {
          Assert.IsFalse(result.Contains(property.Name), "duplicated property");

          result.Add(property.Name);
        }

        foreach (PropertyInfo property in obj2Properties)
        {
          if (!result.Contains(property.Name))
          {
            result.Add(property.Name);
          }
        }
      }

      foreach (var pair in obj1Properties.Join(obj2Properties, propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.Name, (property1, property2) => new { First = property1, Second = property2 }))
      {
        if (pair.First.PropertyType == pair.Second.PropertyType)
        {
          object val1 = pair.First.GetValue(obj1, null);
          object val2 = pair.Second.GetValue(obj2, null);

          if (!compareMatchingPropertiesOnly)
          {
            result.Remove(pair.First.Name);
          }

          foreach (string field in GetDifferentFieldsInternal(val1, val2, compareMatchingPropertiesOnly, visits, overrides))
          {
            result.Add(AppendFieldInformation(pair.First.Name, field));
          }
        }
      }

      return result;
    }
  }
}