// -------------------------------------------------------------------------------------------
// <copyright file="RegisterEcommerceProviders.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce.Pipelines.Loader
{
  using System.Collections.Specialized;
  using DomainModel.Addresses;
  using DomainModel.Currencies;
  using DomainModel.Orders;
  using DomainModel.Payments;
  using DomainModel.Shippings;

  using Microsoft.Practices.Unity;

  using Sitecore.Pipelines;

  using Unity;

  /// <summary>
  /// The Initialize container providers.
  /// </summary>
  public class RegisterEcommerceProviders : InitializeContainerProviderBase
  {
    #region Congifuration Names

    /// <summary>
    /// The description title.
    /// </summary>
    private readonly string description = "description";

    /// <summary>
    /// The settings name title.
    /// </summary>
    private readonly string settingsName = "setting name";

    /// <summary>
    /// The defualt container name.
    /// </summary>
    private readonly string defaultContainerName = "default container name";

    /// <summary>
    /// The containers item template Id.
    /// </summary>
    private readonly string containersItemTemplateId = "containers item template Id";

    #endregion

    /// <summary>
    /// Initializes the shipping method provider.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void InitializePaymentSystemProvider(PipelineArgs args)
    {
      NameValueCollection config = new NameValueCollection
                                     {
                                       { this.description, "Payment System Provider" },
                                       { this.settingsName, "Payment Systems Link" },
                                       { this.defaultContainerName, "Default Payment System" },
                                       { this.containersItemTemplateId, "{19B38990-A440-4C72-987E-82EEEA548636}" }
                                     };
      this.RegisterProvider<PaymentSystem>(config[this.description], config);
    }

    /// <summary>
    /// Initializes the shipping system provider.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void InitializeShippingSystemProvider(PipelineArgs args)
    {
      NameValueCollection config = new NameValueCollection
                                     {
                                       { this.description, "Shipping System Provider" },
                                       { this.settingsName, "Shipping Providers Link" },
                                       { this.defaultContainerName, "Default Shipping Provider" },
                                       { this.containersItemTemplateId, "{EC16C9C2-9368-4CC0-A6DE-F1AC9198D4A3}" }
                                     };
      this.RegisterProvider<ShippingProvider>(config[this.description], config);
    }

    /// <summary>
    /// Initializes the notification option provider.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void InitializeNotificationOptionProvider(PipelineArgs args)
    {
      NameValueCollection config = new NameValueCollection
                                     {
                                        { this.description, "Notification Option Provider" },
                                        { this.settingsName, "Notification Options Link" },
                                        { this.defaultContainerName, "Default Notification Option" },
                                        { this.containersItemTemplateId, "{03AAFDD3-88AD-4D03-BC19-BFFCDB833147}" }
                                     };
      this.RegisterProvider<NotificationOption>(config[this.description], config);
    }

    /// <summary>
    /// Initializes the country provider.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void InitializeCountryProvider(PipelineArgs args)
    {
      NameValueCollection config = new NameValueCollection
                                     {
                                        { this.description, "Country Provider" },
                                        { this.settingsName, "Countries Link" },
                                        { this.defaultContainerName, "Default Country" },
                                        { this.containersItemTemplateId, "{3B064378-2EE3-4720-B3B5-4DF4EAED91C8}" }
                                     };
      this.RegisterProvider<Country>(config[this.description], config);
    }

    /// <summary>
    /// Initializes the country provider.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void InitializeCurrencyProvider(PipelineArgs args)
    {
      NameValueCollection config = new NameValueCollection
                                     {
                                        { this.description, "Currency Provider" },
                                        { this.settingsName, "Currencies Link" },
                                        { this.defaultContainerName, "Default Currency" },
                                        { this.containersItemTemplateId, "{3B4681DD-E900-4A56-B162-CE59358C973D}" }
                                     };
      this.RegisterProvider<Currency>(config[this.description], config);
    }

    /// <summary>
    /// Initializes the country provider.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void InitializeVatRegionProvider(PipelineArgs args)
    {
      NameValueCollection config = new NameValueCollection
                                     {
                                        { this.description, "VAT Region Provider" },
                                        { this.settingsName, "VAT Regions" },
                                        { this.defaultContainerName, "Default VAT region" },
                                        { this.containersItemTemplateId, "{7697FB25-0A0C-4200-8BA7-6374CF3AABFC}" }
                                     };
      this.RegisterProvider<VatRegion>(config[this.description], config);
    }

    /// <summary>
    /// Initializes the order status provider.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public virtual void InitializeOrderStatusProvider(PipelineArgs args)
    {
      NameValueCollection config = new NameValueCollection
                                     {
                                        { this.description, "Order Status Provider" },
                                        { this.settingsName, "Order Statuses Link" },
                                        { this.defaultContainerName, "Default Order Status" },
                                        { this.containersItemTemplateId, "{3F593780-BA47-4AA9-B413-597291DDE655}" }
                                     };
      this.RegisterProvider<OrderStatus>(config[this.description], config);
    }

    public virtual void InitializeBusinessCatalogProviders(PipelineArgs args)
    {
      Context.Entity.Configure<QueryableContainerExtension>();
    }
  }
}