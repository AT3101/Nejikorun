using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
    //フィールド生成
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1.0f;

    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;
    int tragetLane;

    public float gravity;
    public float speedZ;
    public float speedX;
    public float speedJump;
    public float accelerationZ;

    void Start()
    {
        //必要なコンポーネントを自動取得
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //デバック用
        if(Input.GetKeyDown("left"))MoveToLeft();  
        if(Input.GetKeyDown("right"))MoveToRight(); 
        if(Input.GetKeyDown("space"))Jump();       

        //徐々に加速しZ方向に常に加速させる
        float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
        moveDirection.z = Mathf.Clamp(acceleratedZ,0,speedZ);

        //X方向は目標のポジションまでの差分の割合で速度を計算
        float ratioX = (tragetLane * LaneWidth - transform.position.x) / LaneWidth;
        moveDirection.x = ratioX *speedX;

        
        if(controller.isGrounded)
        {                   
            if(Input.GetAxis("Vertical")>0.0f)
            {                                   //↑(W)ボタンを押した時
                moveDirection.z = Input.GetAxis("Vertical")*speedZ;
            }
            else
            {
                moveDirection.z = 0;
            }
                                            //左右(AD)ボタンを押した時
            transform.Rotate(0,Input.GetAxis("Horizontal")*3,0);//3秒で1回転

            if(Input.GetButton("Jump"))
            {
                moveDirection.y = speedJump;//yに向かってジャンプ
                animator.SetTrigger("jump");//アニメーターでセットしたジャンプをしてくれる
            }
        }
        
        //重力分の力を毎フレーム追加
        moveDirection.y -= gravity*Time.deltaTime;

        //移動実行
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection*Time.deltaTime);//controller.Move関数で動いている Vector3に数値を入れて別の方向に移動
        //移動後設置してたらY方向の速度はリセットする
        if(controller.isGrounded)moveDirection.y = 0;
        //速度が０以上なら走っているフラグをtrueにする
        animator.SetBool("run",moveDirection.z>0.0f);
    }
    //左レーンに移動
    public void MoveToLeft()
    {
        if(controller.isGrounded && tragetLane > MinLane)tragetLane--;
    }

    //右のレーンに移動を開始
    public void MoveToRight()
    {
        if(controller.isGrounded && tragetLane < MaxLane)tragetLane++;
    }
    public void Jump()
    {
        if(controller.isGrounded)
        {
            moveDirection.y = speedJump;
            //ジャンプトリガーを設定
            animator.SetTrigger("jump");
        }
    }
}
