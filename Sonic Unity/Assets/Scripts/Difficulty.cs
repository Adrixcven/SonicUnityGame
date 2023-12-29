using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Difficulty : MonoBehaviour
{
    public static Difficulty instance;

    public int currentDifficulty = 1;
    /// <summary>
    /// Asegura que solo haya una instancia de este objeto revisando si ya existe una instancia.
    /// Si no existe, marca el objeto para que persista a través de cambios de escena.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Establece el nivel de dificultad al valor especificado.
    /// </summary>
    /// <param name="num">Número que representa el nuevo nivel de dificultad.</param>
    public void SetDifficulty(int num)
    {
        currentDifficulty = num;
    }
}
