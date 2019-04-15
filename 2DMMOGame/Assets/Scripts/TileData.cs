using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{

    private string type;
    private string name;
    private string level;

    public string Level { get => level; set => level = value; }
    public string Name { get => name; set => name = value; }
    public string Type { get => type; set => type = value; }
}
