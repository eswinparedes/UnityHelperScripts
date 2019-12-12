using UnityEngine;
using System;
using UniRx;

public delegate IDisposable SubscriptionProvider();
public class SubscribableEffect : IDisposable
{
    public SubscribableEffect(SubscriptionProvider subscription)
    {
        this.m_subscriptionFunction = subscription;
    }

    IDisposable m_disposal;
    SubscriptionProvider m_subscriptionFunction;

    //SUHS TODO: Make subscription explicit?  EG Have erroor log if subscribe called twice without disposing
    public IDisposable Subscribe()
    {
        Dispose();
        m_disposal = m_subscriptionFunction();
        return m_disposal;
    }

    public void Dispose() =>
        m_disposal?.Dispose();

    public IDisposable SubscribeAndAddTo(Component attachTo) =>
        Subscribe().AddTo(attachTo);

}
