using UnityEngine;
using TMPro;

public class NPCDialogueTrigger : MonoBehaviour
{
    public string npcName;
    [TextArea(2,5)] public string[] dialogueLines;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            DialogueManager dm = FindObjectOfType<DialogueManager>();
            if (!dm) return;

            if (!dm.IsDialogueActive())
            {
                dm.StartDialogue(npcName, dialogueLines, dialoguePanel, nameText, dialogueText);
            }
            else
            {
                dm.NextLine();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }
}
