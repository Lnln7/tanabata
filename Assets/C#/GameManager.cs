using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [System.NonSerialized]
    public bool gameOn = false;

    public int score;

    [Header("ルール説明時のプッシュが反応しない期間")]
    public float waitReading = 0;

    [Header("ゲーム時間")]
    public float gameEndTime;

    [Header("星の点数")]
    public int sterScore;
    [Header("惑星の点数")]
    public int planetScore;
    [Header("彗星の点数")]
    public int cometScore;
    [Header("プレハブ")]
    public GameObject sterPrefab; // プレハブをアサインする
    public GameObject planetPrefab;
    public GameObject cometPrefab;

    [Header("ルール説明のパネル")]
    public GameObject panel;
    [Header("カウントダウンのテキスト")]
    public Text countDownText;
    [Header("スコアのテキスト")]
    public Text scoreText;

    private int cometTiming;//何個目の惑星が彗星に置き換わるのか
    private int cometTimingCount = 0;//何個目の惑星か

    [Header("ノーマル星がいくつ惑星の間にあるのか")]
    public int planetInterval = 5;//ノーマル星がいくつ惑星の間にあるのか
    private int planetIntervalCount = 0;

    [Header("星の生成間隔")]
    public float spawnInterval = 2.0f; // 生成間隔（秒）

    [System.NonSerialized]
    public Vector3 spawnPoint; // 生成位置
    [Header("生成座標Y")]
    public float spawnPointY; // 生成位置Y

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

    //ゲーム終了時のカウントダウン
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
        Debug.Log("スペースキーが押されるのを待っています...");


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



        // コルーチンを開始
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
                    // 彗星プレハブを生成
                    Instantiate(cometPrefab, spawnPoint, new Quaternion(0, 0, 0, 0));
                }
                else
                {
                    // 惑星プレハブを生成
                    Instantiate(planetPrefab, spawnPoint, new Quaternion(0, 0, 0, 0));
                }
            }
            else
            {

                // 星プレハブを生成
                Instantiate(sterPrefab, spawnPoint, new Quaternion(0, 0, 0, 0));

                planetIntervalCount++;
            }

            // 指定した間隔待機
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
