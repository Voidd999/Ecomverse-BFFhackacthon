using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProductController : MonoBehaviour, IProduct
{

    private Material mat;

    void Start()
    {
        Raycast raycast = Raycast.Instance;
        raycast.OnObjectSelected += Raycast_OnObjectSelected;
        raycast.OnObjectUnSelected += Raycast_OnObjectUnSelected;
    }

    private void Raycast_OnObjectUnSelected(object sender, Raycast.OnObjectUnSelectedArgs e)
    {
        UnSelected(e);
    }

    private void Raycast_OnObjectSelected(object sender, Raycast.OnObjectSelectedArgs e)
    {
        Selected(e);
    }

    public void Selected(Raycast.OnObjectSelectedArgs e)
    {
        if (e.nowHitObj)
        {
            // mat = new Material(Shader.Find();
            mat = e.nowHitObj.transform.GetComponent<Renderer>().material;

            //value for spheres
            //  mat.SetFloat("_outlineThickness", 0.55f);

            mat.SetFloat("_outlineThickness", 0.89f);
        }
    }

    public void UnSelected(Raycast.OnObjectUnSelectedArgs e)
    {
        if (e.lastHitObj)
        {
            mat = e.lastHitObj.transform.GetComponent<Renderer>().material;

            mat.SetFloat("_outlineThickness", 0f);
        }
    }
}
