using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event System.Action OnReachedEnd;

    // Move speed and smooth movement
    public float moveSpeed = 6;
    public float smoothMoveTime = .1f;
    public float turnSpeed = 100;

    // Private variables for smoothing
    float angle;
    Vector3 velocity;
    float smoothInputMagnitude;
    float smoothMoveVelocity;

    // Private variables for rigidbody and camera
    Rigidbody rbody;
    Camera viewCamera;
    bool disabled;

    void Start(){
        rbody = GetComponent<Rigidbody> ();
        viewCamera = Camera.main;

      
    }

    void Update(){
        // Controls to get object turning towards mouse
        // Not currently used in tutorial
        // Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        // transform.LookAt(mousePos + Vector3.up * transform.position.y);

        // Get Input Direction from user WASD
        Vector3 inputDirection = Vector3.zero;
        if(!disabled){
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }
        float inputMagnitude = inputDirection.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

        // Rotate the target in the direction of our target euler angle
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);

        velocity = transform.forward * moveSpeed * smoothInputMagnitude;
    }

    void FixedUpdate() {
        // Using rigidboy in order to set position/rotation
        rbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rbody.MovePosition(GetComponent<Rigidbody>().position + velocity * Time.deltaTime);
    }

}
