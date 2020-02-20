using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGate : Item
{

    public enum EGateType
    {
        AND,
        OR,
        NAND,
        NOT
    };

    [SerializeField]
    private EGateType gateType = EGateType.AND;

    [SerializeField]
    private GameObject _prefab = null;

    public GameObject prefab { get => _prefab; set => _prefab = value; }


    public bool Process(bool []input)
    {
        switch (gateType)
        {
            case EGateType.AND:
                return input[0] && input[1];
            case EGateType.OR:
                return input[0] || input[1];
            case EGateType.NAND:
                return !(input[0] && input[1]);
            case EGateType.NOT:
                return !input[0];
        }
        return false;
    }

        // Start is called before the first frame update
        void Start()
        {
        
        }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}