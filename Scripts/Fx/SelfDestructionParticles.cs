using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructionParticles : MonoBehaviour
{

    private ParticleSystem particles;
    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }
    
    void Update()
    {
        if (!particles.IsAlive())
        {
            Destroy(this.gameObject);
        }
        
    }
}
