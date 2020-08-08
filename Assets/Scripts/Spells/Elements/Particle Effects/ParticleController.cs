using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{

    private ParticleSystem arcanaEffectParticles;
    private ParticleSystem arcanaExplosionParticles;
    private ParticleSystem fireEffectParticles;
    private ParticleSystem fireExplosionParticles;
    private ParticleSystem corruptionEffectParticles;
    private ParticleSystem corruptionExplosionParticles;
    private ParticleSystem frostEffectParticles;
    private ParticleSystem frostExplosionParticles;

    void Awake()
    {
        arcanaEffectParticles = transform.Find("Arcana Effect Particles").gameObject.GetComponent<ParticleSystem>();
        arcanaExplosionParticles = transform.Find("Arcana Explosion Particles").gameObject.GetComponent<ParticleSystem>();
        fireEffectParticles = transform.Find("Fire Effect Particles").gameObject.GetComponent<ParticleSystem>();
        fireExplosionParticles = transform.Find("Fire Explosion Particles").gameObject.GetComponent<ParticleSystem>();
        corruptionEffectParticles = transform.Find("Corruption Effect Particles").gameObject.GetComponent<ParticleSystem>();
        corruptionExplosionParticles = transform.Find("Corruption Explosion Particles").gameObject.GetComponent<ParticleSystem>();
        frostEffectParticles = transform.Find("Frost Effect Particles").gameObject.GetComponent<ParticleSystem>();
        frostExplosionParticles = transform.Find("Frost Explosion Particles").gameObject.GetComponent<ParticleSystem>();
    }

    public void PlayEffect(ElementType elementType) {
        switch (elementType)
        {
            case ElementType.ARCANA:
                arcanaEffectParticles.Play();
                break;
            case ElementType.FIRE:
                fireEffectParticles.Play();
                break;
            case ElementType.CORRUPTION:
                corruptionEffectParticles.Play();
                break;
            case ElementType.FROST:
                frostEffectParticles.Play();
                break;
            default:
                Debug.Log("Unknown element: " + elementType);
                break;
        }
    }

    public void PlayExplosion(ElementType elementType) {
        switch (elementType)
        {
            case ElementType.ARCANA:
                arcanaExplosionParticles.Play();
                break;
            case ElementType.FIRE:
                fireExplosionParticles.Play();
                break;
            case ElementType.CORRUPTION:
                corruptionExplosionParticles.Play();
                break;
            case ElementType.FROST:
                frostExplosionParticles.Play();
                break;
            default:
                Debug.Log("Unknown element: " + elementType);
                break;
        }
    }

    public void StopEffect(ElementType elementType) {
        switch (elementType)
        {
            case ElementType.ARCANA:
                arcanaEffectParticles.Stop();
                break;
            case ElementType.FIRE:
                fireEffectParticles.Stop();
                break;
            case ElementType.CORRUPTION:
                corruptionEffectParticles.Stop();
                break;
            case ElementType.FROST:
                frostEffectParticles.Stop();
                break;
            default:
                Debug.Log("Unknown element: " + elementType);
                break;
        }
    }
}
