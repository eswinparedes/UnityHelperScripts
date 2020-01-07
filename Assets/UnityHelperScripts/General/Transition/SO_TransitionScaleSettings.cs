using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    [CreateAssetMenu(menuName = "Experimental/Transition Scale Settings")]
    public class SO_TransitionScaleSettings : ScriptableObject
    {
        [SerializeField] Vector3 m_scaleEnter = Vector3.one;
        [SerializeField] Vector3 m_scaleExit = Vector3.zero;
        [SerializeField] AnimationCurve m_movementCurve = new AnimationCurve();
        [SerializeField] float m_transitionTime = 0.2f;

        public Vector3 ScaleEnter => m_scaleEnter;
        public Vector3 ScaleExit => m_scaleExit;
        public AnimationCurve MovementCurve => m_movementCurve;
        public float TransitionTime => m_transitionTime;
    }
}

