namespace Sitecore.Ecommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        OrderId = c.String(nullable: false, maxLength: 50),
                        IssueDate = c.DateTime(nullable: false),
                        Note = c.String(),
                        PricingCurrencyCode = c.String(maxLength: 3),
                        TaxCurrencyCode = c.String(maxLength: 3),
                        DestinationCountryCode = c.String(maxLength: 3),
                        ShopContext = c.String(nullable: false, maxLength: 50),
                        BuyerCustomerParty_Alias = c.Long(),
                        SellerSupplierParty_Alias = c.Long(),
                        AccountingCustomerParty_Alias = c.Long(),
                        PaymentMeans_Alias = c.Long(),
                        TaxTotal_Alias = c.Long(),
                        AnticipatedMonetaryTotal_Alias = c.Long(),
                        State_Alias = c.Long(),
                        ReservationTicket_Alias = c.Long(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CustomerParty", t => t.BuyerCustomerParty_Alias)
                .ForeignKey("dbo.SupplierParty", t => t.SellerSupplierParty_Alias)
                .ForeignKey("dbo.CustomerParty", t => t.AccountingCustomerParty_Alias)
                .ForeignKey("dbo.PaymentMeans", t => t.PaymentMeans_Alias)
                .ForeignKey("dbo.TaxTotal", t => t.TaxTotal_Alias)
                .ForeignKey("dbo.MonetaryTotal", t => t.AnticipatedMonetaryTotal_Alias)
                .ForeignKey("dbo.State", t => t.State_Alias)
                .ForeignKey("dbo.ReservationTicket", t => t.ReservationTicket_Alias)
                .Index(t => t.BuyerCustomerParty_Alias)
                .Index(t => t.SellerSupplierParty_Alias)
                .Index(t => t.AccountingCustomerParty_Alias)
                .Index(t => t.PaymentMeans_Alias)
                .Index(t => t.TaxTotal_Alias)
                .Index(t => t.AnticipatedMonetaryTotal_Alias)
                .Index(t => t.State_Alias)
                .Index(t => t.ReservationTicket_Alias);
            
            CreateTable(
                "dbo.CustomerParty",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        CustomerAssignedAccountID = c.String(maxLength: 50),
                        SupplierAssignedAccountID = c.String(maxLength: 50),
                        Party_Alias = c.Long(),
                        DeliveryContact_Alias = c.Long(),
                        AccountingContact_Alias = c.Long(),
                        BuyerContact_Alias = c.Long(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.Party", t => t.Party_Alias)
                .ForeignKey("dbo.Contact", t => t.DeliveryContact_Alias)
                .ForeignKey("dbo.Contact", t => t.AccountingContact_Alias)
                .ForeignKey("dbo.Contact", t => t.BuyerContact_Alias)
                .Index(t => t.Party_Alias)
                .Index(t => t.DeliveryContact_Alias)
                .Index(t => t.AccountingContact_Alias)
                .Index(t => t.BuyerContact_Alias);
            
            CreateTable(
                "dbo.Party",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        WebsiteURI = c.String(maxLength: 260),
                        LogoReferenceID = c.String(maxLength: 260),
                        EndpointID = c.String(maxLength: 50),
                        PartyIdentification = c.String(maxLength: 50),
                        PartyName = c.String(maxLength: 500),
                        LanguageCode = c.String(maxLength: 7),
                        Person_FirstName = c.String(maxLength: 500),
                        Person_FamilyName = c.String(maxLength: 500),
                        Person_Title = c.String(),
                        Person_MiddleName = c.String(maxLength: 500),
                        Person_NameSuffix = c.String(maxLength: 50),
                        Person_JobTitle = c.String(),
                        Person_OrganizationDepartment = c.String(maxLength: 500),
                        PostalAddress_Alias = c.Long(),
                        PhysicalLocation_Alias = c.Long(),
                        PartyLegalEntity_Alias = c.Long(),
                        Contact_Alias = c.Long(),
                        Order_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.Address", t => t.PostalAddress_Alias)
                .ForeignKey("dbo.Location", t => t.PhysicalLocation_Alias)
                .ForeignKey("dbo.PartyLegalEntity", t => t.PartyLegalEntity_Alias)
                .ForeignKey("dbo.Contact", t => t.Contact_Alias)
                .ForeignKey("dbo.Order", t => t.Order_ID)
                .Index(t => t.PostalAddress_Alias)
                .Index(t => t.PhysicalLocation_Alias)
                .Index(t => t.PartyLegalEntity_Alias)
                .Index(t => t.Contact_Alias)
                .Index(t => t.Order_ID);
            
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        ID = c.String(maxLength: 50),
                        AddressTypeCode = c.String(maxLength: 50),
                        AddressFormatCode = c.String(maxLength: 50),
                        PostBox = c.String(),
                        Floor = c.String(maxLength: 50),
                        Room = c.String(maxLength: 50),
                        StreetName = c.String(),
                        AdditionalStreetName = c.String(),
                        BuildingName = c.String(),
                        BuildingNumber = c.String(maxLength: 50),
                        InhouseMail = c.String(),
                        Department = c.String(),
                        MarkAttention = c.String(),
                        MarkCare = c.String(),
                        PlotIdentification = c.String(maxLength: 50),
                        CitySubdivisionName = c.String(),
                        CityName = c.String(),
                        PostalZone = c.String(maxLength: 50),
                        CountrySubentity = c.String(),
                        CountrySubentityCode = c.String(maxLength: 3),
                        Region = c.String(),
                        District = c.String(),
                        AddressLine = c.String(),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.Alias);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        ID = c.String(maxLength: 50),
                        Description = c.String(),
                        CountrySubentity = c.String(),
                        CountrySubEntityCode = c.String(maxLength: 50),
                        ValidityPeriod_StartDate = c.DateTime(nullable: false),
                        ValidityPeriod_StartTime = c.Time(nullable: false),
                        ValidityPeriod_EndDate = c.DateTime(nullable: false),
                        ValidityPeriod_EndTime = c.Time(nullable: false),
                        ValidityPeriod_Description = c.String(),
                        Address_Alias = c.Long(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.Address", t => t.Address_Alias)
                .Index(t => t.Address_Alias);
            
            CreateTable(
                "dbo.PartyTaxScheme",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        RegistrationName = c.String(maxLength: 500),
                        CompanyID = c.String(maxLength: 50),
                        ExemptionReasonCode = c.String(maxLength: 50),
                        ExemptionReason = c.String(),
                        TaxScheme_ID = c.String(maxLength: 50),
                        TaxScheme_Name = c.String(maxLength: 500),
                        TaxScheme_TaxTypeCode = c.String(maxLength: 50),
                        TaxScheme_CurrencyCode = c.String(maxLength: 3),
                        RegistrationAddress_Alias = c.Long(),
                        Party_Alias = c.Long(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.Address", t => t.RegistrationAddress_Alias)
                .ForeignKey("dbo.Party", t => t.Party_Alias)
                .Index(t => t.RegistrationAddress_Alias)
                .Index(t => t.Party_Alias);
            
            CreateTable(
                "dbo.PartyLegalEntity",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        RegistrationName = c.String(maxLength: 500),
                        CompanyID = c.String(maxLength: 50),
                        RegistrationAddress_Alias = c.Long(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.Address", t => t.RegistrationAddress_Alias)
                .Index(t => t.RegistrationAddress_Alias);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        ID = c.String(maxLength: 50),
                        Name = c.String(),
                        Telephone = c.String(maxLength: 50),
                        Telefax = c.String(maxLength: 50),
                        ElectronicMail = c.String(maxLength: 260),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Alias);
            
            CreateTable(
                "dbo.Communication",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        ChannelCode = c.String(maxLength: 50),
                        Channel = c.String(),
                        Value = c.String(),
                        Contact_Alias = c.Long(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.Contact", t => t.Contact_Alias)
                .Index(t => t.Contact_Alias);
            
            CreateTable(
                "dbo.SupplierParty",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        CustomerAssignedAccountID = c.String(maxLength: 50),
                        AddtitionalAccountID = c.String(maxLength: 50),
                        Party_Alias = c.Long(),
                        ShippingContact_Alias = c.Long(),
                        AccountingContact_Alias = c.Long(),
                        OrderContact_Alias = c.Long(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.Party", t => t.Party_Alias)
                .ForeignKey("dbo.Contact", t => t.ShippingContact_Alias)
                .ForeignKey("dbo.Contact", t => t.AccountingContact_Alias)
                .ForeignKey("dbo.Contact", t => t.OrderContact_Alias)
                .Index(t => t.Party_Alias)
                .Index(t => t.ShippingContact_Alias)
                .Index(t => t.AccountingContact_Alias)
                .Index(t => t.OrderContact_Alias);
            
            CreateTable(
                "dbo.Delivery",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinimumQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaximumQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LatestDeliveryDate = c.DateTime(nullable: false),
                        LatestDeliveryTime = c.Time(nullable: false),
                        TrackingID = c.String(maxLength: 50),
                        RequestedDeliveryPeriod_StartDate = c.DateTime(nullable: false),
                        RequestedDeliveryPeriod_StartTime = c.Time(nullable: false),
                        RequestedDeliveryPeriod_EndDate = c.DateTime(nullable: false),
                        RequestedDeliveryPeriod_EndTime = c.Time(nullable: false),
                        RequestedDeliveryPeriod_Description = c.String(),
                        DeliveryLocation_Alias = c.Long(),
                        DeliveryAdress_Alias = c.Long(),
                        AlternativeDeliveryLocation_Alias = c.Long(),
                        DeliveryParty_Alias = c.Long(),
                        Despatch_Alias = c.Long(),
                        Order_ID = c.Guid(),
                        LineItem_Alias = c.Long(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.Location", t => t.DeliveryLocation_Alias)
                .ForeignKey("dbo.Address", t => t.DeliveryAdress_Alias)
                .ForeignKey("dbo.Location", t => t.AlternativeDeliveryLocation_Alias)
                .ForeignKey("dbo.Party", t => t.DeliveryParty_Alias)
                .ForeignKey("dbo.Despatch", t => t.Despatch_Alias)
                .ForeignKey("dbo.Order", t => t.Order_ID)
                .ForeignKey("dbo.LineItem", t => t.LineItem_Alias)
                .Index(t => t.DeliveryLocation_Alias)
                .Index(t => t.DeliveryAdress_Alias)
                .Index(t => t.AlternativeDeliveryLocation_Alias)
                .Index(t => t.DeliveryParty_Alias)
                .Index(t => t.Despatch_Alias)
                .Index(t => t.Order_ID)
                .Index(t => t.LineItem_Alias);
            
            CreateTable(
                "dbo.Despatch",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        ID = c.String(maxLength: 50),
                        RequestedDespatchDate = c.DateTime(nullable: false),
                        RequestedDespatchTime = c.Time(nullable: false),
                        EstimatedDespatchDate = c.DateTime(nullable: false),
                        EstimatedDespatchTime = c.Time(nullable: false),
                        ActualDespatchDate = c.DateTime(nullable: false),
                        ActualDespatchTime = c.Time(nullable: false),
                        DespatchAddress_Alias = c.Long(),
                        DespatchParty_Alias = c.Long(),
                        Contact_Alias = c.Long(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.Address", t => t.DespatchAddress_Alias)
                .ForeignKey("dbo.Party", t => t.DespatchParty_Alias)
                .ForeignKey("dbo.Contact", t => t.Contact_Alias)
                .Index(t => t.DespatchAddress_Alias)
                .Index(t => t.DespatchParty_Alias)
                .Index(t => t.Contact_Alias);
            
            CreateTable(
                "dbo.PaymentMeans",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        ID = c.String(maxLength: 50),
                        PaymentMeansCode = c.String(maxLength: 50),
                        PaymentDueDate = c.DateTime(nullable: false),
                        PaymentChannelCode = c.String(maxLength: 50),
                        PaymentID = c.String(),
                    })
                .PrimaryKey(t => t.Alias);
            
            CreateTable(
                "dbo.AllowanceCharge",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        ID = c.String(maxLength: 50),
                        ChargeIndicator = c.Boolean(nullable: false),
                        AllowanceChargeReasonCode = c.String(maxLength: 50),
                        AllowanceChargeReason = c.String(),
                        MultiplierFactorNumeric = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrepaidIndicator = c.Boolean(nullable: false),
                        SequenceNumeric = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount_CurrencyID = c.String(maxLength: 3),
                        BaseAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BaseAmount_CurrencyID = c.String(maxLength: 3),
                        TaxCategory_Alias = c.Long(),
                        TaxTotal_Alias = c.Long(),
                        Order_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.TaxCategory", t => t.TaxCategory_Alias)
                .ForeignKey("dbo.TaxTotal", t => t.TaxTotal_Alias)
                .ForeignKey("dbo.Order", t => t.Order_ID)
                .Index(t => t.TaxCategory_Alias)
                .Index(t => t.TaxTotal_Alias)
                .Index(t => t.Order_ID);
            
            CreateTable(
                "dbo.TaxCategory",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        ID = c.String(maxLength: 50),
                        Name = c.String(maxLength: 500),
                        Percent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BaseUnitMeasure_UnitCode = c.String(maxLength: 3),
                        BaseUnitMeasure_Content = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PerUnitAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PerUnitAmount_CurrencyID = c.String(maxLength: 3),
                        TaxExemptionReasonCode = c.String(maxLength: 50),
                        TaxExemptionReason = c.String(),
                        TaxScheme_ID = c.String(maxLength: 50),
                        TaxScheme_Name = c.String(maxLength: 500),
                        TaxScheme_TaxTypeCode = c.String(maxLength: 50),
                        TaxScheme_CurrencyCode = c.String(maxLength: 3),
                    })
                .PrimaryKey(t => t.Alias);
            
            CreateTable(
                "dbo.TaxTotal",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        TaxAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxAmount_CurrencyID = c.String(maxLength: 3),
                        RoundingAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RoundingAmount_CurrencyID = c.String(maxLength: 3),
                    })
                .PrimaryKey(t => t.Alias);
            
            CreateTable(
                "dbo.TaxSubTotal",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        TaxableAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxableAmount_CurrencyID = c.String(maxLength: 3),
                        CalculationSequenceNumeric = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionCurrencyTaxAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionCurrencyTaxAmount_CurrencyID = c.String(maxLength: 3),
                        TaxCategory_Alias = c.Long(),
                        TaxTotal_Alias = c.Long(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.TaxCategory", t => t.TaxCategory_Alias)
                .ForeignKey("dbo.TaxTotal", t => t.TaxTotal_Alias)
                .Index(t => t.TaxCategory_Alias)
                .Index(t => t.TaxTotal_Alias);
            
            CreateTable(
                "dbo.MonetaryTotal",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        LineExtensionAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LineExtensionAmount_CurrencyID = c.String(maxLength: 3),
                        TaxExclusiveAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxExclusiveAmount_CurrencyID = c.String(maxLength: 3),
                        TaxInclusiveAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxInclusiveAmount_CurrencyID = c.String(maxLength: 3),
                        AllowanceTotalAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AllowanceTotalAmount_CurrencyID = c.String(maxLength: 3),
                        ChargeTotalAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ChargeTotalAmount_CurrencyID = c.String(maxLength: 3),
                        PrepaidAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrepaidAmount_CurrencyID = c.String(maxLength: 3),
                        PayableRoundingAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PayableRoundingAmount_CurrencyID = c.String(maxLength: 3),
                        PayableAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PayableAmount_CurrencyID = c.String(maxLength: 3),
                    })
                .PrimaryKey(t => t.Alias);
            
            CreateTable(
                "dbo.OrderLine",
                c => new
                    {
                        Alias = c.Long(nullable: false),
                        Note = c.String(),
                        Order_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.LineItem", t => t.Alias, cascadeDelete: true)
                .ForeignKey("dbo.Order", t => t.Order_ID)
                .Index(t => t.Alias)
                .Index(t => t.Order_ID);
            
            CreateTable(
                "dbo.LineItem",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        ID = c.String(maxLength: 50),
                        OrderID = c.String(),
                        Note = c.String(),
                        LineStatusCode = c.String(maxLength: 50),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LineExtensionAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LineExtensionAmount_CurrencyID = c.String(maxLength: 3),
                        TotalTaxAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalTaxAmount_CurrencyID = c.String(maxLength: 3),
                        MinimumQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaximumQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PartialDeliveryIndicator = c.Boolean(nullable: false),
                        Item_Description = c.String(),
                        Item_PackQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Item_PackSizeNumeric = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Item_Code = c.String(maxLength: 50),
                        Item_Name = c.String(maxLength: 500),
                        Item_AdditionalInformation = c.String(),
                        Item_Keyword = c.String(),
                        Item_BrandName = c.String(maxLength: 500),
                        Item_ModelName = c.String(maxLength: 500),
                        OrderedShipment_Alias = c.Long(),
                        ParentLine_Alias = c.Long(),
                        Price_Alias = c.Long(),
                        OrderLine_Alias = c.Long(),
                        OrderLine_Alias1 = c.Long(),
                        OrderLine_Alias2 = c.Long(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.OrderedShipment", t => t.OrderedShipment_Alias)
                .ForeignKey("dbo.LineItem", t => t.ParentLine_Alias)
                .ForeignKey("dbo.Price", t => t.Price_Alias)
                .ForeignKey("dbo.OrderLine", t => t.OrderLine_Alias)
                .ForeignKey("dbo.OrderLine", t => t.OrderLine_Alias1)
                .ForeignKey("dbo.OrderLine", t => t.OrderLine_Alias2)
                .Index(t => t.OrderedShipment_Alias)
                .Index(t => t.ParentLine_Alias)
                .Index(t => t.Price_Alias)
                .Index(t => t.OrderLine_Alias)
                .Index(t => t.OrderLine_Alias1)
                .Index(t => t.OrderLine_Alias2);
            
            CreateTable(
                "dbo.OrderedShipment",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        Shipment_Alias = c.Long(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.Shipment", t => t.Shipment_Alias)
                .Index(t => t.Shipment_Alias);
            
            CreateTable(
                "dbo.Shipment",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        ShippingPriorityLevelCode = c.String(maxLength: 50),
                        HandlingCode = c.String(maxLength: 50),
                        HandlingInstructions = c.String(),
                        Information = c.String(),
                        GrossWeightMeasure_UnitCode = c.String(maxLength: 3),
                        GrossWeightMeasure_Content = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetWeightMeasure_UnitCode = c.String(maxLength: 3),
                        NetWeightMeasure_Content = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetNetWeightMeasure_UnitCode = c.String(maxLength: 3),
                        NetNetWeightMeasure_Content = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GrossVolumeMeasure_UnitCode = c.String(maxLength: 3),
                        GrossVolumeMeasure_Content = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetVolumeMeasure_UnitCode = c.String(maxLength: 3),
                        NetVolumeMeasure_Content = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalGoodsItemQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalTransportHandlingUnitQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InsuranceValueAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InsuranceValueAmount_CurrencyID = c.String(maxLength: 3),
                        DeclaredCustomsValueAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeclaredCustomsValueAmount_CurrencyID = c.String(maxLength: 3),
                        DeclaredForCarriageValueAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeclaredForCarriageValueAmount_CurrencyID = c.String(maxLength: 3),
                        DeclaredStatisticsValueAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeclaredStatisticsValueAmount_CurrencyID = c.String(maxLength: 3),
                        FreeOnBoardValueAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FreeOnBoardValueAmount_CurrencyID = c.String(maxLength: 3),
                        SpecialInstructions = c.String(),
                        DeliveryInstructions = c.String(),
                        SplitConsignmentIndicator = c.Boolean(nullable: false),
                        ExportCountry = c.String(),
                        Delivery_Alias = c.Long(),
                        OriginAddress_Alias = c.Long(),
                        FirstArrivalPortLocation_Alias = c.Long(),
                        LastExitPortLocation_Alias = c.Long(),
                        FreightAllowanceCharge_Alias = c.Long(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.Delivery", t => t.Delivery_Alias)
                .ForeignKey("dbo.Address", t => t.OriginAddress_Alias)
                .ForeignKey("dbo.Location", t => t.FirstArrivalPortLocation_Alias)
                .ForeignKey("dbo.Location", t => t.LastExitPortLocation_Alias)
                .ForeignKey("dbo.AllowanceCharge", t => t.FreightAllowanceCharge_Alias)
                .Index(t => t.Delivery_Alias)
                .Index(t => t.OriginAddress_Alias)
                .Index(t => t.FirstArrivalPortLocation_Alias)
                .Index(t => t.LastExitPortLocation_Alias)
                .Index(t => t.FreightAllowanceCharge_Alias);
            
            CreateTable(
                "dbo.Price",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        PriceAmount_Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceAmount_CurrencyID = c.String(maxLength: 3),
                        BaseQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceChangeReason = c.String(),
                        PriceTypeCode = c.String(maxLength: 50),
                        PriceType = c.String(),
                        OrderableUnitFactorRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Alias);
            
            CreateTable(
                "dbo.Substate",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        Code = c.String(maxLength: 50),
                        Name = c.String(maxLength: 500),
                        Active = c.Boolean(nullable: false),
                        Abbreviation = c.String(),
                        LineItem_Alias = c.Long(),
                        State_Alias = c.Long(),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.LineItem", t => t.LineItem_Alias)
                .ForeignKey("dbo.State", t => t.State_Alias)
                .Index(t => t.LineItem_Alias)
                .Index(t => t.State_Alias);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        Code = c.String(maxLength: 50),
                        Name = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Alias);
            
            CreateTable(
                "dbo.ReservationTicket",
                c => new
                    {
                        Alias = c.Long(nullable: false, identity: true),
                        AuthorizationCode = c.String(),
                        TransactionNumber = c.String(),
                        InvoiceNumber = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CapturedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Alias);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Substate", new[] { "State_Alias" });
            DropIndex("dbo.Substate", new[] { "LineItem_Alias" });
            DropIndex("dbo.Shipment", new[] { "FreightAllowanceCharge_Alias" });
            DropIndex("dbo.Shipment", new[] { "LastExitPortLocation_Alias" });
            DropIndex("dbo.Shipment", new[] { "FirstArrivalPortLocation_Alias" });
            DropIndex("dbo.Shipment", new[] { "OriginAddress_Alias" });
            DropIndex("dbo.Shipment", new[] { "Delivery_Alias" });
            DropIndex("dbo.OrderedShipment", new[] { "Shipment_Alias" });
            DropIndex("dbo.LineItem", new[] { "OrderLine_Alias2" });
            DropIndex("dbo.LineItem", new[] { "OrderLine_Alias1" });
            DropIndex("dbo.LineItem", new[] { "OrderLine_Alias" });
            DropIndex("dbo.LineItem", new[] { "Price_Alias" });
            DropIndex("dbo.LineItem", new[] { "ParentLine_Alias" });
            DropIndex("dbo.LineItem", new[] { "OrderedShipment_Alias" });
            DropIndex("dbo.OrderLine", new[] { "Order_ID" });
            DropIndex("dbo.OrderLine", new[] { "Alias" });
            DropIndex("dbo.TaxSubTotal", new[] { "TaxTotal_Alias" });
            DropIndex("dbo.TaxSubTotal", new[] { "TaxCategory_Alias" });
            DropIndex("dbo.AllowanceCharge", new[] { "Order_ID" });
            DropIndex("dbo.AllowanceCharge", new[] { "TaxTotal_Alias" });
            DropIndex("dbo.AllowanceCharge", new[] { "TaxCategory_Alias" });
            DropIndex("dbo.Despatch", new[] { "Contact_Alias" });
            DropIndex("dbo.Despatch", new[] { "DespatchParty_Alias" });
            DropIndex("dbo.Despatch", new[] { "DespatchAddress_Alias" });
            DropIndex("dbo.Delivery", new[] { "LineItem_Alias" });
            DropIndex("dbo.Delivery", new[] { "Order_ID" });
            DropIndex("dbo.Delivery", new[] { "Despatch_Alias" });
            DropIndex("dbo.Delivery", new[] { "DeliveryParty_Alias" });
            DropIndex("dbo.Delivery", new[] { "AlternativeDeliveryLocation_Alias" });
            DropIndex("dbo.Delivery", new[] { "DeliveryAdress_Alias" });
            DropIndex("dbo.Delivery", new[] { "DeliveryLocation_Alias" });
            DropIndex("dbo.SupplierParty", new[] { "OrderContact_Alias" });
            DropIndex("dbo.SupplierParty", new[] { "AccountingContact_Alias" });
            DropIndex("dbo.SupplierParty", new[] { "ShippingContact_Alias" });
            DropIndex("dbo.SupplierParty", new[] { "Party_Alias" });
            DropIndex("dbo.Communication", new[] { "Contact_Alias" });
            DropIndex("dbo.PartyLegalEntity", new[] { "RegistrationAddress_Alias" });
            DropIndex("dbo.PartyTaxScheme", new[] { "Party_Alias" });
            DropIndex("dbo.PartyTaxScheme", new[] { "RegistrationAddress_Alias" });
            DropIndex("dbo.Location", new[] { "Address_Alias" });
            DropIndex("dbo.Party", new[] { "Order_ID" });
            DropIndex("dbo.Party", new[] { "Contact_Alias" });
            DropIndex("dbo.Party", new[] { "PartyLegalEntity_Alias" });
            DropIndex("dbo.Party", new[] { "PhysicalLocation_Alias" });
            DropIndex("dbo.Party", new[] { "PostalAddress_Alias" });
            DropIndex("dbo.CustomerParty", new[] { "BuyerContact_Alias" });
            DropIndex("dbo.CustomerParty", new[] { "AccountingContact_Alias" });
            DropIndex("dbo.CustomerParty", new[] { "DeliveryContact_Alias" });
            DropIndex("dbo.CustomerParty", new[] { "Party_Alias" });
            DropIndex("dbo.Order", new[] { "ReservationTicket_Alias" });
            DropIndex("dbo.Order", new[] { "State_Alias" });
            DropIndex("dbo.Order", new[] { "AnticipatedMonetaryTotal_Alias" });
            DropIndex("dbo.Order", new[] { "TaxTotal_Alias" });
            DropIndex("dbo.Order", new[] { "PaymentMeans_Alias" });
            DropIndex("dbo.Order", new[] { "AccountingCustomerParty_Alias" });
            DropIndex("dbo.Order", new[] { "SellerSupplierParty_Alias" });
            DropIndex("dbo.Order", new[] { "BuyerCustomerParty_Alias" });
            DropForeignKey("dbo.Substate", "State_Alias", "dbo.State");
            DropForeignKey("dbo.Substate", "LineItem_Alias", "dbo.LineItem");
            DropForeignKey("dbo.Shipment", "FreightAllowanceCharge_Alias", "dbo.AllowanceCharge");
            DropForeignKey("dbo.Shipment", "LastExitPortLocation_Alias", "dbo.Location");
            DropForeignKey("dbo.Shipment", "FirstArrivalPortLocation_Alias", "dbo.Location");
            DropForeignKey("dbo.Shipment", "OriginAddress_Alias", "dbo.Address");
            DropForeignKey("dbo.Shipment", "Delivery_Alias", "dbo.Delivery");
            DropForeignKey("dbo.OrderedShipment", "Shipment_Alias", "dbo.Shipment");
            DropForeignKey("dbo.LineItem", "OrderLine_Alias2", "dbo.OrderLine");
            DropForeignKey("dbo.LineItem", "OrderLine_Alias1", "dbo.OrderLine");
            DropForeignKey("dbo.LineItem", "OrderLine_Alias", "dbo.OrderLine");
            DropForeignKey("dbo.LineItem", "Price_Alias", "dbo.Price");
            DropForeignKey("dbo.LineItem", "ParentLine_Alias", "dbo.LineItem");
            DropForeignKey("dbo.LineItem", "OrderedShipment_Alias", "dbo.OrderedShipment");
            DropForeignKey("dbo.OrderLine", "Order_ID", "dbo.Order");
            DropForeignKey("dbo.OrderLine", "Alias", "dbo.LineItem");
            DropForeignKey("dbo.TaxSubTotal", "TaxTotal_Alias", "dbo.TaxTotal");
            DropForeignKey("dbo.TaxSubTotal", "TaxCategory_Alias", "dbo.TaxCategory");
            DropForeignKey("dbo.AllowanceCharge", "Order_ID", "dbo.Order");
            DropForeignKey("dbo.AllowanceCharge", "TaxTotal_Alias", "dbo.TaxTotal");
            DropForeignKey("dbo.AllowanceCharge", "TaxCategory_Alias", "dbo.TaxCategory");
            DropForeignKey("dbo.Despatch", "Contact_Alias", "dbo.Contact");
            DropForeignKey("dbo.Despatch", "DespatchParty_Alias", "dbo.Party");
            DropForeignKey("dbo.Despatch", "DespatchAddress_Alias", "dbo.Address");
            DropForeignKey("dbo.Delivery", "LineItem_Alias", "dbo.LineItem");
            DropForeignKey("dbo.Delivery", "Order_ID", "dbo.Order");
            DropForeignKey("dbo.Delivery", "Despatch_Alias", "dbo.Despatch");
            DropForeignKey("dbo.Delivery", "DeliveryParty_Alias", "dbo.Party");
            DropForeignKey("dbo.Delivery", "AlternativeDeliveryLocation_Alias", "dbo.Location");
            DropForeignKey("dbo.Delivery", "DeliveryAdress_Alias", "dbo.Address");
            DropForeignKey("dbo.Delivery", "DeliveryLocation_Alias", "dbo.Location");
            DropForeignKey("dbo.SupplierParty", "OrderContact_Alias", "dbo.Contact");
            DropForeignKey("dbo.SupplierParty", "AccountingContact_Alias", "dbo.Contact");
            DropForeignKey("dbo.SupplierParty", "ShippingContact_Alias", "dbo.Contact");
            DropForeignKey("dbo.SupplierParty", "Party_Alias", "dbo.Party");
            DropForeignKey("dbo.Communication", "Contact_Alias", "dbo.Contact");
            DropForeignKey("dbo.PartyLegalEntity", "RegistrationAddress_Alias", "dbo.Address");
            DropForeignKey("dbo.PartyTaxScheme", "Party_Alias", "dbo.Party");
            DropForeignKey("dbo.PartyTaxScheme", "RegistrationAddress_Alias", "dbo.Address");
            DropForeignKey("dbo.Location", "Address_Alias", "dbo.Address");
            DropForeignKey("dbo.Party", "Order_ID", "dbo.Order");
            DropForeignKey("dbo.Party", "Contact_Alias", "dbo.Contact");
            DropForeignKey("dbo.Party", "PartyLegalEntity_Alias", "dbo.PartyLegalEntity");
            DropForeignKey("dbo.Party", "PhysicalLocation_Alias", "dbo.Location");
            DropForeignKey("dbo.Party", "PostalAddress_Alias", "dbo.Address");
            DropForeignKey("dbo.CustomerParty", "BuyerContact_Alias", "dbo.Contact");
            DropForeignKey("dbo.CustomerParty", "AccountingContact_Alias", "dbo.Contact");
            DropForeignKey("dbo.CustomerParty", "DeliveryContact_Alias", "dbo.Contact");
            DropForeignKey("dbo.CustomerParty", "Party_Alias", "dbo.Party");
            DropForeignKey("dbo.Order", "ReservationTicket_Alias", "dbo.ReservationTicket");
            DropForeignKey("dbo.Order", "State_Alias", "dbo.State");
            DropForeignKey("dbo.Order", "AnticipatedMonetaryTotal_Alias", "dbo.MonetaryTotal");
            DropForeignKey("dbo.Order", "TaxTotal_Alias", "dbo.TaxTotal");
            DropForeignKey("dbo.Order", "PaymentMeans_Alias", "dbo.PaymentMeans");
            DropForeignKey("dbo.Order", "AccountingCustomerParty_Alias", "dbo.CustomerParty");
            DropForeignKey("dbo.Order", "SellerSupplierParty_Alias", "dbo.SupplierParty");
            DropForeignKey("dbo.Order", "BuyerCustomerParty_Alias", "dbo.CustomerParty");
            DropTable("dbo.ReservationTicket");
            DropTable("dbo.State");
            DropTable("dbo.Substate");
            DropTable("dbo.Price");
            DropTable("dbo.Shipment");
            DropTable("dbo.OrderedShipment");
            DropTable("dbo.LineItem");
            DropTable("dbo.OrderLine");
            DropTable("dbo.MonetaryTotal");
            DropTable("dbo.TaxSubTotal");
            DropTable("dbo.TaxTotal");
            DropTable("dbo.TaxCategory");
            DropTable("dbo.AllowanceCharge");
            DropTable("dbo.PaymentMeans");
            DropTable("dbo.Despatch");
            DropTable("dbo.Delivery");
            DropTable("dbo.SupplierParty");
            DropTable("dbo.Communication");
            DropTable("dbo.Contact");
            DropTable("dbo.PartyLegalEntity");
            DropTable("dbo.PartyTaxScheme");
            DropTable("dbo.Location");
            DropTable("dbo.Address");
            DropTable("dbo.Party");
            DropTable("dbo.CustomerParty");
            DropTable("dbo.Order");
        }
    }
}
