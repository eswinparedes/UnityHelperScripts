using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Path : MonoBehaviour
{ 
    [SerializeField] M_PathPoint[] m_pathPoints = new M_PathPoint[1];

    public M_PathPoint[] PathPoints { get => m_pathPoints; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(m_pathPoints.Length < 2)
        {
            return;
        }

        for(int i = 1; i < m_pathPoints.Length; i++)
        {
            if(m_pathPoints[i] == null)
            {
                continue;
            }
            if(i == m_pathPoints.Length - 1)
            {    
                if(m_pathPoints[0] != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(m_pathPoints[i].TransformPoint.position, m_pathPoints[0].TransformPoint.position);
                    Gizmos.color = Color.blue;
                }
            }
            Gizmos.DrawLine(m_pathPoints[i - 1].TransformPoint.position, m_pathPoints[i].TransformPoint.position);
        }
    }
}
