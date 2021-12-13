using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This script handles reading the keyboard and mouse input and then 
moving the local player accordingly.
*/

namespace Player {
    public class Move : NetworkBehaviour
    {

        /* Feilds */
        public float speed = 10f;
        public CharacterController controller;
        public float turnSmoothTime = 0.1f;
        public Vector3 cameraOffset = new Vector3(0, 20, -50);
        float turnSmoothVelocity;

        /*
        This function should be called every frame.  It reads the user's keyboard
        and moves the player accordingly. It should only be ran by the local player.
        */
        void MovePlayer() {
            // Read the keyboard input
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 moveDir = new Vector3(horizontal, 0f, vertical).normalized;

            // Move the player if the move keys are pressed
            if(moveDir.magnitude >= 0.1f) {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
                MoveCamera();
            }
        }

        /*
        This function rotates the player to face the cursor. 
        */
        void RotatePlayer() {
            // Convert the mouse position ot a point in 3D-space
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));

            // Calculate the point of intersection between the line going through the camera and the mouse position with the XZ-plane
            float tt = Camera.main.transform.position.y / (Camera.main.transform.position.y - mousePoint.y);
            Vector3 targetPoint = new Vector3(tt * (mousePoint.x - Camera.main.transform.position.x) + Camera.main.transform.position.x, 
                                                1, 
                                                tt * (mousePoint.z - Camera.main.transform.position.z) + Camera.main.transform.position.z);
        
            // Smoothly rotate to face that intersection point
            float targetAngle = Mathf.Atan2(targetPoint.x - transform.position.x, targetPoint.z - transform.position.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle+90, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        /*
        This function moves the camera to match the player position plus a given 
        position offset.
        */
        void MoveCamera() {
            Camera.main.transform.localPosition = transform.position + cameraOffset;
        }

        // Update is called once per frame
        void Update()
        {
            if(isLocalPlayer) {
                MovePlayer();
                RotatePlayer();
            }
        }
    }
}
