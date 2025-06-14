using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

    public class NPCInteraction : MonoBehaviour
{
        public enum InteractionType { OneSentence }

        [Header("Settings")]
        [SerializeField] private Transform player;
        [SerializeField] private InteractionType interactionType;

        [Header("OneSentence")]
        [SerializeField] private GameObject oneSentencePanel;
        [SerializeField] private TMP_Text dialogueComponent;
        [SerializeField] private string dialogueLine;

        [SerializeField] private Animator animator;

        private bool isPlayerNearNPC = false;

    private void Start()
    {
        oneSentencePanel.SetActive(false);
    }


    // Update is called once per frame
    void Update()
        {
            if (player == null) return;

        

            if ((oneSentencePanel.activeSelf) && Input.GetKeyDown(KeyCode.Escape))
            {
                oneSentencePanel.SetActive(false);
        }

            if (oneSentencePanel.activeSelf) return;

            if (isPlayerNearNPC && Input.GetKeyDown(KeyCode.E))
            {
                switch(interactionType)
                {
                    case InteractionType.OneSentence:
                        dialogueComponent.text = dialogueLine;
                        oneSentencePanel.SetActive(true);
                        break;
                }
            }

        if (oneSentencePanel.activeSelf)
            InteractionPrompt.Instance.HidePrompt();
        else if (isPlayerNearNPC)
            InteractionPrompt.Instance.ShowPrompt();

    }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;

            isPlayerNearNPC = true;
            InteractionPrompt.Instance.ShowPrompt();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;

            isPlayerNearNPC = false;
            InteractionPrompt.Instance.HidePrompt();
            oneSentencePanel.SetActive(false);

        if (oneSentencePanel != null) oneSentencePanel.SetActive(false);
        }


    

}
