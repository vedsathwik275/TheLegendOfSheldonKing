using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;



//ah yeah, it's commenting time
//The node class, you can interpret this as a cell in the matrix
public class Node
{
    //The type is what type of tile it'll inhabit. We'll get this from the future tile template class
    public string type = null;
    //self explainatory
    public bool collapsed = false;
    //position of the node within the matrix, not within the world
    public int posx;
    public int posy;
    public int sequence;
    //number of possibilities
    public int entropy;
    public HashSet<string> allPoss;

    //set of types that can go next to current node
    public HashSet<string> possibleUp;
    public HashSet<string> possibleDown;
    public HashSet<string> possibleL;
    public HashSet<string> possibleR;

    public TemplateKeys cellList;

    //constructor, only takes position, everything else can be set with a function or doesn't need to be 
    public Node(int xVal, int yVal)
    {
        posx = xVal;
        posy = yVal;
        string[] allTiles = { "voidTile", "lavaTile", "dirtTile", "stoneTile", "hardStoneTile", "TNT", "diamondTile", "plantTile", "emeraldTile", "blackTreeTile", "whiteTreeTile", "blueTreeTile", "keyStoneTile", "stoneDotTile", "hStoneTile", "goldTile" };
        collapsed = false;
        type = null;
        allPoss = new HashSet<string>(allTiles);
        possibleUp = new HashSet<string>(allTiles);
        possibleDown = new HashSet<string>(allTiles);
        possibleL = new HashSet<string>(allTiles);
        possibleR = new HashSet<string>(allTiles);

        entropy = allPoss.Count;


        cellList = new TemplateKeys();
        cellList.InitializeCells();

    }

    public void updateAdjacency()
    {
        possibleUp = new HashSet<string>();
        foreach (string key in allPoss)
        {
            TileTemplate tile = cellList.cellTypes[key];
            possibleUp.UnionWith(tile.possibleUp);
        }

        possibleDown = new HashSet<string>();
        foreach (string key in allPoss)
        {
            TileTemplate tile = cellList.cellTypes[key];
            possibleDown.UnionWith(tile.possibleDown);
        }

        possibleL = new HashSet<string>();
        foreach (string key in allPoss)
        {
            TileTemplate tile = cellList.cellTypes[key];
            possibleL.UnionWith(tile.possibleL);
        }

        possibleR = new HashSet<string>();
        foreach (string key in allPoss)
        {
            TileTemplate tile = cellList.cellTypes[key];
            possibleR.UnionWith(tile.possibleR);
        }
    }
    //I made this just to see if the creation was working. It is
    public void SetType(string label)
    {
        type = label;
    }

    //finds entropy of tile first, then chooses a tile from its possibilities list at random by converting the hashset to a list 
    public void Collapse()
    {
        entropy = allPoss.Count;
        Debug.Log("Type Pick Count");
        Debug.Log(allPoss.Count);
        int rnd = Random.Range(0, entropy);
        Debug.Log("Type Pick");
        Debug.Log(rnd);
        List<string> tempArray = new List<string>(allPoss);
        type = tempArray[rnd];
        collapsed = true;
        allPoss.Clear();
        allPoss.Add(type);
        entropy = allPoss.Count;

    }
}
//Tile template which we'll put inside of a dictionary. Contains each of our tiles' program data, like rules and indentifiers and such
public class TileTemplate
{
    public string name;
    public HashSet<string> possibleUp = new HashSet<string>();
    public HashSet<string> possibleDown = new HashSet<string>();
    public HashSet<string> possibleL = new HashSet<string>();
    public HashSet<string> possibleR = new HashSet<string>();
    //Constructor for tiletemplate. From my understanding we'll use this for each and every tile we have in the game. Pu, pd, etc they just mean possible+direction
    public TileTemplate(string label, HashSet<string> pu, HashSet<string> pd, HashSet<string> pl, HashSet<string> pr)
    {
        name = label;
        possibleUp = pu;
        possibleDown = pd;
        possibleL = pl;
        possibleR = pr;
    }
}

