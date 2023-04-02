using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class OverseerAgent : Agent
{
    public GunController gunController;
    public float fireCooldown = 0.5f; // Adjust this value according to your desired cooldown time
    private float lastFireTime;

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

    }

    /// <summary>
    /// Controls the agent with human input
    /// </summary>
    /// <param name="actionsOut">The actions parsed from keyboard input</param>
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        float[] continuousActions = actionsOut.ContinuousActions.Array;
        int[] discreteActions = actionsOut.DiscreteActions.Array;

        // Get mouse position
        Vector3 mousePosition = Input.mousePosition;

        gunController.PointToMouse(mousePosition);

        // Try normalized mouse position if regular doesnt work
        //Vector2 normalizedMousePosition = new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);

        // Assign normalized mouse position to the first two dimensions of the action vector
        continuousActions[0] = mousePosition.x;
        continuousActions[1] = mousePosition.y;

        // Assign the left mouse button input to the discrete action vector
        discreteActions[0] = Input.GetMouseButton(0) ? 1 : 0;   
        
    }

    /// <summary>
    /// React to actions coming from either the neural net or human input
    /// </summary>
    /// <param name="actions">The actions received</param>
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float[] vectorAction = actionBuffers.ContinuousActions.Array;
        int[] discreteAction = actionBuffers.DiscreteActions.Array;

        // Convert the action vector to the AI-generated mouse position
        Vector2 mousePosition = new Vector2(vectorAction[0], vectorAction[1]);

        gunController.PointToMouse(mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(gunController.firePoint.transform.position, gunController.firePoint.transform.forward, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                // Add small reward for looking at a player or NPC
                AddReward(0.5f);
            }
            else
            {
                // Add small negative reward for not looking at a player
                AddReward(-0.5f);
            }

            // Use the firing trigger to shoot
            if (discreteAction[0] == 1)
            {
                Debug.Log("Fire");
                if (hit.collider.CompareTag("Player"))
                {
                    //Debug.Log("Hit Player");
                    AddReward(10.0f);
                    EndEpisode();
                }
                else
                {
                    //Debug.Log("Missed Player");
                    AddReward(-3.33f);
                }
            }
        }
    }

}