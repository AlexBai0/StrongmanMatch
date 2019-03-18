using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbs : MonoBehaviour
{
    [Header("Orbs Variables")]
    public int column;
    public int row;
    public ArrayList previousColumn;
    public ArrayList previousRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;
    public bool isMovable = false;
    private GameObject otherOrb;
    private Board board;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    public float swipeAngle = 0;
    public float swipeResist = 1f;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
        previousColumn = new ArrayList();
        previousRow = new ArrayList();

        previousRow.Add(row);
        previousColumn.Add(column);
    }

    // Update is called once per frame
    void Update()
    {
        findMatches();
        targetX = column;
        targetY = row;
        if(Mathf.Abs(targetX - transform.position.x) > .1 )
        {
            //Move to target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.4f);
            if (board.allOrbs[column, row] != this.gameObject)
            {
                board.allOrbs[column, row] = this.gameObject;
            }
        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //Move to target
            tempPosition = new Vector2(transform.position.x,targetY );
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.4f);
            if (board.allOrbs[column, row] != this.gameObject)
            {
                board.allOrbs[column, row] = this.gameObject;
            }
        }
        else
        {
            tempPosition = new Vector2(transform.position.x,targetY );
            transform.position = tempPosition;
        }
        if (isMatched)
        {
            SpriteRenderer mysprite = GetComponent<SpriteRenderer>();
            mysprite.color = new Color(0f, 0f, 0f, .2f);
        }
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint( Input.mousePosition);
    }

    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }
    private void CalculateAngle()
    {
        if(Mathf.Abs(finalTouchPosition.y-firstTouchPosition.y)>swipeResist|| Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MoveOrbs();
            Debug.Log(board.counter);
        }
    }
    void MoveOrbs()
    {
        if(swipeAngle>-45 && swipeAngle <= 45 && column<board.width && isMovable)
        {
            //Right
            otherOrb = board.allOrbs[column + 1, row];
            otherOrb.GetComponent<Orbs>().column -= 1;
            column += 1;
            board.counter += 1;
            previousRow.Add(row);
            previousColumn.Add(column);


        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row<board.height && isMovable) 
        {
            //Up
            otherOrb = board.allOrbs[column , row+1];
            otherOrb.GetComponent<Orbs>().row -= 1;
            row += 1;
            board.counter += 1;
            previousRow.Add(row);
            previousColumn.Add(column);

        }
        else if ((swipeAngle > 135 || swipeAngle <= -135 )&& column>0 && isMovable)
        {
            //Left
            otherOrb = board.allOrbs[column - 1, row];
            otherOrb.GetComponent<Orbs>().column += 1;
            column -= 1;
            board.counter += 1;
            previousRow.Add(row);
            previousColumn.Add(column);

        }
        else if (swipeAngle > -135 && swipeAngle <= -45 && row >0 && isMovable)
        {
            //Down
            otherOrb = board.allOrbs[column, row - 1];
            otherOrb.GetComponent<Orbs>().row += 1;
            row -= 1;
            board.counter += 1;
            previousRow.Add(row);
            previousColumn.Add(column);

        }
        StartCoroutine(CheckMove());
    }
    public void findMatches()
    {
        if (column > 0 && column < board.width-1)
        {
            GameObject leftOrb = board.allOrbs[column - 1,row];
            GameObject rightOrb = board.allOrbs[column + 1,row];
            if (leftOrb != null && rightOrb != null)
            {
                if (leftOrb.tag == this.gameObject.tag && rightOrb.tag == this.gameObject.tag)
                {
                    leftOrb.GetComponent<Orbs>().isMatched = true;
                    rightOrb.GetComponent<Orbs>().isMatched = true;
                    isMatched = true;
                }
            }
        }
        if (row > 0 && row < board.height - 1)
        {
            GameObject downOrb = board.allOrbs[column , row-1];
            GameObject upOrb = board.allOrbs[column , row+1];
            if (downOrb != null && upOrb != null)
            {
                if (downOrb.tag == this.gameObject.tag && upOrb.tag == this.gameObject.tag)
                {
                    downOrb.GetComponent<Orbs>().isMatched = true;
                    upOrb.GetComponent<Orbs>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
    public IEnumerator CheckMove()
    {
        yield return new WaitForSeconds(.5f);

        if (otherOrb != null)
        {
            if (isMatched)
            {
                board.ClearBoard();
            }
        }

    }
}
