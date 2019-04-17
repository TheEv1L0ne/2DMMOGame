using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControllerScript : MonoBehaviour
{

    public Sprite grass;

    List<GameObject> tiles;

    int lastOpenedTile = -1;

    private void Start()
    {
        tiles = new List<GameObject>();
    }


    public void AddTileToList(GameObject tile)
    {
        this.tiles.Add(tile);
        tile.GetComponent<TileData>().TileID = tiles.Count - 1;
    }


    void OpenTile(int id)
    {
        TileData data = tiles[id].GetComponent<TileData>();

        tiles[id].transform.GetChild(0).gameObject.SetActive(true);
        tiles[id].transform.GetChild(0).GetChild(0).GetComponent<TextMesh>().text = data.Name;
        if (data.Type == "house1" || data.Type == "house2")
        {
            tiles[id].transform.GetChild(0).GetChild(0).GetComponent<TextMesh>().text = data.Name;
            tiles[id].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            tiles[id].transform.GetChild(0).GetChild(1).GetComponent<TextMesh>().text = "Level " + data.Level;
            tiles[id].transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            tiles[id].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            tiles[id].transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        }
    }

    void CloseTile(int id)
    {
        tiles[id].transform.GetChild(0).gameObject.SetActive(false);
    }

    bool isDragging = false;
    Vector3 currentClickPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentClickPosition = Input.mousePosition;
        }

        if(Input.GetMouseButton(0))
        {
            if (Input.mousePosition != currentClickPosition)
            {
                isDragging = true;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);

            //Debug.Log("TAG: " + hit.collider.tag);

            if (hit.collider.tag == "Tile" && !isDragging)
            {
                if (lastOpenedTile == -1) //nothing is open
                {
                    OpenTile(hit.collider.gameObject.GetComponent<TileData>().TileID);
                    lastOpenedTile = hit.collider.gameObject.GetComponent<TileData>().TileID;
                }
                else if(lastOpenedTile == hit.collider.gameObject.GetComponent<TileData>().TileID) //click on open tile
                {
                    CloseTile(hit.collider.gameObject.GetComponent<TileData>().TileID);
                    lastOpenedTile = -1;
                }
                else //closing last opened tile and opening new
                {
                    CloseTile(lastOpenedTile);
                    OpenTile(hit.collider.gameObject.GetComponent<TileData>().TileID);
                    lastOpenedTile = hit.collider.gameObject.GetComponent<TileData>().TileID;
                }
            }

            if(hit.collider.tag == "Button")
            {
                //Debug.Log("BUTTON");
                GameObject buttonParent = hit.collider.transform.parent.parent.gameObject;
                TileData data = buttonParent.GetComponent<TileData>();
                //Debug.Log(data.Type);
                if(data.Type == "house1" || data.Type == "house2") //it is house and it needs to be demolished!
                {

                    buttonParent.GetComponent<SpriteRenderer>().sprite = grass;
                    data.Type = "grass";
                    data.Level = null;
                    data.Name = "Empty Tile";
                    CloseTile(data.TileID);
                    lastOpenedTile = -1;
                    //finsih data
                }
            }

            isDragging = false;
        }
    }

}
