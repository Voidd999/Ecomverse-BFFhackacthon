using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class CanvasController : MonoBehaviour
{
    public GameObject canvasPanel;
    public TextMeshProUGUI RoomText;
    public string sceneToLoad;
    public string RoomString;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvasPanel.SetActive(true);
            RoomText.text = RoomString;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvasPanel.SetActive(false);
            RoomText.text = "";
        }
    }

    public void LoadSceneOnClick()
    {
        SceneManager.LoadScene("Scene_" + RoomText.text.ToLower());
    }
        public void test()
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();

        // Set the StartInfo of process
        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        process.StartInfo.FileName = "E:\\Unity\\Projects\\My project\\Game\\My project.exe";
        // process.StartInfo.Arguments = "ARGUMENT";

        // Start the process
        process.Start();
        // process.WaitForExit();
    }
}
