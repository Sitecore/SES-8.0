// -------------------------------------------------------------------------------------------
// <copyright file="Price.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Web.UI.HtmlControls
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Text;
  using System.Web.UI;
  using System.Web.UI.WebControls;
  using System.Xml;
  using System.Xml.Serialization;
  using Globalization;
  using PriceMatrix;
  using Shell.Applications.ContentEditor;
  using Sitecore.Collections;
  using Sitecore.Configuration;
  using Sitecore.Data.Items;
  using Sitecore.Web.UI.HtmlControls;
  using Sitecore.Web.UI.Sheer;
  using Text;

  /// <summary>
  /// The price control. 
  /// </summary>
  public class Price : Sitecore.Web.UI.HtmlControls.Control, IContentField
  {
    #region Constants

    /// <summary>
    /// Path to the Proce Metrix configuration item.
    /// </summary>
    private static readonly string PriceMatrixPath = "/sitecore/system/Modules/Ecommerce/PriceMatrix";

    #endregion

    #region Varibles

    /// <summary>
    /// </summary>
    private string fieldID = string.Empty;

    /// <summary>
    /// </summary>
    private string fieldName = string.Empty;

    /// <summary>
    /// </summary>
    private string itemID = string.Empty;

    /// <summary>
    /// </summary>
    private int priceMatrixConfigurationLevel;

    /// <summary>
    /// </summary>
    private string source = string.Empty;

    /// <summary>
    /// </summary>
    private const int StandardIndentLevel = 20;

    /// <summary>
    /// </summary>
    private const int StandardIndentTextBox = 100;

    #endregion Variables

    /// <summary>
    /// Initializes a new instance of the <see cref="Price"/> class.
    /// </summary>
    public Price()
    {
      this.Class = "scContentControl";
      Activation = true;
      this.Format = "g";
      this.NumberStyle = NumberStyles.Float;
    }

    #region Properties

    /// <summary>
    /// Gets or sets the number style.
    /// </summary>
    /// <value>The number style.</value>
    public NumberStyles NumberStyle { get; set; }

    /// <summary>
    /// Gets or sets the format.
    /// </summary>
    /// <value>The format.</value>
    public string Format { get; set; }

    /// <summary>
    /// </summary>
    public string FieldName
    {
      get
      {
        return this.fieldName;
      }

      set
      {
        this.fieldName = value;
      }
    }

    /// <summary>
    /// </summary>
    public string FieldID
    {
      get
      {
        return this.fieldID;
      }

      set
      {
        this.fieldID = value;
      }
    }

    /// <summary>
    /// </summary>
    public string ItemID
    {
      get
      {
        return this.itemID;
      }

      set
      {
        this.itemID = value;
      }
    }

    /// <summary>
    /// </summary>
    public string Source
    {
      get
      {
        return StringUtil.GetString(this.source);
      }

      set
      {
        if (value.IndexOf('&') > -1)
        {
          this.source = value.Substring(0, value.IndexOf('&'));
          if (value.ToLower().IndexOf("separator", value.IndexOf('&')) > -1)
          {
            string[] parameters = value.Split('&');
            for (int i = 1; i < parameters.Length; i++)
            {
              if (parameters[i].ToLower().IndexOf("separator") > -1)
              {
                this.Separator =
                  parameters[i].Substring((parameters[i].IndexOf("=") > -1)
                                            ? parameters[i].IndexOf("=") + 1
                                            : 0);
              }
            }
          }
        }
        else
        {
          this.source = value;
        }
      }
    }

    /// <summary>
    /// </summary>
    public string Separator
    {
      get
      {
        if (this.ViewState[this.ClientID + "_separator"] != null)
        {
          return this.ViewState[this.ClientID + "_separator"].ToString();
        }

        return ", ";
      }

      set
      {
        this.ViewState[this.ClientID + "_separator"] = value;
      }
    }

    /// <summary>
    /// </summary>
    public bool TrackModified
    {
      get
      {
        return this.GetViewStateBool("TrackModified", true);
      }

      set
      {
        this.SetViewStateBool("TrackModified", value, true);
      }
    }

    #endregion Properties

    #region Overrides

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      // First time the field is loaded in Sitecore shell
      string xml = this.Value;
      PriceMatrix priceMatrixItem = PriceMatrix.Load(xml);
      Item item = Sitecore.Context.ContentDatabase.SelectSingleItem(PriceMatrixPath);
      this.priceMatrixConfigurationLevel = item.Axes.Level;

      if (!Sitecore.Context.ClientPage.IsEvent)
      {
        var panel = new System.Web.UI.WebControls.Panel();
        panel.Attributes.Add("style", string.Format("padding: 4px, 10px, 10px, 10px; border: 1px solid #CECFCE; background-color: white;"));

        this.Controls.Add(panel);

        if (priceMatrixItem != null)
        {
          this.IterateRecursiveToLoad(priceMatrixItem.MainCategory, panel, item);
        }
        else
        {
          this.IterateRecursiveToLoad(null, panel, item);
        }
      }

      base.OnLoad(e);
    }

    /// <summary>
    /// Initalizes the price matrix.
    /// </summary>
    /// <param name="xml">The XML.</param>
    /// <returns></returns>
    public static PriceMatrix InitalizePriceMatrix(string xml)
    {
      Item item = Sitecore.Context.ContentDatabase.SelectSingleItem(PriceMatrixPath);
      PriceMatrix priceMatrix = PriceMatrix.Load(xml);
      if (priceMatrix == null)
      {
        priceMatrix = new PriceMatrix();
      }

      IterateRecursiveToSave(null, priceMatrix.MainCategory, item, null);
      return priceMatrix;
    }

    /// <summary>
    /// Iterates the recursive to load.
    /// </summary>
    /// <param name="priceMatrixItem">The price matrix item.</param>
    /// <param name="panel">The panel.</param>
    /// <param name="item">The item.</param>
    private void IterateRecursiveToLoad(IPriceMatrixItem priceMatrixItem, System.Web.UI.WebControls.Panel panel, Item item)
    {
      string key = item.Template.Key;
      int level = item.Axes.Level - this.priceMatrixConfigurationLevel - 1;
      if (key.Equals("pricematrix settings"))
      {
        ChildList children = item.Children;
        foreach (Item child in children)
        {
          IPriceMatrixItem categoryChild = null;
          if (priceMatrixItem is Category)
          {
            var category = (Category)priceMatrixItem;
            categoryChild = category.GetElement(child.Name);
          }

          this.IterateRecursiveToLoad(categoryChild, panel, child);
        }
      }
      else if (key.Equals("pricematrix site"))
      {
        var subPanel = new System.Web.UI.WebControls.Panel();
        subPanel.Attributes.Add("style", string.Format("display:block; padding: 5px, 0, 0, {0}px;", StandardIndentLevel * level));

        string title = item["title"];
        if (string.IsNullOrEmpty(title))
        {
          title = item.Name;
        }

        if (this.Controls == null)
        {
          throw new Exception("Container is null");
        }

        // Sitecore.Web.UI.HtmlControls.Panel subPanel = new Sitecore.Web.UI.HtmlControls.Panel();
        // subPanel.CssStyle = "";
        // subPanel.Padding = string.Format("0, 0, 0, {0}px;", 20 * level);
        // subPanel.Class = "";
        // subPanel.BorderWidth = new Unit(0);
        // panel.Controls.Add(subPanel);
        var lblTitle = new System.Web.UI.WebControls.Label();
        lblTitle.Text = title;
        lblTitle.Width = new Unit(StandardIndentTextBox + StandardIndentLevel);

        // lblTitle.Height = new Unit(20);

        // lblTitle.Padding = string.Format("0, 0, 0, {0}px;", 20 * level);
        // subPanel.Controls.Add(lblTitle);
        subPanel.Controls.Add(lblTitle);
        panel.Controls.Add(subPanel);

        ChildList children = item.Children;
        if (children.Count == 1)
        {
          lblTitle.Attributes.Add("style",
                                  string.Format("color: #313031; font-weight: bold; padding: 0 0 6px 0;"));

          // no subprice elements
          var textbox = new Text();
          subPanel.Controls.Add(textbox);
          Item child = item.Children[0];
          textbox.ID = this.GetID(item.Name + "_" + child.Name);
          textbox.Width = new Unit(100);

          IPriceMatrixItem categoryChild = null;
          if (priceMatrixItem is Category)
          {
            var category = (Category)priceMatrixItem;
            categoryChild = category.GetElement(child.Name);
          }

          if (categoryChild != null)
          {
            if (categoryChild is CategoryItem)
            {
              var categoryItem = (CategoryItem)categoryChild;
              textbox.Value = this.FormatNumber(categoryItem.Amount ?? String.Empty);

              // if (categoryItem.QuantityPrices.Count > 0)
              // lblQuantity.Text = string.Format("{0} quantity price", categoryItem.QuantityPrices.Count);
            }

            // else
            // {
            // // Something went wrong in the datastructure.
            // // Might be that the datastructure has been changed since last time saved.
            // // Therefor we'll just leave the field empty.
            // }
          }
        }
        else
        {
          lblTitle.Attributes.Add("style", string.Format("color: #313031; font-weight: bold; padding: 0 0 3px 0;"));
          foreach (Item child in children)
          {
            IPriceMatrixItem categoryChild = null;
            if (priceMatrixItem is Category)
            {
              var category = (Category)priceMatrixItem;
              categoryChild = category.GetElement(child.Name);
            }

            this.IterateRecursiveToLoad(categoryChild, panel, child);
          }
        }
      }
      else if (key.Equals("pricematrix price"))
      {
        var subPanel = new System.Web.UI.WebControls.Panel();
        subPanel.Attributes.Add("style", string.Format("display:block; padding: 0, 0, 0, {0}px;", StandardIndentLevel * level));
        var lblTitle = new System.Web.UI.WebControls.Label
        {
          Width = new Unit(StandardIndentTextBox)
        };
        string aTitle = item["title"];
        if (string.IsNullOrEmpty(aTitle))
        {
          aTitle = item.Name;
        }

        lblTitle.Text = aTitle;
        subPanel.Controls.Add(lblTitle);

        var textbox = new Text();
        subPanel.Controls.Add(textbox);
        textbox.ID = this.GetID(item.Parent.Name + "_" + item.Name);
        textbox.Width = new Unit(100);

        // textbox.Float = "left";
        // Set the value from the xml structure on the text box.

        // Commented out not in use
        // System.Web.UI.WebControls.Label lblQuantity = new System.Web.UI.WebControls.Label();
        // subPanel.Controls.Add(lblQuantity);

        if (priceMatrixItem != null)
        {
          if (priceMatrixItem is CategoryItem)
          {
            var categoryItem = (CategoryItem)priceMatrixItem;
            textbox.Value = this.FormatNumber(categoryItem.Amount ?? String.Empty);

            // if (categoryItem.QuantityPrices.Count > 0)
            // lblQuantity.Text = string.Format("{0} quantity price", categoryItem.QuantityPrices.Count);
          }

          // else
          // {
          // // Something went wrong in the datastructure.
          // // Might be that the datastructure has been changed since last time saved.
          // // Therefor we'll just leave the field empty.
          // }
        }

        // Sitecore.Web.UI.HtmlControls.Button imgBtn = new Sitecore.Web.UI.HtmlControls.Button();
        // imgBtn.ID = GetID("btn_" + item.Parent.Name + "_" + item.Name);
        ////imgBtn.Float = "left";
        // imgBtn.Header = "Add";
        ////imgBtn.Src = "/sitecore/shell/Themes/Standard/people/32x32/colors.png";
        ////imgBtn.Width = new Unit(32);
        ////imgBtn.Height = new Unit(32);
        // *Sitecore.Web.UI.HtmlControls.ThemedImage image = new ThemedImage();
        // btnLink.Controls.Add(image);
        // image.Height = new Unit(16);
        // image.Width = new Unit(16);
        // image.Src = */
        // subPanel.Controls.Add(imgBtn);

        ////Sitecore.Web.UI.HtmlControls.Button knap = new Sitecore.Web.UI.HtmlControls.Button();              
        ////subPanel.Controls.Add(knap);
        ////btnLink.Attributes.Add("Click", string.Format("{0}.ListItemClick(\"" + GetID(item.Parent.Name + "_" + item.Name) + "\")", this.ID));
        // imgBtn.ServerProperties["Click"] = string.Format("{0}.ListItemClick(\"" + GetID(item.Parent.Name + "_" + item.Name) + "\")", this.ID);
        ////knap.Header = "Add";   
        panel.Controls.Add(subPanel);
      }
    }


    /// <summary>
    /// Iterates the recursive to save.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="priceMatrixItem">The price matrix item.</param>
    /// <param name="item">The item.</param>
    /// <param name="controlCollection">The control collection.</param>
    private static void IterateRecursiveToSave(Category parent, IPriceMatrixItem priceMatrixItem, Item item, ControlCollection controlCollection)
    {
      string key = item.Template.Key;

      if (key.Equals("pricematrix settings"))
      {
        Category category = null;
        if (priceMatrixItem is Category)
        {
          category = (Category)priceMatrixItem;
        }

        ChildList children = item.Children;
        foreach (Item child in children)
        {
          IPriceMatrixItem categoryChild = null;
          if (category != null)
          {
            categoryChild = category.GetElement(child.Name);
          }

          IterateRecursiveToSave(category, categoryChild, child, controlCollection);
        }
      }
      else if (key.Equals("pricematrix site"))
      {
        string itmId = item.Name;

        /*
                        siteIds += itmId + "|";
        */
        string defaultId = item["standardprice"];
        string defaultName = string.Empty;
        if (!string.IsNullOrEmpty(defaultId))
        {
          Item defaultItem = Sitecore.Context.ContentDatabase.GetItem(defaultId);
          if (defaultItem != null)
          {
            /*
                                    defaultName = defaultItem.Name;
            */
          }
        }

        Category category;
        if (priceMatrixItem is Category)
        {
          category = (Category)priceMatrixItem;
          category.Id = itmId;
        }
        else
        {
          category = new Category(itmId);
          if (parent != null)
          {
            parent.AddCategory(category);
          }
        }

        ChildList children = item.Children;
        foreach (Item child in children)
        {
          IPriceMatrixItem categoryChild = null;
          if (category != null)
          {
            categoryChild = category.GetElement(child.Name);
          }

          IterateRecursiveToSave(category, categoryChild, child, controlCollection);
        }
      }
      else if (key.Equals("pricematrix price"))
      {
        string aTitle = item["title"];
        if (string.IsNullOrEmpty(aTitle))
        {
          /*
                              aTitle = item.Name;
          */
        }

        string itmchildId = item.Name;
        string priceIds = string.Empty;

        /*
                        priceIds += itmchildId + "|";
        */
        string itmchildPrice = string.Empty;

        // Check that it indeed finds the textbox.
        if (controlCollection != null)
        {
          var textBox = Utils.MainUtil.FindControl(item.Parent.Name + "_" + itmchildId, controlCollection) as Text;
          if (textBox != null)
          {
            itmchildPrice = textBox.Value;
          }
        }

        CategoryItem categoryItem;
        if (priceMatrixItem is CategoryItem)
        {
          categoryItem = (CategoryItem)priceMatrixItem;
          categoryItem.Id = itmchildId;
          categoryItem.Amount = itmchildPrice;
        }
        else
        {
          categoryItem = new CategoryItem(itmchildId, itmchildPrice);
          if (parent != null)
          {
            parent.AddItem(categoryItem);
          }
        }
      }
    }

    /// <summary>
    /// Generates the XML.
    /// </summary>
    /// <param name="pmroot">The pmroot.</param>
    /// <returns></returns>
    private string GenerateXml(Item pmroot)
    {
      string xml = this.Value;
      PriceMatrix priceMatrix = null;

      // PriceMatrix priceMatrix = PriceMatrixXmlParser.ReadXml(xml);
      if (priceMatrix == null)
      {
        priceMatrix = new PriceMatrix();
      }

      IterateRecursiveToSave(null, priceMatrix.MainCategory, pmroot, this.Controls);

      if (this.ValidateNumbers(priceMatrix.MainCategory))
      {
        var x = new XmlSerializer(priceMatrix.GetType());
        var sb = new StringBuilder();
        var settings = new XmlWriterSettings();
        settings.OmitXmlDeclaration = true; // Remove the <?xml version="1.0" encoding="utf-8"?>
        XmlWriter xw = XmlWriter.Create(sb, settings);
        var ns = new XmlSerializerNamespaces();
        ns.Add(string.Empty, string.Empty);
        x.Serialize(xw, priceMatrix, ns);
        return sb.ToString();
      }

      return string.Empty;
    }


    /// <summary>
    /// Validates the numbers.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <returns></returns>
    private bool ValidateNumbers(Category category)
    {
      foreach (Category subCategory in category.Categories)
      {
        foreach (CategoryItem item in subCategory.Items)
        {
          if (!this.SetNumber(ref item.Amount))
          {
            SheerResponse.Alert(Translate.Text("\"{0}\" is not a valid number", new object[] { item.Amount }), new string[0]);
            return false;
          }
        }

        if (subCategory.Categories.Count > 0)
        {
          return this.ValidateNumbers(subCategory);
        }
      }

      return true;
    }

    #endregion PriceMatrix load and save methods

    #region Event methods

    /// <summary>
    /// Lists the item click.
    /// </summary>
    /// <param name="id">The id.</param>
    public void ListItemClick(string id)
    {
      // ClientCommand clientCommand = Sitecore.Context.ClientPage.ClientResponse.ShowModalDialog(@"/sitecore/shell/Applications/PriceMatrixQuantityEditor.aspx?id=" + id, "400", "250", "", true);                              
      var args = new ClientPipelineArgs();
      args.Parameters["priceXmlName"] = id;
      Sitecore.Context.ClientPage.Start(this, "DialogProcessor", args);
    }

    /// <summary>
    /// Dialogs the processor.
    /// </summary>
    /// <param name="args">The args.</param>
    protected void DialogProcessor(ClientPipelineArgs args)
    {
      if (!args.IsPostBack)
      {
        // Show the modal dialog if it is not a post back
        string id = this.ItemID;
        string fldName = string.Empty;
        Item itm = Factory.GetDatabase("master").Items[this.ItemID];
        if (itm != null)
        {
          fldName = itm.Fields[this.FieldID].Name;
        }

        var priceXmlPath = args.Properties["priceXmlPath"] as string;
        ClientCommand clientCommand =
          Sitecore.Context.ClientPage.ClientResponse.ShowModalDialog(
            @"/sitecore/shell/Applications/PriceMatrixQuantityEditor.aspx?priceXmlPath=" + priceXmlPath +
            "&itemid=" + id + "&fieldName=" + fldName, "400", "250", string.Empty, true);

        // Sitecore.
        // Suspend the pipeline to wait for a postback and resume from another processor
        // args.WaitForPostBack();
      }
      else
      {
        // the result of a dialog is handled because a post back has occurred
        string res = args.Result;

        // Show an alert message box with the value from the modal dialog
        Sitecore.Context.ClientPage.ClientResponse.Alert(res);
      }
    }

    /// <summary>
    /// Sets the number.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="outValue">The out value.</param>
    /// <returns>Returns true if number is correct</returns>
    protected virtual bool SetNumber(ref string value)
    {
      if (!string.IsNullOrEmpty(value))
      {
        double num;
        CultureInfo cultureInfo = GetCultureInfo();
        if (!double.TryParse(value, this.NumberStyle, cultureInfo, out num))
        {
          return false;
        }
        value = num.ToString(CultureInfo.InvariantCulture);
      }
      else
      {
        value = string.Empty;
      }

      return true;
    }

    /// <summary>
    /// Formats the number.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    protected virtual string FormatNumber(string value)
    {
      double num;
      if (string.IsNullOrEmpty(value))
      {
        return string.Empty;
      }
      if (!double.TryParse(value, this.NumberStyle, CultureInfo.InvariantCulture, out num))
      {
        return value;
      }

      return num.ToString(this.Format, GetCultureInfo());
    }

    private static CultureInfo GetCultureInfo()
    {
      CultureInfo culture = Sitecore.Context.Culture;
      if (culture.IsNeutralCulture)
      {
        culture = Language.CreateSpecificCulture(culture.Name);
      }
      return culture;
    }

    #endregion Event methods

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <returns>The value of the field.</returns>
    public virtual string GetValue()
    {
      string value = this.GenerateXml(Sitecore.Context.ContentDatabase.SelectSingleItem(PriceMatrixPath));
      if (!string.IsNullOrEmpty(value))
      {
        this.Value = value;
      }

      return this.Value;
    }

    /// <summary>
    /// Sets the value.
    /// </summary>
    /// <param name="value">The value of the field.</param>
    public virtual void SetValue(string value)
    {
      this.Value = value;
    }
  }
}