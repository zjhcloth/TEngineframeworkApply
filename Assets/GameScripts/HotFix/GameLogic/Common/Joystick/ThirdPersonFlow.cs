﻿using TEngine;
using UnityEngine;
using zFrame.UI;

public enum FlowPosType
{
    None,
    FontUp,
    Font,
    FontDown,
    Up,
    Center,
    Down,
    BackUp,
    Back,
    BackDown,
    
}

public enum MoveDir
{
    None,
    Up,
    Down,
    Left,
    Right,
    
}

namespace GameLogic
{
    public class ThirdPersonFlow : MonoBehaviour
    {
        public ThirdPersonSolution leader;
        public FlowPosType leaderPosType;
        private SpriteRenderer playerRenderer;
        private Animator animator;

        private float speed = 2;
        public FlowPosType posType;
        private Vector3 targetPosition;
        public bool changeTarget = false;
        public float moveSpeed = 2;
        public float followSpeed = 5f; // 跟随速度
        public float minDistance = 1.4f; // 最小距离
        public float separationForce = 2f; // 分离力度
        public ThirdPersonFlow[] otherPartners;
        public MonserCtrl monster;

        void Start()
        {
            otherPartners = Object.FindObjectsOfType<ThirdPersonFlow>();
            monster = Object.FindObjectsOfType<MonserCtrl>()[0];
            leader = Object.FindObjectsOfType<ThirdPersonSolution>()[0];
            GameEvent.AddEventListener<Vector3>(2, OnLeaderTargetChangeChange);
            GameEvent.AddEventListener<MoveDir>(1, OnLeaderMoveDirChange);
            playerRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            // joystick.OnValueChanged.AddListener(v =>
            // {
            //
            //     if (v.magnitude != 0)
            //     {
            //         animator.SetBool("IsWalking", true);
            //         //float x = transform.position.x;
            //         //Vector3 direction = new Vector3(v.x, v.y, 0);
            //         //controller.Move(direction * speed * Time.deltaTime);
            //         //float afterX = transform.position.x;
            //         playerRenderer.flipX = v.x < 0;
            //     }
            //     else
            //     {
            //         animator.SetBool("IsWalking", false);
            //
            //     }
            // });
        }

        private void OnLeaderTargetChangeChange(Vector3 target)
        {
            // changeTarget = true;
            // speed = moveSpeed;
            // Debug.Log($"----------target:{target}");
            // targetPosition = target;
        }

        /**
     * 当主角方向改变的时候，这边要改变阵型
     */
        private void OnLeaderMoveDirChange(MoveDir dir)
        {
            //Debug.Log($"dir:---{dir}");
            if (dir == MoveDir.None)
            {
                changeTarget = false;
            }
            else
            {
                changeTarget = true;
                speed = followSpeed;
                targetPosition = leader.transform.position + GetOffSet(dir);
            }
        }

        /// <summary>
        /// 默认布阵布局
        /// 7（BackUp）,4（Up）,1（FontUp）
        /// 8（Back）,5（Center）,2（Font）
        /// 9（BackDown）,6（Down）,3（FontDown）
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private Vector3 GetOffSet(MoveDir dir)
        {
            float offSetX = 1.3f;
            float offSetY = 1f;
            Vector3 offSet = new Vector3(0, 0, 0);
            if (leaderPosType == FlowPosType.Center)
            {
                switch (posType)
                {
                    case FlowPosType.FontUp: //1
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.Font: //2
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-0, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.FontDown: //3
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-offSetX, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.Up: //4
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.Center: //5
                        break;
                    case FlowPosType.Down: //6
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.BackUp: //7
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }

                        break;
                    case FlowPosType.Back: //8
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }

                        break;
                    case FlowPosType.BackDown: //9
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-offSetX, offSetY, 0);
                        }

