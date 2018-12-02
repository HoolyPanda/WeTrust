using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light : MonoBehaviour {

    public string requeredClass="";
    public bool satisfyed=true;
    // Use this for initialization
    void Start () 
    {

    }
    
    // Update is called once per frame
    void Update () 
    {
    }
    public void Reset()
    {
        GetComponent<MeshRenderer>().material.color=Color.white;
        satisfyed=true;
    }
}
