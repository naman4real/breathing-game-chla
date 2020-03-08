using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    private float speed = 20f;
    private float direction = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Use FixedUpdate to avoid object shaking.
    void FixedUpdate()
    {
        if (transform.position.x >= 37.5)
        {
            direction = -1;
        }
        else if(transform.position.x <= -22)
        {
            direction = 1;
        }
        transform.Translate(new Vector3(direction, 0, 0) * Time.deltaTime * speed);
    }
}
