using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : NetworkBehaviour {

    // Components for displaying player names
    public TextMesh playerNameText;
    public GameObject floatingInfo;

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    public GameObject kbullet;
    public GameObject firePoint;

    // Used to control the movement of the player
    public CharacterController controller;
    // Player movement speed    
    public float speed = 9f;
    // This may be obsolete, but slows the rotation to look more realistic
    public float turnSmoothTime = 0.1f;
    // Camera position offset from the local player 
    public Vector3 cameraOffSet = new Vector3(0, 20, -50);
    
    // Used by the SmoothDampAngle function for smmooth player rotations.
    float turnSmoothVelocity;

    void OnNameChanged(string _Old, string _New) {
        playerNameText.text = playerName;
    }

    void HandleMovement() {
        

        // Local Players run this code
        if (isLocalPlayer) {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 faceDir = new Vector3(0, 0, -1).normalized;
            Vector3 moveDir = new Vector3(horizontal, 0f, vertical).normalized;

            if(moveDir.magnitude >= 0.1f) {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
                Camera.main.transform.localPosition = transform.position + cameraOffSet;
                //Camera.main.transform.localPosition = new Vector3(transform.position.x + 0, transform.position.y + 20, transform.position.z - 50); 
            }

            // Convert the mouse position ot a point in 3D-space
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1)); // maybe i want z
            // Calculate the point of interseion between the line going through the camera and the mose posiiotn with the XZ-plane
            float tt = Camera.main.transform.position.y / (Camera.main.transform.position.y - mousePoint.y);
            Vector3 targetPoint = new Vector3(tt * (mousePoint.x - Camera.main.transform.position.x) + Camera.main.transform.position.x, 
                                                1, 
                                                tt * (mousePoint.z - Camera.main.transform.position.z) + Camera.main.transform.position.z);
        
            float targetAngle = Mathf.Atan2(targetPoint.x - transform.position.x, targetPoint.z - transform.position.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle+90, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // Every non local player runs this code
        if (!isLocalPlayer){
            floatingInfo.transform.LookAt(Camera.main.transform);
        }

        
        floatingInfo.transform.LookAt(Camera.main.transform);
    }

    public override void OnStartLocalPlayer() {
        //Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 20, -50);

        string name = "Player" + Random.Range(100, 999);
        CmdSetupPlayer(name);
    }

    [Command]
    public void CmdSetupPlayer(string _name) {
        // player info sent to server, then server updates syncvars chich handles it on all 
        // clients
        playerName = _name;
    }
    void Update() {
        HandleMovement();

        if (Input.GetMouseButtonDown(0)){
            CmdShootRay();
        }
    }

    [Command]
    void CmdShootRay(){
        RpcFireWeapon();
    }

    [ClientRpc]
    void RpcFireWeapon(){
        GameObject bullet = Instantiate(kbullet, firePoint.transform.position, transform.rotation);
        bullet.transform.Rotate(new Vector3(0, -90, 0));
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 50;
        Destroy(bullet, 5);
    }
}