using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point : MonoBehaviour {

    // Use this for initialization
    public GameObject tribe;
    public GameObject totem;
    public float circleIteration = 0f;
    public int pos;
    public float circleRadius=2f;
    public float positios;
    public bool freezed=false;
    void Start () {
        //Unfreeze();
    }
    
    // Update is called once per frame
    void Update () {

    }
    private void FixedUpdate()
    {
        RunCircle();
    }
    void RunCircle()
    {
        if (!freezed)
        {
            Vector3 circleTarget=new Vector3(totem.transform.position. x+ (float)Mathf.Cos(circleIteration)*circleRadius,transform.position.y, totem.transform.position.z+(float)(Mathf.Sin(circleIteration)*circleRadius));
            if(Mathf.Abs(circleTarget.x-transform.position.x)+Mathf.Abs(circleTarget.z-transform.position.z)>=0.1)
            {
                //this will detect if human didnt take is positiion in circle
            }else
            {
                //if human took his position
                transform.position=circleTarget;
            }
            transform.position=circleTarget;
            if (circleIteration <= Mathf.PI*2)
            {
                circleIteration+=0.01f;
            }
            else
            {
                circleIteration = 0f;
            }
        }
    }
    public void Unfreeze()
    {
        positios = Mathf.PI*2/tribe.GetComponent<Tribe>().Humans.Count;
        //print(positios.ToString());
        circleIteration= pos*positios;
    }
}
