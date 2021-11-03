using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject Player;
    [SerializeField] float offset = -6f;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.transform.position.z > Player.GetComponent<Player>().GetMaxZ() - 2 || Player.GetComponent<Player>().GetMaxZ() <= 2)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Player.transform.position.z + offset);
        }
    }
}
