using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesScr : MonoBehaviour {
    public GameObject tribe;
    
    void Start () {
    }
    
    // Update is called once per frame
    void Update () {
        GameObject.Find("GoldAmount").GetComponent<Text>().text=tribe.GetComponent<Tribe>().Gold.ToString();
        GameObject.Find("MeatAmount").GetComponent<Text>().text=tribe.GetComponent<Tribe>().Meat.ToString();
        GameObject.Find("WoodAmount").GetComponent<Text>().text=tribe.GetComponent<Tribe>().Wood.ToString();
    }
}
