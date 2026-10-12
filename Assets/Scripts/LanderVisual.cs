using System;
using Unity.Mathematics;
using UnityEngine;

public class LanderVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterParticleSystem;
    [SerializeField] private ParticleSystem middleThrusterParticleSystem;
    [SerializeField] private ParticleSystem rightThrusterParticleSystem;
    [SerializeField] private GameObject explosionVisualVFX;

    private Lander lander;
    private void Awake()
    {
        lander = GetComponent<Lander>();

        lander.OnBeforeForce += Lander_OnBeforeForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnUpForce += Lander_OnUpForce;

        SetThrusterParticleSystem(leftThrusterParticleSystem, false);
        SetThrusterParticleSystem(rightThrusterParticleSystem, false);
        SetThrusterParticleSystem(middleThrusterParticleSystem, false);
    }

    private void Start()
    {
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.TooFastSpeed:
            case Lander.LandingType.TooSteepAngle:
            case Lander.LandingType.WrongLandingArea:
                Instantiate(explosionVisualVFX, transform.position, quaternion.identity);
                gameObject.SetActive(false);
                break;
        }
    }

    private void Lander_OnBeforeForce(object sender, EventArgs e)
    {
        SetThrusterParticleSystem(leftThrusterParticleSystem, false);
        SetThrusterParticleSystem(rightThrusterParticleSystem, false);
        SetThrusterParticleSystem(middleThrusterParticleSystem, false);
    }
    private void Lander_OnUpForce(object sender, EventArgs e)
    {
        SetThrusterParticleSystem(leftThrusterParticleSystem, true);
        SetThrusterParticleSystem(rightThrusterParticleSystem, true);
        SetThrusterParticleSystem(middleThrusterParticleSystem, true);
    }
    private void Lander_OnRightForce(object sender, EventArgs e)
    {
        SetThrusterParticleSystem(leftThrusterParticleSystem, true);
    }
    private void Lander_OnLeftForce(object sender, EventArgs e)
    {
        SetThrusterParticleSystem(rightThrusterParticleSystem, true);
    }
    private void SetThrusterParticleSystem(ParticleSystem thruster, bool boolean)
    {
        ParticleSystem.EmissionModule emissionModule = thruster.emission;
        emissionModule.enabled = boolean;
    }
}
