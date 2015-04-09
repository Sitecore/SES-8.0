// -------------------------------------------------------------------------------------------
// <copyright file="Crypto.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce
{
  using System;
  using System.Collections.Generic;
  using System.Security.Cryptography;
  using System.Text;

  /// <summary>
  /// Simple Cryptographic Services
  /// </summary>
  public static class Crypto
  {
    /// <summary>
    /// The offset.
    /// </summary>
    private static int offsett = 375;

    /// <summary>
    /// Gets MD5 hash of string.
    /// </summary>
    /// <param name="datastr">The data string.</param>
    /// <returns>The MD5 hash.</returns>
    public static string GetMD5Hash(string datastr)
    {
      HashAlgorithm mhash = new MD5CryptoServiceProvider();
      string res = string.Empty;

      byte[] bytValue = Encoding.UTF8.GetBytes(datastr);

      byte[] bytHash = mhash.ComputeHash(bytValue);

      mhash.Clear();

      for (int i = 0; i < bytHash.Length; i++)
      {
        if (bytHash[i] < 16)
        {
          res += "0" + bytHash[i].ToString("x");
        }
        else
        {
          res += bytHash[i].ToString("x");
        }
      }

      return res;
    }

    /// <summary>
    /// Encrypts text with Triple DES encryption using the supplied key
    /// </summary>
    /// <param name="plaintext">
    /// The text to encrypt
    /// </param>
    /// <param name="key">
    /// Key to use for encryption
    /// </param>
    /// <returns>
    /// The encrypted string represented as base 64 text
    /// </returns>
    public static string EncryptTripleDES(string plaintext, string key)
    {
      var des =
        new TripleDESCryptoServiceProvider();
      var hashMD5 = new MD5CryptoServiceProvider();
      des.Key = hashMD5.ComputeHash(Encoding.ASCII.GetBytes(key));
      des.Mode = CipherMode.ECB;
      ICryptoTransform desEncrypt = des.CreateEncryptor();
      byte[] buffer = Encoding.ASCII.GetBytes(plaintext);
      string txt = Convert.ToBase64String(
        desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));

      var charArr = new List<char>();
      var rnd = new Random();
      int r = rnd.Next(10, 99);

      foreach (char chr in txt)
      {
        charArr.Add(chr);
      }

      for (int i = 0; i < r; i++)
      {
        int removeAt = i % (charArr.Count - 1);
        int insertAt = (i + offsett) % (charArr.Count - 2);

        char chrVal = charArr[removeAt];
        charArr.RemoveAt(removeAt);
        charArr.Insert(insertAt, chrVal);
      }

      char[] values = charArr.ToArray();
      string retval = string.Empty;
      foreach (char chr in values)
      {
        retval += chr.ToString();
      }

      return r + retval;
    }

    /// <summary>
    /// Decrypts supplied Triple DES encrypted text using the supplied key
    /// </summary>
    /// <param name="base64Text">
    /// Triple DES encrypted base64 text
    /// </param>
    /// <param name="key">
    /// Decryption Key
    /// </param>
    /// <returns>
    /// The decrypted string
    /// </returns>
    public static string DecryptTripleDES(string base64Text, string key)
    {
      int r = Convert.ToInt32(base64Text.Substring(0, 2), Sitecore.Context.Culture);
      var charArr = new List<char>();
      string txt = base64Text.Remove(0, 2); // .Substring(2, base64Text.Length - 2);

// return txt;
      foreach (char chr in txt)
      {
        charArr.Add(chr);
      }

      for (int i = r - 1; i >= 0; i--)
      {
        int insertAt = i % (charArr.Count - 1);
        int removeAt = (i + offsett) % (charArr.Count - 2);

        char chrVal = charArr[removeAt];
        charArr.RemoveAt(removeAt);
        charArr.Insert(insertAt, chrVal);
      }

      char[] values = charArr.ToArray();
      base64Text = string.Empty;
      foreach (char chr in values)
      {
        base64Text += chr.ToString();
      }

      var des = new TripleDESCryptoServiceProvider();
      var hashMD5 = new MD5CryptoServiceProvider();
      des.Key = hashMD5.ComputeHash(Encoding.ASCII.GetBytes(key));
      des.Mode = CipherMode.ECB;
      ICryptoTransform desDecrypt = des.CreateDecryptor();
      byte[] buffer = Convert.FromBase64String(base64Text);
      return Encoding.ASCII.GetString(
        desDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
    }
  }
}