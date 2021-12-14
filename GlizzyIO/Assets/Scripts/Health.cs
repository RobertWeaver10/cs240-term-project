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
            if (health < 100)
            {
                health += amount;
                updateHealthBar();
                Debug.Log("Healed");
            }
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
                    // Psuedo respawn
                    health = 100;
                    updateHealthBar();
                    transform.Translate(new Vector3(-5, 0, 5));
                }
            }

            // Make sure the health bar is pointing at each player's camera 
            //healthBar.transform.LookAt(Camera.main.transform);
            floatinginfo.transform.LookAt(Camera.main.transform);
        }

        void OnCollisionEnter (Collision collisionInfo)
        {
            if (collisionInfo.collider.tag == "Ketchup")
            {
                RpcDamage(10);
                Debug.Log("I was shot with ketchup");
            }
            
            if (collisionInfo.collider.tag == "Bread")
            {
                RpcHeal(10);
                Debug.Log("I stacked more bread");
            }
        }

        void Start() {
            updateHealthBar();
        }
    }
}
