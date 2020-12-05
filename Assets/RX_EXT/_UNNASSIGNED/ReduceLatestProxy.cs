using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SUHScripts.Functional;

namespace SUHScripts 
{
    public class ReduceLatestProxy<T>
    {
        Dictionary<object, T> m_latestValues = new Dictionary<object, T>();

        public ReduceLatestProxy(Func<T, T, T> aggregator, Func<int, T, T> reducer, Func<T, T, T> interpolator, T defaulValue)
        {
            m_defaultValue = defaulValue;
            m_aggregator = aggregator;
            m_reducer = reducer;
            m_interpolator = interpolator;
        }

        T m_defaultValue;
        Func<T, T, T> m_aggregator;
        Func<int, T, T> m_reducer;
        Func<T, T, T> m_interpolator;
        T m_lastValue;

        public T Update()
        {
            var valueCount = m_latestValues.Count;

            if (valueCount == 0)
            {
                m_lastValue = m_defaultValue;
                return m_defaultValue;
            }

            var enumerator = m_latestValues.GetEnumerator();
            T runningValue = default;
            int iterationCount = 0;
            while (enumerator.MoveNext())
            {
                if (iterationCount == 0) runningValue = enumerator.Current.Value;
                else runningValue = m_aggregator(runningValue, enumerator.Current.Value);
                iterationCount++;
            }

            var newValue = m_reducer(iterationCount, runningValue);
            var finalValue = m_interpolator(m_lastValue, newValue);
            m_lastValue = finalValue;
            return finalValue;
        }

        public void SendLatest(object key, T value)
        {
            m_latestValues[key] = value;
        }

        public void Clear(object key)
        {
            if (m_latestValues.ContainsKey(key))
                m_latestValues.Remove(key);
            else
                Debug.LogError($"this object key does not have a latest value");
        }

        public void ClearAll()
        {
            m_latestValues.Clear();
        }
    }

    [DisallowMultipleComponent]
    public class ReduceLatestOffsetProxy : MonoBehaviour
    {
        ReduceLatestProxy<Vector3> m_rotationLatestProxy;
        Dictionary<Func<float, Vector3>, int> m_rotationLatestSourcesHash = new Dictionary<Func<float, Vector3>, int>();
        List<Func<float, Vector3>> m_rotationLatestSources = new List<Func<float, Vector3>>();

        ReduceLatestProxy<Vector3> m_scaleLatestProxy;
        Dictionary<Func<float, Vector3>, int> m_scaleLatestSourcesHash = new Dictionary<Func<float, Vector3>, int>();
        List<Func<float, Vector3>> m_scaleLatestSources = new List<Func<float, Vector3>>();

        ReduceLatestProxy<Vector3> m_positionLatestProxy;
        Dictionary<Func<float, Vector3>, int> m_positionLatestSourcesHash = new Dictionary<Func<float, Vector3>, int>();
        List<Func<float, Vector3>> m_positionLatestSources = new List<Func<float, Vector3>>();

        Vector3 m_sourceLocalPosition;
        Vector3 m_sourceLocalEuler;
        Vector3 m_sourceLocalScale;

        private void Awake()
        {
            Func<Vector3, Vector3, Vector3> aggregator = (v0, v1) => v0 + v1;
            Func<int, Vector3, Vector3> reducer = (i, v) => v;
            Func<Vector3, Vector3, Vector3> interpolator = (vCurrent, vNext) => Vector3.Lerp(vCurrent, vNext, 7f * Time.deltaTime);
            Vector3 defaultValue = Vector3.zero;

            m_rotationLatestProxy = new ReduceLatestProxy<Vector3>(aggregator, reducer, interpolator, defaultValue);
            m_scaleLatestProxy = new ReduceLatestProxy<Vector3>(aggregator, reducer, interpolator, defaultValue);
            m_positionLatestProxy = new ReduceLatestProxy<Vector3>(aggregator, reducer, interpolator, defaultValue);

            m_sourceLocalPosition = this.transform.localPosition;
            m_sourceLocalEuler = this.transform.localEulerAngles;
            m_sourceLocalScale = this.transform.localScale;
        }

