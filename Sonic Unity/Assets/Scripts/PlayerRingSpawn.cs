using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRingSpawn : MonoBehaviour
{

    public GameObject ringObject;
    /// <summary>
    /// Genera anillos alrededor del objeto en una dirección aleatoria.
    /// Calcula un ángulo aleatorio, lo convierte a radianes y determina la posición de generación en un círculo.
    /// Instancia un objeto de anillo en la posición de generación calculada.
    /// </summary>
    public void SpawnRings()
    {
        float angle = Random.Range(0f, 360f);
        float radians = angle * Mathf.Deg2Rad;

        Vector3 spawnPosition = new Vector3(Mathf.Cos(radians), 0.2f, Mathf.Sin(radians)) * 0.5f;

        Instantiate(ringObject, transform.position + spawnPosition, Quaternion.identity);
    }
}
