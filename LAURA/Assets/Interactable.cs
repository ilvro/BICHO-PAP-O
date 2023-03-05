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
    void Start()
    {
        interactText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange)
        {
            interactText.rectTransform.position = Camera.main.WorldToScreenPoint(characterTransform.position) + new Vector3(0, 80, 0);
            if (Input.GetKeyDown(interactKey))
            {
                interactAction();
            }
        }
    }

    void interactAction()
    {
        Debug.Log("interacted!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // player can interact with the object
            isInRange = true;

            // make the gui visible
            interactText.text = $"Interact [{interactKey}]";
            interactText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // player exited interactable area
            isInRange = false;

            // hide the gui
            interactText.gameObject.SetActive(false);
        }
    }
}
