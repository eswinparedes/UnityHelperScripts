using UnityEngine;

public abstract class SO_A_Vector3ReadWrite : SO_A_Vector3
{
    public virtual void SetValue(SO_A_Vector3 value)
    {
        Value = value.Value;
    }

    public virtual void SetValue(Vector3 value)
    {
        Value = value;
    }
}