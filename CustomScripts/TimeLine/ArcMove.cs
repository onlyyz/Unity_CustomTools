using UnityEngine;
using System.Collections;

public class ArcMove : MonoBehaviour
{
    public Transform target;
    public Transform target2;
    public float time = 1.0f;
    public float height = 1.0f;
    [Range(0.0F,2.0F)]
    public float scaleToTarget;
    [Range(0.0F,2.0F)]
    public float scaleToTarget2;
    public float scaleTime = 1.0f;

    public void StartMove() // Add this method
    {
        
        if (Vector3.Distance(target.position, this.transform.position) < 0.5f)
        {
            StartCoroutine(MoveInArc(target2.position, new Vector3(scaleToTarget2, scaleToTarget2, scaleToTarget2)));
        }
        else
        {
            StartCoroutine(MoveInArc(target.position, new Vector3(scaleToTarget, scaleToTarget, scaleToTarget)));
        }
    }

    IEnumerator MoveInArc(Vector3 target, Vector3 endScale)
    {
        Vector3 midpoint = (transform.position + target) / 2.0f;
        midpoint += Vector3.up * height;

        float progress = 0.0f;
        float scaleProgress = 0.0f;

        Vector3 startScale = transform.localScale;

        while (progress <= 1.0f)
        {
            Vector3 current = Vector3.Lerp(transform.position, midpoint, progress);
            Vector3 next = Vector3.Lerp(midpoint, target, progress);
            transform.position = Vector3.Lerp(current, next, progress);

            // Lerp the scale of the object during the movement
            if (scaleProgress <= 1.0f)
            {
                transform.localScale = Vector3.Lerp(startScale, endScale, scaleProgress);
                scaleProgress += Time.deltaTime / scaleTime;
            }

            progress += Time.deltaTime / time;

            yield return null;
        }
    }
}