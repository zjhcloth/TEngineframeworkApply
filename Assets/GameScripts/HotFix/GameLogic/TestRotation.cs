//=====================================================
// - FileName: TestRotation.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/28 09:54:53
// - Description:
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestRotation : MonoBehaviour
{
       public Transform rotationCenter; // 旋转中心  
       public float rotationSpeed = 50.0f; // 旋转速度  
       public Vector3 rotationAxis = Vector3.right; // 旋转轴，默认为Y轴  
       public Transform rotationCenter1; // 旋转中心  
       void Update()
       {
              // 计算旋转角度  
              float angle = rotationSpeed * Time.deltaTime;

              // 围绕旋转中心旋转  
              transform.RotateAround(rotationCenter.position, rotationCenter.forward, angle);
       }
}