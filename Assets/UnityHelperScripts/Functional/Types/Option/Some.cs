using System;

namespace SUHScripts.Functional
{
    public struct Some<T>
    {
        internal T Value { get; }

        internal Some(T value)
        {
            if (value == null)
                throw new ArgumentNullException();

            Value = value;
        }
    }
}