using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    public GameObject board;
    bool runOnce;
	// Use this for initialization
	void Start () {
        board = GameObject.Find("BoardCreator");
        
        runOnce = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(runOnce==false)
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                walkableTile(Direction.North);
            }
            if(Input.GetKeyDown(KeyCode.D))
            {
                walkableTile(Direction.East);
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                walkableTile(Direction.South);
            }
            if(Input.GetKeyDown(KeyCode.A))
            {
                walkableTile(Direction.West);
            }
        }
	}

    private void LateUpdate()
    {
        if(runOnce==true)
        {
            runOnce = false;
            gameObject.transform.position = board.GetComponent<BoardCreator>().tileArray[board.GetComponent<BoardCreator>().startingPointX][board.GetComponent<BoardCreator>().startingPointY].transform.position;
        }
    }


    bool walkableTile(Direction direction)
    {
        bool result=true;

        switch (direction)
        {
            case Direction.North:
                if (board.GetComponent<BoardCreator>().tileArray[(int)transform.position.x][(int)transform.position.y + 1].GetComponent<TerrainType>().terrain == TerrainType.terrainType.Floor)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 1);
                    //transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Lerp(0.0f, 1.0f, 10000.0f));
                    //Mathf.Lerp(0, 1, 5.0f);
                }
                else { result = false; }
                break;
            case Direction.East:
                if (board.GetComponent<BoardCreator>().tileArray[(int)transform.position.x+1][(int)transform.position.y ].GetComponent<TerrainType>().terrain == TerrainType.terrainType.Floor)
                {
                    transform.position = new Vector3(transform.position.x + 1, transform.position.y);
                }
                else { result = false; }
                break;
            case Direction.South:
                if (board.GetComponent<BoardCreator>().tileArray[(int)transform.position.x][(int)transform.position.y - 1].GetComponent<TerrainType>().terrain == TerrainType.terrainType.Floor)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - 1);
                }
                else { result = false; }
                break;
            case Direction.West:
                if (board.GetComponent<BoardCreator>().tileArray[(int)transform.position.x - 1][(int)transform.position.y].GetComponent<TerrainType>().terrain == TerrainType.terrainType.Floor)
                {
                    transform.position = new Vector3(transform.position.x - 1, transform.position.y);
                }
                else { result = false; }
                break;
            default:
                break;
        }

        

        return result;
    }
}
