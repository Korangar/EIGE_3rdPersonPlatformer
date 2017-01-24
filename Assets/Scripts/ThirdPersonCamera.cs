using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ThirdPersonCamera : MonoBehaviour {

    public struct MyInput
    {
        public Vector2 raw;
        public Vector2 rotation;
    }
    [System.Serializable]
    public struct RotationAxis
    {
        public string name;
        public bool enabled, inverted;
        public float sensetivity;
        public float AxisInput
        {
            get {
                if (enabled)
                {
                    return inverted ? -Input.GetAxis(name) : Input.GetAxis(name);

                }
                else
                {
                    return 0;
                }
            }
        }
    }

    public Transform target;
    public Vector3 focusOffset;
    public Vector3 camOffest;
    public float camSpeed = 100;
    public float camSmoothTime = 1;
    public float catchupDistance = 100;
    public RotationAxis horizontal, vertical;

    private Vector3 focus;
    private Vector3 lastUpdate;
    private Vector3 camVelocity = Vector3.zero;
    private Vector3 desiredCamPosition;

    private MyInput CamInput
    {
        get
        {
            MyInput input = new MyInput();
            float rawX = horizontal.AxisInput;
            float rawY = vertical.AxisInput;
            input.raw = new Vector2(rawX, rawY);
            input.rotation = new Vector2(rawX * horizontal.sensetivity, rawY * vertical.sensetivity);
            return input;
        }
    }

    void Start()
    {
        focus = target.position + focusOffset;
        Vector3 targetRelativeOffset = target.TransformVector(camOffest);
        desiredCamPosition = focus + targetRelativeOffset;
        Debug.DrawRay(focus, camOffest);
        Debug.DrawRay(focus, targetRelativeOffset);
        lastUpdate = target.position;
    }

    // Update is called once per frame
	void Update () {
        // get user input
        MyInput input = CamInput;
        
        float moveSinceLastUpdate = Vector3.Distance(lastUpdate, target.position);
        if (moveSinceLastUpdate > 0)
        {
            lastUpdate = target.position;
            focus = target.position + focusOffset;
            // desired cam position is behind target.
            desiredCamPosition = focus + target.TransformVector(camOffest);
        }


        // teleport to target if too far away
        float distanceFromTarget = Vector3.Distance(transform.position, focus);
        if(distanceFromTarget > catchupDistance)
        {
            transform.position = desiredCamPosition;
            //transform.LookAt(focus);
        }
        
        #region UserRotatesCamera
        {
            bool userChangedRotation = false;
            if (horizontal.enabled && Mathf.Abs(input.raw.x) > 0.15f)
            {
                // rotate transform horizontally
                float horzLookChange = input.rotation.x * Time.deltaTime;
                transform.RotateAround(focus, Vector3.up, horzLookChange);
                userChangedRotation = true;
            }

            if (vertical.enabled && Mathf.Abs(input.raw.y) > 0.3f)
            {
                // rotate transform vertically
                float vertLookChange = input.rotation.y * Time.deltaTime;
                transform.RotateAround(focus, transform.right, vertLookChange);
                userChangedRotation = true;
            }

            if (userChangedRotation)
            {
                // update desired cam position with user preference in mind
            //    desiredCamPosition = target.forward * camOffest.z + target.right * camOffest.y * target.up;
            }
        }
        #endregion
        
        Debug.DrawLine(focus, desiredCamPosition, Color.cyan);
        // allways keep moving to the desired position
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            desiredCamPosition, 
            ref camVelocity, 
            camSmoothTime, 
            camSpeed);

        Debug.DrawRay(transform.position, camVelocity, Color.red);

        // allways point the camera on the target
        transform.LookAt(focus);
    }

    void OnValidate()
    {
        focus = target.position + focusOffset;
        desiredCamPosition = focus + camOffest;
    }
}
