using System;
using SUHScripts.Functional.UnitType;

namespace SUHScripts.Functional
{
    public static partial class Functional
    {
        public static Unit UNIT => default(Unit);

        public static Unit UnitExecute(Action action)
        {
            action();
            return UNIT;
        }

        public static None NONE =>
            None.Default;

        public static Some<T> Some<T>(T value) =>
            new Some<T>(value);


    }
}