﻿using System;
using System.Linq;
using static SUHScripts.Functional.Functional;
using SUHScripts.Functional.UnitType;
namespace SUHScripts.Functional
{
    public static class ValidationExtensions
    {
        public static T GetOrElse<T>(this Validation<T> opt, T defaultValue)
           => opt.Match(
              (errs) => defaultValue,
              (t) => t);

        public static T GetOrElse<T>(this Validation<T> opt, Func<T> fallback)
           => opt.Match(
              (errs) => fallback(),
              (t) => t);

        public static Validation<R> Apply<T, R>(this Validation<Func<T, R>> valF, Validation<T> valT)
           => valF.Match(
              Valid: (f) => valT.Match(
                 Valid: (t) => Valid(f(t)),
                 Invalid: (err) => Invalid(err)),
              Invalid: (errF) => valT.Match(
                 Valid: (_) => Invalid(errF),
                 Invalid: (errT) => Invalid(errF.Concat(errT))));


        public static Validation<Func<T2, R>> Apply<T1, T2, R>
           (this Validation<Func<T1, T2, R>> @this, Validation<T1> arg)
           => Apply(@this.Map(Functional.Curry), arg);

        public static Validation<Func<T2, T3, R>> Apply<T1, T2, T3, R>
           (this Validation<Func<T1, T2, T3, R>> @this, Validation<T1> arg)
           => Apply(@this.Map(Functional.CurryFirst), arg);

        public static Validation<RR> Map<R, RR>
           (this Validation<R> @this, Func<R, RR> f)
           => @this.IsValid
              ? Valid(f(@this.Value))
              : Invalid(@this.Errors);

        public static Validation<Func<T2, R>> Map<T1, T2, R>(this Validation<T1> @this
           , Func<T1, T2, R> func)
            => @this.Map(func.Curry());

        public static Validation<Unit> ForEach<R>
           (this Validation<R> @this, Action<R> act)
           => Map(@this, act.ToFunc());

        public static Validation<T> Do<T>
           (this Validation<T> @this, Action<T> action)
        {
            @this.ForEach(action);
            return @this;
        }

        public static Validation<R> Bind<T, R>
           (this Validation<T> val, Func<T, Validation<R>> f)
            => val.Match(
               Invalid: (err) => Invalid(err),
               Valid: (r) => f(r));


        // LINQ

        public static Validation<R> Select<T, R>(this Validation<T> @this
           , Func<T, R> map) => @this.Map(map);

        public static Validation<RR> SelectMany<T, R, RR>(this Validation<T> @this
           , Func<T, Validation<R>> bind, Func<T, R, RR> project)
           => @this.Match(
              Invalid: (err) => Invalid(err),
              Valid: (t) => bind(t).Match(
                 Invalid: (err) => Invalid(err),
                 Valid: (r) => Valid(project(t, r))));
    }
}
