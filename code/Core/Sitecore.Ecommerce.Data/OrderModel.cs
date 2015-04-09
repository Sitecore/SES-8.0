// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderModel.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the OrderModel type.
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

namespace Sitecore.Ecommerce.Data
{
  using System.Collections;
  using System.Data;
  using System.Data.Entity;
  using System.Data.Entity.Infrastructure;
  using System.Data.Entity.ModelConfiguration.Conventions;
  using System.Data.Metadata.Edm;
  using System.Data.Objects;
  using System.Linq;
  using Common;
  using Diagnostics;
  using Ecommerce.OrderManagement.Orders;
  using ModelConfiguration;
  using Sitecore.Ecommerce.OrderManagement.OrderProcessing;

  /// <summary>
  /// Defines the order model class.
  /// </summary>
  public class OrderModel : DbContext, IOrdersContext
  {
    /// <summary>
    /// Is set to true when instance is disposed.
    /// </summary>
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderModel"/> class.
    /// </summary>
    /// <param name="nameOrConnectionString">The name or connection string.</param>
    public OrderModel([NotNull] string nameOrConnectionString)
      : base(nameOrConnectionString)
    {
      Assert.ArgumentNotNull(nameOrConnectionString, "nameOrConnectionString");

      this.Configuration.LazyLoadingEnabled = true;
    }

    /// <summary>
    /// Gets or sets the orders.
    /// </summary>
    /// <value>The orders.</value>
    public virtual IDbSet<Order> Orders { get; set; }

    /// <summary>
    /// Gets or sets the order lines.
    /// </summary>
    /// <value>The order lines.</value>
    public virtual DbSet<OrderLine> OrderLines { get; set; }

    /// <summary>
    /// Gets or sets the addresses.
    /// </summary>
    /// <value>The addresses.</value>
    public virtual DbSet<Address> Addresses { get; set; }

    /// <summary>
    /// Gets or sets the allowance charges.
    /// </summary>
    /// <value>The allowance charges.</value>
    public virtual DbSet<AllowanceCharge> AllowanceCharges { get; set; }

    /// <summary>
    /// Gets or sets the contacts.
    /// </summary>
    /// <value>The contacts.</value>
    public virtual DbSet<Contact> Contacts { get; set; }

    /// <summary>
    /// Gets or sets the communications.
    /// </summary>
    /// <value>The communications.</value>
    public virtual DbSet<Communication> Communications { get; set; }

    /// <summary>
    /// Gets or sets the customer parties.
    /// </summary>
    /// <value>The customer parties.</value>
    public virtual DbSet<CustomerParty> CustomerParties { get; set; }

    /// <summary>
    /// Gets or sets the deliveries.
    /// </summary>
    /// <value>The deliveries.</value>
    public virtual DbSet<Delivery> Deliveries { get; set; }

    /// <summary>
    /// Gets or sets the des patches.
    /// </summary>
    /// <value>The des patches.</value>
    public virtual DbSet<Despatch> Despatches { get; set; }

    /// <summary>
    /// Gets or sets the line items.
    /// </summary>
    /// <value>The line items.</value>
    public virtual DbSet<LineItem> LineItems { get; set; }

    /// <summary>
    /// Gets or sets the locations.
    /// </summary>
    /// <value>The locations.</value>
    public virtual DbSet<Location> Locations { get; set; }

    /// <summary>
    /// Gets or sets the ordered shipments.
    /// </summary>
    /// <value>The ordered shipments.</value>
    public virtual DbSet<OrderedShipment> OrderedShipments { get; set; }

    /// <summary>
    /// Gets or sets the parties.
    /// </summary>
    /// <value>The parties.</value>
    public virtual DbSet<Party> Parties { get; set; }

    /// <summary>
    /// Gets or sets the party legal entities.
    /// </summary>
    /// <value>The party legal entities.</value>
    public virtual DbSet<PartyLegalEntity> PartyLegalEntities { get; set; }

    /// <summary>
    /// Gets or sets the party tax schemes.
    /// </summary>
    /// <value>The party tax schemes.</value>
    public virtual DbSet<PartyTaxScheme> PartyTaxSchemes { get; set; }

    /// <summary>
    /// Gets or sets the payment means.
    /// </summary>
    /// <value>The payment means.</value>
    public virtual DbSet<PaymentMeans> PaymentMeans { get; set; }

    /// <summary>
    /// Gets or sets the prices.
    /// </summary>
    /// <value>The prices.</value>
    public virtual DbSet<Price> Prices { get; set; }

    /// <summary>
    /// Gets or sets the shipments.
    /// </summary>
    /// <value>The shipments.</value>
    public virtual DbSet<Shipment> Shipments { get; set; }

