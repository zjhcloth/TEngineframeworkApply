using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybordControll : MonoBehaviour
{
    public Transform mapController;
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var position = mapController.position;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = Vector3.forward * vertical + Vector3.right * horizontal;
        position += -direction * speed * Time.deltaTime;
        mapController.position = position;
        //if (position.z < -8) mapController.position = new Vector3(position.x,position.y,-7.9f);
        //if (position.z > -3.8f) mapController.position = new Vector3(position.x,position.y,-3.9f);
        //if (position.x < -1) mapController.position = new Vector3(-0.95f,position.y,position.z);
        //if (position.x > 1f) mapController.position = new Vector3(0.98f,position.y,position.z);
    }
}
