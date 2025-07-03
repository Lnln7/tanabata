using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;


public class TitleManager : MonoBehaviour
{
    SerialPort serial = new SerialPort("COM3", 9600);

    private AudioSource audioSource = null;
    private bool isButton = false;

    public AudioClip startButtonSE;

    // Start is called before the first frame update
    void Start()
    {
        if (!serial.IsOpen)
        {
            serial.Open();
            serial.ReadTimeout = 100;
        }
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(WaitForSpace());
    }

    // Update is called once per frame
    void Update()
    {
        if (serial.IsOpen)
        {
            try
            {
                string data = serial.ReadLine();
                if (data.Contains("PUSHED!"))
                {
                    isButton = true;
                    audioSource.PlayOneShot(startButtonSE);
                    Debug.Log("ボタンが押されました！");
                    // ここに任意の処理を追加（例：オブジェクトを動かすなど）
                }
            }
            catch (System.Exception) { }
        }

    }
    IEnumerator WaitForSpace()
    {
        Debug.Log("ボタンが押されるのを待っています...");

        // isButton が true になるまで待機
        while (!isButton)
        {
            yield return null; // 毎フレーム待機
        }

        //yield return new WaitForSeconds(0.5f);

        SceneFader.Instance.FadeToScene("MainGameScene");

    }

    void OnApplicationQuit()
    {
        if (serial.IsOpen) serial.Close();
    }

}
