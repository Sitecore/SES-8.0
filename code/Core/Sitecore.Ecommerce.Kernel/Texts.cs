// -------------------------------------------------------------------------------------------
// <copyright file="Texts.cs" company="Sitecore Corporation">
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

namespace Sitecore.Ecommerce
{
  /// <summary>
  /// Defines the texts class.
  /// </summary>
  public static class Texts
  {
    /// <summary>
    /// "The field cannot be empty" dictionary key.
    /// </summary>
    public const string TheFieldCannotBeEmpty = "The field {0} cannot be empty.";

    /// <summary>
    /// "The field is not valid email" dictionary entry.
    /// </summary>
    public const string TheFieldContainsAnInvalidEmailAddress = "The field {0} contains an invalid email address.";

    /// <summary>
    /// "The field contains invalid characters" dictionary key.
    /// </summary>
    public const string TheFieldContainsInvalidCharacters = "The field {0} contains invalid characters.";

    /// <summary>
    /// "Search" dictionary key.
    /// </summary>
    public const string Search = "Search";

    /// <summary>
    /// "Order cannot be saved." dictionary key.
    /// </summary>
    public const string TheOrderCannotBeSaved = "The order cannot be saved.";

    /// <summary>
    /// "This Order does not exist." dictionary key.
    /// </summary>
    public const string ThisOrderDoesNotExist = "The order does not exist.";

    /// <summary>
    /// "The order is in the <b>{0}</b> state." dictionary key.
    /// </summary>
    public const string TheOrderIsInTheXState = "The order is in the {0} state.";

    /// <summary>
    /// "The order has no state"  dictionary key.
    /// </summary>
    public const string TheOrderHasNoState = "The order has no state.";

    /// <summary>
    /// Order is cancelled.
    /// </summary>
    public const string TheOrderIsCancelled = "The order is cancelled.";

    /// <summary>
    /// Cancel order command.
    /// </summary>
    public const string CancelOrder = "Cancel order";

    /// <summary>
    /// Order cannot be cancelled.
    /// </summary>
    public const string TheOrderCannotBeCancelled = "The order cannot be cancelled.";

    /// <summary>
    /// Represents the message that is shown when the set of sub-states is not valid.
    /// </summary>
    public const string UnableToSaveTheOrderTheOrderStateOrSubstateIsInvalid = "Unable to save the order. The order state or sub-state is invalid.";

    /// <summary>
    /// Represents the message that is shown when order has been saved.
    /// </summary>
    public const string TheChangesToTheOrderHaveBeenSaved = "The changes to the order have been saved.";

    /// <summary>
    /// Represents the message that the selected order line will be deleted if you proceed. Do You want to proceed?
    /// </summary>
    public const string TheSelectedOrderLineWillBeDeletedIfYouProceed = "The selected order line will be deleted if you proceed.";

    /// <summary>
    /// Represents the warning message shown when the payable amount exceeds the reserved amount.
    /// </summary>
    public const string TheTotalPayableAmountExceedsTheAmountThatIsReservedOnThePaymentProviderSideTheOrderCannotBeCapturedInFull = "The order cannot be captured in full. The payable amount exceeds the authorized reserved amount from the payment provider.";

    /// <summary>
    /// The order was marked as captured.
    /// </summary>
    public const string OrderStateChangedToCaptured = "Order state changed to captured.";

    /// <summary>
    /// The order was marked as packed.
    /// </summary>
    public const string OrderStateChangedToPacked = "Order state changed to packed.";

    /// <summary>
    /// The order was marked as shipped.
    /// </summary>
    public const string OrderStateChangedToShipped = "Order state changed to shipped.";

    /// <summary>
    /// Represents the message that is shown when an order confirmation has been sent to the customer.
    /// </summary>
    public const string AnOrderConfirmationHasBeenSentToTheCustomer = "An order confirmation has been sent to the customer.";

    /// <summary>
    /// Represents the message that the order line has been deleted from the order.
    /// </summary>
    public const string TheOrderLineHasBeenRemovedFromTheOrder = "The order line has been removed from the order.";

    /// <summary>
    /// No order line was selected.
    /// </summary>
    public const string NoOrderLineWasSelected = "No order line was selected.";

    /// <summary>
    /// Represents the message that order line has been added to the order.
    /// </summary>
    public const string TheOrderLineHasBeenAddedToTheOrder = "The order line has been added to the order.";

    /// <summary>
    /// Represents the message that order line has been edited. 
    /// </summary>
    public const string TheOrderLineHasBeenChanged = "The order line has been changed.";

    /// <summary>
    /// Represents the message that order line has been edited. 
    /// </summary>
    public const string TheOrderLineQuantityHasBeenChanged = "The order line quantity has been changed.";

    /// <summary>
    /// Represents the message that allowance charge has been edited. 
    /// </summary>
    public const string TheAllowanceChargeHasBeenChanged = "The allowance charge has been changed.";

