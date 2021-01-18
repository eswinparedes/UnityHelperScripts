using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace RXUI
{
    public class SingleSelectEnd : ASingleSelectChainItem
    {
        void Start()
        {
            SingleSelectableBehaviours.InjectEnterExitObservations(this.gameObject, this.gameObject);
        }
    }

}
