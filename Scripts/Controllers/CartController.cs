using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CartController : MonoBehaviour
{
    public static CartController Instance;
    private GameObject cartUI;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI brandText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private TextMeshProUGUI weigthText;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private RawImage rawImage;

    private List<GameObject> cartUIList = new List<GameObject>();

    private int objIndex;

    private CartListSO cartList;

    private GameObject cartItemTemplate;
    private GameObject scrollUI;

    private ProductSO productSO;
    private PDPListSO pdpListSO;

    private Dictionary<int, ProductSO> productDictionary;

    private bool isPDPOpen;
    private bool isCartOpen;
    private bool isRemoved;
    private bool isCheckoutOpen;
    private float itemValueToRemove;
    public Button CButton;
    //notifier
    private GameObject notifierUI;
    private Image notifierUIImage;
    private GameObject notifier;
    [SerializeField] private TextMeshProUGUI notifierText;
    bool clicked = false;

    //checkout list
    private List<GameObject> checkoutUIList = new List<GameObject>();
    private GameObject checkoutItemTemplate;
    private GameObject checkoutScrollUI;
    public void OnButtonPress()
    {
        clicked = true;
    }
    void Start()
    {
        // CButton.onClick.AddListener(OnButtonPress);
        Instance = this;
        Debug.Log(CartController.Instance);
        isCartOpen = false;
        isRemoved = false;
        itemValueToRemove = 0;

        cartUI = GameObject.Find("CartUI");

        cartItemTemplate = GameObject.Find("CartItem");
        scrollUI = GameObject.Find("Panel");

        cartUI.SetActive(false);
        cartItemTemplate.SetActive(false);

        pdpListSO = Resources.Load<PDPListSO>(typeof(PDPListSO).Name);
        cartList = Resources.Load<CartListSO>(typeof(CartListSO).Name);

        //only for not need to remove manually in test
        cartList.list.Clear();

        objIndex = 0;
        productDictionary = new Dictionary<int, ProductSO>();

        //checkout list
        checkoutItemTemplate = GameObject.Find("CheckoutItem");
        // Debug.Log(checkoutItemTemplate);
        checkoutItemTemplate.SetActive(false);
        checkoutScrollUI = GameObject.Find("PanelCheckout");

        //notifier
        notifier = GameObject.Find("Notifier");
        notifierUI = GameObject.Find("NotifierBackgroundCart");
        notifierUIImage = notifierUI.GetComponent<Image>();
        notifierUI.SetActive(false);
    }

    private void Update()
    {
        //maybe bad pratice, refactor in future
        isPDPOpen = PDPController.Instance.GetPDPStatus();
        isCheckoutOpen = CheckoutController.Instance.GetCheckoutStatus();

        if (Input.GetKeyDown(KeyCode.C) && !isPDPOpen && !isCheckoutOpen)
        {
            clicked = false;
            if (!isCartOpen)
            {

                cartUI.SetActive(true);
                //  Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                isCartOpen = true;
            }
            else
            {
                cartUI.SetActive(false);
                //  Time.timeScale = 1f;
                // Cursor.lockState = CursorLockMode.Locked;
                isCartOpen = false;
            }

        }
    }

    public void CreateProductOnCart(PDPListSO pdpList)
    {
        foreach (ProductSO product in pdpList.list)
        {
            if (!cartList.list.Contains(product))
            {
                GameObject itemTemplate = Instantiate(cartItemTemplate, scrollUI.transform);
                cartUIList.Add(itemTemplate);

                Transform name = itemTemplate.transform.GetChild(2);
                TextMeshProUGUI nameString = name.GetComponent<TextMeshProUGUI>();
                nameString.text = product.productName;

                Transform brand = itemTemplate.transform.GetChild(3);
                TextMeshProUGUI brandString = brand.GetComponent<TextMeshProUGUI>();
                brandString.text = product.brand;

                Transform value = itemTemplate.transform.GetChild(4);
                TextMeshProUGUI valueString = value.GetComponent<TextMeshProUGUI>();
                valueString.text = (product.value * product.quantity).ToString("F2");

                Transform weigth = itemTemplate.transform.GetChild(5);
                TextMeshProUGUI weigthString = weigth.GetComponent<TextMeshProUGUI>();
                weigthString.text = product.weigth;

                Transform image = itemTemplate.transform.GetChild(6);
                RawImage imageTexture = image.GetComponent<RawImage>();
                imageTexture.texture = product.renderTexture;

                Transform quantity = itemTemplate.transform.GetChild(7);
                quantity = quantity.transform.GetChild(0);
                TextMeshProUGUI quantityText = quantity.GetComponent<TextMeshProUGUI>();
                quantityText.text = product.quantity.ToString();

                //     nameText.text = product.productName;
                //    brandText.text = product.brand;
                //      weigthText.text = product.weigth;
                //     quantityText.text = product.quantity.ToString();
                //     valueText.text = (product.value * product.quantity).ToString();
                //    rawImage.texture = product.renderTexture;


                itemTemplate.SetActive(true);

                //  itemTemplate.GetComponent<RectTransform>().position = scrollUI.GetComponent<RectTransform>().position;

                cartList.list.Add(product);
                productDictionary.Add(objIndex, product);
                objIndex++;

                CheckoutController.Instance.UpdateValue();

                //checkoutList

                GameObject checkoutItem = Instantiate(checkoutItemTemplate, checkoutScrollUI.transform);
                checkoutUIList.Add(checkoutItem);

                Transform nameCheckout = checkoutItem.transform.GetChild(2);
                TextMeshProUGUI nameStringCheckout = nameCheckout.GetComponent<TextMeshProUGUI>();
                nameStringCheckout.text = product.productName;

                Transform brandCheckout = checkoutItem.transform.GetChild(3);
                TextMeshProUGUI brandStringCheckout = brandCheckout.GetComponent<TextMeshProUGUI>();
                brandStringCheckout.text = product.brand;

                Transform valueCheckout = checkoutItem.transform.GetChild(4);
                TextMeshProUGUI valueStringCheckout = valueCheckout.GetComponent<TextMeshProUGUI>();
                valueStringCheckout.text = (product.value * product.quantity).ToString("F2");

                Transform quantityCheckout = checkoutItem.transform.GetChild(5);
                quantityCheckout = quantityCheckout.transform.GetChild(0);
                TextMeshProUGUI quantityTextCheckout = quantityCheckout.GetComponent<TextMeshProUGUI>();
                quantityTextCheckout.text = product.quantity.ToString();

                checkoutItem.SetActive(true);

            }
        }
    }

    public void UpdateCart(ProductSO product)
    {
        foreach (KeyValuePair<int, ProductSO> item in productDictionary)
        {
            if (product.productName == item.Value.productName)
            {
                foreach (GameObject uiTemplate in cartUIList)
                {
                    var tempTest = uiTemplate.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                    if (tempTest.text == product.productName)
                    {
                        Transform quantity = uiTemplate.transform.GetChild(7);
                        quantity = quantity.transform.GetChild(0);
                        TextMeshProUGUI quantityText = quantity.GetComponent<TextMeshProUGUI>();
                        var tempQuantity = product.quantity + product.quantityPlus;
                        quantityText.text = tempQuantity.ToString();
                        product.quantity = tempQuantity;

                        Transform value = uiTemplate.transform.GetChild(4);
                        TextMeshProUGUI valueString = value.GetComponent<TextMeshProUGUI>();
                        valueString.text = (product.value * product.quantity).ToString("F2");

                        CheckoutController.Instance.UpdateValue();

                        //CheckoutList
                        foreach (GameObject checkouItemTemplate in checkoutUIList)
                        {
                            var tempCheckoutProductName = checkouItemTemplate.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                            if (tempCheckoutProductName.text == product.productName)
                            {
                                Transform quantityCheckout = checkouItemTemplate.transform.GetChild(5);
                                quantityCheckout = quantityCheckout.transform.GetChild(0);
                                TextMeshProUGUI quantityTextCheckout = quantityCheckout.GetComponent<TextMeshProUGUI>();
                                quantityTextCheckout.text = tempQuantity.ToString();

                                Transform valueCheckout = checkouItemTemplate.transform.GetChild(4);
                                TextMeshProUGUI valueStringCheckout = valueCheckout.GetComponent<TextMeshProUGUI>();
                                valueStringCheckout.text = (product.value * product.quantity).ToString("F2");
                            }
                        }
                    }
                }
            }
        }
    }

    public void IncreaseQuantityCart()
    {
        Transform clickedButton = EventSystem.current.currentSelectedGameObject.transform;
        foreach (ProductSO product in cartList.list)
        {
            foreach (KeyValuePair<int, ProductSO> item in productDictionary)
            {
                if (product.productName == item.Value.productName)
                {
                    foreach (GameObject uiTemplate in cartUIList)
                    {
                        Transform increaseButton = uiTemplate.transform.GetChild(10);
                        var tempProductName = uiTemplate.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                        if (tempProductName.text == product.productName)
                        {
                            if (clickedButton == increaseButton)
                            {
                                if (product.quantity <= product.stock)
                                {
                                    product.quantity++;
                                    if (product.quantity <= product.stock)
                                    {
                                        product.quantityPlus = product.quantity;

                                        Transform quantity = uiTemplate.transform.GetChild(7);
                                        quantity = quantity.transform.GetChild(0);
                                        TextMeshProUGUI quantityText = quantity.GetComponent<TextMeshProUGUI>();
                                        quantityText.text = product.quantity.ToString();

                                        Transform value = uiTemplate.transform.GetChild(4);
                                        TextMeshProUGUI valueString = value.GetComponent<TextMeshProUGUI>();
                                        valueString.text = (product.value * product.quantity).ToString("F2");

                                        CheckoutController.Instance.UpdateValue();

                                        //checkoutListing
                                        foreach (GameObject checkouItemTemplate in checkoutUIList)
                                        {
                                            var tempCheckoutProductName = checkouItemTemplate.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                                            if (tempCheckoutProductName.text == product.productName)
                                            {
                                                Transform quantityCheckout = checkouItemTemplate.transform.GetChild(5);
                                                quantityCheckout = quantityCheckout.transform.GetChild(0);
                                                TextMeshProUGUI quantityTextCheckout = quantityCheckout.GetComponent<TextMeshProUGUI>();
                                                quantityTextCheckout.text = product.quantity.ToString();

                                                Transform valueCheckout = checkouItemTemplate.transform.GetChild(4);
                                                TextMeshProUGUI valueStringCheckout = valueCheckout.GetComponent<TextMeshProUGUI>();
                                                valueStringCheckout.text = (product.value * product.quantity).ToString("F2");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //just for compensate
                                        product.quantity--;

                                        //refactor to notifier
                                        notifierText.text = ($"Item limit quantity {product.productName} reached!");
                                        notifierUIImage.color = Color.white;
                                        notifierText.color = Color.red;
                                        GameObject notifierGO = Instantiate(notifierUI, notifier.transform);
                                        notifierGO.SetActive(true);
                                        Destroy(notifierGO, 3f);

                                    }

                                }
                                else
                                {

                                    //refactor to notifier
                                    notifierText.text = ($"Item limit quantity {product.productName} reached!");
                                    notifierUIImage.color = Color.white;
                                    notifierText.color = Color.red;
                                    GameObject notifierGO = Instantiate(notifierUI, notifier.transform);
                                    notifierGO.SetActive(true);
                                    Destroy(notifierGO, 3f);
                                }

                            }
                        }

                    }
                }
            }
        }
    }

    public void DecreaseQuantityCart()
    {
        Transform clickedButton = EventSystem.current.currentSelectedGameObject.transform;
        foreach (ProductSO product in cartList.list)
        {
            foreach (KeyValuePair<int, ProductSO> item in productDictionary)
            {
                if (product.productName == item.Value.productName)
                {
                    foreach (GameObject uiTemplate in cartUIList)
                    {
                        Transform decreaseButton = uiTemplate.transform.GetChild(11);
                        var tempProductName = uiTemplate.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                        if (tempProductName.text == product.productName)
                        {
                            if (clickedButton == decreaseButton)
                            {
                                if (product.quantity > 1)
                                {

                                    product.quantity--;
                                    product.quantityPlus = product.quantity;
                                    CheckoutController.Instance.UpdateValue();
                                }
                                Transform quantity = uiTemplate.transform.GetChild(7);
                                quantity = quantity.transform.GetChild(0);
                                TextMeshProUGUI quantityText = quantity.GetComponent<TextMeshProUGUI>();
                                quantityText.text = product.quantity.ToString();

                                Transform value = uiTemplate.transform.GetChild(4);
                                TextMeshProUGUI valueString = value.GetComponent<TextMeshProUGUI>();
                                valueString.text = (product.value * product.quantity).ToString("F2");

                                //checkoutListing
                                foreach (GameObject checkouItemTemplate in checkoutUIList)
                                {
                                    var tempCheckoutProductName = checkouItemTemplate.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                                    if (tempCheckoutProductName.text == product.productName)
                                    {
                                        Transform quantityCheckout = checkouItemTemplate.transform.GetChild(5);
                                        quantityCheckout = quantityCheckout.transform.GetChild(0);
                                        TextMeshProUGUI quantityTextCheckout = quantityCheckout.GetComponent<TextMeshProUGUI>();
                                        quantityTextCheckout.text = product.quantity.ToString();

                                        Transform valueCheckout = checkouItemTemplate.transform.GetChild(4);
                                        TextMeshProUGUI valueStringCheckout = valueCheckout.GetComponent<TextMeshProUGUI>();
                                        valueStringCheckout.text = (product.value * product.quantity).ToString("F2");
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
    }


    public void RemoveFromCart()
    {
        //just for initialize
        var tempKey = 0;
        var tempUIList = gameObject;
        var tempProduct = productSO;
        var tempCheckoutUI = gameObject;

        var clickedButton = EventSystem.current.currentSelectedGameObject.transform;
        foreach (ProductSO product in cartList.list)
        {
            foreach (KeyValuePair<int, ProductSO> item in productDictionary)
            {
                if (product.productName == item.Value.productName)
                {
                    foreach (GameObject uiTemplate in cartUIList)
                    {
                        var removeButton = uiTemplate.transform.GetChild(9);
                        var tempProductName = uiTemplate.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                        if (tempProductName.text == product.productName)
                        {
                            if (removeButton == clickedButton)
                            {
                                var itemCart = uiTemplate.transform.gameObject;
                                if (itemCart)
                                {
                                    pdpListSO.list.Remove(product);
                                    tempUIList = uiTemplate;
                                    tempKey = item.Key;
                                    tempProduct = product;
                                    isRemoved = true;

                                    itemValueToRemove = product.value * product.quantity;

                                    CheckoutController.Instance.UpdateValue();

                                    product.quantity = 0;
                                    product.quantityPlus = 0;

                                    //checkoutListing
                                    foreach (GameObject checkouItemTemplate in checkoutUIList)
                                    {
                                        var tempCheckoutProductName = checkouItemTemplate.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                                        if (tempCheckoutProductName.text == product.productName)
                                        {
                                            if (checkouItemTemplate)
                                            {
                                                tempCheckoutUI = checkouItemTemplate;
                                                Destroy(checkouItemTemplate);
                                            }
                                        }
                                    }

                                    Destroy(itemCart);
                                    isRemoved = false;
                                }
                            }
                        }
                    }
                }
            }
        }
        checkoutUIList.Remove(tempCheckoutUI);
        cartList.list.Remove(tempProduct);
        cartUIList.Remove(tempUIList);
        productDictionary.Remove(tempKey);

        //refactor to notifier
        notifierText.text = ($"The item {tempProduct.productName} has been removed from cart successfully!");
        notifierUIImage.color = Color.white;
        notifierText.color = Color.red;
        GameObject notifierGO = Instantiate(notifierUI, notifier.transform);
        notifierGO.SetActive(true);
        Destroy(notifierGO, 3f);
    }

    public void ClearCartUI()
    {
        foreach (GameObject item in cartUIList)
        {
            if (item)
            {
                Destroy(item);
            }
        }
        cartUIList.Clear();
    }

    public float GetItemValueToRemove()
    {
        return itemValueToRemove;
    }

    public bool GetRemovedStatus()
    {
        return isRemoved;
    }

    public int GetCartListAmmount()
    {
        return cartList.list.Count;
    }

    public bool GetCartStatus()
    {
        return isCartOpen;
    }

    public void ClearDictionary()
    {
        productDictionary.Clear();
    }

    //checkout
    public void ClearCheckoutUI()
    {
        foreach (GameObject item in checkoutUIList)
        {
            if (item)
            {
                Destroy(item);
            }
        }
        checkoutUIList.Clear();
    }
}
