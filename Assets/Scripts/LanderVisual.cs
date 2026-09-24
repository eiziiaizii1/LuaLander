using System;
using UnityEngine;

public class LanderVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterParticleSystem;
    [SerializeField] private ParticleSystem middleThrusterParticleSystem;
    [SerializeField] private ParticleSystem rightThrusterParticleSystem;

    private Lander lander;
    private void Awake()
    {
        lander = GetComponent<Lander>();
    }

    private void Start()
    {
        lander.OnBeforeForce += Lander_OnBeforeForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnUpForce += Lander_OnUpForce;
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
