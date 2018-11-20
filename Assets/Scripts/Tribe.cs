using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tribe : MonoBehaviour {

    public int Meat= 10;
    public int Gold=10;
    public int Wood=10;
    public int totalHunters=0;
    public int totalTraders=0;
    public int totalGatherers=0;
    public bool overloaded=false;
    public GameObject ui;
    
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        CheckResourses();
    }
    void CheckResourses()
    {
        if (Meat<=0)
        {
            print("low on meat");
        }
        if (Gold<=0)
        {
            print("low on gold");
        }
        if (Wood<=0)
        {
            print("low on wood");
        }
        if(totalGatherers+totalHunters+totalTraders>=15)
        {
            Won();
        }
    }
    void Won()
    {
        ui.GetComponent<Text>().enabled=true;
    }
}
