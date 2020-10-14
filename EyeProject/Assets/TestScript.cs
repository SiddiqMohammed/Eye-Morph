using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TestScript : MonoBehaviour
{
    
    int row = 5;
    int column = 6;


    float width;
    float height;

    float timer = 5f;
    float shuffle = 20f;
    float videoUpdater = 60f;


    List<GameObject> ri = new List<GameObject>();
    List<GameObject> players = new List<GameObject>();
    List<RenderTexture> textures = new List<RenderTexture>();
    List<string> videoUrl = new List<string>();
    string[] files;

    int[] Imgarray = new int[30];

    private void Start()
    {

        if (!(Display.displays.Length == 1))
        {
            Display.displays[1].Activate();
        }
        arrayGenarator();
        createRI();
        scanClips();

        
    }

    private void Update()
    {
        shuffle -= Time.deltaTime;
        videoUpdater -= Time.deltaTime;
        if (shuffle < 0)
        {

            ShuffleVideo();
            shuffle = 20f;
            
        }

        if (timer<0)
        {
            scanClips();

            timer = 5f;
        }

        if (videoUpdater < 0) 
        {
            updateVideo();

            videoUpdater = 40f;
        }


    }

    void createRI()
    {
        int count = row * column;

        for (int i = 0; i < count; i++)
        {
            GameObject go = new GameObject();
            go.AddComponent<RawImage>();
            go.name = (i+1).ToString();
            ri.Add(go);
        }

        scaleUI();
    }


    void scaleUI()
    {
        Screen.fullScreen = true;
       if(Display.displays.Length==1)
        {
            width = Screen.width / column;
            height = Screen.height / row;
      }
        else
        {
            width = Display.displays[1].systemWidth / column;

            height = Display.displays[1].systemHeight / row;
        } 
       

      
        int x = 0;
        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                ri[x].GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                ri[x].GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

                ri[x].GetComponent<Transform>().SetParent(GameObject.Find("mainPanel").transform);
                ri[x].GetComponent<Transform>().position = new Vector3(width / 2 + i * width, height / 2 + j * height, 0);
                x += 1;
                
            }
        }
    }


    void scanClips()
    {
        
        files = Directory.GetFiles(Application.dataPath + "/Videos", "*.mp4");
        

        if (files.Length > videoUrl.Count)
        {

            StartCoroutine(ClipAdder());

        }
        else
        {
            Array.Clear(files, 0, files.Length);
        }
        
    }

    RenderTexture CreateVideoPlayer(string url)
    {
        GameObject go = new GameObject();
        go.name = (textures.Count +1).ToString();
        RenderTexture rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        rt.name = (textures.Count + 1).ToString();
        rt.Create();
        go.AddComponent<VideoPlayer>();
        go.GetComponent<VideoPlayer>().aspectRatio = VideoAspectRatio.Stretch;
        go.GetComponent<VideoPlayer>().renderMode = VideoRenderMode.RenderTexture;
        go.GetComponent<VideoPlayer>().isLooping = true;
        go.GetComponent<VideoPlayer>().playOnAwake = true;
        go.GetComponent<VideoPlayer>().playbackSpeed = UnityEngine.Random.Range(1.5f, 3f);
        go.GetComponent<VideoPlayer>().url = url;
        go.GetComponent<VideoPlayer>().targetTexture = rt;
        go.transform.SetParent(GameObject.Find("Players").transform);
        players.Add(go);
        return rt;
    }

    void updateVideo()
    {
        int x = textures.Count - 1;
        for(int i=0; i<ri.Count; i++)
        {
            ri[i].GetComponent<RawImage>().texture = textures[x];
            x--;
        }
    }

   
    void ShuffleVideo()
    {
        Shuffler(Imgarray);
        int counter = 0;
        for (int i = textures.Count-1; i >=textures.Count-30; i--)
        {

            ri[Imgarray[counter]].GetComponent<RawImage>().texture = textures[i];
            counter++;
            

        }


    }
/*
    void ImageArray()
    {
        System.Random rand = new System.Random();
        Array.Clear(Imgarray, 0, Imgarray.Length);
        int number;
        
        for (int i = 0; i <5; i++)
        {

            do
            {
                number = rand.Next(0, ri.Count-1);
            } while (Imgarray.Contains(number));
            Imgarray[i] = number;
            

        }

    }

    void TextureArray()
    {
        System.Random rand = new System.Random();
        Array.Clear(TexArray, 0, TexArray.Length);
        int number;
        for (int i = 0; i < 5; i++)
        {

            do
            {
                number = rand.Next(textures.Count - ri.Count, textures.Count-1);
            } while (TexArray.Contains(number));
            TexArray[i] = number;


        }
    }
  
    */


    void Cleanup()
    {
        if (players.Count > ri.Count)
        {
            for(int i=0; i<players.Count-ri.Count; i++)
            {
                Destroy(players[i]);
            }
        }
    }
    void arrayGenarator()
    {

        for (int j = 0; j < 30; j++)
        {
            Imgarray[j] = j;
        }
    }

    void Shuffler(int[] array)
    {
        int length = array.Length;
        System.Random rand = new System.Random();
        for (int i = 0; i < 29; i++)
        {
            swap(array, i, rand.Next(length - i));

        }

    }
    void swap(int[] array, int a, int b)
    {
        int temp = array[a];
        array[a] = array[b];
        array[b] = temp;
    }

    IEnumerator ClipAdder()
    {
        yield return new WaitForSeconds(2);
        for (int i = videoUrl.Count; i < videoUrl.Count + (files.Length - videoUrl.Count); i++)
        {
            videoUrl.Add(files[i]);


            textures.Add(CreateVideoPlayer(files[i]));

        }
        updateVideo();
        Cleanup();

    }
}
