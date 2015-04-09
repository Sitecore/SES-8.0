// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyConfiguration.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the PartyConfiguration class.
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

namespace Sitecore.Ecommerce.Data.ModelConfiguration
{
  using System.Data.Entity.ModelConfiguration;
  using Common;

  /// <summary>
  /// Defines the party configuration class.
  /// </summary>
  public class PartyConfiguration : EntityTypeConfiguration<Party>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="PartyConfiguration"/> class.
    /// </summary>
    public PartyConfiguration()
    {
      this.HasKey(o => o.Alias);
      this.Property(p => p.EndpointID).HasMaxLength(Constants.BigCodeFieldLength);
      this.Property(p => p.LanguageCode).HasMaxLength(Constants.LanguageCodeFieldLength);
      this.Property(p => p.LogoReferenceID).HasMaxLength(Constants.UriFieldLenght);
      this.Property(p => p.PartyIdentification).HasMaxLength(Constants.BigCodeFieldLength);
      this.Property(p => p.PartyName).HasMaxLength(Constants.NameFieldLength);
      this.Property(p => p.WebsiteURI).HasMaxLength(Constants.UriFieldLenght);
      this.Property(p => p.PartyName).HasMaxLength(Constants.NameFieldLength);
    }
  }
}