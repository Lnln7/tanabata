using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;


public class TitleManager : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("COM6", 9600);

    private bool isButton = false;

    // Start is called before the first frame update
    void Start()
    {
        serialPort.Open();
        serialPort.ReadTimeout = 100;

        StartCoroutine(WaitForSpace());
    }

    // Update is called once per frame
    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine();
                if (data.Contains("PUSHED!"))
                {
                    isButton = true;
                    Debug.Log("ボタンが押されました！");
                    // ここに任意の処理を追加（例：オブジェクトを動かすなど）
                }
            }
            catch (System.Exception) { }
        }

    }
    IEnumerator WaitForSpace()
    {
        Debug.Log("スペースキーが押されるのを待っています...");

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
        if (serialPort.IsOpen) serialPort.Close();
    }

}
