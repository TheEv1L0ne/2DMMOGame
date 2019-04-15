using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONParserClass
{
    public static IEnumerator GetJSONData(string url, System.Action<JSONDataClass> callback)
    {

        Debug.Log("Parsing data from url: " + url);

        WWW www = new WWW(url);

        yield return www;

        if (www.error == null)
        {
            Debug.Log("DATA: " + www.text);
            JSONDataClass myObject = JsonUtility.FromJson<JSONDataClass>(www.text);
            callback(myObject);

            //mapGeneratorScript.GenerateMap(myObject);
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
        }

    }
}
