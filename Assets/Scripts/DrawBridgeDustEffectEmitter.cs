using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBridgeDustEffectEmitter : MonoBehaviour {

    ParticleSystem myParticleSystem;

	// Use this for initialization
	void Start () {
        myParticleSystem = GetComponent<ParticleSystem>();
	}
	
    void OnTriggerEnter()
    {
        myParticleSystem.Emit(20);
    }
}
