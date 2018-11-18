using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Diagnostics;
using System.IO;
using System;

/// <summary>
/// ボタンを押した際にファイルパスから外部ファイルを起動させる処理など、内部処理を担うクラス
/// </summary>
public class LauncharManager : MonoBehaviour
{

    public static LauncharManager Instance;

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }

    [SerializeField] private GameObject changeActiveNowGoObject; //「ゲーム起動中」のパネルデータ

    [SerializeField] private Image startButtonImage;

    private Process exProcess;

    public GameDataParam displayGameDataParam; //現在選択しているゲームデータ(ゲームタイトル、起動パス等)

    private void Start()
    {
        displayGameDataParam = new GameDataParam();
    }

    private void Update()
    {
        if (exProcess != null)
        {
            if (exProcess.HasExited == true)
            {
                //起動しているゲームが何らかの理由(正常に閉じた場合含む)で存在しない時
                //イベントを初期状態に戻し、「ゲーム起動中」パネルを非表示にする
                changeActiveNowGoObject.SetActive(false);
                exProcess = null;
            }
        }

        if(displayGameDataParam.gameID != -1) startButtonImage.enabled = true;
        else startButtonImage.enabled = false;

        //==========================================================================
        //　キー入力処理
        //==========================================================================
        if (Input.GetAxisRaw("Vertical") != 0.0f && displayGameDataParam.gameID == -1)
        {
            //まだどのゲームも選択していない時に上下キー入力を受け付けた時 -> 0番目のゲームを選択させる
            displayGameDataParam = CSVManager.Instance.cacheList[0];
            PanelDisplay.Instance.UpdatePanel();
        }
        if (Input.GetAxisRaw("Vertical") < -0.1f && displayGameDataParam.gameID < CSVManager.Instance.cacheList.Count - 1)
        {
            //下キー入力 -> ゲーム選択カーソルを一段下に移動
            displayGameDataParam = CSVManager.Instance.cacheList[displayGameDataParam.gameID + 1];
            PanelDisplay.Instance.UpdatePanel();
        }
        if (Input.GetAxisRaw("Vertical") > 0.1f && displayGameDataParam.gameID > 0)
        {
            //上キー入力 -> ゲーム選択カーソルを一段上に移動
            displayGameDataParam = CSVManager.Instance.cacheList[displayGameDataParam.gameID - 1];
            PanelDisplay.Instance.UpdatePanel();
        }

        if (Input.GetButtonDown("Enter"))
        {
            //決定キー入力 -> 選択中のゲームを起動させる
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //escキー -> ゲーム選択カーソルを外す(初期状態に戻す)
            changeActiveNowGoObject.SetActive(false);
            exProcess = null;
            displayGameDataParam = new GameDataParam();
            PanelDisplay.Instance.ResetDisplay();
        }
    }

    public void StartGame()
    {
        if (exProcess == null && displayGameDataParam.gameID != -1)
        {
            //起動しているアプリのデータパスを取得
            string path = Environment.CurrentDirectory;

            //Processインスタンスを作成し開く対象のファイルパス(ランチャーの場合exe)を設定
            exProcess = new Process();
            exProcess.StartInfo.FileName
                = path + "\\Games\\" + displayGameDataParam.openDirName + "\\" + displayGameDataParam.openFileName;

            /*
             * カレントディレクトリを起動するファイルがあるディレクトリに設定する
             * ランチャー経由の場合、カレントディレクトリがランチャーを起動したディレクトリのままに
             * 設定されており、そのままだと起動したゲーム内の相対パス指定に狂いが生じる為以下の設定を行う。
             */
            Environment.CurrentDirectory = path + "\\Games\\" + displayGameDataParam.openDirName;

            //「ゲーム起動中」画像表示＆デバッグログ表示
            changeActiveNowGoObject.SetActive(true);
            UnityEngine.Debug.Log("Open:" + exProcess.StartInfo.FileName);

            //外部のプロセスを実行する(exe起動)
            exProcess.Start();

            //終了後カレントディレクトリをランチャー経由のものに戻す
            Environment.CurrentDirectory = path;
        }
    }
}
