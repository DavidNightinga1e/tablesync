using System;
using System.Collections;
using UnityEngine;

namespace TableSync
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemBulletController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem trailParticleSystem;
        [SerializeField] private ParticleSystem burstParticleSystem;

        public void Trail()
        {
            trailParticleSystem.Play(false);
        }
        
        public void Cutoff()
        {
            StartCoroutine(CutoffEnumerator());
        }

        private IEnumerator CutoffEnumerator()
        {
            trailParticleSystem.transform.SetParent(null, true);
            trailParticleSystem.transform.localScale = Vector3.one;
            var particleSystemEmission = trailParticleSystem.emission;
            particleSystemEmission.rateOverTime = 0;
            burstParticleSystem.Play();

            while (trailParticleSystem.particleCount > 0 || burstParticleSystem.particleCount > 0)
                yield return null;

            Destroy(gameObject);
        }
    }
}