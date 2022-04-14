using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHealth : MonoBehaviour
{
    public float blockHealth = 20;
    public float yes;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (blockHealth == 0)
        {
            Debug.Log("Block Destroyed");
            Destroy(this.gameObject);
        }
    }
}
