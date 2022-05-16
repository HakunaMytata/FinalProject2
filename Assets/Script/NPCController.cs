using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MathUtil;

public class NPCController : MonoBehaviour
{
    GameObject player;
    public NavMeshAgent enemy;
    public Transform Player;


    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

    private float waitTime = 1f; // in seconds
    private float waitCounter = 0f;
    private bool waiting = false;




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();

    }
    void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        







        if (Util.CanSeeObj(player, gameObject, 0.9f))
        {
            Debug.Log("I can see the player!");

            enemy.SetDestination(Player.position);

        }else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {

            GotoNextPoint();
        }






    }
    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

}
