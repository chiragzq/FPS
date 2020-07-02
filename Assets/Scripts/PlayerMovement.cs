using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public CharacterController controller;

    float ACCELERATION = 192f;
    float FRICTION_SCALE = 0.75f;
    float GRAVITATIONAL_FIELD_STRENGTH = 48f;
    float JUMP_VELOCITY = 14f;

    float GROUND_DETECT_RADIUS = 0.3f;
    public Transform groundDetector;
    public LayerMask groundMask;

    Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update() {
        float xAccel = Input.GetAxis("Horizontal");
        float zAccel = Input.GetAxis("Vertical");

        bool isGround = Physics.CheckSphere(groundDetector.position, GROUND_DETECT_RADIUS, groundMask);
        Debug.Log(isGround);

        Vector3 acceleration;
        if(isGround) {
            acceleration = new Vector3(xAccel, 0f, zAccel).normalized * ACCELERATION;
        } else {
            acceleration = new Vector3(0f, -GRAVITATIONAL_FIELD_STRENGTH, 0f);
        }

        velocity += acceleration * Time.deltaTime;
        if(isGround) {
            velocity *= FRICTION_SCALE;

            if(Input.GetButtonDown("Jump")) {
                velocity.y = JUMP_VELOCITY;
            }
        }


        Vector3 delta = transform.right * velocity.x + transform.forward * velocity.z;
        delta.y = velocity.y;
        // Debug.Log(delta.y);
        controller.Move(delta * Time.deltaTime);
    }
}
