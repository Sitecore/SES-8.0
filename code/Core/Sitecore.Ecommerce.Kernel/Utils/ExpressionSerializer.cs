// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionSerializer.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// <summary>
//   Defines the expression serializer class.
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

namespace Sitecore.Ecommerce.Utils
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;
  using System.Linq.Expressions;
  using System.Reflection;
  using CodeDom.Scripts;
  using Diagnostics;
  using Globalization;

  /// <summary>
  /// Defines the expression serializer class.
  /// </summary>
  public class ExpressionSerializer
  {
    /// <summary>
    /// Represents code for expression provider.
    /// </summary>
    private const string CodeTemplateForFilter = @"
      namespace Sitecore.Ecommerce.Apps.OrderManagement.OrderManager.Utils.DynamicallyGenerated
      {{
        using System;
        using System.Collections.Generic;
        using System.Linq;
        
        public static class Filter
        {{
          private static bool False = false;
          private static bool True = true;

          private static T IIF<T>(bool condition, T left, T right)
          {{
            return condition ? left : right;
          }}
          
          public static object ApplyCompiledFilterExpression({0}<{1}> source)
          {{
            return {2};
          }}
        }}
      }}";

    /// <summary>
    /// Serializes the specified expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>Serialized expression string.</returns>
    [NotNull]
    public virtual string Serialize([NotNull] Expression expression)
    {
      using (new ThreadCultureSwitcher(CultureInfo.InvariantCulture))
      {
        var result = this.SubstituteNonConstantSubexpressionsWithCurrentValues(expression).ToString();
        Expression root = this.GetExpressionCallRoot(expression);

        if (root != null)
        {
          result = result.Replace(root.ToString(), "{0}");
        }

        return this.FixBooleanOperations(result);
      }
    }
    
    /// <summary>
    /// Applies the expression as filter.
    /// </summary>
    /// <typeparam name="T">the type.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="filterExpression">The filter expression.</param>
    /// <returns>object that has been filtered by</returns>
    public virtual object ApplyExpressionAsFilter<T>([NotNull] IQueryable<T> source, [NotNull] string filterExpression)
    {
      Assert.ArgumentNotNull(source, "source");
      Assert.ArgumentNotNull(filterExpression, "filterExpression");

      ICollection<string> references = new HashSet<string>();

      this.PopulateSetWithReferencedAssembliesLocations(typeof(T), references);
      this.PopulateSetWithReferencedAssembliesLocations(typeof(IQueryable), references);

      var code = string.Format(CodeTemplateForFilter, "IQueryable", this.BuildTypeName(typeof(T)), string.Format(this.FixCasing(filterExpression), "source"));

      var script = ScriptFactory.Compile(filterExpression, references, code, "C#");
      Assert.IsTrue(script.IsValid, "Unable to apply the filter. Expression compilation failed.");

      return script.InvokeDefaultMethod<object>(source);
    }

    /// <summary>
    /// Populates the set with referenced assemblies locations.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="result">The result.</param>
    protected virtual void PopulateSetWithReferencedAssembliesLocations(Type type, ICollection<string> result)
    {
      if (!result.Contains(Assembly.GetAssembly(type).Location))
      {
        result.Add(Assembly.GetAssembly(type).Location);
      }

      foreach (var t in type.GetGenericArguments())
      {
        var currentType = t;

        while (currentType != null)
        {
          this.PopulateSetWithReferencedAssembliesLocations(currentType, result);

          currentType = currentType.BaseType;
        }
      }
    }

    /// <summary>
    /// Builds the name of the type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>Name of the type appropriate for using in C# code.</returns>
    protected virtual string BuildTypeName(Type type)
    {
      var result = type.FullName.Replace('+', '.');
      var genericArguments = type.GetGenericArguments();

      if (genericArguments.Length > 0)
      {
        result = string.Format("{0}<{1}>", result.Substring(0, result.IndexOf('`')), string.Join(",", genericArguments.Select(this.BuildTypeName)));
      }

      return result;
    }

    /// <summary>
    /// Fixes the casing.
    /// </summary>
    /// <param name="expressionSerialization">The expression serialization.</param>
    /// <returns>The casing.</returns>
    protected virtual string FixCasing(string expressionSerialization)
    {
      Assert.ArgumentNotNull(expressionSerialization, "expressionSerialization");

      return expressionSerialization.Replace(" Is", " is ");
    }

    /// <summary>
    /// Fixes AND and OR boolean operations by changing AndAlso and OrElse to the operators defined previously.
    /// </summary>
    /// <param name="expression">The expression to fix operators for.</param>
    /// <returns>Fixed expression.</returns>
    protected virtual string FixBooleanOperations(string expression)
    {
      return expression.Replace(" AndAlso ", " && ").Replace(" OrElse ", " || ").Replace(" And ", " & ").Replace(" Or ", " | ");
    }

    /// <summary>
    /// Gets the expression call root.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>The expression call root.</returns>
    [CanBeNull]
    protected virtual ConstantExpression GetExpressionCallRoot([NotNull] Expression expression)
    {
      Assert.ArgumentNotNull(expression, "expression");

      var result = expression;
      while (!(result == null || result is ConstantExpression))
      {
        var methodCallExpression = result as MethodCallExpression;

        if (methodCallExpression != null && methodCallExpression.Method.IsStatic)
        {
          result = ((MethodCallExpression)result).Arguments.FirstOrDefault();
        }
        else
        {
          result = null;
        }
      }

      return (ConstantExpression)result;
    }

    /// <summary>
    /// Substitutes the non constant sub expressions with current values.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>
    /// The non constant sub expressions with current values.
    /// </returns>
    protected virtual Expression SubstituteNonConstantSubexpressionsWithCurrentValues([NotNull] Expression expression)
    {
      Assert.ArgumentNotNull(expression, "expression");

      if (expression is ConstantExpression)
      {
        return expression;
      }

      if (expression is ParameterExpression)
      {
        return expression;
      }

      var memberExpression = expression as MemberExpression;

      if (memberExpression != null)
      {
        var processedExpression = memberExpression.Expression != null ? this.SubstituteNonConstantSubexpressionsWithCurrentValues(memberExpression.Expression) : null;

        if ((processedExpression != null) && (!(processedExpression is ConstantExpression)))
        {
          return memberExpression;
        }

        object instance = null;

        if (processedExpression != null)
        {
          instance = ((ConstantExpression)processedExpression).Value;
        }

        var info = memberExpression.Member as FieldInfo;
        return Expression.Constant(info != null ? info.GetValue(instance) : ((PropertyInfo)memberExpression.Member).GetValue(instance, new object[0]));
      }

      var methodCallExpression = expression as MethodCallExpression;

      if (methodCallExpression != null)
      {
        var processedExpression = methodCallExpression.Object != null ? this.SubstituteNonConstantSubexpressionsWithCurrentValues(methodCallExpression.Object) : null;

        if ((processedExpression == null) || (processedExpression is ConstantExpression))
        {
          object instance = null;

          if (processedExpression != null)
          {
            instance = ((ConstantExpression)processedExpression).Value;
          }

          var argumentExpressions = methodCallExpression.Arguments.Select(this.SubstituteNonConstantSubexpressionsWithCurrentValues);

          if (methodCallExpression.Method.IsStatic &&
            ((methodCallExpression.Method.DeclaringType == typeof(Enumerable)) ||
            (methodCallExpression.Method.DeclaringType == typeof(Queryable))))
          {
            return Expression.Call(processedExpression, methodCallExpression.Method, argumentExpressions);
          }

          var expressions = argumentExpressions as Expression[] ?? argumentExpressions.ToArray();
          if (!expressions.All(argumentExpression => argumentExpression is ConstantExpression))
          {
            throw new NotSupportedException("Parameters are not allowed on external method call");
          }

          return Expression.Constant(methodCallExpression.Method.Invoke(instance, expressions.Select(argument => ((ConstantExpression)argument).Value).ToArray()));
        }

        foreach (var argumentExpression in methodCallExpression.Arguments)
        {
          this.SubstituteNonConstantSubexpressionsWithCurrentValues(argumentExpression);
        }
        
        return Expression.Call(methodCallExpression.Object, methodCallExpression.Method, methodCallExpression.Arguments.Select(this.SubstituteNonConstantSubexpressionsWithCurrentValues));
      }

      var unaryExpression = expression as UnaryExpression;

      if (unaryExpression != null)
      {
        return Expression.MakeUnary(unaryExpression.NodeType, this.SubstituteNonConstantSubexpressionsWithCurrentValues(unaryExpression.Operand), unaryExpression.Type, unaryExpression.Method);
      }

      var binaryExpression = expression as BinaryExpression;

      if (binaryExpression != null)
      {
        return Expression.MakeBinary(binaryExpression.NodeType, this.SubstituteNonConstantSubexpressionsWithCurrentValues(binaryExpression.Left), this.SubstituteNonConstantSubexpressionsWithCurrentValues(binaryExpression.Right), binaryExpression.IsLiftedToNull, binaryExpression.Method);
      }

      var conditionalExpression = expression as ConditionalExpression;

      if (conditionalExpression != null)
      {
        return Expression.Condition(this.SubstituteNonConstantSubexpressionsWithCurrentValues(conditionalExpression.Test), this.SubstituteNonConstantSubexpressionsWithCurrentValues(conditionalExpression.IfTrue), this.SubstituteNonConstantSubexpressionsWithCurrentValues(conditionalExpression.IfFalse));
      }

      var typeBinaryExpression = expression as TypeBinaryExpression;

      if (typeBinaryExpression != null)
      {
        return Expression.TypeIs(this.SubstituteNonConstantSubexpressionsWithCurrentValues(typeBinaryExpression.Expression), typeBinaryExpression.TypeOperand);
      }

      var lambdaExpression = expression as LambdaExpression;

      if (lambdaExpression != null)
      {
        return Expression.Lambda(this.SubstituteNonConstantSubexpressionsWithCurrentValues(lambdaExpression.Body), lambdaExpression.Parameters);
      }

      throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Serialization is not supported for the following expression: {0}", expression));
    }
  }
}