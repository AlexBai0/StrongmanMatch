using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbs : MonoBehaviour
{
    public int column;
    public int row;
    public int targetX;
    public int targetY;
    public bool isMatched;
    private GameObject otherOrb;
    private Board board;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    public float swipeAngle = 0;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
        isMatched = false;

    }

    // Update is called once per frame
    void Update()
    {   
        targetX = column;
        targetY = row;
        if(Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //Move to target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allOrbs[column, row] = this.gameObject;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //Move to target
            tempPosition = new Vector2(transform.position.x,targetY );
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            tempPosition = new Vector2(transform.position.x,targetY );
            transform.position = tempPosition;
            board.allOrbs[column, row] = this.gameObject;
        }
        FindMatches();
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(0f, 0f, 0f, .8f);
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
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        Debug.Log(swipeAngle);
        MoveOrbs();
        Debug.Log(board.counter);

    }
    void MoveOrbs()
    {
        if(swipeAngle>-45 && swipeAngle <= 45 && column<board.width)
        {
            //Right
            otherOrb = board.allOrbs[column + 1, row];
            otherOrb.GetComponent<Orbs>().column -= 1;
            column += 1;
            board.counter += 1;

        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row<board.height) 
        {
            //Up
            otherOrb = board.allOrbs[column , row+1];
            otherOrb.GetComponent<Orbs>().row -= 1;
            row += 1;
            board.counter += 1;

        }
        else if (swipeAngle > 135 || swipeAngle <= -135 && column>0)
        {
            //Left
            otherOrb = board.allOrbs[column - 1, row];
            otherOrb.GetComponent<Orbs>().column += 1;
            column -= 1;
            board.counter += 1;

        }
        else if (swipeAngle > -135 && swipeAngle <= -45 && row >0)
        {
            //Down
            otherOrb = board.allOrbs[column, row - 1];
            otherOrb.GetComponent<Orbs>().row += 1;
            row -= 1;
            board.counter += 1;

        }
    }
    void FindMatches(){
        if(column> 0 && column< board.width-1){
            GameObject leftOrb = board.allOrbs[column-1,row];
            GameObject rightOrb = board.allOrbs[column+1,row];
            if(leftOrb.tag == this.gameObject.tag && rightOrb.tag == this.gameObject.tag){
                leftOrb.GetComponent<Orbs>().isMatched = true;
                rightOrb.GetComponent<Orbs>().isMatched = true;
                this.gameObject.GetComponent<Orbs>().isMatched = true;
            }
        }
    }
}
