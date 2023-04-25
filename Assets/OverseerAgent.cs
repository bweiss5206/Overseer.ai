using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;
using Unity.MLAgents.Sensors;
using System.Collections.Generic;


public class OverseerAgent : Agent
{
    public GunController gunController;
    public float fireCooldown = 0.5f; // Adjust this value according to your desired cooldown time
    private float lastFireTime;
    private int chosenEntityIndex;
    public List<GameObject> entities;
    public GameObject player;
    public GameObject npc;
    public GameObject npc1;
    public GameObject npc2;
    public GameObject npc3;
    public GameObject npc4;
    public GameObject ObjectiveR;
    public GameObject ObjectiveG;
    public GameObject ObjectiveB;
    public GameObject ObjectiveY;
    private Vector3 startPosition;

    public bool endEpisode;

    /// <summary>
    /// Called once when the agent is first initialized
    /// </summary>
    public override void Initialize()
    {
        startPosition = player.transform.position;
        gunController = this.GetComponent<GunController>();
    }

    // void Update()
    // {
    //     PointToMouse();
    //     //Shoot();
    // }


    /// <summary>
    /// Called every time an episode begins. This is where we reset the challenge.
    /// </summary>
    public override void OnEpisodeBegin()
    {
        //endEpisode = false;
        player.transform.position = startPosition + Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f)) * Vector3.forward * UnityEngine.Random.Range(0f, 100f);
        player.transform.rotation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
        npc.transform.position = startPosition + Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f)) * Vector3.forward * UnityEngine.Random.Range(0f, 100f);
        npc.transform.rotation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
        npc1.transform.position = startPosition + Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f)) * Vector3.forward * UnityEngine.Random.Range(0f, 100f);
        npc1.transform.rotation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
        npc2.transform.position = startPosition + Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f)) * Vector3.forward * UnityEngine.Random.Range(0f, 100f);
        npc2.transform.rotation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
        npc3.transform.position = startPosition + Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f)) * Vector3.forward * UnityEngine.Random.Range(0f, 100f);
        npc3.transform.rotation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
        npc4.transform.position = startPosition + Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f)) * Vector3.forward * UnityEngine.Random.Range(0f, 100f);
        npc4.transform.rotation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
        
        ObjectiveComplete R = (ObjectiveComplete) ObjectiveR.GetComponent(typeof(ObjectiveComplete));
        R.Start();
        ObjectiveComplete G = (ObjectiveComplete) ObjectiveG.GetComponent(typeof(ObjectiveComplete));
        G.Start();
        ObjectiveComplete B = (ObjectiveComplete) ObjectiveB.GetComponent(typeof(ObjectiveComplete));
        B.Start();
        ObjectiveComplete Y = (ObjectiveComplete) ObjectiveY.GetComponent(typeof(ObjectiveComplete));
        Y.Start();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //entities.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        int count = 0;
        foreach (GameObject entity in entities)
        {
            if (entity != null)
            {   
                // Add position observation
                sensor.AddObservation(entity.transform.position);
            }
            else
            {
                // count++;
                // Debug.LogWarning("An entity in the entities list is null.");
            }
        }
    }

    /// <summary>
    /// Controls the agent with human input
    /// </summary>
    /// <param name="actionsOut">The actions parsed from keyboard input</param>
    public override void Heuristic(in ActionBuffers actionsOut)
    {

        if (Input.GetKey(KeyCode.Alpha1))
        {
            actionsOut.DiscreteActions.Array[0] = 0;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            actionsOut.DiscreteActions.Array[0] = 1;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {

            actionsOut.DiscreteActions.Array[0] = 2;
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            actionsOut.DiscreteActions.Array[0] = 3;
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            actionsOut.DiscreteActions.Array[0] = 4;
        }
        else {
            actionsOut.DiscreteActions.Array[0] = 5;
        }
       
    }
    


    /// <summary>
    /// React to actions coming from either the neural net or human input
    /// </summary>
    /// <param name="actions">The actions received</param>
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int chosenEntityIndex = actionBuffers.DiscreteActions[0];

        //Debug.Log(GetCumulativeReward());
        GameObject chosenEntity = null;
        entities.Sort((a, b) =>
        {
            if (a == null && b == null)
                return 0; // Both are null, they are equal
            else if (a == null)
                return 1; // a is null, b is not null, a should be placed after b
            else if (b == null)
                return -1; // b is null, a is not null, a should be placed before b
            else
                return a.transform.position.x.CompareTo(b.transform.position.x); // Compare by their x positions
        });

        chosenEntity = entities[chosenEntityIndex];
        Debug.Log(chosenEntityIndex);
        //gunController.PointToMouse(chosenEntity.transform.position);

        if(entities[chosenEntityIndex] != null){
            // Check if the chosen entity is the player and assign rewards accordingly
            if (chosenEntity.CompareTag("Player"))
            {
                // Positive reward for choosing the correct entity
                AddReward(1.0f);
            }

            else
            {
                // Negative reward for choosing the wrong entity
                AddReward(-1.0f);
            }
            Debug.Log("EndEpisode");
            EndEpisode();
        }

        // End the episode
        
    }
}