using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonScript : MonoBehaviour
{
    [HideInInspector] public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the variables
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = transform.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
