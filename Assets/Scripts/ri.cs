using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ri : MonoBehaviour
{
    public Rigidbody HumansBody;
    public GameObject currentGate = null;
    public GameObject tribe;
    public GameObject totem;

    public string humanName = "Todd";
    public int minusMeat = 1;
    public int minusGold = 1;
    public int minusWood = 1;
    public int plusFaith = 5;
    public int level = 1;
    public int plusMeat;
    private int plusGold;
    private int plusWood;

    public string humanClass = null;
    public bool onGate = false;
    public bool alive = true;
    public bool grabbed = false;
    public bool onGround = false;
    public bool fromTotem = false;
    bool gettingResourses = false;
    public bool reachingTarget = false;
    private Vector3 target = new Vector3(0, 0, 0);
    public int fu;
    public bool toGate = false;
    Ray ray;
    Camera Camera;
    RaycastHit hit;

    public int PlusMeat
    {
        get
        {
            return plusMeat * level;
        }
        set
        {
            plusMeat = value;
        }
    }

    public int PlusGold
    {
        get
        {
            return plusGold * level;
        }
        set
        {
            plusGold = value;
        }
    }

    public int PlusWood
    {
        get
        {
            return plusWood * level;
        }
        set
        {
            plusWood = value;
        }
    }
    
    void Start()
    {
        HumansBody = GetComponent<Rigidbody>();
        Camera = GetComponent<Camera>();
        humanClass = null;
        gameObject.transform.Find("Body").GetComponent<MeshRenderer>().material.color = Color.white;
        fu = tribe.GetComponent<Tribe>().Humans.IndexOf(this.gameObject);
        Unfreeze();
    }

    void Update()
    {
        if (!alive)
        {
            Invoke("Death", 0f);
        }
    }

    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        gameObject.transform.Find("Body").GetComponent<MeshRenderer>().material.color = Color.magenta;
                        grabbed = true;
                        onGround = false;
                        foreach (GameObject human in tribe.GetComponent<Tribe>().Humans)
                        {
                            human.GetComponent<ri>().onGround = false;
                        }
                        totem.GetComponent<Totem>().Freeze();
                    }
                }
            }
            if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && grabbed)
            {
                ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));
                if (Physics.Raycast(ray, out hit))
                {
                    //target.x = hit.point.x;
                    //target.y = transform.position.y;
                    //target.z = hit.point.z;
                    if (hit.collider.isTrigger)
                    {
                        //target.x = hit.collider.gameObject.transform.position.x;
                        //target.y = transform.position.y;
                        //target.z = hit.collider.gameObject.transform.position.z;
                        //target=hit.collider.gameObject.transform.position;
                    }
                }
            }
            if (touch.phase == TouchPhase.Ended && grabbed)
            {
                ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));
                if (Physics.Raycast(ray, out hit))
                {
                    //target.x= hit.point.x;
                    //target.y= transform.position.y;
                    //target.z= hit.point.z;
                    if (hit.collider.gameObject.Equals(GameObject.Find("LeftProtoGate")))
                    {
                        currentGate = GameObject.Find("LeftProtoGate");
                        Vector3 t = GameObject.Find("BottomLeftPoint").transform.position;
                        target = t;
                        if (!fromTotem)
                        {
                            target = GameObject.Find("LeftProtoGate").transform.position;
                            currentGate = null;
                        }
                        toGate = true;
                        //totem.GetComponent<Totem>().Unfreeze();
                    }
                    else
                    {

                    }
                    if (hit.collider.gameObject.Equals(GameObject.Find("RightProtoGate")))
                    {
                        currentGate = GameObject.Find("RightProtoGate");
                        Vector3 t = GameObject.Find("BottomRightPoint").transform.position;
                        target = t;
                        if (!fromTotem)
                        {
                            target = GameObject.Find("RightProtoGate").transform.position;
                            currentGate = null;
                        }
                        toGate = true;
                        //totem.GetComponent<Totem>().Unfreeze();
                    }
                    if (hit.collider.gameObject.Equals(GameObject.Find("TopProtoGate")))
                    {
                        currentGate = GameObject.Find("TopProtoGate");
                        Vector3 t = GameObject.Find("TopProtoGate").transform.position;
                        target = t;
                        if (!fromTotem)
                        {
                            target = GameObject.Find("TopProtoGate").transform.position;
                            currentGate = null;
                        }
                        toGate = true;
                        //Invoke()
                        //totem.GetComponent<Totem>().Unfreeze();
                    }
                    if (hit.collider.gameObject.Equals(GameObject.Find("Totem")))
                    {
                        //Invoke()
                        toGate = false;
                        reachingTarget = false;
                        onGround = true;
                        toGate = false;
                        //totem.GetComponent<Totem>().Unfreeze();
                    }
                    if (hit.collider.gameObject.Equals(GameObject.Find("Plane")))
                    {
                        onGate = false;
                        reachingTarget = false;
                        onGround = true;
                        toGate = false;
                        grabbed = false;
                        target = transform.position;
                        //totem.GetComponent<Totem>().Unfreeze();
                    }
                    totem.GetComponent<Totem>().Unfreeze();
                }
                grabbed = false;
                reachingTarget = true;
                gameObject.transform.Find("Body").GetComponent<MeshRenderer>().material.color = Color.white;
                foreach (GameObject human in tribe.GetComponent<Tribe>().Humans)
                {
                    if (!human.Equals(gameObject) && !human.GetComponent<ri>().toGate)
                    {
                        human.GetComponent<ri>().onGround = true;
                    }
                }
            }
            if (!grabbed)
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
        if (onGround && !onGate)
        {
            if (!gettingResourses)
            {
                Invoke("DoMinusResourses", 3f);
                gettingResourses = true;
            }
            target = transform.position;
            transform.position = Vector3.LerpUnclamped(transform.position, totem.GetComponent<Totem>().points[fu].GetComponent<point>().transform.position, Time.deltaTime);
            if (Mathf.Abs(transform.position.x - totem.GetComponent<Totem>().points[fu].GetComponent<point>().transform.position.x) <= 1.1 && Mathf.Abs(transform.position.z - totem.GetComponent<Totem>().points[fu].GetComponent<point>().transform.position.z) <= 0.1)
            {
                fromTotem = true;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Gate")
        {
            fromTotem = false;
            onGate = true;
            collision.gameObject.GetComponent<Gate>().AddNewHuman(gameObject);
            if (humanClass == null || humanClass == "")
            {
                humanClass = collision.GetComponent<Gate>().neededClass;
                gameObject.transform.Find("Mask").GetComponent<MeshRenderer>().material = collision.GetComponent<Gate>().gateClassColor;
                collision.GetComponent<Gate>().InitializeNewHuman();
                plusGold = collision.GetComponent<Gate>().InitGold();
                plusMeat = collision.GetComponent<Gate>().InitMeat();
                plusWood = collision.GetComponent<Gate>().InitWood();
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (true)
        {
            collision.gameObject.GetComponent<Gate>().RemoveCaravaner(gameObject);
            currentGate = null;
            onGate = false;
        }
        humanName = "Todd";
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.name == "Plane") && !onGate)
        {
            GetOnGround();
            gettingResourses = false;
            tribe = collision.gameObject;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.name == "Plane") && !onGate)
        {
            GetOnGround();

        }
    }

    void OnCollisionExit(Collision collision)
    {
        if ((collision.gameObject.name == "Plane") && !onGate)
        {
            onGround = false;
        }
    }

    void DoMinusResourses()
    {
        tribe.GetComponent<Tribe>().Meat -= minusMeat;
        tribe.GetComponent<Tribe>().Gold -= minusGold;
        tribe.GetComponent<Tribe>().Wood -= minusWood;
        tribe.GetComponent<Tribe>().Faith += plusFaith;
        gettingResourses = false;
    }

    public void DoPlusResourses()
    {
        if (tribe != null)
        {
            tribe.GetComponent<Tribe>().Meat += PlusMeat;
            tribe.GetComponent<Tribe>().Gold += PlusGold;
            tribe.GetComponent<Tribe>().Wood += PlusWood;
        }
    }

    public void Unfreeze()
    {
        onGate = false;
    }

    void ReachPoint()
    {
        if (!transform.position.Equals(target) && reachingTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, transform.position.y, target.z), Time.deltaTime);
            if (transform.position.x == target.x && transform.position.z == target.z && reachingTarget)
            {
                reachingTarget = false;
                if (currentGate != null && currentGate.Equals(GameObject.Find("LeftProtoGate")))
                {
                    target = GameObject.Find("LeftProtoGate").transform.position;
                    currentGate = null;
                    onGate = true;
                    reachingTarget = true;
                    onGround = false;
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, transform.position.y, target.z), Time.deltaTime);
                }
                if (currentGate != null && currentGate.Equals(GameObject.Find("RightProtoGate")))
                {
                    target = GameObject.Find("RightProtoGate").transform.position;
                    currentGate = null;
                    reachingTarget = true;
                    onGround = false;
                    onGate = true;
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, transform.position.y, target.z), Time.deltaTime);
                }
            }
        }
    }

    void GetOnGround()
    {
        if (!onGate && !reachingTarget && !grabbed)
        {
            onGround = true;
        }
    }

    void Death()
    {
        tribe.GetComponent<Tribe>().Humans = new List<GameObject>(GameObject.FindGameObjectsWithTag("Human").Length);
        //UBO here, need to be fixed later
        gameObject.transform.Find("Body").GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Mask").GetComponent<MeshRenderer>().enabled = false;
        transform.position = totem.transform.position;
        foreach (GameObject human in GameObject.FindGameObjectsWithTag("Human"))
        {
            if (!human.Equals(gameObject))
            {
                tribe.GetComponent<Tribe>().Humans.Add(human);
                human.GetComponent<ri>().fu = tribe.GetComponent<Tribe>().Humans.IndexOf(human);
            }
        }
        Destroy(gameObject, 0.1f);
    }

    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
    }
}