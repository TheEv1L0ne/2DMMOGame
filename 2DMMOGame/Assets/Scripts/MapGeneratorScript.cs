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

    // Start is called before the first frame update
    void Start()
    {
        GetData();
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

    public void GenerateMap(JSONDataClass mapData)
    {
        //Sets the number of houses on map
        numberOfHousesText.text = "Number of houses: " + mapData.number_of_houses;

        int size = 20;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Vector3 tilePosition = new Vector3(i - (size / 2), j - (size / 2), 0);
                GameObject mapTile = Instantiate(mapTilePrefab, tilePosition, Quaternion.identity);                         
                mapTile.transform.SetParent(mapTilesParent.transform);

                int chosenTile = Random.Range(0, 7);
                string s = mapData.tiles[chosenTile].type;

                switch(s)
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

    }
}
