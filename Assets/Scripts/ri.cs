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
    public bool reachingTarget=false;
    private Vector3 target = new Vector3(0, 0, 0);
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
            Touch touch =Input.GetTouch(0);
            if (touch.phase==TouchPhase.Began)
            {
                ray= Camera.main.ScreenPointToRay(new Vector3(touch.position.x,touch.position.y,0));
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject==this.gameObject)
                    {
                        gameObject.transform.Find("Body").GetComponent<MeshRenderer>().material.color=Color.magenta;
                        grabbed = true;
                        onGround = false;
                        Unfreeze();
                    }
                }
            }
            if ((touch.phase==TouchPhase.Moved || touch.phase==TouchPhase.Stationary)&&grabbed)
            {
                ray= Camera.main.ScreenPointToRay(new Vector3(touch.position.x,touch.position.y,0));
                if (Physics.Raycast(ray, out hit))
                {
                    target.x= hit.point.x;
                    target.y= 0.03000009f;
                    target.z= hit.point.z;
                    if (hit.collider.isTrigger)
                    {
                        print(hit.collider.name);
                        target.x= hit.collider.gameObject.transform.position.x;
                        target.y= 0.03000009f;
                        target.z= hit.collider.gameObject.transform.position.z;
                        //target=hit.collider.gameObject.transform.position;
                    }
                }
            }
            if(touch.phase==TouchPhase.Ended && grabbed)
            {
                grabbed = false;
                reachingTarget=true;
                gameObject.transform.Find("Body").GetComponent<MeshRenderer>().material.color=Color.white;
            }
            if (grabbed)
            {
                //ray= Camera.main.ScreenPointToRay(new Vector3(touch.position.x,touch.position.y,0));
                //if (Physics.Raycast(ray, out hit))
                //{
                    //target.z = hit.point.z;
                    //target.x = hit.point.x;
                    //transform.position =Vector3.MoveTowards(transform.position,new Vector3(target.x, transform.position.y, target.z),Time.deltaTime); 
                    
                //}
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
        ReachPoint();
    }
    void RunCircle()
    {
        if (onGround&&!onGate)
        {
            if (circleIteration < 36000)
            {
                Vector3 circleTarget=new Vector3(totem.transform.position. x+ (float)Math.Cos(circleIteration/100f)*circleRadius,transform.position.y, totem.transform.position.z+(float)(Math.Sin(circleIteration/100f)*circleRadius));
                if(Math.Abs(circleTarget.x-transform.position.x)+Math.Abs(circleTarget.z-transform.position.z)>=0.25)
                {
                    //this will detect if human didnt take is positiion in circle
                    gameObject.transform.Find("Mask").GetComponent<MeshRenderer>().material.color=Color.black;
                }else
                {
                    //if human took his position
                    gameObject.transform.Find("Mask").GetComponent<MeshRenderer>().material.color=Color.white;
                }
                transform.position=Vector3.MoveTowards(transform.position,circleTarget,Time.deltaTime);
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
                gameObject.transform.Find("Mask").GetComponent<MeshRenderer>().material.color=Color.red;
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
                gameObject.transform.Find("Mask").GetComponent<MeshRenderer>().material.color=Color.green;
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
            onGate=false;
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
    void Unfreeze()
    {
        circleIteration= (int)UnityEngine.Random.Range(0,3600);
        onGate=false;
    }
    void ReachPoint()
    {
        if(!transform.position.Equals(target) && reachingTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position,new Vector3(target.x, transform.position.y, target.z),Time.deltaTime); 
            if (transform.position.x==target.x&&transform.position.y==target.y&&transform.position.z==target.z)
            {
                print("reaced target");
                reachingTarget=false;
                onGround=true;
                circleIteration= (int)UnityEngine.Random.Range(0,3600);
                //Invoke("GetOnGround()",3f);
            }
        }
        else
        {
            if (transform.position==target)
            {
                print("reaced target");
                reachingTarget=false;
                onGround=true;
                circleIteration= (int)UnityEngine.Random.Range(0,3600);
                //Invoke("GetOnGround()",3f);
            }
        }
    }
    void GetOnGround()
    {
        onGround=true;
        circleIteration= (int)UnityEngine.Random.Range(0,3600);
    }
}