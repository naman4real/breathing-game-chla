﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(-.41f, 20.2f, -14.68f);
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;

    }
}