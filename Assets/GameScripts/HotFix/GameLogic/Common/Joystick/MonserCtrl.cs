//=====================================================
// - FileName: MonserCtrl.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/19 11:18:12
// - Description: 怪物控制类
//======================================================
using System.Collections;
using System.Collections.Generic;
using GameLogic;
using TEngine;
using UnityEngine;
public class MonserCtrl : MonoBehaviour
{
       public float speed = 1;
       public float stopDistance = 3f; // 停止移动的距离
       public bool isMoving = true;  // 是否正在移动
       public ThirdPersonFlow[] otherPartners;
       public ThirdPersonSolution leader;
       private SpriteRenderer playerRenderer;
       private Animator animator;
       void Start ()
       {
              otherPartners = Object.FindObjectsOfType<ThirdPersonFlow>();
              leader = Object.FindObjectsOfType<ThirdPersonSolution>()[0];
              playerRenderer = GetComponent<SpriteRenderer>();
              animator = GetComponent<Animator>();
              GameEvent.AddEventListener<MoveDir>(1, OnLeaderMoveDirChange);
       }
       private void OnLeaderMoveDirChange(MoveDir dir)
       {
              if (dir != MoveDir.None)
                     isMoving = true;
       }
       void Update ()
       {
              if (isMoving)
              {
                     // 计算两个对象之间的距离
                     float distance = Vector2.Distance(transform.position, leader.transform.position);

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
                     animator.SetBool("IsWalking", false);
              }
              playerRenderer.flipX = leader.transform.position.x < transform.position.x;
       }
       
       void MoveTowardsTarget()
       {
              // 计算从当前对象到目标的方向
              Vector3 direction = (leader.transform.position - transform.position).normalized;
              
              // 更新物体位置
              transform.position = (Vector3)transform.position + direction * speed * Time.deltaTime;
       }
}