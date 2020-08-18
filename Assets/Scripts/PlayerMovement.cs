using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public CharacterController controller;

    float ACCELERATION = 90f;
    float FRICTION_SCALE = 10f;
    float GRAVITATIONAL_FIELD_STRENGTH = 48f;
    float JUMP_VELOCITY = 14f;
    public float MAX_VEL = 10f;

    float GROUND_DETECT_RADIUS = 0.2f;
    public Transform groundDetector;
    public Transform ceilingDetector;
    public LayerMask groundMask;
    public LayerMask ceilingMask;

    public bool isGround;

    public Vector3 horizontalVel = Vector3.zero;
    public Vector3 normalVel = Vector3.zero;

    // Update is called once per frame
    void Update() {
        float xAccel = Input.GetAxis("Horizontal");
        float zAccel = Input.GetAxis("Vertical");

        isGround = Physics.CheckSphere(groundDetector.position, GROUND_DETECT_RADIUS, groundMask);
        bool isCeiling = Physics.CheckSphere(ceilingDetector.position, GROUND_DETECT_RADIUS, ceilingMask);

        Vector3 acceleration = Vector3.zero;
        if(isGround) {
            acceleration = new Vector3(xAccel, 0f, zAccel).normalized * ACCELERATION;
            if(normalVel.y < -5f) 
                normalVel.y = -5f;
            acceleration.y = -5f;
        } else {
            acceleration = new Vector3(0f, -GRAVITATIONAL_FIELD_STRENGTH, 0f);
        }

        if(!isGround && isCeiling) {
            normalVel.y *= -1;
        }

        if(isGround) {
            acceleration += -FRICTION_SCALE * horizontalVel;
        }
        
        // Velocity verlet
        Vector3 absoluteDelta = normalVel * Time.deltaTime + 0.5f * acceleration * Time.deltaTime * Time.deltaTime;
        Vector3 relativeDelta = transform.right * absoluteDelta.x + transform.up * absoluteDelta.y + transform.forward * absoluteDelta.z;
        controller.Move(relativeDelta);

        normalVel += acceleration * Time.deltaTime;
        horizontalVel += acceleration * Time.deltaTime;
        horizontalVel.y = 0;
        if(isGround) {
            if(Input.GetButtonDown("Jump")) {
                normalVel.y = JUMP_VELOCITY;
            }
        }
        horizontalVel = Vector3.ClampMagnitude(horizontalVel, MAX_VEL);
        normalVel.x = horizontalVel.x;
        normalVel.z = horizontalVel.z;
    }
}
