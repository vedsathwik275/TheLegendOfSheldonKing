using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHealth : MonoBehaviour
{
    public float blockHealth;
    public float yes;
    public string bname;
    public float point;
    public float damage;
    public float dboost;
    public float sboost;
    public float hpboost;
    // Start is called before the first frame update
    void Start()
    {
        bname = this.name;
        Debug.Log(name);
        switch (name)
        {
            case "BlackTree(Clone)":
                blockHealth = 1;
                point = 10;
                damage = 0;
                dboost = .1f;
                sboost = 1;
                hpboost = 10;
                break;

            case "BlueTree(Clone)":
                blockHealth = 1;
                point = 10;
                damage = 0;
                dboost = 1;
                sboost = 1;
                hpboost = 20;
                break;

            case "Diamond(Clone)":
                blockHealth = 100;
                point = 10000;
                damage = 0;
                dboost = 0;
                sboost = 0;
                hpboost = 0;
                break;

            case "dirt(Clone)":
                blockHealth = 5;
                point = 1;
                damage = 0;
                dboost = 0;
                sboost = 0;
                hpboost = 0;
                break;

            case "Emerald(Clone)":
                blockHealth = 60;
                point = 5000;
                damage = 0;
                dboost = 0;
                sboost = 0;
                hpboost = 0;
                break;

            case "Gold(Clone)":
                blockHealth = 40;
                point = 2500;
                damage = 0;
                dboost = 0;
                sboost = 0;
                hpboost = 0;
                break;

            case "HardStone(Clone)":
                blockHealth = 500;
                point = 100;
                damage = 0;
                dboost = 0;
                sboost = 0;
                hpboost = 0;
                break;

            case "HardStoneDotted(Clone)":
                blockHealth = 350;
                point = 75;
                damage = 0;
                dboost = 0;
                sboost = 0;
                hpboost = 0;
                break;

            case "KeyStone(Clone)":
                blockHealth = 999999;
                point = 1;
                damage = 0;
                dboost = 0;
                sboost = 0;
                hpboost = 0;
                break;

            case "Lava(Clone)":
                blockHealth = 1000;
                point = -1000;
                damage = 0.5f;
                dboost = 0;
                sboost = 0;
                hpboost = 0;
                break;

            case "Plant(Clone)":
                blockHealth = 2;
                point = 1;
                damage = 0;
                dboost = 0;
                sboost = 0;
                hpboost = 0;
                break;

            case "Stone(Clone)":
                blockHealth = 250;
                point = 50;
                damage = 0;
                dboost = 0;
                sboost = 0;
                hpboost = 0;
                break;

            case "StoneDotted(Clone)":
                blockHealth = 150;
                point = 25;
                damage = 0;
                dboost = 0;
                sboost = 0;
                hpboost = 0;
                break;

            case "TNT(Clone)":
                blockHealth = 30;
                point = 500;
                damage = 0;
                dboost = 0;
                sboost = 0;
                hpboost = -25;
                break;

            case "Void(Clone)":
                blockHealth = 999999999999999;
                point = -1000000000;
                damage = 0.5f;
                dboost = 0;
                sboost = 0;
                hpboost = 0;
                break;

            case "WhiteTree(Clone)":
                blockHealth = 1;
                point = 10;
                damage = 0;
                dboost = .5f;
                sboost = .5f;
                hpboost = 15;
                break;


        }
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
