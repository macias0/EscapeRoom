using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

//[System.Serializable]
//public class PinAcceptedEvent : UnityEvent<string>
//{
//}
[RequireComponent(typeof(AudioSource))]
public class PinpadController : MonoBehaviour, IUsable
{
    [SerializeField]
    private TMP_Text display = null;

    [SerializeField]
    private int maxCharacters = 4;

    [SerializeField]
    //private PinAcceptedEvent onPinAccepted = new PinAcceptedEvent();
    private UnityEvent onPinAccepted = new UnityEvent();

    [SerializeField]
    private UnityEvent onPinError = new UnityEvent();


    [SerializeField]
    private PlayerController playerController = null;

    [SerializeField]
    private string pin = "";


    [SerializeField]
    private AudioClip acceptedSound = null;

    [SerializeField]
    private AudioClip errorSound = null;

    [SerializeField]
    private AudioClip clickSound = null;

    private AudioSource audioSource = null;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void KeyPressed(string key)
    {
        audioSource.clip = clickSound;

        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        if (key == "<") //back
        {
            if(display.text.Length > 0)
                display.text = display.text.Remove(display.text.Length - 1);
        }
        else if(key == ">") //OK
        {
            if (display.text == pin)
            {
                Debug.Log("PIN PRAWIDLOWY");
                onPinAccepted.Invoke();
                audioSource.clip = acceptedSound;
                GetComponent<Collider>().enabled = false;
            }
            else
            {
                onPinError.Invoke();
                audioSource.clip = errorSound;
            }
        }
        else
        {
            if(display.text.Length < maxCharacters)
                display.text += key;
        }


        audioSource.Play();

    }

    public Item Use(Item other = null)
    {
        Toggle();
        return null;
    }

    public void Toggle()
    {
        Debug.Log("PinpadController USE");
        var gr = gameObject.GetComponentInParent<GraphicRaycaster>();
        gr.enabled = !gr.enabled;
        playerController.freezed = !playerController.freezed;
        Cursor.visible = !Cursor.visible;
    }
}
