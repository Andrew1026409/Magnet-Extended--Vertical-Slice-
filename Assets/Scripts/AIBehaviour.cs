using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour
{
    // Variables for the speed
    public float speed = 5;
    public float stoppingDistance = 5;

    // Variables for the effects
    public ParticleSystem dieVFX;
    public ParticleSystem shootVFX;
    public Material launchMaterial;
    public Material targetMaterial;
    Material material;

    // Variables for firing of the projectiles
    [Range(0.1f, 2f)]
    public float fireRate = 2;
    public float projectileSpeed = 15;
    public float attackRange = 7;
    public Transform Projectile;

    // Ignore
    Rigidbody rb;
    Transform player;
    MeshRenderer m_renderer;
    MeshRenderer m_bodyRenderer;
    float startDrag;
    float lastAttackTime;
    [HideInInspector] public float launchTime;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the variables
        rb = GetComponent<Rigidbody>();
        m_renderer = GetComponentInChildren<MeshRenderer>();
        m_bodyRenderer = m_renderer.transform.GetChild(0).GetComponent<MeshRenderer>();
        material = m_renderer.material;
        player = FindObjectOfType<PlayerMovement>().transform;
        startDrag = rb.drag;
        fireRate = Random.Range(0.2f, 2);
    }

    void FixedUpdate()
    {
        // Sets the colour if the AI is in range of the player's attacjs
        if (player)
            if (player.GetComponent<MagnetScript>().GetClosestAI() == this & player.GetComponent<MagnetScript>().isPulling & player.GetComponent<MagnetScript>().isSpinning)
                m_bodyRenderer.material = targetMaterial;
            else
                m_bodyRenderer.material = material;
        if (Time.time > launchTime + 1)
            launchTime = 0;
        if (launchTime > 0)
            m_bodyRenderer.material = launchMaterial;
        m_bodyRenderer.enabled = true ? m_bodyRenderer.material : false;

        // Fire a projectile at the player when it is time to fire
        if (player)
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
                if (Time.time >= lastAttackTime + fireRate)
                    Fire();

        // Kill AI if they fall off the map
        if (transform.position.y < 0)
            Kill(false);

        // Check if AI is close to player
        if (player)
            if (Vector3.Distance(transform.position, player.position) > stoppingDistance)
            {
                // Moves AI to player
                rb.drag = startDrag;
                Transform angle = new GameObject().transform;
                angle.transform.position = transform.position;
                angle.LookAt(player.position);
                rb.AddForce(angle.forward * speed);
                Destroy(angle.gameObject);
            }
            else
                rb.drag = 2;
    }

    // Fires a projectile at the player
    public void Fire()
    {
        lastAttackTime = Time.time;
        Transform newProjectile = Instantiate(Projectile);
        Transform angle = new GameObject().transform;
        angle.transform.position = transform.position;
        if (player)
            angle.LookAt(player.position);
        newProjectile.position = transform.position;
        newProjectile.eulerAngles = angle.eulerAngles;
        newProjectile.position += newProjectile.forward * 1;
        ParticleSystem vfx = Instantiate(shootVFX);
        vfx.transform.position = transform.position;
        vfx.transform.eulerAngles = angle.eulerAngles;
        newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.forward * projectileSpeed);
        Destroy(angle.gameObject);
        Destroy(newProjectile.gameObject, 1.5f);
    }

    // When the AI is killed
    public void Kill(bool showFX)
    {
        AIManager.me.ais.Remove(this);
        ParticleSystem vfx = Instantiate(dieVFX);
        vfx.transform.position = transform.position;
        vfx.transform.eulerAngles = transform.eulerAngles;
        if (player)
            player.GetComponent<PlayerMovement>().Heal(10, Vector3.Distance(transform.position, player.position) < 8 & showFX);
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        AIManager.me.ais.Remove(this);
    }
}
