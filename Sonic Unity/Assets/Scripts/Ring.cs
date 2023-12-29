using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public GameObject player;
    public GameObject gameController;
    public GameObject sfxManager;

    /// <summary>
    /// Asigna el GameController encontrando un GameObject con la etiqueta "GC".
    /// </summary>
    /// </summary>
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GC");
        sfxManager = GameObject.FindGameObjectWithTag("SFX");
    }
    /// <summary>
    /// Rota el objeto alrededor del eje Y.
    /// </summary>
    void Update()
    {
        transform.Rotate(new Vector3(0f, 1f, 0f));
    }
     /// <summary>
    /// Verifica si el objeto que entra es un jugador y, si es así, realiza acciones
    /// como agregar anillos, puntos, reproducir sonidos y actualizar vidas en GameController.
    /// </summary>
    /// <param name="other">El Collider que entra en la zona de activación.</param>
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
