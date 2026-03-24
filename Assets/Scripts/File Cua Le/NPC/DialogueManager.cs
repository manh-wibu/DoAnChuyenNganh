using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class DialogueManager : MonoBehaviour
{
    [Header("Typing Settings")]
    public float typingSpeed = 0.3f;

    private string[] lines;
    private int index;
    private bool isTyping;

    private GameObject dialoguePanel;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI dialogueText;

    // Callback khi Dialogue kết thúc
    private Action onDialogueEnd;

    // Callback khi đến câu thoại cuối
    private Action onLastLineReached;

    public void StartDialogue(string npcName, string[] dialogueLines,
        GameObject panel, TextMeshProUGUI nameField, TextMeshProUGUI textField,
        Action onEnd = null, Action onLastLine = null)
    {
        dialoguePanel = panel;
        nameText = nameField;
        dialogueText = textField;

        onDialogueEnd = onEnd;
        onLastLineReached = onLastLine;

        dialoguePanel.SetActive(true);
        nameText.text = npcName;
        lines = dialogueLines;
        index = 0;

        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in lines[index])
        {
            dialogueText.text += c;
            yield return null; // mượt hơn so với WaitForSeconds
        }

        isTyping = false;

        // Nếu câu hiện tại là câu cuối → gọi callback
        if (index == lines.Length - 1 && onLastLineReached != null)
        {
            onLastLineReached.Invoke();
        }
    }

    public void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = lines[index];
            isTyping = false;

            // Kiểm tra câu cuối
            if (index == lines.Length - 1 && onLastLineReached != null)
            {
                onLastLineReached.Invoke();
            }

            return;
        }

        if (index < lines.Length - 1)
        {
            index++;
            StopAllCoroutines();
            StartCoroutine(TypeLine());
        }
        else
        {
            dialoguePanel.SetActive(false);
            onDialogueEnd?.Invoke();
            onDialogueEnd = null;
            onLastLineReached = null;
        }
    }

    public bool IsDialogueActive()
    {
        return dialoguePanel != null && dialoguePanel.activeSelf;
    }

    public void EndDialogueImmediate()
    {
        StopAllCoroutines();
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
        onDialogueEnd?.Invoke();
        onDialogueEnd = null;
        onLastLineReached = null;
    }
}
