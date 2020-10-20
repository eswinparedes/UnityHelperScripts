using System;
using UniRx;

namespace RXUI
{
    public abstract class ASingleSelectChainItem : AEnterExitObservable
    {
        public void ObserveEnterExit(IObservable<bool> enterExitCommands)
        {
            enterExitCommands.Subscribe(isEnter =>
            {
                if (isEnter) RaiseEnter();
                else RaiseExit();
            }).AddTo(this);
        }

    }
}
