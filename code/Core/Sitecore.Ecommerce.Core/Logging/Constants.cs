// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the LogEntry type.
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

namespace Sitecore.Ecommerce.Logging
{
  /// <summary>
  /// Defines the Logging constants class.
  /// </summary>
  public class Constants
  {
    #region Details

    /// <summary>
    /// Represents the message that is shown when the order reservation is canceled.
    /// </summary>
    public const string OrderReservationCanceled = "Reservation of amount for order is canceled";

    /// <summary>
    /// Represents the message that is shown when canceling of the order reservation is rejected by payment system.
    /// </summary>
    public const string OrderReservationCancelingFailed = "Canceling of the reserved amount is denied from payment system - reason - {0}";

    /// <summary>
    /// Represents the message that is shown when the order is captured.
    /// </summary>
    public const string OrderCaptured = "Approved from payment system";

    /// <summary>
    /// Represents the message that is shown when the order is captured.
    /// </summary>
    public const string OrderCapturedWithCard = "Approved from payment system. Card type - {0}";

    /// <summary>
    /// Constant informs the user that order is captured in full.
    /// </summary>
    public const string OrderIsCapturedInFull = "Order is captured in full";

    /// <summary>
    /// Represents the message that is shown when the order is not captured. 
    /// </summary>
    public const string OrderCapturingFailed = "Denied from payment system - reason - {0}";

    /// <summary>
    /// Represents the message that is shown when the new order line is added to the order.
    /// </summary>
    public const string OrderLineAdded = "New order line {0} is added";

    /// <summary>
    ///  Represents the message that is shown when the adding of the new order line is failed.
    /// </summary>
    public const string OrderLineAddingFailed = "New order line is not added to the order - reason - {0}";

    /// <summary>
    ///  Represents the message that is shown when the order line was edited.
    /// </summary>
    public const string OrderLineEdited = "Order line was changed. Old product - {0}. New product - {1}";

    /// <summary>
    ///  Represents the message that is shown when the order line was edited.
    /// </summary>
    public const string OrderLineEditingFailed = "Order line was not changed - reason - {0}";

    /// <summary>
    /// Represents the message that is shown when the order amount is invalid.
    /// </summary>
    public const string OrderAmountIsInvalid = "Order amount is invalid";

    /// <summary>
    /// Order confirmation sended constant.
    /// </summary>
    public const string OrderConfirmationSent = "Order confirmation was sent to the customer";

    /// <summary>
    /// Represents the message that is shown when order has been saved.
    /// </summary>
    public const string OrderConfirmationFailed = "Order confirmation cannot be sent to the customer";

    /// <summary>
    /// Property set message constant.
    /// </summary>
    public const string OrderLineDeleted = "Order Line {0} is deleted";

    /// <summary>
    /// Property set message constant.
    /// </summary>
    public const string OrderLineDeletingFailed = "Order Line cannot be deleted";

    /// <summary>
    /// Property set message constant.
    /// </summary>
    public const string OrderLinePropertySet = "Order Line field {0} was changed from {1} to {2}";

    /// <summary>
    /// Order printed message constant.
    /// </summary>
    public const string OrderPrinted = "Order is printed";

    /// <summary>
    /// Order Status set message constant.
    /// </summary>
    public const string OrderStatusSet = "State is changed from {0}, sub-states {1} to {2}, sub-states {3}";

    /// <summary>
    /// Order Status set failed message constant.
    /// </summary>
    public const string OrderStatusSetFailed = "Tried to change from {0}, sub-states {1} to {2}, sub-states {3}";

    /// <summary>
    /// Property set message constant.
    /// </summary>
    public const string PropertySet = "The field {0} has changed from {1} to {2}";

    /// <summary>
    /// Property changed message constant.
    /// </summary>
    public const string PropertyChanged = "Changed {0} from {1} to {2}.";

    /// <summary>
    /// Added order line message constant.
    /// </summary>
    public const string AddedOrderLine = "Added order line {0} with {1} items.";

    /// <summary>
    /// Change order line quantity constant.
    /// </summary>
    public const string ChangeOrderLineQuantity = "Quantity for the line item '{0}' was changed from {1} to {2}";

