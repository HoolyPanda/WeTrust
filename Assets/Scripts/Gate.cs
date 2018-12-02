using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {
    public string neededClass=null;
    public GameObject tribe;
    public GameObject simpleHuman;
    public GameObject totem;
    public GameObject gate;
    public List<GameObject> caravan= new List<GameObject>();
    public List<GameObject> positions=new List<GameObject>();
    bool[] satisfyed = new bool[5];
    int neededHunters;
    int neededTraders;
    int neededGatheres;
    public int expeditionsToPass;
    public int goneExpeditions=0;
    public List<GameObject> ligths = new List<GameObject>();
    void Start () 
    {
        ligths.Add(GameObject.Find(name+"/gate/Gate_L1"));
        ligths.Add(GameObject.Find(name+"/gate/Gate_L2"));
        ligths.Add(GameObject.Find(name+"/gate/Gate_L3"));
        ligths.Add(GameObject.Find(name+"/gate/Gate_L4"));
        ligths.Add(GameObject.Find(name+"/gate/Gate_L5")); 
        positions.Add(GameObject.Find(name+"/point"));
        positions.Add(GameObject.Find(name+"/point (1)"));
        positions.Add(GameObject.Find(name+"/point (2)"));
        positions.Add(GameObject.Find(name+"/point (3)"));
        positions.Add(GameObject.Find(name+"/point (4)"));
        expeditionsToPass=Random.Range(5,15);
        foreach (GameObject light in ligths)
        {
            light.GetComponent<light>().Reset();
        }
    }
    void Update () 
    {
        if(Input.touchCount>0)
        {
            Touch touch =Input.GetTouch(0);
            if (touch.phase==TouchPhase.Began)
            {
                Ray ray= Camera.main.ScreenPointToRay(new Vector3(touch.position.x,touch.position.y,0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out  hit))
                {
                    if (hit.collider.gameObject.Equals(gameObject))
                    {
                        SendCaravan();
                    }
                }
            }
        }
    }
    public void SendCaravan()
    {
        if(caravan.Count>0 && CheckQuest())
        {
            foreach(GameObject light in ligths)
            {
                light.GetComponent<light>().Reset();
            }
            foreach (GameObject human in caravan)
            {
                if(!human.Equals(null))
                {
                    human.GetComponent<ri>().onGate=true;
                    human.transform.Find("Body").GetComponent<MeshRenderer>().enabled=false;
                    human.transform.Find("Mask").GetComponent<MeshRenderer>().enabled=false;
                }
            }
            Invoke("ReturnCaravan",5f);
        }
    }
    void ReturnCaravan()
    {
        foreach (GameObject human in caravan)
        {
            if(!human.Equals(null))
            {
                if (human.GetComponent<ri>().humanClass!=neededClass&&neededClass!=null)
                    {
                        if(Random.value<0.5)
                        {
                            human.GetComponent<ri>().alive=false;
                        }
                    }
                human.GetComponent<ri>().reachingTarget=false;
                human.GetComponent<ri>().onGround=true;
                human.GetComponent<ri>().onGate=false;
                human.GetComponent<ri>().toGate=false;
                human.transform.Find("Body").GetComponent<MeshRenderer>().enabled=true;
                human.transform.Find("Mask").GetComponent<MeshRenderer>().enabled=true;
                human.GetComponent<ri>().DoPlusResourses();
            }
        }
        GenerateEvent();
        goneExpeditions++;
        CheckIncomingHuman();
    }
    void GenerateEvent()
    {
        //min(5,hunters)
        neededHunters= (int)Random.Range(0,tribe.GetComponent<Tribe>().totalHunters);
        if(neededHunters>=5)
        {
            Debug.Log("Hreroll");
            neededHunters=(int)Random.Range(0,5);
        }
        neededTraders= (int)Random.Range(0,tribe.GetComponent<Tribe>().totalTraders);
        if(neededTraders>=5-neededHunters)
        {
            Debug.Log("Treroll");
            neededTraders=(int)Random.Range(0,5-neededHunters);
        }
        neededGatheres= (int)Random.Range(0,tribe.GetComponent<Tribe>().totalGatherers);
        if(neededGatheres>=5-neededHunters-neededTraders)
        {
            Debug.Log("Greroll");
            neededGatheres=(int)Random.Range(0,5-neededHunters-neededTraders);
        }
        int i=0;
        //UBO
        for (int j=i;j<neededHunters;j++)
        {
            if(neededHunters!=0)
            {
                ligths[i].GetComponent<light>().requeredClass="hunter";
                ligths[i].GetComponent<light>().satisfyed=false;
                ligths[i].GetComponent<MeshRenderer>().material.color=Color.red;
                i++;
            }
        }
        //j=i;//check
        for (int j=i;j<neededTraders;j++)
        {
            if(neededTraders!=0)
            {
                ligths[i].GetComponent<light>().requeredClass="trader";
                ligths[i].GetComponent<light>().satisfyed=false;
                ligths[i].GetComponent<MeshRenderer>().material.color=Color.yellow;
                i++;
            }
        }
        for (int j=i;j<neededGatheres;j++)
        {
            if (neededGatheres!=0)
            {
                ligths[i].GetComponent<light>().requeredClass="gatherer";
                ligths[i].GetComponent<light>().satisfyed=false;
                ligths[i].GetComponent<MeshRenderer>().material.color=Color.green;
                i++;
            }
        }
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
                //print("Wood overlaoded");;
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
            expeditionsToPass=Random.Range(5,15);
            CreateNewHuman();
        }
    }
    void CreateNewHuman()
    {
        GameObject newHuman = Instantiate(simpleHuman,new Vector3(-4.6f,0.02043986f,-2.72f),Quaternion.identity);
        newHuman.GetComponent<ri>().totem=totem;
        newHuman.GetComponent<ri>().tribe=tribe;
        tribe.GetComponent<Tribe>().Humans.Add(newHuman);
    }
    bool CheckQuest()
    {
        foreach (GameObject human in caravan)
        {
            foreach (var light in ligths)
            {
                if (light.GetComponent<light>().requeredClass==human.GetComponent<ri>().humanClass&&!light.GetComponent<light>().satisfyed)
                {
                    light.GetComponent<light>().satisfyed=true;
                }
            }
        }
        foreach (var light in ligths)
        {
            if (!light.GetComponent<light>().satisfyed)
            {
                return false;
            }
        }
        return true;
    }
    public void AddNewHuman(GameObject newCaravaner)
    {
        caravan.Add(newCaravaner);
        newCaravaner.GetComponent<ri>().SetTarget(positions[caravan.IndexOf(newCaravaner)].transform.position);
    }
}
