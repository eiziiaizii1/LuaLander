using System;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.UnPauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
    }
    private void Start()
    {
        GameManager.Instance.onGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.onGameUnPaused += GameManager_OnGameUnPaused;

        

        Hide();
    }

    private void GameManager_OnGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        resumeButton.Select();
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
