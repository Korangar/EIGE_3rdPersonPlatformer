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
    private Vector3 targetRelativeOffset;

    #region debug
    Vector3 rotatedVector;
    #endregion

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

    private bool TargetReport
    {
        get
        {
            Vector3 focus = target.position + focusOffset;

            Vector3 targetToCamHorizontalVector = transform.position - focus;
            targetToCamHorizontalVector.y = 0;

            Vector3 moveSinceLastUpdate = target.position - lastUpdate;
            return false;
        }
    }

    void Start()
    {
        focus = target.position + focusOffset;
        targetRelativeOffset = target.TransformVector(camOffest);
        lastUpdate = target.position;

        #region debug
        rotatedVector = Vector3.right;
        #endregion
    }

    // Update is called once per frame
    void Update () {
        // get user input
        MyInput input = CamInput;

        float moveSinceLastUpdate = Vector3.Distance(lastUpdate, target.position);
        if (moveSinceLastUpdate > 0.0f)
        #region UpdateCamFocus
        {
            focus = target.position + focusOffset;
            targetRelativeOffset = target.TransformVector(camOffest);
            lastUpdate = target.position;
        }
        #endregion

        #region UserRotatesCamera
        {
            if (horizontal.enabled)
            {
                // rotate transform horizontally
                float lookChange = input.rotation.x * Time.deltaTime;
                //transform.RotateAround(focus, Vector3.up, lookChange);
                targetRelativeOffset = Quaternion.AngleAxis(lookChange, Vector3.up) * targetRelativeOffset;
            }

            if (vertical.enabled)
            {
                // rotate transform vertically
                float lookChange = input.rotation.y * Time.deltaTime;
                //transform.RotateAround(focus, transform.right, lookChange);
                targetRelativeOffset = Quaternion.AngleAxis(lookChange, transform.right) * targetRelativeOffset;
            }
            Debug.DrawRay(focus, targetRelativeOffset, Color.cyan);
        }
        #endregion

        // set the desired cam position
        Vector3 desiredCamPosition = focus + targetRelativeOffset;
        
        float distanceFromTarget = Vector3.Distance(transform.position, focus);
        if(distanceFromTarget > catchupDistance)
        {
            // teleport to target if too far away
            transform.position = desiredCamPosition;
        }
        else
        #region MoveCamera
        {

            // move to the desired position
            transform.position = Vector3.SmoothDamp(
                transform.position, 
                desiredCamPosition, 
                ref camVelocity, 
                camSmoothTime, 
                camSpeed);

            Debug.DrawRay(transform.position, camVelocity, Color.red);
        }
        #endregion

        // allways point the camera on the target
        transform.LookAt(focus);
    }

    void OnValidate()
    {
        focus = target.position + focusOffset;
    }
}
