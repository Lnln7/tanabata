using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float positionY = 0;
    [SerializeField]
    private float division = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // マウスのスクリーン座標を取得
        Vector3 mousePosition = Input.mousePosition;

        // スクリーン座標をワールド座標に変換
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // オブジェクトをマウス位置に移動
        transform.position = new Vector3(mousePosition.x/division,positionY,0);
    }
}
