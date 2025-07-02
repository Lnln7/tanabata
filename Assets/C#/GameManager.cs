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
    [Header("���[���������̃v�b�V�����������Ȃ�����")]
    public float waitReading = 0;

    [SerializeField]
    [Header("�Q�[������")]
    private float gameEndTime;
    [SerializeField]
    [Header("���U���g�҂��̎���")]
    private float resultWaitTime;
    [Header("���U���g����")]
    [SerializeField]
    private float resultTime;
    private SceneFader sceneFader;


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

    [SerializeField]
    [Header("���[�������̃p�l��")]
    private GameObject panel;
    [SerializeField]
    [Header("���U���g�̃p�l��")]
    private GameObject resultPanel;
    [SerializeField]
    [Header("���U���g�̃X�R�A")]
    private Text resultScoreText;
    [SerializeField]
    [Header("�J�E���g�_�E���̃e�L�X�g")]
    private Text countDownText;
    [SerializeField]
    [Header("�X�R�A�̃e�L�X�g")]
    private Text scoreText;

    private int cometTiming;//���ڂ̘f�����a���ɒu�������̂�
    private int cometTimingCount = 0;//���ڂ̘f����

    [SerializeField]
    [Header("�m�[�}�����������f���̊Ԃɂ���̂�")]
    private int planetInterval = 5;//�m�[�}�����������f���̊Ԃɂ���̂�
    private int planetCometIntervalCount = 0;//�f���Ɯa�������ɉ��o������
    [System.NonSerialized]
    public int planetIntervalCount = 0;//�f�������ɉ��o������
    private List<int> planetOrder;
    private List<bool> getPlanetList;
    [SerializeField]
    [Header("�f���̃X�v���C�g")]
    private List<Sprite> sprites; // �؂�ւ������X�v���C�g��z��Őݒ�

    [SerializeField]
    [Header("���̐����Ԋu")]
    private float spawnInterval = 2.0f; // �����Ԋu�i�b�j

    [System.NonSerialized]
    private Vector3 spawnPoint; // �����ʒu
    [SerializeField]
    [Header("�������WY")]
    private float spawnPointY; // �����ʒuY
    
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
    [Header("BGM�t�F�[�h����")]
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
        return getPlanetList; // ���ӁF�Ăяo�������ύX�ł���
    }
    public List<int> PlanetOder()
    {
        return planetOrder; // ���ӁF�Ăяo�������ύX�ł���
    }

    public List<Sprite> PlanetSprite()
    {
        return sprites; // ���ӁF�Ăяo�������ύX�ł���
    }

    //�Q�[���I�����̃J�E���g�_�E��
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
        Debug.Log("�{�^�����������̂�҂��Ă��܂�...");

        // isButton �� true �ɂȂ�܂őҋ@
        while (!isButton)
        {
            yield return null; // ���t���[���ҋ@
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

        // �R���[�`�����J�n
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
                    // �a���v���n�u�𐶐�
                    Instantiate(cometPrefab, spawnPoint, new Quaternion(0, 0, 0, 0));
                }
                else
                {
                    //���ڂ̘f����������
                    planetIntervalCount++;
                    // �f���v���n�u�𐶐�
                    Instantiate(planetPrefab, spawnPoint, new Quaternion(0, 0, 0, 0));
                }
            }
            else
            {

                // ���v���n�u�𐶐�
                Instantiate(sterPrefab, spawnPoint, new Quaternion(0, 0, 0, 0));

                planetCometIntervalCount++;
            }

            // �w�肵���Ԋu�ҋ@
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
        Debug.Log("�{�^�����������̂�҂��Ă��܂�...");

        // isButton �� true �ɂȂ�܂őҋ@
        while (!isButton)
        {
            yield return null; // ���t���[���ҋ@
        }

        PlaySE(buttonSE);
        SceneFader.Instance.FadeToScene("TitleScene");
    }


    public void PlaySE(AudioClip clip)//�Ăяo���ꂽ��SE
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
        audioSource.loop = true; // ���[�v��L���ɂ���
        audioSource.clip = ruleBGM;
        audioSource.Play();
    }

    public void PlayGameBGM()
    {
        audioSource.Stop();
        audioSource.loop = false; // ���[�v�𖳌���
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
        audioSource.volume = startVolume; // ���̉��ʂɖ߂��Ă���
    }


    public void PlayResultBGM()
    {
        audioSource.Stop();
        audioSource.loop = true; // ���[�v��L���ɂ���
        audioSource.clip = resultBGM;
        audioSource.Play();
    }
}
