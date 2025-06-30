using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance { get; private set; }


    public Image fadeImage;
    public float fadeSpeed = 1.0f;

    void Awake()
    {
        // �V���O���g����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // �d����h��
        }
    }


    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutIn(sceneName));
    }

    IEnumerator FadeIn()
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    IEnumerator FadeOutIn(string sceneName)
    {
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(1.0f); // �ǂݍ��ݑҋ@�i�K�v�ɉ����āj
        StartCoroutine(FadeIn());
    }

}
