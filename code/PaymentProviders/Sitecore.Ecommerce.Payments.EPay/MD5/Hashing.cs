// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Hashing.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the Hashing class.
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

namespace Sitecore.Ecommerce.Paymens.EPay.MD5
{
  using System.Security.Cryptography;
  using System.Text;

  /// <summary>
  /// </summary>
  public class Hashing
  {
    /// <summary>
    /// Used to verify the hashed string received from Epay
    /// </summary>
    /// <param name="hashStringToVerify"></param>
    /// <param name="hashString"></param>
    /// <returns></returns>
    public static bool VerifyInboundHashing(string hashStringToVerify, string hashString)
    {
      // compare the recieved MD5Key on the stamp generated and if they are equal
      // the data received are valid
      if (hashStringToVerify.CompareTo(hashString) == 0)
      {
        return true;
      }
     
      return false;
    }

    /// <summary>
    /// Returnes the string as MD5 hashing
    /// </summary>
    /// <param name="datastr"></param>
    /// <returns></returns>
    public static string EncryptString(string datastr)
    {
      HashAlgorithm mhash = new MD5CryptoServiceProvider();
      string res = string.Empty; // the returning result

      // Convert the original string to array of Bytes
      byte[] bytValue = Encoding.UTF8.GetBytes(datastr);

      // Compute the Hash, returns an array of Bytes
      byte[] bytHash = mhash.ComputeHash(bytValue);

      mhash.Clear();

      // convert the byte data to hex string values
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
  }
}