public class Floor
{
    //Floor is the matrix. It contains 2 int vars for the size (instead of one roomdimension var so we can have long or wide levels)
    public int sizeX;
    public int sizeY;
    //TotalNodes is propably redundant but I see myself using it in the future in generate function
    int totalNodes;
    public Node[,] grid;
    public Text helpertext = GameObject.Find("Text").GetComponent<Text>();

    //Why do we need a constructor? Because we need multiple levels. It takes in 2 vars for the size. It also takes grid but It does this because I was having a reference error
    //What might end up happening is that I'll remove the grid paramater and just make grid a variable within the floor class
    public Floor(int width, int height)
    {
        sizeX = width;
        sizeY = height;
        totalNodes = width * height; //area
        grid = new Node[sizeX, sizeY];

        //Loops through each column then the floor and creates a node
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                //create the actual array or the program will complain
                grid[y, x] = new Node(x, y);
                //Debugging stuff. Sets type of each node to hi and its coordinate and prints
                grid[y, x].SetType("hi" + y + x);
                //Debug.Log(grid[y, x]);
                //Debug.Log(grid[y, x].type);
                grid[y, x].SetType("null");
            }
        }
    }

    //Propagate: The following text is taken straight from my notepad
    //get floor and node location
    //take allPoss possibilities set from node
    //create 4 temporary neighboorrules hashsets 
    //for all possibilities within allPoss set, union possibles of each to the 4 neighboors
    //take nodes ajacent and update possible to be the unions
    void Propagate(int x, int y)
    {
        HashSet<string> tempUp = new HashSet<string>();
        HashSet<string> tempDown = new HashSet<string>();
        HashSet<string> tempL = new HashSet<string>();
        HashSet<string> tempR = new HashSet<string>();

        bool changeup = true;
        bool changedown = true;
        bool changel = true;
        bool changer = true;

        TemplateKeys tempkeys = new TemplateKeys();
        tempkeys.InitializeCells();
        //fill new temp hashs with rules of the superposition
        foreach (string pos in grid[y, x].allPoss)
        {
            tempUp.UnionWith(tempkeys.cellTypes[pos].possibleUp);
            tempDown.UnionWith(tempkeys.cellTypes[pos].possibleDown);
            tempL.UnionWith(tempkeys.cellTypes[pos].possibleL);
            tempR.UnionWith(tempkeys.cellTypes[pos].possibleR);

        }
        //check to see if ajacent tiles are within bounds first in order up down right left
        if (y + 1 >= sizeY) { changeup = false; }
        if (y - 1 < 0) { changedown = false; }
        if (x + 1 >= sizeX) { changer = false; }
        if (x - 1 < 0) { changel = false; }
        //for each variable, if it is true, go to the node in the respective direction and change its superposition then update ajacency
        if (changeup)
        {
            if (!grid[y + 1, x].collapsed)
            {
                grid[y + 1, x].allPoss.IntersectWith(tempUp);
                grid[y + 1, x].updateAdjacency();
                if (grid[y + 1, x].allPoss.Count == 0)
                {
                    grid[y + 1, x].allPoss.Add("voidTile");
                }
            }
        }
        if (changedown)
        {
            if (!grid[y - 1, x].collapsed)
            {
                grid[y - 1, x].allPoss.IntersectWith(tempDown);
                grid[y - 1, x].updateAdjacency();
                if (grid[y - 1, x].allPoss.Count == 0)
                {
                    grid[y - 1, x].allPoss.Add("voidTile");
                }
            }
        }
        if (changer)
        {
            if (!grid[y, x + 1].collapsed)
            {
                grid[y, x + 1].allPoss.IntersectWith(tempR);
                grid[y, x + 1].updateAdjacency();
                if (grid[y, x + 1].allPoss.Count == 0)
                {
                    grid[y, x + 1].allPoss.Add("voidTile");
                }
            }
        }
        if (changel)
        {
            if (!grid[y, x - 1].collapsed)
            {
                grid[y, x - 1].allPoss.IntersectWith(tempL);
                grid[y, x - 1].updateAdjacency();
                if (grid[y, x - 1].allPoss.Count == 0)
                {
                    grid[y, x - 1].allPoss.Add("voidTile");
                }
            }
        }

    }
    //Helper function. Takes most of the info about a node and puts it into a string which can be either printed to console or put onto a text message in game
    public void DumpNode(int x, int y)
    {
        int dumpposx = grid[y, x].posx;
        int dumpposy = grid[y, x].posy;
        int order = grid[y, x].sequence;
        bool colstatus = grid[y, x].collapsed;
        string super = "Possibilities: ";
        string type = grid[y, x].type;
        string rulesup = "Tiles that can go above this: ";
        string rulesdown = "Tiles that can go below this: ";
        string rulesleft = "Tiles that can go left of this: ";
        string rulesright = "Tiles that can go to the right of: ";

        foreach (string pos in grid[y, x].allPoss)
        {
            super = super + pos + ", ";
        }
        foreach (string pos in grid[y, x].possibleUp)
        {
            rulesup = rulesup + pos + ", ";
        }
        foreach (string pos in grid[y, x].possibleDown)
        {
            rulesdown = rulesdown + pos + ", ";
        }
        foreach (string pos in grid[y, x].possibleL)
        {
            rulesleft = rulesleft + pos + ", ";
        }
        foreach (string pos in grid[y, x].possibleR)
        {
            rulesright = rulesright + pos + ", ";
        }

        string dump = "Position: {0},{1} \n Type: {8} \n Generate# {9} \n Collapsed: {2} \n{3} \n{4} \n{5} \n{6} \n{7} \nDone";
        helpertext.text = string.Format(dump, dumpposx, dumpposy, colstatus, super, rulesup, rulesdown, rulesleft, rulesright, type, order);
    }


    //Generates world randomly in sequential order by row. No actual WFC used
    public void generateRandom()
    {
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                grid[y, x].Collapse();
                grid[y, x].updateAdjacency();
            }
        }
    }
    //wfc generate
    public void generate()
    {

        int complete = 0;
        List<Node> lowestents = new List<Node>();
        int lowestent = 99;

        Queue<Node> dirty = new Queue<Node>();
        while (complete < totalNodes)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {

                    //essentially, find a node with a low entropy. If it is lower than current low, clear list of low entropy nodes
                    //and start a new list with that node. If nodes have the same value, add to the list
                    if (grid[y, x].entropy < lowestent && !grid[y, x].collapsed)
                    {
                        lowestent = grid[y, x].entropy;
                        lowestents.Clear();
                        lowestents.Add(grid[y, x]);
                    }
                    else if (grid[y, x].entropy == lowestent && !grid[y, x].collapsed)
                    {
                        lowestents.Add(grid[y, x]);
                    }


                }
            }
            //creates an int based on entropy list size and chooses a node. Node gets collapsed, updated, and then propagated
            Debug.Log("Tile Pick Size");
            Debug.Log(lowestents.Count);
            int tocollapse = lowestents.Count;
            int rndm = Random.Range(0, tocollapse);
            Debug.Log("Tile Pick");
            Debug.Log(rndm);
            grid[lowestents[rndm].posy, lowestents[rndm].posx].Collapse();
            grid[lowestents[rndm].posy, lowestents[rndm].posx].updateAdjacency();
            this.Propagate(lowestents[rndm].posy, lowestents[rndm].posx);
            complete++;
            grid[lowestents[rndm].posy, lowestents[rndm].posx].sequence = complete;
            lowestent = 99;
            lowestents.Clear();

        }
    }
}

