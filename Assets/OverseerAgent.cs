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

    public bool endEpisode;

    /// <summary>
    /// Called once when the agent is first initialized
    /// </summary>
    public override void Initialize()
    {
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

        Debug.Log(GetCumulativeReward());
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
                endEpisode = true;
                AddReward(1.0f);
                Debug.Log("EndEpisode");
                EndEpisode();
            }

            else
            {
                // Negative reward for choosing the wrong entity
                AddReward(-1.0f);
            }
        }

        // End the episode
        
    }
}