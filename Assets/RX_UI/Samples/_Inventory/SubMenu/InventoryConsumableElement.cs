using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SUHScripts.Functional;
using System;
using UniRx;

namespace RXUI
{
    public class InventoryConsumableElement : ASingleSelectChainItem
    {
        [SerializeField] Text m_nameText = default;
        [SerializeField] Text m_stackText = default;
        [SerializeField] Button m_selectButton = default;
        [SerializeField] Button m_consumeButton = default;

        public IObservable<Unit> SelectStream => m_selectButton.OnClickAsObservable();
        public IObservable<Unit> ConsumeStream => m_consumeButton.OnClickAsObservable();
        public void DisplayFor(InventoryItem item, uint count)
        {
            m_nameText.text = item.Name;
            m_stackText.text = count.ToString();
        }

        protected override void Awake()
        {
            base.Awake();
            //EnterExitStream.Subscribe(isEnter => Debug.Log($"{gameObject.name} isEnter: {isEnter}")).AddTo(this);

            SingleSelectableBehaviours.InjectEnterExitObservations(this.gameObject, this.gameObject);
        }
    }
}
