using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class SimpleCollector : Agent
{
    float startTime;
    float timer;
    public TrainingArea trainingArea;
    private new Rigidbody rBody;
    private new Transform transform;

    public override void Initialize() 
    {
        MaxStep = 5000;
        rBody = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
    }

    //public Transform Target;
    public override void OnEpisodeBegin()
    {
        rBody.angularVelocity = Vector3.zero;
        rBody.velocity = Vector3.zero;
        transform.localPosition = new Vector3(Random.Range(-5.0f, 5.0f), 0.5f, Random.Range(-5.0f, 5.0f));
        transform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));
        trainingArea.ResetArea();
    }


 
    public override void CollectObservations(VectorSensor sensor)
    {

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        
        var action = actions.DiscreteActions;
        Vector3 dir = Vector3.zero;
        Vector3 rot = Vector3.zero;

        switch (action[0]){
            case 1: dir = transform.forward; break;
            case 2: dir = -transform.forward; break;
        }
        switch (action[1]){
            case 1: rot = transform.up; break;
            case 2: rot = -transform.up; break;
        }

        transform.Rotate(rot, Time.fixedDeltaTime * 200.0f);
        rBody.AddForce(dir * 1.5f, ForceMode.VelocityChange);
        AddReward(-1 / (float)MaxStep);

        if(trainingArea.endEpisode){
            EndEpisode();
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var action = actionsOut.DiscreteActions;
        actionsOut.Clear();

        if(Input.GetKey(KeyCode.W)){
            action[0] = 1;
        }
        if(Input.GetKey(KeyCode.S)){
            action[0] = 2;
        }
        if(Input.GetKey(KeyCode.A)){
            action[1] = 1;
        }
        if(Input.GetKey(KeyCode.D)){
            action[1] = 2;
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        // If the other object is a target, reward
        if (coll.collider.CompareTag("Target"))
        {
            rBody.velocity = rBody.angularVelocity = Vector3.zero;
            Debug.Log("Object collected");
            trainingArea.Collect(coll.gameObject);
            AddReward(1.0f);
        }
        if (coll.collider.CompareTag("Wall"))
        {
            Debug.Log("Hit wall");
            AddReward(-0.1f);
        }
    }
}


