using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {
    float INPUT_MULTIPLIER = 180.0f;
    float xRotation = 0.0f;

    public Transform playerTransform;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        float mouseX = Input.GetAxis("Mouse X") * INPUT_MULTIPLIER * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * INPUT_MULTIPLIER * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation - mouseY, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerTransform.Rotate(Vector3.up * mouseX);
    }
}
