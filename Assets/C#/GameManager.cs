using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int starCount;
    public GameObject sterPrefab; // プレハブをアサインする
    public float spawnInterval = 2.0f; // 生成間隔（秒）
    public Vector3 spawnPoint; // 生成位置
    public float spawnPointY; // 生成位置Y

    // Start is called before the first frame update
    void Start()
    {
        // コルーチンを開始
        StartCoroutine(SpawnPrefab());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator SpawnPrefab()
    {
        while (true)
        {
            spawnPoint = new Vector3(Random.Range(-3.0f, 3.0f), spawnPointY, 0);

            // プレハブを生成
            Instantiate(sterPrefab, spawnPoint,new Quaternion(0,0,0,0));

            // 指定した間隔待機
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
