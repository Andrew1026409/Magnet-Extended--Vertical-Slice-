using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUI : MonoBehaviour
{
    // Variables
    public TextMeshProUGUI timerT;
    public Image timerBar;

    // Update is called once per frame
    void Update()
    {
        // Updates the time every frame
        timerT.text = GameManager.me.minutes + " Minutes" + "\n" + GameManager.me.seconds + " Seconds Left";
        float time = GameManager.me.minutes * 60 + GameManager.me.seconds;
        float maxTime = GameManager.me.maxMinutes * 60 + GameManager.me.maxSeconds;
        timerBar.fillAmount = Mathf.Lerp(timerBar.fillAmount, time / maxTime, 0.1f);
    }
}
