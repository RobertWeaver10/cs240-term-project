using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {

    public class Health : NetworkBehaviour
    {
        public float health = 100f;
        public GameObject healthBar;
        public GameObject floatinginfo;

        /*
        This function increases the player's 
        health by the specified amount. Calls updateHealthBar() to reflect the change.
        */
        public void RpcHeal(float amount) {
            health += amount;
            updateHealthBar();
            Debug.Log("Healed");
        }

        /*
        This function gets called by the server and runs on clients. It decreases the player's 
        health by the specified amount. Calls updateHealthBar() to reflect the change.
        */
        public void RpcDamage(float amount) {
            health -= amount;
            updateHealthBar();
            Debug.Log("Damage");
        }

        /*
        This function scales and changes the color of the health bar to match the current 
        amount of health the player has. It should be called anytime health is modified.
        */
        public void updateHealthBar() {
            // Scale health bar to match health level
            healthBar.transform.localScale = new Vector3(0.5f * health / 100, 0.05f, 0.05f);
            if (health > 66)
            {
                healthBar.GetComponent<Renderer>().material.color = Color.green;
            }
            else if (health > 33)
            {
                healthBar.GetComponent<Renderer>().material.color = Color.yellow;
            }
            else
            {
                healthBar.GetComponent<Renderer>().material.color = Color.red;
            }

        }

        /*
        Update runs every frame.
        */
        void Update() {
            // Check to see if the local player has died.
            if (isLocalPlayer) {
                if(health <= 0) {
                    // The player has died!
                    Application.LoadLevel(2);
                    Destroy(transform.gameObject);
                }
            }

            // Make sure the health bar is pointing at each player's camera 
            //healthBar.transform.LookAt(Camera.main.transform);
            floatinginfo.transform.LookAt(Camera.main.transform);
        }

        void OnCollisionEnter (Collision collisionInfo)
        {
            Debug.Log("I was hit");
            RpcDamage(10);
        }

        void Start() {
            updateHealthBar();
        }
    }
}
