using System;
using System.Collections;
using Sirenix.Utilities;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    private void Start()
    {
        Destroy(gameObject,3f);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Destroy();
    }

    public IEnumerator move(Vector3 start, Vector3 midPoint, Transform target)
    {
        for (float i = 0; i <=1; i+=Time.deltaTime)
        {
            Vector3 p1 = Vector3.Lerp(start,midPoint,i);
            Vector3 p2 = Vector3.Lerp(midPoint, target.position, i);

            Vector3 P = Vector3.Lerp(p1, p2, i);
 
            yield return StartCoroutine(MoveToPoint(P));
        }
        yield return StartCoroutine(MoveToPoint(target));
    }

    IEnumerator MoveToPoint(Vector3 pos)
    {
        yield return null;
        while (Vector3.Distance(transform.position, pos)>0.1f)
        {
            Vector3 dir = pos - transform.position;
            transform.up = dir;
            transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
            yield return null;
        }
    }
    IEnumerator MoveToPoint(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position)>0.1f)
        {
            Vector3 dir = target.position - transform.position;
            transform.up = dir;
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
            yield return null;
        }
    }
}