    /// <summary>
    /// Gets or sets the supplier parties.
    /// </summary>
    /// <value>The supplier parties.</value>
    public virtual DbSet<SupplierParty> SupplierParties { get; set; }

    /// <summary>
    /// Gets or sets the tax categories.
    /// </summary>
    /// <value>The tax categories.</value>
    public virtual DbSet<TaxCategory> TaxCategories { get; set; }

    /// <summary>
    /// Gets or sets the tax sub totals.
    /// </summary>
    /// <value>The tax sub totals.</value>
    public virtual DbSet<TaxSubTotal> TaxSubTotals { get; set; }

    /// <summary>
    /// Gets or sets the tax totals.
    /// </summary>
    /// <value>The tax totals.</value>
    public virtual DbSet<TaxTotal> TaxTotals { get; set; }

    /// <summary>
    /// Gets or sets the order states.
    /// </summary>
    /// <value>The order states.</value>
    public virtual DbSet<State> OrderStates { get; set; }

    /// <summary>
    /// Gets or sets the sub states.
    /// </summary>
    /// <value>The sub states.</value>
    public virtual DbSet<Substate> Substates { get; set; }

    /// <summary>
    /// Gets or sets the monetary totals.
    /// </summary>
    /// <value>The monetary totals.</value>
    public virtual DbSet<MonetaryTotal> MonetaryTotals { get; set; }

    /// <summary>
    /// Gets or sets ReservationTicket.
    /// </summary>
    public virtual DbSet<ReservationTicket> ReservationTicket { get; set; }

    #region IOrdersContext members

    /// <summary>
    /// Applies the entity values to the entity stored in EF ObjectStateManager.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void IOrdersContext.ApplyValuesFromEntity([NotNull] object entity)
    {
      Debug.ArgumentNotNull(entity, "entity");

      this.ApplyValuesFromEntity(entity);
    }

    /// <summary>
    /// Applies the entity values to the entity stored in EF ObjectStateManager.
    /// </summary>
    /// <param name="entity">The entity.</param>
    public virtual void ApplyValuesFromEntity([NotNull] object entity)
    {
      Debug.ArgumentNotNull(entity, "entity");

      var objectStateEntry = this.GetObjectStateEntry(entity);

      if (objectStateEntry.State != EntityState.Unchanged)
      {
        return;
      }

      objectStateEntry.SetModified();
      objectStateEntry.ApplyCurrentValues(entity);
    }

    /// <summary>
    /// Deletes the entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void IOrdersContext.CascadeDelete([NotNull] object entity)
    {
      Debug.ArgumentNotNull(entity, "entity");
      this.CascadeDelete(entity);
    }

    /// <summary>
    /// Deletes the entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    public virtual void CascadeDelete([NotNull] object entity)
    {
      Debug.ArgumentNotNull(entity, "entity");

      var objectContext = ((IObjectContextAdapter)this).ObjectContext;

      foreach (var relatedEntity in this.GetRelatedEntities(entity))
      {
        objectContext.DeleteObject(relatedEntity);
      }

      objectContext.DeleteObject(entity);
    }

    /// <summary>
    /// Determines whether the specified order is attached.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>
    ///   <c>true</c> if the specified order is attached; otherwise, <c>false</c>.
    /// </returns>
    bool IOrdersContext.IsAttached([NotNull] object entity)
    {
      return this.IsAttached(entity);
    }

    /// <summary>
    /// Determines whether the specified order is attached.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>
    ///   <c>true</c> if the specified order is attached; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool IsAttached([NotNull] object entity)
    {
      Debug.ArgumentNotNull(entity, "entity");

      return this.ChangeTracker.Entries().Any(e => (e.Entity == entity) && (e.State != EntityState.Detached));
    }

    /// <summary>
    /// Changes the state of the object.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="entityState">State of the entity.</param>
    void IOrdersContext.Entry([NotNull] object entity, EntityState entityState)
    {
      this.Entry(entity, entityState);
    }

    /// <summary>
    /// Changes the state of the object.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="entityState">State of the entity.</param>
    public virtual void Entry([NotNull] object entity, EntityState entityState)
    {
      Debug.ArgumentNotNull(entity, "entity");

      this.Entry(entity).State = entityState;
    }

