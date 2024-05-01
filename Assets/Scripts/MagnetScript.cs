using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetScript : MonoBehaviour
{
    public static MagnetScript me;

    // Variables for the magnetic field
    public float pullRadius = 30;
    public float pullForce = 5;
    public float spinRate = 200;
    public float fireRate = 0.6f;


    // Ignore
    public SphereCollider innerCircle;
    public Light m_light;
    [HideInInspector] public bool isPulling;
    [HideInInspector] public bool isSpinning;
    public List<PropScript> props = new List<PropScript>();
    public List<PropScript> attatchedProps = new List<PropScript>();
    public float startLight;
    float lastFireTime;

    private void Awake()
    {
        me = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Sets the variables
        foreach (PropScript prop in FindObjectsOfType<PropScript>())
            props.Add(prop);
        startLight = m_light.intensity;
        m_light.intensity = 0;
        innerCircle.transform.parent.transform.SetParent(null);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Controls the input for the magnet
        isPulling = Input.GetAxis("Magnet") > 0;
        isSpinning = Input.GetAxis("Fire2") > 0;
        innerCircle.enabled = isPulling;
        innerCircle.transform.parent.transform.position = transform.position;

        // Spinning the magnetic field
        if (isSpinning)
        {
            innerCircle.transform.eulerAngles += new Vector3(0, spinRate, 0) * Time.deltaTime;
            if (Input.GetAxis("Fire1") > 0 & Time.time >= lastFireTime + fireRate & attatchedProps.Count > 0)
                Fire();
        }

        // Removes the props if they are invalid
        foreach (PropScript prop in attatchedProps)
            if (!prop)
                attatchedProps.Remove(prop);

        // Controls the props caught in the magnetic field
        foreach (PropScript prop in attatchedProps)
        {
            if (isPulling & Time.time >= prop.lastHitTime + prop.recoverTime)
            {
                prop.magnet = this;
                prop.transform.position = new Vector3(prop.transform.position.x, transform.position.y, prop.transform.position.z);
                prop.transform.SetParent(innerCircle.transform, true);
                prop.rb.constraints = RigidbodyConstraints.FreezePositionY;
                prop.rb.isKinematic = true;
            }
            else
            {
                prop.magnet = null;
                prop.transform.SetParent(null, true);
                prop.rb.constraints = RigidbodyConstraints.None;
                prop.rb.constraints = RigidbodyConstraints.FreezeRotation;
                prop.rb.isKinematic = false;
            }
        }

        // Deactivates the props if the magnetic field is down
        if (!isPulling)
        {
            m_light.intensity = Mathf.Lerp(m_light.intensity, 0, 0.05f);
            foreach (PropScript prop in innerCircle.GetComponentsInChildren<PropScript>())
            {
                prop.magnet = null;
                prop.transform.SetParent(null, true);
                prop.rb.isKinematic = false;
            }
            attatchedProps.Clear();
        }
        else
            m_light.intensity = Mathf.Lerp(m_light.intensity, startLight, 0.05f);

        // Pulls close props
        if (isPulling)
            PullProps();
    }

    // Launches props at closest AI
    public void Fire()
    {
        lastFireTime = Time.time;
        PropScript prop = attatchedProps[Random.Range(0, attatchedProps.Count - 1)];
        GetClosestAI().launchTime = Time.time;
        prop.Launch(GetClosestAI().transform.position);
    }

    public AIBehaviour GetClosestAI()
    {
        AIBehaviour ai = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (AIBehaviour t in AIManager.me.ais)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                ai = t;
                minDist = dist;
            }
        }
        return ai;
    }

    // Pulls props towards the player
    public void PullProps()
    {
        Collider[] closeProps = Physics.OverlapSphere(transform.position, pullRadius);
        foreach (Collider col in closeProps)
        {
            PropScript prop = col.GetComponent<PropScript>();
            if (prop)
            {
                // Checks if the prop is ready to be moved
                if (Time.time >= prop.lastHitTime + prop.recoverTime & attatchedProps.Count < 4)
                {
                    // Pulls props if the maximum count has not been reached
                    if (Vector3.Distance(transform.position, prop.transform.position) <= pullRadius - prop.pullModifier & !prop.launched)
                    {
                        prop.magnet = this;
                        prop.pullModifier = 0;
                        // Checks if prop is close to the inner circle
                        if (Vector3.Distance(transform.position, prop.transform.position) <= innerCircle.radius + 2)
                        {
                            if (Vector3.Distance(transform.position, prop.transform.position) > innerCircle.radius + 1)
                            {
                                // Attatches the prop to the inner circle
                                if (!attatchedProps.Contains(prop))
                                    attatchedProps.Add(prop);
                            }
                            else
                            {
                                // Deactivates prop if too far from magnetic field
                                if (attatchedProps.Contains(prop))
                                    attatchedProps.Remove(prop);
                                Transform angle = new GameObject().transform;
                                angle.transform.position = transform.position;
                                angle.LookAt(prop.transform.position);
                                prop.rb.AddForce(angle.forward * pullForce);
                                Destroy(angle.gameObject);
                            }
                        }
                        else
                        {
                            // Deactivates prop if too far from player
                            Transform angle = new GameObject().transform;
                            angle.transform.position = prop.transform.position;
                            angle.LookAt(transform.position);
                            prop.rb.AddForce(angle.forward * pullForce);
                            Destroy(angle.gameObject);
                            if (attatchedProps.Contains(prop))
                                attatchedProps.Remove(prop);
                        }
                    }
                    else
                        prop.magnet = null;
                }
            }
        }
    }
}
