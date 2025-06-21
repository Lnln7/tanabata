using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ster : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    private Rigidbody2D rb;
    private GameObject gameManagerObj;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerObj = GameObject.Find("GameManager");
        gameManager = gameManagerObj.GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
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
            //Debug.Log("����������I�I");
            gameManager.PlaySE(gameManager.starSE);
            gameManager.score += gameManager.sterScore;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"�^�O: {other.tag} �̃I�u�W�F�N�g���g���K�[�ɓ���܂����B");
        }
    }
}
