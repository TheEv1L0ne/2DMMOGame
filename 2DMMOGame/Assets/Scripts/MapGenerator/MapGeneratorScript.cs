using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Genrates map tiles

public class MapGeneratorScript : MonoBehaviour
{
    public string url;

    public GameObject mapTilesParent;
    public GameObject mapTilePrefab;

    public Sprite[] mapTilesGraphics;

    public Text numberOfHousesText;

    public MapControllerScript mapControllerScript;
    public CameraController cameraController;

    int[,] mapMatrix; //map matrix for generating tiles.
    int m;
    int n;

    int numberOfHousesOnMap;

    // Start is called before the first frame update
    void Start()
    {

        GetData();
        //StartCoroutine(GenerateMapUnderCamera());
    }

    void GetData()
    {
        Debug.Log("Getting data...");
        //Retruns data from given URL
        StartCoroutine(JSONParserClass.GetJSONData(url, (jSONDataClass) =>
        {
            //gets data from callback
            GenerateMap(jSONDataClass);
        }));
    }

    JSONDataClass mapData;

    public void GenerateMap(JSONDataClass mapDataParsed)
    {
        mapData = mapDataParsed;
        //Sets the number of houses on map
        numberOfHousesText.text = "Number of houses: " + mapData.number_of_houses;
        mapControllerScript.SetNoOfHauses(mapData.number_of_houses);
        numberOfHousesOnMap = mapData.number_of_houses;

        m = mapData.map_width;
        n = mapData.map_height;

        //m = 25; //small test data
        //n = 25; 

        cameraController.areaSize = m;
        cameraController.SetAreaBounds();

        Debug.Log("M: " + m);
        Debug.Log("N: " + n);

        //makes empty map matrix
        mapMatrix = new int[m, n];

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                mapMatrix[i, j] = 0;
            }

        }

        makeTiles();
    }

    /*
     * Makes tiles based on camera location
     * first it gets camera current location and calulates size of area it should look inside matrix
     * then it checks matix in that area
     * if element of matix is 0 it means there is space to insert tile
     * 
     * tiles size is always 1 unity meter
     */ 
    public void makeTiles()
    {
        //m = 512;
        //n = 512;

        int xC = (int)Mathf.Round(Camera.main.gameObject.transform.position.x); //current X coordinate  of camera
        int yC = (int)Mathf.Round(Camera.main.gameObject.transform.position.y); //current Y coordinate of camera
        int yLR = (int)(Camera.main.orthographicSize + 1); //leght from center left and right
        int xUD = (int)(Mathf.Ceil(Camera.main.orthographicSize * Camera.main.aspect) + 1); //lenght from center up and down

        int xim = xC + (m / 2); //moves camera coordinates from unity coordinate to matrix coordinates
        int yim = yC + (m / 2); //

        //this are for loop bounds 
        int xS = xim - xUD; 
        int xE = xim + xUD;
        int yS = yim - yLR;
        int yE = yim + yLR;

        //checks if bounds are out of matrix bounds 
        if (xS < 0)
            xS = 0;
        if (xE > m)
            xE = m;

        if (yS < 0)
            yS = 0;
        if (yE > n)
            yE = n;

        //makes new tiles
        for (int x = xS; x < xE; x++)
            for (int y = yS; y < yE; y++)
            {
                //cheks if there is space to make new one
                if (mapMatrix[x, y] == 0)
                {
                    Vector3 tilePosition = new Vector3(x - (m / 2), y - (n / 2), 0);
                    GameObject mapTile = Instantiate(mapTilePrefab, tilePosition, Quaternion.identity);
                    mapTile.transform.SetParent(mapTilesParent.transform);


                    int chosenTile = Random.Range(0, 7);
                    string s = mapData.tiles[chosenTile].type;

                    switch (s)
                    {
                        case "sand":
                            MakeTile(0, mapTile, mapData.tiles[chosenTile]);
                            break;
                        case "water":
                            MakeTile(1, mapTile, mapData.tiles[chosenTile]);
                            break;
                        case "grass":
                            MakeTile(2, mapTile, mapData.tiles[chosenTile]);
                            break;
                        case "trees1":
                            MakeTile(3, mapTile, mapData.tiles[chosenTile]);
                            break;
                        case "trees2":
                            MakeTile(4, mapTile, mapData.tiles[chosenTile]);
                            break;
                        case "house1":
                            MakeTile(5, mapTile, mapData.tiles[chosenTile]);
                            break;
                        case "house2":
                            MakeTile(6, mapTile, mapData.tiles[chosenTile]);
                            break;
                    }

                    mapMatrix[x, y] = 1;
                }
            }
    }

    //sets data for specific tile
    void MakeTile(int graphicsID, GameObject mapTile, Tiles parsedTilesData)
    {
        mapTile.GetComponent<SpriteRenderer>().sprite = mapTilesGraphics[graphicsID];
        TileData tileData = mapTile.GetComponent<TileData>();
        tileData.Name = parsedTilesData.name;
        tileData.Level = parsedTilesData.level;
        tileData.Type = parsedTilesData.type;

        mapControllerScript.AddTileToList(mapTile);

    }
}
