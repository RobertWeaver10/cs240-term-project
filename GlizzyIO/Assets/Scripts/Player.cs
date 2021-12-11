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
        updateHealthbar();
    }    

    public void heal(int amount) {
        if (!isServer) return;

        health += amount;
        RpcHeal(amount);
        updateHealthbar();
    }

    [ClientRpc]
    public void RpcHeal(int amount) {
        Debug.Log("Healed: " + amount);
    }

    [ClientRpc]
    public void RpcDamage(int amount) {
        Debug.Log("Took damage:" + amount);
    }

    void Update() {
        // If player dies
        if (health <= 0 ) {
            // Ask rob how to destroy an object
        }      
    }


    void updateHealthbar() {
        // NOTE: this function should be called anytime the player health is changed.

        // Scale health bar to match health level
        healthBar.transform.localScale = new Vector3(0.5f * health/100, 0.05f, 0.05f);
        if(health > 66) {
            healthBar.GetComponent<Renderer>().material.color = Color.green;
        } else if (health > 33) {
            healthBar.GetComponent<Renderer>().material.color = Color.yellow;
        } else {
            healthBar.GetComponent<Renderer>().material.color = Color.red;
        }
        
    }

    void Start() {
        updateHealthbar();
    }
}