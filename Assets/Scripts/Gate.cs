using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {
    public string neededClass=null;
    public GameObject tribe;
    public GameObject simpleHuman;
    public GameObject totem;
    public List<GameObject> caravan= new List<GameObject>();
    int neededHunters;
    int neededTraders;
    int neededGatheres;
    public int expeditionsToPass;
    public int goneExpeditions=0;
    void Start () 
    {
        expeditionsToPass=Random.Range(5,15);
    }
    void Update () 
    {
    }
    public void SendCaravan()
    {
        //print(gameObject.name+" was sended");
        foreach (GameObject human in caravan)
        {
            
            human.transform.Find("Body").GetComponent<MeshRenderer>().enabled=false;
            human.transform.Find("Mask").GetComponent<MeshRenderer>().enabled=false;
            //human.GetComponent<MeshRenderer>().enabled=false;
            if (human.GetComponent<ri>().humanClass!=neededClass&&neededClass!=null)
            {
                if(Random.value<0.5)
                {
                    human.GetComponent<ri>().alive=false;
                }
            }
        }
        Invoke("ReturnCaravan",5f);
    }
    void ReturnCaravan()
    {
        foreach (GameObject human in caravan)
        {
            human.GetComponent<ri>().onGate=false;
            human.GetComponent<ri>().reachingTarget=false;
            human.GetComponent<ri>().onGround=true;
            human.transform.Find("Body").GetComponent<MeshRenderer>().enabled=true;
            human.transform.Find("Mask").GetComponent<MeshRenderer>().enabled=true;
            //human.GetComponent<MeshRenderer>().enabled=true;
            human.GetComponent<ri>().DoPlusResourses();
        }
        GenerateEvent();
        goneExpeditions++;
        CheckIncomingHuman();
    }
    void GenerateEvent()
    {
        neededHunters= (int)Random.Range(0,tribe.GetComponent<Tribe>().totalHunters);
        neededTraders= (int)Random.Range(0,tribe.GetComponent<Tribe>().totalTraders);
        neededGatheres= (int)Random.Range(0,tribe.GetComponent<Tribe>().totalGatherers);
        print ("Need "+neededHunters.ToString()+" Hunters "+neededGatheres.ToString()+" Gatherers "+neededTraders.ToString()+" Taders at "+gameObject.name);
    }
    void CheckIncomingHuman()
    {
        int Meat= tribe.GetComponent<Tribe>().Meat;
        int Gold= tribe.GetComponent<Tribe>().Gold;
        int Wood= tribe.GetComponent<Tribe>().Wood;
        if (Mathf.Abs((float)Meat-Gold)>20|Mathf.Abs((float)Meat-Wood)>20)
        {
            if(Meat>Wood)
            {
                if (Wood>Gold)
                {
                    //print("Meat ovreloaded");
                    CreateNewHuman();
                }
            }else if (Wood>Gold)
            {
                //print("Wood overlaoded");
                GameObject newHuman =Instantiate(simpleHuman,new Vector3(2f,0.26f,1.5f),Quaternion.identity);
                CreateNewHuman();
            }
            else if (Gold>=Wood)
            {
                //print("Gold overladed");
                CreateNewHuman();
            }
        }
        if (goneExpeditions>=expeditionsToPass)
        {
            goneExpeditions=0;
            CreateNewHuman();
        }
    }
    void CreateNewHuman()
    {
        GameObject newHuman =Instantiate(simpleHuman,new Vector3(2f,0.26f,2f),Quaternion.identity);
        newHuman.GetComponent<ri>().totem=totem;
        newHuman.GetComponent<ri>().tribe=tribe;
    }
}
