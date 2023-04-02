using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public Transform playerBody;

    private bool isPDPOpen;
    private bool isCheckoutOpen;
    private bool isCartOpen;
    public TextMeshProUGUI fpsString;
    float deltaTime;


    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 90;
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsString.text = Mathf.Ceil(fps).ToString();


        //maybe bad pratice, refactor in future
        isPDPOpen = PDPController.Instance.GetPDPStatus();
        isCheckoutOpen = CheckoutController.Instance.GetCheckoutStatus();
        isCartOpen = CartController.Instance.GetCartStatus();

        if (!isCartOpen && !isPDPOpen && !isCheckoutOpen)
        {

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);

        }
    }

}
