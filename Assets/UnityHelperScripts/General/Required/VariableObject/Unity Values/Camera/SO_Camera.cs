using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Unity Values/Camera/Camera")]
public class SO_Camera : ScriptableObject{

    [HideInInspector] public Camera cameraReference;
}