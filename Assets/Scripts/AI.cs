using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public Waypoint startWaypoint;
    public float speed;
    private Waypoint currentWaypoint, nextWaypoint;
    private Vector3 destination;
    private float step;
    private Animator anim;
    private int caseSwitch = 1;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = startWaypoint.transform.position;
        currentWaypoint = startWaypoint;
        step = speed * Time.deltaTime;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (caseSwitch)
        {
            case 1: //Get waypoint
                Debug.Log("getting the next waypoint");
                nextWaypoint = selectWaypoint(currentWaypoint);
                if (nextWaypoint != null) { 
                    destination = nextWaypoint.transform.position;
                    caseSwitch = 2; 
                }
                break;
            case 2: //Start idling coroutine
                Debug.Log("starting idle coroutine");
                StartCoroutine(idleForAWhile());
                break;
            case 3: //Wait coroutine to finish
                Debug.Log("idling");
                break;
            case 4: //Move
                Debug.Log("moving");
                moveToWaypoint(nextWaypoint);
                break;
        }

    }

    private IEnumerator idleForAWhile()
    {
        caseSwitch = 3; //switching case so coroutine will not be called multiple times
        yield return new WaitForSeconds(3);
        caseSwitch = 4; //coroutine is done, switch case to moving
        anim.SetBool("isMoving", true);
    }

    private Waypoint selectWaypoint(Waypoint currentWaypoint)
    {
        if(currentWaypoint.connectedWaypoints != null)
        {
            int waypointIndex = Random.Range(0, currentWaypoint.connectedWaypoints.Length);
            return currentWaypoint.connectedWaypoints[waypointIndex];
        }
        return null;
    }

    private void moveToWaypoint(Waypoint nextWaypoint)
    {
        Vector3 moveTo = Vector3.MoveTowards(transform.position, destination, step);
        transform.position = moveTo;
        transform.rotation = Quaternion.LookRotation(destination);
        if (transform.position == nextWaypoint.transform.position)
        {
            anim.SetBool("isMoving", false);
            caseSwitch = 1;
            currentWaypoint = nextWaypoint;
        }
    }
}
