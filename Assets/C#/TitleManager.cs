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
                    Debug.Log("�{�^����������܂����I");
                    // �����ɔC�ӂ̏�����ǉ��i��F�I�u�W�F�N�g�𓮂����Ȃǁj
                }
            }
            catch (System.Exception) { }
        }

    }
    IEnumerator WaitForSpace()
    {
        Debug.Log("�X�y�[�X�L�[���������̂�҂��Ă��܂�...");

        // isButton �� true �ɂȂ�܂őҋ@
        while (!isButton)
        {
            yield return null; // ���t���[���ҋ@
        }

        //yield return new WaitForSeconds(0.5f);

        SceneFader.Instance.FadeToScene("MainGameScene");

    }

    void OnApplicationQuit()
    {
        if (serialPort.IsOpen) serialPort.Close();
    }

}
