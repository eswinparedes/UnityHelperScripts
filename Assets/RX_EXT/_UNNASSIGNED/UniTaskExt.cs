using System.Threading;
using UnityEngine;
using UniRx;

public static class UniTaskExt
{
    public static void TryQuickCancelAndDispose(this CancellationTokenSource @this)
    {
        if (@this == null) return;

        if (!@this.IsCancellationRequested) @this.Cancel();

        @this.Dispose();
    }

    public static void DisposeOn(this CancellationTokenSource @this, Component component)
    {
        Disposable.Create(() => @this.TryQuickCancelAndDispose()).AddTo(component);   
    }

    public static CancellationToken RefreshToken(ref CancellationTokenSource tokenSource)
    {
        tokenSource?.TryQuickCancelAndDispose();
        tokenSource = new CancellationTokenSource();
        return tokenSource.Token;
    }
}