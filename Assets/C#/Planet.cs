using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    private Rigidbody2D rb;
    private GameObject gameManagerObj;
    private GameManager gameManager;
    private int planetNumber;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerObj = GameObject.Find("GameManager");
        gameManager = gameManagerObj.GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();

        var list = gameManager.PlanetOder();


        if (gameManager.planetIntervalCount != 0)
        {
            //���̘f���̔ԍ����擾�B�Ȃ�̘f�����B
            planetNumber = list[gameManager.planetIntervalCount-1];
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(0, -speed);
    }

    // �g���K�[�ɓ������Ƃ��ɌĂяo�����
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �Փ˂����I�u�W�F�N�g�̃^�O���擾
        if (other.CompareTag("Player"))
        {
            //Debug.Log("�f����������I�I");
            var list = gameManager.GetPlanetList();

            gameManager.score += gameManager.planetScore;
            list[planetNumber] = true;
            Debug.Log(planetNumber+"���擾");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"�^�O: {other.tag} �̃I�u�W�F�N�g���g���K�[�ɓ���܂����B");
        }
    }
}
