using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCloser : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Check if the backtick (`) key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Close the game
            CloseGame();
        }
    }

    // Function to close the game
    void CloseGame()
    {
        // Check if the application is running in the Unity editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Close the application
        Application.Quit();
#endif
    }
}
