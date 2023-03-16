using UnityEngine;

public class RotatingCube : MonoBehaviour
{
    public float rotationSpeed = 0.0001f; // the speed of the rotation
    public float radius = 2.0f; // the radius of the circle

    private Vector3 center; // the center point of the circle
    private float angle; // the current angle of the cube

    void Start()
    {
        center = transform.position; // set the center point to the cube's position
        angle = 0.0f; // set the initial angle to 0
    }

    void Update()
    {
        angle += rotationSpeed * Time.deltaTime; // increment the angle based on the rotation speed and time

        Vector3 offset = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius; // calculate the offset from the center point
        offset = offset.normalized * Time.deltaTime; // scale the offset based on the movement speed and time

        transform.position += offset; // move the cube based on the offset
        transform.rotation = Quaternion.LookRotation(offset); // rotate the cube to face the direction of the offset
    }
}
