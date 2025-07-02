using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titlePlayer : MonoBehaviour
{
    public Vector2 areaMin = new Vector2(-5f, -3f);
    public Vector2 areaMax = new Vector2(5f, 3f);
    public float speed = 1f;
    private Vector2 direction;

    void Start()
    {
        // 初期のドリフト方向をランダムに設定
        float angle = Random.Range(0f, 360f);
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    void Update()
    {
        // 移動
        Vector2 pos = transform.position;
        pos += direction * speed * Time.deltaTime;

        // 範囲チェックして反射
        if (pos.x < areaMin.x || pos.x > areaMax.x)
        {
            direction.x *= -1;
            pos.x = Mathf.Clamp(pos.x, areaMin.x, areaMax.x);
        }
        if (pos.y < areaMin.y || pos.y > areaMax.y)
        {
            direction.y *= -1;
            pos.y = Mathf.Clamp(pos.y, areaMin.y, areaMax.y);
        }

        transform.position = pos;

        // 漂うような回転（見た目の演出）
        transform.Rotate(0, 0, 15f * Time.deltaTime);
    }
}
