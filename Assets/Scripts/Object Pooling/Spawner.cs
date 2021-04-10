using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    ObjectPooler objectPooler;
    float timer = 2f;

    private void Start()
    {
        objectPooler = ObjectPooler._instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            objectPooler.SpawnFromPool("Rock A", transform.position, Quaternion.identity);
            objectPooler.SpawnFromPool("Rock B", transform.position, Quaternion.identity);

            timer = 2f;
        }
    }
}
