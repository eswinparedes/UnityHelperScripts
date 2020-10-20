using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;
using SUHScripts.Functional;

namespace RXUI
{
    public class SingleSelectAutoRoot : AEnterExitObservable
    {
        [Header("Activation Settings")]
        [SerializeField] List<SingleSelectChainWithButton> m_items = default;
        [SerializeField] KeyCode m_activationKey = KeyCode.Escape;
        [SerializeField] bool m_enterOnStart = true;

        [Header("Single Select Root Settings")]
        [SerializeField] bool m_initWithFirst = true;
        [SerializeField] bool m_resetToFirstOnEnter = false;
        [SerializeField] bool m_exitAllOnExit = false;

        protected override void Awake()
        {
            base.Awake();

            var comparer = SingleSelectableOperations.ClassComparer<ISingleSelectable>();

            var root = new SingleSelector<ISingleSelectable, Option<ISingleSelectable>>(
                section => section.SelectStream,
                section => section.AsOption_SAFE(),
                section => None.Default,
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
            SingleSelectableBehaviours.InjectEnterExitObservations(this.gameObject, this.gameObject);

            if (m_enterOnStart)
                RaiseEnter();
        }
    }

}
