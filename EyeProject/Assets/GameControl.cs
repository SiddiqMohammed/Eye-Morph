using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameControl : MonoBehaviour
{
    List<GameObject> tiles = new List<GameObject>();
    List<RenderTexture> textures = new List<RenderTexture>();
   
    public GameObject parent;
    private float height;
    private float width;


    void Start()
    {
        height = Screen.height / 2;
        width = Screen.width / 3;

        CreateRawimage();
        
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(tiles.Count);
            ChangeVideo();
        }
    }

    void CreateRawimage()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {

                GameObject image = new GameObject();
                image.transform.parent = parent.transform;
                image.name = tiles.Count.ToString();
                image.transform.parent = parent.transform;
                image.AddComponent<RawImage>();
                image.AddComponent<VideoPlayer>();

                image.GetComponent<RawImage>().texture = CreateRendertexture(tiles.Count.ToString());
                image.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                image.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
                image.transform.position = new Vector2(i * (width), j * height);
                CreateRendertexture(tiles.Count.ToString());
                CreateVideoPlayer(image);
                
                tiles.Add(image);
            }

        }
    }

     RenderTexture CreateRendertexture(string name)
    {

        RenderTexture rend = new RenderTexture((int)width, (int)height, 24, RenderTextureFormat.ARGB32);
        rend.name = name;
        rend.Create();
        textures.Add(rend);
        return rend;
    }


    void CreateVideoPlayer(GameObject go)
    {
        go.GetComponent<VideoPlayer>().aspectRatio = VideoAspectRatio.Stretch;
        go.GetComponent<VideoPlayer>().playOnAwake = true;
        go.GetComponent<VideoPlayer>().isLooping = true;
        go.GetComponent<VideoPlayer>().renderMode = VideoRenderMode.RenderTexture;
        go.GetComponent<VideoPlayer>().targetTexture = textures[textures.Count -1];
        
    }


   void ChangeVideo()
    {
        if (tiles.Count > 0)
        {
          
                if (tiles[0].GetComponent<VideoPlayer>().isPlaying)
                {
                    tiles[0].GetComponent<VideoPlayer>().Stop();
                }
            tiles[0].GetComponent<VideoPlayer>().url = Application.dataPath + "/Videos/0.avi";
            tiles[0].GetComponent<VideoPlayer>().Play();


        }
   }
    

}
