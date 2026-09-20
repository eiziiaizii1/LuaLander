using UnityEngine;
using TMPro;

public class LandingPadVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro multiplierText;

    private void Awake()
    {
        LandingPad landingPad = GetComponent<LandingPad>();
        multiplierText.text = landingPad.getScoreMultiplier() + "x";
    }
}
