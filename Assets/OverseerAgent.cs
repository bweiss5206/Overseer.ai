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

    public float guessInterval = 1f;
    private float lastGuessTime;
    public bool endEpisode;

    public Transform firePoint;
    //public bool isEndGame = false;
    public LineRenderer laser;

    public Camera sensorCamera;

    /// <summary>
    /// Called once when the agent is first initialized
    /// </summary>
    public override void Initialize()
    {
        startPosition = player.transform.position;
        gunController = this.GetComponent<GunController>();
        Rigidbody overseerRigidbody = GetComponent<Rigidbody>();
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
                sensor.AddObservation(entity.transform.position.x);
                sensor.AddObservation(entity.transform.position.z);

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
            actionsOut.DiscreteActions.Array[0] = 1;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            actionsOut.DiscreteActions.Array[0] = 2;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            actionsOut.DiscreteActions.Array[0] = 3;
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            actionsOut.DiscreteActions.Array[0] = 4;
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            actionsOut.DiscreteActions.Array[0] = 5;
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            actionsOut.DiscreteActions.Array[0] = 6;
        }
        else {
            actionsOut.DiscreteActions.Array[0] = 0;
        }
       
    }
    


    /// <summary>
    /// React to actions coming from either the neural net or human input
    /// </summary>
    /// <param name="actions">The actions received</param>
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float currentTime = Time.time;
        int chosenEntityIndex = actionBuffers.DiscreteActions[0];
        Debug.Log("Reward: "+ GetCumulativeReward());

        entities.Sort((a, b) =>
        {
            if (a == null) return -1; // If a is null, it should come after b
            if (b == null) return 1; // If b is null, it should come after a

            // If both a and b are not null, compare their positions using sqrMagnitude
            return a.transform.position.sqrMagnitude.CompareTo(b.transform.position.sqrMagnitude);
        });

        PointToChosenEntity(chosenEntityIndex);

        if (currentTime - lastGuessTime >= guessInterval)
        {
            GameObject chosenEntity = null;
            chosenEntity = entities[chosenEntityIndex];
            Debug.Log("Button: "+chosenEntityIndex);


            if(entities[chosenEntityIndex] != null){

                // Check if the chosen entity is the player and assign rewards accordingly
                if (chosenEntity.CompareTag("Player"))
                {
                    // Positive reward for choosing the correct entity
                    AddReward(1.0f);
                    Debug.Log("+");
                    Debug.Log("EndEpisode: "+ entities[chosenEntityIndex]);
                    EndEpisode();
                }

                else
                {
                    // Negative reward for choosing the wrong entity
                    AddReward(-1.0f);
                    Debug.Log("incorrect guess");
                    //last spot in the array, null pick
                    chosenEntityIndex = 6;
                }

            } 
        
            else {
                    this.GetComponent<Rigidbody>().rotation = Quaternion.Euler(new Vector3(90, this.GetComponent<Rigidbody>().rotation.eulerAngles.y, 0));
                }

            lastGuessTime = currentTime;

        }
    }
public void PointToChosenEntity(int chosenEntityIndex)
{
    Rigidbody thisRigidbody = GetComponent<Rigidbody>();

    if (chosenEntityIndex >= entities.Count || entities[chosenEntityIndex] == null)
    {
        thisRigidbody.rotation = Quaternion.Euler(90, thisRigidbody.rotation.eulerAngles.y, 0);
        
        laser.SetPosition(0, Vector3.zero);
        laser.SetPosition(1, Vector3.zero);

        return;
    }

    Vector3 targetPosition = entities[chosenEntityIndex].transform.position;
    Vector3 directionToTarget = targetPosition - transform.position;

    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
    thisRigidbody.rotation = targetRotation;

    RaycastHit hit;
    if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, Mathf.Infinity))
    {
        laser.SetPosition(0, firePoint.position);
        laser.SetPosition(1, hit.point);
    }
    else
    {
        laser.SetPosition(0, firePoint.position);
        laser.SetPosition(1, firePoint.position + firePoint.forward * 1000);
    }
}

}