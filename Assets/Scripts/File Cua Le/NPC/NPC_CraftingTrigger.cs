/*
using UnityEngine;
using TMPro;

public class NPCCraftingTrigger : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName;
    [TextArea(2, 5)] public string[] dialogueLines;

    [Header("UI References")]
    public GameObject craftChoicePanel;      // Panel 2 nút: Ghép / Không
    public TextMeshProUGUI choiceNameText;   // Tên NPC trong panel chọn
    public TextMeshProUGUI choiceDialogueText; // Text Dialogue trong panel chọn
    public GameObject craftingPanel;         // Panel ghép đồ

    private bool playerInRange = false;
    private DialogueManager dm;

    void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
        craftChoicePanel.SetActive(false);
        craftingPanel.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange || dm == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!dm.IsDialogueActive())
            {
                // Dừng game khi bắt đầu Dialogue
                Time.timeScale = 0f;

                // Mở Dialogue, callback khi đến câu cuối → hiện 2 nút
                dm.StartDialogue(npcName, dialogueLines,
                    craftChoicePanel, choiceNameText, choiceDialogueText,
                    onEnd: null,
                    onLastLine: () => { craftChoicePanel.SetActive(true); });
            }
            else
            {
                dm.NextLine();
            }
        }
    }

    /// <summary>
    /// Nhấn nút "Ghép"
    /// </summary>
    public void OnClickCraft()
    {
        craftChoicePanel.SetActive(false);
        dm.EndDialogueImmediate();  // ẩn Dialogue panel luôn
        craftingPanel.SetActive(true);
        // Game vẫn dừng để chọn vật phẩm
    }

    /// <summary>
    /// Nhấn nút "Không"
    /// </summary>
    public void OnClickCancel()
    {
        craftChoicePanel.SetActive(false);
        dm.EndDialogueImmediate();
        Time.timeScale = 1f; // tiếp tục game
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            craftChoicePanel.SetActive(false);
            craftingPanel.SetActive(false);
            if (dm != null) dm.EndDialogueImmediate();
            Time.timeScale = 1f; // đảm bảo game tiếp tục
        }
    }
}
*/