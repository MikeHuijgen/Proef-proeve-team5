using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int _sceneNubmer;
    public void loadscene()
    {
        SceneManager.LoadScene(_sceneNubmer);
    }

    public void QuitScene()
    {
        Application.Quit();
    }
}