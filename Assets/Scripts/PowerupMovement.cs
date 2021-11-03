using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMovement : MonoBehaviour
{
    private Vector3 StartPos;
    private Vector3 EndPos;
    private bool up = true;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        EndPos = transform.position + new Vector3(0, 1f, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (up)
        {
            transform.position = Vector3.MoveTowards(transform.position, EndPos, Time.deltaTime * 0.75f);
            if (transform.position == EndPos)
            {
                up = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, StartPos, Time.deltaTime * 0.75f);
            if (transform.position == StartPos)
            {
                up = true;
            }
        }
        transform.Rotate(0, 90 * Time.deltaTime, 0);
    }
}
