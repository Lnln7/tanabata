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
        // �V���O���g�������ďd����h��
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[���؂�ւ��ł��j������Ȃ�
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
        Debug.Log("�����L�[���͑҂�...");

        int pressedNumber = -1;

        while (true)
        {
            for (KeyCode key = KeyCode.Alpha0; key <= KeyCode.Alpha9; key++)
            {
                if (Input.GetKeyDown(key))
                {
                    pressedNumber = key - KeyCode.Alpha0; // 0?9�ɕϊ�
                    Debug.Log("�����ꂽ����: " + pressedNumber);
                    if (pressedNumber > 0 && pressedNumber < 10)
                    {
                        audioSource.PlayOneShot(buttonSE);
                        // COM�|�[�g�ݒ�i���ɍ��킹�ĕύX�j
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

            yield return null; // ���̃t���[���܂őҋ@
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
