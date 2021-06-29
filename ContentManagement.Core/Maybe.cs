using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatoutCMS.ConntentManagement
{
    public sealed class Maybe<ValueType>
    {
        private readonly IEnumerable<ValueType> values;

        public Maybe(ValueType value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            values = new[] { value };
        }

        public Maybe() =>
            values = new ValueType[0];

        public ValueType ValueOrDefault(ValueType defaultValue) =>
            values.DefaultIfEmpty(defaultValue).Single();

        public void Apply(Action<ValueType> action)
        {
            if (values.Any())
                action.Invoke(values.Single());
        }

        public void IfEmpty(Action action)
        {
            if (!values.Any())
                action.Invoke();
        }
    }
}
