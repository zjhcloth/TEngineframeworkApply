//=====================================================
// - FileName: MapCtrl.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/11/05 11:11:07
// - Description:
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zFrame.UI;

public class MapCtrl : MonoBehaviour
{
       public Joystick joystick;
       public float bgSpeed = 0.5f;
       public float frontSpeed = 1f;
       public Transform bgTf;
       public Transform frontTf;
       void Start ()
       {
              joystick.OnValueChanged.AddListener(v =>
              {
                     if (v.magnitude != 0)
                     {
                            //背景移动
                            Vector3 bgDirection = new Vector3(-v.x,  -v.y,0);//背景要反向移动
                            //bgTf.position += bgDirection * bgSpeed * Time.deltaTime;
                            Vector3 bgPosition = bgTf.position;
                            bgPosition += bgDirection * bgSpeed * Time.deltaTime;
                            bgPosition.x = Mathf.Clamp(bgPosition.x, -1.2f, 1.2f);
                            bgPosition.y = Mathf.Clamp(bgPosition.y, 1.5f, 2.2f);
                            bgTf.position = bgPosition;
                            // // //前景移动
                            Vector3 frontDirection = new Vector3(-v.x,  0,-v.y);//背景要反向移动
                            //frontTf.position += frontDirection * frontSpeed * Time.deltaTime;
                            Vector3 fontfPosition = frontTf.position;
                            fontfPosition += frontDirection * frontSpeed * Time.deltaTime;
                            fontfPosition.x = Mathf.Clamp(fontfPosition.x, -16f, 2f);
                            fontfPosition.z = Mathf.Clamp(fontfPosition.z, -6f, 6f);
                            frontTf.position = fontfPosition;
                     }
              });
       }
       void Update ()
       {
       
       }
}