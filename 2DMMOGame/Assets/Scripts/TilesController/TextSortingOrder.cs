﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSortingOrder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var parent = transform.parent;

        var parentRenderer = parent.GetComponent<Renderer>();
        var renderer = GetComponent<Renderer>();
        renderer.sortingLayerID = parentRenderer.sortingLayerID;
        renderer.sortingOrder = parentRenderer.sortingOrder + 1;

        //var spriteTransform = parent.transform;
        //var text = GetComponent<TextMesh>();
        //var pos = spriteTransform.position;
        //text.text = string.Format("{0}, {1}", pos.x, pos.y);
    }
}
