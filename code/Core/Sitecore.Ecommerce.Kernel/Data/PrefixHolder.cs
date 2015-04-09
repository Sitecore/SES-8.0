// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrefixHolder.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the PrefixHolder class.
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

namespace Sitecore.Ecommerce.Data
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  public sealed class PrefixHolder
  {
    private const char Delimiter = '.';

    private System.Text.StringBuilder prefix;
    private System.Collections.Generic.Stack<int> lengths;

    public PrefixHolder()
    {
      this.prefix = new System.Text.StringBuilder();
      this.lengths = new System.Collections.Generic.Stack<int>();
    }

    public void Clear()
    {
      this.prefix.Length = 0;
      this.lengths.Clear();
    }

    public void AppendToPrefix(string val)
    {
      lengths.Push(this.prefix.Length);

      if (this.prefix.Length != 0) this.prefix.Append(Delimiter);
      this.prefix.Append(val);
    }

    public void RemoveLastPart()
    {
      this.prefix.Length = lengths.Pop();
    }

    public string Prefix
    {
      get
      {
        return this.prefix.ToString();
      }
    }

    public override string ToString()
    {
      return this.Prefix;
    }
  }
}
