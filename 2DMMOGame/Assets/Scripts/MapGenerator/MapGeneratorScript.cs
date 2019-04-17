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

    int[,] mapMatrix;
    int m;
    int n;

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

        m = mapData.map_width;
        n = mapData.map_height;

        m = 15;
        n = 15;

        cameraController.areaSize = m;
        cameraController.SetAreaBounds();

        Debug.Log("M: " + m);
        Debug.Log("N: " + n);

        mapMatrix = new int[m, n];

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                mapMatrix[i, j] = 0;
            }

        }

        int xC = (int)Mathf.Round(Camera.main.gameObject.transform.position.x);
        int yC = (int)Mathf.Round(Camera.main.gameObject.transform.position.y);
        int yLR = (int)(Camera.main.orthographicSize + 1);
        int xUD = (int)(Mathf.Ceil(Camera.main.orthographicSize * Camera.main.aspect) + 1);

        int xim = xC + (m / 2);
        int yim = yC + (m / 2);

        int xS = xim - xUD;
        int xE = xim + xUD;
        int yS = yim - yLR;
        int yE = yim + yLR;

        for (int x = xS; x < xE; x++)
            for (int y = yS; y < yE; y++)
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

    public void makeTiles()
    {
        //m = 512;
        //n = 512;

        int xC = (int)Mathf.Round(Camera.main.gameObject.transform.position.x);
        int yC = (int)Mathf.Round(Camera.main.gameObject.transform.position.y);
        int yLR = (int)(Camera.main.orthographicSize + 1);
        int xUD = (int)(Mathf.Ceil(Camera.main.orthographicSize * Camera.main.aspect) + 1);

        int xim = xC + (m / 2);
        int yim = yC + (m / 2);

        int xS = xim - xUD;
        int xE = xim + xUD;
        int yS = yim - yLR;
        int yE = yim + yLR;

        if (xS < 0)
            xS = 0;
        if (xE > m)
            xE = m;

        if (yS < 0)
            yS = 0;
        if (yE > n)
            yE = n;

        for (int x = xS; x < xE; x++)
            for (int y = yS; y < yE; y++)
            {
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
