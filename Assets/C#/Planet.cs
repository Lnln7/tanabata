using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float delay = 4.0f; // 削除までの秒数
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
            //この惑星の番号を取得。なんの惑星か。
            planetNumber = oderList[gameManager.planetIntervalCount-1];
        }

        //見た目をリストから選んで変える
        if(sprites[planetNumber] != null)
          spriteRenderer.sprite = sprites[planetNumber];

        Destroy(gameObject, delay);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(0, -speed);
    }

    // トリガーに入ったときに呼び出される
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 衝突したオブジェクトのタグを取得
        if (other.CompareTag("Player"))
        {
            //Debug.Log("惑星を取った！！");
            var list = gameManager.GetPlanetList();

            Instantiate(getEffectPrefab, transform.position, transform.rotation, null); // ワールド空間に生成
            gameManager.PlaySE(gameManager.planetSE);

            gameManager.score += gameManager.planetScore;
            list[planetNumber] = true;
            Debug.Log(planetNumber+"を取得");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"タグ: {other.tag} のオブジェクトがトリガーに入りました。");
        }
    }
}
