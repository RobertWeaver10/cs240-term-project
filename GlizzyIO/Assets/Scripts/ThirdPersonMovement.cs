using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : NetworkBehaviour {

    public CharacterController controller;

    public float speed = 9f;

    public float turnSmoothTime = 0.1f;
    
    public Vector3 cameraOffSet = new Vector3(0, 20, -50);
    float turnSmoothVelocity;

    // Update is called once per frame

    void HandleMovement() {

        if (isLocalPlayer) {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 faceDir = new Vector3(0, 0, -1).normalized;
            Vector3 moveDir = new Vector3(horizontal, 0f, vertical).normalized;

            if(moveDir.magnitude >= 0.1f) {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
                Camera.main.transform.localPosition = transform.position + cameraOffSet;
                //Camera.main.transform.localPosition = new Vector3(transform.position.x + 0, transform.position.y + 20, transform.position.z - 50); 
            }

            // Convert the mouse position ot a point in 3D-space
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1)); // maybe i want z
            // Calculate the point of interseion between the line going through the camera and the mose posiiotn with the XZ-plane
            float tt = Camera.main.transform.position.y / (Camera.main.transform.position.y - mousePoint.y);
            Vector3 targetPoint = new Vector3(tt * (mousePoint.x - Camera.main.transform.position.x) + Camera.main.transform.position.x, 
                                                1, 
                                                tt * (mousePoint.z - Camera.main.transform.position.z) + Camera.main.transform.position.z);
        
            float targetAngle = Mathf.Atan2(targetPoint.x - transform.position.x, targetPoint.z - transform.position.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle+90, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    public override void OnStartLocalPlayer() {
        //Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 20, -50);
    }
    void Update() {
        HandleMovement();
    }
}
