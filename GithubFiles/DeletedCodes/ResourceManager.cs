using System.Collections;
using System.IO;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    private WWW www;

    // Use this for initialization
    void Start ()
    {
        loadFilestoPresists();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void Example()
    {
        print(Application.persistentDataPath);
    }


    void Copy(string sourceDir, string targetDir)
    {
        if (!Directory.Exists(targetDir))
            Directory.CreateDirectory(targetDir);
        foreach (var file in Directory.GetFiles(sourceDir))
            if (!File.Exists(Path.Combine(targetDir, Path.GetFileName(file))))
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));
        foreach (var directory in Directory.GetDirectories(sourceDir))
            Copy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
    }

    void loadFilestoPresists()
    {
        //persistentDataPath:  C:/Users/anarimani/AppData/LocalLow/DefaultCompany/Infinite Terrain/Phoenix

        string character = "Phoenix";
        string targetDir = Application.persistentDataPath + "/" + character;
        string sourceDir = @"D:\Avid\Unity\Tempfiles";

        print(targetDir + " ### " + sourceDir);
        Copy(sourceDir, targetDir);
    }


    void LoadAnimation()
    {
        string url = "file:///" + Application.persistentDataPath + "/" + "Phoenix/PhoenixRightWalk.controller";
        //string url = "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg";
        print(url);
        print(File.Exists(url).ToString());
        // Start a download of the given URL
        var www = new WWW(url);
        // Wait for download to complete
        //yield return www;
        while (!www.isDone)
        {
            print("*");
        }

        if (string.IsNullOrEmpty(www.error) && www.bytes != null) print("Errorr");
        //load file
        print(www.bytesDownloaded);
        Debug.Log(www.texture.name);
        Debug.Log(www.bytes.Length);
        byte[] bytes = www.bytes;
        //print(bytes);
        www.Dispose();
        www = null;

    }

    //TODO
    //later for cashing
    //https://stackoverflow.com/questions/35695154/load-sprites-from-persistent-data-path

    //https://answers.unity.com/questions/714809/how-to-load-files-from-persistentdatapath-to-memor.html
    //https://answers.unity.com/questions/1359866/dlc-help-downloading-assets-and-saving-locally.html
    //I couldn't get an image into the resources so I figured another way around it.
    //First thing I do is check if the image exists in the Resources folder, if that doesn't exist i then use File.exists to check if the image is in the persistantDataPath subdirectory, if not then use a WWW to download it from my server, and then File.WriteAllBytes to write it to the persistantDataPath.
    //So from now on, when the game loads it will look in the persistantDataPath and it will load it without checking my server.
    //It works for what I need and after testing multiple times, the game loads at similar times with the image in the resource folder compared to downloading it from the server.
    void LocateSprite()
    {
        
        //    Sprite requestedSprite = new Sprite();
        //    //requestedSprite = Resources.Load<Sprite>("Sprites/" + Path.GetFileNameWithoutExtension(jn["IMG"]));
        //    // Load all sprites in atlas
        //    string characterPath = "Characters/phoenix";
        //    Sprite[] abilityIconsAtlas = Resources.LoadAll<Sprite>(characterPath);
        //    // Get specific sprite
        //    requestedSprite = abilityIconsAtlas.Single(s => s.name == "right_3");
        //    if (requestedSprite == null)
        //    {
        //        if (!File.Exists(Application.persistentDataPath + "/Sprites/phoenix" ))
        //        {
        //            string imageURL = "http://localhost/game/Sprites/";
        //            www = new WWW(imageURL );
        //            byte[] bytes = www.texture.EncodeToPNG();
        //            File.WriteAllBytes(Application.persistentDataPath + "/Sprites/phoenix", bytes);
        //            Debug.Log("Downloaded Sprite");
        //        }
        //        byte[] data = File.ReadAllBytes(Application.persistentDataPath + "/Sprites/phoenix" );
        //        Texture2D tex = new Texture2D(450, 500, TextureFormat.ARGB32, false);
        //        tex.LoadImage(data);
        //        Sprite s = Sprite.Create(tex, new Rect(0, 0, 450, 500), new Vector2(0, 0));
        //        requestedSprite = s;
        //    }
        //    yield return requestedSprite;
        }




    }
