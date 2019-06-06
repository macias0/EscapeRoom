using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPainter : MonoBehaviour
{
    [SerializeField]
    private int itemsInRow = 4;

    [SerializeField]
    private int itemsInColumn = 4;

    [SerializeField]
    private float margins = 5.0f;

    [SerializeField]
    private GameObject itemPrefab = null;

    [SerializeField]
    private InventoryController inventoryController = null;

    private int itemCount = 0;


    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Debug.Log("InventoryPainter Clear");
            Destroy(child.gameObject);
        }
        itemCount = 0;
    }

    public void ToggleItem(GameObject go)
    {
        Image img = go.GetComponent<Image>();
        if (img.color == Color.white)
        {
            img.color = Color.yellow;
        }
        else
            img.color = Color.white;
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
            rt.anchoredPosition = new Vector3((itemCount % itemsInRow) * width + margins, -((int)(itemCount / itemsInColumn) * height + margins), 1.0f);

            //fill with Item data

            go.GetComponentInChildren<Text>().text = it.name;
            go.transform.Find("Thumbnail").gameObject.GetComponent<Image>().sprite = it.sprite;

            if (selectedItem == it)
                ToggleItem(go);


            go.GetComponent<Button>().onClick.AddListener(() => {
                if(inventoryController.ItemClicked(it))
                    ToggleItem(go);
            });

            itemCount++;

        }
    }

}
