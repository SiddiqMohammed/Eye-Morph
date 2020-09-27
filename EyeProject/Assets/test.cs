using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;


public class test : MonoBehaviour
{
	List<GameObject> objects= new List<GameObject>();
	List<GameObject> videoPlayers = new List<GameObject>();
	List<RenderTexture> Textures = new List<RenderTexture>();
	RenderTexture rt;
	public GameObject parent;
	// public RenderTexture[] textures;
	public VideoClip[] clips;
	float changer = 5f;
	float shuffle = 20f;
	/*string[] files;*/
	List<string> files = new List<string>();
	int clipMonitor = 0;
	int oldCount;

	float width;
    float height;
    public int row = 2;
    public int column = 3;

	private void Start()
	{
        createImages();
		ResponsiveScreen();
		scanClips();
		createVideoPlayer();

		
		// Textures.Reverse();
		for(int i=0; i < objects.Count; i++)
        {
			objects[i].GetComponent<RawImage>().texture = Textures[(int)Math.Floor((Decimal)i/2)];
        }

		oldCount = files.Count;


	}

	void updateVideo()
	{


		// Textures.Reverse();

		// updates the first video
		// for(int i=0; i < videoPlayers.Count; i++)
        // {
		// 	videoPlayers[i].GetComponent<VideoPlayer>().url = files[files.Count - 1 -i];
        // }


		// randomizes the videos except the first video
		videoPlayers[0].GetComponent<VideoPlayer>().url = files[files.Count - 1];

		for(int i=0; i < videoPlayers.Count; i++)
        {
			// objects[i].GetComponent<RawImage>().texture = Textures[UnityEngine.Random.Range(0, files.Count)];
			int x = UnityEngine.Random.Range(1, videoPlayers.Count);
			int y = UnityEngine.Random.Range(0, files.Count);
			// print(x);
			// print(y);
			videoPlayers[x].GetComponent<VideoPlayer>().url = files[y];
        }



		print("updated");


	}

   void createImages()
    {
		int x = row * column;

		for (int i = 0; i < x; i++)
		{
			GameObject go = new GameObject();

			go.AddComponent<RawImage>();

			go.name = objects.Count.ToString();
			objects.Add(go);
		}
    }

	void ResponsiveScreen()
    {
        width = Screen.width / column;
        height = Screen.height / row;
		int count = 0;
        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {


                objects[count].GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                objects[count].GetComponent<RectTransform>().pivot = new Vector2(0, 0);
                objects[count].GetComponent<Transform>().position = new Vector3(i * width, j * height, 0);
                objects[count].transform.parent = parent.transform;

				count += 1;

			}
		}
	}

	void createVideoPlayer()
    {
        for(int i=0; i<3; i++)
        
        {
            GameObject vp = new GameObject();
            vp.transform.parent = GameObject.Find("Player").transform;
            vp.AddComponent<VideoPlayer>();
			rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
			rt.Create();
			rt.name = Textures.Count.ToString();
			
			
            vp.GetComponent<VideoPlayer>().playOnAwake = true;
            vp.GetComponent<VideoPlayer>().isLooping = true;
            vp.GetComponent<VideoPlayer>().renderMode = VideoRenderMode.RenderTexture;
            vp.GetComponent<VideoPlayer>().aspectRatio = VideoAspectRatio.Stretch;
            vp.GetComponent<VideoPlayer>().playbackSpeed = 0.5f;
            vp.GetComponent<VideoPlayer>().targetTexture = rt;
            vp.GetComponent<VideoPlayer>().name = videoPlayers.Count.ToString();
            vp.GetComponent<VideoPlayer>().url = files[i];

			Textures.Add(rt);
			videoPlayers.Add(vp);

        }
        
    }

	void OnApplicationQuit()
	{
		// for (int i = 0; i < files.Count; i++)
		// {
		// 	Textures[i].Release();
		// }
		print("QUIT");
	}

	private void Update()
	{
		changer -= Time.deltaTime;
		shuffle -= Time.deltaTime;
		if (changer < 0)
		{
			
			scanClips();

			clipMonitor = files.Count - oldCount;

			if (clipMonitor > 0)
			{
				updateVideo();

				oldCount = files.Count;
			}
			changer = 5f;
		}

        if (shuffle < 0)
        {
			shuffleVideo();
			shuffle = 20f;
		}

		
	}

	void scanClips()
	{
		files.Clear();
		foreach (string file in Directory.GetFiles(Application.dataPath + "/Videos", "*.mp4"))
		{
			files.Add(file);
		}

	}


	void shuffleVideo()
    {
		for(int i=1; i<objects.Count; i++)
        {
			int x = UnityEngine.Random.Range(0, Textures.Count);
			objects[i].GetComponent<RawImage>().texture = Textures[x];
			
		}
		
		
    }
}
