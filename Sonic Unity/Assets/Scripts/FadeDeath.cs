using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FadeDeath : MonoBehaviour
{
    public GameController gameController;
    /// <summary>
    /// Asigna el componente GameController encontrando un GameObject con la etiqueta "GC".
    /// </summary>
    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GC").GetComponent<GameController>();
    }
    /// <summary>
    /// Inicia una función de muerte por desvanecimiento, activando el metodo de reaparición en el GameController,
    /// y reproduciendo una animación "FadeOff" en el GameObject actual.
    /// </summary>
    public async void FadeDeathFunction()
    {
        gameController.GetComponent<GameController>().Respawn();
        gameObject.GetComponent<Animation>().Play("FadeOff");
    }
    /// <summary>
    /// Desactiva el GameObject estableciendo su estado activo como falso.
    /// </summary>
    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
