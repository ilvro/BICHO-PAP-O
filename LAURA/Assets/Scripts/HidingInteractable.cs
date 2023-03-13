using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HidingInteractable : MonoBehaviour
{
    private bool isInRange;
    public KeyCode interactKey;
    public TMP_Text interactText;
    public Transform characterTransform;
    public HidingSystem hidingSystem;
    private Animator playerAnimator;
    [SerializeField] public GameObject playerObject;
    [SerializeField] public string displayText;
    void Start()
    {
        interactText.gameObject.SetActive(false);
        playerAnimator = playerObject.GetComponent<Animator>();
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
        //playerAnimator.SetBool("isHiding", true);
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
            playerAnimator.SetBool("isInteracting", false);
            StartCoroutine(opacityFadeOut(interactText));
        }
    }

    IEnumerator opacityFadeIn(TMP_Text text, float fadeSpeed = 7f)
    {
        Color color = text.color;
        color.a = 0;
        interactText.text = displayText;

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
