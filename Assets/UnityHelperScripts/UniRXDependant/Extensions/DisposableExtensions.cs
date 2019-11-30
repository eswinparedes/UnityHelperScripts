using System.Collections.Generic;
using UniRx;
using System;
public static class DisposableExtensions 
{
    public static Subject<T> AddTo<T>(this Subject<T> @this, List<IDisposable> addTo)
    {
        addTo.Add(@this);
        return @this;
    }

    public static IDisposable AddTo(this IDisposable @this, List<IDisposable> subList)
    {
        subList.Add(@this);
        return @this;
    }

    public static IDisposable AsDisposable(this List<IDisposable> @this) =>
        Disposable.Create(() =>
        {
            for (int i = 0; i < @this.Count; i++)
                @this[i].Dispose();

            @this.Clear();
        });
}
