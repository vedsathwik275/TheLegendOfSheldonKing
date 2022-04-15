using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 8;
    public Rigidbody2D rb;
    public GameObject SpawnPoint;
    public GameObject Drill;

    Vector2 input;

    public float health = 0;
    public float digSpeed = 0;
    public float score = 0;
    public float fuel = 0;
    public float starthp = 50;
    public float startdig = 1;
    public float startfuel;


    [HideInInspector]
    public bool isFacingLeft;
    [HideInInspector]
    public bool isFacingUp;

    public bool spawnFacingLeft;
    public bool spawnFacingUp;
    public Vector2 facingLeft;
    public Vector2 facingUp;

    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        health = starthp;
        digSpeed = startdig;
        fuel = startfuel;

        healthBar.SetMaxHealth(starthp);


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

        if (health <= 0)
        {
            SceneManager.LoadScene("End");
        }
    }

    private void FixedUpdate()
    {
        if (input.y > 0)
        {
            rb.velocity = input * 0;
        }
        else
        {
            rb.velocity = input * moveSpeed;
        }

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
        if (input.y >= 0 && !isFacingUp)
        {
            //turn drill off
        }
        if (input.y < 0)
        {
            //turn drill on
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