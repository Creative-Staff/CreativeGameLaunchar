using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

/// <summary>
/// CSV(param.csv)を読み込んで左のゲームリストオブジェクトを生成するクラス
/// </summary>
public class CSVManager : MonoBehaviour
{
    public static CSVManager Instance;

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }

    [SerializeField] private GameObject GameDataTemp;
    [SerializeField] private Transform GameList;

    public List<GameDataParam> cacheList;

    private int readGameId = 0;

    void Start()
    {
        string csvPath = Environment.CurrentDirectory + "\\Games\\param.csv";
        var result = CSVReader.Instance.ParseCSV(File.ReadAllText(csvPath));

        bool isHead = true;

        foreach (var line in result)
        {
            //ヘッダ(パラメータに関係の無いCSV取扱説明文など)を飛ばす
            if (line[0].Equals("EndHead"))
            {
                isHead = false;
                continue;
            }
            else
            {
                if (isHead) continue;
            }
            //ヘッダ以降の行 => パラメータ読み込み

            //ゲーム情報リスト用のクラス(gameDataParam)を作成してcsvで読み込んだ内容を格納する
            GameDataParam cacheParam = new GameDataParam();
            cacheParam.gameID = readGameId;
            cacheParam.gameTitle = line[1];
            cacheParam.gameType = (GameType)int.Parse(line[2]);
            cacheParam.openDirName = line[3];
            cacheParam.openFileName = line[4];
            cacheParam.description = line[5];
            if (line[6].Equals("TRUE")) cacheParam.is3dGame = true;
            else cacheParam.is3dGame = false;
            if (line[7].Equals("TRUE")) cacheParam.isTrialGame = true;
            else cacheParam.isTrialGame = false;

            cacheList.Add(cacheParam);

            //テンプレートオブジェクト生成
            GameObject gameData = Instantiate(GameDataTemp, GameList);
            //各種パラメータ設定(タイトル名、ジャンルなど)
            gameData.GetComponent<GameData>().Initiate(cacheParam);
            //バナー画像設定(Bannar.pngが無い場合はテンプレートバナーを作成する)
            string bannarPath = Environment.CurrentDirectory + "\\Games\\" + line[3] + "\\cgl\\bannar.png";
            if (GameImage.Instance.SpriteFromFile(bannarPath) != null)
                gameData.transform.GetChild(0).GetComponent<Image>().sprite = GameImage.Instance.SpriteFromFile(bannarPath);
            else gameData.transform.GetChild(1).GetComponent<Text>().text = line[1];

            readGameId++;
        }
    }
}
