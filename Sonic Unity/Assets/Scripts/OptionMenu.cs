using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OptionMenu : MonoBehaviour
{
    private static OptionMenu instance;
    public Toggle fullscreenTog;
    public List<resItem> resolutions = new List<resItem>();
    private int selectedRes;
    public TMP_Text resolutionTXT;
    /// <summary>
    /// Configura el patrón singleton para garantizar que solo exista una instancia en todas las escenas.
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
    /// Comprueba si la resolución actual de la pantalla coincide con alguna resolución almacenada.
    /// Si se encuentra, actualiza la resolución seleccionada; de lo contrario, agrega la resolución actual a la lista.
    /// </summary>
    public void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;
        bool foundRes = false;
        
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;
                selectedRes = i;
                UpdateResTXT();
            }
        }
        if (!foundRes)
        {
            resItem newRes = new resItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;
            resolutions.Add(newRes);
            selectedRes = resolutions.Count - 1;
            UpdateResTXT();
        }
    }
    /// <summary>
    /// Cambia entre el modo de pantalla completa y actualiza la resolución de la pantalla en consecuencia.
    /// </summary>
    public void OnToggleFullscreen()
    {
        Screen.SetResolution(resolutions[selectedRes].horizontal, resolutions[selectedRes].vertical, fullscreenTog.isOn);
        print("changed");
    }

    /// <summary>
    /// Disminuye el índice de la resolución seleccionada y actualiza la interfaz de usuario y la resolución de la pantalla.
    /// Asegura que el índice no sea inferior a cero.
    /// </summary>
    public void ResLeft()
    {
        selectedRes--;
        if (selectedRes < 0)
        {
            selectedRes = 0;
        }
        UpdateResTXT();

        Screen.SetResolution(resolutions[selectedRes].horizontal, resolutions[selectedRes].vertical, fullscreenTog.isOn);
    }
    /// <summary>
    /// Aumenta el índice de la resolución seleccionada y actualiza la interfaz de usuario y la resolución de la pantalla.
    /// Asegura que el índice no supere el máximo de resoluciones disponibles.
    /// </summary>
    public void ResRight()
    {
        selectedRes++;
        if (selectedRes > resolutions.Count - 1)
        {
            selectedRes = resolutions.Count - 1;
        }
        UpdateResTXT();
        Screen.SetResolution(resolutions[selectedRes].horizontal, resolutions[selectedRes].vertical, fullscreenTog.isOn);
    }
    /// <summary>
    /// Actualiza el texto de visualización de la resolución según la resolución actualmente seleccionada.
    /// </summary>
    public void UpdateResTXT()
    {
        resolutionTXT.text = resolutions[selectedRes].horizontal.ToString() + " x " + resolutions[selectedRes].vertical.ToString();
    }
}
/// <summary>
/// Resolución del Item
/// </summary>
[System.Serializable]
public class resItem
{
    public int horizontal, vertical;
}
