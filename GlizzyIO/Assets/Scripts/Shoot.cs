using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This class handles shooting the player's weapons.
*/
namespace Player {
    public class Shoot : NetworkBehaviour
    {
        public GameObject ammo;
        public GameObject firePoint;

        /*
        This function is called by clients and runs on the server.  The server tells the player to 
        fire the weapon.
        */
        [Command]
        void CmdShootRay() {
            RpcfireWeapon();
        }

        /*
        This function is called by the server to run on the client. It creates a new shot and 
        shoots it across the map. The shot spawns just in front of the player at the firePoint
        GameObject.
        */
        [ClientRpc]
        void RpcfireWeapon() {
            GameObject bullet = Instantiate(ammo, firePoint.transform.position, transform.rotation);
            bullet.transform.Rotate(new Vector3(0, -90, 0));
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 50;
            Destroy(bullet, 5);
        }

        // Start is called before the first frame update
        void Start()
        {
            
        } 

        // Update is called once per frame
        void Update()
        {
           if (Input.GetMouseButtonDown(0)) {
               CmdShootRay();
           } 
        }
    }
}

