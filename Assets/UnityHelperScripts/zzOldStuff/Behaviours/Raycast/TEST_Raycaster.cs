using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Raycaster : MonoBehaviour
{
    public void OnRaycastHit(RaycastOutcome outcome) =>
        Debug.Log($"Hit: {outcome.outcomeRaycastHit.collider.gameObject}");

    public void OnRaycastMiss() =>
        Debug.Log("Raycast Miss");

    public void RaycastAny() =>
        Debug.Log("Raycast Any");
}
