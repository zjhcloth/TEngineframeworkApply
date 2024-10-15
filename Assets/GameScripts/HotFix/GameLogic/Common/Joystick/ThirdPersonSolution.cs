// Copyright (c) Bian Shanghai
// https://github.com/Bian-Sh/UniJoystick
// Licensed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Buffers;
using TEngine;

public enum MoveDir
{
    None,
    Up,
    Down,
    Left,
    Right,
    
}

namespace zFrame.Example
{
    using UnityEngine;
    using zFrame.UI;
    public class ThirdPersonSolution : MonoBehaviour
    {
        public Transform bgTf;
        public Transform frontTf;
        public Joystick joystick;
        public float speed = 1;
        public float bgSpeed = 0.1f;
        public float frontSpeed = 2f;
        CharacterController controller;
        private Animator animator;
        private SpriteRenderer playerRenderer;
        private MoveDir moveDir = MoveDir.None;


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
            joystick.OnValueChanged.AddListener(v =>
            {
                if (v.magnitude != 0)
                {
                    animator.SetBool("IsWalking", true);

                    Vector3 direction = new Vector3(v.x, 0, v.y);
                    controller.Move(direction * speed * Time.deltaTime);
                    
                    // if (transform.position.z < -6.4) transform.position = new Vector3(transform.position.x,transform.position.y,-6.3f);
                    // if (transform.position.z > -3.8f) transform.position = new Vector3(transform.position.x,transform.position.y,-3.9f);
                    // if (transform.position.x < -1) transform.position = new Vector3(-0.95f,transform.position.y,transform.position.z);
                    // if (transform.position.x > 1) transform.position = new Vector3(0.95f,transform.position.y,transform.position.z);
                    
                    //背景移动
                    Vector3 bgDirection = new Vector3(-v.x,  -v.y,0);//背景要反向移动
                    bgTf.position += bgDirection * bgSpeed * Time.deltaTime;
                    
                    // if (bgTf.position.z < -6.4) bgTf.position = new Vector3(bgTf.position.x,bgTf.position.y,-6.3f);
                    // if (bgTf.position.z > -3.8f) bgTf.position = new Vector3(bgTf.position.x,bgTf.position.y,-3.9f);
                    // if (bgTf.position.x < -1) bgTf.position = new Vector3(-0.95f,bgTf.position.y,bgTf.position.z);
                    // if (bgTf.position.x > 1) bgTf.position = new Vector3(0.95f,bgTf.position.y,bgTf.position.z);
                    
                    //前景移动
                    Vector3 frontDirection = new Vector3(-v.x,  0,-v.y);//背景要反向移动
                    frontTf.position += frontDirection * frontSpeed * Time.deltaTime;

                    
                    //角色转向
                    playerRenderer.flipX = v.x < 0;
                    //变阵用的主角朝向
                    if (Mathf.Abs(v.x)> Mathf.Abs(v.y))//左右为主
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
                    animator.SetBool("IsWalking", false);
                }
            });
        }
        
        
        

        void Update()
        {
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
        
    }
}
