using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/CartList")]
public class CartListSO : ScriptableObject
{
    public List<ProductSO> list;
}
