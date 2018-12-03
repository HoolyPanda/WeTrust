using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{

    public GameObject tribe;
    public List<GameObject> points;
    public GameObject newPoint;
    public float circleIteration = 0f;
    float circleRadius = 2f;
    int prevHumansSize;
    void Start()
    {
        prevHumansSize = tribe.GetComponent<Tribe>().Humans.Count;
        foreach (GameObject point in points)
        {
            point.GetComponent<point>().pos = points.IndexOf(point);
            point.GetComponent<point>().Unfreeze();
        }
    }
    void Update()
    {
        if (prevHumansSize < tribe.GetComponent<Tribe>().Humans.Count)
        {
            CreateNewPoint();
            points = new List<GameObject>(tribe.GetComponent<Tribe>().Humans.Count);
            foreach (GameObject cPoint in GameObject.FindGameObjectsWithTag("point"))
            {
                points.Add(cPoint);
                if (tribe.GetComponent<Tribe>().Humans.Count <= 5)
                {
                    cPoint.GetComponent<point>().circleRadius = 2f;
                }
                else
                {
                    cPoint.GetComponent<point>().circleRadius = 0.2f * tribe.GetComponent<Tribe>().Humans.Count;
                }
                cPoint.GetComponent<point>().pos = points.IndexOf(cPoint);
                cPoint.GetComponent<point>().Unfreeze();
            }
            prevHumansSize = tribe.GetComponent<Tribe>().Humans.Count;
            /*
            foreach(GameObject point in points)
            {
                point.GetComponent<point>().pos=points.IndexOf(point);
                point.GetComponent<point>().Unfreeze();
            }
             */
        }
        if (prevHumansSize > tribe.GetComponent<Tribe>().Humans.Count)
        {
            foreach (GameObject point in points)
            {
                DestroyImmediate(point);
            }
            points = new List<GameObject>(tribe.GetComponent<Tribe>().Humans.Count);
            for (int i = 0; i < tribe.GetComponent<Tribe>().Humans.Count; i++)
            {
                CreateNewPoint();
            }
            foreach (GameObject cPoint in GameObject.FindGameObjectsWithTag("point"))
            {
                points.Add(cPoint);
                if (tribe.GetComponent<Tribe>().Humans.Count <= 5)
                {
                    cPoint.GetComponent<point>().circleRadius = 2f;
                }
                else
                {
                    cPoint.GetComponent<point>().circleRadius = 0.2f * tribe.GetComponent<Tribe>().Humans.Count;
                }
                cPoint.GetComponent<point>().pos = points.IndexOf(cPoint);
                cPoint.GetComponent<point>().Unfreeze();
            }
            prevHumansSize = tribe.GetComponent<Tribe>().Humans.Count;
        }
    }
    public void PointUpdate()
    {
        float pos = Mathf.PI * 2 / tribe.GetComponent<Tribe>().Humans.Count;
        //print(pos.ToString());
        circleIteration = tribe.GetComponent<Tribe>().Humans.IndexOf(gameObject) * pos;
    }
    public void Freeze()
    {
        foreach (GameObject point in points)
        {
            point.GetComponent<point>().freezed = true;
        }
    }
    public void Unfreeze()
    {
        Invoke("__unfreeze", 2f);
    }
    void __unfreeze()
    {
        foreach (GameObject point in points)
        {
            point.GetComponent<point>().freezed = false;
        }
    }
    void CreateNewPoint()
    {
        GameObject npoint = Instantiate(newPoint, transform.position, Quaternion.identity);
        npoint.transform.parent = GameObject.Find("points").transform;
        npoint.GetComponent<point>().totem = gameObject;
        npoint.GetComponent<point>().tribe = tribe;
    }
}
