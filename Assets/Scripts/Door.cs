using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour , Iinteractable
{
    [SerializeField]
    private Animator animator;
    private bool openedDoor = false;
    private Collider collider;
    public bool key = false;
    public bool isSyringe = false;


    private void Start()
    {
        animator = GetComponentInParent<Animator>(); 
        collider = GetComponent<Collider>();
    }
    public string GetInteractText()
    {
        if (gameObject.CompareTag("FinalDoor")){
            if (isSyringe)
            {
                return "Leave";
            }
            else
            {


                return "Need Syringe";
            }
        }

        if (gameObject.CompareTag("DoorKey")){
            if (key)
            {
                return "Open Door";
            }
            else
            {
                return "Door Locked";
            }
        }
        else {
            return "Open Door";
        }

    }

    public void Interact(Transform interactorTransform)
    {
        if (!openedDoor)
        {
            if (gameObject.CompareTag("FinalDoor"))
            {
                if (isSyringe)
                {
                    SceneManager.LoadScene("StartScene");


                }else
                {
                    Debug.Log("I need Syringe");
                }

            }else
            if (gameObject.CompareTag("DoorKey"))
            {
                if(key)
                {
                    animator.SetTrigger("Open");
                    openedDoor = true;
                    collider.enabled = false;
                }else
                {
                    Debug.Log("I need Key");
                }
            }
            else
            {


                animator.SetTrigger("Open");
                openedDoor = true;
                collider.enabled = false;
            }

        }
    }

}
