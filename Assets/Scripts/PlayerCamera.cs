using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Variables
    Transform player;
    public float height = 6;
    public float startHeight = 15;
    public float smoothing = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the variables
        transform.SetParent(null);
        player = FindObjectOfType<PlayerMovement>().transform;
        transform.position += new Vector3(0, startHeight, 0);

        // Locks the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        // Moves the camera to the player if the player is active
        if (player)
            transform.position = Vector3.Lerp(transform.position, player.position + new Vector3(0, height, 0), smoothing);
    }
}
