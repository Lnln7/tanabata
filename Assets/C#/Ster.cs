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

    // トリガーに入ったときに呼び出される
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 衝突したオブジェクトのタグを取得
        if (other.CompareTag("Player"))
        {
            //Debug.Log("星を取った！！");
            gameManager.PlaySE(gameManager.starSE);
            gameManager.score += gameManager.sterScore;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"タグ: {other.tag} のオブジェクトがトリガーに入りました。");
        }
    }
}
