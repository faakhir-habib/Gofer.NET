﻿using System.Collections.Generic;
using System.Linq.Expressions;

namespace Gofer.NET.Utils
{
    public class MethodCallArgumentResolutionVisitor : ExpressionVisitor
    {
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var argumentExpressions = new List<Expression>();
            foreach (var argument in node.Arguments)
            {
                var argumentResolver = Expression.Lambda(argument);
                var argumentValue = argumentResolver.Compile().DynamicInvoke();

                var valueExpression = Expression.Constant(argumentValue, argument.Type);
                argumentExpressions.Add(valueExpression);
            }

            if (node.Method.Name == nameof(TaskExtensions.T)
                && node.Method.DeclaringType == typeof(TaskExtensions))
            {
                node = (MethodCallExpression) node.Object;
            }
            
            return Expression.Call(node.Object, node.Method, argumentExpressions);
        }
    }
}