// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingHandler.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the LoggingHandler type.
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
  using System.Collections.Generic;
  using System.Linq;
  using Diagnostics;
  using Microsoft.Practices.Unity.InterceptionExtension;
  using OrderManagement.Orders;

  /// <summary>
  /// Logging handler.
  /// </summary>
  public class LoggingHandler : ICallHandler
  {
    /// <summary>
    /// Gets or sets the logger.
    /// </summary>
    /// <value>
    /// The logger.
    /// </value>
    public Logger Logger { get; set; }

    /// <summary>
    /// Gets or sets the order in which the handler will be executed
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Implement this method to execute your handler processing.
    /// </summary>
    /// <param name="input">
    /// Inputs to the current call to the target.</param><param name="getNext">
    /// Delegate to execute to get the next delegate in the handler chain.
    /// </param>
    /// <returns>
    /// Return value from the target.
    /// </returns>
    [NotNull]
    public IMethodReturn Invoke([NotNull] IMethodInvocation input, [NotNull] GetNextHandlerDelegate getNext)
    {
      Assert.ArgumentNotNull(input, "input");
      Assert.ArgumentNotNull(getNext, "getNext");

      Assert.IsNotNull(this.Logger, "Unable to log the action. Logger is not set.");
      
      LogThisAttribute logThisAttribute = input.MethodBase.GetCustomAttributes(typeof(LogThisAttribute), false).FirstOrDefault() as LogThisAttribute;
      Assert.IsNotNull(logThisAttribute, "Unable to log the action. The intercepted method should be marked with LogThisAttribute.");

      Assert.IsTrue(input.Arguments.Count == 1, "Unable to log the action. The intercepted method should take exaclty one argument.");
      Assert.IsTrue(input.Arguments[0] is Order, "Unable to log the action. The intercepted method should take Order as an argument.");

      Order order = input.Arguments[0] as Order;
      string orderNumber = order.OrderId;
      Assert.IsNotNullOrEmpty(orderNumber, "Unable to log the action. The intercepted method should take Order with filled OrderId.");


      // OrderId can also be a string.
      //long entityId;
      //Assert.IsTrue(long.TryParse(orderNumber, out entityId), "Unable to log the action. The intercepted method should take Order wich OrderId can be cast to System.Int64.");
      
      string action = logThisAttribute.Action;
      string levelCode = logThisAttribute.LevelCode;

      const string EntityType = "Order"; 
      const string SuccessResult = "Approved";
      const string FailResult = "Denied";

      IMethodReturn result = getNext()(input, getNext);

      this.Logger.Write(EntityType, orderNumber, action, levelCode, result.Exception == null ? SuccessResult : FailResult);

      return result;
    }
  }
}