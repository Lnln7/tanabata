using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.IO.Ports;


public class ArduinoReceiver : MonoBehaviour
{
    public static ArduinoReceiver Instance;

    SerialPort serialPort;
    public string receivedData;
    private AudioSource audioSource = null;
    public AudioClip buttonSE;

    void Awake()
    {
        // シングルトン化して重複を防ぐ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン切り替えでも破棄されない
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        audioSource = GameObject.Find("TitleManager").GetComponent<AudioSource>();
        StartCoroutine(WaitForNumberKey());
    }

    IEnumerator WaitForNumberKey()
    {
        Debug.Log("数字キー入力待ち...");

        int pressedNumber = -1;

        while (true)
        {
            for (KeyCode key = KeyCode.Alpha0; key <= KeyCode.Alpha9; key++)
            {
                if (Input.GetKeyDown(key))
                {
                    pressedNumber = key - KeyCode.Alpha0; // 0?9に変換
                    Debug.Log("押された数字: " + pressedNumber);
                    if (pressedNumber > 0 && pressedNumber < 10)
                    {
                        audioSource.PlayOneShot(buttonSE);
                        // COMポート設定（環境に合わせて変更）
                        serialPort = new SerialPort("COM" + pressedNumber, 9600);
                        serialPort.Open();
                    }
                    else
                    {
                        serialPort = new SerialPort("COM3", 9600);
                        serialPort.Open();
                    }
                    yield break;
                }
            }

            yield return null; // 次のフレームまで待機
        }
    }


    void Update()
    {
        if (serialPort != null && serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            receivedData = serialPort.ReadLine();
            Debug.Log(receivedData);
        }
    }

}
