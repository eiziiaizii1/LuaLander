using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score;
    private void Start()
    {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
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
}