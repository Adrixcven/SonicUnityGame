using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeDeath : MonoBehaviour
{
    public GameController gameController;
    /// <summary>
    /// Assigns the GameController component by finding a GameObject with the tag "GC".
    /// </summary>
    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>();
    }
    /// <summary>
    /// Initiates a fade death function, triggering a delayed respawn routine in the GameController,
    /// and playing a "FadeOff" animation on the current GameObject.
    /// </summary>
    public void FadeDeathFunction()
    {
        StartCoroutine(gameController.GetComponent<GameController>().DelayedRespawnRoutine());
        gameObject.GetComponent<Animation>().Play("FadeOff");
    }
    /// <summary>
    /// Disables the GameObject by setting its active state to false
    /// </summary>
    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
