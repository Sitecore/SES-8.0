// -------------------------------------------------------------------------------------------
// <copyright file="XslExtensions.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Xsl
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Text;
  using System.Xml;
  using System.Xml.Linq;
  using System.Xml.XPath;
  using Catalogs;
  using Configuration;
  using Data;
  using Diagnostics;
  using DomainModel.Carts;
  using DomainModel.Catalogs;
  using DomainModel.Configurations;
  using DomainModel.Currencies;
  using DomainModel.Data;
  using DomainModel.Prices;
  using DomainModel.Products;
  using Globalization;
  using Links;
  using Products;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Exceptions;
  using Sitecore.Web.UI.WebControls;
  using Unity;
  using Utils;
  using Xml.XPath;

  using ProductStock = Sitecore.Ecommerce.DomainModel.Products.ProductStock;

  /// <summary>
  /// XslMehtodes for accesing eCommerce API from xslt
  /// </summary>
  public class XslExtensions : Xml.Xsl.XslHelper
  {
    /// <summary>
    /// Gets an Item List from an XPathNodeIterator.
    /// </summary>
    /// <param name="iterator">
    /// The node iterator.
    /// </param>
    /// <returns>Item List from an XPathNodeIterator.</returns>
    public List<Item> GetItemList(XPathNodeIterator iterator)
    {
      var items = new List<Item>();
      Database database = Sitecore.Context.Database;

      Assert.ArgumentNotNull(iterator, "iterator");
      while (iterator.MoveNext())
      {
        ID id;
        if (ID.TryParse(iterator.Current.GetAttribute("id", string.Empty), out id)
            && database != null)
        {
          items.Add(ItemManager.GetItem(id, Language.Current, Sitecore.Data.Version.Latest, database));
        }
      }

      return items;
    }

    /// <summary>
    /// Gets the first item from an XPathNodeIterator
    /// </summary>
    /// <param name="iterator">The node iterator.</param>
    /// <returns>The item.</returns>
    public new Item GetItem(XPathNodeIterator iterator)
    {
      // var helper = new ScXslHelper();
      // return helper.GetItem(iterator);
      ID id;
      Assert.ArgumentNotNull(iterator, "iterator");
      if (!iterator.MoveNext())
      {
        return null;
      }

      if (!ID.TryParse(iterator.Current.GetAttribute("id", string.Empty), out id))
      {
        return null;
      }

      return Sitecore.Context.Database == null ? null : ItemManager.GetItem(id, Language.Current, Sitecore.Data.Version.Latest, Sitecore.Context.Database);
    }

    /// <summary>
    /// Gets an XPathNodeIterator for a set of items.
    /// </summary>
    /// <param name="idlist">Pipe-separated string containing Sitecore Item IDs</param>
    /// <returns>An XPathNodeIterator for the Sitecore item</returns>
    public new XPathNodeIterator Items(string idlist)
    {
      var items = new List<XPathNavigator>();
      Database database = Sitecore.Context.Database;

      string[] ids = idlist.Trim(' ', '|').Split('|');
      foreach (string id in ids)
      {
        Item item = database.GetItem(id, Sitecore.Context.ContentLanguage);
        if (item != null)
        {
          ItemNavigator nav = Factory.CreateItemNavigator(item);
          items.Add(nav.CreateNavigator());
        }
      }

      return new ListNodeIterator(items, false);
    }

    /// <summary>
    /// Gets an XPathNodeIterator for an item.
    /// </summary>
    /// <param name="id">
    /// Sitecore Item ID
    /// </param>
    /// <returns>
    /// An XPathNodeIterator for the Sitecore item
    /// </returns>
    public XPathNodeIterator Item(string id)
    {
      var items = new List<XPathNavigator>();
      Database database = Sitecore.Context.Database;

      if (string.IsNullOrEmpty(id))
      {
        return new ListNodeIterator();
      }

      Item item = database.GetItem(id, Sitecore.Context.Language);
      if (item != null)
      {
        ItemNavigator nav = Factory.CreateItemNavigator(item);
        items.Add(nav.CreateNavigator());
      }

      return new ListNodeIterator(items, false);
    }

    /// <summary>
    /// Gets an XPathNodeIterator for a set of items.
    /// </summary>
    /// <param name="field">
    /// The field that contains the ID list
    /// </param>
    /// <param name="ni">
    /// An XPathNodeIterator for the Sitecore item
    /// </param>
    public XPathNodeIterator Items(string field, XPathNodeIterator ni)
    {
      string idlist = fld(field, ni);
      return this.Items(idlist);
    }

    /// <summary>
    /// Gets the first image item.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <param name="ni">The ni.</param>
    /// <returns>The first image item.</returns>
    public XPathNodeIterator GetFirstImageItem(string field, XPathNodeIterator ni)
    {
      string values = fld(field, ni);
      string[] arr = values.Trim('|').Split('|');
      if (arr.Length > 0)
      {
        var item = (MediaItem)Sitecore.Context.Database.GetItem(arr[0]);
        if (item != null)
        {
          return new ListNodeIterator(new Item[]
          {
            item
          });

          // return "/~/media/" + item.MediaPath + ".ashx";
        }
      }

      return new ListNodeIterator(new Item[]
      {
      });
    }

    /// <summary>
    /// Creates an empty XPathNavigator
    /// </summary>
    /// <returns>
    /// </returns>
    protected XPathNavigator CreateEmptyNavigator()
    {
      XPathNavigator nav = new XmlDocument().CreateNavigator();
      return nav;
    }

    /// <summary>
    /// Gets the Ecommerce settings root node
    /// </summary>
    /// <returns>
    /// </returns>
    public XPathNodeIterator GetSettingsRootNode()
    {
      var items = new Item[1];
      items[0] = Sitecore.Context.Database.SelectSingleItem("/*/system/Modules/*[@@templatekey='configuration']");
      return new ListNodeIterator(items);
    }

    /// <summary>
    /// Get an XML structure for a pager
    /// </summary>
    /// <param name="pageSize">
    /// Items to display on each page
    /// </param>
    /// <param name="groupSize">
    /// Size of the group.
    /// </param>
    /// <param name="itemCount">
    /// Total items for the pager
    /// </param>
    /// <param name="currentPage">
    /// Current page index
    /// </param>
    /// <returns>
    /// </returns>
    public XmlDocument GetPagerXml(int pageSize, int groupSize, int itemCount, int currentPage)
    {
      pageSize = (pageSize == 0) ? 1 : pageSize;
      int pageCount = itemCount / pageSize;
      if (itemCount % pageSize != 0)
      {
        pageCount += 1;
      }

      var root = new XElement("root");
      root.SetAttributeValue("pageSize", pageSize);
      root.SetAttributeValue("itemCount", itemCount);
      root.SetAttributeValue("currentPage", currentPage);
      root.SetAttributeValue("pageCount", pageCount);

      root.SetAttributeValue("first", currentPage == 1);
      root.SetAttributeValue("last", currentPage == pageCount);

      int startIndex;
      int endIndex;
      if (currentPage <= groupSize)
      {
        startIndex = 1;
        endIndex = groupSize;
      }
      else
      {
        startIndex = (((currentPage - 1) / groupSize) * groupSize) + 1;
        endIndex = startIndex + (groupSize - 1);
      }

      for (int i = 1; i <= pageCount; i++)
      {
        var page = new XElement("page", i);
        root.Add(page);
        if (i == currentPage)
        {
          page.SetAttributeValue("current", true);
        }
        else if ((i + 1) == currentPage)
        {
          page.SetAttributeValue("previous", true);
        }
        else if ((i - 1) == currentPage)
        {
          page.SetAttributeValue("next", true);
        }

        if (i >= startIndex && i <= endIndex)
        {
          page.SetAttributeValue("visible", true);
        }
        else if ((i + 1) == startIndex)
        {
          page.SetAttributeValue("previousgroup", true);
        }
        else if ((i - 1) == endIndex)
        {
          page.SetAttributeValue("nextgroup", true);
        }
      }

      var doc = new XmlDocument();
      doc.LoadXml(root.ToString());

      return doc;
    }

    /// <summary>
    /// Gets a link that can be used in the pager, link includes any field specified in PagerLinkParameters
    /// </summary>
    /// <param name="ni">
    /// An XPathNodeIterator for the Sitecore item
    /// </param>
    /// <param name="pageNumber">
    /// Current page index
    /// </param>
    /// <returns>
    /// </returns>
    public string GetPagerLink(XPathNodeIterator ni, string pageNumber)
    {
      string url = path(ni);
      url += "?pg=" + pageNumber;

      string pagerLinkParameters = Settings.GetSetting("PagerLinkParameters");
      if (!string.IsNullOrEmpty(pagerLinkParameters))
      {
        string[] arr = pagerLinkParameters.Split(',');
        foreach (string param in arr)
        {
          if (!string.IsNullOrEmpty(this.RequestVariable(param)))
          {
            url += "&" + param + "=" + this.RequestVariable(param);
          }
        }
      }

      return url;
    }

    /// <summary>
    /// Gets the page size link.
    /// </summary>
    /// <param name="ni">
    /// The ni.
    /// </param>
    /// <param name="pageNumber">
    /// The page number.
    /// </param>
    /// <returns>
    /// </returns>
    public string GetPageSizeLink(XPathNodeIterator ni, string pageNumber)
    {
      string url = path(ni);
      url += "?pg=" + pageNumber;

      string pagerLinkParameters = Settings.GetSetting("Ecommerce.PagerLinkParameters");
      if (!string.IsNullOrEmpty(pagerLinkParameters))
      {
        string[] arr = pagerLinkParameters.Split(',');
        foreach (string param in arr)
        {
          if (!string.IsNullOrEmpty(this.RequestVariable(param))
              && param != "pagesize")
          {
            url += "&" + param + "=" + this.RequestVariable(param);
          }
        }
      }

      return url;
    }

    /// <summary>
    /// Gets a POST or GET variable
    /// </summary>
    /// <param name="key">
    /// POST or GET key
    /// </param>
    /// <param name="defaultValue">
    /// Default value if requst varible is not present or is empty
    /// </param>
    /// <returns>
    /// </returns>
    public string RequestVariable(string key, string defaultValue)
    {
      string value = form(key);
      if (string.IsNullOrEmpty(value)
          && !string.IsNullOrEmpty(qs(key)))
      {
        value = qs(key);
      }

      if (value != null)
      {
        return (value.Length > 0) ? value : defaultValue;
      }

      return string.Empty;
    }

    /// <summary>
    /// Gets a POST or GET variable
    /// </summary>
    /// <param name="key">
    /// POST or GET key
    /// </param>
    /// <returns>
    /// </returns>
    public string RequestVariable(string key)
    {
      return this.RequestVariable(key, string.Empty);
    }

    /// <summary>
    /// Gets the items filtered.
    /// </summary>
    /// <param name="filter1">
    /// The filter1.
    /// </param>
    /// <param name="filter2">
    /// The filter2.
    /// </param>
    /// <param name="filter3">
    /// The filter3.
    /// </param>
    /// <param name="ni">
    /// An XPathNodeIterator for the Sitecore item
    /// </param>
    /// <returns>
    /// </returns>
    public XPathNodeIterator GetItemsFiltered(string filter1, string filter2, string filter3, XPathNodeIterator ni)
    {
      XPathNodeIterator items1 = this.GetItemsFiltered(filter1, ni);
      XPathNodeIterator items2 = this.GetItemsFiltered(filter2, items1);
      XPathNodeIterator items3 = this.GetItemsFiltered(filter3, items2);
      return items3;
    }

    /// <summary>
    /// Gets the items filtered.
    /// </summary>
    /// <param name="filter">
    /// The filter.
    /// </param>
    /// <param name="ni">
    /// An XPathNodeIterator for the Sitecore item
    /// </param>
    /// <returns>
    /// </returns>
    private XPathNodeIterator GetItemsFiltered(string filter, XPathNodeIterator ni)
    {
      string[] arr = filter.Split(',');
      if (filter.Length == 0)
      {
        return ni;
      }

      // lager en liste over alle items i ni
      List<Item> items = this.GetItemList(ni);

      var itemList = new List<Item>();

      foreach (Item item in items)
      {
        ItemLink[] links = item.Links.GetAllLinks();
        foreach (string id in arr)
        {
          foreach (ItemLink link in links)
          {
            if (link.TargetItemID.ToString() != id)
            {
              continue;
            }

            if (!itemList.Contains(item))
            {
              itemList.Add(item);
            }
          }
        }
      }

      return new ListNodeIterator(itemList);
    }

    /// <summary>
    /// Gets the sort field.
    /// </summary>
    /// <param name="expression">
    /// The expression.
    /// </param>
    /// <param name="defaultValue">
    /// The default value.
    /// </param>
    /// <returns>
    /// </returns>
    public string GetSortField(string expression, string defaultValue)
    {
      string val = expression.Replace(" descending", string.Empty).Replace(" ascending", string.Empty).Replace(" number", string.Empty);
      return (val.Length == 0) ? defaultValue : val;
    }

    /// <summary>
    /// Gets the sort direction.
    /// </summary>
    /// <param name="expression">
    /// The expression.
    /// </param>
    /// <returns>
    /// </returns>
    public string GetSortDirection(string expression)
    {
      if (expression.Contains("descending"))
      {
        return "descending";
      }

      return "ascending";
    }

    /// <summary>
    /// Gets the type of the sort.
    /// </summary>
    /// <param name="expression">
    /// The expression.
    /// </param>
    /// <returns>
    /// </returns>
    public string GetSortType(string expression)
    {
      if (expression.Contains("number"))
      {
        return "number";
      }

      return "text";
    }

    /// <summary>
    /// Gets the first ascendant or self with a value in fild
    /// </summary>
    /// <param name="field">
    /// The field.
    /// </param>
    /// <param name="ni">
    /// An XPathNodeIterator for the Sitecore item
    /// </param>
    /// <returns>
    /// Value from field
    /// </returns>
    public string GetFirstAscendantOrSelfWithValue(string field, XPathNodeIterator ni)
    {
      Item item = this.GetItem(ni);
      return Utils.ItemUtil.GetFirstAscendantOrSelfWithValueFromItem(field, item);
    }

    /// <summary>
    /// Gets the customer price.
    /// </summary>
    /// <param name="ni">The ni.</param>
    /// <returns>The customer price.</returns>
    public string GetCustomerPrice(XPathNodeIterator ni)
    {
      string value = "-";

      try
      {
        GeneralSettings settings = Context.Entity.GetConfiguration<GeneralSettings>();
        Totals productPricesList = this.GetProductTotals(ni);

        if (productPricesList != null && productPricesList.MemberPrice != decimal.Zero)
        {
          value = MainUtil.FormatPrice(productPricesList.MemberPrice, settings.DisplayCurrencyOnPrices);
        }
      }
      catch (Exception ex)
      {
        Log.Error("Unable to resolve customer price.", ex, this);
      }

      return value;
    }

    /// <summary>
    /// Gets the list price.
    /// </summary>
    /// <param name="ni">The ni.</param>
    /// <returns>The list price.</returns>
    public string GetListPrice(XPathNodeIterator ni)
    {
      string value = "-";

      try
      {
        GeneralSettings settings = Context.Entity.GetConfiguration<GeneralSettings>();
        Totals productPricesList = this.GetProductTotals(ni);

        if (productPricesList != null && productPricesList.PriceExVat != decimal.Zero)
        {
          value = MainUtil.FormatPrice(productPricesList.PriceExVat + productPricesList.DiscountExVat, settings.DisplayCurrencyOnPrices);
        }
      }
      catch (Exception ex)
      {
        Log.Error("Unable to resolve product list price.", ex, this);
      }

      return value;
    }

    /// <summary>
    /// Gets the single price.
    /// </summary>
    /// <param name="ni">The ni.</param>
    /// <returns>The single price.</returns>
    public string GetSinglePrice(XPathNodeIterator ni)
    {
      string value = "-";

      try
      {
        Totals productPricesList = this.GetProductTotals(ni);

        if (productPricesList != null && productPricesList.PriceExVat != decimal.Zero)
        {
          value = MainUtil.FormatPrice(productPricesList.PriceExVat, true);
        }
      }
      catch (Exception ex)
      {
        Log.Error("Unable to resolve single price.", ex, this);
      }

      return value;
    }

    /// <summary>
    /// Gets the general price.
    /// </summary>
    /// <param name="ni">The ni.</param>
    /// <returns>The general price.</returns>
    private Totals GetProductTotals(XPathNodeIterator ni)
    {
      Item item = this.GetItem(ni);
      Assert.ArgumentNotNull(item, "item");

      IProductRepository productProvider = Context.Entity.Resolve<IProductRepository>();

      ProductBaseData product = productProvider.Get<ProductBaseData>(item["Product Code"]);

      if (product != null)
      {
        ShoppingCart shoppingCart = Context.Entity.GetInstance<ShoppingCart>();
        IProductPriceManager productPriceManager = Context.Entity.Resolve<IProductPriceManager>();
        Totals productPricesList = productPriceManager.GetProductTotals<Totals, ProductBaseData, Currency>(product, shoppingCart.Currency);

        return productPricesList;
      }

      return default(Totals);
    }

    /// <summary>
    /// return number in stock to an xsl controll
    /// </summary>
    /// <param name="ni">
    /// The node iterator for the product item
    /// </param>
    /// <returns>
    /// </returns>
    public string numberInStock(XPathNodeIterator ni)
    {
      Item item = this.GetItem(ni);
      int cPrice = 0;

      // int cPrice = ProductUtil.GetNumberInStock(item);
      return cPrice + string.Empty;
    }

    /// <summary>
    /// Get expected stoc date for a product
    /// </summary>
    /// <param name="ni">
    /// The node for the product.
    /// </param>
    /// <param name="format">
    /// The datetime format.
    /// </param>
    /// <returns>
    /// </returns>
    public string expectedStockDate(XPathNodeIterator ni, string format)
    {
      Item item = this.GetItem(ni);
      DateTime dt = DateTime.Now;

      // DateTime dt = ProductUtil.GetExpectedStockDate(item);
      return dt.ToString(format);
    }

    /// <summary>
    /// A helper method to retrieve the right Item. If the current item is a variant and 
    /// a specific field doesn't exist on the variant, then the parent item is returned.
    /// </summary>
    /// <param name="fieldName">
    /// </param>
    /// <param name="ni">
    /// </param>
    public XPathNodeIterator selectProduct(string fieldName, XPathNodeIterator ni)
    {
      XPathNodeIterator clone = ni.Clone();

      var helper = new Xml.Xsl.XslHelper();

      string fldValue = helper.fld(fieldName, clone);
      if (fldValue.Length > 0)
      {
        return ni;
      }

      string currentId = clone.Current.GetAttribute("id", string.Empty);

      Item currentItem = Sitecore.Context.Database.GetItem(currentId);
      if (currentItem != null)
      {
        Item parentItem = currentItem.Parent;
        if (parentItem != null &&
            parentItem.Fields[fieldName] != null)
        {
          return helper.item(parentItem.Paths.FullPath, ni);
        }
      }

      return ni;
    }

    /// <summary>
    /// Gets the parent from item URL.
    /// </summary>
    /// <returns></returns>
    public XPathNodeIterator GetParentFromItemUrl()
    {
      Item catalogItem = Context.Entity.Resolve<VirtualProductResolver>().ProductCatalogItem ?? Sitecore.Context.Item.Parent;

      Assert.IsNotNull(catalogItem, "Catalog item is null.");

      return new ListNodeIterator(new[] { catalogItem });
    }

    /// <summary>
    /// Gets the closest actual item.
    /// </summary>
    /// <returns></returns>
    public XPathNodeIterator GetClosestActualItem()
    {
      Item catalogItem = Context.Entity.Resolve<VirtualProductResolver>().ProductCatalogItem ?? Sitecore.Context.Item;

      Assert.IsNotNull(catalogItem, "Catalog item is null.");

      return new ListNodeIterator(new[] { catalogItem });
    }

    /// <summary>
    /// A helper method to retrieve the right Item. If the current item is a variant and
    /// a specific field doesn't exist on the variant, then the parent item is returned.
    /// </summary>
    /// <param name="ni">The ni.</param>
    /// <returns></returns>
    /// <exception cref="InvalidValueException">Product Selection Method field is invalid.</exception>
    public XPathNodeIterator GetProductsForCatalog(XPathNodeIterator ni)
    {
      Item currentItem = this.GetItem(ni);
      Assert.IsNotNull(currentItem, "Current item is null");

      string selectedMethod = currentItem["Product Selection Method"];
      if (string.IsNullOrEmpty(selectedMethod) || !ID.IsID(selectedMethod))
      {
        throw new InvalidValueException("Product Selection Method field is invalid.");
      }

      Item selectionMethodItem = Sitecore.Context.Database.GetItem(selectedMethod);
      Assert.IsNotNull(selectionMethodItem, "Product Selection Method was noe found");

      string selectionMethodName = selectionMethodItem["Code"];
      Assert.IsNotNullOrEmpty(selectionMethodName, "Selection method name is null or empty");

      ICatalogProductResolveStrategy resolveStrategy = Context.Entity.Resolve<ICatalogProductResolveStrategy>(selectionMethodName);
      IEnumerable<ProductBaseData> products = resolveStrategy.GetCatalogProducts<ProductBaseData, Item>(currentItem);

      List<Item> itemList = new List<Item>();
      if (products.IsNullOrEmpty())
      {
        return new ListNodeIterator(itemList);
      }

      foreach (ProductBaseData product in products)
      {
        if (!(product is IEntity))
        {
          continue;
        }

        IEntity entity = (IEntity)product;
        Item productItem = Sitecore.Context.Database.GetItem(entity.Alias);
        itemList.Add(productItem);
      }

      return new ListNodeIterator(itemList);
    }

    /// <summary>
    /// Gets the max image heigh from children.
    /// </summary>
    /// <param name="ni">
    /// The ni.
    /// </param>
    /// <param name="fieldName">
    /// Name of the field.
    /// </param>
    /// <param name="width">
    /// The width.
    /// </param>
    /// <returns>
    /// </returns>
    public int GetMaxImgHeighFromChildren(XPathNodeIterator ni, string fieldName, string width)
    {
      Item item = this.GetItem(ni);

      decimal maxWidht;
      if (!decimal.TryParse(width, out maxWidht))
      {
        return 0;
      }

      decimal maxHeight = 0;

      if (item != null)
      {
        foreach (Item child in item.Children)
        {
          var imgField = (ImageField)child.Fields[fieldName];
          if (imgField != null)
          {
            decimal h;
            decimal w;

            if (decimal.TryParse(imgField.Height, out h)
                && decimal.TryParse(imgField.Width, out w))
            {
              decimal calcHeight = (maxWidht / w) * h;
              if (calcHeight > maxHeight)
              {
                maxHeight = calcHeight;
              }
            }
          }
        }
      }

      return (int)maxHeight;
    }

    /// <summary>
    /// Gets the stock.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <returns>The stock.</returns>
    public long GetStock(string productId)
    {
      if (string.IsNullOrEmpty(productId))
      {
        return 0;
      }

      long stockValue = 0;

      try
      {
        IProductStockManager stockManager = Context.Entity.Resolve<IProductStockManager>();
        ProductStock stock = stockManager.GetStock(new ProductStockInfo { ProductCode = productId });
        stockValue = stock.Stock;
      }
      catch (Exception ex)
      {
        Log.Error("Unable to resolve stock value.", ex, this);
      }

      return stockValue;
    }

    /// <summary>
    /// Gets the delivery time.
    /// </summary>
    /// <returns>The delivery time.</returns>
    public string GetDeliveryTime()
    {
      GeneralSettings generalSettings = Context.Entity.GetConfiguration<GeneralSettings>();
      return generalSettings.ProductDeliveryTime;
    }

    /// <summary>
    /// Format a date in iso format to a date using a specified format
    /// </summary>
    /// <param name="sIsoDate">The date in iso format</param>
    /// <param name="sFormat">Formatting string</param>
    /// <returns>
    /// The date formatted as specified in the sFormat parameters<br/>
    /// If sIsoDate is blank or null, an empty string is returned.
    /// </returns>
    public override string formatdate(string sIsoDate, string sFormat)
    {
      CultureInfo cultureInfo = Sitecore.Context.Language.CultureInfo;
      if (!cultureInfo.IsNeutralCulture)
      {
        return base.formatdate(sIsoDate, sFormat);
      }

      DateTime time = DateUtil.IsoDateToDateTime(sIsoDate);

      Item langItm = Sitecore.Context.Database.Items[Consts.LanguagesRootPath + "/" + Sitecore.Context.Language.Name];

      if (langItm != null)
      {
        Language lang;
        if (Language.TryParse(langItm.Fields["Regional Iso Code"].Value, out lang))
        {
          cultureInfo = lang.CultureInfo;
          if (!cultureInfo.IsNeutralCulture)
          {
            return time.ToString(sFormat, cultureInfo);
          }
        }
      }

      return time.ToString(sFormat);
    }

    /// <summary>
    /// Determines whether [is editing mode].
    /// </summary>
    /// <returns>
    /// <c>true</c> if [is editing mode]; otherwise, <c>false</c>.
    /// </returns>
    public bool IsEditingMode()
    {
      return Sitecore.Context.PageMode.IsPageEditorEditing;
    }

    /// <summary>
    /// Fields the title.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns></returns>
    public string FieldTitle(string id, string fieldName)
    {
      Item itm = Sitecore.Context.Database.GetItem(id);
      if (itm != null)
      {
        if (itm.Fields[fieldName] != null)
        {
          string fieldTitle = itm.Fields[fieldName].Title;
          return !fieldTitle.Equals(string.Empty) ? fieldTitle : fieldName;
        }
      }

      return fieldName;
    }

    /// <summary>
    /// Translates the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public string Translate(string key)
    {
      return Globalization.Translate.Text(key);
    }

    /// <summary>
    /// Translates the format.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="params1">The params1.</param>
    /// <returns></returns>
    public string TranslateFormat(string key, string params1)
    {
      return this.TranslateFormatParams(key, params1);
    }

    /// <summary>
    /// Translates the format.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="params1">The params1.</param>
    /// <param name="params2">The params2.</param>
    /// <returns></returns>
    public string TranslateFormat(string key, string params1, string params2)
    {
      return this.TranslateFormatParams(key, params1, params2);
    }

    /// <summary>
    /// Translates the format.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="params1">The params1.</param>
    /// <param name="params2">The params2.</param>
    /// <param name="params3">The params3.</param>
    /// <returns></returns>
    public string TranslateFormat(string key, string params1, string params2, string params3)
    {
      return this.TranslateFormatParams(key, params1, params2, params3);
    }

    /// <summary>
    /// Toes the lower.
    /// </summary>
    /// <param name="str">The STR.</param>
    /// <returns></returns>
    public override string ToLower(string str)
    {
      return string.IsNullOrEmpty(str) ? string.Empty : str.ToLowerInvariant();
    }

    /// <summary>
    /// Metas the data QS.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="qsParamsNames">The qs params names.</param>
    /// <returns></returns>
    public string MetaDataQS(string id, string qsParamsNames)
    {
      var str = new StringBuilder();

      Item itm = Sitecore.Context.Database.GetItem(id);
      string[] qsParams = qsParamsNames.Split(',');
      if (itm != null)
      {
        foreach (string qsParam in qsParams)
        {
          Field fld = itm.Fields[qsParam];
          if (fld != null)
          {
            str.Append(string.Concat(qsParam, "="));
            str.Append(fld.Value);
            str.Append("&");
          }
        }
      }

      return str.Length > 0 ? str.ToString(0, str.Length - 1) : string.Empty;
    }

    /// <summary>
    /// Gets the item from database.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="databaseName">Name of the database.</param>
    /// <returns></returns>
    public XPathNodeIterator GetItemFromDatabase(string path, string databaseName)
    {
      Database db = Factory.GetDatabase(databaseName);
      if (db != null)
      {
        Item itm = db.GetItem(path);
        if (itm != null)
        {
          ItemNavigator navigator = Factory.CreateItemNavigator(itm);
          if (navigator != null)
          {
            return navigator.Select(".");
          }
        }
      }

      XPathNavigator emptyNavigator = new XmlDocument().CreateNavigator();
      if (emptyNavigator != null)
      {
        return emptyNavigator.Select("*");
      }

      return null;
    }

    /// <summary>
    /// Gets the FLD from database.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="iterator">The iterator.</param>
    /// <returns></returns>
    public string GetFldFromDatabase(string fieldName, string databaseName, XPathNodeIterator iterator)
    {
      ID id;
      if (!iterator.MoveNext())
      {
        return null;
      }

      if (!ID.TryParse(iterator.Current.GetAttribute("id", string.Empty), out id))
      {
        return null;
      }

      Database database = Factory.GetDatabase(databaseName);
      if (database == null)
      {
        return null;
      }

      Item itm = ItemManager.GetItem(id, Language.Current, Sitecore.Data.Version.Latest, database);

      var renderer = new FieldRenderer
      {
        Item = itm,
        FieldName = fieldName
      };
      return renderer.Render();
    }

    /// <summary>
    /// Translates the format params.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="stringParams">The string params.</param>
    /// <returns></returns>
    private string TranslateFormatParams(string key, params string[] stringParams)
    {
      string str = Globalization.Translate.Text(key);
      for (int i = 0; i < stringParams.Length; i++)
      {
        string sFormat = string.Concat(@"{", i.ToString(), @"}");
        if (str.IndexOf(sFormat) == -1)
        {
          return str;
        }
      }

      str = string.Format(str, stringParams);
      return str;
    }

    /// <summary>
    /// Gets the site's start item.
    /// </summary>
    /// <returns>
    /// </returns>
    public XPathNodeIterator GetSiteStartItem()
    {
      var items = new Item[1];
      items[0] = Sitecore.Context.Site.Database.GetItem(Sitecore.Context.Site.StartPath);
      return new ListNodeIterator(items);
    }

    /// <summary>
    /// Gets the design setting.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>The settings value</returns>
    public object GetDesignSetting(string key)
    {
      DesignSettings designSettings = Context.Entity.GetConfiguration<DesignSettings>();
      EntityHelper entityHelper = Context.Entity.Resolve<EntityHelper>();
      return entityHelper.GetPropertyValueByField<object, DesignSettings>(designSettings, key);
    }
  }
}