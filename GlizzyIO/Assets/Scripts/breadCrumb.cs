using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breadCrumb : MonoBehaviour
{
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Player")
        {
            Destroy(gameObject);
        }
    } 
}
