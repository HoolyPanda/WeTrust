using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light : MonoBehaviour
{

    public string requeredClass = "";
    Color prevColor;
    public bool satisfyed = true;
    // Use this for initialization
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (satisfyed)
        {
            //GetComponent<MeshRenderer>().material.color=Color.clear;
        }
        else
        {
            //GetComponent<MeshRenderer>().material.color=prevColor;
        }
    }

    public void Reset()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
        prevColor = GetComponent<MeshRenderer>().material.color;
        satisfyed = true;
    }

    public void SetLight(string clss)
    {
        requeredClass = clss;
        switch (clss)
        {
            case "hunter":
                GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case "trader":
                GetComponent<MeshRenderer>().material.color = Color.yellow;
                break;
            case "gatherer":
                GetComponent<MeshRenderer>().material.color = Color.green;
                break;
        }
        satisfyed = false;
    }
}