public class TemplateKeys
{
    public Dictionary<string, float> cellWeights = new Dictionary<string, float>();
    public Dictionary<string, TileTemplate> cellTypes = new Dictionary<string, TileTemplate>();

    public float voidWeight, stoneWeight, hardStoneWeight, lavaWeight, dirtWeight;

    public void InitializeCells()
    {
        //probability weights
        cellWeights.Add("voidTile", voidWeight);
        cellWeights.Add("stoneTile", stoneWeight);
        cellWeights.Add("hardStoneTile", hardStoneWeight);
        cellWeights.Add("lavaTile", lavaWeight);
        cellWeights.Add("dirtTile", dirtWeight);

        string[] allStone = { "stoneTile", "hardStoneTile", "dirtTile", "diamondTile", "plantTile", "emeraldTile", "blackTreeTile", "whiteTreeTile", "blueTreeTile", "keyStoneTile", "stoneDotTile", "hStoneTile", "goldTile" };   //stone
        TileTemplate stoneTile = new TileTemplate
        ("stoneTile",
         new HashSet<string>(allStone),
         new HashSet<string>(allStone),
         new HashSet<string>(allStone),
         new HashSet<string>(allStone));
        cellTypes.Add(stoneTile.name, stoneTile);

        string[] allVoid = { "voidTile", "hardStoneTile", "TNT" };   //void
        TileTemplate voidTile = new TileTemplate
        ("voidTile",
         new HashSet<string>(allVoid),
         new HashSet<string>(allVoid),
         new HashSet<string>(allVoid),
         new HashSet<string>(allVoid));
        cellTypes.Add(voidTile.name, voidTile);

        string[] allHardStone = { "voidTile", "stoneTile", "hardStoneTile", "lavaTile", "TNT", "stoneDotTile", "hStoneTile", "goldTile" };   //hardStone
        TileTemplate hardStoneTile = new TileTemplate
        ("hardStoneTile",
         new HashSet<string>(allHardStone),
         new HashSet<string>(allHardStone),
         new HashSet<string>(allHardStone),
         new HashSet<string>(allHardStone));
        cellTypes.Add(hardStoneTile.name, hardStoneTile);

        string[] allLava = { "hardStoneTile", "lavaTile", "TNT" };   //lavaTile
        TileTemplate lavaTile = new TileTemplate
        ("lavaTile",
         new HashSet<string>(allLava),
         new HashSet<string>(allLava),
         new HashSet<string>(allLava),
         new HashSet<string>(allLava));
        cellTypes.Add(lavaTile.name, lavaTile);

        string[] allDirt = { "stoneTile", "dirtTile", "diamondTile", "emeraldTile", "blackTreeTile", "whiteTreeTile", "blueTreeTile", "keyStoneTile", "goldTile" };   //dirtTile
        TileTemplate dirtTile = new TileTemplate
        ("dirtTile",
         new HashSet<string>(allDirt),
         new HashSet<string>(allDirt),
         new HashSet<string>(allDirt),
         new HashSet<string>(allDirt));
        cellTypes.Add(dirtTile.name, dirtTile);

        string[] allTNT = { "hardStoneTile", "lavalTile", "TNT", "voidTile" };   //tnt
        TileTemplate tntTile = new TileTemplate
        ("TNT",
         new HashSet<string>(allTNT),
         new HashSet<string>(allTNT),
         new HashSet<string>(allTNT),
         new HashSet<string>(allTNT));
        cellTypes.Add(tntTile.name, tntTile);

        string[] allDiamond = { "stoneTile", "hardStoneTile", "dirtTile", "plantTile", "blackTreeTile" };   //diamond
        TileTemplate diamondTile = new TileTemplate
        ("diamondTile",
         new HashSet<string>(allDiamond),
         new HashSet<string>(allDiamond),
         new HashSet<string>(allDiamond),
         new HashSet<string>(allDiamond));
        cellTypes.Add(diamondTile.name, diamondTile);

        string[] allPlant = { "stoneTile", "dirtTile", "plantTile", "blackTreeTile", "whiteTreeTile", "blueTreeTile" };   //plant
        TileTemplate plantTile = new TileTemplate
        ("plantTile",
         new HashSet<string>(allPlant),
         new HashSet<string>(allPlant),
         new HashSet<string>(allPlant),
         new HashSet<string>(allPlant));
        cellTypes.Add(plantTile.name, plantTile);

        string[] allEmerald = { "stoneTile", "hardStoneTile", "dirtTile", "plantTile", "whiteTreeTile" };   //emerald
        TileTemplate emeraldTile = new TileTemplate
        ("emeraldTile",
         new HashSet<string>(allEmerald),
         new HashSet<string>(allEmerald),
         new HashSet<string>(allEmerald),
         new HashSet<string>(allEmerald));
        cellTypes.Add(emeraldTile.name, emeraldTile);

        string[] allBlackTree = { "stoneTile", "hardStoneTile", "dirtTile", "plantTile", "blackTreeTile", "whiteTreeTile", "blueTreeTile" };   //blackTree
        TileTemplate blackTreeTile = new TileTemplate
        ("blackTreeTile",
         new HashSet<string>(allBlackTree),
         new HashSet<string>(allBlackTree),
         new HashSet<string>(allBlackTree),
         new HashSet<string>(allBlackTree));
        cellTypes.Add(blackTreeTile.name, blackTreeTile);

        string[] allWhiteTree = { "stoneTile", "hardStoneTile", "dirtTile", "plantTile", "blackTreeTile", "whiteTreeTile", "blueTreeTile" };   //whiteTree
        TileTemplate whiteTreeTile = new TileTemplate
        ("whiteTreeTile",
         new HashSet<string>(allWhiteTree),
         new HashSet<string>(allWhiteTree),
         new HashSet<string>(allWhiteTree),
         new HashSet<string>(allWhiteTree));
        cellTypes.Add(whiteTreeTile.name, whiteTreeTile);

        string[] allBlueTree = { "stoneTile", "hardStoneTile", "dirtTile", "plantTile", "blackTreeTile", "whiteTreeTile", "blueTreeTile" };   //blueTree
        TileTemplate blueTreeTile = new TileTemplate
        ("blueTreeTile",
         new HashSet<string>(allBlueTree),
         new HashSet<string>(allBlueTree),
         new HashSet<string>(allBlueTree),
         new HashSet<string>(allBlueTree));
        cellTypes.Add(blueTreeTile.name, blueTreeTile);

        string[] allKeyStoneTile = { "stoneTile", "hardStoneTile", "dirtTile", "keyStoneTile" };   //keyStone
        TileTemplate keyStoneTile = new TileTemplate
        ("keyStoneTile",
         new HashSet<string>(allKeyStoneTile),
         new HashSet<string>(allKeyStoneTile),
         new HashSet<string>(allKeyStoneTile),
         new HashSet<string>(allKeyStoneTile));
        cellTypes.Add(keyStoneTile.name, keyStoneTile);

        string[] allStoneDot = { "stoneTile", "hardStoneTile", "dirtTile", "stoneDotTile" };   //stoneDot
        TileTemplate stoneDotTile = new TileTemplate
        ("stoneDotTile",
         new HashSet<string>(allStoneDot),
         new HashSet<string>(allStoneDot),
         new HashSet<string>(allStoneDot),
         new HashSet<string>(allStoneDot));
        cellTypes.Add(stoneDotTile.name, stoneDotTile);

        string[] allHStoneDot = { "stoneTile", "hardStoneTile", "dirtTile", "hStoneTile", "lavaTile", "TNT" };   //hStoneDot
        TileTemplate hStoneDotTile = new TileTemplate
        ("hStoneTile",
         new HashSet<string>(allHStoneDot),
         new HashSet<string>(allHStoneDot),
         new HashSet<string>(allHStoneDot),
         new HashSet<string>(allHStoneDot));
        cellTypes.Add(hStoneDotTile.name, hStoneDotTile);

        string[] allGold = { "stoneTile", "hardStoneTile", "dirtTile", "plantTile", "blueTreeTile" };   //gold
        TileTemplate goldTile = new TileTemplate
        ("goldTile",
         new HashSet<string>(allGold),
         new HashSet<string>(allGold),
         new HashSet<string>(allGold),
         new HashSet<string>(allGold));
        cellTypes.Add(goldTile.name, goldTile);
    }
}


