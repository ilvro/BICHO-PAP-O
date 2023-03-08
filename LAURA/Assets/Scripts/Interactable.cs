using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    public bool isInRange;
    public KeyCode interactKey;
    public TMP_Text interactText;
    public Transform characterTransform;
    public DialogueSystem dialogueSystem;
    public Animator animator;
    [SerializeField] public string Text;
    [SerializeField] public string dialogueText;
    void Start()
    {
        interactText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange)
        {
            // position the text at the top-center of the screen
            interactText.rectTransform.position = new Vector3(Screen.width / 2, Screen.height - 40, 0);
            if (Input.GetKeyDown(interactKey))
            {
                interactAction();
            }
        }
    }

    void interactAction()
    {
        Debug.Log("interacted!");
        animator.SetBool("isInteracting", true);
        dialogueSystem.StartTyping(dialogueText);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // player can interact with the object
            isInRange = true;
            StartCoroutine(opacityFadeIn(interactText));
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // player exited interactable area
            isInRange = false;
            animator.SetBool("isInteracting", false);
            StartCoroutine(opacityFadeOut(interactText));
        }
    }


    // text: the text object
    // fadeSpeed: speed in which the opacity fades in or out, 1.0 is slow while 10.0 is almost instantaneous, but visible
    // direction: fade in/fade out

    IEnumerator opacityFadeIn(TMP_Text text, float fadeSpeed = 7f)
    {
        Color color = text.color;
        color.a = 0;
        interactText.text = Text;

        text.gameObject.SetActive(true);
        
        while (color.a < 1)
        {
            color.a += Time.deltaTime * fadeSpeed;
            text.color = color;
            yield return null;
        }
    }

    IEnumerator opacityFadeOut(TMP_Text text, float fadeSpeed = 7f)
    {
        Color color = text.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            text.color = color;
            yield return null;
        }

        text.gameObject.SetActive(false);
    }
}
