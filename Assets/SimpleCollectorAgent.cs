using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Windows;
using System;
using System.Collections;

public class SimpleCollectorAgent : Agent
{
    //public gameObject Objective;
    private Vector3 startPosition;
    private Vector3 pos;
    private SimpleCharacterController characterController;
    private bool canMove = true;
    private bool done = true;
    new private Rigidbody rigidbody;
    public GameObject ObjectiveR;
    public GameObject ObjectiveG;
    public GameObject ObjectiveB;
    public GameObject ObjectiveY;
    //public gameObject Coin;
    //private Vector3 CoinStart;
    public GameObject gameo;

    private OverseerAgent overseerAgent;


    /// <summary>
    /// Called once when the agent is first initialized
    /// </summary>
    public override void Initialize()
    {
        startPosition = transform.position;
        characterController = GetComponent<SimpleCharacterController>();
        rigidbody = GetComponent<Rigidbody>();

        // Find the GameObject with the "Overseer" tag
        GameObject overseerObject = GameObject.FindGameObjectWithTag("Overseer");

        // Get the OverseerAgent component on the GameObject
        overseerAgent = overseerObject.GetComponent<OverseerAgent>();
        
    }
 
    /// <summary>
    /// Called every time an episode begins. This is where we reset the challenge.
    /// </summary>
     public override void OnEpisodeBegin()
     {
        overseerAgent.endEpisode = false;
        done = true;
         transform.position = startPosition + Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f)) * Vector3.forward * UnityEngine.Random.Range(0f, 100f);
         transform.rotation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
         rigidbody.velocity = Vector3.zero;

        ObjectiveComplete R = (ObjectiveComplete) ObjectiveR.GetComponent(typeof(ObjectiveComplete));
        R.Start();
        ObjectiveComplete G = (ObjectiveComplete) ObjectiveG.GetComponent(typeof(ObjectiveComplete));
        G.Start();
        ObjectiveComplete B = (ObjectiveComplete) ObjectiveB.GetComponent(typeof(ObjectiveComplete));
        B.Start();
        ObjectiveComplete Y = (ObjectiveComplete) ObjectiveY.GetComponent(typeof(ObjectiveComplete));
        Y.Start();
        OnStart S = (OnStart) gameo.GetComponent(typeof(OnStart));
        S.Start();
         
        
    //      // Reset agent position, rotation
    //      transform.position = startPosition;
    //      transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
    //      rigidbody.velocity = Vector3.zero;
        
    //          var clones = GameObject.FindGameObjectsWithTag("clone");
    //          foreach (var clone in clones){
    //                   Destroy(clone);
    //              }
         
    //      //Reset platform position (5 meters away from the agent in a random direction)
    //      //platform.transform.position = startPosition + Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) * Vector3.forward * Random.Range(0f, 30f);
    //     for (int i = 0; i<25; ++i){
            
    //         GameObject a = Instantiate(gameObject);
    //         a.transform.position = startPosition + Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) * Vector3.forward * Random.Range(0f, 250f);
    //         a.SetActive(true);
    //         a.tag = "clone";
    //     }
    //     //platform(clone).SetActive(true);
     }

    /// <summary>
    /// Controls the agent with human input
    /// </summary>
    /// <param name="actionsOut">The actions parsed from keyboard input</param>
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (canMove){
        // Read input values and round them. GetAxisRaw works better in this case
        // because of the DecisionRequester, which only gets new decisions periodically.
        int vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
        int horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        bool jump = Input.GetKey(KeyCode.Space);

        // Convert the actions to Discrete choices (0, 1, 2)
        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = vertical >= 0 ? vertical : 2;
        actions[1] = horizontal >= 0 ? horizontal : 2;
        actions[2] = jump ? 1 : 0;
            /*if (GameObject.Find("Coin") == null)
            {
                EndEpisode();
            }*/
            
    }
    }

    /// <summary>
    /// React to actions coming from either the neural net or human input
    /// </summary>
    /// <param name="actions">The actions received</param>
    public override void OnActionReceived(ActionBuffers actions)
    {
        //if(overseerAgent != null){
            
        //}
        
        // Punish and end episode if the agent strays too far
        if(rigidbody.position.y < 1)
        {
            AddReward(-2f);
            EndEpisode();
            print("fell");
        }

        // Convert actions from Discrete (0, 1, 2) to expected input values (-1, 0, +1)
        // of the character controller
        float vertical = actions.DiscreteActions[0] <= 1 ? actions.DiscreteActions[0] : -1;
        float horizontal = actions.DiscreteActions[1] <= 1 ? actions.DiscreteActions[1] : -1;
        bool jump = actions.DiscreteActions[2] > 0;

        characterController.ForwardInput = vertical;
        characterController.TurnInput = horizontal;
        characterController.JumpInput = jump;
    }

    /// <summary>
    /// Respond to entering a trigger collider
    /// </summary>
    /// <param name="other">The object (with trigger collider) that was touched</param>
    private void OnTriggerEnter(Collider other)
    {
        //print(other);
        //print(other.tag);
        // If the other object is a collectible, reward and end episode
        if (other.tag == "Target")
        {
            //print("test");
        //System.Threading.Thread.Sleep(5000);
            //yield return new WaitForSeconds(2);
            if (done){
                StartCoroutine(waiter());
                //print("waiting");
            }
            
            //ObjectiveComplete O = (ObjectiveComplete) other.GetComponent(typeof(ObjectiveComplete));
            //O.Fade();
            //ObjectiveComplete:Fade(other.gameObject);
        }
        /*if (other.tag == Coin.gameObject.tag){
            Coin.transform.position = CoinStart + Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f)) * Vector3.forward * UnityEngine.Random.Range(0f, 100f);
        }*/
    }
    IEnumerator waiter()
    {
        done = false;
        canMove = false;
        AddReward(1f);
        print("reward");
        if (this.gameObject.tag == "clone"){
            yield return new WaitForSeconds(15);
        } else {
            yield return new WaitForSeconds(3);
        }
        canMove = true;
        done = true;
        //done = true;
        if (GameObject.FindWithTag("Target") == null)
        {
            print("Game Won");
            EndEpisode();
        }
        
    }
}
  