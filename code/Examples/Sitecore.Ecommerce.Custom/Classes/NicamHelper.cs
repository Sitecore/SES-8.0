// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NicamHelper.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the NicamHelper class.
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

namespace Sitecore.Ecommerce.Classes
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Linq;
  using System.Text;
  using System.Web;
  using System.Web.Security;
  using System.Web.UI.WebControls;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Globalization;
  using Publishing;
  using SecurityModel;
  using Sitecore.Security;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;
  using Xml.Xsl;

  /// <summary>
  /// </summary>
  public static class NicamHelper
  {
    /// <summary>
    /// Gets the logged in status for the current user
    /// </summary>
    public static bool IsLoggedIn
    {
      get
      {
        User user = Sitecore.Context.User;
        return
          user != null
          && !Sitecore.Context.User.Name.Equals(GetValidUserInDomain("Anonymous"))
          && user.IsInRole(Consts.ExtranetRole)
          && user.GetDomainName().Equals(Sitecore.Context.Domain.Name);
      }
    }

    /// <summary>
    /// Gets the title of the current context item in HTML
    /// </summary>
    /// <returns>
    /// </returns>
    public static string GetTitle()
    {
      Item itmCurrent = Sitecore.Context.Item;
      if (itmCurrent != null)
      {
        if (itmCurrent.Fields[Consts.MenuTitleFieldname] != null &&
            !string.IsNullOrEmpty(itmCurrent.Fields[Consts.MenuTitleFieldname].Value))
        {
          return HtmlEncode(itmCurrent.Fields[Consts.MenuTitleFieldname].Value);
        }

        return HtmlEncode(itmCurrent.DisplayName);
      }

      return "Ecommerce";
    }

    /// <summary>
    /// Gets a HTML encoded string
    /// </summary>
    /// <param name="str">
    /// </param>
    /// <returns>
    /// </returns>
    public static string HtmlEncode(string str)
    {
      if (string.IsNullOrEmpty(str))
      {
        return string.Empty;
      }

      return HttpUtility.HtmlEncode(str);
    }

    /// <summary>
    /// </summary>
    /// <param name="rootPath">
    /// </param>
    /// <param name="fieldname">
    /// </param>
    /// <param name="emptyItem">
    /// </param>
    /// <returns>
    /// </returns>
    public static IEnumerable<KeyValuePair<string, string>> GetList(string rootPath, string fieldname,
                                                                    bool emptyItem)
    {
      Item root = Sitecore.Context.Database.GetItem(rootPath);
      if (root != null)
      {
        if (emptyItem)
        {
          yield return new KeyValuePair<string, string>(string.Empty, " - ");
        }

        foreach (Item itm in root.Children)
        {
          string key = string.IsNullOrEmpty(fieldname) ? itm.DisplayName : itm[fieldname];
          yield return new KeyValuePair<string, string>(itm.ID.ToString(), key);
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="rootPath">
    /// </param>
    /// <param name="fieldname">
    /// </param>
    /// <returns>
    /// </returns>
    public static IEnumerable<KeyValuePair<string, string>> GetList(string rootPath, string fieldname)
    {
      return GetList(rootPath, fieldname, false);
    }

    /// <summary>
    /// </summary>
    /// <param name="rootPath">
    /// </param>
    /// <returns>
    /// </returns>
    public static IEnumerable<KeyValuePair<string, string>> GetList(string rootPath)
    {
      return GetList(rootPath, null);
    }

    /// <summary>
    /// </summary>
    /// <param name="listItems">
    /// </param>
    /// <returns>
    /// </returns>
    public static string ComposeSitecoreList(ListItemCollection listItems)
    {
      string selctedItems = string.Empty;
      foreach (ListItem listItem in listItems)
      {
        if (listItem.Selected)
        {
          selctedItems += "|" + listItem.Value;
        }
      }

      if (!string.IsNullOrEmpty(selctedItems))
      {
        selctedItems = selctedItems.Substring(1);
      }

      return selctedItems;
    }

    /// <summary>
    /// </summary>
    /// <param name="userOptions">
    /// </param>
    /// <param name="userProfile">
    /// </param>
    public static void RegisterUser(StringDictionary userOptions, StringDictionary userProfile)
    {
      User newUser = User.Create(userOptions["UserName"], userOptions["Password"]);
      newUser.Roles.Add(Role.FromName(userOptions["Roles"]));

      UserProfile profile = newUser.Profile;
      using (new SecurityDisabler())
      {
        Item item = Client.CoreDatabase.GetItem(userOptions["UserProfile"]);
        profile.ProfileItemId = item.ID.ToString();
        profile.FullName = userOptions["FullName"];
        profile.Email = userOptions["Email"];
        profile.Comment = string.Empty;
        profile.Portrait = userOptions["Portrait"];

        foreach (string key in userProfile.Keys)
        {
          profile.SetCustomProperty(key, userProfile[key]);
        }

        profile.Save();
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="user">
    /// </param>
    /// <param name="userOptions">
    /// </param>
    /// <param name="userProfile">
    /// </param>
    public static void UpdateUserProfile(User user, StringDictionary userOptions, StringDictionary userProfile)
    {
      if (User.Exists(user.Name))
      {
        UserProfile profile = user.Profile;
        using (new SecurityDisabler())
        {
          Item item = Client.CoreDatabase.GetItem(userOptions["UserProfile"]);
          profile.ProfileItemId = item.ID.ToString();
          profile.FullName = userOptions["FullName"];
          profile.Email = userOptions["Email"];
          profile.Comment = string.Empty;
          profile.Portrait = userOptions["Portrait"];
          foreach (string key in userProfile.Keys)
          {
            profile.SetCustomProperty(key, userProfile[key]);
          }

          profile.Save();
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="user">
    /// </param>
    /// <param name="oldPassword">
    /// </param>
    /// <param name="newPassword">
    /// </param>
    /// <returns>
    /// </returns>
    public static bool ChangeUserPassword(User user, string oldPassword, string newPassword)
    {
      MembershipUser memeberShipUser = Membership.GetUser(user.Name);
      return memeberShipUser.ChangePassword(oldPassword, newPassword);
    }

    /// <summary>
    /// </summary>
    /// <param name="userName">
    /// </param>
    /// <returns>
    /// </returns>
    public static string GetValidUserInDomain(string userName)
    {
      return string.Concat(Sitecore.Context.Domain.Name, @"\", userName);
    }

    /// <summary>
    /// </summary>
    /// <param name="userName">
    /// </param>
    /// <param name="password">
    /// </param>
    /// <returns>
    /// </returns>
    public static bool Login(string userName, string password)
    {
      try
      {
        return AuthenticationManager.Login(userName, password);
      }
      catch
      {
        return false;
      }
    }

    /// <summary>
    /// </summary>
    public static void Logout()
    {
      AuthenticationManager.Logout();
    }

    /// <summary>
    /// </summary>
    /// <param name="list">
    /// </param>
    /// <param name="rootPath">
    /// </param>
    /// <param name="fieldName">
    /// </param>
    /// <param name="selectedValue">
    /// </param>
    /// <param name="selectedText">
    /// </param>
    /// <param name="emptyItem">
    /// </param>
    public static void BindToListControl(ListControl list, string rootPath, string fieldName, string selectedValue,
                                         string selectedText, bool emptyItem)
    {
      IEnumerable<KeyValuePair<string, string>> listDataSource = GetList(rootPath, fieldName, emptyItem);
      BindToListControl(list, listDataSource, selectedValue, selectedText);
    }

    /// <summary>
    /// </summary>
    /// <param name="list">
    /// </param>
    /// <param name="listDataSource">
    /// </param>
    /// <param name="selectedValue">
    /// </param>
    /// <param name="selectedText">
    /// </param>
    public static void BindToListControl(ListControl list, IEnumerable<KeyValuePair<string, string>> listDataSource,
                                         string selectedValue, string selectedText)
    {
      list.Items.Clear();
      list.DataSource = listDataSource;
      list.DataValueField = "Key";
      list.DataTextField = "Value";
      list.DataBind();
      if (!string.IsNullOrEmpty(selectedValue))
      {
        string[] selectedValues = selectedValue.Split('|');
        foreach (string val in selectedValues)
        {
          if (!string.IsNullOrEmpty(val))
          {
            ListItem selctedItem = list.Items.FindByValue(val);
            if (selctedItem != null)
            {
              selctedItem.Selected = true;
            }
          }
        }

        return;
      }

      if (!string.IsNullOrEmpty(selectedText))
      {
        ListItem selectedItem = list.Items.FindByText(selectedText);
        if (selectedItem != null)
        {
          selectedItem.Selected = true;
        }

        return;
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="list">
    /// </param>
    /// <param name="rootPath">
    /// </param>
    /// <param name="fieldName">
    /// </param>
    /// <param name="selectedValue">
    /// </param>
    /// <param name="selectedText">
    /// </param>
    public static void BindToListControl(ListControl list, string rootPath, string fieldName, string selectedValue,
                                         string selectedText)
    {
      BindToListControl(list, rootPath, fieldName, selectedValue, selectedText, false);
    }

    /// <summary>
    /// </summary>
    /// <param name="name">
    /// </param>
    /// <returns>
    /// </returns>
    public static string SafeRequest(string name)
    {
      return HttpContext.Current.Request[name] ?? string.Empty;
    }

    /// <summary>
    /// </summary>
    /// <param name="name">
    /// </param>
    /// <returns>
    /// </returns>
    public static string SafeRequestForm(string name)
    {
      return HttpContext.Current.Request.Form[name] ?? string.Empty;
    }

    /// <summary>
    /// </summary>
    /// <param name="name">
    /// </param>
    /// <returns>
    /// </returns>
    public static string SafeRequestQs(string name)
    {
      return HttpContext.Current.Request.QueryString[name] ?? string.Empty;
    }

    /// <summary>
    /// </summary>
    /// <param name="name">
    /// </param>
    /// <returns>
    /// </returns>
    public static string SafeRequestSession(string name)
    {
      return HttpContext.Current.Session[name] != null
               ? HttpContext.Current.Session[name].ToString()
               : string.Empty;
    }

    /// <summary>
    /// </summary>
    /// <param name="url">
    /// </param>
    /// <returns>
    /// </returns>
    public static string UrlEncode(string url)
    {
      return url != null ? HttpUtility.UrlEncode(url) : string.Empty;
    }

    /// <summary>
    /// </summary>
    /// <param name="url">
    /// </param>
    /// <returns>
    /// </returns>
    public static string UrlDecode(string url)
    {
      return url != null ? HttpUtility.UrlDecode(url) : string.Empty;
    }

    /// <summary>
    /// </summary>
    /// <param name="page">
    /// </param>
    /// <param name="qsParameter">
    /// </param>
    /// <param name="qs">
    /// </param>
    /// <returns>
    /// </returns>
    public static string RedirectUrl(string page, string qsParameter, string qs)
    {
      string url = UrlEncode(qs);
      url = string.Concat(page, "?", qsParameter, "=", url);
      return url;
    }

    /// <summary>
    /// </summary>
    /// <param name="reviewTitle">
    /// </param>
    /// <param name="reviewText">
    /// </param>
    /// <param name="reviewRate">
    /// </param>
    public static void AddReview(string reviewTitle, string reviewText, string reviewRate)
    {
      using (new SecurityDisabler())
      {
        Item currentItm = Sitecore.Context.Item;
        Database masterDb = Factory.GetDatabase("master");
        if (currentItm != null && masterDb != null)
        {
          // ------------------------------------------
          // validation
          // ------------------------------------------
          bool reviewValidate = false;
          if (!reviewTitle.Equals(string.Empty) &&
              !reviewText.Equals(string.Empty) &&
              !reviewRate.Equals(string.Empty))
          {
            reviewValidate = true;
          }

          if (reviewValidate)
          {
            Item reviewsFolderItm =
              masterDb.Items[string.Concat(currentItm.Paths.FullPath, "/", Consts.ReviewFolderName)];
            Database webDB = Factory.GetDatabase("web");
            Item reviewsFolderItmOnWeb =
              webDB.Items[string.Concat(currentItm.Paths.FullPath, "/", Consts.ReviewFolderName)];
            if (reviewsFolderItm == null)
            {
              if (reviewsFolderItmOnWeb != null)
              {
                reviewsFolderItmOnWeb.Delete();
              }

              string templatePath = string.Concat(Consts.TemplatesPath, "/",
                                                  Consts.ReviewFolderTemplateName);
              TemplateItem reviewsFolderTemplate = masterDb.Templates[templatePath];
              Item curItmInMasterDb = masterDb.Items[currentItm.Paths.FullPath];
              reviewsFolderItm = curItmInMasterDb.Add("Reviews", reviewsFolderTemplate);
            }
            else
            {
              if (reviewsFolderItmOnWeb != null)
              {
                if (!reviewsFolderItmOnWeb.ID.ToString().Equals(reviewsFolderItm.ID.ToString()))
                {
                  reviewsFolderItmOnWeb.Delete();
                }
              }
            }

            if (reviewsFolderItm != null)
            {
              // ------------------------------------------
              // create name from date and time
              // ------------------------------------------
              string reviewName = DateUtil.ToIsoDate(DateTime.Now, false);

              // ------------------------------------------
              // create item from template
              // ------------------------------------------
              string templatePath = string.Concat(Consts.TemplatesPath, "/", Consts.ReviewItemTemplateName);
              TemplateItem reviewTemplate = masterDb.Templates[templatePath];
              if (reviewTemplate != null)
              {
                Item itm = reviewsFolderItm.Add(reviewName, reviewTemplate);

                // ------------------------------------------
                // review item fields
                // ------------------------------------------
                using (new EditContext(itm, true, false))
                {
                  itm.Fields["Title"].Value = reviewTitle;
                  itm.Fields["Description"].Value = reviewText;
                  itm.Fields["Rate"].Value = reviewRate;
                }

                itm.Locking.Unlock();

                // ------------------------------------------
                // publish item
                // ------------------------------------------
                PublishOneItem(itm.Parent);
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="item">
    /// </param>
    private static void PublishOneItem(Item item)
    {
      Language lang = Sitecore.Context.Language;
      Database masterDb = Factory.GetDatabase("master");
      Database webDB = Factory.GetDatabase("web");
      var opt = new PublishOptions(masterDb, webDB, PublishMode.SingleItem, lang, DateTime.Now);
      opt.RootItem = item;
      opt.Deep = true;
      var pb = new Publisher(opt);

      pb.PublishAsync();
    }

    /// <summary>
    /// </summary>
    /// <param name="currentItm">
    /// </param>
    /// <returns>
    /// </returns>
    public static double RatePage(Item currentItm)
    {
      double rateDouble = 0;
      if (currentItm != null)
      {
        double totalMarks;
        double totalVotes;
        if (currentItm.Fields["Total Marks"] == null || currentItm.Fields["Total Votes"] == null)
        {
          return rateDouble;
        }

        Double.TryParse(currentItm.Fields["Total Marks"].Value, out totalMarks);
        Double.TryParse(currentItm.Fields["Total Votes"].Value, out totalVotes);
        if (totalMarks == 0 || totalVotes == 0)
        {
          return rateDouble;
        }

        double d = totalMarks / totalVotes;
        rateDouble = Math.Round(d);
      }

      return rateDouble;
    }

    /// <summary>
    /// </summary>
    /// <param name="id">
    /// </param>
    /// <returns>
    /// </returns>
    public static double RatePage(string id)
    {
      Item currentItm = Sitecore.Context.Database.GetItem(id);
      return RatePage(currentItm);
    }

    /// <summary>
    /// </summary>
    /// <param name="id">
    /// </param>
    /// <param name="voice">
    /// </param>
    /// <param name="rate">
    /// </param>
    /// <returns>
    /// </returns>
    public static bool VoteForPage(string id, int voice, int rate)
    {
      Database db = Factory.GetDatabase("master");
      if (db != null)
      {
        Item votePage = db.Items.GetItem(id);
        if (votePage != null && votePage.Fields["Total Marks"] != null &&
            votePage.Fields["Total Votes"].Value != null)
        {
          int totalMarks;
          int totalVotes;
          int.TryParse(votePage.Fields["Total Marks"].Value, out totalMarks);
          int.TryParse(votePage.Fields["Total Votes"].Value, out totalVotes);
          totalMarks += rate;
          totalVotes += voice;
          using (new SecurityDisabler())
          {
            using (new EditContext(votePage))
            {
              votePage.Fields["Total Marks"].Value = totalMarks.ToString();
              votePage.Fields["Total Votes"].Value = totalVotes.ToString();
            }
          }

          // Publishing 
          PublishOneItem(votePage);
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// </summary>
    /// <param name="itemId">
    /// </param>
    /// <param name="sectionName">
    /// </param>
    /// <returns>
    /// </returns>
    public static TemplateFieldItem[] GetSectionFields(string itemId, string sectionName)
    {
      Item itm = Sitecore.Context.Database.GetItem(itemId);
      return itm != null ? Array.FindAll(itm.Template.Fields, fld => fld.Section.Name.Equals("Specification") && itm.Fields[fld.Name].Value != string.Empty) : null;
    }

    /// <summary>
    /// </summary>
    /// <param name="templateName">
    /// </param>
    /// <returns>
    /// </returns>
    public static ID GetTemplateID(string templateName)
    {
      TemplateItem templateItem = GetTemplateItem(templateName);
      if (templateItem != null)
      {
        return templateItem.ID;
      }

      return null;
    }

    /// <summary>
    /// </summary>
    /// <param name="templateName">
    /// </param>
    /// <returns>
    /// </returns>
    public static TemplateItem GetTemplateItem(string templateName)
    {
      string fullTemplatePath = String.Concat(Consts.TemplatesRootPath, @"/", templateName);
      return Sitecore.Context.Database.Templates[fullTemplatePath];
    }

    /// <summary>
    /// </summary>
    /// <param name="words">
    /// </param>
    /// <returns>
    /// </returns>
    public static string SafeSearchString(string words)
    {
      return !string.IsNullOrEmpty(words) ? words.Replace("'", string.Empty).Replace("\"", string.Empty) : string.Empty;
    }

    /// <summary>
    /// </summary>
    /// <param name="foundItems">
    /// </param>
    /// <returns>
    /// </returns>
    public static IEnumerable<Item> SortByUpdatedDate(IEnumerable<Item> foundItems)
    {
      IOrderedEnumerable<Item> sortedItems = from item in foundItems
                                             orderby item.Fields[FieldIDs.Updated].Value descending
                                             select item;

      return sortedItems;
    }

    /// <summary>
    /// </summary>
    /// <param name="items">
    /// </param>
    /// <param name="top">
    /// </param>
    /// <returns>
    /// </returns>
    public static IEnumerable<Item> ReturnTopResults(IEnumerable<Item> items, int top)
    {
      IEnumerable<Item> topItems = items.Take(top);
      return topItems;
    }

    /// <summary>
    /// </summary>
    /// <param name="queries">
    /// </param>
    /// <returns>
    /// </returns>
    public static List<Item> DoQueries(StringCollection queries)
    {
      var list = new List<Item>();
      foreach (string queryString in queries)
      {
        List<Item> items = DoQuery(queryString);
        list.AddRange(items);
      }

      return list;
    }

    /// <summary>
    /// </summary>
    /// <param name="queryString">
    /// </param>
    /// <returns>
    /// </returns>
    public static List<Item> DoQuery(string queryString)
    {
      var list = new List<Item>();
      Item[] items = Sitecore.Context.Database.SelectItems(queryString);
      list.AddRange(items);
      return list;
    }

    /// <summary>
    /// </summary>
    /// <param name="rootItemId">
    /// </param>
    /// <returns>
    /// </returns>
    public static string GetChildrenStr(string rootItemId)
    {
      var str = new StringBuilder();
      Item rootItm = Sitecore.Context.Database.GetItem(rootItemId);

      if (rootItm != null)
      {
        foreach (Item itm in rootItm.Children)
        {
          str.Append(string.Concat(itm.ID.ToString(), "|"));
        }
      }

      return str.ToString();
    }
  }
}