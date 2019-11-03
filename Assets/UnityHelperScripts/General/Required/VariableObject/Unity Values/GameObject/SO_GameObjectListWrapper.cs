using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Unity Values/GameObject/GameObject List Wrapper")]
public class SO_GameObjectListWrapper : ScriptableObject
{
    [HideInInspector]
    public SO_GameObjectList gameObjectListObject = default;
}

[System.Serializable]
public class CL_GameObjectListWrapperReference
{
    [SerializeField]
    SO_GameObjectListWrapper gameObjectWrapper = default;

    public SO_GameObjectList GameObjects
    {
        get { return gameObjectWrapper.gameObjectListObject; }
    }
}
