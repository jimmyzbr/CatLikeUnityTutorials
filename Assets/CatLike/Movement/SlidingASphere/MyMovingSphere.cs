using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMovingSphere : MonoBehaviour
{
    /// <summary>
    /// 最大速度
    /// </summary>
    [SerializeField,Range(0,100)]
    private float MaxSpeed;
    

    /// <summary>
    /// 最大加速度
    /// </summary>
    [SerializeField,Range(0,10)]
    private float MaxAcceleration;


    /// <summary>
    /// 小球位置范围约束
    /// </summary>
    public Rect AllowArea = new Rect(-5, -5, 10, 10);
    
    /// <summary>
    /// 当前速度
    /// </summary>
    public Vector3 velocity;

    
    /// <summary>
    /// 开启反弹
    /// </summary>
    [Header("开启反弹")]
    public bool EnableBounce;

    /// <summary>
    /// 反弹参数
    /// </summary>
    [Header("反弹参数")]
    [Range(0,10)]
    public float Bounciness = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");  //返回值在[-1,1]之间
        playerInput.y = Input.GetAxis("Vertical");
        

        //playerInput.Normalize();
        //绝对运动,把玩家输入直接应用到位置
        //playerInput = Vector2.ClampMagnitude(playerInput, 1);  //把playerInput的长度约束在1以内
        //transform.localPosition = new Vector3(playerInput.x, 0.5f, playerInput.y);
        
        
        //相对运动,,把玩家输入当作为球上一次状态的位移
        // playerInput = Vector2.ClampMagnitude(playerInput, 1);
        // var displacement = new Vector3(playerInput.x, 0, playerInput.y);
        // transform.localPosition += displacement;
        
        //把输入当作速度 
        // playerInput = Vector2.ClampMagnitude(playerInput, 1);
        // var velocity = new Vector3(playerInput.x, 0, playerInput.y) * MaxSpeed;
        // //计算这一帧的位移
        // var displacement = velocity * Time.deltaTime;
        // transform.localPosition += displacement;
        
        
        //把输入当作加速度
        /*
        playerInput = Vector2.ClampMagnitude(playerInput, 1);
        var acceleration = new Vector3(playerInput.x, 0, playerInput.y) * MaxSpeed;
        //根据当前加速度计算当前速度
        velocity += acceleration * Time.deltaTime; 
        //再根据当前速度计算位置
        var displacement = velocity * Time.deltaTime;
        //应用位移
        transform.localPosition += displacement;
        
        */
        
        //把输入当作加速度，直接控制速度
        //控制加速度而不是速度会产生更平滑的运动，
        //但这也削弱了我们对球体的控制。就像我们在开车而不是走路。在大多数游戏中，
        //玩家都需要更直接地控制速度，所以让我们回到这一方法。然而，加速度的应用确实产生了更平滑的运动。

        //移动方向
        playerInput = Vector2.ClampMagnitude(playerInput, 1);
        //期望的速度
        var desiredVelocity = new Vector3(playerInput.x, 0, playerInput.y) * MaxSpeed;
        //一帧速度最大改变量
        float maxSpeedChange = MaxAcceleration * Time.deltaTime;
      
        //叠加当前速度,并把速度控制在期望速度以内
        
        // if (velocity.x < desiredVelocity.x)
        //     velocity.x = Mathf.Min(velocity.x + maxSpeedChange,desiredVelocity.x);
        //
        // else if (velocity.x > desiredVelocity.x)
        //     velocity.x = Mathf.Max(velocity.x - maxSpeedChange,desiredVelocity.x);
        //
        // if (velocity.z < desiredVelocity.z)
        //     velocity.z = Mathf.Min(velocity.z + maxSpeedChange,desiredVelocity.z);
        //
        // else if (velocity.z > desiredVelocity.z)
        //     velocity.z = Mathf.Max(velocity.z - maxSpeedChange,desiredVelocity.z);
    
        //以下代码等效于上面
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
        
        //再根据当前速度计算位置
        var displacement = velocity * Time.deltaTime;
        //应用位移,计算出新的位置
        var newPosition = transform.localPosition + displacement;
        
        //对于超出范围的位置进行重新约束
        // if (!AllowArea.Contains(new Vector2(newPosition.x, newPosition.z)))
        // {
        //     newPosition.x = Mathf.Clamp(newPosition.x, AllowArea.xMin, AllowArea.xMax);
        //     newPosition.z = Mathf.Clamp(newPosition.z, AllowArea.yMin, AllowArea.yMax);
        // }
        
        //对于超出边缘的球,减去它在碰撞方向上的速度,使之可以在另外一个方向上继续沿着边缘移动
        if (newPosition.x > AllowArea.xMax)
        {
            newPosition.x = AllowArea.xMax;
            //开启反弹
            if (EnableBounce)
            {
                velocity.x = -velocity.x * Bounciness;
                return;
            }
            velocity.x = 0;
        }
        else if (newPosition.x < AllowArea.xMin)
        {
            newPosition.x = AllowArea.xMin;
            if (EnableBounce)
            {
                velocity.x = -velocity.x * Bounciness;
                return;
            }
            velocity.x = 0;
        }
        
        if (newPosition.z > AllowArea.yMax)
        {
            newPosition.z = AllowArea.yMax;
            if (EnableBounce)
            {
                velocity.z = -velocity.z * Bounciness;
                return;
            }
            velocity.z = 0;
        }
        else if (newPosition.z < AllowArea.yMin)
        {
            newPosition.z = AllowArea.yMin;
            if (EnableBounce)
            {
                velocity.z = -velocity.z * Bounciness;
                return;
            }
            velocity.z = 0;
        }

        transform.localPosition = newPosition;

    }
    
}
