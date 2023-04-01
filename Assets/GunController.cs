using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 100f;
    public Transform firePoint;
    public bool isEndGame = false;
    public LineRenderer laser;

    public Camera mainCamera;

    void Start()
    {
        //mainCamera = Camera.main;
    }

    void Update()
    {
        PointToMouse();
        Shoot();
    }

    void PointToMouse()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
            transform.LookAt(new Vector3(pointToLook.x, -pointToLook.y, pointToLook.z));
        }

        UpdateLaser();
    }

    void UpdateLaser()
    {
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

    void Shoot()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletInstance.GetComponent<Rigidbody>().velocity = bulletInstance.transform.forward * bulletSpeed;
            StartCoroutine(DeleteBullet(bulletInstance));
        }
    }

    IEnumerator DeleteBullet(GameObject bullet)
    {
        RaycastHit hit;
        while (!Physics.Raycast(bullet.transform.position, bullet.transform.forward, out hit))
        {
            yield return null;
        }

        if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Ground"))
        {
            if (hit.collider.CompareTag("Player"))
            {
                isEndGame = true;
            }
            Destroy(bullet);
        }
    }
}