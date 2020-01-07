using UnityEngine;
using UnityEngine.SceneManagement;
using SUHScripts;

public class M_SceneManager : MonoBehaviour
{
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void CloseApplication()
    {
        Application.Quit();
    }

    public void OnDisable()
    {
        PrefabPoolingSystem.Reset();
    }
}
