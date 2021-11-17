using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    void HandleMovement() {
        if (isLocalPlayer) {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal * 0.25f, 0, moveVertical * 0.25f);
            transform.position = transform.position + movement;
        }
    }

    void Update() {
        HandleMovement();
    }
}