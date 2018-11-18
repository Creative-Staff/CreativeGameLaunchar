using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameType
{
    Act,
    RPG,
    Shooting,
    Simulation,
    Novel,
    Race,
    Other
}

[System.Serializable]
public class GameDataParam
{
    public int gameID;          //ゲームID -> 読み込んだ順に連番で振っただけの番号
    public string gameTitle;    //ゲームタイトル
    public GameType gameType;   //ゲームのジャンル(RPG、シューティングなど)
    public string openDirName;  //開くファイルを格納しているディレクトリ(フォルダ)の名前
    public string openFileName; //開くファイルの名前
    public string description;  //ゲーム説明文
    public bool is3dGame;       //3Dゲームかどうか
    public bool isTrialGame;    //体験版かどうか

    public GameDataParam()
    {
        gameID = -1;
    }
}