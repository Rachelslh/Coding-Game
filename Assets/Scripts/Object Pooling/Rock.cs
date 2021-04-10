using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour, IPooledObject
{
    public float upForce = 2f;
    public float sideForce = 10f;

    public void OnObjectSpawn()
    {
        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(upForce / 2f, upForce);
        float zForce = Random.Range(-sideForce, sideForce);

        Vector3 force = new Vector3(xForce, yForce, zForce);

        GetComponent<Rigidbody>().AddForce(force);
    }
 
}
