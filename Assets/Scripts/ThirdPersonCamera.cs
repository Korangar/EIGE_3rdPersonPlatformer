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
    public Vector3 camOffest;
    public float sensetivityH = 10, sensetivityV = 10;
    public float maxLookDown = 30, maxLookUp = -30;
    public float catchupDistance = 100;

    private Vector3 focus;
    private Vector3 lastUpdate;
    private Vector3 camVelocity = Vector3.zero;
    private Vector3 desiredCamPosition;

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

    void Start()
    {
        focus = target.position + focusOffset;
        desiredCamPosition = focus + camOffest;
    }

    // Update is called once per frame
	void Update () {
        MyInput input = CamInput;
        
        float moveSinceLastUpdate = Vector3.Distance(lastUpdate, target.position);
        if (moveSinceLastUpdate > 0)
        {
            focus = target.position + focusOffset;
            desiredCamPosition = focus + camOffest;
        }

        // teleport to target if too far away
        float distanceFromTarget = Vector3.Distance(transform.position, focus);
        if(distanceFromTarget > catchupDistance)
        {
            transform.position = desiredCamPosition;
            //transform.LookAt(focus);
        }

        // rotate horz
        if (Mathf.Abs(input.horizontal) > 0.2)
        {
            float horzLookChange = input.horizontal * sensetivityH * Time.deltaTime;
            transform.RotateAround(focus, Vector3.up, horzLookChange);
            desiredCamPosition = transform.forward * camOffest.z + transform.right * camOffest.x;
        }
        // rotate vert
        if(Mathf.Abs(input.vertical) > 0.2)
        {
            float vertLookChange = input.vertical * sensetivityV * Time.deltaTime;
            transform.RotateAround(focus, transform.right, vertLookChange);
            desiredCamPosition = transform.forward * camOffest.z + transform.right * camOffest.x;
        }

        transform.position = Vector3.SmoothDamp(
            transform.position, 
            desiredCamPosition, 
            ref camVelocity, 
            1, 
            Mathf.Max(sensetivityH, sensetivityV));

        transform.LookAt(focus);


        // save last position
        lastUpdate = target.position;
    }

    void OnValidate()
    {
        focus = target.position + focusOffset;
        desiredCamPosition = focus + camOffest;
    }
}
