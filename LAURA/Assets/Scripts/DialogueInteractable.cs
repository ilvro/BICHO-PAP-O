using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueInteractable : MonoBehaviour
{
    private bool isInRange;
    private KeyCode interactKey = KeyCode.E;
    public TMP_Text TextObject;
    public DialogueSystem dialogueSystem;
    public Animator playerAnimator;
    public GameObject playerObject;
    public Transform characterTransform;
    [SerializeField] public string displayText;
    [SerializeField] public string dialogueText;
    void Start()
    {
        // set up variables; dialogueSystem and TextObject dont work with setIfNull, will work on a fix
        //setIfNull(ref TextObject, GameObject.Find("Canvas/Interactable Text").GetComponent<TMP_Text>()); // doesnt work with setIfNull, has to be manually defined on the inspector
        dialogueSystem = FindObjectOfType<DialogueSystem>(); // doesnt work with setIfNull
        setIfNull(ref playerObject, GameObject.Find("Player"));
        setIfNull(ref characterTransform, playerObject.GetComponent<Transform>());
        setIfNull(ref displayText, "Interact [E]");
        setIfNull(ref dialogueText, "cu(bo)");
        setIfNull(ref playerAnimator, GetComponent<Animator>());

        //
        TextObject.gameObject.SetActive(false);
        playerAnimator = playerObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange)
        {
            // position the text at the top-center of the screen
            TextObject.rectTransform.position = new Vector3(Screen.width / 2, Screen.height - 40, 0);
            if (Input.GetKeyDown(interactKey))
            {
                if (!playerAnimator.GetBool("isInteracting")) // animator is also used as a storage for global variables
                {
                    interactAction();
                }
                else
                {
                    playerAnimator.SetBool("isInteracting", false);
                }
            }
        }
    }

    void interactAction()
    {
        Debug.Log("interacted!");
        playerAnimator.SetBool("isInteracting", true);
        dialogueSystem.StartTyping(dialogueText);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // player can interact with the object
            isInRange = true;
            StartCoroutine(opacityFadeIn(TextObject));
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // player exited interactable area
            isInRange = false;
            playerAnimator.SetBool("isInteracting", false);
            StartCoroutine(opacityFadeOut(TextObject));
        }
    }

    IEnumerator opacityFadeIn(TMP_Text text, float fadeSpeed = 7f)
    {
        Color color = text.color;
        color.a = 0;
        TextObject.text = displayText;

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

    private void setIfNull<T>(ref T property, T value) where T: class
    {
        if (property?.Equals(null) == true || string.IsNullOrEmpty(property.ToString()))
        {
            Debug.Log("setting property " + typeof(T));
            property = value;
        }
        else
        {
            // double check
            if (EqualityComparer<T>.Default.Equals(property, default(T)))
            {
                Debug.Log("(other method) setting property " + typeof(T));
                property = value;
            }
        }
    }
}
