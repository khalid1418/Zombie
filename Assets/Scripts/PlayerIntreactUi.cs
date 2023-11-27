using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerIntreactUi : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI interactText;
    [SerializeField]
    private GameObject containerGameObject;
    [SerializeField]
    private PlayerIntract playerIntract;


    private void Update()
    {
            if(playerIntract.GetInteractableObject() != null)
        {
            Show(playerIntract.GetInteractableObject());
        }
        else
        {
            Hide();
        }
    }

    private void Show(Iinteractable iinteractable)
    {
        containerGameObject.SetActive(true);
        interactText.text = iinteractable.GetInteractText();
    }
    private void Hide()
    {
        containerGameObject.SetActive(false);
    }
}
