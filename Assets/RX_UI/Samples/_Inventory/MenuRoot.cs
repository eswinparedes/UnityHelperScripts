using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;
using SUHScripts.Functional;

namespace RXUI
{
    public class MenuRoot : AEnterExitObservable
    {
        [Header("Activation Settings")]
        [SerializeField] List<SingleSelectChainWithButton> m_items = default;
        [SerializeField] KeyCode m_activationKey = KeyCode.Escape;
        [Header("Single Select Root Settings")]
        [SerializeField] bool m_initWithFirst = true;
        [SerializeField] bool m_resetToFirstOnEnter = false;
        [SerializeField] bool m_exitAllOnExit = false;

        protected override void Awake()
        {
            base.Awake();

            SingleSelectableBehaviours.InjectEnterExitObservations(this.gameObject, this.gameObject);

            this.UpdateAsObservable().Where(_ => Input.GetKeyDown(m_activationKey))
                .Scan(false, (p, n) => !p).Subscribe(isEnter =>
                {
                    if (isEnter) RaiseEnter();
                    else RaiseExit();
                }).AddTo(this);

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

        }
    }

}
