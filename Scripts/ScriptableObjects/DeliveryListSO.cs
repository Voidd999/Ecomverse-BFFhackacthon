using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/DeliveryList")]
public class DeliveryListSO : ScriptableObject
{
    public List<ProductSO> list;
}
