using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRingSpawn : MonoBehaviour
{

    public GameObject ringObject;
    /// <summary>
    /// Spawns rings around the object in a random direction.
    /// Calculates a random angle, converts it to radians, and determines the spawn position on a circle.
    /// Instantiates a ring object at the calculated spawn position.
    /// </summary>
    public void SpawnRings()
    {
        float angle = Random.Range(0f, 360f);
        float radians = angle * Mathf.Deg2Rad;

        Vector3 spawnPosition = new Vector3(Mathf.Cos(radians), 0.2f, Mathf.Sin(radians)) * 0.5f;

        Instantiate(ringObject, transform.position + spawnPosition, Quaternion.identity);
    }
}
