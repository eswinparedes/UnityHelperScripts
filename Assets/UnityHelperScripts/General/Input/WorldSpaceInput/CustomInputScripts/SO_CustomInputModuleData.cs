using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[CreateAssetMenu(menuName = "Input/Custom Input Module Data")]
public class SO_CustomInputModuleData : A_CustomInputModuleSettings
{
    public bool ResultIsValid { get; private set; }
    public Vector3 ResultPoint { get; private set; }
    public override bool PressedCondition { get; set; }
    public override bool ReleasedCondition { get; set; }

    public override bool SubmitCondition { get; set; }
    public override bool CancelCondition { get; set; }

    public override float HorizontalAxis { get; set; }
    public override float VerticalAxis { get; set; }
    public override bool HorizontalAxisThisFrame { get; set; }
    public override bool VerticalAxisThisFrame { get; set; }

    public override Ray InputRay { get; set; }
    public override List<RaycastResult> InputRaycastResult { get; set; }

    /* (= (result) =>
    {
        ResultIsValid = result.isValid;
        ResultPoint = result.worldPosition;
        return result;
    }
    */
}
