using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropScript : MonoBehaviour
{
    // Variables
    [Range(0.5f, 2f)]
    public float size = 1.5f;
    public float recoverTime = 1;
    public ParticleSystem hitVFX;
    [HideInInspector] public Rigidbody rb;
    public MagnetScript magnet;
    public float lastHitTime;
    public float pullModifier;
    public bool launched;
    [HideInInspector] public float spawnTime;
    [HideInInspector] public bool ready;

    // Start is called before the first frame update
    void Start()
    {
        // Sets variables
        rb = GetComponent<Rigidbody>();
        transform.localScale = transform.localScale * Random.Range(1, size);
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Decides when the prop is ready to be moved
        if (Time.time >= spawnTime + 1)
            ready = true;
        if (Time.time >= lastHitTime + recoverTime)
            launched = false;
    }

    // When the prop hits something
    public void OnCollisionEnter(Collision collision)
    {
        if (!ready) return;
        bool hit = false;
        // detects if hits an AI
        if (collision.transform.GetComponentInParent<AIBehaviour>())
        {
            if (collision.transform.GetComponentInParent<AIBehaviour>())
                if (collision.relativeVelocity.sqrMagnitude > 15 & ready)
                {
                    hit = true;
                    collision.transform.GetComponentInParent<AIBehaviour>().Kill(true);
                }
        }
        else
        {
            // Checks if hit a wall
            if (launched & collision.relativeVelocity.sqrMagnitude > 50)
                hit = true;
        }
        if (hit)
        {
            // detects if hits an AI
            if (!collision.transform.GetComponentInParent<AIBehaviour>())
            {
                ParticleSystem vfx = Instantiate(hitVFX);
                vfx.transform.position = transform.position;
                vfx.transform.eulerAngles = transform.eulerAngles;
            }
            Destroy(gameObject);
        }
    }

    // Check if the prop has been launched
    public void CheckLaunch()
    {
        if (!launched)
            Hit();
    }

    // When hit something
    public void Hit()
    {
        lastHitTime = Time.time;
        if (magnet)
            magnet.attatchedProps.Remove(this);
        ParticleSystem vfx = Instantiate(hitVFX);
        vfx.transform.position = transform.position;
        vfx.transform.eulerAngles = transform.eulerAngles;
        transform.SetParent(null, true);
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.isKinematic = false;
        Transform angle = new GameObject().transform;
        if (magnet)
            angle.transform.position = magnet.transform.position;
        angle.LookAt(transform.position);
        rb.AddForce(angle.forward * 1000);
        pullModifier = 5;
        Destroy(angle.gameObject);
    }

    // Launches the prop at a position
    public void Launch(Vector3 pos)
    {
        lastHitTime = Time.time + 2;
        if (magnet)
            magnet.attatchedProps.Remove(this);
        transform.SetParent(null, true);
        rb.isKinematic = false;
        Transform angle = new GameObject().transform;
        angle.transform.position = transform.position;
        angle.LookAt(pos);
        rb.AddForce(angle.forward * 3000);
        pullModifier = 5;
        Destroy(angle.gameObject);
        launched = true;
    }

    public void OnDestroy()
    {
        PropsManager.me.props.Remove(this);
    }
}
