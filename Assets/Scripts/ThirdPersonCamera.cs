using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public struct MyInput
    {
        public float horizontal, vertical;
    }

    public Transform target;
    public Vector3 focusOffset;
    public float desiredCamDistance = 2f;
    public float sensetivityH = 10, sensetivityV = 10;
    public float maxLookDown = 30, maxLookUp = -30;

    private Vector3 focus;
    private Vector3 lastUpdate;
    private Vector3 camVelocity = Vector3.zero;


    private MyInput CamInput
    {
        get
        {
            MyInput input = new MyInput();
            input.horizontal = Input.GetAxis("Horizontal_Look");
            input.vertical = Input.GetAxis("Vertical_Look");
            return input;
        }
    }
	// Update is called once per frame
	void Update () {
        MyInput input = CamInput;

        float moveSinceLastUpdate = Vector3.Distance(lastUpdate, target.position);
        if (moveSinceLastUpdate > 0)
        {
            focus = target.position + focusOffset;
        }
        float distanceFromTarget = Vector3.Distance(transform.position, focus);

        if(distanceFromTarget > 50)
        {
            transform.position += focus - target.forward * desiredCamDistance;
            transform.LookAt(focus);
        }

        transform.position = Vector3.SmoothDamp(transform.position, focus, ref camVelocity, 1, 20);
        transform.LookAt(focus);
        

        // rotate horz
        if (Mathf.Abs(input.horizontal) > 0.2)
        {
            transform.RotateAround(focus, Vector3.up, input.horizontal * sensetivityH * Time.deltaTime);
        }

        // rotate vert
        if(Mathf.Abs(input.vertical) > 0.2)
        {
            float vertLookChange = input.vertical * sensetivityV * Time.deltaTime;
            transform.RotateAround(focus, transform.right, vertLookChange);
        }

        // save last position
        lastUpdate = target.position;
    }
}
