using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public TMP_Text dialogueText;
    public Transform characterTransform;
    private bool isDialogActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine = null; // keep track of the typing coroutine so it can be stopped if necessary
    [SerializeField] private float typingSpeed = 0.02f;
    [SerializeField] public string queuedDialog;

    void Start()
    {
        dialogueText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(true);
        dialogueText.text = ""; // necessary
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isTyping)
        {
            if (isDialogActive)
            {
                EndTyping();
            }
        }
        
        if (isDialogActive)
        {
            dialogueText.rectTransform.position = Camera.main.WorldToScreenPoint(characterTransform.position) + new Vector3(0, 80, 0);
        }

        if (Input.GetKeyDown(KeyCode.R) && isTyping) // check for skip input while typing
        {
            if (typingCoroutine != null) // check if coroutine is running
            {
                StopCoroutine(typingCoroutine); // stop coroutine
                dialogueText.text = queuedDialog; // set text to full dialog
                isTyping = false; // set typing flag to false
            }
        }
    }

    public void StartTyping(string dialog)
    {
        dialogueText.gameObject.SetActive(true);
        isDialogActive = true;

        typingCoroutine = StartCoroutine(TypeText(dialog));
    }

    void EndTyping()
    {
        dialogueText.gameObject.SetActive(false);
        dialogueText.text = "";
        isDialogActive = false;

        if (typingCoroutine != null) // stop coroutine if it is running
        {
            StopCoroutine(typingCoroutine);
            isTyping = false;
        }
    }

    IEnumerator TypeText(string dialog)
    {
        isTyping = true;
        dialogueText.text = ""; // reset text

        for (int i = 0; i < dialog.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Player pressed R key, skip to the end of the dialogue
                dialogueText.text = dialog;
                isTyping = false;
                yield break; // exit coroutine
            }

            if (dialog[i] == '<') // check for color tag
            {
                int endIndex = dialog.IndexOf('>', i); // find the end of the color tag
                if (endIndex != -1)
                {
                    i = endIndex;
                }
            }
            else // normal character
            {
                dialogueText.text += dialog[i];
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        isTyping = false;
    }
}