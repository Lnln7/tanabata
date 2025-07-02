using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class Player : MonoBehaviour
{
    private Animator animator;
    public float moveSpeed = 3f;
    private SpriteRenderer spriteRenderer;
    private GameObject gameManagerObj;
    private GameManager gameManager;



    //レバーで
    SerialPort serial = new SerialPort("COM7", 9600);
    public Rigidbody2D rb;
    



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManagerObj = GameObject.Find("GameManager");
        gameManager = gameManagerObj.GetComponent<GameManager>();


        if (!serial.IsOpen)
        {
            serial.Open();
            serial.ReadTimeout = 100;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (serial.IsOpen)
        {
            try
            {
                string data = serial.ReadLine(); // 例: "X: 300  Y: 500  SW: 1"
                //int x = int.Parse(data);
                int x = ParseXValue(data);

                Debug.Log("x_value = " + x);
                if (data.Contains("PUSHED!"))
                {
                    gameManager.isButton = true;
                    Debug.Log("ボタンが押されました！");
                    // ここに任意の処理を追加（例：オブジェクトを動かすなど）
                }
                else
                {
                    gameManager.isButton = false;
                }


                if (x < 100 && transform.position.x > -2.5)
                {
                    animator.SetBool("isMove", true);
                    spriteRenderer.flipX = true;  // 左向き
                    rb.velocity = new Vector2(-moveSpeed, rb.velocity.y); // 左
                }
                else if (x < 450 && transform.position.x > -2.5)
                {
                    animator.SetBool("isMove", true);
                    spriteRenderer.flipX = true;  // 左向き
                    rb.velocity = new Vector2(-moveSpeed/2, rb.velocity.y); // 左
                }
                else if (x > 900 && transform.position.x < 2.5)
                {
                    animator.SetBool("isMove", true);
                    spriteRenderer.flipX = false; // 右向き
                    rb.velocity = new Vector2(moveSpeed, rb.velocity.y); // 右
                }
                else if (x > 680 && transform.position.x < 2.5)
                {
                    animator.SetBool("isMove", true);
                    spriteRenderer.flipX = false; // 右向き
                    rb.velocity = new Vector2(moveSpeed/2, rb.velocity.y); // 右
                }
                else
                {
                    animator.SetBool("isMove", false);
                    spriteRenderer.flipX = false; // 右向き
                    rb.velocity = new Vector2(0, rb.velocity.y); // 停止
                }
            }
            catch (System.Exception) { }
        }


        /*
        float move = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * move * moveSpeed * Time.deltaTime);
        if(Mathf.Abs(move)>0.1)
        {
            animator.SetBool("isMove", true);
            if(move>0)
                spriteRenderer.flipX = false; // 右向き
            else
                spriteRenderer.flipX = true;  // 左向き
        }
        else
        {
            animator.SetBool("isMove", false);
        }
        */


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
