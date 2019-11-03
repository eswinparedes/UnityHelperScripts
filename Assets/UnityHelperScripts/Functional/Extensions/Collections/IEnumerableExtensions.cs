using System;
using System.Collections.Generic;
using System.Linq;

namespace SUHScripts.Functional
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Executes an Action on each item returning Unit for that item- execution is deffered
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<Unit> ForEach<T>(this IEnumerable<T> ts, Action<T> action) =>
            ts.Select(action.ToFunc());

        /// <summary>
        /// Executes an Action on each item, return that item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<T> ForEachPass<T>(this IEnumerable<T> ts, Action<T> action)
        {
            foreach (T item in ts)
            {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        /// Quick method to evaluate Ienumerable items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        public static void Consume<T>(this IEnumerable<T> ts) =>
            ts.ToList();

        public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> ts, Func<T, IEnumerable<R>> f)
        {
            foreach (T t in ts)
                foreach (R r in f(t))
                    yield return r;
        }

        public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> list, Func<T, Option<R>> func) =>
            list.Bind(t => func(t).AsEnumerable());

        public static IEnumerable<R> Map<T, R>(this IEnumerable<T> ts, Func<T, R> f)
        {
            foreach (var t in ts)
                yield return f(t);
        }

        public static bool ContainsAllItemsIn<T>(this IEnumerable<T> a, IEnumerable<T> b) =>
        !b.Except(a).Any();

        public static bool ContainsAtLeastOneIn<T>(this IEnumerable<T> a, IEnumerable<T> b) =>
            a.Intersect(b).Any();

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N) =>
            source.Skip(Math.Max(0, source.Count() - N));

    }
}