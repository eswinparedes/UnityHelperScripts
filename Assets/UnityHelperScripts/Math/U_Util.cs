using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class U_Util
{
    public static bool IsWithinRange(Vector3 EventPosition, GameObject Receiver, float Range)
    {
        return (EventPosition - Receiver.transform.position).magnitude <= Range;
    }

    public static RaycastHit GetNearestRaycastHit(IEnumerable<RaycastHit> hits)
    {
        return hits.OrderBy(hit => hit.distance).First();
    }

    public static Transform GetClosestGameObject(IEnumerable<Transform> transforms, Transform transformToCompareTo)
    {
        return transforms.OrderBy(go => (go.position - transformToCompareTo.position).sqrMagnitude).First();
    }

    public static void TransformConstrainedLookAt(Transform trans, Vector3 position, Vector3 constraints)
    {
        Vector3 pos = trans.position;
        pos = Vector3.Scale(pos, Vector3.one - constraints);
        position = Vector3.Scale(position, constraints);
        Vector3 lookPos = pos + position;
        trans.LookAt(lookPos, trans.up);
    }

    public static Quaternion LookToMouseRotation_OnX(Transform _transform)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(_transform.position);

        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = -Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg + Camera.main.transform.rotation.eulerAngles.z;

        return Quaternion.Euler(new Vector3(angle, _transform.rotation.x, _transform.rotation.y));
    }

    public static int CycleNext(int currentIndex, int dir, int Length, bool loop = true)
    {
        if(dir > 0)
        {
            if(currentIndex + 1 >= Length)
            {
                return loop ? 0 : -1;
            }
            else
            {
                return currentIndex + 1;
            }
        }
        else
        {

            if(currentIndex - 1 < 0)
            {
                return loop ? Length - 1 : -1;
            }
            else
            {
                return currentIndex - 1;
            }
        }
    }


}
