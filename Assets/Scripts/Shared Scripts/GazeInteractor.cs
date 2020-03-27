using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeInteractor : MonoBehaviour
{
    public float gazeRadius = .5f;
    public LayerMask interactionLayer;

    Interactable interactable;
    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, gazeRadius, out hit, 150f, interactionLayer))
        {
            Debug.Log(hit.transform);
            interactable = hit.transform.GetComponent<Interactable>();
            interactable?.Interacting();
        }
        else if(interactable)
        {
            interactable.StoppedInteracting();
            interactable = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, gazeRadius);
    }
}
