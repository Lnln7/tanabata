using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float delay = 4.0f; // 削除までの秒数
    [SerializeField]
    private float getDelay = 3.0f; // 取得から削除までの秒数
    [SerializeField]
    private GameObject getEffectPrefab;
    private Rigidbody2D rb;
    private GameObject gameManagerObj;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerObj = GameObject.Find("GameManager");
        gameManager = gameManagerObj.GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        gameManager.PlaySE(gameManager.cometComeSE);

        Destroy(gameObject, delay);
    }

    // Update is called once per frame
    void Update()
    {
        if(rb != null)
           rb.velocity = new Vector2(0, -speed);
    }

    // トリガーに入ったときに呼び出される
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 衝突したオブジェクトのタグを取得
        if (other.CompareTag("Player"))
        {
            Debug.Log("彗星を取った！！");
            gameManager.PlaySE(gameManager.cometSE);

            gameManager.score += gameManager.cometScore;
            Destroy(rb, 0.1f);
            Destroy(GetComponent<SpriteRenderer>(), 0.1f);
            Destroy(GetComponent<CircleCollider2D>(),0.1f);
            StartCoroutine(TrailOff());

            Destroy(gameObject, getDelay);
        }
        else
        {
            Debug.Log($"タグ: {other.tag} のオブジェクトがトリガーに入りました。");
        }
    }

    IEnumerator TrailOff()
    {
        yield return new WaitForSeconds(0.05f);
        Instantiate(getEffectPrefab, transform.position, transform.rotation, null); // ワールド空間に生成
        GetComponent<TrailRenderer>().emitting = false;
    }
}
