using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class test : MonoBehaviour
{
	public GameObject[] objects;
	public RenderTexture[] textures;
	public VideoClip[] clips;
	float changer = 5f;
	/*string[] files;*/
	List<string> files = new List<string>();
	int clipMonitor = 0;
	int oldCount;

	private void Start()
	{
		scanClips();

		oldCount = files.Count;
		/*print(oldCount);*/

		if (files.Count != 0)
		{
			updateVideo();
		}
	}

	void updateVideo()
	{
		print(objects.Length);
		print(files.Count);


		files.Reverse();

		if (files.Any())
		{
			if (files.Count > objects.Length)
			{
				files.RemoveAt(files.Count - 1);
			}
		}

		for (int i = 0; i < files.Count; i++)
		{
			objects[i].GetComponent<VideoPlayer>().url = files[i];
		}

	}

	void OnApplicationQuit()
	{
		for (int i = 0; i < files.Count; i++)
		{
			textures[i].Release();
		}
	}

	private void Update()
	{
		changer -= Time.deltaTime;
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
	}

	void scanClips()
	{
		files.Clear();
		foreach (string file in Directory.GetFiles(Application.dataPath + "/Videos", "*.mp4"))
		{
			files.Add(file);
		}

	}
	void assignVideos()
	{
		for (int i = 0; i < textures.Length; i++)
		{
			objects[i].GetComponent<VideoPlayer>().clip = clips[i];
			objects[i].GetComponent<VideoPlayer>().targetTexture = textures[i];
		}
	}
}
