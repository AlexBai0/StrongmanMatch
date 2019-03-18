using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int height;
    public int width;
    public GameObject tilePrefab;
    public int counter = 0;//indicate moves
    private BgTile[,] bgTiles;
    public GameObject[] orbs;
    public GameObject[,] allOrbs;
    
    // Start is called before the first frame update
    void Start()
    {
        bgTiles = new BgTile[width, height];
        allOrbs = new GameObject[width, height];
        SetUp();
        //Debug.Log(allOrbs[1,1]);
    }

    private void SetUp()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j= 0; j < height; j++)
            {
                Vector2 tempPos = new Vector2(i, j);
           
                GameObject backGroundTiles =  Instantiate(tilePrefab,tempPos, Quaternion.identity) as GameObject;
                backGroundTiles.transform.parent = this.transform;
                backGroundTiles.name = "(" + i + "," + j + ")";
                int orbToUse = Random.Range(0, orbs.Length);
                GameObject orb = Instantiate(orbs[orbToUse], tempPos, Quaternion.identity);
                orb.transform.parent = this.transform;
                orb.name = "(" + i + "," + j + ")";
                allOrbs[i, j] = orb;
            }
        }
    }

    public int[,] orbsMatri()
    {
        int[,] reslt = new int[width, height];
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                switch (allOrbs[i, j].tag)
                {
                    case "healOrb": {
                            reslt[i, j] = 0;
                            break;
                        }
                    case "fireOrb":
                        {
                            reslt[i, j] = 1;
                            break;
                        }
                    case "windOrb":
                        {
                            reslt[i, j] = 2;
                            break;
                        }
                    case "waterOrb":
                        {
                            reslt[i, j] = 3;
                            break;
                        }

                }
            }
        }
        return reslt;
    }
    public void checkOrbMatch()
    {

    }
}
