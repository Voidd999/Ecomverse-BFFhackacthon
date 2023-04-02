using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PDPLink : MonoBehaviour
{
    public static PDPLink Instance;
    [SerializeField] private ProductSO productSO;

    [SerializeField] private TextMeshProUGUI nameText;
    // [SerializeField] private TextMeshProUGUI nameTextNextPage;
    [SerializeField] private TextMeshProUGUI brandText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private TextMeshProUGUI weigthText;
    [SerializeField] private TextMeshProUGUI manufactureText;
    [SerializeField] private TextMeshProUGUI expirationText;
    [SerializeField] private TextMeshProUGUI ingredientsText;
    [SerializeField] private TextMeshProUGUI allergiesText;
    [SerializeField] private RawImage renderTexture;
    // [SerializeField] private RawImage nextPagerenderTexture;


    public ProductSO GetSO()
    {
        return productSO;
    }

    void Start()
    {
        Instance = this;

        Raycast raycast = Raycast.Instance;
        raycast.OnObjectChangeRayPDP += Raycast_OnObjectChangeRayPDP;

    }

    private void Raycast_OnObjectChangeRayPDP(object sender, Raycast.OnObjectChangeRayPDPArgs e)
    {
        nameText.text = e.productSO.productName;
        brandText.text = e.productSO.brand;
        valueText.text = e.productSO.value.ToString("F2");
        weigthText.text = e.productSO.weigth;
        expirationText.text = e.productSO.expirationDate;
        manufactureText.text = e.productSO.manufactureDate;
        // ingredientsText.text = e.productSO.ingredients.ToString();
        // allergiesText.text = e.productSO.allergies;
        renderTexture.texture = e.productSO.renderTexture;

        //NextPage PDP
        // nameTextNextPage.text = e.productSO.productName;
        // nextPagerenderTexture.texture = e.productSO.renderTexture;
    }
}
