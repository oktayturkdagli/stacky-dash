using System;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] effects;

    private void Start()
    {
        EventManager.current.onLastDanceTriggered += OnLastDanceTriggered;
        EventManager.current.onFinishGame += OnFinishGame;
    }

    private void OnDestroy()
    {
        EventManager.current.onLastDanceTriggered -= OnLastDanceTriggered;
        EventManager.current.onFinishGame -= OnFinishGame;
    }

    void OnLastDanceTriggered()
    {
        PlayParticle(effects[0]);
        PlayParticle(effects[1]);
    }
    
    void OnFinishGame()
    {
        PlayParticle(effects[2]);
        PlayParticle(effects[3]);
    }
    
    void PlayParticle( ParticleSystem particle)
    {
        if (!particle)
            return;
        
        particle.Play();
    }
    
}
