using UnityEngine;






[System.Serializable]
abstract public class Item : ScriptableObject, IUsable
{
    [SerializeField]
    private string _name = "";
    
    public new string name { get => _name;  }

    [SerializeField]
    private string _description = "";

    public string description { get => _description; protected set => _description = value; }

    [SerializeField]
    private Sprite _sprite = null;

    public Sprite sprite { get => _sprite; protected set => _sprite = value; }

    [SerializeField]
    protected bool _combinable = false;

    public bool combinable { get => _combinable; protected set => _combinable = value; }

    [SerializeField]
    protected bool _active = false;
    public bool active { get => _active; protected set => _active = value; }


    //public abstract bool Use();

    public override string ToString()
    {
        return "ITEM: " + name;
        
    }

    public void OnDestroy()
    {
        Debug.Log("Destroy Item: " + name);
    }



    public virtual Item Use(Item other = null)
    {
        Debug.Log("Nic sie nie stalo");
        return null;
    }

    //    public void PickUp()
    //    {
    //        gameObject.SetActive(false);
    //    }
}



