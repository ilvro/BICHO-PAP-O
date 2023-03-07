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
            StartCoroutine(opacityFadeOut(interactText));
        }
    }


    // text: the text object
    // fadeSpeed: speed in which the opacity fades in or out, 1.0 is slow while 5.0 is fast
    // direction: fade in/fade out

    IEnumerator opacityFadeIn(TMP_Text text, float fadeSpeed = 7.0f)
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

    IEnumerator opacityFadeOut(TMP_Text text, float fadeSpeed = 7.0f)
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