    /// <summary>
    /// Change order line quantity constant.
    /// </summary>
    public const string ChangeOrderLineQuantityFailed = "Failed to change quantity for the line item '{0}' from {1} to {2}";

    /// <summary>
    /// Order created message constant
    /// </summary>
    public const string OrderCreated = "Order was created from {0}.";

    /// <summary>
    /// Order state changed message constant.
    /// </summary>
    public const string OrderStateChanged = "Order state was changed from {0} to {1}.";

    /// <summary>
    /// Order status was changed message constant.
    /// </summary>
    public const string OrderStatusChanged = "Order status was changed from {0} to {1}.";

    /// <summary>
    /// Cannot change order state message constant.
    /// </summary>
    public const string CannotChangeState = "State not ready for changing from Order in {0} to {1}.";

    /// <summary>
    /// Approved from Payement system message constant.
    /// </summary>
    public const string PaymentSystemAprooved = "Code{0} : Approved from payment system.";

    /// <summary>
    /// Declined from payment system message constant.
    /// </summary>
    public const string PaymentSystemDeclined = "Code{0} : Declined from payment system.";

    /// <summary>
    /// Message shown when order is packed successfully.
    /// </summary>
    public const string InProcessSubstateSuccessfullyChanged = "Changed from {0} to {1}";

    /// <summary>
    /// Message shown when order is packing has failed.
    /// </summary>
    public const string InProcessSubstateFailedToChange = "Tried to change from {0} to {1}";

    /// <summary>
    /// Message shown when order is marked as suspicious successfully.
    /// </summary>
    public const string MarkedAsSuspiciousSuccessfully = "Marked as suspicious. The {0} product (code - {1}) has suspicious quantity - {2}";

    /// <summary>
    /// Message shown when order is failed to mark as suspicious.
    /// </summary>
    public const string FailedToMarkAsSuspicious = "Failed to mark as suspicious";

    #endregion

    #region Actions

    /// <summary>
    /// Create order action contsant.
    /// </summary>
    public const string CreateOrderAction = "Create order";
    
    /// <summary>
    /// Capture order action constant.
    /// </summary>
    public const string CaptureOrderAction = "Capture order";

    /// <summary>
    /// Cancel reservation action constant.
    /// </summary>
    public const string CancelReservationOrderAction = "Cancel reservation of amount for order";

    /// <summary>
    /// Print order action constant.
    /// </summary>
    public const string PrintOrderAction = "Print order";

    /// <summary>
    /// Send order confirmation action constant.
    /// </summary>
    public const string SendOrderConfirmationAction = "Send order confirmation";

    /// <summary>
    /// Update order action constant.
    /// </summary>
    public const string UpdateOrderAction = "Update order";

    /// <summary>
    /// Delete order line constant.
    /// </summary>
    public const string DeleteOrderLineAction = "Delete order line";

    /// <summary>
    /// Add order line constant.
    /// </summary>
    public const string AddOrderLineAction = "Add new order line";

    /// <summary>
    /// Edit order line constant.
    /// </summary>
    public const string EditOrderLineAction = "Change order line";

    /// <summary>
    /// Change order line quantity constant.
    /// </summary>
    public const string ChangeOrderLineQuantityAction = "Change order line quantity action";

    /// <summary>
    /// Pack In Full constant.
    /// </summary>
    public const string ProcessInFullAction = "{0} In Full";

    /// <summary>
    /// The "Mark as suspicious" constant.
    /// </summary>
    public const string MarkAsSuspicious = "Mark as suspicious";

    #endregion

    #region Entity Types

    /// <summary>
    /// Order entity type.
    /// </summary>
    public const string OrderEntityType = "Order";

    #endregion

    #region Level Codes

    /// <summary>
    /// Payment system level constant.
    /// </summary>
    public const string PaymentSystemLevel = "Payment System";

    /// <summary>
    /// User level constant.
    /// </summary>
    public const string UserLevel = "User";

    #endregion

    #region Results

    /// <summary>
    /// Approved result constant.
    /// </summary>
    public const string ApprovedResult = "Approved";

    /// <summary>
    /// Denied result constant.
    /// </summary>
    public const string DeniedResult = "Denied";

    #endregion
  }
}