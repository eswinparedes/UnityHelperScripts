﻿using System;

namespace SUHScripts.Functional
{
    public struct Some<T>
    {
        internal T Value { get; }

        internal Some(T value)
        {
            if (value.IsNull())
                throw new ArgumentNullException();

            Value = value;
        }
    }
}