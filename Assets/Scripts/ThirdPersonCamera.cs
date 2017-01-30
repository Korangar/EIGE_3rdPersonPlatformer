using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Camera))]
public class ThirdPersonCamera : MonoBehaviour {

    #region Structs
    [System.Serializable]
    public struct RotationAxis
    {
        public string name;
        public bool enabled, inverted;
        public float sensetivity;
    }

    public struct MyInput
    {
        public Vector2 raw;
        public Vector2 rotation;
    }
    #endregion

    #region EditorVariables
    public Vector3 focusOffset;
    public Vector3 camOffest;
    public float camSpeed = 100;
    public float camSmoothTime = 1;
    public RotationAxis horizontal, vertical;
    #endregion

    private Vector3 camVelocity = Vector3.zero;
    private Vector3 targetRelativeOffset;
    private PlayerController target;

    private AudioSource audio;
    private bool isAudioPlaying;
    private NoiseAndGrain noise;

    #region Properties
    private Transform pTransform
    {
        get { return target.transform; }
    }

    private Vector3 focus
    {
        get { return pTransform.position + transform.TransformVector(focusOffset); }
    }
    #endregion

    void Start()
    {
        target = FindObjectOfType<PlayerController>();
        targetRelativeOffset = pTransform.TransformVector(camOffest);
        noise = GetComponent<NoiseAndGrain>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        // get user input
        MyInput input = GetLookInput();
        
        #region UserRotatesCamera
        {
            // rotate transform horizontally
            float lookChangeH = input.rotation.x * Time.deltaTime;
            //transform.RotateAround(focus, Vector3.up, lookChange);
            targetRelativeOffset = Quaternion.AngleAxis(lookChangeH, Vector3.up) * targetRelativeOffset;

            // rotate transform vertically
            float lookChangeV = input.rotation.y * Time.deltaTime;
            //transform.RotateAround(focus, transform.right, lookChange);
            targetRelativeOffset = Quaternion.AngleAxis(lookChangeV, transform.right) * targetRelativeOffset;
        }
        #endregion

        #region MoveCamera
        {
            // set the desired cam position
            Vector3 desiredCamPosition = focus + targetRelativeOffset;

            // move to the desired position
            transform.position = Vector3.MoveTowards(
                transform.position,
                desiredCamPosition,
                camSpeed * Time.deltaTime);

            //transform.position = Vector3.SmoothDamp(
            //    transform.position,
            //    desiredCamPosition,
            //    ref camVelocity,
            //    camSmoothTime,
            //    camSpeed * Time.deltaTime);
            
            // allways point the camera on the target
            transform.LookAt(focus);
        }
        #endregion

        #region noise
        {
            float tc = Input.GetAxis("TimeControl");
            noise.enabled = tc != 0;
            noise.intensityMultiplier = tc * 5 + 2.5f;

            audio.pitch = tc * 2 + 1;

            if (noise.enabled && !isAudioPlaying)
            {
                audio.Play();
                isAudioPlaying = true;
            }
            if (!noise.enabled && isAudioPlaying)
            {
                audio.Stop();
                isAudioPlaying = false;
            }
        }
        #endregion

        #region debug
        Debug.DrawRay(pTransform.position, transform.TransformVector(focusOffset));
        Debug.DrawRay(focus, targetRelativeOffset, Color.cyan);
        Debug.DrawRay(transform.position, camVelocity, Color.red);
        #endregion
    }

    private MyInput GetLookInput()
    {
        MyInput input = new MyInput();

        float rawX = horizontal.enabled ? horizontal.inverted ? -1: 1 : 0;
        rawX *= Input.GetAxis(horizontal.name);

        float rawY = vertical.enabled ? vertical.inverted ? -1 : 1 : 0;
        rawY *= Input.GetAxis(vertical.name);

        input.raw = new Vector2(rawX, rawY);

        input.rotation = new Vector2(rawX * horizontal.sensetivity, rawY * vertical.sensetivity);

        return input;
    }
}
