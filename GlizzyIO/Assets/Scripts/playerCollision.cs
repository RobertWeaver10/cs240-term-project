using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCollision : MonoBehaviour
{
    Player playerscript;

    public void Start()
    {
        playerscript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    void OnCollisionEnter (Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Player")
        {
            playerscript.TakeDamage(25);
            Destroy(gameObject);
        }
    }
}
