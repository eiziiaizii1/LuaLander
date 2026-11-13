using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    private static int totalScore = 0;
    private int score;
    private float time;
    private bool isTimerActvie;

    [SerializeField] private static int levelNumber = 1;
    [SerializeField] private List<GameLevel> gameLevelList;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    public event EventHandler onGamePaused;
    public event EventHandler onGameUnPaused;

    public static void ResetStaticData()
    {
        levelNumber = 1;
        totalScore = 0;
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;

        GameInput.Instance.onMenuButtonPressed += GameInput_OnMenuButtonPressed;

        LoadCurrentLevel();
    }

    private void GameInput_OnMenuButtonPressed(object sender, EventArgs e)
    {
        PauseUnpauseGame();
    }

    private void PauseUnpauseGame()
    {
        if (Time.timeScale == 0)
        {
            UnPauseGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void LoadCurrentLevel()
    {
        GameLevel gameLevel = GetGameLevel();
        GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero,Quaternion.identity);
        Lander.Instance.transform.position = spawnedGameLevel.GetStartPosition();
        cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartTargetTransform();
        CinemachineCameraZoom2D.Instance.SetTargetOrthographicsSize(spawnedGameLevel.GetZoomedOutOrthographicSize());

    }

    private GameLevel GetGameLevel()
    {
        foreach (GameLevel gameLvl in gameLevelList)
        {
            if (gameLvl.GetLevelNumber() == levelNumber)
            {
                return gameLvl;
            }
        }
        return null;
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isTimerActvie = e.state == Lander.State.Normal;
        
        if(e.state == Lander.State.Normal)
        {
            cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            CinemachineCameraZoom2D.Instance.SetNormalOrthographicsSize();
        }
    }

    private void Update()
    {
        if (isTimerActvie)
        {
            time += Time.deltaTime; 
        }
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.point);
    }

    private void Lander_OnCoinPickup(object sender, EventArgs e)
    {
        AddScore(500);
    }

    private void AddScore(int point)
    {
        score += point;
        Debug.Log("score ==>"+ score);
    }
    public int GetScore()
    {
        return score;
    }
    
    public float GetTime()
    {
        return time;
    }

    public void GoToNextLevel()
    {
        levelNumber++;
        totalScore += score;
        if (GetGameLevel()== null)
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        }
        else
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        }
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public void RetryLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }
    public int GetLevelNumber()
    {
        return levelNumber;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        onGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        onGameUnPaused?.Invoke(this, EventArgs.Empty);
    } 
}