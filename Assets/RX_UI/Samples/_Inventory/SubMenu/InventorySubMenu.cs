using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using SUHScripts;
using SUHScripts.Functional;
using System;
using UniRx.Triggers;

namespace RXUI
{
    
    public class InventorySubMenu : ASingleSelectChainItem
    {
        [Header("Spawn Settings")]
        [SerializeField] InventoryConsumableElement m_elementPrefab = default;
        [SerializeField] RectTransform m_contentRoot = default;
        [Header("Single Select Link Settings")]
        [SerializeField] bool m_initWithFirst = true;
        [SerializeField] bool m_resetToFirstOnEnter = false;
        [SerializeField] bool m_exitAllOnExit = false;

        static Subject<InventoryItem> m_inventoryAddStream = new Subject<InventoryItem>();
        static Subject<InventoryItem> m_inventoryUseStream = new Subject<InventoryItem>();
        public static IObservable<InventoryItem> InventoryUseStream => m_inventoryUseStream;
        public static void RequestInventoryAdd(InventoryItem item)
        {
            m_inventoryAddStream.OnNext(item);
        }

        ReactiveCollection<ISingleSelectable> m_items = new ReactiveCollection<ISingleSelectable>();
        ReactiveDictionary<InventoryItem, MutableInventoryGroup> m_inventory = new ReactiveDictionary<InventoryItem, MutableInventoryGroup>();

        protected override void Awake()
        {
            base.Awake();

            SingleSelectableBehaviours.InjectEnterExitObservations(this.gameObject, this.gameObject);

            var comparer = SingleSelectableOperations.ClassComparer<ISingleSelectable>();

            var root = new SingleSelector<ISingleSelectable, Option<ISingleSelectable>>(
                item => item.SelectStream,
                item => item.AsOption_SAFE(),
                item => None.Default,
                obvs => obvs.ToggleSelect(comparer),
                EnterExitStream,
                comparer,
                m_initWithFirst,
                m_resetToFirstOnEnter, 
                m_exitAllOnExit);

            root.AddTo(this);

            m_inventoryAddStream.Subscribe(item =>
            {
                if (m_inventory.ContainsKey(item))
                {
                    var grp = m_inventory[item];
                    grp.Count += 1;
                    grp.Element.DisplayFor(item, grp.Count);
                    m_inventory[item] = grp;

                    if (!grp.Element.gameObject.activeSelf) grp.Element.gameObject.SetActive(true);
                }
                else
                {
                    var spawned = GameObject.Instantiate(m_elementPrefab, m_contentRoot);
                    var singleSelect = new SingleSelectableSource(spawned.SelectStream, isEnter => spawned.ObserveEnterExit(isEnter));
                    var grp = new MutableInventoryGroup(spawned, singleSelect, 1);                    
                    
                    var obvs = root.RegisterSingleSelection(singleSelect);
                    singleSelect.ObserveEnterExit(obvs);
                    spawned.DisplayFor(item, grp.Count);
                    m_inventory[item] = grp;

                    spawned.ConsumeStream.Subscribe(_ =>
                    {
                        var _grp = m_inventory[item];
                        _grp.Count--;
                        spawned.DisplayFor(item, _grp.Count);
                        if(_grp.Count <= 0)
                        {
                            root.ExitFor(_grp.Selectable);
                            _grp.Element.gameObject.SetActive(false);
                        }
                        m_inventory[item] = _grp;
                    }).AddTo(this);
                }
            }).AddTo(this);

            

            List<InventoryItem> invItems = new List<InventoryItem>();
            this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.Space)).Subscribe(_ =>
            {
                if(invItems.Count > 100 || (UnityEngine.Random.Range(0, 1f) > .5f && invItems.Count > 0))
                {
                    RequestInventoryAdd(invItems.RandomElement());
                }
                else
                {
                    var element = new InventoryItem(DateTime.Now.Millisecond.ToString(), 1);
                    invItems.Add(element);
                    RequestInventoryAdd(element);
                }
            }).AddTo(this);
        }

        class MutableInventoryGroup
        {
            public MutableInventoryGroup(InventoryConsumableElement element, ISingleSelectable selectable, uint count)
            {
                Element = element;
                Selectable = selectable;
                Count = count;
            }

            public InventoryConsumableElement Element { get; }
            public ISingleSelectable Selectable { get; }
            public uint Count { get; set; }
        }
    }

}
