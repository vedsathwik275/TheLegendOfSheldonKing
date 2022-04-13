using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    public GameObject SpawnPoint;

    Vector2 input;

    public float health;
    public float digSpeed;
    public float score;
    public float fuel;

    [HideInInspector]
    public bool isFacingLeft;
    [HideInInspector]
    public bool isFacingUp;

    public bool spawnFacingLeft;
    public bool spawnFacingUp;
    public Vector2 facingLeft;
    public Vector2 facingUp;
    // Start is called before the first frame update
    void Start()
    {
        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        facingUp = new Vector2(transform.localScale.x, transform.localScale.y);
        if (spawnFacingLeft)
        {
            transform.localScale = facingLeft;
            isFacingLeft = true;
        }
        if (spawnFacingUp)
        {
            transform.localScale = facingUp;
            isFacingUp = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input.Normalize();

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = UtilsClass.GetMouseWorldPosition();
            Vector3 mouseDir = (mousePos - transform.position);
            mouseDir.Normalize();
            float attackOffset = 5f;
            Vector3 attackPos = transform.position + mouseDir;
            Debug.Log("" + mouseDir);
            Debug.Log("" + attackPos);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = input * moveSpeed;

        if (input.x > 0 && isFacingLeft)
        {
            isFacingLeft = false;
            FlipH();
        }
        if (input.x < 0 && !isFacingLeft)
        {
            isFacingLeft = true;
            FlipH();
        }
        if (input.y > 0 && !isFacingUp)
        {
            isFacingUp = true;
            FlipV();
        }
        if (input.y < 0 && isFacingUp)
        {
            isFacingUp = false;
            FlipV();
        }
    }

    private void FlipH()
    {
        if (isFacingLeft)
        {
            transform.localScale = facingLeft;
        }
        if (!isFacingLeft)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
    private void FlipV()
    {
        if (isFacingUp)
        {
            transform.localScale = facingUp;
        }
        if (!isFacingUp)
        {
            transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);
        }
    }
}
