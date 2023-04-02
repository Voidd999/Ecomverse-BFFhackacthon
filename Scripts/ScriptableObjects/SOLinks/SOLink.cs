using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SOLink : MonoBehaviour
{
    public static SOLink instance;
    [SerializeField] private ProductSO productSO;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI brandText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private TextMeshProUGUI weigthText;
    [SerializeField] private TextMeshProUGUI expirationText;
    [SerializeField] private RawImage renderTexture;

    public static ProductSO GetSO_Static()
    {
        return instance.GetSO();
    }


    public ProductSO GetSO()
    {
        return productSO;
    }

    void Start()
    {
        instance = this;

        Raycast raycast = Raycast.Instance;
        raycast.OnObjectChangeRay += Raycast_OnObjectChangeRay;
        
    }

    private void Raycast_OnObjectChangeRay(object sender, Raycast.OnObjectChangeRayArgs e)
    {
        nameText.text = e.productSO.productName;
        brandText.text = e.productSO.brand;
        valueText.text = e.productSO.value.ToString();
        weigthText.text = e.productSO.weigth;
        expirationText.text = e.productSO.expirationDate;
        renderTexture.texture = e.productSO.renderTexture;
    }

}
