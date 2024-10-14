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
        

        
        public Joystick joystick;
        public float speed = 5;
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
                    float x = transform.position.x;
                    Vector3 direction = new Vector3(v.x, v.y, 0);
                    controller.Move(direction * speed * Time.deltaTime);
                    float afterX = transform.position.x;
                    // if (afterX>x)
                    // {
                    //     transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    // }
                    // else
                    // {
                    //     transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    // }
                    //transform.rotation = Quaternion.LookRotation(new Vector3(v.x, v.y, 0));
                    playerRenderer.flipX = v.x < 0;
                    if (Mathf.Abs(v.x)> Mathf.Abs(v.y))//左右为主
                    {
                        LeaderMoveDir = v.x>0?MoveDir.Right:MoveDir.Left;
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
