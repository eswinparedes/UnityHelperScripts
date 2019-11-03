using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Unity Values/GameObject/GameObject List")]
public class SO_GameObjectList : ScriptableObject
{

    [SerializeField]
    private List<GameObject> gameObjects = new List<GameObject>();
    [SerializeField]
    bool b_isPrefabList = false;
    #region Properties
    public List<GameObject> GameObjects
    {
        get { return gameObjects; }
    }
    #endregion

    private void OnEnable()
    {
        if (!b_isPrefabList) GameObjects.Clear();
    }

    public void AddItem(GameObject item)
    {
        if (!gameObjects.Contains(item))
        {
            gameObjects.Add(item);
        }
    }

    public void RemoveItem(GameObject item)
    {
        if (gameObjects.Contains(item))
        {
            gameObjects.Remove(item);
        }
    }

    public GameObject PopRandomItem()
    {
        int r_index = Random.Range(0, gameObjects.Count);
        GameObject inst = gameObjects[r_index];
        RemoveItem(inst);
        return inst;
    }

    public GameObject GetRandomitem()
    {
        return gameObjects[Random.Range(0, gameObjects.Count)];
    }
}

[System.Serializable]
public class CL_L_GameObjectReference
{
    [SerializeField] SO_GameObjectList gameObjectReference = default;

    public SO_GameObjectList GameObjectReference
    {
        get { return gameObjectReference; }
    }
}