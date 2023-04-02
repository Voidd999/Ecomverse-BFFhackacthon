using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckoutController : MonoBehaviour
{
    public static CheckoutController Instance;
    [SerializeField] private TextMeshProUGUI totalValue;
    [SerializeField] private TextMeshProUGUI walletTotalText;
    [SerializeField] private TextMeshProUGUI totalValueCheckout;
    private float valueProduct;
    private CartListSO cartList;
    private PDPListSO pdpList;
    private DeliveryListSO deliveryList;
    private List<float> tempListValue;
    private int totalItens = 0;
    private float walletMoney = 1000000.00f;

    private GameObject checkoutUI;

    private bool isPDPOpen;
    private bool isCartOpen;
    private bool isCheckoutOpen = false;

    //notifier
    private GameObject notifierUI;
    private Image notifierUIImage;
    public int totalAmount = 0;
    private GameObject notifier;
    [SerializeField] private TextMeshProUGUI notifierText;

    private void Start()
    {
        Instance = this;

        valueProduct = 0f;

        cartList = Resources.Load<CartListSO>(typeof(CartListSO).Name);
        pdpList = Resources.Load<PDPListSO>(typeof(PDPListSO).Name);
        deliveryList = Resources.Load<DeliveryListSO>(typeof(DeliveryListSO).Name);

        checkoutUI = GameObject.Find("CheckoutUI");
        checkoutUI.SetActive(false);

        walletTotalText.text = walletMoney.ToString("F2");

        ShowOrHideValue();

        //only for not need to remove manually in test
        deliveryList.list.Clear();

        //notifier
        notifier = GameObject.Find("Notifier");
        notifierUI = GameObject.Find("NotifierBackgroundCheckout");
        notifierUIImage = notifierUI.GetComponent<Image>();
        notifierUI.SetActive(false);

    }

    private void Update()
    {

        //refactor in future
    }

    public void ShowCheckout()
    {
        CheckIfCheckoutCanBeOpen();
        if (!isPDPOpen && !isCartOpen)
        {
            if (!isCheckoutOpen)
            {
                checkoutUI.SetActive(true);
                //  Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                isCheckoutOpen = true;
            }
        }
    }


    public void HideCheckout()
    {

        CheckIfCheckoutCanBeOpen();
        if (!isPDPOpen && !isCartOpen)
        {
            if (isCheckoutOpen)
            {
                checkoutUI.SetActive(false);
                //   Time.timeScale = 1f;
                // Cursor.lockState = CursorLockMode.Locked;
                // isCheckoutOpen = false;
            }
        }
    }

    private void CheckIfCheckoutCanBeOpen()
    {
        isPDPOpen = PDPController.Instance.GetPDPStatus();
        isCartOpen = CartController.Instance.GetCartStatus();
    }
    private void ShowOrHideValue()
    {
        if (totalValue.text == "0,00")
        {
            totalValue.gameObject.SetActive(false);
        }
        else
        {
            totalValue.gameObject.SetActive(true);
        }

        if (totalValueCheckout.text == "0,00")
        {
            totalValueCheckout.gameObject.SetActive(false);
        }
        else
        {
            totalValueCheckout.gameObject.SetActive(true);
        }
    }

    public bool GetCheckoutStatus()
    {
        return isCheckoutOpen;
    }

    public void UpdateValue()
    {
        int i = 0;
        totalItens = CartController.Instance.GetCartListAmmount();
        tempListValue = new List<float>(new float[totalItens]);

        foreach (var item in cartList.list)
        {
            tempListValue[i] = item.quantity * item.value;
            i++;
        }

        foreach (var val in tempListValue)
        {
            valueProduct += val;
        }
        totalAmount = ((int)valueProduct);

        totalValue.text = valueProduct.ToString("F2");
        totalValueCheckout.text = valueProduct.ToString("F2");



        bool isRemoved = CartController.Instance.GetRemovedStatus();

        if (isRemoved)
        {
            var valueToRemove = CartController.Instance.GetItemValueToRemove();
            valueProduct -= valueToRemove;
            totalValue.text = valueProduct.ToString("F2");
            totalValueCheckout.text = valueProduct.ToString("F2");
        }

        valueProduct = 0f;
        tempListValue.Clear();

        ShowOrHideValue();
    }

    public void FinishBuy()
    {
        if (cartList.list.Count == 0)
        {
            notifierText.text = ($"Empty cart! No purchases were made");
            notifierUIImage.color = Color.white;
            notifierText.color = Color.red;
            GameObject notifierGOEmpty = Instantiate(notifierUI, notifier.transform);
            notifierGOEmpty.SetActive(true);
            Destroy(notifierGOEmpty, 3f);

            HideCheckout();
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            float totalBuy = float.Parse(totalValueCheckout.text);

            foreach (ProductSO item in cartList.list)
            {
                deliveryList.list.Add(item);
                item.stock -= item.quantity;
            }

            walletMoney -= totalBuy;
            walletTotalText.text = walletMoney.ToString("F2");
            totalValue.text = "0,00";
            totalValueCheckout.text = valueProduct.ToString("F2");

            CartController.Instance.ClearCheckoutUI();
            CartController.Instance.ClearCartUI();
            CartController.Instance.ClearDictionary();

            //see better approach
            // cartList.list.Clear();
            // pdpList.list.Clear();

            HideCheckout();
            SceneManager.LoadScene("Assets/Scenes/checkoutDetails.unity");


            ShowOrHideValue();


            //refactor to notifier
            notifierText.text = ($"Purchase completed successfully, your items have been added to delivery! Thanks for the Purchase!");
            notifierUIImage.color = Color.grey;
            notifierText.color = Color.green;
            GameObject notifierGO = Instantiate(notifierUI, notifier.transform);
            notifierGO.SetActive(true);
            Destroy(notifierGO, 3f);


        }
    }
}
