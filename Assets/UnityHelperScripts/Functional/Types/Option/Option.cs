using System;
using static SUHScripts.Functional.Functional;
using System.Collections.Generic;

namespace SUHScripts.Functional
{
    public struct Option<T>
    {
        readonly bool isSome;
        readonly T value;

        private Option(T value)
        {

            this.isSome = value.NotNull();
            this.value = value;
        }

        public static Option<TOut> FromValueType<TOut>(TOut value) where TOut : struct
        {
            return new Option<TOut>(value);
        }

        public static implicit operator Option<T>(None _) =>
            new Option<T>();

        public static implicit operator Option<T>(Some<T> some) =>
            new Option<T>(some.Value);

        public static implicit operator Option<T>(T value) =>
            value.IsNull() ? (Option<T>) NONE : Some(value);

        public R Match<R>(Func<R> onNone, Func<T, R> onSome) =>
            isSome ?  onSome(value) : onNone();

        public R Match<R>(R onNone, R onSome) =>
            isSome ? onSome : onNone;

        public T Reduce(T onNone) =>
            isSome ? value : onNone;

        public IEnumerable<T> AsEnumerable()
        {
            if (isSome) yield return value;
        }

        public bool IsSome => isSome;
        public T Value => value;

        public override string ToString() =>
            isSome ? value.ToString() : "NONE";
    }
}