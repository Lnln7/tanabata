using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [System.NonSerialized]
    public bool gameOn = false;

    public int score;

    [SerializeField]
    [Header("ルール説明時のプッシュが反応しない期間")]
    public float waitReading = 0;

    [SerializeField]
    [Header("ゲーム時間")]
    private float gameEndTime;
    [SerializeField]
    [Header("リザルト待ちの時間")]
    private float resultWaitTime;
    [Header("リザルト時間")]
    [SerializeField]
    private float resultTime;
    private SceneFader sceneFader;


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

    [SerializeField]
    [Header("ルール説明のパネル")]
    private GameObject panel;
    [SerializeField]
    [Header("リザルトのパネル")]
    private GameObject resultPanel;
    [SerializeField]
    [Header("リザルトのスコア")]
    private Text resultScoreText;
    [SerializeField]
    [Header("カウントダウンのテキスト")]
    private Text countDownText;
    [SerializeField]
    [Header("スコアのテキスト")]
    private Text scoreText;

    private int cometTiming;//何個目の惑星が彗星に置き換わるのか
    private int cometTimingCount = 0;//何個目の惑星か

    [SerializeField]
    [Header("ノーマル星がいくつ惑星の間にあるのか")]
    private int planetInterval = 5;//ノーマル星がいくつ惑星の間にあるのか
    private int planetCometIntervalCount = 0;//惑星と彗星を既に何個出したか
    [System.NonSerialized]
    public int planetIntervalCount = 0;//惑星を既に何個出したか
    private List<int> planetOrder;
    private List<bool> getPlanetList;
    [SerializeField]
    [Header("惑星のスプライト")]
    private List<Sprite> sprites; // 切り替えたいスプライトを配列で設定

    [SerializeField]
    [Header("星の生成間隔")]
    private float spawnInterval = 2.0f; // 生成間隔（秒）

    [System.NonSerialized]
    private Vector3 spawnPoint; // 生成位置
    [SerializeField]
    [Header("生成座標Y")]
    private float spawnPointY; // 生成位置Y
    
    private AudioSource audioSource = null;

    [Header("SE")]
    public AudioClip buttonSE;
    public AudioClip starSE;
    public AudioClip planetSE;
    public AudioClip cometSE;
    public AudioClip planetComeSE;
    public AudioClip cometComeSE;
    [Header("BGM")]
    public AudioClip ruleBGM;
    public AudioClip gameBGM;
    public AudioClip resultBGM;
    [Header("BGMフェード時間")]
    [SerializeField]
    private float fadeDuration = 2.0f;


    [System.NonSerialized]
    public bool isButton = false;


    void Awake()
    {
        planetOrder = new() { 0, 1, 2, 3, 4, 5, 6 ,7};
        getPlanetList = new List<bool> { false, false, false, false, false, false, false ,false};
    }


    // Start is called before the first frame update
    void Start()
    {
        gameOn = false;
        panel.SetActive(true);
        resultPanel.SetActive(false);
        cometTiming = Random.Range(2, 9);
        cometTimingCount = 0;
        planetCometIntervalCount = 0;
        countDownText.text = "";
        audioSource = GetComponent<AudioSource>();

        if (planetOrder!=null && planetOrder.Count != 0)
        {
            Shuffle(planetOrder);
        }

        PlayRuleBGM();
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

    
    private void Shuffle(List<int> list)
    {
        if (list != null && list.Count != 0)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                int temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }
    }

    public List<bool> GetPlanetList()
    {
        return getPlanetList; // 注意：呼び出し元も変更できる
    }
    public List<int> PlanetOder()
    {
        return planetOrder; // 注意：呼び出し元も変更できる
    }

    public List<Sprite> PlanetSprite()
    {
        return sprites; // 注意：呼び出し元も変更できる
    }

    //ゲーム終了時のカウントダウン
    IEnumerator wateGameEnd()
    {
        int count=0;

        yield return new WaitForSeconds(gameEndTime-11);

        count = 10;

        while(count != 1)
        {
            countDownText.text = count.ToString();
            yield return new WaitForSeconds(1.0f);
            count--;
        }

        gameOn = false;

        while (count != -1)
        {
            countDownText.text = count.ToString();
            yield return new WaitForSeconds(1.0f);
            count--;
        }
        countDownText.text = "";

        yield return new WaitForSeconds(resultWaitTime);
        PlayResultBGM();
        StartCoroutine(WaitResult());
        StartCoroutine(WaitResultButton());
    }

    IEnumerator WaitReading()
    {
        yield return new WaitForSeconds(waitReading);
        StartCoroutine(WaitForSpace());
    }

    IEnumerator WaitForSpace()
    {
        Debug.Log("ボタンが押されるのを待っています...");

        // isButton が true になるまで待機
        while (!isButton)
        {
            yield return null; // 毎フレーム待機
        }

        StartCoroutine(FadeOutCoroutine());

        PlaySE(buttonSE);

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

        PlayGameBGM();

        // コルーチンを開始
        StartCoroutine(SpawnPrefab());
        StartCoroutine(wateGameEnd());
    }

    private IEnumerator SpawnPrefab()
    {
        while (gameOn)
        {
            spawnPoint = new Vector3(Random.Range(-2.7f, 2.7f), spawnPointY, 0);

            if (planetCometIntervalCount == planetInterval && cometTimingCount != 9)
            {
                planetCometIntervalCount = 0;
                cometTimingCount++;

                if(cometTiming == cometTimingCount)
                {
                    // 彗星プレハブを生成
                    Instantiate(cometPrefab, spawnPoint, new Quaternion(0, 0, 0, 0));
                }
                else
                {
                    //何個目の惑星か数える
                    planetIntervalCount++;
                    // 惑星プレハブを生成
                    Instantiate(planetPrefab, spawnPoint, new Quaternion(0, 0, 0, 0));
                }
            }
            else
            {

                // 星プレハブを生成
                Instantiate(sterPrefab, spawnPoint, new Quaternion(0, 0, 0, 0));

                planetCometIntervalCount++;
            }

            // 指定した間隔待機
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator WaitResult()
    {
        resultPanel.SetActive(true);
        for (int i = 0; i < 8; i++)
        {
            if (getPlanetList[i])
            {
                resultPanel.transform.GetChild(i+1).gameObject.SetActive(true);
            }
            else
            {
                resultPanel.transform.GetChild(i + 1).gameObject.SetActive(false);
            }
        }
        resultScoreText.text = score.ToString();
        yield return new WaitForSeconds(resultTime);
        SceneFader.Instance.FadeToScene("TitleScene");
    }

    IEnumerator WaitResultButton()
    {
        Debug.Log("ボタンが押されるのを待っています...");

        // isButton が true になるまで待機
        while (!isButton)
        {
            yield return null; // 毎フレーム待機
        }

        PlaySE(buttonSE);
        SceneFader.Instance.FadeToScene("TitleScene");
    }


    public void PlaySE(AudioClip clip)//呼び出されたらSE
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("audiosource=null");
        }
    }

    public void PlayRuleBGM()
    {
        audioSource.Stop();
        audioSource.loop = true; // ループを有効にする
        audioSource.clip = ruleBGM;
        audioSource.Play();
    }

    public void PlayGameBGM()
    {
        audioSource.Stop();
        audioSource.loop = false; // ループを無効に
        audioSource.clip = gameBGM;
        audioSource.Play();
    }

    private IEnumerator FadeOutCoroutine()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // 元の音量に戻しておく
    }


    public void PlayResultBGM()
    {
        audioSource.Stop();
        audioSource.loop = true; // ループを有効にする
        audioSource.clip = resultBGM;
        audioSource.Play();
    }
}
