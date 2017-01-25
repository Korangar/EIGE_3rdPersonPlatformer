using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region StructDefinition
    public struct MyInput
    {
        public Vector3 raw, lookAdjusted, groundAdjusted;
        public float strength;
    }

    [System.Serializable]
    public struct MovementInfo
    {
        public float walkVelocity;
        public float jumpVelocity;
        public float pounceFactor;
        public float customGravity;
    }
    #endregion

    #region EditorVariables
    [SerializeField]
    private MovementInfo movement;
    #endregion

    private ThirdPersonCamera myCamera;
    private CapsuleCollider myCollider;
    private MeshRenderer myRenderer;
    private Rigidbody myRigidbody;
    private bool inputLock = false;
    private bool vkJump = false;
    
    #region StateInfo
    private bool isGrounded = false;
    #endregion

    #region Properties
    private Vector3 Position
    {
        get { return myRigidbody.position; }
    }
    #endregion

    #region Events
    public event System.Action e_Death;
    public event System.Action e_Jump; 
    public event System.Action e_Land;
    public event System.Action e_Goal;
    #endregion

    
    // Use this for initialization
    void Start () {
        myCamera = FindObjectOfType<ThirdPersonCamera>();
        myRenderer = GetComponentInChildren<MeshRenderer>();
        myCollider = GetComponent<CapsuleCollider>();
        myRigidbody = GetComponent<Rigidbody>();

        e_Death += Event_Death;
        e_Goal += Event_Goal;
        e_Jump += Event_Jump;
        e_Land += Event_Land;
    }
	
	// Update is called once per frame
	void Update () {
        // get keypress only when on ground to prevent a button queue
        vkJump = !inputLock && isGrounded && 
        // dont overwrite jump input once it was pressed under the right conditions
        // prevents missing presses when update and fixUpdate are not synced
            (vkJump || Input.GetButtonDown("Jump"));
	}

    void FixedUpdate()
    {
        // update ground information
        DoGroundCheck();

        // retrieve input
        MyInput input = GetPlayerInput();
        //Quaternion desiredLookRotation = Quaternion.LookRotation(input.lookAdjusted, transform.up);

        if (isGrounded)
        #region GroundMovement
        {
            // create movement Vector
            Vector3 move;
            {
                float distance = input.strength * movement.walkVelocity * Time.fixedDeltaTime;
                move = input.groundAdjusted * distance;
            }
         
            if (input.strength > 0.2f)
            #region Walking
            {
                #region RotateRigidBody
                Quaternion newRotation;
                // get current rotation
                newRotation = myRigidbody.rotation;
                // direct it into the movement direction relative to camera
                newRotation.SetLookRotation(input.lookAdjusted, Vector3.up);
                // apply the rotation
                myRigidbody.MoveRotation(newRotation);
                #endregion

                // try to prevent moving into tight spots or diving into walls by proactively resolving collisions
                RaycastHit hit;
                if (myRigidbody.SweepTest(input.groundAdjusted, out hit, move.magnitude))
                #region SweepTestHitHandling
                {
                    if (hit.collider.isTrigger)
                    {
                        // do not interact with triggers
                        goto endSweepTestHandling;
                    }
                    // TODO check for different tags here

                    // evaluate wheter obstacle is bigger than climbing threshold
                    if (hit.point.y - Position.y >= myCollider.radius)
                    #region WallShmier
                    {
                        // evaluate how far player would approach the obstacle and prevent him from getting closer than the safetyDistance
                        Vector3 safetyDistance = hit.normal * proactiveSafetyDistance;
                        // adjust move vector
                        move = input.groundAdjusted * wallSlideCoefficient * hit.distance + safetyDistance;
                        // adjust velocities (just in case)
                        myRigidbody.velocity = Vector3.ProjectOnPlane(myRigidbody.velocity, hit.normal);
                    }
                    #endregion
                    else
                    {
                        // if an obstacle is small enough, the check ground method can simply try to climb over it
                        // by taking its normal and moving over its surface ... blah blah blah
                    }
                }
                // exit point after the collision prevention has been resolved
                endSweepTestHandling:
                #endregion

                // move to new position
                myRigidbody.MovePosition(myRigidbody.position + move);
            }
            #endregion
            
            if (vkJump)
            #region JumpLaunch
            {
                // consume the button press
                vkJump = false;
                
                // evaluate the amount of impulse to be directed in movement direction 
                Vector3 jumpImpulseDirection = 
                    move.normalized * input.strength * movement.pounceFactor + 
                    Vector3.up * (1 - movement.pounceFactor);

                jumpImpulseDirection.Normalize();
                // apply velocity impulse;
                myRigidbody.velocity += jumpImpulseDirection * movement.jumpVelocity;
                e_Jump();
            }
            #endregion
        }
        #endregion
        else
        #region AirHandling
        {
            Vector3 flightDirection = myRigidbody.velocity.normalized;
            float distance = myRigidbody.velocity.magnitude;
            // no air controll yet 
            // and probably it won't be needed.
        }
        #endregion
    }

    void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("WorldBounds"))
        {
            e_Death();
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Goal"))
        {
            StartCoroutine(Routine_GoalAnimation(c.transform.position));
            e_Goal();
        }
    }


    #region Input
    private MyInput GetPlayerInput()
    {
        if (inputLock) { return new MyInput(); }

        // fetch input
        Vector3 axisInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //get input strength
        float inputStrength = axisInput.magnitude;

        // quadratic function gives the user higher fidelity with low input while pertaining same maximum magnitude.
        // apparently this has been used by microsoft when designing controlls for their Xbox racing games.
        // this step is optional
        inputStrength *= inputStrength;
        axisInput.Normalize();
        axisInput *= inputStrength;

        // make input relative to camera look direction
        Vector3 lookOrientedInput = myCamera.transform.TransformVector(axisInput);
        // ignore vertical camera rotation
        lookOrientedInput.y = 0;
        lookOrientedInput.Normalize();

        // initialize and return
        MyInput input = new MyInput();
        // "raw" axis input
        input.raw = axisInput;
        // strength
        input.strength = axisInput.magnitude;
        // look oriented input
        input.lookAdjusted = lookOrientedInput;
        // get movedirection from look oriented input
        input.groundAdjusted = Vector3.ProjectOnPlane(lookOrientedInput, nGround);
        input.groundAdjusted.Normalize();
        return input;
    }
    #endregion

    #region PhysicsDuktape
    const float wallSlideCoefficient = 1.1f;
    // distance to be kept from obstacles that are too big for stepping over
    const float proactiveSafetyDistance = .01f;
    // cast needs a bit of room, so this is there to controll the offset.
    const float sphereCastOriginOffset = 0.1f;
    Vector3 nGround;

    void DoGroundCheck()
    {
        /* Calculates a surface normal for the floor the player stands on 
         * even though I could live without slopes in this project
         * it is used to make rigidbody.MovePosition more consistent
         * like walking over small obstacles, sticking to slopes and smoother walking off edges
         */
        
        // offset ray origin upwards so the cast starts above the collider skin
        Vector3 origin = Position;
        origin.y += sphereCastOriginOffset + myCollider.radius;

        RaycastHit hit;
        if (Physics.SphereCast(origin, myCollider.radius, Vector3.down, out hit, sphereCastOriginOffset + .01f))
        #region OnGround
        {
            if (!isGrounded)
            {
                #region FeetTouchedGround
                myRigidbody.velocity = Vector3.zero;
                #endregion
            }
            isGrounded = true;
            nGround = hit.normal;

            // Debug show ground surface normal.
            Debug.DrawRay(hit.point, nGround * 0.5f, Color.green, Time.fixedDeltaTime);
        }
        #endregion
        else
        #region InAir
        {
            isGrounded = false;
            
            // standard input orientation should be the horizontal plane in worldspace
            nGround = Vector3.up;
            myRigidbody.AddForce(0, -movement.customGravity, 0);
        }
        #endregion
    }
    #endregion

    #region EventHandling
    public void Event_Death()
    {
        Debug.Log("Death.");
        GameObject rswpn = GameObject.FindGameObjectWithTag("Respawn");
        StartCoroutine(Routine_Respawn(rswpn.transform.position));
    }

    public void Event_Goal()
    {

    }

    public void Event_Jump()
    {

    }

    public void Event_Land()
    {

    }
    #endregion

    #region RoutineDefinitions
    IEnumerator Routine_GoalAnimation(Vector3 g)
    {
        Transform model = GetComponentInChildren<MeshRenderer>().transform;
        Vector3 originalPos = model.position;
        inputLock = true;
        
        for (float t = 0; t <= 1; t += Time.deltaTime/2.5f)
        {
            model.position = Vector3.Lerp(originalPos, g, t);
            yield return null;
        }
    }

    IEnumerator Routine_Respawn(Vector3 respawnPosition)
    {
        bool prevLock = inputLock;
        inputLock = true;
        yield return new WaitForEndOfFrame();
        transform.position = respawnPosition;
        yield return new WaitForSeconds(1);
        inputLock = prevLock;
    }
    #endregion
}
