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
                    Debug.Log("�{�^����������܂����I");
                    // �����ɔC�ӂ̏�����ǉ��i��F�I�u�W�F�N�g�𓮂����Ȃǁj
                }
            }
            catch (System.Exception) { }
        }

    }
    IEnumerator WaitForSpace()
    {
        Debug.Log("�{�^�����������̂�҂��Ă��܂�...");

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
        if (serial.IsOpen) serial.Close();
    }

}
