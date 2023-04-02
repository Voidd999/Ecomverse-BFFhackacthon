using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.IO;
// using Newtonsoft.Json;
using Newtonsoft.Json;
using System.Net;
using System;
using System.Text;

public class checkOutInput : MonoBehaviour
{
    public static checkOutInput instance;
    string data;
    [SerializeField] string endpoint = "https://thecodes.tech/wp-json/wc/v3/orders";

    [SerializeField] string consumer_key = "ck_7f7d961ccf64d26dfc73cd5c76d44a562149cae1";
    [SerializeField] string consumer_secret = "cs_66c336182121380fb6b22c56554ba61afdcb5c2b";
    public TMP_InputField firstNameField;
    public TMP_InputField lastNameField;
    public TMP_InputField addressField;
    public TMP_InputField cityField;
    public TMP_InputField stateField;
    public TMP_InputField postCodeField;
    public TMP_InputField countryField;
    public TMP_InputField emailField;
    public TMP_InputField phoneField;
    public Button saveButton;
    public Button backButton;
    public Button confirmButton;
    private Selectable currentSelectable;
    public GameObject downloadLink;
    public TextMeshProUGUI inCafeText;
    private CartListSO cartList;
    public Dictionary<string, object> formDataDict = new Dictionary<string, object>(){
        { "payment_method", "bacs" },
        { "payment_method_title", "Direct Bank Transfer" },
        { "set_paid", false },
        { "billing", new Dictionary<string, object>
            {
                { "first_name", "John" },
                { "last_name", "Doe" },
                { "address_1", "969 Market" },
                { "address_2", "" },
                { "city", "San Francisco" },
                { "state", "CA" },
                { "postcode", "94103" },
                { "country", "US" },
                { "email", "john.doe@example.com" },
                { "phone", "(555) 555-5555" }
            }
        },
        { "shipping", new Dictionary<string, object>
            {
                { "first_name", "John" },
                { "last_name", "Doe" },
                { "address_1", "969 Market" },
                { "address_2", "" },
                { "city", "San Francisco" },
                { "state", "CA" },
                { "postcode", "94103" },
                { "country", "US" }
            }
        },
        { "line_items", new List<Dictionary<string, object>>
            {
                // new Dictionary<string, object>
                // {
                //     { "product_id", 0 },
                //     { "quantity", 2 }
                // },
                // new Dictionary<string, object>
                // {
                //     { "product_id", 1 },
                //     { "quantity", 1 }
                // }
            }
        },
        { "shipping_lines", new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "method_id", "flat_rate" },
                    { "method_title", "Flat Rate" },
                    { "total", "10" }
                }
            }
        }
    };
    public void BackButton()
    {
        SceneManager.LoadScene("Assets/Scenes/Scene_lobby.unity");
    }
    public void Start()
    {

        cartList = Resources.Load<CartListSO>(typeof(CartListSO).Name);

        endpoint = "https://thecodes.tech/wp-json/wc/v3/orders";

        consumer_key = "ck_7f7d961ccf64d26dfc73cd5c76d44a562149cae1";
        consumer_secret = "cs_66c336182121380fb6b22c56554ba61afdcb5c2b";

        instance = this;
        saveButton.onClick.AddListener(SaveInput);
        backButton.onClick.AddListener(BackButton);
        currentSelectable = this.GetComponent<Selectable>();
        // if (CheckoutTrigger.instance.InCafe)
        // {
        //     inCafeText.text = "NOTE: After Completing your order, it will be delivered n 30 minutes.";
        // }
    }

    private void SaveInput()
    {
        string firstName = firstNameField.text;
        string lastName = lastNameField.text;
        string address = addressField.text;
        string city = cityField.text;
        string state = stateField.text;
        string postcode = postCodeField.text;
        string country = countryField.text;
        string email = emailField.text;
        string phone = phoneField.text;


        ((Dictionary<string, object>)formDataDict["billing"])["first_name"] = firstName;
        ((Dictionary<string, object>)formDataDict["shipping"])["first_name"] = firstName;

        ((Dictionary<string, object>)formDataDict["billing"])["last_name"] = lastName;
        ((Dictionary<string, object>)formDataDict["shipping"])["last_name"] = lastName;

        ((Dictionary<string, object>)formDataDict["billing"])["address_1"] = address;
        ((Dictionary<string, object>)formDataDict["shipping"])["address_1"] = address;

        ((Dictionary<string, object>)formDataDict["billing"])["city"] = city;
        ((Dictionary<string, object>)formDataDict["shipping"])["city"] = city;

        ((Dictionary<string, object>)formDataDict["billing"])["state"] = state;
        ((Dictionary<string, object>)formDataDict["shipping"])["state"] = state; ;

        ((Dictionary<string, object>)formDataDict["billing"])["postcode"] = postcode;
        ((Dictionary<string, object>)formDataDict["shipping"])["postcode"] = postcode;

        ((Dictionary<string, object>)formDataDict["billing"])["country"] = country;
        ((Dictionary<string, object>)formDataDict["shipping"])["country"] = country;

        ((Dictionary<string, object>)formDataDict["billing"])["email"] = email;
        ((Dictionary<string, object>)formDataDict["billing"])["phone"] = phone;
        Debug.LogWarning(CartController.Instance);
        try
        {
            int totalItens = CartController.Instance.GetCartListAmmount();
        }
        catch (System.Exception e)
        {
            Debug.LogError("An error occurred: " + e.Message);
        }

        foreach (ProductSO item in cartList.list)
        {
            ((List<Dictionary<string, object>>)formDataDict["line_items"]).Add(new Dictionary<string, object>
{
    { "product_id", item.ingredients },
    { "quantity", item.quantity}
});
        }
        ((List<Dictionary<string, object>>)formDataDict["shipping_lines"])[0]["total"] = CheckoutController.Instance.totalAmount.ToString();


        data = JsonConvert.SerializeObject(formDataDict);

        confirmButton.gameObject.SetActive(true);
        confirmButton.interactable = true;


        // Debug.Log("Input saved: First Name: " + firstName + ", Last Name: " + lastName + ", Address: " + address + ", City: " + city + ", Country: " + country + ", Email: " + email + ", Phone: " + phone);



    }

    public void SendData()
    {
        WebRequest request = WebRequest.Create(endpoint);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(consumer_key + ":" + consumer_secret)));

        using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
        {
            writer.Write(data);
        }

        try
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.Created)
            {
                Debug.Log("Order created successfully");

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string response_content = reader.ReadToEnd();
                    Dictionary<string, object> response_data = JsonConvert.DeserializeObject<Dictionary<string, object>>(response_content);
                    int order_id = Convert.ToInt32(response_data["id"]);
                    Debug.Log("Order ID: " + order_id);
                }
            }
            else
            {
                Console.WriteLine("Order creation failed with status code " + (int)response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    Debug.Log("Response content: " + reader.ReadToEnd());
                }
            }
        }
        catch (WebException ex)
        {
            Debug.Log("API request failed with error:");
            Debug.Log(ex.Message);



        }
        confirmButton.GetComponent<Button>().interactable = false;
    }
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Tab))
    //     {
    //         Selectable nextSelectable = Selectable.FindSelectableOnDown(currentSelectable.gameObject);
    //         if (nextSelectable != null)
    //         {
    //             currentSelectable = nextSelectable;
    //             currentSelectable.Select();
    //         }
    //     }
    // }
}
