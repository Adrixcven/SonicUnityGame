using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : MonoBehaviour
{
    public Toggle fullscreenTog;
    public List<resItem> resolutions = new List<resItem>();
    private int selectedRes;
    public TMP_Text resolutionTXT;
    /// <summary>
    /// Checks if the current screen resolution matches any stored resolutions.
    /// If found, updates the selected resolution; otherwise, adds the current resolution to the list.
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
    /// Toggles fullscreen mode and updates the screen resolution accordingly.
    /// </summary>
    public void OnToggleFullscreen()
    {
        Screen.SetResolution(resolutions[selectedRes].horizontal, resolutions[selectedRes].vertical, fullscreenTog.isOn);
        print("changed");
    }

    /// <summary>
    /// Decreases the selected resolution index and updates UI and screen resolution.
    /// Ensures that the index does not go below zero.
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
    /// Increases the selected resolution index and updates UI and screen resolution.
    /// Ensures that the index does not exceed the maximum available resolutions.
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
    /// Updates the resolution display text based on the currently selected resolution.
    /// </summary>
    public void UpdateResTXT()
    {
        resolutionTXT.text = resolutions[selectedRes].horizontal.ToString() + " x " + resolutions[selectedRes].vertical.ToString();
    }
}
[System.Serializable]
public class resItem
{
    public int horizontal, vertical;
}
