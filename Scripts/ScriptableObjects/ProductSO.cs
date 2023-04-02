using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Product")]
public class ProductSO : ScriptableObject
{
    //  public int id;
    public string productName;
    public string brand;
    public float value;
    public string weigth;
    public string manufactureDate;
    public string expirationDate;
    public int ingredients;
    public string allergies;
    public int stock;
    public Transform model3D;
    public RenderTexture renderTexture;

    [HideInInspector] public int quantity;
    [HideInInspector] public int quantityPlus;
}
