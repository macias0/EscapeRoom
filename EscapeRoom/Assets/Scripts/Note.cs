using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Note : Item
{
    [SerializeField]
    private string _content = "";
    public string content { get => _content;  }

    GameObject model;

    [SerializeField]
    private TextMeshProUGUI text;

    public void Awake()
    {
        text.text = content;
    }

    public override Item Use(Item other)
    {
        if (other)
            return null;
        else
        {
            if (active)
            {
                Destroy(model);
            }
            else
            {
                GameObject prefab = Resources.Load("Prefabs/Clipboard") as GameObject;
                model = Instantiate(prefab, PlayerController.mainPlayer.transform);
                model.GetComponentInChildren<TextMeshProUGUI>().text = content;
            }
            active = !active;
        }

        return active ? this : null;

    }


}
