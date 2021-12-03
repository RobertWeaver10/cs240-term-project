using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour {

    int health = 100;
    public GameObject healthBar;

    public void TakeDamage(int amount) {
        if (!isServer) return;

        health -= amount;
        RpcDamage(amount);
    }    

    [ClientRpc]
    public void RpcDamage(int amount) {
        Debug.Log("Took damage:" + amount);
    }

    void Update() {
        if (health <= 0 ) {
            // Ask rob how to destroy an object
        }            
    }

    void Start() {
        healthBar.transform.localScale = new Vector3(0.5f, 0.05f, 0.05f);
        healthBar.GetComponent<Renderer>().material.color = Color.yellow;
    }
}