using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foresight.Utilities;

[RequireComponent(typeof(AudioSource), typeof (LineRenderer))]
public class InteractiveDot : MonoBehaviour
{
    public Transform target;
    public AudioClip sound;
    public SpriteRenderer dot;
    public float lineSpeed = 5f;

    float distance;
    float counter;

    bool activated;

    AudioSource source;
    LineRenderer line;


    private void Start()
    {
        source = GetComponent<AudioSource>();
        line = GetComponent<LineRenderer>();

        distance = Vector3.Distance(transform.position, target.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated || !other.tag.Equals("Player")) return;
        activated = true;
        StartCoroutine(Activate());
    }

    IEnumerator Activate()
    {
        source.PlayOneShot(sound);
        line.enabled = true;
        line.SetPosition(0, transform.position);
        yield return new WaitUntil(() => HitTarget());
        target.gameObject.SetActive(true);
        dot.color = Color.green;

        yield return new WaitForSeconds(2);
        StartCoroutine(Disappear());
    }

    bool HitTarget()
    {
        if (counter < distance)
        {
            counter += .1f / lineSpeed;
            float x = Mathf.Lerp(0, distance, counter);
            var point0 = transform.position;
            var point1 = target.position;

            var pointALongLine = x * Vector3.Normalize(point1 - point0) + point0;

            line.SetPosition(1, pointALongLine);
        }

        // line.SetPosition(1, Vector3.MoveTowards(line.GetPosition(1), target.position, lineSpeed * Time.deltaTime));
        return MathUtilities.CheckDistance(line.GetPosition(1), target.position) <= .01f;
    }

    IEnumerator Disappear()
    {
        yield return new WaitUntil(() => Disappearing());
    }

    bool Disappearing()
    {
        line.SetPosition(1, Vector3.MoveTowards(line.GetPosition(1), transform.position, lineSpeed * 2 * Time.deltaTime));
        return MathUtilities.CheckDistance(line.GetPosition(1), transform.position) <= .01f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!activated || !other.tag.Equals("Player")) return;
        StartCoroutine(Disappear());
    }
}
