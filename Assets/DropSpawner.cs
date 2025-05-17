using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DropType
{
    public GameObject prefab;
    public float weight;
}

public class DropSpawner : MonoBehaviour
{
    public List<DropType> drops;
    private float spawnInterval = 2f;

    float timer;
    float halfWidth;
    float topY;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Here we load the area of the screen where the drops will spawn
        // since this is a static screen game, we can just calculate them
        float orthoSize = Camera.main.orthographicSize;
        float aspect = Camera.main.aspect;

        halfWidth = orthoSize * aspect;
        topY = Camera.main.transform.position.y + orthoSize;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnDrop();
            timer = 0f;
        }
    }

    private void SpawnDrop()
    {
        float randomX = Random.Range(-halfWidth, halfWidth);
        Vector3 spawnPos = new Vector3(randomX, topY + 1f, 0f); // Add 1f so its above the screen
        Instantiate(GetRandomDrop(), spawnPos, Quaternion.identity);
    }

    private GameObject GetRandomDrop()
    {
        float totalWeight = drops.Sum(d => d.weight);

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;
        
        foreach (var drop in drops)
        {
            cumulative += drop.weight;
            if (roll <= cumulative) return drop.prefab;
        }

        // In theory the function should never return this
        // its here just as a fallback
        return drops[0].prefab; 
    }
}
