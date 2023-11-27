using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickUp : MonoBehaviour , Iinteractable
{
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    public string GetInteractText()
    {
        return "PickUp -> Press(1) to change weapons";
    }

    public void Interact(Transform interactorTransform)
    {
        player.isPickUp = true;
        Destroy(gameObject);
        
    }

}
