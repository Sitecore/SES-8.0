namespace Sitecore.Ecommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecimalScale : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AllowanceCharge", "Amount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.AllowanceCharge", "BaseAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.TaxCategory", "PerUnitAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.TaxTotal", "TaxAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.TaxTotal", "RoundingAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.TaxSubTotal", "TaxableAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.TaxSubTotal", "TransactionCurrencyTaxAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonetaryTotal", "LineExtensionAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonetaryTotal", "TaxExclusiveAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonetaryTotal", "TaxInclusiveAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonetaryTotal", "AllowanceTotalAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonetaryTotal", "ChargeTotalAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonetaryTotal", "PrepaidAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonetaryTotal", "PayableRoundingAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.MonetaryTotal", "PayableAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.LineItem", "LineExtensionAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.LineItem", "TotalTaxAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Shipment", "InsuranceValueAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Shipment", "DeclaredCustomsValueAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Shipment", "DeclaredForCarriageValueAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Shipment", "DeclaredStatisticsValueAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Shipment", "FreeOnBoardValueAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.Price", "PriceAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Price", "PriceAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Shipment", "FreeOnBoardValueAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Shipment", "DeclaredStatisticsValueAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Shipment", "DeclaredForCarriageValueAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Shipment", "DeclaredCustomsValueAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Shipment", "InsuranceValueAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LineItem", "TotalTaxAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LineItem", "LineExtensionAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonetaryTotal", "PayableAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonetaryTotal", "PayableRoundingAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonetaryTotal", "PrepaidAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonetaryTotal", "ChargeTotalAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonetaryTotal", "AllowanceTotalAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonetaryTotal", "TaxInclusiveAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonetaryTotal", "TaxExclusiveAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MonetaryTotal", "LineExtensionAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TaxSubTotal", "TransactionCurrencyTaxAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TaxSubTotal", "TaxableAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TaxTotal", "RoundingAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TaxTotal", "TaxAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TaxCategory", "PerUnitAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.AllowanceCharge", "BaseAmount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.AllowanceCharge", "Amount_Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
