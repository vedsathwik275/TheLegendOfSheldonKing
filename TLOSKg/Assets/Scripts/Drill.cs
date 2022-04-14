using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public BlockHealth healthDiff;
    public GameObject player;
    public Movement pScript;
    // Start is called before the first frame update

    // Update is called once per frame
    void Start()
    {
        player = GameObject.Find("player");
        pScript = player.GetComponent<Movement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("down");
        }

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        healthDiff = gameObject.GetComponent<BlockHealth>();
        
        if (other.gameObject.tag == "block")
        {
            pScript.health -= other.gameObject.GetComponent<BlockHealth>().damage;
            other.gameObject.GetComponent<BlockHealth>().blockHealth =  other.gameObject.GetComponent<BlockHealth>().blockHealth - pScript.digSpeed;
            if(other.gameObject.GetComponent<BlockHealth>().blockHealth <=0)
            {
                Destroy(other.gameObject);
                pScript.score += other.gameObject.GetComponent<BlockHealth>().point;
                pScript.health += other.gameObject.GetComponent<BlockHealth>().hpboost;
                pScript.digSpeed += other.gameObject.GetComponent<BlockHealth>().dboost;
                pScript.moveSpeed += other.gameObject.GetComponent<BlockHealth>().sboost;
            }
            
            // Debug.Log(healthDiff.blockHealth);

        }
    }
}
