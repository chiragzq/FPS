using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public CharacterController controller;

    float ACCELERATION = 90f;
    float FRICTION_SCALE = 9f;
    float GRAVITATIONAL_FIELD_STRENGTH = 48f;
    float JUMP_VELOCITY = 14f;
    public float maxVel = 10f;

    public float RUN_SPEED = 10f;
    float WALK_SPEED = 5f;

    float GROUND_DETECT_RADIUS = 0.2f;
    public Transform groundDetector;
    public Transform ceilingDetector;
    public LayerMask groundMask;
    public LayerMask ceilingMask;

    bool prevGround;
    public bool isGround;
    public bool isCeiling;

    public Vector3 horizontalVel = Vector3.zero;
    public Vector3 normalVel = Vector3.zero;

    public AudioSource audioSource;
    AudioClip[] walkAudio = new AudioClip[6];
    AudioClip[] jumpLandAudio = new AudioClip[6];


    public float audioPlayThreshold = 195f;
    public float audioCountdownThreshold = 6f;
    float audioThresholdLeft; 

    void Start() {
        for(int i = 1;i <= 6;i ++) {
            walkAudio[i - 1] = Resources.Load<AudioClip>("Sounds/footstep/footstep" + i);
            jumpLandAudio[i - 1] = Resources.Load<AudioClip>("Sounds/jumpland/jumpland" + i);
        }
        audioThresholdLeft = audioPlayThreshold;
    }

    // Update is called once per frame
    void Update() {
        float xAccel = Input.GetAxis("Horizontal");
        float zAccel = Input.GetAxis("Vertical");

        prevGround = isGround;
        isGround = Physics.CheckSphere(groundDetector.position, GROUND_DETECT_RADIUS, groundMask);
        isCeiling = Physics.CheckSphere(ceilingDetector.position, GROUND_DETECT_RADIUS, ceilingMask);

        if(!prevGround && isGround && normalVel.y < -7f) {
            audioSource.PlayOneShot(jumpLandAudio[Random.Range(0, 6)], 0.5f);
        }

        Vector3 acceleration = (transform.right * xAccel + transform.forward * zAccel) * ACCELERATION;
        if(isGround && Input.GetAxis("Jump") == 0) {
            if(normalVel.y < -5f) 
                normalVel.y = -5f;
            else
                acceleration.y = -5f;
        } else {
            Vector3 parallelAccel = Vector3.Project(acceleration, horizontalVel);
            if(parallelAccel.normalized == horizontalVel.normalized) {  // Prevent mid air acceleration
                parallelAccel = Vector3.zero;
            } else { // Only slow down in midair, can't change direction
                parallelAccel = Vector3.ClampMagnitude(parallelAccel, horizontalVel.magnitude) * 5;
            }
            Vector3 rotatedVelocity = new Vector3(horizontalVel.z, 0, -horizontalVel.x);
            Vector3 normalAccel = Vector3.Project(acceleration, rotatedVelocity) / 8;
            acceleration = parallelAccel + normalAccel;
            acceleration.y = -GRAVITATIONAL_FIELD_STRENGTH;
        }

        if(!isGround && isCeiling) {
            normalVel.y *= -1;
        }

        if(isGround && Input.GetAxis("Jump") == 0) {
            acceleration += -FRICTION_SCALE * horizontalVel;
        }
        
        // Velocity verlet
        Vector3 absoluteDelta = normalVel * Time.deltaTime + 0.5f * acceleration * Time.deltaTime * Time.deltaTime;
        controller.Move(absoluteDelta);

        normalVel += acceleration * Time.deltaTime;
        horizontalVel += acceleration * Time.deltaTime;
        horizontalVel.y = 0;
        if(isGround) {
            if(Input.GetAxis("Jump") > 0) {
                normalVel.y = JUMP_VELOCITY;
            }
        }


        if(isGround && horizontalVel.sqrMagnitude >= audioCountdownThreshold * audioCountdownThreshold) {
            audioThresholdLeft -= horizontalVel.magnitude * Time.deltaTime;
            if(audioThresholdLeft < 0) {
                audioSource.PlayOneShot(walkAudio[Random.Range(0, 6)], 0.5f);
                audioThresholdLeft += audioPlayThreshold;
            }
        }

        if(Input.GetAxis("Walk") > 0) {
            maxVel = WALK_SPEED;
        } else {
            maxVel = RUN_SPEED;
        }

        horizontalVel = Vector3.ClampMagnitude(horizontalVel, maxVel);
        normalVel.x = horizontalVel.x;
        normalVel.z = horizontalVel.z;
    }
}
