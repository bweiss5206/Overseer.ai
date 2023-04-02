using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //public GameObject bulletPrefab;
    //public float bulletSpeed = 100f;
    public Transform firePoint;
    //public bool isEndGame = false;
    public LineRenderer laser;

    public Camera sensorCamera;

    // void Start()
    // {
    //     mainCamera = Camera.main;
    // }

    void Update()
    {
        //PointToMouse();
        //Shoot();
    }

    public void PointToMouse(Vector2 mousePosition)
    {
        Ray cameraRay = sensorCamera.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
            transform.LookAt(new Vector3(pointToLook.x, -pointToLook.y, pointToLook.z));
        }

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

    // void Shoot()
    // {
    //     RaycastHit hit;
    //     while (!Physics.Raycast(firePoint.transform.position, firePoint.transform.forward, out hit))
    //     {
    //         yield return null;
    //     }
        
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         if(hit.collider.CompareTag("Player")){
    //             Debug.Log("Hit Player");
    //             isEndGame = true;
    //         }
    //         else {
    //             Debug.Log("Missed Player");
    //         }
    //         GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    //         bulletInstance.GetComponent<Rigidbody>().velocity = bulletInstance.transform.forward * bulletSpeed;
    //         StartCoroutine(DeleteBullet(bulletInstance));
    //     }
    // }

    // IEnumerator DeleteBullet(GameObject bullet)
    // {
    //     RaycastHit hit;
    //     while (!Physics.Raycast(bullet.transform.position, bullet.transform.forward, out hit))
    //     {
    //         yield return null;
    //     }

    //     if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Ground"))
    //     {
    //         if (hit.collider.CompareTag("Player"))
    //         {
                
    //         }
    //         Destroy(bullet);
    //     }
    // }
}