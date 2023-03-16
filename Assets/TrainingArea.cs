using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingArea : MonoBehaviour
{

    public List<GameObject> collectables;
    public int numCollected = 0;
    public bool endEpisode = false;
    public float minX = -5.0f; // Minimum X position
    public float maxX = 5.0f; // Maximum X position
    public float minY = 0.5f; // Minimum Y position
    public float maxY = 0.5f; // Maximum Y position
    public float minZ = -5.0f; // Minimum Z position
    public float maxZ = 5.0f; // Maximum Z position

    public void ResetArea()
    {
        numCollected = 0;
        endEpisode = false;
        foreach (GameObject col in collectables){
            col.SetActive(true);
            RandomizeObjectPositions(collectables);
        } 
    }

    public void Collect(GameObject gameObject)
    {
        gameObject.SetActive(false);
        numCollected += 1;
    }

    void Update()
    {
        if(numCollected == collectables.Count){
            endEpisode = true;
        }
    }
    public void RandomizeObjectPositions(List<GameObject> objectsToRandomize)
{
    List<Vector3> usedPositions = new List<Vector3>(); // List of used positions

    foreach (GameObject obj in objectsToRandomize)
    {
        Vector3 newPos = Vector3.zero;
        bool foundNewPos = false;

        // Loop until a unique position is found
        while (!foundNewPos)
        {
            // Generate a new random position
            float x = Random.Range(-5.0f, 5.0f);
            float y = 0.5f;
            float z = Random.Range(-5.0f, 5.0f);
            newPos = new Vector3(x, y, z);

            // Check if the new position is unique
            bool isUnique = true;
            foreach (Vector3 usedPos in usedPositions)
            {
                if (Vector3.Distance(newPos, usedPos) < 1f)
                {
                    isUnique = false;
                    break;
                }
            }

            // If the new position is unique, set foundNewPos to true and add the position to usedPositions
            if (isUnique)
            {
                foundNewPos = true;
                usedPositions.Add(newPos);
            }
        }

        // Set the object's position to the new position
        obj.transform.position = newPos;
    }
}
}