public class WFC : MonoBehaviour
{
    //Temporary values to manipulate in the editor to set floor size
    public int tempfloorx;
    public int tempfloory;
    public bool generated = false;
    public Floor level1;

    public GameObject inputx;
    public GameObject inputy;

    public int peeky = 0;
    public int peekx = 0;

    public GameObject[] tiles;
    public Text helptext;

    void Start()
    {
        //Initializes a floor then generates tiles and paints them
        level1 = new Floor(tempfloory, tempfloorx);
        level1.generate();
        Paint(level1);


    }


    void Update()
    {

    }

    //Uses dumpnode function basically. Reason this is here is so I could attach to the button
    public void testdump()
    {
        level1.DumpNode(peekx, peeky);
    }
    //Function I made that updates peek values. Attached this to input fields so you could enter in which tile you wanted to peek
    public void updateTestValues()
    {
        peekx = Int32.Parse(inputx.GetComponent<InputField>().text);
        peeky = Int32.Parse(inputy.GetComponent<InputField>().text);
    }


    //This function basically takes the floor class, accesses the grid and picks out every node and instantiates a tile in the node's place based on its label
    public void Paint(Floor floor)
    {
        for (int y = 0; y < floor.sizeY; y++)
        {
            for (int x = 0; x < floor.sizeX; x++)
            {
                Node n = floor.grid[y, x];
                switch (n.type)
                {
                    case "null":
                        Instantiate(tiles[0], new Vector3(n.posx, n.posy, 0), tiles[0].transform.rotation);
                        break;

                    case "voidTile":
                        Instantiate(tiles[0], new Vector3(n.posx, n.posy, 0), tiles[0].transform.rotation);
                        break;

                    case "hardStoneTile":
                        Instantiate(tiles[1], new Vector3(n.posx, n.posy, 0), tiles[4].transform.rotation);
                        break;

                    case "lavaTile":
                        Instantiate(tiles[2], new Vector3(n.posx, n.posy, 0), tiles[3].transform.rotation);
                        break;

                    case "stoneTile":
                        Instantiate(tiles[3], new Vector3(n.posx, n.posy, 0), tiles[1].transform.rotation);
                        break;

                    case "dirtTile":
                        Instantiate(tiles[4], new Vector3(n.posx, n.posy, 0), tiles[2].transform.rotation);
                        break;
                    case "TNT":
                        Instantiate(tiles[5], new Vector3(n.posx, n.posy, 0), tiles[3].transform.rotation);
                        break;
                    case "diamondTile":
                        Instantiate(tiles[6], new Vector3(n.posx, n.posy, 0), tiles[3].transform.rotation);
                        break;
                    case "plantTile":
                        Instantiate(tiles[7], new Vector3(n.posx, n.posy, 0), tiles[3].transform.rotation);
                        break;
                    case "emeraldTile":
                        Instantiate(tiles[8], new Vector3(n.posx, n.posy, 0), tiles[3].transform.rotation);
                        break;
                    case "blackTreeTile":
                        Instantiate(tiles[9], new Vector3(n.posx, n.posy, 0), tiles[3].transform.rotation);
                        break;
                    case "whiteTreeTile":
                        Instantiate(tiles[10], new Vector3(n.posx, n.posy, 0), tiles[3].transform.rotation);
                        break;
                    case "blueTreeTile":
                        Instantiate(tiles[11], new Vector3(n.posx, n.posy, 0), tiles[3].transform.rotation);
                        break;
                    case "keyStoneTile":
                        Instantiate(tiles[12], new Vector3(n.posx, n.posy, 0), tiles[3].transform.rotation);
                        break;
                    case "stoneDotTile":
                        Instantiate(tiles[12], new Vector3(n.posx, n.posy, 0), tiles[3].transform.rotation);
                        break;
                    case "hStoneTile":
                        Instantiate(tiles[13], new Vector3(n.posx, n.posy, 0), tiles[3].transform.rotation);
                        break;
                    case "goldTile":
                        Instantiate(tiles[13], new Vector3(n.posx, n.posy, 0), tiles[3].transform.rotation);
                        break;
                }
            }
        }
    }
}

