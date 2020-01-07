using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Other/String/String")]
public class SO_String : SO_A_StringReadWrite {

    [SerializeField] string m_value;

    public override string Value { get => m_value; set => m_value = value;  }
}