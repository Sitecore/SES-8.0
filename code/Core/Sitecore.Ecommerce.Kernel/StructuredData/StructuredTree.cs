// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructuredTree.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   The structured tree.
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

namespace Sitecore.Ecommerce.StructuredData
{
  #region

  using System.Collections.Generic;
  using Configuration;
  using Diagnostics;  
  using Lucene.Net.Analysis;
  using Lucene.Net.Analysis.Standard;
  using Lucene.Net.Documents;
  using Lucene.Net.QueryParsers;
  using Lucene.Net.Search;
  using SecurityModel;
  using Sitecore.Data;
  using Sitecore.Data.Indexing;
  using Sitecore.Data.Items;
  using Sitecore.Search;

  #endregion

  /// <summary>
  /// The structured tree.
  /// </summary>
  public class StructuredTree
  {
    #region Fields

    /// <summary>
    /// The current folder.
    /// </summary>
    private readonly Item CurrentFolder;

    /// <summary>
    /// The folder master item.
    /// </summary>
    private readonly TemplateItem FolderMasterItem;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="StructuredTree"/> class.
    /// </summary>
    /// <param name="structuredTreeRootItem">
    /// The structured tree root item.
    /// </param>
    public StructuredTree(Item structuredTreeRootItem)
    {
      Assert.ArgumentNotNull(structuredTreeRootItem, "structuredTreeRootItem");

      Database db = structuredTreeRootItem.Database;

      this.FolderMasterItem = db.Templates["Ecommerce/StructuredData/StructuredData StructuredFolder"];
      this.StructuredTreeRootItem = structuredTreeRootItem;

      string id = this.StructuredTreeRootItem["enumeratorCurrentFolder"];
      if (!string.IsNullOrEmpty(id))
      {
        Item itm = db.GetItem(id);
        if (itm != null)
        {
          this.CurrentFolder = itm;
          return;
        }
      }

      if (this.CurrentFolder == null)
      {
        this.CurrentFolder = this.StructuredTreeRootItem;
      }
    }

    #endregion

    #region Private properties

    /// <summary>
    /// Gets the item limit in folder.
    /// </summary>
    /// <value>The item limit in folder.</value>
    private int StructuredDataFolderLevels
    {
      get
      {
        if (this.StructuredTreeRootItem != null)
        {
          int val;
          int.TryParse(this.StructuredTreeRootItem["StructuredDataFolderLevels"], out val);
          if (val != 0)
          {
            return val;
          }
        }

        // return default value.
        return 4;
      }
    }

    /// <summary>
    /// Gets or sets the structured tree root item.
    /// </summary>
    /// <value>The structured tree root item.</value>
    private Item StructuredTreeRootItem { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Adds an item to the structured tree root.
    /// The item is placed at the 
    /// </summary>
    /// <param name="item">
    /// The item.
    /// </param>
    public void AddItem(Item item)
    {
      string id = new ShortID(item.ID).ToString();
      Item currentNode = this.StructuredTreeRootItem;
      int level = 1;

      foreach (char letter in id)
      {
        Item child = currentNode.Children[letter.ToString()];
        if (child == null)
        {
          using (new SecurityDisabler())
          {
            using (new EditContext(currentNode))
            {
              child = currentNode.Add(letter.ToString(), this.FolderMasterItem);
            }
          }
        }

        currentNode = child;

        if (level == this.StructuredDataFolderLevels)
        {
          item.MoveTo(currentNode);
          return;
        }

        level += 1;
      }
    }

    /// <summary>
    /// Gets the indexes from the Use Search Form Settings
    /// </summary>
    /// <returns>
    /// Returns Indexes.
    /// </returns>
    public List<string> GetIndexes()
    {
      if (this.StructuredTreeRootItem != null)
      {
        List<string> indexes = new List<string>();

        // Find the Indexes/Index item which holds the id of the search Index or Indexes to use.
        string indexIDs = this.StructuredTreeRootItem["Use Search Form Settings"];
        if (!string.IsNullOrEmpty(indexIDs))
        {
          string[] indexesString = indexIDs.Split('|');
          foreach (string index in indexesString)
          {
            Item indexItem = this.StructuredTreeRootItem.Database.GetItem(index);
            if (indexItem != null)
            {
              string indexName = indexItem["name"];
              if (!string.IsNullOrEmpty(indexName))
              {
                indexes.Add(indexItem["name"]);
              }
            }
          }
        }

        return indexes;
      }

      return null;
    }

    /// <summary>
    /// Gets the items from the GetIndexes()
    /// </summary>
    /// <param name="queryString">
    /// The query string.
    /// </param>
    /// <param name="useQueryParser">
    /// if set to <c>true</c> [use query parser].
    /// </param>
    /// <returns>
    /// Returns Query Result
    /// </returns>
    public QueryResult GetItems(string queryString, bool useQueryParser)
    {
      // Result object used to pass result and errormessages back to sender.
      QueryResult result = new QueryResult();

      List<string> indexes = this.GetIndexes();
      string[] resultItemIds = null;
      foreach (string indexName in indexes)
      {
        string database = "master";
        HighResTimer timer = new HighResTimer(true);

        // get the specified index
        Sitecore.Search.Index searchIndex = SearchManager.GetIndex(indexName);

        // get the database to perform the search in..
        Database db = Factory.GetDatabase(database);

        SearchHits hits;
        try
        {
          if (useQueryParser)
          {
            using (IndexSearchContext searchContext = searchIndex.CreateSearchContext())
            {
              // get a new standard analyser so we can create a query..
              Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
              QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, queryString, analyzer);
              Query qry = queryParser.Query("_content");
              hits = searchContext.Search(qry, int.MaxValue);
            }
          }
          else
          {
            using (IndexSearchContext searchContext = searchIndex.CreateSearchContext())
            {
              // perform the search and get the results back as a Hits list..
              hits = searchContext.Search(queryString, int.MaxValue);
            }
          }

          result.Success = true;
          
          resultItemIds = new string[hits.Length];

          int i = 0;

          foreach (Document document in hits.Documents)
          {
            string str = document.Get("_docID");
            resultItemIds[i++] = str;
          }
        }
        catch (ParseException e)
        {
          result.ErrorMessage = e.Message;
          result.ParseException = e;
          result.Success = false;
        }
      }

      result.Result = resultItemIds;
      return result;
    }

    /// <summary>
    /// Gets the templates in indexes.
    /// </summary>
    /// <returns>
    /// Returns templates.
    /// </returns>
    public List<string> GetTemplatesInIndexes()
    {
      if (this.StructuredTreeRootItem != null)
      {
        List<string> templates = new List<string>();

        // Find the Indexes/Index item which holds the id of the search Index or Indexes to use.
        List<string> indexes = this.GetIndexes();
        foreach (string index in indexes)
        {
          Item indexItem = this.StructuredTreeRootItem.Database.GetItem(index);
          if (indexItem != null)
          {
            string indexName = indexItem["includeItemOfTemplates"];
            if (!string.IsNullOrEmpty(indexName))
            {
              templates.Add(indexItem["name"]);
            }
          }
        }

        return templates;
      }

      return null;
    }

    #endregion
  }
}