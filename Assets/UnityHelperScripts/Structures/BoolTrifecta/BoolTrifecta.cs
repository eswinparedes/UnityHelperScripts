using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BoolTrifecta
{
    public BoolTrifecta(bool isTrue, bool isTrueThisFrame, bool isTrueStay, bool isfalseThisFrame)
    {
        IsTrue = isTrue;
        IsTrueThisFrame = isTrueThisFrame;
        IsTrueStay = isTrueStay;
        IsFalseThisFrame = isfalseThisFrame;
    }

    public bool IsTrue { get; private set; }
    public bool IsTrueThisFrame { get; private set; }
    public bool IsTrueStay { get; private set; }
    public bool IsFalseThisFrame { get; private set; }

    public (bool isTrue, bool isTrueThisFrame, bool isTrueStay, bool isFalseThisFrame) Deconstruct() =>
        (IsTrue, IsTrueThisFrame, IsTrueStay, IsFalseThisFrame);

    public override string ToString() =>
        $"IsTrue: {IsTrue}  ThisFrame: {IsTrueThisFrame}  Stay: {IsTrueStay}  Released {IsFalseThisFrame}";

}