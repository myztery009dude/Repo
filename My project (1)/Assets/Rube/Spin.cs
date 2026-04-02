using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float ROTATION_SPEED = 90f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * ROTATION_SPEED * Time.deltaTime);
    }
}
