using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTeleport : MonoBehaviour
{
    [SerializeField] private float teleportDistance = 10f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(-Mathf.Sign(transform.position.x) * teleportDistance, 0, 0);
        }
    }
}
