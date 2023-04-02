using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutTrigger : MonoBehaviour
{
    public static CheckoutTrigger instance;
    public string SellerName;
    public bool InCafe;
    void Start()
    {
        instance = this;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CheckoutController.Instance.ShowCheckout();
        }
    }
}
