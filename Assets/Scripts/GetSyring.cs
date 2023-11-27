using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSyring : MonoBehaviour , Iinteractable
{

    private Door door;

    private void Start()
    {
        door = FindObjectOfType<Door>();
    }

    public string GetInteractText()
    {
        return "Take Syringe";
    }

    public void Interact(Transform interactorTransform)
    {
        door.isSyringe = true;
        Destroy(gameObject);
    }
}
