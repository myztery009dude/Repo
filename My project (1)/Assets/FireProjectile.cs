using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform spawntransform;
    public float force = 500;

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            GameObject newProjectile = Instantiate(projectilePrefab, spawntransform.position, spawntransform.rotation);
            newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * force);
        }
    }
}
