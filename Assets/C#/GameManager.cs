using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int starCount;
    public GameObject sterPrefab; // �v���n�u���A�T�C������
    public float spawnInterval = 2.0f; // �����Ԋu�i�b�j
    public Vector3 spawnPoint; // �����ʒu
    public float spawnPointY; // �����ʒuY

    // Start is called before the first frame update
    void Start()
    {
        // �R���[�`�����J�n
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

            // �v���n�u�𐶐�
            Instantiate(sterPrefab, spawnPoint,new Quaternion(0,0,0,0));

            // �w�肵���Ԋu�ҋ@
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
