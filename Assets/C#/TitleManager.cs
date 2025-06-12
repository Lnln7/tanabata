using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForSpace());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator WaitForSpace()
    {
        Debug.Log("スペースキーが押されるのを待っています...");


        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("MainGameScene");

    }
}
