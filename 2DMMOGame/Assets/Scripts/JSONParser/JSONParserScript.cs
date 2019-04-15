using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONParserScript : MonoBehaviour
{

    public string url;
    private string JSONstring;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetJSONData());
    }

    IEnumerator GetJSONData()
    {

        WWW www = new WWW(url);

        yield return www;

        if (www.error == null)
        {
            Debug.Log("DATA: " + www.text);
            JSONDataClass myObject = JsonUtility.FromJson<JSONDataClass>(www.text);
            Debug.Log(myObject.map_height);
            Debug.Log(myObject.map_width);
            Debug.Log(myObject.tiles.Length);
            Debug.Log(myObject.tiles[6].level);
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
        }

    }
}
