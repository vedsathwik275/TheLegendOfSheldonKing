using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public BlockHealth healthDiff;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("down");
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        healthDiff = gameObject.GetComponent<BlockHealth>();
        Debug.Log("colliding");
        if (other.gameObject.tag == "block")
        {
            Debug.Log("Destroy");
            // Debug.Log(healthDiff.blockHealth);
            Destroy(other.gameObject);
        }
    }
}
