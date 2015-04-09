// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateBranchLinksCommand.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines template command used to update links after creating items from branch.
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

namespace Sitecore.Ecommerce.CommandTemplates
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Shell.Framework.Commands;

  /// <summary>
  /// Defines template command used to update links after creating items from branch.
  /// </summary>
  public class UpdateBranchLinksCommand : Command
  {
    /// <summary>
    /// Defines the "Content database must not be null." phrase.
    /// </summary>
    private const string ContentDatabaseMustNotBeNull = "Content database must not be null.";

    /// <summary>
    /// Defines the "Item cannot be found." phrase.
    /// </summary>
    private const string ItemCannotBeFound = "Item cannot be found.";

    /// <summary>
    /// The branch cannot be found.
    /// </summary>
    private const string BranchCannotBeFound = "Branch cannot be found";

    /// <summary>
    /// The item key.
    /// </summary>
    private const string ItemKey = "itemId";

    /// <summary>
    /// The branch key.
    /// </summary>
    private const string BranchKey = "branchId";

    /// <summary>
    /// The default item name key.
    /// </summary>
    private const string NameKey = "name";

    /// <summary>
    /// The branch link updater.
    /// </summary>
    private BranchLinkUpdater branchLinkUpdater;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateBranchLinksCommand" /> class.
    /// </summary>
    public UpdateBranchLinksCommand()
    {
      this.branchLinkUpdater = new BranchLinkUpdater();
    }

    /// <summary>
    /// Gets or sets the the branch link updater.
    /// </summary>
    /// <value>The branch link updater.</value>
    [NotNull]
    public BranchLinkUpdater BranchLinkUpdater
    {
      get
      {
        return this.branchLinkUpdater;
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.branchLinkUpdater = value;
      }
    }

    /// <summary>
    /// Executes the command in the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    public override void Execute(CommandContext context)
    {
      Assert.ArgumentNotNull(context, "context");
      Assert.IsNotNull(Context.ContentDatabase, ContentDatabaseMustNotBeNull);

      Item item = Context.ContentDatabase.GetItem(context.Parameters[ItemKey]);
      Item branchInnerItem = Context.ContentDatabase.GetItem(context.Parameters[BranchKey]);

      Assert.IsNotNull(item, ItemCannotBeFound);
      Assert.IsNotNull(branchInnerItem, BranchCannotBeFound);

      BranchItem branchItem = new BranchItem(branchInnerItem);

      this.BranchLinkUpdater.UpdateItemLinks(branchItem.AddTo(item, context.Parameters[NameKey]), branchItem);
    }
  }
}