        private void Update()
        {
            var localPositionSource = Vector3.zero;
            for(int i =0; i < m_positionLatestSources.Count; i++)
            {
                localPositionSource += m_positionLatestSources[i](Time.deltaTime);
            }

            var localEulerSource = Vector3.zero;
            for (int i = 0; i < m_rotationLatestSources.Count; i++)
            {
                localEulerSource += m_rotationLatestSources[i](Time.deltaTime);
            }

            var localScaleSource = Vector3.zero;
            for (int i = 0; i < m_scaleLatestSources.Count; i++)
            {
                localScaleSource += m_scaleLatestSources[i](Time.deltaTime);
            }

            var localEulerOffset = m_rotationLatestProxy.Update() + localEulerSource + m_sourceLocalEuler;
            var localScaleOFfset = m_scaleLatestProxy.Update() + localScaleSource + m_sourceLocalScale;
            var localPositionOffset = m_positionLatestProxy.Update() + localPositionSource + m_sourceLocalPosition;

            this.transform.localEulerAngles = localEulerOffset;
            this.transform.localScale = localScaleOFfset;
            this.transform.localPosition = localPositionOffset;
        }

        public void SendLatestLocalPostionValue(object key, Vector3 localPostion)
        {
            m_positionLatestProxy.SendLatest(key, localPostion);
        }

        public void PushLocalPositionSource(Func<float, Vector3> localPositionSource)
        {
            m_positionLatestSources.Add(localPositionSource);
            m_positionLatestSourcesHash.Add(localPositionSource, m_positionLatestSources.Count);
        }

        public bool TryRemoveLocalPositionSource(Func<float, Vector3> localPositionSource)
        {
            var contains = m_positionLatestSourcesHash.ContainsKey(localPositionSource);
            if (!contains) return false;


            m_positionLatestSources.RemoveAt(m_positionLatestSourcesHash[localPositionSource]);
            m_positionLatestSourcesHash.Remove(localPositionSource);

            return true;
        }



        public void SendLatestLocalEulerValue(object key, Vector3 localEuler)
        {
            m_rotationLatestProxy.SendLatest(key, localEuler);
        }

        public void PushLocalEulerSource(Func<float, Vector3> localEulerSource)
        {
            m_rotationLatestSources.Add(localEulerSource);
            m_rotationLatestSourcesHash.Add(localEulerSource, m_rotationLatestSources.Count);
        }

        public bool TryRemoveLocalEulerSource(Func<float, Vector3> localEulerSource)
        {
            var contains = m_rotationLatestSourcesHash.ContainsKey(localEulerSource);
            if (!contains) return false;


            m_rotationLatestSources.RemoveAt(m_rotationLatestSourcesHash[localEulerSource]);
            m_rotationLatestSourcesHash.Remove(localEulerSource);

            return true;
        }



        public void PushLocalScaleValue(object key, Vector3 localScale)
        {
            m_scaleLatestProxy.SendLatest(key, localScale);
        }

        public void PushLocalScaleSource(Func<float, Vector3> localScaleSource)
        {
            m_scaleLatestSources.Add(localScaleSource);
            m_scaleLatestSourcesHash.Add(localScaleSource, m_scaleLatestSources.Count);
        }

        public bool TryRemoveLocalScaleSource(Func<float, Vector3> localScaleSource)
        {
            var contains = m_scaleLatestSourcesHash.ContainsKey(localScaleSource);
            if (!contains) return false;


            m_scaleLatestSources.RemoveAt(m_scaleLatestSourcesHash[localScaleSource]);
            m_scaleLatestSourcesHash.Remove(localScaleSource);

            return true;
        }
    }
}

