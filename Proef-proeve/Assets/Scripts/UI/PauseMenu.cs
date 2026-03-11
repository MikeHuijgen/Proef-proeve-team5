using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject PauzeMenu;

    bool isPaused;

    private void Start()
    {
        PauzeMenu.SetActive(false);
    }

    public void IsPaused()
    {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        
    }

    private void PauseGame()
    {
        PauzeMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void ResumeGame()
    {
        PauzeMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