                        break;
                }
            }
            else if (leaderPosType == FlowPosType.FontUp)
            {
                switch (posType)
                {
                    case FlowPosType.FontUp: //1

                        break;
                    case FlowPosType.Font: //2
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.FontDown: //3
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(2 * offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.Up: //4
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }

                        break;
                    case FlowPosType.Center: //5
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }

                        break;
                    case FlowPosType.Down: //6
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(2 * offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(2 * offSetX, offSetY, 0);
                        }

                        break;
                    case FlowPosType.BackUp: //7
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, 2 * offSetY, 0);
                        }

                        break;
                    case FlowPosType.Back: //8
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-2 * offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(2 * offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, 2 * offSetY, 0);
                        }

                        break;
                    case FlowPosType.BackDown: //9
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-2 * offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(2 * offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(2 * offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(2 * offSetX, 2 * offSetY, 0);
                        }

                        break;
                }
            }
            else if (leaderPosType == FlowPosType.Font)
            {
                switch (posType)
                {
                    case FlowPosType.FontUp: //1
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.Font: //2

                        break;
                    case FlowPosType.FontDown: //3
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.Up: //4
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-offSetX, offSetY, 0);
                        }

                        break;
                    case FlowPosType.Center: //5
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }

                        break;
                    case FlowPosType.Down: //6
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }

                        break;
                    case FlowPosType.BackUp: //7
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-2 * offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(2 * offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-offSetX, 2 * offSetY, 0);
                        }

                        break;
                    case FlowPosType.Back: //8
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, 2 * offSetY, 0);
                        }

                        break;
                    case FlowPosType.BackDown: //9
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-2 * offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(2 * offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, 2 * offSetY, 0);
                        }

                        break;
                }
            }
            else if (leaderPosType == FlowPosType.FontDown)
            {
                switch (posType)
                {
                    case FlowPosType.FontUp: //1
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-2 * offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.Font: //2
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.FontDown: //3


                        break;
                    case FlowPosType.Up: //4
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-2 * offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-2 * offSetX, offSetY, 0);
                        }

                        break;
                    case FlowPosType.Center: //5
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-offSetX, offSetY, 0);
                        }

                        break;
                    case FlowPosType.Down: //6
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }

                        break;
                    case FlowPosType.BackUp: //7
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-2 * offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(2 * offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-2 * offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-2 * offSetX, 2 * offSetY, 0);
                        }

                        break;
                    case FlowPosType.Back: //8
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-2 * offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(2 * offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-offSetX, 2 * offSetY, 0);
                        }

                        break;
                    case FlowPosType.BackDown: //9
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, 2 * offSetY, 0);
                        }

                        break;
                }
            }
            else if (leaderPosType == FlowPosType.Up)
            {
                switch (posType)
                {
                    case FlowPosType.FontUp: //1
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.Font: //2
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.FontDown: //3
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(2 * offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(2 * offSetX, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.Up: //4

                        break;
                    case FlowPosType.Center: //5
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.Down: //6
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(2 * offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.BackUp: //7
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }

                        break;
                    case FlowPosType.Back: //8
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }

                        break;
                    case FlowPosType.BackDown: //9
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(2 * offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(2 * offSetX, offSetY, 0);
                        }

                        break;
                }
            }
            else if (leaderPosType == FlowPosType.Down)
            {
                switch (posType)
                {
                    case FlowPosType.FontUp: //1
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-2 * offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(2 * offSetX, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.Font: //2
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.FontDown: //3
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.Up: //4
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(2 * offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.Center: //5
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.Down: //6


                        break;
                    case FlowPosType.BackUp: //7
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-2 * offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(2 * offSetX, offSetY, 0);
                        }

                        break;
                    case FlowPosType.Back: //8
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }

                        break;
                    case FlowPosType.BackDown: //9
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }

                        break;
                }
            }
            else if (leaderPosType == FlowPosType.BackUp)
            {
                switch (posType)
                {
                    case FlowPosType.FontUp: //1
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, -2 * offSetY, 0);
                        }

                        break;
                    case FlowPosType.Font: //2
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(2 * offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-2 * offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, -2 * offSetY, 0);
                        }

                        break;
                    case FlowPosType.FontDown: //3
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(2 * offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-2 * offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(2 * offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(2 * offSetX, -2 * offSetY, 0);
                        }

                        break;
                    case FlowPosType.Up: //4
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.Center: //5
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.Down: //6
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(2 * offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(2 * offSetX, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.BackUp: //7

                        break;
                    case FlowPosType.Back: //8
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.BackDown: //9
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, -2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(2 * offSetX, 0, 0);
                        }

                        break;
                }
            }
            else if (leaderPosType == FlowPosType.Back)
            {
                switch (posType)
                {
                    case FlowPosType.FontUp: //1
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(2 * offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-2 * offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-offSetX, -2 * offSetY, 0);
                        }

                        break;
                    case FlowPosType.Font: //2
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {

                            offSet = new Vector3(2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, -2 * offSetY, 0);
                        }

                        break;
                    case FlowPosType.FontDown: //3
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(2 * offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-2 * offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, -2 * offSetY, 0);
                        }

                        break;
                    case FlowPosType.Up: //4
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-offSetX, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.Center: //5
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.Down: //6
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.BackUp: //7
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.Back: //8


                        break;
                    case FlowPosType.BackDown: //9
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }

                        break;
                }
            }
            else if (leaderPosType == FlowPosType.BackDown)
            {
                switch (posType)
                {
                    case FlowPosType.FontUp: //1
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(2 * offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-2 * offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-2 * offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-2 * offSetX, -2 * offSetY, 0);
                        }

                        break;
                    case FlowPosType.Font: //2
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(2 * offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-2 * offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-offSetX, -2 * offSetY, 0);
                        }

                        break;
                    case FlowPosType.FontDown: //3
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, -2 * offSetY, 0);
                        }

                        break;
                    case FlowPosType.Up: //4
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-2 * offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-2 * offSetX, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.Center: //5
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-offSetX, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.Down: //6
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(0, -offSetY, 0);
                        }

                        break;
                    case FlowPosType.BackUp: //7

                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, 2 * offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-2 * offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-2 * offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.Back: //8
                        if (dir == MoveDir.None || dir == MoveDir.Right)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Left)
                        {
                            offSet = new Vector3(0, offSetY, 0);
                        }
                        else if (dir == MoveDir.Up)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }
                        else if (dir == MoveDir.Down)
                        {
                            offSet = new Vector3(-offSetX, 0, 0);
                        }

                        break;
                    case FlowPosType.BackDown: //9

                        break;
                }
            }

            return offSet;
        }



        public bool isMoving = true;  // 是否正在移动
        public float stopDistance = 3f; // 停止移动的距离
        void Update()
        {
            if (changeTarget)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
                playerRenderer.flipX = leader.flipX;
                animator.SetBool("IsWalking", true);
            }
            else if (isMoving)
            {
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
                playerRenderer.flipX = monster.transform.position.x < transform.position.x;
            }
            else
            {
                animator.SetBool("IsWalking", false);
                playerRenderer.flipX = monster.transform.position.x < transform.position.x;
                float distance = Vector2.Distance(transform.position, monster.transform.position);
                if(distance > stopDistance)
                {
                    isMoving = true; // 停止移动
                }
            }
            
            //保持距离
            // 检测并进行分离调整
            foreach (ThirdPersonFlow partner in otherPartners)
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

            float leaderDistance = Vector2.Distance(transform.position, leader.transform.position);
            if (leaderDistance < minDistance)
            {
                Vector2 separationDirection = (transform.position - leader.transform.position).normalized;
                transform.position += (Vector3)(separationDirection * separationForce * Time.deltaTime);
            }
            
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
