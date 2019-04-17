using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControllerScript : MonoBehaviour
{
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
        tiles[id].transform.GetChild(0).gameObject.SetActive(true);
        tiles[id].transform.GetChild(0).GetChild(0).GetComponent<TextMesh>().text = name; //SET NAME AND LVL
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

            isDragging = false;
        }
    }

}
