using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKey : MonoBehaviour, Iinteractable
{
    private Door[] door;
    private void Start()
    {
       door = FindObjectsOfType<Door>(); 
    }
    public string GetInteractText()
    {
        return "Key";
    }

    public void Interact(Transform interactorTransform)
    {
        foreach (Door door in door)
        {
            door.key = true;
        }
        Destroy(gameObject);
    }


}
