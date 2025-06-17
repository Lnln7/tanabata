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
            //この惑星の番号を取得。なんの惑星か。
            planetNumber = list[gameManager.planetIntervalCount-1];
        }
        
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
