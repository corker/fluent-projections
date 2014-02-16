﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentProjections.EventHandlingStrategies.Arguments
{
    public class Filter<TEvent>
    {
        private readonly PropertyInfo _property;
        private readonly Func<TEvent, object> _getValue;

        private Filter(PropertyInfo property, Func<TEvent, object> getValue)
        {
            _property = property;
            _getValue = getValue;
        }

        public FluentProjectionFilterValue GetValue(TEvent @event)
        {
            return new FluentProjectionFilterValue(_property, _getValue(@event));
        }

        public static Filter<TEvent> Create<TProjection, TValue>(
            Expression<Func<TProjection, TValue>> projectionProperty, 
            Func<TEvent, TValue> getValue)
        {
            var memberExpression = (MemberExpression)projectionProperty.Body;
            var property = (PropertyInfo)memberExpression.Member;
            return new Filter<TEvent>(property, e => getValue(e));
        }
    }
}