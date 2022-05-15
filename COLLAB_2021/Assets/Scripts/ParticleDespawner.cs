using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDespawner : MonoBehaviour
{

    [SerializeField] ParticleSystem particle;
    [SerializeField] float destroyTimer;

    private void OnEnable()
    {
        particle = GetComponent<ParticleSystem>();
        destroyTimer = particle.main.duration;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(destroyTimer + 0.5f);

        Destroy(gameObject);
    }

}
