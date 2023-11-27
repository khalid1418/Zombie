using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour , Iinteractable
{
    [SerializeField]
    private Dialog dialog;
    [SerializeField] 
    private string interactText;
    private Collider myCollider;

    private Collider[] ragdollColliders;
    private Rigidbody[] ragdollBodies;
    private Animator animator;


    private void Start()
    {
        myCollider = GetComponent<Collider>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();

        foreach (Collider collider in ragdollColliders)
        {
            if (!collider.CompareTag("NPC"))
            {
                collider.enabled = false;
            }
        }
        foreach (Rigidbody rigidbody in ragdollBodies)
        {

                rigidbody.isKinematic = true;
            
        }

    }
    public string GetInteractText()
    {
        return interactText;
    }

    public void Interact(Transform interactorTransform)
    {
        FindObjectOfType<DialogManager>().StartDialogue(dialog);
        myCollider.enabled = false;
        
    }
    public void EndCharacter()
    {
        animator.enabled = false;
        foreach (Collider collider in ragdollColliders)
        {
            if (!collider.CompareTag("NPC"))
            {
                collider.enabled = true;
            }
        }
        foreach (Rigidbody rigid in ragdollBodies)
        {
            rigid.isKinematic = false;
        }
    }


}
