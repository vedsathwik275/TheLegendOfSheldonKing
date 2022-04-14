using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //LevelChooser
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "five")
        {
            SceneManager.LoadScene("FiveByFive");
        }
        if (other.gameObject.tag == "ten")
        {
            SceneManager.LoadScene("TenByTen");
        }
        if (other.gameObject.tag == "fifteen")
        {
            SceneManager.LoadScene("FifteenByFifteen");
        }

        // if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        // {
        //     if (other.gameObject.tag == "block")
        //     {
        //         Debug.Log("Destroy");
        //         Destroy(other.gameObject);
        //     }
        // }
    }

    /*Drill
        on collision(col2d other)
            if input arrow key down or s (down)
                check collision
                    destroy(other)
            if input arrow key left or a (left)
                check collision
                    destroy(other)
            if input arrow key right or d (right)
                check collision
                    destroy(other)*/

}
