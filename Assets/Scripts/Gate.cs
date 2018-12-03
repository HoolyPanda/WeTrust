using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    int neededHunters;
    int neededTraders;
    int neededGatheres;
    public int minusFaith;
    public int expeditionsToPass;
    public int goneExpeditions = 0;
    public string neededClass = null;

    public GameObject gate;
    public GameObject tribe;
    public GameObject totem;
    public GameObject simpleHuman;
    public Material gateClassColor;

    public List<string> quest = new List<string>();
    public List<GameObject> ligths = new List<GameObject>();
    public List<GameObject> caravan = new List<GameObject>();
    public List<GameObject> positions = new List<GameObject>();

    bool[] satisfyed = new bool[5];
    
    void Start()
    {
        ligths.Add(GameObject.Find(name + "/gate/Gate_L1"));
        ligths.Add(GameObject.Find(name + "/gate/Gate_L2"));
        ligths.Add(GameObject.Find(name + "/gate/Gate_L3"));
        ligths.Add(GameObject.Find(name + "/gate/Gate_L4"));
        ligths.Add(GameObject.Find(name + "/gate/Gate_L5"));

        positions.Add(GameObject.Find(name + "/point"));
        positions.Add(GameObject.Find(name + "/point (1)"));
        positions.Add(GameObject.Find(name + "/point (2)"));
        positions.Add(GameObject.Find(name + "/point (3)"));
        positions.Add(GameObject.Find(name + "/point (4)"));

        expeditionsToPass = Random.Range(5, 15);

        foreach (GameObject light in ligths)
        {
            light.GetComponent<light>().Reset();
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
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
        if (caravan.Count > 0 && CheckQuest())
        {
            foreach (GameObject light in ligths)
            {
                light.GetComponent<light>().Reset();
            }
            foreach (GameObject human in caravan)
            {
                if (human != null)
                {
                    human.GetComponent<ri>().onGate = true;
                    human.transform.Find("Body").GetComponent<MeshRenderer>().enabled = false;
                    human.transform.Find("Mask").GetComponent<MeshRenderer>().enabled = false;
                }
            }
            tribe.GetComponent<Tribe>().Faith -= minusFaith;
            quest = new List<string>();
            Invoke("ReturnCaravan", 5f);
        }
    }

    void ReturnCaravan()
    {
        foreach (GameObject human in caravan)
        {
            if (human != null)
            {
                if (human.GetComponent<ri>().humanClass != neededClass && neededClass != null)
                {
                    if (Random.value < 0.5)
                    {
                        human.GetComponent<ri>().alive = false;
                    }
                }
                human.GetComponent<ri>().reachingTarget = false;
                human.GetComponent<ri>().onGround = true;
                human.GetComponent<ri>().onGate = false;
                human.GetComponent<ri>().toGate = false;
                human.transform.Find("Body").GetComponent<MeshRenderer>().enabled = true;
                human.transform.Find("Mask").GetComponent<MeshRenderer>().enabled = true;
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
        neededHunters = (int)Random.Range(0, Mathf.Min(5, tribe.GetComponent<Tribe>().totalHunters));
        neededTraders = (int)Random.Range(0, Mathf.Min(5, tribe.GetComponent<Tribe>().totalTraders));
        neededGatheres = (int)Random.Range(0, Mathf.Min(5, tribe.GetComponent<Tribe>().totalGatherers));
        for (int i = 0; i < neededHunters; i++)
        {
            quest.Add("hunter");
        }
        for (int i = 0; i < neededTraders; i++)
        {
            quest.Add("trader");
        }
        for (int i = 0; i < neededGatheres; i++)
        {
            quest.Add("gatherer");
        }
        foreach (string q in quest)
        {
            ligths[quest.IndexOf(q)].GetComponent<light>().SetLight(q);
        }
        print("Need " + neededHunters.ToString() + " Hunters " + neededGatheres.ToString() + " Gatherers " + neededTraders.ToString() + " Taders at " + gameObject.name);
    }

    List<string> __FillQuest(int h, int g, int t)//complete later for current moment dead code
    {
        List<string> nq = new List<string>();
        int iterations = h + g + t;
        for (int i = 0; i < iterations; i++)
        {
            int a = (int)Random.Range(0, 2);
            switch (a)
            {
                case 0:
                    if (h > 0)
                    {
                        nq.Add("hunter");
                        h--;
                    }
                    else if (g > 0)
                    {
                        nq.Add("gatherer");
                        g--;
                    }
                    else if (t > 0)
                    {
                        nq.Add("trader");
                        t--;
                    }
                    else
                    {
                        return nq;
                    }
                    break;
                case 1:
                    if (g > 0)
                    {
                        nq.Add("gatherer");
                        g--;
                    }
                    else if (h > 0)
                    {
                        nq.Add("hunter");
                        h--;
                    }
                    else if (t > 0)
                    {
                        nq.Add("trader");
                        t--;
                    }
                    else
                    {
                        return nq;
                    }
                    break;
                case 2:
                    if (t > 0)
                    {
                        nq.Add("trader");
                        t--;
                    }
                    else if (g > 0)
                    {
                        nq.Add("gatherer");
                        g--;
                    }
                    else if (h > 0)
                    {
                        nq.Add("hunter");
                        h--;
                    }
                    else
                    {
                        return nq;
                    }
                    break;
            }
        }
        return nq;
    }

    void CheckIncomingHuman()
    {
        int Meat = tribe.GetComponent<Tribe>().Meat;
        int Gold = tribe.GetComponent<Tribe>().Gold;
        int Wood = tribe.GetComponent<Tribe>().Wood;
        if (Mathf.Abs((float)Meat - Gold) > 20 | Mathf.Abs((float)Meat - Wood) > 20)
        {
            if (Meat > Wood)
            {
                if (Wood > Gold)
                {
                    //print("Meat ovreloaded");
                    CreateNewHuman();
                }
            }
            else if (Wood > Gold)
            {
                //print("Wood overlaoded");;
                CreateNewHuman();
            }
            else if (Gold >= Wood)
            {
                //print("Gold overladed");
                CreateNewHuman();
            }
        }
        if (goneExpeditions >= expeditionsToPass)
        {
            goneExpeditions = 0;
            expeditionsToPass = Random.Range(5, 15);
            CreateNewHuman();
        }
    }

    void CreateNewHuman()
    {
        GameObject newHuman = Instantiate(simpleHuman, new Vector3(-4.6f, 0.02043986f, -2.72f), Quaternion.identity);
        newHuman.GetComponent<ri>().totem = totem;
        newHuman.GetComponent<ri>().tribe = tribe;
        tribe.GetComponent<Tribe>().Humans.Add(newHuman);
    }

    bool CheckQuest()
    {
        foreach (var light in ligths)
        {
            int index = ligths.IndexOf(light);
            if (!ligths[index].GetComponent<light>().satisfyed)
            {
                if (caravan[index] != null)
                {
                    if (ligths[index].GetComponent<light>().requeredClass == caravan[index].GetComponent<ri>().humanClass)
                    {
                        light.GetComponent<light>().satisfyed = true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }

    void TCheck(int index)//taste later
    {
        if (!ligths[index].GetComponent<light>().satisfyed)
        {
            if (ligths[index].GetComponent<light>().requeredClass == caravan[index].GetComponent<ri>().humanClass)
            {
                ligths[index].GetComponent<light>().satisfyed = true;
            }
            else
            {
            }
        }
    }

    public void AddNewHuman(GameObject newCaravaner)
    {
        //caravan.Add(newCaravaner);
        bool placed = false;
        foreach (GameObject caravaner in caravan)
        {
            if (caravaner == null && !placed)
            {
                caravan[caravan.IndexOf(caravaner)] = newCaravaner;
                newCaravaner.GetComponent<ri>().SetTarget(positions[caravan.IndexOf(newCaravaner)].transform.position);
                placed = true;
            }
        }
        if (!placed)
        {
            caravan.Add(newCaravaner);
            newCaravaner.GetComponent<ri>().SetTarget(positions[caravan.IndexOf(newCaravaner)].transform.position);
        }
    }

    public void RemoveCaravaner(GameObject rmCaravaner)
    {
        caravan[caravan.IndexOf(rmCaravaner)] = null;
        bool isChecked = false;
        foreach (GameObject human in caravan)
        {
            if (human != null)
            {
                isChecked = true;
            }
        }
        if (!isChecked)
        {
            caravan = new List<GameObject>();
        }
    }

    public void InitializeNewHuman()
    {
        switch (neededClass)
        {
            case "trader":
                tribe.GetComponent<Tribe>().totalTraders++;
                break;
            case "hunter":
                tribe.GetComponent<Tribe>().totalHunters++;
                break;
            case "gatherer":
                tribe.GetComponent<Tribe>().totalGatherers++;
                break;
            default:
                break;
        }
    }

    public int InitMeat()
    {
        switch (neededClass)
        {
            case "hunter":
                return 10;
            default:
                return 5;
        }
    }

    public int InitGold()
    {
        switch (neededClass)
        {
            case "trader":
                return 10;
            default:
                return 5;
        }
    }

    public int InitWood()
    {
        switch (neededClass)
        {
            case "gatherer":
                return 10;
            default:
                return 5;
        }
    }
}
