using TEngine;
using UnityEngine;
using zFrame.UI;
namespace GameLogic
{
    public class ThirdPersonSolution : MonoBehaviour
    {
        public Transform bgTf;
        public Transform frontTf;
        public Joystick joystick;
        public float speed = 3;//普通移动速度
        public float bgSpeed = 1f;
        public float frontSpeed = 2f;
        CharacterController controller;
        private Animator animator;
        public SpriteRenderer playerRenderer;
        private MoveDir moveDir = MoveDir.None;
        private bool userCtrl = false;// 是否摇杆控制
        private Vector3 targetPosition;
        public MonserCtrl monster;
        
        public Vector3 TargetPosition
        {
            set
            {
                if (targetPosition == value) return;
                targetPosition = value;
                GameEvent.Send(2, value);
                speed = 2;
                Debug.Log($"---SetTargetPosition:{value}");
            }
            get
            {
                //targetPosition = new Vector3(-4, -1, -7);
                Debug.Log($"++++++++++GetTargetPosition:{targetPosition}");
                return targetPosition;
            }
        }
        public MoveDir LeaderMoveDir
        {
            set
            {
                //if (moveDir == value) return;
                moveDir = value;
                GameEvent.Send(1, moveDir);
            }
            get { return moveDir; }
        }
        void Start()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();
            playerRenderer = GetComponent<SpriteRenderer>();
            monster = Object.FindObjectsOfType<MonserCtrl>()[0];
            joystick.OnValueChanged.AddListener(v =>
            {
                if (v.magnitude != 0)
                {
                    speed = 2;
                    userCtrl = true;
                    animator.SetBool("IsWalking", true);
                    //角色不用移动，移动背景就好了
                    Vector3 direction = new Vector3(v.x, 0, v.y);
                    controller.Move(direction * speed * Time.deltaTime);
                    //背景移动
                    Vector3 bgDirection = new Vector3(-v.x,  -v.y,0);//背景要反向移动
                    //bgTf.position += bgDirection * bgSpeed * Time.deltaTime;
                    Vector3 bgPosition = bgTf.position;
                    bgPosition += bgDirection * bgSpeed * Time.deltaTime;
                    bgPosition.x = Mathf.Clamp(bgPosition.x, -9f, 9f);
                    bgPosition.y = Mathf.Clamp(bgPosition.y, 2.6f, 6f);
                    bgTf.position = bgPosition;
                
                    // //前景移动
                    // Vector3 frontDirection = new Vector3(-v.x,  0,-v.y);//背景要反向移动
                    // //frontTf.position += frontDirection * frontSpeed * Time.deltaTime;
                    // Vector3 fontfPosition = frontTf.position;
                    // fontfPosition += frontDirection * frontSpeed * Time.deltaTime;
                    // fontfPosition.x = Mathf.Clamp(fontfPosition.x, -4.6f, 4.3f);
                    // fontfPosition.z = Mathf.Clamp(fontfPosition.z, -6.6f, -0.3f);
                    // frontTf.position = fontfPosition;


                    //角色转向
                    playerRenderer.flipX = v.x < 0;
                    flipX = playerRenderer.flipX;
                    //变阵用的主角朝向
                    if (Mathf.Abs(v.x) > Mathf.Abs(v.y))//左右为主
                    {
                        LeaderMoveDir = v.x > 0 ? MoveDir.Right : MoveDir.Left;
                    }
                    else //上下为主
                    {
                        LeaderMoveDir = v.y > 0 ? MoveDir.Up : MoveDir.Down;
                    }
                }
                else
                {
                    userCtrl = false;
                    animator.SetBool("IsWalking", false);
                    LeaderMoveDir = MoveDir.None;
                    //autoWalk = true;
                    //TargetPosition = new Vector3(-4,-2,-7);
                }
            });
        }
        
        public bool flipX = false;  // 是否反转
        public bool isMoving = true;  // 是否正在移动
        public float stopDistance = 3f; // 停止移动的距离
        void Update()
        {
            // if (!userCtrl)
            // {
            //     transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
            //     
            // }
            if (!userCtrl)
            {
                if (isMoving)
                {
                    playerRenderer.flipX = monster.transform.position.x < transform.position.x;
                    // 计算两个对象之间的距离
                    float distance = Vector2.Distance(transform.position, monster.transform.position);

                    // 当距离小于等于停止距离时，停止移动
                    if (distance <= stopDistance)
                    {
                        isMoving = false; // 停止移动
                        return;           // 不再更新位置
                    }
                    animator.SetBool("IsWalking", true);
                    // 移动物体朝向目标方向
                    MoveTowardsTarget();
                }
                else
                {
                    playerRenderer.flipX = monster.transform.position.x < transform.position.x;
                    animator.SetBool("IsWalking", false);
                    float distance = Vector2.Distance(transform.position, monster.transform.position);
                    if(distance > stopDistance)
                    {
                        isMoving = true; // 停止移动
                    }
                }
            }

            
            
            // // 检测输入并播放不同的动画
            // if (Input.GetKey(KeyCode.W))
            // {
            //     // 播放行走动画
            //     animator.SetBool("IsWalking", true);
            // }
            // else
            // {
            //     // 停止行走动画，播放待机动画
            //     animator.SetBool("IsWalking", false);
            // }
        }
        
        void MoveTowardsTarget()
        {
            // 计算从当前对象到目标的方向
            Vector3 direction = (monster.transform.position - transform.position).normalized;
            playerRenderer.flipX = direction.x < 0;
            // 更新物体位置
            transform.position = (Vector3)transform.position + direction * speed * Time.deltaTime;
        }
        
    }
}
