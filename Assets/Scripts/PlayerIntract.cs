using StarterAssests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerIntract : MonoBehaviour
{
    private StarterAssestsInputs _input;
    private void Awake()
    {
        _input = GetComponent<StarterAssestsInputs>();
    }

    void Update()
    {
        if (_input.interact)
        {
            Iinteractable interactable = GetInteractableObject();
            if (interactable != null)
            {
                interactable.Interact(transform);
            }
        }
    }

    public Iinteractable GetInteractableObject()
    {
        float interactRange = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out Iinteractable iinteractable))
            {
                return iinteractable;
            }
        }
        return null;

    }
}
