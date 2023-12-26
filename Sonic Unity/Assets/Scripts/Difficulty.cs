using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the difficulty methods.
/// </summary>
public class Difficulty : MonoBehaviour
{
    public static Difficulty instance;

    public int currentDifficulty = 1;
    /// <summary>
    ///  Ensures that there is only one instance of this object by checking for an existing instance and, if it doesn't exist, marks the object to persist across scene loads.
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
    /// Sets the difficulty level to the specified value.
    /// </summary>
    /// <param name="num"></param>
    public void SetDifficulty(int num)
    {
        currentDifficulty = num;
    }
}
