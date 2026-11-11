using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private GameObject arrowLeft;
    [SerializeField] private GameObject arrowRight;
    [SerializeField] private GameObject arrowUp;
    [SerializeField] private GameObject arrowDown;
    [SerializeField] private Image fuelBarImage;
    void Update()
    {
        UpdateUI();
        
    }

    private void UpdateUI()
    {
        
        arrowLeft.SetActive(Lander.Instance.GetVelocityX() < 0f);
        arrowDown.SetActive(Lander.Instance.GetVelocityY() < 0f);
        arrowRight.SetActive(Lander.Instance.GetVelocityX() >= 0f);
        arrowUp.SetActive(Lander.Instance.GetVelocityY() >= 0f);

        statsText.text =
        GameManager.Instance.GetLevelNumber() + "\n" 
        + GameManager.Instance.GetScore() + "\n"
        + Math.Round(GameManager.Instance.GetTime()) + "\n"
        + Math.Round(Math.Abs(Lander.Instance.GetVelocityX() * 10f)) + "\n"
        + Math.Round(Math.Abs(Lander.Instance.GetVelocityY() * 10f)) + "\n";
        
        fuelBarImage.fillAmount =  Lander.Instance.GetFuelNormalized();

    }
}
