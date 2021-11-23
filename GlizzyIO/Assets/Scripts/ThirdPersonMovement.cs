using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : NetworkBehaviour {

    public CharacterController controller;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // Update is called once per frame

    void HandleMovement() {

        if (!isLocalPlayer) {return; }

        /*
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 10f;

        transform.Rotate(0, moveX, 0);

        controller.Move(new Vector3(0, 0, moveZ));
        Camera.main.transform.Translate(new Vector3(0, 0, moveZ));
        */
        if (isLocalPlayer) {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if(direction.magnitude >= 0.1f) {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; 
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle+90, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);

                Camera.main.transform.Translate(moveDir.normalized * speed * Time.deltaTime);
            }
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
