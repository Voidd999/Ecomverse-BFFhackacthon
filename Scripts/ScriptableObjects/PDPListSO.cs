using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PDPList")]
public class PDPListSO : ScriptableObject
{
    public List<ProductSO> list;
}
