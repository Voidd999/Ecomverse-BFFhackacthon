using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PDPController : MonoBehaviour
{
    public GameObject UIDetails;
    private GameObject UIButtons;
    // private GameObject UIDetailsNextPage;
    public static PDPController Instance;
    private int quantity;
    [SerializeField] TextMeshProUGUI quantityText;
    private PDPListSO pdpList;
    private bool isEnableAddToCart;
    private bool isPDPOpen;

    //notifier
    private GameObject notifierUI;
    private Image notifierUIImage;
    private GameObject notifier;
    [SerializeField] private TextMeshProUGUI notifierText;

    private void Start()
    {
        isEnableAddToCart = false;
        isPDPOpen = false;
        Raycast raycast = Raycast.Instance;
        raycast.OnObjectChangeRayPDP += Raycast_OnObjectChangeRayPDP;

        pdpList = Resources.Load<PDPListSO>(typeof(PDPListSO).Name);

        //only for not need to remove manually in test
        pdpList.list.Clear();
        // 


        quantity = 1;
        Instance = this;
        UIDetails = GameObject.Find("details");
        UIDetails.SetActive(false);
        UIButtons = GameObject.Find("Buttons");
        UIButtons.SetActive(false);
        // UIDetailsNextPage = GameObject.Find("nextPage");
        // UIDetailsNextPage.SetActive(false);

        //notifier
        notifier = GameObject.Find("Notifier");
        notifierUI = GameObject.Find("NotifierBackgroundPDP");
        notifierUIImage = notifierUI.GetComponent<Image>();
        notifierUI.SetActive(false);
    }

    private void Raycast_OnObjectChangeRayPDP(object sender, Raycast.OnObjectChangeRayPDPArgs e)
    {
        if (isEnableAddToCart)
        {
            if (!pdpList.list.Contains(e.productSO))
            {
                //just for clean quantity on initialization
                //maybe create a bug need analise
                e.productSO.quantity = 0;
                e.productSO.quantityPlus = 0;

                if (quantity <= e.productSO.stock)
                {
                    e.productSO.quantity = quantity;
                    pdpList.list.Add(e.productSO);
                    CartController.Instance.CreateProductOnCart(pdpList);

                    //refactor to notifier
                    notifierText.text = ($"Successfully added {e.productSO.productName} x  {quantity} to your cart.");
                    notifierUIImage.color = Color.grey;
                    notifierText.color = Color.green;
                    GameObject notifierGO = Instantiate(notifierUI, notifier.transform);
                    notifierGO.SetActive(true);
                    Destroy(notifierGO, 3f);
                }
                else
                {
                    int avaliableQuantity = e.productSO.stock - e.productSO.quantity;
                    if (avaliableQuantity < 0)
                    {
                        avaliableQuantity = 0;
                    }

                    //refactor to notifier
                    notifierText.text = ($"The item does not have enough stock to be added to the cart. Available Stock:{avaliableQuantity}. Please try again!");
                    notifierUIImage.color = Color.white;
                    notifierText.color = Color.red;
                    GameObject notifierGO = Instantiate(notifierUI, notifier.transform);
                    notifierGO.SetActive(true);
                    Destroy(notifierGO, 3f);

                }
            }
            else
            {
                if (e.productSO.quantity + quantity <= e.productSO.stock)
                {
                    e.productSO.quantityPlus = quantity;
                    CartController.Instance.UpdateCart(e.productSO);

                    //refactor to notifier
                    notifierText.text = ($"The quantity of the item {e.productSO.productName} was added in {quantity} units.");
                    notifierUIImage.color = Color.grey;
                    notifierText.color = Color.green;
                    GameObject notifierGO = Instantiate(notifierUI, notifier.transform);
                    notifierGO.SetActive(true);
                    Destroy(notifierGO, 3f);
                }
                else
                {
                    int avaliableQuantity = e.productSO.stock - e.productSO.quantity;
                    if (avaliableQuantity < 0)
                    {
                        avaliableQuantity = 0;
                    }

                    //refactor to notifier
                    notifierText.text = ($"The item does not have enough stock to be added to the cart. Available Stock: {avaliableQuantity}. Please try again!");
                    notifierUIImage.color = Color.white;
                    notifierText.color = Color.red;
                    GameObject notifierGO = Instantiate(notifierUI, notifier.transform);
                    notifierGO.SetActive(true);
                    Destroy(notifierGO, 3f);
                }
            }
            isEnableAddToCart = false;
        }
    }

    public bool GetPDPStatus()
    {
        return isPDPOpen;
    }

    public void ShowPDP()
    {
        UIDetails.SetActive(true);
        UIButtons.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        //  Time.timeScale = 0f;
        isPDPOpen = true;
    }

    public void HidePDP()
    {
        quantity = 1;
        quantityText.text = quantity.ToString();
        UIDetails.SetActive(false);
        // UIDetailsNextPage.SetActive(false);
        UIButtons.SetActive(false);
        // Cursor.lockState = CursorLockMode.Locked;
        //   Time.timeScale = 1f;
        isPDPOpen = false;
    }

    public void NextPage()
    {
        // UIDetails.SetActive(false);
        // UIButtons.SetActive(true);
        // UIDetailsNextPage.SetActive(true);
    }

    public void BackFistPagePDP()
    {
        UIButtons.SetActive(true);
        // UIDetailsNextPage.SetActive(false);
        UIDetails.SetActive(true);
    }

    public void AddToWishList()
    {
        Debug.Log($"Add To WishList: {quantity} ");
    }

    public void AddToCart()
    {
        isEnableAddToCart = true;
    }

    public void IncreaseQuantity()
    {
        quantity++;
        quantityText.text = quantity.ToString();
    }

    public void DecreaseQuantity()
    {
        if (quantity > 1)
        {
            quantity--;
        }

        quantityText.text = quantity.ToString();
    }
}

