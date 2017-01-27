using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour {

	void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
        {
            StartCoroutine(R_AnyKey());
            c.GetComponent<PlayerController>().enabled = false;
            Destroy(c.GetComponent<ConstantForce>(), 0.1f);
            Destroy(c.GetComponent<Rigidbody>(), 0.1f);
        }
    }

    IEnumerator R_AnyKey()
    {
        yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
        Debug.Log("LoadMenu");
    }
}
