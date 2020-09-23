using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    string[] files;
    int clipMonitor = 0;
    int oldCount;

    private void Start()
    {

        scanClips();
        oldCount = files.Length;
        for(int i=0; i<objects.Length; i++)
        {
            objects[i].GetComponent<VideoPlayer>().url = files[i];
        }
        
        
    }

    

    private void Update()
    {
        changer -= Time.deltaTime;
        if (changer < 0)
        {
            scanClips();
            clipMonitor = files.Length - oldCount;
            if (clipMonitor > 0)
            {
                for (int i = 0; i < clipMonitor-1; i++)
                {
                    objects[i].GetComponent<VideoPlayer>().url = files[oldCount+i];
                }

                oldCount = files.Length;
            }
            changer = 5f;
        }
    }

    void scanClips()
    {
        files = Directory.GetFiles(Application.dataPath + "/Videos", "*.mp4");
        
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
