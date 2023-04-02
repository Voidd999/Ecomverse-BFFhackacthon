using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IProduct
{
    public void Selected(Raycast.OnObjectSelectedArgs e);
    public void UnSelected(Raycast.OnObjectUnSelectedArgs e);
    
}
