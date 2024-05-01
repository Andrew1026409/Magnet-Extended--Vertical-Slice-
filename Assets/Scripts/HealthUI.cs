using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    // Variables
    public static HealthUI me;
    public Image healthBar;
    public Image healthBarAfter;

    void Awake()
    {
        me = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Updates the health bar on update
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, PlayerMovement.me.health / PlayerMovement.me.maxHealth, 0.1f);
        healthBarAfter.fillAmount = Mathf.Lerp(healthBarAfter.fillAmount, healthBar.fillAmount, 0.005f);
    }
}
