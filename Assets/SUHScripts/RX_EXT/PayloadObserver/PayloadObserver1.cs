using System;

namespace SUHScripts
{
    public class PayloadObserver<TPayload> : IPayloadObserver<TPayload>
    {   
        public PayloadObserver(Action<TPayload> onPayload)
        {
            m_onPayload = onPayload;
        }

        Action<TPayload> m_onPayload;

        public void ObservePayload(TPayload payLoad)
        {
            m_onPayload(payLoad);
        }
    }
}