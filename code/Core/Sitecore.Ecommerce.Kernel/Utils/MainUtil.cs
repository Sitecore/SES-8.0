// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainUtil.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The main util class.
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
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Text.RegularExpressions;
  using System.Web.Security;
  using System.Web.UI;
  using Catalogs;
  using Diagnostics;
  using DomainModel.Carts;
  using DomainModel.Configurations;
  using DomainModel.Currencies;
  using DomainModel.Data;
  using Globalization;
  using Links;
  using Security;
  using Sitecore.Security.Accounts;
  using Sitecore.Web;
  using Sitecore.Web.UI.WebControls;
  using Text;

  /// <summary>
  /// The main utility class.
  /// </summary>
  public static class MainUtil
  {
    /// <summary>
    /// Formats the string. Support anonymous types.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="source">The source.</param>
    /// <returns>The formatted string.</returns>
    public static string FormatWith(this string format, object source)
    {
      return FormatWith(format, null, source);
    }

    /// <summary>
    /// Formats the string. Support anonymous types.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="provider">The provider.</param>
    /// <param name="source">The source.</param>
    /// <returns>The formatted string.</returns>
    /// <exception cref="ArgumentNullException"><c>format</c> is null.</exception>
    public static string FormatWith(this string format, IFormatProvider provider, object source)
    {
      Assert.ArgumentNotNull(format, "format");
      Assert.ArgumentNotNull(source, "source");

      var regex = new Regex(@"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
      var values = new List<object>();

      var rewrittenFormat = regex.Replace(
        format, 
        delegate(Match m)
        {
          var startGroup = m.Groups["start"];
        var propertyGroup = m.Groups["property"];
        var formatGroup = m.Groups["format"];
        var endGroup = m.Groups["end"];
        try
        {
          values.Add((propertyGroup.Value == "0") ? source : DataBinder.Eval(source, propertyGroup.Value));
          return new string('{', startGroup.Captures.Count) + (values.Count - 1) + formatGroup.Value + new string('}', endGroup.Captures.Count);
        }
        catch (Exception)
        {
          return string.Empty;
        }
      });

      return string.Format(provider, rewrittenFormat, values.ToArray());
    }

    /// <summary>
    /// Determines whether [is null or empty] [the specified items].
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="items">The items.</param>
    /// <returns>
    /// <c>true</c> if [is null or empty] [the specified items]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
    {
      return items == null || !items.Any();
    }

    /// <summary>
    /// Determines whether [is logged in].
    /// </summary>
    /// <returns>
    /// <c>true</c> if [is logged in]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsLoggedIn()
    {
      var membershipUser = Membership.GetUser();

      if (membershipUser == null)
      {
        return false;
      }

      var domain = Sitecore.Context.Domain.Name;
      var anonymousName = string.Format("{0}\\Anonymous", domain);
      return membershipUser.UserName.StartsWith(domain) && !membershipUser.UserName.Equals(anonymousName);
    }

    /// <summary>
    /// Determines whether this instance is customer.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance is customer; otherwise, <c>false</c>.
    /// </returns>
    [Obsolete("Use Sitecore.Ecommerce.Security.CustomerMembership.IsCustomer instead.")]
    public static bool IsCustomer()
    {
      var user = Context.Entity.Resolve<User>();
      var membership = new CustomerMembership();
      return membership.IsCustomer(user);
    }

    /// <summary>
    /// Determines whether the specified value is valid email address.
    /// </summary>
    /// <param name="value">The email value.</param>
    /// <returns>
    /// <c>true</c> if the specified value is valid email address, otherwise, <c>false</c>.
    /// </returns>
    public static bool IsValidEmailAddress(string value)
    {
      var r = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
      return r.Match(value).Success;
    }

    /// <summary>
    /// Formats the price.
    /// </summary>
    /// <param name="price">The product price.</param>
    /// <returns>
    /// The formatted price.
    /// </returns>
    [NotNull]
    public static string FormatPrice([NotNull] object price)
    {
      Assert.ArgumentNotNull(price, "price");

      var generalSettings = Context.Entity.GetConfiguration<GeneralSettings>();

      return FormatPrice(price, generalSettings.DisplayCurrencyOnPrices);
    }

    /// <summary>
    /// Formats the price.
    /// </summary>
    /// <param name="price">The price.</param>
    /// <param name="includeCurrency">if set to <c>true</c> [include currency].</param>
    /// <returns>
    /// The formatted price.
    /// </returns>
    [NotNull]
    public static string FormatPrice([NotNull] object price, bool includeCurrency)
    {
      Assert.ArgumentNotNull(price, "price");

      return FormatPrice(price, includeCurrency, string.Empty);
    }

    /// <summary>
    /// Formats the price.
    /// </summary>
    /// <param name="price">The price.</param>
    /// <param name="includeCurrency">if set to <c>true</c> [include currency].</param>
    /// <param name="priceFormatString">The price format string.</param>
    /// <returns>
    /// The formatted price.
    /// </returns>
    /// <exception cref="Exception">Price format string is invalid</exception>
    [NotNull]
    public static string FormatPrice([NotNull] object price, bool includeCurrency, [CanBeNull] string priceFormatString)
    {
      Assert.ArgumentNotNull(price, "price");

      var shoppingCart = Context.Entity.GetInstance<ShoppingCart>();

      return FormatPrice(price, includeCurrency, priceFormatString, shoppingCart.Currency.Code);
    }

    /// <summary>
    /// Formats the price.
    /// </summary>
    /// <param name="price">The price.</param>
    /// <param name="includeCurrency">if set to <c>true</c> [include currency].</param>
    /// <param name="priceFormatString">The price format string.</param>
    /// <param name="currencyCode">The currency code.</param>
    /// <returns>
    /// Returns formatted price.
    /// </returns>
    [NotNull]
    public static string FormatPrice([NotNull] object price, bool includeCurrency, [CanBeNull] string priceFormatString, [CanBeNull] string currencyCode)
    {
      Assert.ArgumentNotNull(price, "price");

      if (string.IsNullOrEmpty(priceFormatString))
      {
        var shop = Context.Entity.GetConfiguration<ShopContext>();
        priceFormatString = shop.GeneralSettings.PriceFormatString ?? string.Empty;
      }

      var priceString = "-";
      try
      {
        priceString = string.Format(Sitecore.Context.Culture, priceFormatString, price);

        if (includeCurrency && !string.IsNullOrEmpty(currencyCode))
        {
          var provider = Context.Entity.Resolve<IEntityProvider<Currency>>();
          var cur = provider.Get(currencyCode);

          Assert.IsNotNull(cur, "Currency \"{0}\" is not found.", currencyCode);

          return string.Format("{0} {1}", cur.Title, priceString);
        }
      }
      catch (Exception exception)
      {
        Log.Error("Price format string is invalid", exception, typeof(MainUtil));
      }

      return priceString;
    }

    /// <summary>
    /// Searches a control collection and descendants to find a control in the UI hierarchy.
    /// </summary>
    /// <param name="id">Id to search for</param>
    /// <param name="controls">The control collection to start out search in.</param>
    /// <returns>The control.</returns>
    public static Control FindControl(string id, ControlCollection controls)
    {
      Assert.ArgumentNotNullOrEmpty(id, "id");
      Assert.ArgumentNotNull(controls, "controls");

      foreach (Control control in controls)
      {
        if (control.ID == id)
        {
          return control;
        }

        var child = FindControl(id, control);
        if (child != null)
        {
          return child;
        }
      }

      return null;
    }

    /// <summary>
    /// Searches a control and descendants to find a control in the UI hierarchy.
    /// </summary>
    /// <param name="id">Id to search for</param>
    /// <param name="control">The root control</param>
    /// <returns>The control.</returns>
    public static Control FindControl(string id, Control control)
    {
      Assert.ArgumentNotNullOrEmpty(id, "id");
      Assert.ArgumentNotNull(control, "control");

      if (control.ID != null && control.ID.Contains("_" + id))
      {
        return control;
      }

      return (from Control c in control.Controls select FindControl(id, c)).FirstOrDefault(rc => rc != null);
    }

    /// <summary> Renders a sub layout from the layouts folder. </summary>
    /// <param name="id"> The id of current item </param>
    /// <param name="sublayout"> The sub layout filename without extension </param>
    /// <returns>returns the sub layout to render.</returns>
    public static string RenderSublayout(string id, string sublayout)
    {
      Assert.ArgumentNotNullOrEmpty(id, "id");
      Assert.ArgumentNotNullOrEmpty(sublayout, "sublayout");

      Sitecore.Context.Item = Sitecore.Context.Database.GetItem(id);
      var tmp = new UserControl();
      var ctrl = (UserControl)tmp.LoadControl("~/Layouts/" + sublayout + ".ascx");
      var page = new Page();

      page.Controls.Add(ctrl);

      var args = new EventArgs();

      // Run OnInit
      Reflection.ReflectionUtil.CallMethod(ctrl, "OnInit", new object[] { args });

      // Run OnLoad
      Reflection.ReflectionUtil.CallMethod(ctrl, "OnLoad", new object[] { args });

      // Run OnPreRender
      Reflection.ReflectionUtil.CallMethod(ctrl, "OnPreRender", new object[] { args });

      ctrl.DataBind();

      var sb = new StringBuilder();
      using (var sw = new StringWriter(sb))
      {
        using (var hw = new HtmlTextWriter(sw))
        {
          ctrl.RenderControl(hw);
        }
      }

      return sb.ToString();
    }

    /// <summary>
    /// Loads the rendering from the XSL folder
    /// </summary>
    /// <param name="rendering">The rendering filename without extension</param>
    /// <returns>The rendering from the XSL folder</returns>
    public static string LoadRendering(string rendering)
    {
      Assert.ArgumentNotNullOrEmpty(rendering, "rendering");

      if (Sitecore.Context.Item == null)
      {
        return "Sitecore.Context.Item is null";
      }

      var xslFilename = "/xsl/" + rendering + ".xslt";
      var xslFile = new XslFile
                      {
                        Path = xslFilename
                      };

      var sb = new StringBuilder();
      using (var sw = new StringWriter(sb))
      {
        using (var hw = new HtmlTextWriter(sw))
        {
          xslFile.RenderControl(hw);
        }
      }

      return sb.ToString();
    }

    /// <summary>
    /// Gets the language query string.
    /// </summary>
    /// <param name="languageName">Name of the language.</param>
    /// <returns>The language query string.</returns>
    public static string GetLanguageLink(string languageName)
    {
      Assert.ArgumentNotNullOrEmpty(languageName, "languageName");

      var language = Language.Parse(languageName);
      if (language == null || Sitecore.Context.Item == null || WebUtil.CurrentPage == null)
      {
        return string.Empty;
      }

      using (new LanguageSwitcher(language))
      {
        var contextItemLang = Sitecore.Context.Database.Items[Sitecore.Context.Item.ID, language];
        if (contextItemLang == null)
        {
          return string.Empty;
        }

        var virtualProductResolver = Context.Entity.Resolve<VirtualProductResolver>();
        var catalogItem = virtualProductResolver.ProductCatalogItem;

        var queryString = WebUtil.GetQueryString();

        if (catalogItem == null || contextItemLang.ID == catalogItem.ID)
        {
          return string.IsNullOrEmpty(queryString)
                 ?
                 LinkManager.GetItemUrl(contextItemLang)
                 :
                 new UrlString { Path = LinkManager.GetItemUrl(contextItemLang), Query = queryString }.ToString();
        }

        var catalogItemLang = Sitecore.Context.Database.Items[catalogItem.ID, language];
        return string.IsNullOrEmpty(queryString)
               ?
               virtualProductResolver.GetVirtualProductUrl(catalogItemLang, contextItemLang)
               :
               new UrlString { Path = virtualProductResolver.GetVirtualProductUrl(catalogItemLang, contextItemLang), Query = queryString }.ToString();
      }
    }
  }
}