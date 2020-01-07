using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SUHScripts
{
    [System.Serializable]
    public class EventCycler 
    {
        [Header("Cycle Events")]
        [SerializeField] List<UnityEvent> m_events = new List<UnityEvent>();
        [Header("Cycle Properties")]
        [SerializeField] int m_startIndex = 0;
        [SerializeField] [Range(-1, 1)] int m_dir = 1;
        [SerializeField] bool m_doesCycleAround = true;

        IndexCycle indexTravler;

        public void Start()
        {
            indexTravler = new IndexCycle(m_startIndex, m_dir);
        }

        public void CycleNext()
        {
            indexTravler = indexTravler.WithDirection(m_dir);

            indexTravler = 
                indexTravler
                .CycleNext(m_events.Count, m_doesCycleAround);

            m_dir = indexTravler.Direction;

            m_events[indexTravler.Index].Invoke();
        }
    }

}
