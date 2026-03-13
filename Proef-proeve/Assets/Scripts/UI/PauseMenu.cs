using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static event Action OnPauseStarted;
    public static event Action OnPauseEnded;
    [SerializeField] GameObject _pauzeMenu;
    [SerializeField] GameObject _playerInput;

    bool isPaused;

    private void Start()
    {
        _pauzeMenu.SetActive(false);
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
        _pauzeMenu.SetActive(true);
        _playerInput.SetActive(false);
        //Time.timeScale = 0f;
        isPaused = true;
        OnPauseEnded?.Invoke();
    }

    private void ResumeGame()
    {
        _pauzeMenu.SetActive(false);
        _playerInput.SetActive(true);
        //Time.timeScale = 1f;
        isPaused = false;
        OnPauseEnded?.Invoke();
    }
}
