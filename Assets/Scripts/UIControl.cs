using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// This is the difficulty variables
[Serializable]
public class Difficulty
{
    public float maxAICount = 6;
    public float aiSpawnRate = 0.65f;
}

public class UIControl : MonoBehaviour
{
    // The variables for the UI
    public static UIControl me;

    // A list of difficulties
    [SerializeField] public List<Difficulty> difficultyList = new List<Difficulty>();

    void Awake()
    {
        me = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Hides the objects for the game on start
        GameManager.me.gameObject.SetActive(false);
        HealthUI.me.gameObject.SetActive(false);
        PlayerMovement.me.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Hides the cursor during the game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetDifficulty(int difficulty)
    {
        GameManager.me.difficulty = difficulty;
        AIManager.me.maxAICount = difficultyList[difficulty - 1].maxAICount;
        AIManager.me.aiSpawnRate = difficultyList[difficulty - 1].aiSpawnRate;
    }
    
    // Starts the actual game
    public void Play()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.me.gameObject.SetActive(true);
        HealthUI.me.gameObject.SetActive(true);
        PlayerMovement.me.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
