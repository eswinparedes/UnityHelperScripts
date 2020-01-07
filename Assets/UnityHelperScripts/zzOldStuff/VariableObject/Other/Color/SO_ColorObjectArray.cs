using UnityEngine;

//TODO: Rename to SO_ColorObjectArray
[CreateAssetMenu(menuName = "Variables/Other/Color/SO_Color Array")]
public class SO_ColorObjectArray : ScriptableObject {

    public SO_A_Color[] colorObjects;

    public Color this[int index]
    {
        get => colorObjects[index].color;
        set => colorObjects[index].color = value;
    }
}