using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    // The variables for the AI
    public AIBehaviour[] aiPref;
    public float maxAICount = 6;
    public float aiSpawnRate = 1;
    public float spawnRadius = 60;
    public static AIManager me;
    public List<AIBehaviour> ais = new List<AIBehaviour>();
    float lastSpawn;

    private void Awake()
    {
        me = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Spawns the AI every couple of seconds if the maxiumum count hasn't been reached
        if(Time.timeSinceLevelLoad > 1 & Time.time > lastSpawn + aiSpawnRate & ais.Count <= maxAICount)
            SpawnAI();
    }

    public void SpawnAI()
    {
        if (!FindObjectOfType<PlayerMovement>()) return;
        lastSpawn = Time.time;
        AIBehaviour AI = Instantiate(aiPref[Random.Range(0, aiPref.Length - 1)]);
        Vector3 pos = FindObjectOfType<PlayerMovement>().transform.position;
        pos += new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
        AI.transform.position = pos;
        ais.Add(AI);
    }
}
