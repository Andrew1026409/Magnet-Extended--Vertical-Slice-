using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement me;

    // Variables
    public float speed = 15;
    public float health;
    public float maxHealth = 100;
    public ParticleSystem hitVFX;
    public ParticleSystem dieVFX;
    public Color damageColour = Color.red;
    public Color healColour = Color.green;
    public Light m_damageLight;
    Rigidbody rb;
    PostProcessVolume postproccessVolume;
    ColorGrading cg;
    float lastHit;
    float lastHeal;
    Color colour;

    void Awake()
    {
        me = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Sets the variables on start
        health = maxHealth;
        rb = GetComponent<Rigidbody>();
        m_damageLight.intensity = 0;
        postproccessVolume = Camera.main.GetComponent<PostProcessVolume>();
        postproccessVolume.profile.TryGetSettings(out cg);
        colour = cg.colorFilter.value;
    }

    void FixedUpdate()
    {
        // Check for the inputs
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        rb.AddForce(movement * speed);

        // Kills player if falling off arena
        if (transform.position.y <= -8)
            Damage(100);

        // Changes the colour filter when hit and when killing an AI
        if (Time.time > 1 & !GameManager.me.over)
            if (Time.time >= lastHit + 0.5f)
            {
                m_damageLight.intensity = Mathf.Lerp(m_damageLight.intensity, 0, 0.1f);
                if (Time.time >= lastHeal + 0.5f)
                    cg.colorFilter.value = Color.Lerp(cg.colorFilter.value, colour, 0.1f);
                else
                    cg.colorFilter.value = Color.Lerp(cg.colorFilter.value, healColour, 0.1f);
            }
            else
            {
                m_damageLight.intensity = Mathf.Lerp(m_damageLight.intensity, 40, 0.7f);
                cg.colorFilter.value = Color.Lerp(cg.colorFilter.value, damageColour, 0.1f);
            }
    }

    // Damages and heals the player
    public void Damage(float dmg)
    {
        lastHit = Time.time;
        health -= dmg;
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health <= 0)
        {
            ParticleSystem vfx = Instantiate(dieVFX);
            vfx.transform.position = transform.position;
            vfx.transform.eulerAngles = transform.eulerAngles;
            GameManager.me.StartLose();
            Destroy(gameObject);
        }
        else
        {
            ParticleSystem vfx = Instantiate(hitVFX);
            vfx.transform.position = transform.position;
            vfx.transform.eulerAngles = transform.eulerAngles;
        }
    }
    public void Heal(float heal, bool healFX)
    {
        if (healFX)
        {
            lastHeal = Time.time;
            HintsUI.me.FlashGainHealth();
        }
        health += heal;
        health = Mathf.Clamp(health, 0, maxHealth);
    }
}
