﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ri : MonoBehaviour
{
    public Rigidbody HumansBody;
    public GameObject currentGate = null;
    public GameObject tribe;
    public GameObject totem;
    public string humanName = "Todd";
    public int minusMeat=1;
    public int minusGold=1;
    public int minusWood=1;
    public int level=1;
    private int plusMeat;
    public int PlusMeat
    {
        get
        {
            return plusMeat*level;
        }
        set
        {
            plusMeat=value;
        }
    }
    private int plusGold;
    public int PlusGold
    {
        get
        {
            return plusGold*level;
        }
        set
        {
            plusGold=value;
        }
    }
    private int plusWood;
    public int PlusWood
    {
        get
        {
            return plusWood*level;
        }
        set
        {
            plusWood=value;
        }
    }
    public string humanClass = null;
    public bool onGate=false;
    public bool alive=true;
    private bool grabbed = false;
    bool onGround=false;
    bool gettingResourses=false;
    private Vector3 rot = new Vector3(0, 0, 0);
    int circleIteration = 0;
    Ray ray;
    Camera Camera;
    RaycastHit hit;
    void Start()
    {
        HumansBody = GetComponent<Rigidbody>();
        Camera = GetComponent<Camera>();
        humanClass=null;
    }
    void Update()
    {
        if (humanClass==null)
        {
            HumansBody.GetComponent<MeshRenderer>().sharedMaterial.color = Color.white;
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float mx;
            float mz;
            if (Physics.Raycast(ray, out hit))
            {
                float y = hit.point.y;
                mx = hit.point.x;
                mz = hit.point.z;
                if (
                    (Math.Abs(HumansBody.transform.position.x - hit.point.x) <= 0.25) &&
                    (Math.Abs(HumansBody.transform.position.z - hit.point.z) <= 0.25)
                    )
                {
                    grabbed = true;
                    Unfreeze();
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float mx;
            float mz;
            if (Physics.Raycast(ray, out hit))
            {
                float y = hit.point.y;
                mx = hit.point.x;
                mz = hit.point.z;
                if (
                    (Math.Abs(HumansBody.transform.position.x - hit.point.x) <= 0.25) &&
                    (Math.Abs(HumansBody.transform.position.z - hit.point.z) <= 0.25)
                    )
                {
                    grabbed = false;
                }
            }
        }
        if (grabbed)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                rot.z = hit.point.z;
                rot.x = hit.point.x;
                transform.position = new Vector3(rot.x, 3, rot.z);
            }
        }
        else
        {
            RunCircle();
        }
    }
    void RunCircle()
    {
        if (onGround)
        {
            if (circleIteration < 36000)
            {
                transform.position = new Vector3(totem.transform.position.x + (float)Math.Cos(circleIteration/100f),
                transform.position.y,
                totem.transform.position.z + (float)(Math.Sin(circleIteration/100f)));
            }
            else
            {
                circleIteration = 0;
            }
            circleIteration+=1;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "RightProtoGate")
        {
            onGate=true;
            collision.gameObject.GetComponent<Gate>().caravan.Add(gameObject);
            if (humanClass==null)
            {
                humanClass="trader";
                HumansBody.GetComponent<MeshRenderer>().material.color = Color.yellow;
                tribe.GetComponent<Tribe>().totalTraders++;
                PlusMeat=5;
                PlusGold=10;
                PlusWood=5;
            }
        }
        if (collision.gameObject.name == "TopProtoGate")
        {
            onGate=true;
            collision.gameObject.GetComponent<Gate>().caravan.Add(gameObject);
            if(humanClass==null)
            {
                humanClass="hunter";
                HumansBody.GetComponent<MeshRenderer>().material.color = Color.red;
                tribe.GetComponent<Tribe>().totalHunters++;
                PlusMeat=10;
                PlusGold=5;
                PlusWood=5;
            }
        }
        if (collision.gameObject.name == "LeftProtoGate")
        {
            onGate=true;
            collision.gameObject.GetComponent<Gate>().caravan.Add(gameObject);
            if(humanClass==null)
            {
                humanClass="gatherer";
                HumansBody.GetComponent<MeshRenderer>().material.color = Color.green;
                tribe.GetComponent<Tribe>().totalGatherers++;
                PlusMeat=5;
                PlusGold=5;
                PlusWood=10;
            }
        }
        if (collision.gameObject.name == "BottomProtoGate")
        {
            onGate=true;
            collision.gameObject.GetComponent<Gate>().caravan.Add(gameObject);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (true)
        {
            collision.gameObject.GetComponent<Gate>().caravan.Remove(gameObject);
            currentGate = null;
        }
        humanName = "Todd";
    }
    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.name=="Plane")&&!onGate)
        {
            Invoke("GetOnGround",3);
            gettingResourses=false;
            tribe=collision.gameObject;
            print("OnGround");
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.name=="Plane")&&!onGate)
        {
            if(!gettingResourses)
            {
                Invoke("DoMinusResourses",3);
                gettingResourses=true;
            }
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if ((collision.gameObject.name=="Plane")&&!onGate)
        {
            onGround=false;
            print("NotOnGround");
        }
    }
    void DoMinusResourses()
    {
        tribe.GetComponent<Tribe>().Meat-=minusMeat;
        tribe.GetComponent<Tribe>().Gold-=minusGold;
        tribe.GetComponent<Tribe>().Wood-=minusWood;
        gettingResourses=false;
    }
    public void DoPlusResourses()
    {
        if(tribe!=null)
        {
            tribe.GetComponent<Tribe>().Meat+=PlusMeat;
            tribe.GetComponent<Tribe>().Gold+=PlusGold;
            tribe.GetComponent<Tribe>().Wood+=PlusWood;
        }
    }
    void GetOnGround()
    {
        onGround=true;
    }
    void Unfreeze()
    {
        onGate=false;
        HumansBody.constraints=RigidbodyConstraints.None;
        HumansBody.constraints=RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationY|RigidbodyConstraints.FreezeRotationZ;
    }
}