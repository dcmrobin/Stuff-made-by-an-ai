using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;
    public float fireRate = 0.5f;
    private float fireCooldown = 0f;

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (Input.GetButton("Fire1") && fireCooldown <= 0f)
        {
            fireCooldown = fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Get the position of the player's head
        Transform head = transform.Find("Head");
        Vector3 headPosition = head.position;

        // Instantiate the projectile at the head position
        GameObject projectile = Instantiate(projectilePrefab, headPosition, transform.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Set the velocity of the projectile
        rb.velocity = transform.forward * projectileSpeed;
    }
}