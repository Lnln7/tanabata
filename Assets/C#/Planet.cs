using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float delay = 4.0f; // �폜�܂ł̕b��
    [SerializeField]
    private GameObject getEffectPrefab;
    private Rigidbody2D rb;
    private GameObject gameManagerObj;
    private GameManager gameManager;
    private int planetNumber;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerObj = GameObject.Find("GameManager");
        gameManager = gameManagerObj.GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        //gameManager.PlaySE(gameManager.planetComeSE);

        spriteRenderer = GetComponent<SpriteRenderer>();
        

        var oderList = gameManager.PlanetOder();
        var sprites = gameManager.PlanetSprite();


        if (gameManager.planetIntervalCount != 0)
        {
            //���̘f���̔ԍ����擾�B�Ȃ�̘f�����B
            planetNumber = oderList[gameManager.planetIntervalCount-1];
        }

        //�����ڂ����X�g����I��ŕς���
        if(sprites[planetNumber] != null)
          spriteRenderer.sprite = sprites[planetNumber];

        Destroy(gameObject, delay);
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

            Instantiate(getEffectPrefab, transform.position, transform.rotation, null); // ���[���h��Ԃɐ���
            gameManager.PlaySE(gameManager.planetSE);

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
