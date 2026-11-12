using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    private int score;
    private float time;
    private bool isTimerActvie;

    [SerializeField] private static int levelNumber = 1;
    [SerializeField] private List<GameLevel> gameLevelList;
    [SerializeField] private CinemachineCamera cinemachineCamera;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;

        LoadCurrentLevel();
    }

    private void LoadCurrentLevel()
    {
        foreach (GameLevel gameLvl in gameLevelList)
        {
            if (gameLvl.GetLevelNumber() == levelNumber)
            {
                GameLevel spawnedGameLevel = Instantiate(gameLvl,Vector3.zero, Quaternion.identity);
                Lander.Instance.transform.position = spawnedGameLevel.GetStartPosition();
                cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartTargetTransform();
                CinemachineCameraZoom2D.Instance.SetTargetOrthographicsSize(spawnedGameLevel.GetZoomedOutOrthographicSize());
            }
        }
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
        SceneManager.LoadScene(0);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(0);
    }
    public int GetLevelNumber()
    {
        return levelNumber;
    }
}