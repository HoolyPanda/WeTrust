using System.Collections;
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
    public bool grabbed = false;
    public bool onGround=false;
    bool gettingResourses=false;
    private Vector3 rot = new Vector3(0, 0, 0);
    int circleIteration = 0;
    float circleRadius=1.5f;
    Ray ray;
    Camera Camera;
    RaycastHit hit;
    void Start()
    {
        HumansBody = GetComponent<Rigidbody>();
        Camera = GetComponent<Camera>();
        humanClass=null;
        gameObject.transform.Find("Body").GetComponent<MeshRenderer>().material.color=Color.white;
    }
    void Update()
    {
    }
    private void FixedUpdate()
    {
        if(Input.touchCount>0)
        {
            if (Input.GetTouch(0).phase==TouchPhase.Began)
            {
                ray= Camera.main.ScreenPointToRay(new Vector3(Input.GetTouch(0).position.x,Input.GetTouch(0).position.y,0));
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
                        onGround=false;
                        Unfreeze();
                    }
                }
            }
            if (Input.GetTouch(0).phase==TouchPhase.Ended)
            {
                ray= Camera.main.ScreenPointToRay(new Vector3(Input.GetTouch(0).position.x,Input.GetTouch(0).position.y,0));
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
                        Invoke("GetOnGround()",3f);
                    }
                }
            }
            if (grabbed)
            {
                ray= Camera.main.ScreenPointToRay(new Vector3(Input.GetTouch(0).position.x,Input.GetTouch(0).position.y,0));
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
                Vector3 target=new Vector3(totem.transform.position. x+ (float)Math.Cos(circleIteration/100f)*circleRadius,transform.position.y, totem.transform.position.z+(float)(Math.Sin(circleIteration/100f)*circleRadius));
                if(Math.Abs(target.x-transform.position.x)+Math.Abs(target.z-transform.position.z)>=0.25)
                {
                    //this will detect if human didnt take is positiion in circle
                    gameObject.transform.Find("Mask").GetComponent<MeshRenderer>().material.color=Color.black;
                }else
                {
                    //if human took his position
                    gameObject.transform.Find("Mask").GetComponent<MeshRenderer>().material.color=Color.white;
                }
                transform.position=Vector3.MoveTowards(transform.position,target,Time.deltaTime);
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
                gameObject.transform.Find("Mask").GetComponent<MeshRenderer>().material.color=Color.yellow;
                tribe.GetComponent<Tribe>().totalTraders++;
                PlusGold=10;
                PlusMeat=5;
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
                gameObject.transform.Find("Body").GetComponent<MeshRenderer>().material.color=Color.red;
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
                gameObject.transform.Find("Body").GetComponent<MeshRenderer>().material.color=Color.green;
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
            Invoke("GetOnGround",3f);
            gettingResourses=false;
            tribe=collision.gameObject;
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.name=="Plane")&&!onGate)
        {
            if(!gettingResourses)
            {
                Invoke("DoMinusResourses",3f);
                gettingResourses=true;
            }
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if ((collision.gameObject.name=="Plane")&&!onGate)
        {
            onGround=false;
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
        circleIteration= (int)UnityEngine.Random.Range(0,3600);
    }
    void Unfreeze()
    {
        //GetOnGround();
        circleIteration= (int)UnityEngine.Random.Range(0,3600);
        onGate=false;
        //HumansBody.constraints=RigidbodyConstraints.None;
        //HumansBody.constraints=RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationY|RigidbodyConstraints.FreezeRotationZ;
    }
}