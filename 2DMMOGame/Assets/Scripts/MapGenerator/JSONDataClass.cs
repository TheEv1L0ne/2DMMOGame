using System;

[Serializable]
public class JSONDataClass
{
    public int map_width;
    public int map_height;
    public int number_of_houses;
    public Tiles[] tiles; 
}

[Serializable]
public class Tiles
{
    public string type;
    public string name;
    public string level;
}