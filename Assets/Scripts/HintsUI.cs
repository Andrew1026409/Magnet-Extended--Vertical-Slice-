using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintsUI : MonoBehaviour
{
    // The variables for the hints
    public static HintsUI me;
    public CanvasGroup surviveT;
    public CanvasGroup movementT;
    public CanvasGroup magnetPullT;
    public CanvasGroup magnetSpinT;
    public CanvasGroup magnetLaunchT;
    public Animator healthAni;

    private void Awake()
    {
        me = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Sets the variables at the start
        surviveT.alpha = 1;
        movementT.alpha = 1;
        magnetPullT.alpha = 0;
        magnetSpinT.alpha = 0;
        magnetLaunchT.alpha = 0;
        healthAni.GetComponent<CanvasGroup>().alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Shows the hints after the intro
        if (Time.timeSinceLevelLoad >= 3)
            surviveT.alpha = Mathf.Lerp(surviveT.alpha, 0, 0.03f);
        // Shows the hints after the intro
        if (Time.timeSinceLevelLoad >= 3)
        {
            // Shows the hints at the appropriate time
            movementT.alpha = Mathf.Lerp(movementT.alpha, 0, 0.03f);
            if (MagnetScript.me.isPulling)
            {
                magnetSpinT.alpha = Mathf.Lerp(magnetSpinT.alpha, 1, 0.1f);
                magnetPullT.alpha = Mathf.Lerp(magnetPullT.alpha, 0, 0.1f);
            }
            else
            {
                magnetSpinT.alpha = Mathf.Lerp(magnetSpinT.alpha, 0, 0.1f);
                magnetPullT.alpha = Mathf.Lerp(magnetPullT.alpha, 1, 0.1f);
            }
            if (MagnetScript.me.isSpinning)
                magnetLaunchT.alpha = Mathf.Lerp(magnetLaunchT.alpha, 1, 0.1f);
            else
                magnetLaunchT.alpha = Mathf.Lerp(magnetLaunchT.alpha, 0, 0.1f);
        }
    }

    // Flash when an AI is killed
    public void FlashGainHealth()
    {
        healthAni.CrossFadeInFixedTime("Flash", 0.1f);
    }
}
