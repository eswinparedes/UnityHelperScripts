using SUHScripts;
using SUHScripts.Functional;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace RXUI
{
    public class SingleSelector<TSingleSelection, TRawSelect> : IDisposable
    {
        Func<TSingleSelection, IObservable<TSingleSelection>> m_commandSelector;
        Func<TSingleSelection, TRawSelect> m_rawSelectFunction;
        Func<TSingleSelection, TRawSelect> m_clearFunction;
        Func<TSingleSelection, TSingleSelection, bool> m_comparer;
        bool m_doesRootClearAllOnExit = false;

        public SingleSelector(
            Func<TSingleSelection, IObservable<TSingleSelection>> commandSelector,
            Func<TSingleSelection, TRawSelect> rawSelectFunction,
            Func<TSingleSelection, TRawSelect> clearFunction,
            Func<IObservable<TRawSelect>, IObservable<ASingleSelect<TSingleSelection>>> singleSelection,
            IObservable<bool> rootSelectionEnterExit, Func<TSingleSelection, TSingleSelection, bool> comparer,
            bool doesInitWithFirst = true,
            bool doesResetToFirstOnEnter = false,
            bool doesRootClearAllOnExit = false)
        {
            m_commandSelector = commandSelector;
            m_rawSelectFunction = rawSelectFunction;
            m_clearFunction = clearFunction;
            m_comparer = comparer;
            var clearEnterExit = rootSelectionEnterExit.DistinctUntilChanged();

            m_doesRootClearAllOnExit = doesRootClearAllOnExit;

            EnterExitStream = clearEnterExit;

            List<IObservable<TRawSelect>> rawSelections = new List<IObservable<TRawSelect>>();

            if (doesInitWithFirst)
            {
                var toMerge = clearEnterExit.Where(isEnter => isEnter).Where(_ => m_singleSelections.Count > 0).Take(1).Select(_ => rawSelectFunction(m_singleSelections[0]));
                rawSelections.Add(toMerge);
            }

            if (doesResetToFirstOnEnter)
            {
                if (doesInitWithFirst)
                {
                    var toMerge = clearEnterExit.Where(isEnter => isEnter).Where(_ => m_singleSelections.Count > 0).Skip(1).Select(_ => rawSelectFunction(m_singleSelections[0]));
                    rawSelections.Add(toMerge);
                }
                else
                {
                    var toMerge = clearEnterExit.Where(isEnter => isEnter).Where(_ => m_singleSelections.Count > 0).Select(_ => rawSelectFunction(m_singleSelections[0]));
                    rawSelections.Add(toMerge);
                }
            }

            var selectStream = singleSelection(m_newSubject);
            SelectStream = selectStream;

            m_rawSelectionStream.Merge().Merge(rawSelections.Merge()).Subscribe(m_newSubject).AddTo(m_disposables);
            EnterExitStream.Subscribe(isEnter => IsEntered = isEnter).AddTo(m_disposables);
        }

        public IObservable<bool> RegisterSingleSelection(TSingleSelection selection)
        {
            IObservable<TRawSelect> selectionStream = null;

            if (m_doesRootClearAllOnExit)
            {
                var commandStream = m_commandSelector(selection).Select(m_rawSelectFunction);//.Valve(EnterExitStream, IsEntered).Select(m_rawSelectFunction);
                var exitStream = EnterExitStream.Where(isEnter => !isEnter).Select(_ => m_clearFunction(selection));
                selectionStream = commandStream.Merge(exitStream);
            }
            else
            {
                selectionStream = m_commandSelector(selection).Select(m_rawSelectFunction);// .Valve(EnterExitStream, IsEntered).Select(m_rawSelectFunction);
            }

            m_rawSelectionStream.OnNext(selectionStream);

            var obvs = SelectStream.Choose(selected =>
            {
                if (m_comparer(selected.Select.Value, selection)) return true.AsOption_SAFE();
                if (m_comparer(selected.Deselect.Value, selection)) return false.AsOption_SAFE();
                return None.Default;
            });

            m_singleSelections.Add(selection);

            return obvs;
        }

        public void ExitFor(TSingleSelection selection)
        {
            m_newSubject.OnNext(m_clearFunction(selection));
        }

        public void Dispose()
        {
            new CompositeDisposable(m_disposables).Dispose();
        }

        ReactiveCollection<TSingleSelection> m_singleSelections = new ReactiveCollection<TSingleSelection>();
        Subject<TRawSelect> m_newSubject = new Subject<TRawSelect>();
        Subject<IObservable<TRawSelect>> m_rawSelectionStream = new Subject<IObservable<TRawSelect>>();
        List<IDisposable> m_disposables = new List<IDisposable>();

        public IObservable<ASingleSelect<TSingleSelection>> SelectStream { get; }
        public IObservable<bool> EnterExitStream { get; }
        public bool IsEntered { get; private set; }
    }

    public interface IEnterExitObserver
    {
        void ObserveEnterExitCommands(IEnterExitObservable enterExitCommands);
    }

    public interface IEnterExitObservable
    {
        IObservable<bool> EnterExitStream { get; }
        IObservable<(float alpha, FTimer progress)> Progress { get; }
    }

    public interface ISingleSelectable
    {
        IObservable<ISingleSelectable> SelectStream { get; }
        void ObserveEnterExit(IObservable<bool> enterExitCommands);
    }


    public class SingleSelectableSource : ISingleSelectable
    {
        public SingleSelectableSource(IObservable<Unit> selectSelfStream, Action<IObservable<bool>> onEnterExitCommands)
        {
            SelectStream = selectSelfStream.Select(_ => this);
            OnEnterExitCommands = onEnterExitCommands;
        }
        public SingleSelectableSource(IObservable<ISingleSelectable> selectStream, Action<IObservable<bool>> onEnterExitCommands)
        {
            SelectStream = selectStream;
            OnEnterExitCommands = onEnterExitCommands;
        }

        Action<IObservable<bool>> OnEnterExitCommands { get; }
        public IObservable<ISingleSelectable> SelectStream { get; }

        public void ObserveEnterExit(IObservable<bool> enterExitCommands)
        {
            OnEnterExitCommands(enterExitCommands);
        }
    }

    [System.Serializable]
    public class SingleSelectChainWithButton : ISingleSelectable
    {
        [SerializeField] ASingleSelectChainItem m_chainItem = default;
        [SerializeField] Button m_button = default;

        public ASingleSelectChainItem ChainItem => m_chainItem;
        public Button Button => m_button;
        public IObservable<ISingleSelectable> SelectStream => m_button.OnClickAsObservable().Select(_ => this);
        public void ObserveEnterExit(IObservable<bool> enterExitCommands) => m_chainItem.ObserveEnterExit(enterExitCommands);
    }
}