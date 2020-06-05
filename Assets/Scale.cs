using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    public Vector3 endingScale;
    public float scaleSpeed;

    private void OnEnable()
    {
        StartCoroutine(StartScale());
    }

    IEnumerator StartScale()
    {
        yield return new WaitUntil(() => Scaling());
    }

    bool Scaling()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, endingScale, scaleSpeed * Time.deltaTime);
        return Vector3.Distance(transform.localScale, endingScale) <= 0;
    }
}
