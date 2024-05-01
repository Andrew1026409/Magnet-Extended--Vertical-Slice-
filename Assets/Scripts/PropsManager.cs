using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsManager : MonoBehaviour
{
    // Variables for the props
    public PropScript[] propPrefs;
    public float maxPropCount = 15;
    public float propSpawnRate = 0.5f;
    public float spawnRadius = 50;
    public static PropsManager me;
    public List<PropScript> props = new List<PropScript>();
    float lastSpawn;

    private void Awake()
    {
        me = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Spawns a prop every couple of seconds
        if (Time.time > lastSpawn + propSpawnRate & props.Count <= maxPropCount)
            SpawnProp();
    }

    public void SpawnProp()
    {
        if (!FindObjectOfType<PlayerMovement>()) return;
        lastSpawn = Time.time;
        PropScript AI = Instantiate(propPrefs[Random.Range(0, propPrefs.Length - 1)]);
        Vector3 pos = FindObjectOfType<PlayerMovement>().transform.position;
        pos += new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
        AI.transform.position = pos;
        props.Add(AI);
    }
}