    #endregion
    /// <summary>
    /// This method is called when the model for a derived context has been initialized, but
    /// before the model has been locked down and used to initialize the context.  The default
    /// implementation of this method does nothing, but it can be overridden in a derived class
    /// such that the model can be further configured before it is locked down.
    /// </summary>
    /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
    protected override void OnModelCreating([NotNull] DbModelBuilder modelBuilder)
    {
      Debug.ArgumentNotNull(modelBuilder, "modelBuilder");

      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

      modelBuilder.Configurations.Add(new AddressConfiguration());
      modelBuilder.Configurations.Add(new AllowanceChargeConfiguration());
      modelBuilder.Configurations.Add(new AmountConfiguration());
      modelBuilder.Configurations.Add(new ContactConfiguration());
      modelBuilder.Configurations.Add(new CommunicationConfiguration());
      modelBuilder.Configurations.Add(new CustomerPartyConfiguration());
      modelBuilder.Configurations.Add(new DeliveryConfiguration());
      modelBuilder.Configurations.Add(new DespatchConfiguration());
      modelBuilder.Configurations.Add(new ItemConfiguration());
      modelBuilder.Configurations.Add(new LineItemConfiguration());
      modelBuilder.Configurations.Add(new LocationConfiguration());
      modelBuilder.Configurations.Add(new MeasureConfiguration());
      modelBuilder.Configurations.Add(new MonetaryTotalConfiguration());
      modelBuilder.Configurations.Add(new OrderConfiguration());
      modelBuilder.Configurations.Add(new OrderLineConfiguration());
      modelBuilder.Configurations.Add(new OrderedShipmentConfiguration());
      modelBuilder.Configurations.Add(new PartyConfiguration());
      modelBuilder.Configurations.Add(new PartyLegalEntityConfiguration());
      modelBuilder.Configurations.Add(new PartyTaxSchemeConfiguration());
      modelBuilder.Configurations.Add(new PaymentMeansConfiguration());
      modelBuilder.Configurations.Add(new PersonConfiguration());
      modelBuilder.Configurations.Add(new PriceConfiguration());
      modelBuilder.Configurations.Add(new ShipmentConfiguration());
      modelBuilder.Configurations.Add(new StateConfiguration());
      modelBuilder.Configurations.Add(new SubstateConfiguration());
      modelBuilder.Configurations.Add(new SupplierPartyConfiguration());
      modelBuilder.Configurations.Add(new TaxCategoryConfiguration());
      modelBuilder.Configurations.Add(new TaxSchemeConfiguration());
      modelBuilder.Configurations.Add(new TaxSubTotalConfiguration());
      modelBuilder.Configurations.Add(new TaxTotalConfiguration());
      modelBuilder.Configurations.Add(new ReservationTicketConfiguration());

      modelBuilder.Ignore<ProcessingOrder>();
      modelBuilder.Ignore<MonetaryTotalProcessing>();
      modelBuilder.Ignore<LineItemProcessing>();
      modelBuilder.Ignore<TaxTotalProcessing>();
      modelBuilder.Ignore<TaxSubTotalProcessing>();

      // base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Disposes the context. The underlying <see cref="T:System.Data.Objects.ObjectContext" /> is also disposed if it was created
    /// is by this context or ownership was passed to this context when this context was created.
    /// The connection to the database (<see cref="T:System.Data.Common.DbConnection" /> object) is also disposed if it was created
    /// is by this context or ownership was passed to this context when this context was created.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (this.isDisposed)
      {
        return;
      }

      if (disposing)
      {
        // Free any other managed objects here. 

      }

      // Free any unmanaged objects here. 
      this.isDisposed = true;
      
      // Call base class implementation. 
      base.Dispose(disposing);
    }


    /// <summary>
    /// Gets the related entities.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>
    /// The related entities.
    /// </returns>
    [NotNull]
    private IEnumerable GetRelatedEntities([NotNull] object entity)
    {
      Debug.ArgumentNotNull(entity, "entity");

      var result = new Stack();

      foreach (var relatedEnd in ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetRelationshipManager(entity).GetAllRelatedEnds())
      {
        foreach (var relatedEntity in relatedEnd)
        {
          result.Push(relatedEntity);
        }
      }

      return result;
    }

    /// <summary>
    /// Gets the object state entry.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>
    /// The object state entry.
    /// </returns>
    [NotNull]
    private ObjectStateEntry GetObjectStateEntry([NotNull] object entity)
    {
      Debug.ArgumentNotNull(entity, "entity");

      var objectContext = ((IObjectContextAdapter)this).ObjectContext;
      var entityContainer = objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName, DataSpace.CSpace);
      var entityTypeName = ObjectContext.GetObjectType(entity.GetType()).Name;
      var entitySetName = entityContainer.BaseEntitySets.First(entitySet => entitySet.ElementType.Name == entityTypeName).Name;
      var entityKey = objectContext.CreateEntityKey(entitySetName, entity);

      ObjectStateEntry result;

      var retrieved = objectContext.ObjectStateManager.TryGetObjectStateEntry(entityKey, out result);
      Assert.IsTrue(retrieved, "Unable to save the order. The order state entry not found.");

      return result;
    }
  }
}