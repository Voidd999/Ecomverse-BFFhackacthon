using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Button EButton;

    private bool isCartOpen;
    public bool isCheckoutOpen;
    bool clicked = false;
    void Start()
    {
        EButton.onClick.AddListener(OnButtonPress);
        // Application.targetFrameRate = 90;
        Raycast raycast = Raycast.Instance;
        raycast.OnObjectSelected += Raycast_OnObjectSelected;
    }

    private void Raycast_OnObjectSelected(object sender, Raycast.OnObjectSelectedArgs e)
    {
        isCartOpen = CartController.Instance.GetCartStatus();
        isCheckoutOpen = CheckoutController.Instance.GetCheckoutStatus();
        if (Input.GetKeyDown(KeyCode.E) && !isCartOpen && !isCheckoutOpen)
        {
            clicked = false;
            ToolTip.Instance.HideToolTipUI();
            PDPController.Instance.ShowPDP();
        }
    }
    public void OnButtonPress()
    {
        clicked = true;
    }
}
