// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the Person class.
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

namespace Sitecore.Ecommerce.Common
{
  public class Person
  {
    /// <summary>
    /// Name for the person
    /// </summary>
    public virtual string FirstName { get; set; }

    /// <summary>
    /// Family name for the person ( surename
    /// </summary>
    public virtual string FamilyName { get; set; }

    /// <summary>
    /// Title for the person
    /// </summary>
    public virtual string Title { get; set; }

    /// <summary>
    /// Middle name for the person
    /// </summary>
    public virtual string MiddleName { get; set; }

    /// <summary>
    /// Suffix for the person
    /// </summary>
    public virtual string NameSuffix { get; set; }

    /// <summary>
    /// Job title for the person
    /// </summary>
    public virtual string JobTitle { get; set; }

    /// <summary>
    /// Department for the person
    /// </summary>
    public virtual string OrganizationDepartment { get; set; }
  }
}
