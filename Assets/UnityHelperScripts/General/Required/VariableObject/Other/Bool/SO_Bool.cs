using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Other/Bool/Bool")]
public class SO_Bool : SO_A_BoolReadWrite
{
    [SerializeField] bool m_isTrue;

    public override bool IsTrue
    {
        get => m_isTrue;
        set => m_isTrue = value;
    }
}