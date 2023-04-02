using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    public static Raycast Instance { get; private set; }

    private GameObject mainCamera;
    [SerializeField] private float raycastRange = 5f;
    private GameObject player;

    private RaycastHit hit;
    private Transform lastHitObj;
    private Transform nowHitObj;
    private bool rayBool;

    private SOLink productLink;
    private PDPLink pdpLink;
    private ProductSO productSO;
    private ProductSO productSOPDP;

    //envia referencia do produto
    public event EventHandler<OnObjectChangeRayArgs> OnObjectChangeRay;
    public event EventHandler<OnObjectChangeRayPDPArgs> OnObjectChangeRayPDP;

    //envia informacoes de qual produto esta selecionado
    //modifica shader de selecao de produto
    public event EventHandler<OnObjectSelectedArgs> OnObjectSelected;
    public event EventHandler<OnObjectUnSelectedArgs> OnObjectUnSelected;


    //tavez seja codigo inutil duplicado, mas para deixar sem confusao cada evento com seu argumento
    public class OnObjectUnSelectedArgs : EventArgs
    {
        public Transform lastHitObj;
    }

    public class OnObjectSelectedArgs : EventArgs
    {
        public Transform nowHitObj;
    }

    public class OnObjectChangeRayPDPArgs : EventArgs
    {
        public ProductSO productSO;
    }

    public class OnObjectChangeRayArgs : EventArgs
    {
        public ProductSO productSO;
    }

    void Awake()
    {
        Instance = this;

        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

    }

    void Update()
    {
        RaycastHit();
    }

    //analisar para substituir raycast por raycastAll
    void RaycastHit()
    {

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, raycastRange))
        {
            rayBool = true;


            if (!hit.collider.gameObject.CompareTag("Player"))
            {
             
                if (lastHitObj != null)
                {
                    nowHitObj = hit.collider.transform;
                    productLink = hit.collider.GetComponent<SOLink>();
                    productSO = productLink.GetSO();

                    pdpLink = hit.collider.GetComponent<PDPLink>();
                    productSOPDP = pdpLink.GetSO();

                    ToolTip.Instance.ShowToolTipUI();

                    OnObjectChangeRay?.Invoke(this, new OnObjectChangeRayArgs { productSO = productSO });
                    OnObjectChangeRayPDP?.Invoke(this, new OnObjectChangeRayPDPArgs { productSO = productSOPDP });
                    OnObjectSelected?.Invoke(this, new OnObjectSelectedArgs { nowHitObj = nowHitObj });

                    if(nowHitObj != lastHitObj)
                    {
                        OnObjectUnSelected?.Invoke(this, new OnObjectUnSelectedArgs { lastHitObj = lastHitObj });
                    }

                }
               lastHitObj = hit.collider.transform;
            }
        }
        else if(rayBool)
        {
            OnObjectUnSelected?.Invoke(this, new OnObjectUnSelectedArgs { lastHitObj = lastHitObj });
            ToolTip.Instance.HideToolTipUI();
            rayBool = false;
        }
    }

}
