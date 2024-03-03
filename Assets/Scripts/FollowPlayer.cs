using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float rotationSpeed = 5f;  // Adjust the rotation speed as needed

    
    // Update is called once per frame
    void Update()
    {
        // Update the camera position based on the player's position and offset
        transform.position = player.position + offset;

    }
}
