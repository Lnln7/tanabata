using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [System.NonSerialized]
    public bool gameOn = false;

    public int score;

    [Header("���[���������̃v�b�V�����������Ȃ�����")]
    public float waitReading = 0;

    [Header("�Q�[������")]
    public float gameEndTime;

    [Header("���̓_��")]
    public int sterScore;
    [Header("�f���̓_��")]
    public int planetScore;
    [Header("�a���̓_��")]
    public int cometScore;
    [Header("�v���n�u")]
    public GameObject sterPrefab; // �v���n�u���A�T�C������
    public GameObject planetPrefab;
    public GameObject cometPrefab;

    [Header("���[�������̃p�l��")]
    public GameObject panel;
    [Header("�J�E���g�_�E���̃e�L�X�g")]
    public Text countDownText;
    [Header("�X�R�A�̃e�L�X�g")]
    public Text scoreText;

    private int cometTiming;//���ڂ̘f�����a���ɒu�������̂�
    private int cometTimingCount = 0;//���ڂ̘f����

    [Header("�m�[�}�����������f���̊Ԃɂ���̂�")]
    public int planetInterval = 5;//�m�[�}�����������f���̊Ԃɂ���̂�
    private int planetIntervalCount = 0;

    [Header("���̐����Ԋu")]
    public float spawnInterval = 2.0f; // �����Ԋu�i�b�j

    [System.NonSerialized]
    public Vector3 spawnPoint; // �����ʒu
    [Header("�������WY")]
    public float spawnPointY; // �����ʒuY

    // Start is called before the first frame update
    void Start()
    {
        gameOn = false;
        panel.SetActive(true);
        cometTiming = Random.Range(1, 8);
        cometTimingCount = 0;
        planetIntervalCount = 0;
        countDownText.text = "";


        StartCoroutine(WaitReading());
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOn)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            scoreText.text = "";
        }
    }

    //�Q�[���I�����̃J�E���g�_�E��
    IEnumerator wateGameEnd()
    {
        int count=0;

        yield return new WaitForSeconds(gameEndTime-11);

        count = 10;

        while(count != -1)
        {
            countDownText.text = count.ToString();
            yield return new WaitForSeconds(1.0f);
            count--;
        }
        countDownText.text = "";

        gameOn = false;
    }

    IEnumerator WaitReading()
    {
        yield return new WaitForSeconds(waitReading);
        StartCoroutine(WaitForSpace());
    }

    IEnumerator WaitForSpace()
    {
        Debug.Log("�X�y�[�X�L�[���������̂�҂��Ă��܂�...");


        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));


        gameOn = true;
        panel.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        countDownText.text = "3";
        yield return new WaitForSeconds(1.0f);
        countDownText.text = "2";
        yield return new WaitForSeconds(1.0f);
        countDownText.text = "1";
        yield return new WaitForSeconds(1.0f);
        countDownText.text = "";



        // �R���[�`�����J�n
        StartCoroutine(SpawnPrefab());
        StartCoroutine(wateGameEnd());
    }

    private IEnumerator SpawnPrefab()
    {
        while (gameOn)
        {
            spawnPoint = new Vector3(Random.Range(-3.0f, 3.0f), spawnPointY, 0);

            if (planetIntervalCount == planetInterval && cometTimingCount != 8)
            {
                planetIntervalCount = 0;
                cometTimingCount++;

                if(cometTiming == cometTimingCount)
                {
                    // �a���v���n�u�𐶐�
                    Instantiate(cometPrefab, spawnPoint, new Quaternion(0, 0, 0, 0));
                }
                else
                {
                    // �f���v���n�u�𐶐�
                    Instantiate(planetPrefab, spawnPoint, new Quaternion(0, 0, 0, 0));
                }
            }
            else
            {

                // ���v���n�u�𐶐�
                Instantiate(sterPrefab, spawnPoint, new Quaternion(0, 0, 0, 0));

                planetIntervalCount++;
            }

            // �w�肵���Ԋu�ҋ@
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
