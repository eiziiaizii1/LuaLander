using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private TextMeshProUGUI nextButtonTextMesh;
    [SerializeField] private Button nextButton;

    private Action nextButtonClickAction;

    private void Awake()
    {
        nextButton.onClick.AddListener(() =>
        {
            nextButtonClickAction();
        });
    }

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
            nextButtonTextMesh.text = "CONTINUE";
            nextButtonClickAction = GameManager.Instance.GoToNextLevel;
        }
        else
        {
            titleText.text = "<color=#ff0000>CRASHH!!!</color>";
            nextButtonTextMesh.text = "RETRY";
            nextButtonClickAction = GameManager.Instance.RetryLevel;
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
        nextButton.Select();
       gameObject.SetActive(true); 
    }

    private void Hide()
    {
       gameObject.SetActive(false); 
    }
}
