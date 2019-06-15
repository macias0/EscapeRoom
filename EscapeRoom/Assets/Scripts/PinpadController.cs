using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


[System.Serializable]
public class PinAcceptedEvent : UnityEvent<string>
{
}

public class PinpadController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text display = null;

    [SerializeField]
    private int maxCharacters = 4;

    [SerializeField]
    private PinAcceptedEvent onPinAccepted = new PinAcceptedEvent();

    public void KeyPressed(string key)
    {
        if (key == "<") //back
        {
            if(display.text.Length > 0)
                display.text = display.text.Remove(display.text.Length - 1);
        }
        else if(key == ">") //OK
        {
            onPinAccepted.Invoke(display.text);
        }
        else
        {
            if(display.text.Length < maxCharacters)
                display.text += key;
        }
    }

}
