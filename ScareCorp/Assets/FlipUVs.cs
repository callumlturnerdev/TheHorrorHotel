﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipUVs : MonoBehaviour {

	void OnValidate () {
         Vector2[] uvs = GetComponent<MeshFilter> ().sharedMesh.uv;
 
         uvs [6] = new Vector2 (0, 0);
         uvs [7] = new Vector2 (1, 0);
         uvs [10] = new Vector2 (0, 1);
         uvs [11] = new Vector2 (1, 1);
 
         GetComponent<MeshFilter> ().sharedMesh.uv = uvs;
     }
}
