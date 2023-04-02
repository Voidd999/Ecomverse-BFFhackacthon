using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public static ToolTip Instance;

    public GameObject UI;

    private void Start()
    {
        Instance = this;

        UI = GameObject.Find("toolTip");
        UI.SetActive(false);
    }

    public void ShowToolTipUI()
    {
        UI.SetActive(true);
    }

    public void HideToolTipUI()
    {
        UI.SetActive(false);
    }
}
