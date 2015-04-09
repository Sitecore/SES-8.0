// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderStateList.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderStateList type.
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

namespace Sitecore.Ecommerce.Apps.Web.UI.WebControls
{
  using System;
  using System.Collections;
  using System.Linq;
  using System.Web.UI;
  using System.Web.UI.WebControls;
  using Ecommerce.OrderManagement.Orders;
  using Newtonsoft.Json;
  using OrderManagement.Models;
  using Sitecore.Web.UI.WebControls;
  using Sitecore.Web.UI.WebControls.Specialized;

  /// <summary>
  /// Defines the state substate class.
  /// </summary>
  [FieldControl("OrderStateList")]
  [ToolboxData("<{0}:OrderStateList runat=\"server\"></{0}:OrderStateList>")]
  public class OrderStateList : System.Web.UI.WebControls.DataBoundControl, Sitecore.Web.UI.WebControls.Specialized.IFieldControl
  {
    /// <summary>
    /// Stores reference to the drop down list of states.
    /// </summary>
    private readonly DropDownList stateDropDownList;

    /// <summary>
    /// Stores reference to the control that serves as a container for all related substates.
    /// </summary>
    private readonly System.Web.UI.HtmlControls.HtmlGenericControl substateContainer;

    /// <summary>
    /// Stores reference to the drop down list of sub-states.
    /// </summary>
    private readonly ListView substateList;

    /// <summary>
    /// Stores reference to the list of available states.
    /// </summary>
    private readonly System.Collections.Generic.ICollection<StateModel> availableStates;

    /// <summary>
    /// Stores reference to the control container.
    /// </summary>
    private UpdatePanel controlContainer;

    /// <summary>
    /// Stores reference to the current state object.
    /// </summary>
    private State currentState;

    /// <summary>
    /// Stores reference to the restored instance of order state.
    /// </summary>
    private State restoredState;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderStateList"/> class.
    /// </summary>
    public OrderStateList()
    {
      this.availableStates = new System.Collections.Generic.List<StateModel>();

      this.stateDropDownList = new DropDownList
      {
        DataTextField = "Name",
        DataValueField = "Code",
        ID = "stateDropDownList"
      };
      this.substateContainer = new System.Web.UI.HtmlControls.HtmlGenericControl("ul")
      {
        ID = "SubstateContainer"
      };
      this.substateList = new ListView
      {
        ID = "SubstateList"
      };

      this.substateContainer.Attributes["class"] = "substates";

      this.substateList.LayoutTemplate = new SubstateListLayoutTemplate();
      this.substateList.ItemTemplate = new SubstateListItemTemplate(this);

      this.stateDropDownList.SelectedIndexChanged += this.StateDropDownListOnSelectedIndexChanged;
      this.stateDropDownList.DataBound += this.StateDropDownListOnDataBound;

      this.substateList.DataBinding += this.SubstateList_OnDataBinding;
    }

    /// <summary>
    /// Occurs when current state changing.
    /// </summary>
    public event EventHandler<CurrentStateChangingEventArgs> CurrentStateChanging;

    /// <summary>
    /// Occurs when current state changed.
    /// </summary>
    public event EventHandler<CurrentStateChangedEventArgs> CurrentStateChanged;

    /// <summary>
    /// Occurs when active substates changing.
    /// </summary>
    public event EventHandler<ActiveSubstateChangingEventArgs> ActiveSubstateChanging;

    /// <summary>
    /// Occurs when active substates changed.
    /// </summary>
    public event EventHandler<ActiveSubstateChangedEventArgs> ActiveSubstateChanged;

    /// <summary>
    /// Occurs when [substate control data bound].
    /// </summary>
    public event EventHandler<SubstateControlDataBoundEventArgs> SubstateControlDataBound;

    /// <summary>
    /// Gets or sets the state of the current.
    /// </summary>
    /// <value>The state of the current.</value>
    public State CurrentState
    {
      get
      {
        if ((this.currentState != null) && (string.IsNullOrEmpty(this.currentState.Code) || (!this.AvailableStates.Select(state => state.Code).Contains(this.currentState.Code))))
        {
          return null;
        }

        return this.currentState;
      }

      set
      {
        if (!this.IsCurrentStateDirty)
        {
          this.currentState = value;
          this.IsCurrentStateDirty = true;
        }
      }
    }

