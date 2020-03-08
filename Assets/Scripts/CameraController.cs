using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(-.41f, 20.2f, -23.68f);
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;

    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CameraController : MonoBehaviour
//{
//    public GameObject player;
//    private Vector3 offset; 
//    private Vector3 offsetYPos = new Vector3(0f, 20.2f, 0f);

//    float distance;
//    Vector3 playerPrevPos;
//    Vector3 playerMoveDir;

//    void Start()
//    {
//        offset = transform.position - player.transform.position;
//        distance = offset.magnitude;
//        playerPrevPos = player.transform.position;
//    }

//    // Update is called once per frame
//    void LateUpdate()
//    {
//        // Update player move direction at each cycle.
//        playerMoveDir = player.transform.position - playerPrevPos;
//        if (playerMoveDir != Vector3.zero)
//        {
//            playerMoveDir.Normalize();

//            // Place camera behind player object at all times with offset.
//            transform.position = player.transform.position + offsetYPos - playerMoveDir * distance;

//            // Point forward vector to point at player object.
//            transform.LookAt(player.transform.position);

//            // Adjust angle of camera.
//            transform.rotation *= Quaternion.Euler(-30, 0, 0);

//            // Update previous position to current position at each cycle end.
//            playerPrevPos = player.transform.position;
//        }

//    }
//}
