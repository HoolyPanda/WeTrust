using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour {

    public GameObject tribe;
    public List<GameObject> points;
    public float circleIteration = 0f;
    float circleRadius=2f;
    void Start () 
    {
        foreach(GameObject point in points)
        {
            point.GetComponent<point>().pos=points.IndexOf(point);
            point.GetComponent<point>().Unfreeze();
        }
    }
    void Update () 
    {
        //RunCircle();
       
    }
    void RunCircle()
    {
        foreach(GameObject point in points)
        {
            Vector3 circleTarget=new Vector3(point.transform.position. x+ (float)Mathf.Cos(circleIteration)*circleRadius,point.transform.position.y,point.transform.position.z+(float)(Mathf.Sin(circleIteration)*circleRadius));
            if(Mathf.Abs(circleTarget.x-transform.position.x)+Mathf.Abs(circleTarget.z-transform.position.z)>=0.1)
            {
            }else
            {
                point.transform.position=circleTarget;
            }
            point.transform.position=Vector3.MoveTowards(transform.position,circleTarget,Time.deltaTime);
        }
        if (true)
        {
            Vector3 circleTarget=new Vector3(transform.position. x+ (float)Mathf.Cos(circleIteration)*circleRadius,transform.position.y,transform.position.z+(float)(Mathf.Sin(circleIteration)*circleRadius));
            if(Mathf.Abs(circleTarget.x-transform.position.x)+Mathf.Abs(circleTarget.z-transform.position.z)>=0.1)
            {
                //this will detect if human didnt take is positiion in circle
            }else
            {
                //if human took his position
                transform.position=circleTarget;
                //fromTotem=true;
            }
            transform.position=Vector3.MoveTowards(transform.position,circleTarget,Time.deltaTime);
        }
        if (circleIteration <= Mathf.PI*2)
        {
            circleIteration+=0.01f;
        }
        else
        {
            circleIteration = 0f;
        }
    }
    public void Unfreeze()
    {
        float pos = Mathf.PI*2/tribe.GetComponent<Tribe>().Humans.Count;
        print(pos.ToString());
        circleIteration= tribe.GetComponent<Tribe>().Humans.IndexOf(gameObject)*pos;
    }
}