    /// <summary>
    /// Gets the available states.
    /// </summary>
    /// <value>The available states.</value>
    public System.Collections.Generic.ICollection<StateModel> AvailableStates
    {
      get
      {
        return this.availableStates;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the control is in read only state.
    /// </summary>
    /// <value><c>true</c> if control is in read only state; otherwise, <c>false</c>.</value>
    public virtual bool ReadOnly
    {
      get
      {
        return !this.StateDropDownList.Enabled;
      }

      set
      {
        this.StateDropDownList.Enabled = !value;
        this.SubstateList.Enabled = !value;
      }
    }

    /// <summary>
    /// Gets or sets the source.
    /// </summary>
    /// <value>The source.</value>
    public virtual string Source { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to disable substate animation.
    /// </summary>
    /// <value>
    /// <c>true</c> if to disable substate animation; otherwise, <c>false</c>.
    /// </value>
    public bool DisableSubstateAnimation { get; set; }

    /// <summary>
    /// Gets the control container.
    /// </summary>
    /// <value>The control container.</value>
    protected UpdatePanel ControlContainer
    {
      get
      {
        return this.controlContainer;
      }
    }

    /// <summary>
    /// Gets the state list.
    /// </summary>
    /// <value>The state list.</value>
    protected DropDownList StateDropDownList
    {
      get
      {
        return this.stateDropDownList;
      }
    }

    /// <summary>
    /// Gets the substate container.
    /// </summary>
    /// <value>The substate container.</value>
    protected System.Web.UI.HtmlControls.HtmlGenericControl SubstateContainer
    {
      get
      {
        return this.substateContainer;
      }
    }

    /// <summary>
    /// Gets the substate list.
    /// </summary>
    /// <value>The substate list.</value>
    protected ListView SubstateList
    {
      get
      {
        return this.substateList;
      }
    }

    /// <summary>
    /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> value that corresponds to this Web server control. This property is used primarily by control developers.
    /// </summary>
    /// <value></value>
    /// <returns>One of the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> enumeration values.</returns>
    protected override HtmlTextWriterTag TagKey
    {
      get
      {
        return HtmlTextWriterTag.Div;
      }
    }

    /// <summary>
    /// Gets the state of the restored.
    /// </summary>
    /// <value>The state of the restored.</value>
    [CanBeNull]
    protected State RestoredState
    {
      get
      {
        return this.restoredState;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is current state dirty.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is current state dirty; otherwise, <c>false</c>.
    /// </value>
    private bool IsCurrentStateDirty { get; set; }

    /// <summary>
    /// Gets the control.
    /// </summary>
    /// <param name="readOnly">if set to <c>true</c> read only instance of the control will be returned.</param>
    /// <param name="enabled">if set to <c>true</c> the control is enabled, otherwise it is not.</param>
    /// <param name="visible">if set to <c>true</c> the control is visible, otherwise it is not.</param>
    /// <returns>
    /// The control.
    /// </returns>
    public virtual Control GetControl(bool readOnly, bool enabled, bool visible)
    {
      return null;
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <returns>The value.</returns>
    public virtual string GetValue()
    {
      return JsonConvert.SerializeObject(this.SaveControlState());
    }

    /// <summary>
    /// Sets the value.
    /// </summary>
    /// <param name="value">The value.</param>
    public virtual void SetValue(string value)
    {
      this.OverwriteCurrentState(JsonConvert.DeserializeObject<ControlStateHolder>(value));
    }

    /// <summary>
    /// Called when the state has changing.
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected virtual void OnCurrentStateChanging(CurrentStateChangingEventArgs e)
    {
      EventHandler<CurrentStateChangingEventArgs> handler = this.CurrentStateChanging;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    /// <summary>
    /// Called when the state has changed.
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected virtual void OnCurrentStateChanged(CurrentStateChangedEventArgs e)
    {
      EventHandler<CurrentStateChangedEventArgs> handler = this.CurrentStateChanged;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    /// <summary>
    /// Called when the substates has changing.
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected virtual void OnActiveSubstateChanging(ActiveSubstateChangingEventArgs e)
    {
      EventHandler<ActiveSubstateChangingEventArgs> handler = this.ActiveSubstateChanging;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    /// <summary>
    /// Called when the substates has changed.
    /// </summary>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected virtual void OnActiveSubstateChanged(ActiveSubstateChangedEventArgs e)
    {
      EventHandler<ActiveSubstateChangedEventArgs> handler = this.ActiveSubstateChanged;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    /// <summary>
    /// Called when the substate control data has bound.
    /// </summary>
    /// <param name="e">The <see cref="SubstateControlDataBoundEventArgs"/> instance containing the event data.</param>
    protected virtual void OnSubstateControlDataBound(SubstateControlDataBoundEventArgs e)
    {
      EventHandler<SubstateControlDataBoundEventArgs> handler = this.SubstateControlDataBound;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    /// <summary>
    /// When overridden in a derived class, binds data from the data source to the control.
    /// </summary>
    /// <param name="data">The <see cref="T:System.Collections.IEnumerable"/> list of data returned from a <see cref="M:System.Web.UI.WebControls.DataBoundControl.PerformSelect"/> method call.</param>
    protected override void PerformDataBinding(IEnumerable data)
    {
      base.PerformDataBinding(data);

      if (data is System.Collections.Generic.IEnumerable<StateModel>)
      {
        this.BindAvailableStateList((System.Collections.Generic.IEnumerable<StateModel>)data);
      }
    }

    /// <summary>
    /// Handles the <see cref="E:System.Web.UI.Control.Init"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnInit(System.EventArgs e)
    {
      base.OnInit(e);

      this.CssClass = "state-substate";

      this.CreateControlTree();
      this.DataBind();
      this.BindControls();

      this.Page.RegisterRequiresControlState(this);
    }

    /// <summary>
    /// Restores control-state information from a previous page request that was saved by the <see cref="M:System.Web.UI.Control.SaveControlState"/> method.
    /// </summary>
    /// <param name="savedState">An <see cref="T:System.Object"/> that represents the control state to be restored.</param>
    protected override void LoadControlState(object savedState)
    {
      if (savedState is ControlStateHolder)
      {
        ControlStateHolder controlStateHolder = (ControlStateHolder)savedState;

        State temporaryState = this.CurrentState;

        this.OverwriteCurrentState(controlStateHolder);

        this.BindControls();

        this.restoredState = this.CurrentState;
        this.OverwriteCurrentState(temporaryState);
      }

      base.LoadControlState(savedState);
    }

    /// <summary>
    /// Saves any server control state changes that have occurred since the time the page was posted back to the server.
    /// </summary>
    /// <returns>
    /// Returns the server control's current state. If there is no state associated with the control, this method returns null.
    /// </returns>
    protected override object SaveControlState()
    {
      ControlStateHolder result = new ControlStateHolder();

      if (this.CurrentState != null)
      {
        result.CurrentStateCode = this.CurrentState.Code;

        if (this.CurrentState.Substates != null)
        {
          result.SubstateStatuses = this.CurrentState.Substates.ToDictionary(substate => substate.Code, substate => substate.Active);
        }
      }

      return result;
    }

    /// <summary>
    /// Handles the <see cref="E:System.Web.UI.Control.Load"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains event data.</param>
    protected override void OnLoad([NotNull] EventArgs e)
    {
      base.OnLoad(e);

      if (this.RestoredState != null)
      {
        this.OverwriteCurrentState(this.RestoredState);
      }
    }

    /// <summary>
    /// Handles the <see cref="E:System.Web.UI.Control.PreRender"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnPreRender(System.EventArgs e)
    {
      base.OnPreRender(e);

      this.BindControls();
      this.RegisterClientScripts();
      this.HideUnnecessaryControls();
    }

    /// <summary>
    /// Adds the controls.
    /// </summary>
    protected virtual void AddControls()
    {
      this.Controls.Add(this.ControlContainer);
    }

    /// <summary>
    /// Binds the available state list.
    /// </summary>
    /// <param name="states">The states.</param>
    protected virtual void BindAvailableStateList(System.Collections.Generic.IEnumerable<StateModel> states)
    {
      this.AvailableStates.Clear();

      if (states != null)
      {
        foreach (StateModel state in states)
        {
          this.AvailableStates.Add(state);
        }
      }
    }

    /// <summary>
    /// Binds the controls.
    /// </summary>
    protected virtual void BindControls()
    {
      this.StateDropDownList.DataSource = this.AvailableStates;
      this.StateDropDownList.DataBind();

      if (this.CurrentState == null)
      {
        this.StateDropDownList.SelectedValue = null;
        this.SubstateList.Items.Clear();
      }
      else
      {
        this.StateDropDownList.SelectedValue = this.CurrentState.Code;

        this.SubstateList.DataSource = this.CurrentState.Substates;
        this.SubstateList.DataBind();
      }
    }

    /// <summary>
    /// Registers the client scripts.
    /// </summary>
    protected virtual void RegisterClientScripts()
    {
      this.DisableSubstateAnimation = true;

      this.stateDropDownList.Attributes["onchange"] = string.Format("var s=$('#{0}');if(s.length!=0)s.toggle('slide',{{direction:'up'}},300,function(){{{1}}});else {1};", this.SubstateContainer.ClientID, this.Page.ClientScript.GetPostBackEventReference(this.StateDropDownList, string.Empty));
      ScriptManagerExtensions.RegisterStartupScript(ScriptManager.GetCurrent(this.Page), string.Format(this.DisableSubstateAnimation ? "$('#{0}').show()" : "$('#{0}').toggle('slide',{{direction:'up'}},300)", this.SubstateContainer.ClientID), this.SubstateContainer);
    }

    /// <summary>
    /// Hides the unnecessary controls.
    /// </summary>
    protected virtual void HideUnnecessaryControls()
    {
      this.SubstateContainer.Visible = this.SubstateList.Items.Count > 0;
    }

    /// <summary>
    /// Overwrites the state of the current.
    /// </summary>
    /// <param name="value">The value.</param>
    protected virtual void OverwriteCurrentState(State value)
    {
      this.IsCurrentStateDirty = false;
      this.CurrentState = value;
    }

    /// <summary>
    /// States from code.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <returns>The from code.</returns>
    private State GetStateFromCode(string code)
    {
      if (string.IsNullOrEmpty(code))
      {
        return null;
      }

      var availableState = this.AvailableStates.SingleOrDefault(state => state.Code == code);

      return availableState == null ? null : availableState.InnerState;
    }

    /// <summary>
    /// Handles the OnSelectedIndexChanged event of the stateDropDownList control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void StateDropDownListOnSelectedIndexChanged(object sender, System.EventArgs e)
    {
      if (!this.ReadOnly)
      {
        CurrentStateChangingEventArgs eventArgs = new CurrentStateChangingEventArgs(this.CurrentState, this.GetStateFromCode(this.StateDropDownList.SelectedValue));

        this.OnCurrentStateChanging(eventArgs);
        if (!eventArgs.Cancel)
        {
          this.OverwriteCurrentState(eventArgs.NewState);
          this.OnCurrentStateChanged(new CurrentStateChangedEventArgs(eventArgs.NewState));
        }
      }
    }

    /// <summary>
    /// Handles the DataBound event of the stateDropDownList control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void StateDropDownListOnDataBound(object sender, EventArgs e)
    {
      foreach (StateModel state in this.AvailableStates)
      {
        ListItem item = this.stateDropDownList.Items.FindByValue(state.Code);
        if (!state.Enabled)
        {
          item.Attributes.Add("style", "color:gray;");
          item.Attributes.Add("disabled", "true");
          item.Value = "-1";
        }
      }
    }

    /// <summary>
    /// Handles the OnCheckedChanged event of the Substate control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void Substate_OnCheckedChanged(object sender, System.EventArgs e)
    {
      if ((this.CurrentState != null) && (this.CurrentState.Substates != null) && (!this.ReadOnly))
      {
        System.Web.UI.WebControls.CheckBox substateCheckBox = (System.Web.UI.WebControls.CheckBox)sender;
        Substate substate = this.CurrentState.Substates.Single(s => s.Code == substateCheckBox.ID);
        ActiveSubstateChangingEventArgs eventArgs = new ActiveSubstateChangingEventArgs(substate, substateCheckBox.Checked);

        this.DisableSubstateAnimation = true;

        this.OnActiveSubstateChanging(eventArgs);
        if (!eventArgs.Cancel)
        {
          substate.Active = substateCheckBox.Checked;
          this.OnActiveSubstateChanged(new ActiveSubstateChangedEventArgs(substate));
        }
      }
    }

    /// <summary>
    /// Handles the OnDataBinding event of the SubstateList control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void SubstateList_OnDataBinding(object sender, System.EventArgs e)
    {
      System.Collections.Generic.IEnumerable<string> controlIDs = this.SubstateList.Items.Select(dataItem => dataItem.Controls.OfType<System.Web.UI.WebControls.CheckBox>().Single().UniqueID);
      UpdatePanelControlTrigger[] triggersToRemove = this.ControlContainer.Triggers.OfType<UpdatePanelControlTrigger>().Where(trigger => controlIDs.Contains(trigger.ControlID)).ToArray();

      foreach (UpdatePanelControlTrigger trigger in triggersToRemove)
      {
        this.ControlContainer.Triggers.Remove(trigger);
      }
    }

    /// <summary>
    /// Handles the OnDataBinding event of the Substate control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void Substate_OnDataBinding(object sender, System.EventArgs e)
    {
      System.Web.UI.WebControls.CheckBox substateCheckBox = (System.Web.UI.WebControls.CheckBox)sender;
      Substate dataItem = (Substate)this.Page.GetDataItem();

      substateCheckBox.ID = dataItem.Code;
      substateCheckBox.Checked = dataItem.Active;
      substateCheckBox.Text = dataItem.Name;

      this.RegisterSubstateUpdateDependency(substateCheckBox);

      this.OnSubstateControlDataBound(new SubstateControlDataBoundEventArgs(substateCheckBox, dataItem));
    }

    /// <summary>
    /// Overwrites the state of the current.
    /// </summary>
    /// <param name="value">The value.</param>
    private void OverwriteCurrentState(ControlStateHolder value)
    {
      this.OverwriteCurrentState(string.IsNullOrEmpty(value.CurrentStateCode) ? null : this.AvailableStates.SingleOrDefault(state => state.Code == value.CurrentStateCode).InnerState);

      if ((this.CurrentState != null) && (this.CurrentState.Substates != null))
      {
        foreach (Substate substate in this.CurrentState.Substates)
        {
          substate.Active = value.SubstateStatuses[substate.Code];
        }
      }
    }

    /// <summary>
    /// Creates the control tree.
    /// </summary>
    private void CreateControlTree()
    {
      this.CreateControlContainer();
      this.AddControls();
    }

    /// <summary>
    /// Creates the control container.
    /// </summary>
    private void CreateControlContainer()
    {
      this.controlContainer = this.IsPopupPage() ? new Sitecore.Web.UI.WebControls.Specialized.PopupUpdatePanel() : new UpdatePanel();
      this.controlContainer.ID = "ControlContainer";
      this.controlContainer.ContentTemplate = new ControlContainerContentTemplate(this);
    }

    /// <summary>
    /// Registers the state list update dependency.
    /// </summary>
    private void RegisterStateListUpdateDependency()
    {
      this.ControlContainer.Triggers.Add(new AsyncPostBackTrigger { ControlID = this.StateDropDownList.UniqueID, EventName = "SelectedIndexChanged" });
    }

    /// <summary>
    /// Registers the substate update dependency.
    /// </summary>
    /// <param name="substateCheckBox">The substate check box.</param>
    private void RegisterSubstateUpdateDependency(System.Web.UI.WebControls.CheckBox substateCheckBox)
    {
      this.ControlContainer.Triggers.Add(new AsyncPostBackTrigger { ControlID = substateCheckBox.UniqueID, EventName = "CheckedChanged" });
    }

    /// <summary>
    /// Determines whether the page is popup page.
    /// </summary>
    /// <returns>
    /// <c>true</c> if popup page otherwise, <c>false</c>.
    /// </returns>
    private bool IsPopupPage()
    {
      return typeof(PopupPage).IsAssignableFrom(this.Page.GetType());
    }

    /// <summary>
    /// Defines the control state holder class.
    /// </summary>
    [Serializable]
    private sealed class ControlStateHolder
    {
      /// <summary>
      /// Gets or sets the current state code.
      /// </summary>
      /// <value>The current state code.</value>
      public string CurrentStateCode { get; set; }

      /// <summary>
      /// Gets or sets the substate statuses.
      /// </summary>
      /// <value>The substate statuses.</value>
      public System.Collections.Generic.Dictionary<string, bool> SubstateStatuses { get; set; }
    }

    /// <summary>
    /// Defines the substate list layout template class.
    /// </summary>
    private sealed class SubstateListLayoutTemplate : ITemplate
    {
      /// <summary>
      /// When implemented by a class, defines the <see cref="T:System.Web.UI.Control"/> object that child controls and templates belong to. These child controls are in turn defined within an inline template.
      /// </summary>
      /// <param name="container">The <see cref="T:System.Web.UI.Control"/> object to contain the instances of controls from the inline template. </param>
      public void InstantiateIn(Control container)
      {
        container.Controls.Add(new System.Web.UI.HtmlControls.HtmlGenericControl("li") { ID = "itemPlaceholder", TemplateControl = container.Page });
      }
    }

    /// <summary>
    /// Defines the substate list item template class.
    /// </summary>
    private sealed class SubstateListItemTemplate : ITemplate
    {
      /// <summary>
      /// Stores reference to the owner control.
      /// </summary>
      private readonly OrderStateList owner;

      /// <summary>
      /// Initializes a new instance of the <see cref="SubstateListItemTemplate"/> class.
      /// </summary>
      /// <param name="owner">The owner.</param>
      public SubstateListItemTemplate(OrderStateList owner)
      {
        this.owner = owner;
      }

      /// <summary>
      /// When implemented by a class, defines the <see cref="T:System.Web.UI.Control"/> object that child controls and templates belong to. These child controls are in turn defined within an inline template.
      /// </summary>
      /// <param name="container">The <see cref="T:System.Web.UI.Control"/> object to contain the instances of controls from the inline template.</param>
      public void InstantiateIn(Control container)
      {
        System.Web.UI.WebControls.CheckBox substateCheckBox = new System.Web.UI.WebControls.CheckBox();

        substateCheckBox.AutoPostBack = true;
        substateCheckBox.CheckedChanged += this.owner.Substate_OnCheckedChanged;
        substateCheckBox.DataBinding += this.owner.Substate_OnDataBinding;

        container.Controls.Add(new LiteralControl("<li>"));
        container.Controls.Add(substateCheckBox);
        container.Controls.Add(new LiteralControl("</li>"));
      }
    }

    /// <summary>
    /// Defines the control container content template class.
    /// </summary>
    private sealed class ControlContainerContentTemplate : ITemplate
    {
      /// <summary>
      /// Stores reference to the owner control.
      /// </summary>
      private readonly OrderStateList owner;

      /// <summary>
      /// Initializes a new instance of the <see cref="ControlContainerContentTemplate"/> class.
      /// </summary>
      /// <param name="owner">The owner.</param>
      public ControlContainerContentTemplate(OrderStateList owner)
      {
        this.owner = owner;
      }

      /// <summary>
      /// When implemented by a class, defines the <see cref="T:System.Web.UI.Control"/> object that child controls and templates belong to. These child controls are in turn defined within an inline template.
      /// </summary>
      /// <param name="container">The <see cref="T:System.Web.UI.Control"/> object to contain the instances of controls from the inline template. </param>
      public void InstantiateIn(Control container)
      {
        container.Controls.Add(this.owner.StateDropDownList);
        container.Controls.Add(this.owner.SubstateContainer);
        this.owner.SubstateContainer.Controls.Add(this.owner.SubstateList);

        this.owner.RegisterStateListUpdateDependency();
      }
    }
  }
}