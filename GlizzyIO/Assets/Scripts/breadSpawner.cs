using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breadSpawner : MonoBehaviour
{
    public GameObject breadspanwer;
    public GameObject breadCrumb;
    float timer = 0.0f;

    void Start()
    {
        timer = Time.time;
    }

    void Update()
    {
        if (Time.time - timer > 4)
        {
            spawnBreadForward();
            spawnBreadBackward();
            timer = Time.time;
        }
    }

    public void spawnBreadForward()
    {
        GameObject bread = Instantiate(breadCrumb, breadspanwer.transform.position, Quaternion.identity);
        bread.GetComponent<Rigidbody>().velocity = bread.transform.forward * -5;
        Destroy(bread, 8f);
    }

    public void spawnBreadBackward()
    {
        GameObject bread = Instantiate(breadCrumb, breadspanwer.transform.position, Quaternion.identity);
        bread.GetComponent<Rigidbody>().velocity = bread.transform.forward * 5;
        Destroy(bread, 8f);
    }
}
