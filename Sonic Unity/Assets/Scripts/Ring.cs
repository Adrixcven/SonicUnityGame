using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public GameObject player;
    public GameObject gameController;
    public GameObject sfxManager;

    // Update is called once per frame

    /// <summary>
    /// Assigns the GameController by finding a GameObject with the tag "GC".
    /// </summary>
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GC");
        sfxManager = GameObject.FindGameObjectWithTag("SFX");
    }
    /// <summary>
    /// Rotates the object around the Y-axis.
    /// </summary>
    void Update()
    {
        transform.Rotate(new Vector3(0f, 1f, 0f));
    }
    /// <summary>
    /// Checks if the entering object is a player, and if so, performs actions
    /// such as adding rings, points, playing sounds, and updating lives in the GameController.
    /// </summary>
    /// <param name="other">The Collider entering the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        GameObject enteringObject = other.transform.parent.gameObject;
        if (enteringObject.CompareTag("Player"))
        {
            gameController.GetComponent<GameController>().AddRings(1);
            gameController.GetComponent<GameController>().AddPointsRing();
            sfxManager.GetComponent<MusicManager>().PlayRingsGot();
            if (gameController.GetComponent<GameController>().rings % 100 == 0)
            {
                gameController.GetComponent<GameController>().AddLives(1);
                sfxManager.GetComponent<MusicManager>().PlayLifeUp();
            }
            gameObject.SetActive(false);
        }
    }
}
