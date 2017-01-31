using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour {

    private bool isActive;

    public bool IsActive
    {
        get { return isActive; }
        set {
            isActive = value;

            Collider col = GetComponent<Collider>();
            if (col) col.enabled = value;

            MeshRenderer rndr = GetComponentInChildren<MeshRenderer>();
            if (rndr) rndr.enabled = value;

            Light light = GetComponentInChildren<Light>();
            if (light) light.enabled = value;

            ParticleSystem prtcl = GetComponentInChildren<ParticleSystem>();
            if (prtcl)
            {
                if (value) prtcl.Play();
                else prtcl.Stop();
            }

        }
    }

    void Awake()
    {
        IsActive = true;
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
        {
            AudioSource audio = GetComponent<AudioSource>();
            if (audio)
            {
                audio.Play();
            }

            IsActive = false;

            ParticleSystem prtcl = GetComponentInChildren<ParticleSystem>();
            StartCoroutine(R_PlayerShine(c.transform, prtcl, 10));
        }
    }

    IEnumerator R_PlayerShine(Transform player, ParticleSystem ps, float duration)
    {
        const int particles = 25;
        const float tick = 0.1f;
        float startTime = Time.time;

        ParticleSystem.EmitParams param = new ParticleSystem.EmitParams();
        param.applyShapeToPosition = true;

        for(float t = 0; t < duration; t = Time.time - startTime)
        {
            param.position = player.position;
            ps.Emit(param, (int) Mathf.Max(particles * (1 - t/duration) * tick, 1));

            yield return new WaitForSeconds(tick);
        }
    }
}

