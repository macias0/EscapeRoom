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
    private PlayerController playerController = null;

    [SerializeField]
    private string pin = "";

    public void KeyPressed(string key)
    {
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
            }
        }
        else
        {
            if(display.text.Length < maxCharacters)
                display.text += key;
        }
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