    /// <summary>
    /// Represents the message when an order cannot be saved because the allowance charge amount value is incorrect.
    /// </summary>
    public const string UnableToSaveTheChangesTheAllowanceChargeAmountIsIncorrect = "Unable to save the changes. The allowance charge amount is incorrect.";

    #region Order states. Required to localize order states from Orders database.

    /// <summary>
    /// The InProcess text key.
    /// </summary>
    public const string InProcess = "In process";

    #endregion  

    /// <summary>
    /// Defines the message of exception thrown when OrderSecurity property is not set for OrderRepository class.
    /// </summary>
    public const string UnableToSaveTheOrdersOrderSecurityCannotBeNull = "Unable to save the order. OrderSecurity cannot be null.";

    /// <summary>
    /// Defines the message of exception thrown when one of the orders saved cannot be processed.
    /// </summary>
    public const string YouDoNotHaveTheNecessaryPermissionsToEditTheOrder = "You do not have the necessary permissions to edit the order.";

    /// <summary>
    /// Defines the message of exception thrown when order reopening is denied.
    /// </summary>
    public const string YouDoNotHaveTheNecessaryPermissionsToReopenTheOrder = "You do not have the necessary permissions to reopen the order.";

    /// <summary>
    /// Defines the message of exception thrown when order cancelling is denied.
    /// </summary>
    public const string YouDoNotHaveTheNecessaryPermissionsToCancelTheOrder = "You do not have the necessary permissions to cancel the order.";

    /// <summary>
    /// Defines the message of exception thrown when the order that was not created by customer is requested.
    /// </summary>
    public const string YouDoNotHaveTheNecessaryPermissionsToEditThisOrderItWasCreatedByAnotherCustomer = "You do not have the necessary permissions to edit this order. It was created by another customer. ";

    /// <summary>
    /// Unable to perform operation. Price is not available.
    /// </summary>
    public const string UnableToPerformTheOperationPriceIsNotAvailable = "Unable to perform the operation. Price is not available.";

    /// <summary>
    /// Quantity field.
    /// </summary>
    public const string Quantity = "Quantity:";

    #region Validation messages.

    /// <summary>
    /// Represents the message that quantity is not entered.
    /// </summary>
    public const string EnterAQuantity = "Enter a quantity.";

    /// <summary>
    /// Represents the message that quantity is in appropriate format.
    /// </summary>
    public const string TheQuantityShouldBeANumberGreaterThan0 = "The quantity should be a number greater than 0.";

    /// <summary>
    /// Represents the message that product is not selected. 
    /// </summary>
    public const string SelectAProduct = "Select a product.";

    /// <summary>
    /// Represents the message that product is out of stock. 
    /// </summary>
    public const string TheProductIsOutOfStock = "The product is out of stock.";

    #endregion

    #region Order Conversion Tool Messages

    /// <summary>
    /// Represents the message for exception thrown when ShopContext is null.
    /// </summary>
    public const string UnableToSaveTheOrderShopContextCannotBeNull = "Unable to save the order. ShopContext cannot be null.";

    /// <summary>
    /// Represents the message for exception thrown when OrderManager is null.
    /// </summary>
    public const string UnableToSaveTheOrderOrderManagerCannotBeNull = "Unable to save the order. OrderManager cannot be null.";

    /// <summary>
    /// Represents the message for exception thrown when OrderRepository is null.
    /// </summary>
    public const string UnableToSaveTheOrderOrderRepositoryCannotBeNull = "Unable to save the order. OrderRepository cannot be null.";

    /// <summary>
    /// Represents the message for exception thrown then TransientOrderConverter is null.
    /// </summary>
    public const string UnableToSaveTheOrderTransientOrderConverterCannotBeNull = "Unable to save the order. TransientOrderConverter cannot be null.";

    /// <summary>
    /// Represents the message shown when conversion process is finished.
    /// </summary>
    public const string AllOrdersHaveBeenSuccessfullyConvertedToTheNewFormat = "All orders have been successfully converted to the new format.";

    /// <summary>
    /// Represents template for the message shown when order creation process is interrupted due to unhandled exception.
    /// </summary>
    public const string AnUnexpectedErrorOccurredDuringOrderConversion = "An unexpected error occurred during order conversion: {0}";

    #endregion

    #region Order Confirmation Messages

    /// <summary>
    /// Represents the message for the confirmation header before the capturing.
    /// </summary>
    public const string DoYouWantToProceed = "Do you want to proceed?";

    /// <summary>
    /// Represents the message for the confirmation text before capturing.
    /// </summary>
    public const string TheFullAmountWillBeCapturedFromTheCustomerAccountIfYouProceed = "The full amount will be captured from the customer account if you proceed.";

    #endregion

    #region ShopContextSwitcher

    /// <summary>
    /// Represents the message that appears when the one opens Order Management pages
    /// without access to any web shop.
    /// </summary>
    public const string ThereAreNoAvailableWebShops = "There are no available web shops.";

    #endregion
  }
}