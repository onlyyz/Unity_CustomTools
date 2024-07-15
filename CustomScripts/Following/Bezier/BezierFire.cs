using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BezierFire : MonoBehaviour
{
    public Transform target;
    public float r;
    public  GameObject fireObj;
    
    private void Start()
    {
        StartFire();
    }

    public void StartFire()
    {
        StartCoroutine(Fire());
    }
    public void StopFire()
    {
        StopAllCoroutines();
    }

    // bezier center point
    public Vector3 GetRandomPoint(float r)
    {
        return transform.position + new Vector3(Random.Range(-r, r), Random.Range(-r, r), Random.Range(-r, r));
    }

    IEnumerator Fire()
    {
        while (true)
        {
            GameObject VFX = GameObject.Instantiate(fireObj, transform.position, Quaternion.identity);
            StartCoroutine(VFX.GetComponent<Bullet>().move(VFX.transform.position, GetRandomPoint(r), target));
            yield return new WaitForSeconds(0.1f);
        }
    }
}