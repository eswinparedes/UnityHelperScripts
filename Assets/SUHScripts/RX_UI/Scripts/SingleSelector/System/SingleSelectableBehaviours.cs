using System;
using System.Collections.Generic;
using UnityEngine;

namespace RXUI
{
    public static class SingleSelectableBehaviours
    {
        public static void InjectEnterExitObservations(GameObject observableTargetSource, params GameObject[] observerTargetSources)
        {
            if (observerTargetSources.Length == 0) Debug.LogWarning($"No sources for Enter Exit Observations were submitted, was this on purpose?");

            var enterExitObservable = observableTargetSource.GetComponent<IEnterExitObservable>();

            if (enterExitObservable == null)
            {
                Debug.LogWarning($"No Enter Exit Observable found in {observableTargetSource.name}, was this on purpose?");
                return;
            }

            List<IEnterExitObserver[]> observersFound = new List<IEnterExitObserver[]>();

            for (int i = 0; i < observerTargetSources.Length; i++)
            {
                var observers = observerTargetSources[i].GetComponents<IEnterExitObserver>();
                if (observers == null || observers.Length == 0)
                {
                    Debug.LogWarning($"No Enter Exit Observer found in {observerTargetSources[i].name}, was this on purpose?");
                }
                else
                {
                    observersFound.Add(observers);
                }
            }

            for (int i = 0; i < observersFound.Count; i++)
            {
                for (int j = 0; j < observersFound[i].Length; j++)
                {
                    observersFound[i][j].ObserveEnterExitCommands(enterExitObservable);
                }
            }
        }

        public static Dictionary<TSingleSelection, IObservable<bool>> RegisterSingleSelectionGroup<TSingleSelection, TRawSelection>(this SingleSelector<TSingleSelection, TRawSelection> @this, IReadOnlyList<TSingleSelection> group)
        {
            var d = new Dictionary<TSingleSelection, IObservable<bool>>();

            for (int i = 0; i < group.Count; i++)
            {
                var closure = group[i];
                var obvs = @this.RegisterSingleSelection(closure);
                d.Add(closure, obvs);
            }

            return d;
        }
    }
}

