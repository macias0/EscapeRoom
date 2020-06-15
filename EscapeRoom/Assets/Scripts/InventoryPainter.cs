using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPainter : MonoBehaviour
{
    [SerializeField]
    private int itemsInRow = 4;

    //[SerializeField]
    //private int itemsInColumn = 4;

    [SerializeField]
    private float margins = 5.0f;

    [SerializeField]
    private GameObject itemPrefab = null;

    [SerializeField]
    private InventoryController inventoryController = null;

    [SerializeField]
    private GameObject itemDescription = null;

    private int itemCount = 0;


    public void Clear()
    {
        StopCoroutine("TrackPointer");
        itemDescription.SetActive(false);
        foreach (Transform child in transform)
        {
            Debug.Log("InventoryPainter Clear");
            Destroy(child.gameObject);
        }
        itemCount = 0;
    }

    public void HighlightItem(GameObject go, EInventoryItemStatus status)
    {
        Image img = go.GetComponent<Image>();
        switch (status)
        {
            case EInventoryItemStatus.None:
                img.color = Color.white;
                break;
            case EInventoryItemStatus.Selected:
                img.color = Color.yellow;
                break;
            case EInventoryItemStatus.InUse:
                //if (img.color == Color.white)
                    img.color = Color.green;
                //else
                  //  img.color = Color.white;
                break;
        }

    }

    public void Paint( List<Item> inventory, Item selectedItem)
    {
        Clear();

        foreach (Item it in inventory)
        {
            
            

            GameObject go = Instantiate(itemPrefab, transform);
            RectTransform rt = go.GetComponent<RectTransform>();

            float width = rt.sizeDelta.x;
            float height = rt.sizeDelta.y;
            rt.anchoredPosition = new Vector3((itemCount % itemsInRow) * width + margins, -((int)(itemCount / itemsInRow) * height + margins), 1.0f);

            //fill with Item data

            go.GetComponentInChildren<Text>().text = it.name;
            go.transform.Find("Thumbnail").gameObject.GetComponent<Image>().sprite = it.sprite;

            if (it.active)
                HighlightItem(go, EInventoryItemStatus.InUse);

            if (selectedItem == it)
                HighlightItem(go, EInventoryItemStatus.Selected);


            go.GetComponent<Button>().onClick.AddListener(() => {
                EInventoryItemStatus status = inventoryController.ItemClicked(it);
                HighlightItem(go, status);
            });


            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((eventData) => {
                itemDescription.SetActive(true);
                itemDescription.GetComponentInChildren<TextMeshProUGUI>().text = it.description;
                itemDescription.transform.position = Input.mousePosition;
                StartCoroutine("TrackPointer", itemDescription);
            });
            go.GetComponent<EventTrigger>().triggers.Add(entry);

            //entry = new EventTrigger.Entry();
            //entry.eventID = EventTriggerType.Move;
            //entry.callback.AddListener((eventData) => {
            //    itemDescription.transform.position = Input.mousePosition;
                
            //});
            //go.GetComponent<EventTrigger>().triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerExit;
            entry.callback.AddListener((eventData) => {
                StopCoroutine("TrackPointer");
                itemDescription.SetActive(false);
            });
            go.GetComponent<EventTrigger>().triggers.Add(entry);

            itemCount++;

        }
    }


    IEnumerator TrackPointer(GameObject go)
    {
        var ray = GetComponentInParent<GraphicRaycaster>();
        var input = FindObjectOfType<StandaloneInputModule>();

        if (ray != null && input != null)
        {
            while (Application.isPlaying)
            {
                Vector2 localPos; // Mouse position  
                RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, ray.eventCamera, out localPos);

                // local pos is the mouse position.
                go.transform.position = Input.mousePosition + new Vector3(10, 10);
                yield return 0;
            }
        }
        else
            UnityEngine.Debug.LogWarning("Could not find GraphicRaycaster and/or StandaloneInputModule");
    }

}
