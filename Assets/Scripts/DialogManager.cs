using StarterAssests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text nameText;
    public Text DialogueText;
    private Queue<string> sentences;
    [SerializeField]
    private GameObject boxGameObject;
    private bool isAhmedSpeaking = false;
    private FirstPersonController player;
    private Player playerAttack;
    private NPCInteractable npcInteractable;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        player = FindObjectOfType<FirstPersonController>();
        playerAttack = FindObjectOfType<Player>();
        npcInteractable = FindObjectOfType<NPCInteractable>();
        
    }

    public void StartDialogue(Dialog dialog)
    {
        playerAttack.enabled = false;
        player.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        boxGameObject.SetActive(true);
        nameText.text = dialog.name;
        
        sentences.Clear();
        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }
       DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));

        if (sentence.Contains("...")) // Modify this condition based on your specific sentence
        {
            nameText.text = "Ahmad";
            isAhmedSpeaking = true;
        }
        else if (isAhmedSpeaking)
        {
            // If Ahmed is speaking and the condition isn't met anymore, switch back to the default name
            nameText.text = "Me";
            isAhmedSpeaking = false;
        }
    
}

    IEnumerator TypeSentence(string sentence)
    {
        DialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            DialogueText.text += letter;
            yield return null;
        }
    }
    void EndDialogue()
    {
        npcInteractable.EndCharacter();
        Cursor.lockState = CursorLockMode.Locked;
        playerAttack.enabled = true;
        player.enabled = true;
        Destroy(gameObject);
    }

}
