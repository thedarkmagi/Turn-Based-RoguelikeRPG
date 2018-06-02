using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    // the type of tile that will be laid in a specific position.
    public enum TileType
    {
        Floor, Wall,
    }

    public int columns = 100;
    public int rows = 100;

    public IntRange numRooms = new IntRange(15, 20);
    public IntRange roomWitdh = new IntRange(3, 10);
    public IntRange roomHeight = new IntRange(3, 10);
    public IntRange corridorLength = new IntRange(6, 10);

    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] outerWallTiles;

    private TileType[][] tiles;
    private Room[] rooms;
    private Corridor[] corridors;
    private GameObject boardHolder;

    public GameObject[][] tileArray;
    public bool[][] spaceOccupied; 
    public int startingPointX, startingPointY;
    bool firstTimeOnly;
    public GameObject chest;

    // Use this for initialization
    void Start ()
    {
        firstTimeOnly = true;
        boardHolder = new GameObject("BoardHolder");

        // incert run the functions
        SetupTilesArray();
        CreateRoomsAndCorridors();

        SetTileValuesForRooms();
        SetTilesValueForCorridors();

        InstantiateTiles();

        spawnChests(5);
        // not needed as it simply adds a boarder 
        //InstantiateOuterWalls();
    }
	
    void SetupTilesArray()
    {
        tiles = new TileType[columns][];
        tileArray = new GameObject[columns][];
        spaceOccupied = new bool[columns][];
        for(int i=0; i<tiles.Length;i++)
        {
            tiles[i] = new TileType[rows];
            tileArray[i] = new GameObject[rows];
            spaceOccupied[i] = new bool[rows];
        }


        //populated the full array as walls so that rooms can be carved out later.
        for(int j=0;j<tiles.Length;j++)
        {
            for(int k=0;k<tiles[j].Length;k++)
            {
                tiles[j][k] = TileType.Wall;
                spaceOccupied[j][k] = true;
            }
        }
    }

    void CreateRoomsAndCorridors()
    {
        rooms = new Room[numRooms.Random];

        // so there isn't a corridor going into nowhere
        corridors = new Corridor[rooms.Length - 1];

        rooms[0] = new Room();
        corridors[0] = new Corridor();

        rooms[0].SetupRoom(roomWitdh,roomHeight,columns,rows);

        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWitdh, roomHeight, columns, rows, true);

        for(int i = 1; i< rooms.Length;i++)
        {
            rooms[i] = new Room();

            rooms[i].SetupRoom(roomWitdh, roomHeight, columns, rows, corridors[i - 1]);

            if(i<corridors.Length)
            {
                corridors[i] = new Corridor();

                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWitdh, roomHeight, columns, rows, false);
            }
        }

    }


    void SetTileValuesForRooms()
    {
        for( int i = 0; i < rooms.Length;i++)
        {
            Room currentRoom = rooms[i];

            for ( int j=0;j<currentRoom.roomWidth;j++)
            {
                int xCoord = currentRoom.xPos + j;

                for ( int k =0; k<currentRoom.roomHeight;k++)
                {
                    int yCoord = currentRoom.yPos + k;

                    tiles[xCoord][yCoord] = TileType.Floor;
                    spaceOccupied[xCoord][yCoord] = false;

                    if(firstTimeOnly==true)
                    {
                        startingPointX = xCoord;
                        startingPointY = yCoord;
                        spaceOccupied[xCoord][yCoord] = true; // so that the players spawn can't have other stuff placed on it 
                        firstTimeOnly = false;
                    }
                }
            }
        }
    }

    void SetTilesValueForCorridors()
    {
        for ( int i =0; i<corridors.Length;i++)
        {
            Corridor currentCorridor = corridors[i];

            for(int j=0;j<currentCorridor.corridorLength;j++)
            {
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                switch(currentCorridor.direction)
                {
                    case Direction.North:
                        yCoord += j;
                        break;
                    case Direction.East:
                        xCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        break;
                    case Direction.West:
                        xCoord -= j;
                        break;
                }

                tiles[xCoord][yCoord] = TileType.Floor;
            }
        }
    }

    void InstantiateTiles()
    {
        for(int i=0;i<tiles.Length;i++)
        {
            for(int j=0;j<tiles[i].Length;j++)
            {
                // if floor make floor? 
                //if (tiles[i][j] == TileType.Floor)
                //{
                    InstantiateFromArray(floorTiles, i, j);
               // }
                if(tiles[i][j]==TileType.Wall)
                {
                    InstantiateFromArray(wallTiles, i, j);
                }
            }
        }
    }


    void InstantiateOuterWalls()
    {
        // The outer walls are one unit left, right, up and down from the board.
        float leftEdgeX = -1f;
        float rightEdgeX = columns + 0f;
        float bottomEdgeY = -1f;
        float topEdgeY = rows + 0f;

        // Instantiate both vertical walls (one on each side).
        InstantiateVerticalOuterWall(leftEdgeX, bottomEdgeY, topEdgeY);
        InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

        // Instantiate both horizontal walls, these are one in left and right from the outer walls.
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
    }


    void InstantiateVerticalOuterWall(float xCoord, float startingY, float endingY)
    {
        // Start the loop at the starting value for Y.
        float currentY = startingY;

        // While the value for Y is less than the end value...
        while (currentY <= endingY)
        {
            // ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
            InstantiateFromArray(outerWallTiles, xCoord, currentY);

            currentY++;
        }
    }


    void InstantiateHorizontalOuterWall(float startingX, float endingX, float yCoord)
    {
        // Start the loop at the starting value for X.
        float currentX = startingX;

        // While the value for X is less than the end value...
        while (currentX <= endingX)
        {
            // ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
            InstantiateFromArray(outerWallTiles, currentX, yCoord);

            currentX++;
        }
    }

    void InstantiateFromArray (GameObject[] prefabs, float xCoord, float yCoord)
    {
        int randomIndex = Random.Range(0, prefabs.Length);

        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;
        tileArray[(int)xCoord][(int)yCoord] = tileInstance;

        //int tileType = (int)
       tileInstance.AddComponent<TerrainType>().terrain = (TerrainType.terrainType)tiles[(int)xCoord][(int)yCoord];//tileType;

        // this will probably cause errors?
        tileInstance.transform.parent = boardHolder.transform;
    }

    void spawnChests(int numberOfChests)
    {
        // pick a floor tile considered empty 
        while(numberOfChests>0)
        {
            bool validPlacement=false;
            while (validPlacement != true)
            {
                int x, y;
                x = Random.Range(0, tileArray.Length);
                y = Random.Range(0, tileArray[0].Length);
                if (spaceOccupied[x][y] == false)
                {
                    bool[] validDirections = { false, false, false, false };
                    bool result = false;
                    for (int i = 0; i < 4; i++)
                    {
                        // check direction tiles
                        switch ((Direction)i)
                        {
                            case Direction.North:
                                if (spaceOccupied[x][y+1] == false)
                                {
                                    if(spaceOccupied[x+1][y+1]==false && spaceOccupied[x-1][y+1]==false)
                                    {
                                        validDirections[i] = true;
                                    }
                                }

                                break;
                            case Direction.East:
                                if (spaceOccupied[x+1][y ] == false)
                                {
                                    if (spaceOccupied[x + 1][y + 1] == false && spaceOccupied[x + 1][y - 1] == false)
                                    {
                                        validDirections[i] = true;
                                    }
                                }

                                break;
                            case Direction.South:
                                if (spaceOccupied[x][y - 1] == false)
                                {
                                    if (spaceOccupied[x + 1][y - 1] == false && spaceOccupied[x - 1][y - 1] == false)
                                    {
                                        validDirections[i] = true;
                                    }
                                }

                                break;
                            case Direction.West:
                                if (spaceOccupied[x - 1][y] == false)
                                {
                                    if (spaceOccupied[x - 1][y + 1] == false && spaceOccupied[x - 1][y - 1] == false)
                                    {
                                        validDirections[i] = true;
                                    }
                                }

                                break;
                            default:
                                break;
                        }
                    }

                    for(int i=0; i<validDirections.Length;i++)
                    {
                        if(validDirections[i] == false)
                        {
                            result = false;
                            break;
                        }
                        else
                        {
                            result = true;
                        }
                        
                    }
                    if(result == true)
                    {
                        // Create THE CHEST! 
                        GameObject chestInstance = Instantiate(chest, new Vector3(x,y), Quaternion.identity) as GameObject;
                        validPlacement = true;
                    }
                }

            }


            numberOfChests--;  // iterate 
        }
        // check surrounding tiles if a center tile is empty and both on either side are not. move left or right one and check again. 
    }


    // Update is called once per frame
    void Update ()
    {
		
	}
}
