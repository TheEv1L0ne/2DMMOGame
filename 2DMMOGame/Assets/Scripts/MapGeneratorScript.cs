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


                string s = mapData.tiles[Random.Range(0, 7)].type;

                switch(s)
                {
                    case "grass":
                        mapTile.GetComponent<SpriteRenderer>().sprite = mapTilesGraphics[2];
                        break;
                    case "sand":
                        mapTile.GetComponent<SpriteRenderer>().sprite = mapTilesGraphics[0];
                        break;
                    case "water":
                        mapTile.GetComponent<SpriteRenderer>().sprite = mapTilesGraphics[1];
                        break;
                    case "trees1":
                        mapTile.GetComponent<SpriteRenderer>().sprite = mapTilesGraphics[3];
                        break;
                    case "trees2":
                        mapTile.GetComponent<SpriteRenderer>().sprite = mapTilesGraphics[4];
                        break;
                    case "house1":
                        mapTile.GetComponent<SpriteRenderer>().sprite = mapTilesGraphics[5];
                        break;
                    case "house2":
                        mapTile.GetComponent<SpriteRenderer>().sprite = mapTilesGraphics[6];
                        break;
                }

                //mapTile.GetComponent<SpriteRenderer>().sprite = mapTilesGraphics[Random.Range(0, 7)];

                TileData tileData = mapTile.GetComponent<TileData>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
