using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // variables
    public static GameManager me;
    public int minutes = 3;
    public int seconds = 0;
    public GameObject winUI;
    public GameObject loseUI;
    public float lastTime;
    public int difficulty;
    [HideInInspector] public float maxMinutes;
    [HideInInspector] public float maxSeconds;
    [SerializeField] public bool over;

    private void Awake()
    {
        me = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Sets the timer
        maxMinutes = minutes;
        maxSeconds = seconds;
    }

    // Update is called once per frame
    void Update()
    {
        // Counts down the timer
        if(seconds <= 0)
        {
            minutes -= 1;
            seconds = 60;
        }
        if (Time.timeSinceLevelLoad >= lastTime + 1)
        {
            seconds -= 1;
            lastTime = Time.timeSinceLevelLoad;
        }
        // Triggers the lose state when timer is complete
        if (minutes <= 0 & seconds <= 0)
            if (!over)
                StartCoroutine(Complete());
    }
    
    // when lose or win
    public void StartLose()
    {
        StartCoroutine(Lose());
    }
    public IEnumerator Complete()
    {
        over = true;
        Instantiate(winUI);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }
    public IEnumerator Lose()
    {
        over = true;
        Instantiate(loseUI);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }
}
