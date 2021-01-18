
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using SUHScripts;
using SUHScripts.Functional;

namespace RXUI
{
    public class SingleSelectLink : ASingleSelectChainItem
    {
        [Header("Select Settings")]
        [SerializeField] List<SingleSelectChainWithButton> m_items = default;

        [Header("Single Select Link Settings")]
        [SerializeField] bool m_initWithFirst = true;
        [SerializeField] bool m_resetToFirstOnEnter = false;
        [SerializeField] bool m_exitAllOnExit = false;

        protected override void Awake()
        {
            base.Awake();

            SingleSelectableBehaviours.InjectEnterExitObservations(this.gameObject, this.gameObject);

            var comparer = SingleSelectableOperations.ClassComparer<ISingleSelectable>();

            var root = new SingleSelector<ISingleSelectable, Option<ISingleSelectable>>(
                item => item.SelectStream,
                item => item.AsOption_SAFE(),
                item => None.Default,
                obvs => obvs.NewEntrySelect(comparer),
                EnterExitStream,
                comparer,
                m_initWithFirst,
                m_resetToFirstOnEnter,
                m_exitAllOnExit);

            root.AddTo(this);

            foreach (var kvp in root.RegisterSingleSelectionGroup(m_items))
            {
                kvp.Key.ObserveEnterExit(kvp.Value);
            }
        }

        private void Start()
        {
            
        }
    }
}

