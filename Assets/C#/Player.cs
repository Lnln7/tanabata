using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class Player : MonoBehaviour
{
    
    [SerializeField]//マウスで
    private float positionY = -5.7f;
    [SerializeField]
    private float division = 1;
    

    /*
    //レバーで
    SerialPort serial = new SerialPort("COM5", 9600);
    public Rigidbody2D rb;
    public float moveSpeed = 3f;
    */


    // Start is called before the first frame update
    void Start()
    {
        /*
        if (!serial.IsOpen)
        {
            serial.Open();
            serial.ReadTimeout = 100;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (serial.IsOpen)
        {
            try
            {
                string data = serial.ReadLine(); // 例: "X: 300  Y: 500  SW: 1"
                //int x = int.Parse(data);
                int x = ParseXValue(data);

                Debug.Log("x_value = " + x);


                if (x < 400)
                {
                    rb.velocity = new Vector2(-moveSpeed, rb.velocity.y); // 左
                }
                else if (x > 600)
                {
                    rb.velocity = new Vector2(moveSpeed, rb.velocity.y); // 右
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y); // 停止
                }
            }
            catch (System.Exception) { }
        }
        */


      
      // マウスのスクリーン座標を取得
      Vector3 mousePosition = Input.mousePosition;

      // スクリーン座標をワールド座標に変換
      mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

      // オブジェクトをマウス位置に移動
      transform.position = new Vector3(mousePosition.x/division,positionY,0);
      
    }

    int ParseXValue(string input)
    {
        string[] parts = input.Split(' ');
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] == "X:")
            {
                int.TryParse(parts[i + 1], out int xVal);
                return xVal;
            }
        }
        return 512;
    }

}
