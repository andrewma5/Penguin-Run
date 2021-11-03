using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private float moveSpeed = 5f;
    private float jumpLength = 2f;
    private Rigidbody rb;
    private Rigidbody rbc;
    private Animator anim;
    private bool lerping = false;
    private Vector3 end;
    public bool ded = false;
    private Transform t;

    private Vector2 TouchStart;
    private Vector2 TouchEnd;

    private float maxZ = 0f;

    private bool grounded = false;
    private bool aboveGround = false;
    LayerMask s;

    UIManager uiman;

    private string nextSwipe = "";


    public enum Powerup
    {
        Nothing,
        Flight,
        SpeedJump
    }

    public Powerup CurrentPower = Powerup.Nothing;
    private int extraLives = 0;
    private bool helicoptering = false;
    private Vector3 heliDest;
    private int helitime = 0;
    private float heliSpinSpeed = 1440;
    private float heliMoveSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rbc = transform.GetChild(0).GetComponent<Rigidbody>();
        t = transform.GetChild(0);
        anim = t.GetComponent<Animator>();
        uiman = GameObject.Find("Canvas").GetComponent<UIManager>();
        s = LayerMask.GetMask("Safe");
        uiman.UpdateLives(extraLives);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, 0f, Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed);
        if (!ded && !lerping && Input.GetKey(KeyCode.W))
        {
            MoveUp();
        }

        if (!ded && !lerping && Input.GetKey(KeyCode.A))
        {
            MoveLeft();
        }

        if (!ded && !lerping && Input.GetKey(KeyCode.S))
        {
            MoveBack();
        }

        if (!ded && !lerping && Input.GetKey(KeyCode.D))
        {
            MoveRight();
        }
        

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                TouchStart = touch.position;
                TouchEnd = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                TouchEnd = touch.position;
                CheckDirection();
            }
        }

        grounded = Physics.Raycast(transform.position, -Vector3.up, 1.2f, s);
        aboveGround = Physics.Raycast(transform.position, -Vector3.up, 4f, s);
    }

    private void FixedUpdate()
    {
        if (lerping && !helicoptering)
        {
            transform.position = Vector3.MoveTowards(transform.position, end, Time.deltaTime * moveSpeed);
        }

        if (lerping == true && transform.position == end && !helicoptering)
        {
            lerping = false;
            if (!ded && transform.position.z > maxZ && Mathf.RoundToInt(transform.position.z) % 2 == 0)
            {
                maxZ = transform.position.z;
            }
            if (nextSwipe.Length > 1)
            {
                switch (nextSwipe)
                {
                    case "up":
                        MoveUp();
                        nextSwipe = "";
                        break;
                    case "down":
                        MoveBack();
                        nextSwipe = "";
                        break;
                    case "left":
                        MoveLeft();
                        nextSwipe = "";
                        break;
                    case "right":
                        MoveRight();
                        nextSwipe = "";
                        break;
                }
            }
        }

        if (helicoptering)
        {
            transform.position = Vector3.MoveTowards(transform.position, heliDest, Time.deltaTime * heliMoveSpeed);
            if (transform.position == heliDest)
            {
                heliDest = new Vector3(heliDest.x, 2.5f, heliDest.z);
            }
            transform.Rotate(0, heliSpinSpeed * Time.deltaTime, 0);
            if ((aboveGround && helitime > 100) || helitime > 250)
            {
                anim.SetBool("Helicopter", false);
                rb.isKinematic = false;
                helitime = 0;
                rb.useGravity = true;
                heliSpinSpeed = 1440;
                heliMoveSpeed = 2;
                nextSwipe = "";
                helicoptering = false;
            }

            helitime += 1;
            heliSpinSpeed -= 4;
            heliMoveSpeed -= Time.deltaTime * 0.8f;
        }

        if (!ded && t.transform.position.y < 0 && !aboveGround && !helicoptering)
        {
            if (extraLives > 0) 
            {
                helicoptering = true;
                rb.isKinematic = true;
                rb.useGravity = false;
                anim.SetBool("Helicopter", true);
                heliDest = new Vector3(Mathf.RoundToInt(transform.position.x), 4, Mathf.RoundToInt(transform.position.z));
                extraLives -= 1;
                uiman.UpdateLives(extraLives);
            }
            else
            {
                ded = true;
                rb.isKinematic = true;
                anim.SetTrigger("Death");
                StartCoroutine(Restart());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            ActivatePower();
            Destroy(other.gameObject);
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(2);
        /*
        rb.isKinematic = false;
        transform.position = new Vector3(0, 1.5f, -2f);
        t.transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        ded = false;
        t.transform.rotation = Quaternion.identity;
        maxZ = 0;
        */
        GameManager.instance.LoseGame(uiman.GetScore());
    }

    private void MoveLeft()
    {
        if (!ded && !lerping && !helicoptering && (grounded || CurrentPower == Powerup.Flight))
        {
            transform.LookAt(transform.position + Vector3.left);
            end = transform.position + new Vector3(-jumpLength, 0, 0);
            lerping = true;
            if (CurrentPower == Powerup.SpeedJump)
            {
                anim.SetTrigger("FastJump");
            }
            else if (CurrentPower == Powerup.Nothing)
            {
                anim.SetTrigger("Jump");
            }
        }
        else if(!ded && lerping)
        {
            nextSwipe = "left";
        }
    }

    private void MoveRight()
    {
        if (!ded && !lerping && !helicoptering && (grounded || CurrentPower == Powerup.Flight))
        {
            transform.LookAt(transform.position + Vector3.right);
            end = transform.position + new Vector3(jumpLength, 0, 0);
            lerping = true;
            if (CurrentPower == Powerup.SpeedJump)
            {
                anim.SetTrigger("FastJump");
            }
            else if (CurrentPower == Powerup.Nothing)
            {
                anim.SetTrigger("Jump");
            }
        }
        else if (!ded && lerping)
        {
            nextSwipe = "right";
        }
    }

    private void MoveUp()
    {
        if (!ded && !lerping && !helicoptering && (grounded || CurrentPower == Powerup.Flight))
        {
            transform.LookAt(transform.position + Vector3.forward);
            end = transform.position + new Vector3(0, 0, jumpLength);
            lerping = true;
            if (CurrentPower == Powerup.SpeedJump)
            {
                anim.SetTrigger("FastJump");
            }
            else if (CurrentPower == Powerup.Nothing)
            {
                anim.SetTrigger("Jump");
            }
        }
        else if (!ded && lerping)
        {
            nextSwipe = "up";
        }
    }

    private void MoveBack()
    {
        if (!ded && !lerping && !helicoptering && (grounded || CurrentPower == Powerup.Flight))
        {
            transform.LookAt(transform.position + Vector3.back);
            end = transform.position + new Vector3(0, 0, -jumpLength);
            if (end.z + 8 >= maxZ)
            {
                lerping = true;
                if (CurrentPower == Powerup.SpeedJump)
                {
                    anim.SetTrigger("FastJump");
                }
                else if (CurrentPower == Powerup.Nothing)
                {
                    anim.SetTrigger("Jump");
                }
            }
        }
        else if (!ded && lerping)
        {
            nextSwipe = "down";
        }
    }

    private void CheckDirection()
    {
        if (Mathf.Abs(TouchStart.y - TouchEnd.y) > 5 || Mathf.Abs(TouchStart.x - TouchEnd.x) > 5)
        {
            if (Mathf.Abs(TouchStart.y - TouchEnd.y) > Mathf.Abs(TouchStart.x - TouchEnd.x))
            {
                if (TouchStart.y > TouchEnd.y)
                {
                    MoveBack();
                }
                else
                {
                    MoveUp();
                }
            }
            else
            {
                if (TouchStart.x > TouchEnd.x)
                    MoveLeft();
                else
                    MoveRight();
            }
        }
    }

    private void ActivatePower()
    {
        int power = Random.Range(1, 4);
        switch (power)
        {
            case 1:
                ActivateExtraLife();
                uiman.OneUp();
                break;
            case 2:
                ActivateSpeedJump();
                uiman.StartTimer("speed");
                break;
            case 3:
                ActivateFlight();
                uiman.StartTimer("flight");
                break;
        }
    }

    private void ActivateSpeedJump()
    {
        CurrentPower = Powerup.SpeedJump;
        moveSpeed = 10f;
        rb.useGravity = true;
        anim.SetBool("Flying", false);
    }

    private void ActivateFlight()
    {
        CurrentPower = Powerup.Flight;
        rb.useGravity = false;
        moveSpeed = 5f;
        anim.SetBool("Flying", true);
    }

    private void ActivateExtraLife()
    {
        ActivateNothing();
        if (extraLives < 5)
        {
            extraLives += 1;
            uiman.UpdateLives(extraLives);
        }
    }

    public void ActivateNothing()
    {
        CurrentPower = Powerup.Nothing;
        moveSpeed = 5f;
        rb.useGravity = true;
        anim.SetBool("Flying", false);
    }

    public float GetMaxZ()
    {
        return maxZ;
    }
}
