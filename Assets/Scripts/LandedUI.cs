using System;
using TMPro;
using UnityEngine;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI statsText;

    private void Start()
    {
        Lander.Instance.OnLanded += Lander_OnLanded;

        Hide();
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        if (e.landingType == Lander.LandingType.Success)
        {
            titleText.text = "<color=#00ff00>SUCCESSFUL LANDING!</color>";
        }
        else
        {
            titleText.text = "<color=#ff0000>CRASHH!!!</color>";
        }

        statsText.text =
        Math.Round(e.speed * 3) + "\n" +
        Math.Round(e.angle * 100) + "\n" +
        e.multiplier + "\n" +
        e.point + "\n";

        Show();
    }

    private void Show()
    {
       gameObject.SetActive(true); 
    }

    private void Hide()
    {
       gameObject.SetActive(false); 
    }
}
