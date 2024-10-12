// Copyright (c) Bian Shanghai
// https://github.com/Bian-Sh/UniJoystick
// Licensed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace zFrame.Example
{
    using UnityEngine;
    using zFrame.UI;
    public class ThirdPersonSolution : MonoBehaviour
    {
       public Joystick joystick;
        public float speed = 5;
        CharacterController controller;
        void Start()
        {
            controller = GetComponent<CharacterController>();

            joystick.OnValueChanged.AddListener(v =>
            {
                if (v.magnitude != 0)
                {
                    float x = transform.position.x;
                    Vector3 direction = new Vector3(v.x, v.y, 0);
                    controller.Move(direction * speed * Time.deltaTime);
                    float afterX = transform.position.x;
                    if (afterX>x)
                    {
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);;
                    }
                    else
                    {
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);;
                    }
                    //transform.rotation = Quaternion.LookRotation(new Vector3(v.x, v.y, 0));
                }
            });
        }
    }
}
