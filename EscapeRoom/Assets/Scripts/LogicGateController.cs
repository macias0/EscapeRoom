using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class LogicGateController : MonoBehaviour, IUsable
{
    [SerializeField]
    private bool fixedGate = false;

    [SerializeField]
    private LogicGate gate;

    [SerializeField]
    private bool[] input;

    private bool output { get =>  gate.Process(input); }

    [SerializeField]
    private LogicGateController next = null;

    [SerializeField]
    private int nextInputIndex;

    [SerializeField]
    private GameObject[] outputWires;

    [SerializeField]
    private Vector3 gatePosition;

    private GameObject gateModel = null;

    [SerializeField]
    private InventoryController inventoryController;

    [SerializeField]
    private UnityEvent onOutputTrue;

    [SerializeField]
    private UnityEvent onOutputFalse;

    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnValidate()
    {
        if (fixedGate && gate == null)
            gate = ScriptableObject.CreateInstance<LogicGate>();
        else if(!fixedGate)
            gate = null;
    }

    public Item Use(Item other = null)
    {
        if(gate == null && other != null && other.GetType() == typeof(LogicGate))
        {
            Debug.Log("LogicGateController USE");

            gate = other as LogicGate;

            gateModel = GameObject.Instantiate(gate.prefab, gameObject.transform);

            if (audioSource != null)
                audioSource.Play();

            //update inputs
            UpdateInput(input[0], 0);
            
            Debug.Log("LogicGateController USE END");
            return other;
        }
        else if(other == null && gate != null && inventoryController != null)
        {
            inventoryController.inventory.AddItem(gate);
            gate = null;
            UpdateWires(false);
            next.UpdateInput(false, nextInputIndex);
            Destroy(gateModel);
            gateModel = null;
        }
        return null;
    }

    public void UpdateInput(bool value, int index)
    {
        Debug.Log("UpdateInput obj " + gameObject.name);

        //update input property
        input[index] = value;


        UpdateWires(output);

        //update next element input
        if (next != null)
            next.UpdateInput(output, nextInputIndex);


        if (output)
            onOutputTrue.Invoke();
        else
            onOutputFalse.Invoke();



        Debug.Log("END UpdateInput obj " + gameObject.name);

    }


    void UpdateWires(bool signal)
    {
        //update materials of wires
        foreach (GameObject go in outputWires)
        {
            if (signal)
                go.GetComponent<MeshRenderer>().material = GetTrueMaterial();
            else
                go.GetComponent<MeshRenderer>().material = GetFalseMaterial();
        }
    }

    static Material trueMaterial = null;

    static Material GetTrueMaterial()
    {
        if(trueMaterial == null)
            trueMaterial = Resources.Load("Materials/Green") as Material;
        return trueMaterial;
    }

    static Material falseMaterial = null;

    static Material GetFalseMaterial()
    {
        if (falseMaterial == null)
            falseMaterial = Resources.Load("Materials/Red") as Material;
        return falseMaterial;
    }
}
