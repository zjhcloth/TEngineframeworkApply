using TEngine;
using UnityEngine;
using UnityEngine.UIElements;
using zFrame.UI;

public enum FlowPosType
{
    None,
    FontUp,
    Font,
    FontDown,
    Up,
    Leader,
    Down,
    BackUp,
    Back,
    BackDown,
    
}

public class ThirdPersonFlow : MonoBehaviour
{
    public Joystick joystick;
    public Transform leader;
    private SpriteRenderer playerRenderer;
    private Animator animator;
    CharacterController controller;
    public float speed = 5;
    public FlowPosType posType;
    private Vector3 targetPosition;
    private bool changeTarget = false;
    void Start()
    {
        GameEvent.AddEventListener<MoveDir>(1, OnLeaderMoveDirChange);
        playerRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        joystick.OnValueChanged.AddListener(v =>
        {
            if (v.magnitude != 0)
            {
                animator.SetBool("IsWalking", true);
                //float x = transform.position.x;
                //Vector3 direction = new Vector3(v.x, v.y, 0);
                //controller.Move(direction * speed * Time.deltaTime);
                //float afterX = transform.position.x;
                playerRenderer.flipX = v.x < 0;
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }
        });
    }

    /**
     * 当主角方向改变的时候，这边要改变阵型
     */
    private void OnLeaderMoveDirChange(MoveDir dir)
    {
        changeTarget = true;
        switch (posType)
        {
            case FlowPosType.FontUp:
                if (dir == MoveDir.None || dir == MoveDir.Right)
                {
                    targetPosition = leader.position + new Vector3(0.3f, 0.3f, 0);
                }
                else if (dir == MoveDir.Left)
                {
                    targetPosition = leader.position + new Vector3(-0.3f, 0.3f, 0);
                }
                else if (dir == MoveDir.Up)
                {
                    targetPosition = leader.position + new Vector3(-0.3f, 0.3f, 0);
                }
                else if (dir == MoveDir.Down)
                {
                    targetPosition = leader.position + new Vector3(0.3f, -0.3f, 0);
                }

                break;
            case FlowPosType.Font:
                if (dir == MoveDir.None || dir == MoveDir.Right)
                {
                    targetPosition = leader.position + new Vector3(0.3f, 0, 0);
                }
                else if (dir == MoveDir.Left)
                {
                    targetPosition = leader.position + new Vector3(-0.3f, 0, 0);
                }
                else if (dir == MoveDir.Up)
                {
                    targetPosition = leader.position + new Vector3(0, 0.3f, 0);
                }
                else if (dir == MoveDir.Down)
                {
                    targetPosition = leader.position + new Vector3(0, -0.3f, 0);
                }

                break;
            case FlowPosType.FontDown:
                if (dir == MoveDir.None || dir == MoveDir.Right)
                {
                    targetPosition = leader.position + new Vector3(0.3f, -0.3f, 0);
                }
                else if (dir == MoveDir.Left)
                {
                    targetPosition = leader.position + new Vector3(-0.3f, -0.3f, 0);
                }
                else if (dir == MoveDir.Up)
                {
                    targetPosition = leader.position + new Vector3(0.3f, 0.3f, 0);
                }
                else if (dir == MoveDir.Down)
                {
                    targetPosition = leader.position + new Vector3(-0.3f, -0.3f, 0);
                }

                break;
            case FlowPosType.Up:
                if (dir == MoveDir.None || dir == MoveDir.Right)
                {
                    targetPosition = leader.position + new Vector3(0, 0.3f, 0);
                }
                else if (dir == MoveDir.Left)
                {
                    targetPosition = leader.position + new Vector3(0, 0.3f, 0);
                }
                else if (dir == MoveDir.Up)
                {
                    targetPosition = leader.position + new Vector3(-0.3f, 0, 0);
                }
                else if (dir == MoveDir.Down)
                {
                    targetPosition = leader.position + new Vector3(0.3f, 0, 0);
                }

                break;
            case FlowPosType.Down:
                if (dir == MoveDir.None || dir == MoveDir.Right)
                {
                    targetPosition = leader.position + new Vector3(0, -0.3f, 0);
                }
                else if (dir == MoveDir.Left)
                {
                    targetPosition = leader.position + new Vector3(0, -0.3f, 0);
                }
                else if (dir == MoveDir.Up)
                {
                    targetPosition = leader.position + new Vector3(0.3f, 0, 0);
                }
                else if (dir == MoveDir.Down)
                {
                    targetPosition = leader.position + new Vector3(-0.3f, 0, 0);
                }

                break;
            case FlowPosType.BackUp:
                if (dir == MoveDir.None || dir == MoveDir.Right)
                {
                    targetPosition = leader.position + new Vector3(-0.3f, 0.3f, 0);
                }
                else if (dir == MoveDir.Left)
                {
                    targetPosition = leader.position + new Vector3(0.3f, 0.3f, 0);
                }
                else if (dir == MoveDir.Up)
                {
                    targetPosition = leader.position + new Vector3(-0.3f, -0.3f, 0);
                }
                else if (dir == MoveDir.Down)
                {
                    targetPosition = leader.position + new Vector3(0.3f, 0.3f, 0);
                }

                break;
            case FlowPosType.BackDown:
                if (dir == MoveDir.None || dir == MoveDir.Right)
                {
                    targetPosition = leader.position + new Vector3(-0.3f, -0.3f, 0);
                }
                else if (dir == MoveDir.Left)
                {
                    targetPosition = leader.position + new Vector3(0.3f, -0.3f, 0);
                }
                else if (dir == MoveDir.Up)
                {
                    targetPosition = leader.position + new Vector3(0.3f, -0.3f, 0);
                }
                else if (dir == MoveDir.Down)
                {
                    targetPosition = leader.position + new Vector3(-0.3f, 0.3f, 0);
                }

                break;
            case FlowPosType.Back:
                if (dir == MoveDir.None || dir == MoveDir.Right)
                {
                    targetPosition = leader.position + new Vector3(-0.3f, 0, 0);
                }
                else if (dir == MoveDir.Left)
                {
                    targetPosition = leader.position + new Vector3(0.3f, 0, 0);
                }
                else if (dir == MoveDir.Up)
                {
                    targetPosition = leader.position + new Vector3(0, -0.3f, 0);
                }
                else if (dir == MoveDir.Down)
                {
                    targetPosition = leader.position + new Vector3(0, 0.3f, 0);
                }

                break;
        }

        Debug.Log(targetPosition);
    }
    
    
    public float followSpeed = 5f;  // 跟随速度
    public float minDistance = 0.5f;  // 最小距离
    public float separationForce = 0.5f;  // 分离力度
    public PartnerController[] otherPartners;

    void Update()
    {
        if (changeTarget)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            // 检测并进行分离调整
            foreach (PartnerController partner in otherPartners)
            {
                if (partner != this)
                {
                    float distance = Vector2.Distance(transform.position, partner.transform.position);
                    if (distance < minDistance)
                    {
                        Vector2 separationDirection = (transform.position - partner.transform.position).normalized;
                        transform.position += (Vector3)(separationDirection * separationForce * Time.deltaTime);
                    }
                }
            }

            float leaderDistance = Vector2.Distance(transform.position, leader.position);
            if (leaderDistance < minDistance)
            {
                Vector2 separationDirection = (transform.position - leader.position).normalized;
                transform.position += (Vector3)(separationDirection * separationForce * Time.deltaTime);
            }
        }
    }
    
}

