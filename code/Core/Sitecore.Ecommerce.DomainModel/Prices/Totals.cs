// -------------------------------------------------------------------------------------------
// <copyright file="Totals.cs" company="Sitecore Corporation">
//  Copyright (c) Sitecore Corporation 1999-2015 
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

namespace Sitecore.Ecommerce.DomainModel.Prices
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Runtime.Serialization;

  /// <summary>
  /// The product prices list abstract class.
  /// </summary>
  [Serializable, CollectionDataContract]
  public class Totals : IDictionary<string, decimal>
  {
    /// <summary>
    /// The totals dictionary.
    /// </summary>
    private readonly IDictionary<string, decimal> totals;

    /// <summary>
    /// Initializes a new instance of the <see cref="Totals"/> class.
    /// </summary>
    public Totals()
    {
      this.totals = new Dictionary<string, decimal>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Totals"/> class.
    /// </summary>
    /// <param name="totals">The totals.</param>
    public Totals(IDictionary<string, decimal> totals)
    {
      this.totals = totals;
    }
      
    /// <summary>
    /// Gets or sets the member price.
    /// </summary>
    /// <value>The member price.</value>
    public virtual decimal MemberPrice 
    { 
      get { return this["MemberPrice"]; }
      set { this["MemberPrice"] = value; }
    }

    /// <summary>
    /// Gets or sets the indirect tax.
    /// </summary>
    /// <value>The indirect tax.</value>
    public virtual decimal VAT
    {
      get { return this["VAT"]; }
      set { this["VAT"] = value; }
    }

    /// <summary>
    /// Gets or sets the total vat.
    /// </summary>
    /// <value>The total vat.</value>
    public virtual decimal TotalVat
    {
      get { return this["TotalVat"]; }
      set { this["TotalVat"] = value; }
    }

    /// <summary>
    /// Gets or sets the price inc vat.
    /// </summary>
    /// <value>The price inc vat.</value>
    public virtual decimal PriceIncVat
    {
      get { return this["PriceIncVat"]; }
      set { this["PriceIncVat"] = value; }
    }

    /// <summary>
    /// Gets or sets the price ex vat.
    /// </summary>
    /// <value>The price ex vat.</value>
    public virtual decimal PriceExVat
    {
      get { return this["PriceExVat"]; }
      set { this["PriceExVat"] = value; }
    }

    /// <summary>
    /// Gets or sets the discount inc vat.
    /// </summary>
    /// <value>The discount inc vat.</value>
    public virtual decimal DiscountIncVat
    {
      get { return this["DiscountIncVat"]; }
      set { this["DiscountIncVat"] = value; }
    }

    /// <summary>
    /// Gets or sets the discount ex vat.
    /// </summary>
    /// <value>The discount ex vat.</value>
    public virtual decimal DiscountExVat
    {
      get { return this["DiscountExVat"]; }
      set { this["DiscountExVat"] = value; }
    }

    /// <summary>
    /// Gets or sets the possible discount inc vat.
    /// </summary>
    /// <value>The possible discount inc vat.</value>
    public virtual decimal PossibleDiscountIncVat
    {
      get { return this["PossibleDiscountIncVat"]; }
      set { this["PossibleDiscountIncVat"] = value; }
    }

    /// <summary>
    /// Gets or sets the possible discount ex vat.
    /// </summary>
    /// <value>The possible discount ex vat.</value>
    public virtual decimal PossibleDiscountExVat
    {
      get { return this["PossibleDiscountExVat"]; }
      set { this["PossibleDiscountExVat"] = value; }
    }

    /// <summary>
    /// Gets or sets the total price ex vat.
    /// </summary>
    /// <value>The total price ex vat.</value>
    public virtual decimal TotalPriceExVat
    {
      get { return this["TotalPriceExVat"]; }
      set { this["TotalPriceExVat"] = value; }
    }

    /// <summary>
    /// Gets or sets the total price inc vat.
    /// </summary>
    /// <value>The total price inc vat.</value>
    public virtual decimal TotalPriceIncVat
    {
      get { return this["TotalPriceIncVat"]; }
      set { this["TotalPriceIncVat"] = value; }
    }

    #region Implementation of IEnumerable

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public virtual IEnumerator<KeyValuePair<string, decimal>> GetEnumerator()
    {
      return this.totals.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    #endregion

    #region Implementation of ICollection<KeyValuePair<string,decimal>>

    /// <summary>
    /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </summary>
    /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
    /// </exception>
    public virtual void Add(KeyValuePair<string, decimal> item)
    {
      this.totals.Add(item);
    }

    /// <summary>
    /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </summary>
    /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. 
    /// </exception>
    public virtual void Clear()
    {
      this.totals.Clear();
    }

    /// <summary>
    /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
    /// </summary>
    /// <returns>
    /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
    /// </returns>
    /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </param>
    public virtual bool Contains(KeyValuePair<string, decimal> item)
    {
      return this.totals.Contains(item);
    }

    /// <summary>
    /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.
    /// </exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.
    /// </exception>
    /// <exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.
    /// -or-
    /// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
    /// -or-
    /// The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
    /// -or-
    /// </exception>
    public virtual void CopyTo(KeyValuePair<string, decimal>[] array, int arrayIndex)
    {
      this.totals.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </summary>
    /// <returns>
    /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </returns>
    /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
    /// </exception>
    public virtual bool Remove(KeyValuePair<string, decimal> item)
    {
      return this.totals.Remove(item);
    }

    /// <summary>
    /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </summary>
    /// <value>The count.</value>
    /// <returns>
    /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </returns>
    public virtual int Count
    {
      get { return this.totals.Count; }
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
    /// </summary>
    /// <returns>
    /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
    /// </returns>
    public virtual bool IsReadOnly
    {
      get { return this.totals.IsReadOnly; }
    }

    #endregion

    #region Implementation of IDictionary<string,decimal>

    /// <summary>
    /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
    /// </summary>
    /// <returns>
    /// true if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key; otherwise, false.
    /// </returns>
    /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
    /// </param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.
    /// </exception>
    public virtual bool ContainsKey(string key)
    {
      return this.totals.ContainsKey(key);
    }

    /// <summary>
    /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
    /// </summary>
    /// <param name="key">The object to use as the key of the element to add.
    /// </param><param name="value">The object to use as the value of the element to add.
    /// </param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.
    /// </exception><exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
    /// </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.
    /// </exception>
    public virtual void Add(string key, decimal value)
    {
      this.totals.Add(key, value);
    }

    /// <summary>
    /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
    /// </summary>
    /// <returns>
    /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
    /// </returns>
    /// <param name="key">The key of the element to remove.
    /// </param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.
    /// </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.
    /// </exception>
    public virtual bool Remove(string key)
    {
      return this.totals.Remove(key);
    }

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <returns>
    /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key; otherwise, false.
    /// </returns>
    /// <param name="key">The key whose value to get.
    /// </param><param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.
    /// </param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.
    /// </exception>
    public virtual bool TryGetValue(string key, out decimal value)
    {
      return this.totals.TryGetValue(key, out value);
    }

    /// <summary>
    /// Gets or sets the element with the specified key.
    /// </summary>
    /// <returns>
    /// The element with the specified key.
    /// </returns>
    /// <param name="key">The key of the element to get or set.
    /// </param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.
    /// </exception><exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found.
    /// </exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.
    /// </exception>
    public virtual decimal this[string key]
    {
      get { return this.totals.ContainsKey(key) ? this.totals[key] : default(decimal); }
      set { this.totals[key] = value; }
    }

    /// <summary>
    /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
    /// </returns>
    public virtual ICollection<string> Keys
    {
      get { return this.totals.Keys; }
    }

    /// <summary>
    /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
    /// </returns>
    public virtual ICollection<decimal> Values
    {
      get { return this.totals.Values; }
    }

    #endregion
  }
}