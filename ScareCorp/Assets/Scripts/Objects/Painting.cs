using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Painting : MonoBehaviour {

    [SerializeField]
    private List<Sprite> possibleImages;
    private GameObject paintingRef;
    private int randNum;
 

    void Awake() {
        paintingRef = transform.GetChild(0).gameObject;
        if (!Directory.Exists(Application.dataPath + "/Images/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Images/");
        }
        var files = new DirectoryInfo((Application.dataPath + "/Images/"));
        var fileInfo = files.GetFiles("*.jpg");
        
        foreach (var file in fileInfo)
        {
            possibleImages.Add(LoadNextImage(Application.dataPath + "/Images/" + file.Name));
        }
        randNum = Random.Range(0, fileInfo.Length);
        paintingRef.GetComponent<SpriteRenderer>().sprite = possibleImages[randNum];
    }


    private Sprite LoadNextImage(string filePath, float pixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
    {
        Sprite NewSprite = new Sprite();
        Texture2D SpriteTexture = LoadTexture(filePath);
        NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), pixelsPerUnit, 0, spriteType);
        return NewSprite;
    }

    public Texture2D LoadTexture(string filepath)
    {
        Texture2D Tex2D;
        byte[] FileData;
        if (File.Exists(filepath))
        {
            FileData = File.ReadAllBytes(filepath);
            Tex2D = new Texture2D(2, 2);
            if (Tex2D.LoadImage(FileData))
                return Tex2D; 
        }
        return null;
    }
}
