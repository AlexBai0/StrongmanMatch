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
    private bool cleaned = false;

    // Start is called before the first frame update
    void Start()
    {
        bgTiles = new BgTile[width, height];
        allOrbs = new GameObject[width, height];
        SetUp();
        startTurn();
    }
    //private void Update()
    //{
    //    if (counter == 5 && !cleaned)
    //    {
    //        checkAllOrbs();
    //        ClearBoard();
    //        cleaned = true;
    //        counter = 0;
    //    }
    //}
    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPos = new Vector2(i, j);

                GameObject backGroundTiles = Instantiate(tilePrefab, tempPos, Quaternion.identity) as GameObject;
                backGroundTiles.transform.parent = this.transform;
                backGroundTiles.name = "(" + i + "," + j + ")";
                int orbToUse = Random.Range(0, orbs.Length);
                while (matchGen(i, j, orbs[orbToUse]))
                {
                    orbToUse = Random.Range(0, orbs.Length);

                }
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
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
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
    public void startTurn()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                allOrbs[i, j].GetComponent<Orbs>().isMovable = true;
            }
        }
    }
    public void checkAllOrbs()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                allOrbs[i, j].GetComponent<Orbs>().findMatches();
            }
        }
    }
    public bool matchGen(int column, int row, GameObject orb)
    {
        if (column > 1 && row > 1)
        {
            if (allOrbs[column - 1, row].tag == orb.tag && allOrbs[column - 2, row].tag == orb.tag)
            {
                return true;
            }
            if (allOrbs[column, row - 1].tag == orb.tag && allOrbs[column, row - 2].tag == orb.tag)
            {
                return true;
            }
        } else if (column <= 1 || row <= 1)
        {
            if (column > 1 && (allOrbs[column - 1, row].tag == orb.tag && allOrbs[column - 2, row].tag == orb.tag))
            {
                return true;
            }
            if (row > 1 && (allOrbs[column, row - 1].tag == orb.tag && allOrbs[column, row - 2].tag == orb.tag))
            {
                return true;
            }
        }
        return false;
    }
    public void clearMatchOn(int i, int j)
    {
        if (allOrbs[i, j].GetComponent<Orbs>().isMatched)
        {
            Destroy(allOrbs[i, j]);
            allOrbs[i, j] = null;
        }
    }
    public void ClearBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allOrbs[i, j] != null)
                {
                    clearMatchOn(i, j);
                }
            }
        }
        StartCoroutine(FallDown());
    }
    private IEnumerator FallDown()
    {
        int nullCount = 0; //count how many null in this column
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allOrbs[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allOrbs[i, j].GetComponent<Orbs>().row -= nullCount; //fall down nullCount rows
                    allOrbs[i, j] = null;
                }
            }
            nullCount = 0;
        }

        yield return new WaitForSeconds(.8f);
        StartCoroutine(FillOrbs());

    }
    private void RefillOrbs()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allOrbs[i, j] == null)
                {
                    Vector2 tempPos = new Vector2(i, j);
                    int orbToUse = Random.Range(0, orbs.Length);
                    GameObject newOrb = Instantiate(orbs[orbToUse], tempPos, Quaternion.identity);
                    allOrbs[i, j] = newOrb;
                }
            }
        }
    }
    private bool MatchesOn()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allOrbs[i, j] != null)
                {
                    if (allOrbs[i, j].GetComponent<Orbs>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private IEnumerator FillOrbs()
    {
        RefillOrbs();
        startTurn();
        yield return new WaitForSeconds(.5f);
        while (MatchesOn())
        {
            yield return new WaitForSeconds(.5f);
            ClearBoard();
        }
    }
}
