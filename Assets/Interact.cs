using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private float _radius = 5f;
    [SerializeField] private Transform _target;
    [SerializeField] private LayerMask _detectionLayer;

    private void OnEnable()
    {
        DetectObjectsInRange();
    }

    private void DetectObjectsInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(_target.position, _radius, _detectionLayer);

        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;

            IInteractable interactable = obj.GetComponent<IInteractable>();
            if (interactable != null)
                interactable.Interact();
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_target.position, _radius);
    }
}
