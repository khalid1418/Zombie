using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstKitHeal : MonoBehaviour, Iinteractable
{
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }



    public string GetInteractText()
    {
        if (!player.IsHealthAtMax())
        {
            return "heal";
        }
        else
        {
            return "You Are Full";
        }
        
    }

    public void Interact(Transform interactorTransform)
    {
        if (!player.IsHealthAtMax())
        {
            player.GetHeal();
            Destroy(gameObject);
        }else
        {
            Debug.Log("full");
        }

    }
}
