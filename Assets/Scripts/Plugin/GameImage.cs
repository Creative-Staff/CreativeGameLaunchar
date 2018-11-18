using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

/// Copyright (c) 2016 @consolesoup
/// https://qiita.com/consolesoup/items/5faf1b48c8db2e08f393

public class GameImage : MonoBehaviour {

    public static GameImage Instance;

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }

    /// <summary>
    /// ファイルパスからTexture2Dを読み込む
    /// </summary>
    public Texture2D Texture2DFromFile(string path)
    {
        Texture2D texture = null;
        if (File.Exists(path))
        {
            //byte取得
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader bin = new BinaryReader(fileStream);
            byte[] readBinary = bin.ReadBytes((int)bin.BaseStream.Length);
            bin.Close();
            fileStream.Dispose();
            fileStream = null;
            if (readBinary != null)
            {
                //横サイズ
                int pos = 16;
                int width = 0;
                for (int i = 0; i < 4; i++)
                {
                    width = width * 256 + readBinary[pos++];
                }
                //縦サイズ
                int height = 0;
                for (int i = 0; i < 4; i++)
                {
                    height = height * 256 + readBinary[pos++];
                }
                //byteからTexture2D作成
                texture = new Texture2D(width, height);
                texture.LoadImage(readBinary);
            }
            readBinary = null;
        }
        return texture;
    }

    /// <summary>
    /// Texture2DからSpriteへ変換
    /// </summary>
    public Sprite SpriteFromTexture2D(Texture2D texture)
    {
        Sprite sprite = null;
        if (texture)
        {
            //Texture2DからSprite作成
            sprite = Sprite.Create(texture, new UnityEngine.Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
        return sprite;
    }

    /// <summary>
    /// ファイルからSpriteへの変換
    /// </summary>
    public Sprite SpriteFromFile(string path)
    {
        Sprite sprite = null;
        Texture2D texture = Texture2DFromFile(path);
        if (texture)
        {
            //Texture2DからSprite作成
            sprite = SpriteFromTexture2D(texture);
        }
        texture = null;
        return sprite;
    }

    /// <summary>
    /// フォルダパスから画像一覧を取得出来る関数
    /// </summary>
    public List<Sprite> SpriteListFromFolder(string path)
    {
        List<Sprite> images = null;
        if (images == null)
        {
            //新規読み込み
            images = new List<Sprite>();
            if (Directory.Exists(path))
            {
                //フォルダ内のpngファイルのファイル名取得
                DirectoryInfo dir = new DirectoryInfo(path);
                FileInfo[] info = dir.GetFiles("*.png");
                foreach (FileInfo file in info)
                {
                    string filePath = path + file.Name;
                    //ファイルパスからpng読み込み
                    Sprite sprite = SpriteFromFile(filePath);
                    if (sprite && !file.Name.Equals("bannar.png") && !file.Name.Equals("bannar.PNG") && !file.Name.Equals("main.png") && !file.Name.Equals("main.PNG"))
                    {
                        images.Add(sprite);
                    }
                    sprite = null;
                }
                info = null;
                dir = null;
            }
        }
        return images;
    }
}
