using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    // Variables
    public float damage = 15;
    public ParticleSystem hitVFX;

    // When the projectile hits something
    public void OnCollisionEnter(Collision collision)
    {
        // Checks if hit a prop
        if (collision.transform.GetComponentInParent<PropScript>())
            collision.transform.GetComponentInParent<PropScript>().CheckLaunch();

        // Checks if hit the player
        if (collision.transform.GetComponentInParent<PlayerMovement>())
            collision.transform.GetComponentInParent<PlayerMovement>().Damage(damage);
        ParticleSystem vfx = Instantiate(hitVFX);
        vfx.transform.position = transform.position;
        vfx.transform.eulerAngles = transform.eulerAngles;
        Destroy(gameObject);
    }

}
