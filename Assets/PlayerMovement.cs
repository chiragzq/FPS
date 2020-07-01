using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public CharacterController controller;

    float SPEED_MULTIPLIER = 16f;

    // Update is called once per frame
    void Update() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 delta = transform.right * x + transform.forward * z;
        controller.Move(delta * SPEED_MULTIPLIER * Time.deltaTime);
    }
}
