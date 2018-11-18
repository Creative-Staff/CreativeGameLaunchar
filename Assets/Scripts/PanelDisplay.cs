using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.IO;
using System;

//インスペクタ整理の為に各リソースは別クラスにまとめる
[System.Serializable]
public class DisplayReresources
{
    public Image mainBackImage;
    public Image titleBackImage;

    public Text gameTitleText;
    public Text description;

    public Transform gameTypeTransform;
    public Transform ssImageTransform;
    public Transform ssImageListTransform;

    public Sprite[] typeImages;
    public Sprite Bannar3DGame;
    public Sprite BannarTrialGame;

    public GameObject typeImageTemp;
    public GameObject ssImageTemp;
    public GameObject ssVideoTemp;

    public SnapScroll ssImageScrollRect;
}

/// <summary>
/// 選択中ゲームのスクリーンショットリスト動作やゲーム概要の表示処理など、主に外観処理を担うクラス
/// </summary>
public class PanelDisplay : MonoBehaviour
{

    private List<Sprite> ssImageList;
    private List<List<Sprite>> ssImageGameList = new List<List<Sprite>>();

    [SerializeField] private float SSScrollInterval; //スクショのスクロール間隔
    private float scrollFrame;

    private bool hasSSVideo;
    private bool isVideoEnd;

    [SerializeField] private DisplayReresources resources = new DisplayReresources();

    public static PanelDisplay Instance;

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        ResetDisplay();
        for (int i = 0; i < CSVManager.Instance.cacheList.Count; i++)
        {
            ssImageGameList.Add(null);
        }
        scrollFrame = 0.0f;
        hasSSVideo = false;
        isVideoEnd = true;
    }

    private void Update()
    {
        //SSスクロール処理
        if (ssImageList != null && isVideoEnd)
        {
            if (ssImageList.Count != 0)
            {
                scrollFrame += Time.deltaTime;
                if (scrollFrame > SSScrollInterval)
                {
                    scrollFrame = 0.0f;
                    if (resources.ssImageScrollRect.hIndex < (ssImageList.Count + Convert.ToInt32(hasSSVideo)) - 1)
                    {
                        resources.ssImageScrollRect.ScrollTo(resources.ssImageScrollRect.hIndex + 1, 0);
                    }
                    else
                    {
                        resources.ssImageScrollRect.ScrollTo(0, 0);
                    }
                }
            }
        }
        if (hasSSVideo && resources.ssImageScrollRect.hIndex == 0)
            isVideoEnd = false;
        else
            isVideoEnd = true;
    }

    public void UpdatePanel()
    {
        //初期化・リセット
        ResetDisplay();

        //ゲームタイトル更新
        resources.gameTitleText.text = LauncharManager.Instance.displayGameDataParam.gameTitle;

        //ゲームジャンル画像更新
        GameObject typeObject = Instantiate(resources.typeImageTemp, resources.gameTypeTransform);
        typeObject.GetComponent<Image>().sprite = resources.typeImages[(int)LauncharManager.Instance.displayGameDataParam.gameType];

        //3Dゲームの場合はジャンル欄に追加画像
        if (LauncharManager.Instance.displayGameDataParam.is3dGame)
        {
            typeObject = Instantiate(resources.typeImageTemp, resources.gameTypeTransform);
            typeObject.GetComponent<Image>().sprite = resources.Bannar3DGame;
        }

        //体験版の場合はジャンル欄に追加画像
        if (LauncharManager.Instance.displayGameDataParam.isTrialGame)
        {
            typeObject = Instantiate(resources.typeImageTemp, resources.gameTypeTransform);
            typeObject.GetComponent<Image>().sprite = resources.BannarTrialGame;
        }

        //ゲーム説明文更新
        resources.description.text = LauncharManager.Instance.displayGameDataParam.description;

        string cglPath = Environment.CurrentDirectory + "\\Games\\" + LauncharManager.Instance.displayGameDataParam.openDirName + "\\cgl";

        //メイン画像更新
        resources.mainBackImage.sprite = GameImage.Instance.SpriteFromFile(cglPath + "\\main.png");
        resources.titleBackImage.sprite = GameImage.Instance.SpriteFromFile(cglPath + "\\main.png");
        resources.titleBackImage.enabled = true;

        //SSリスト更新(動画)
        string videoPath = cglPath + "\\video.mp4";
        hasSSVideo = false;
        if (File.Exists(videoPath))
        {
            GameObject ssVideoObject = Instantiate(resources.ssVideoTemp, resources.ssImageTransform);
            VideoPlayer vp = ssVideoObject.GetComponent<VideoPlayer>();
            vp.url = videoPath;
            vp.loopPointReached += VideoEndProcess;
            vp.Play();

            ssVideoObject = Instantiate(ssVideoObject, resources.ssImageListTransform);
            ssVideoObject.GetComponent<VideoPlayer>().Pause();
            ssVideoObject.GetComponent<Button>().onClick.AddListener(() => { OnSSButtonClick(0); });

            hasSSVideo = true;
            isVideoEnd = false;
        }

        //SSリスト(画像)処理(既に読み込んでいる画像リストがない時 -> 新たに読み込む)
        if (ssImageGameList[LauncharManager.Instance.displayGameDataParam.gameID] == null)
        {
            //SSリスト更新(画像)
            ssImageList = GameImage.Instance.SpriteListFromFolder(cglPath + "\\");
            //先頭にメイン画像を挿入
            Sprite mainImage = GameImage.Instance.SpriteFromFile(cglPath + "\\main.png");
            if (mainImage != null)
            {
                ssImageList.Insert(0, mainImage);
            }
            ssImageGameList[LauncharManager.Instance.displayGameDataParam.gameID] = ssImageList;
        }
        else
        {
            ssImageList = ssImageGameList[LauncharManager.Instance.displayGameDataParam.gameID];
        }

        for (int s = 0; s < ssImageList.Count; s++)
        {
            GameObject ssImage = Instantiate(resources.ssImageTemp, resources.ssImageTransform);
            ssImage.GetComponent<Image>().sprite = ssImageList[s];
            ssImage = Instantiate(ssImage, resources.ssImageListTransform);
            int index = s + Convert.ToInt32(hasSSVideo); //ラムダ式用一時変数
            ssImage.GetComponent<Button>().onClick.AddListener(() => { OnSSButtonClick(index); });
        }
        resources.ssImageScrollRect.hPageNum = ssImageList.Count + Convert.ToInt32(hasSSVideo);
        resources.ssImageScrollRect.ScrollTo(0, 0);
        resources.ssImageScrollRect.ReAwake();
    }

    public void ResetDisplay()
    {
        foreach (Transform n in resources.gameTypeTransform)
        {
            GameObject.Destroy(n.gameObject);
        }
        foreach (Transform n in resources.ssImageListTransform)
        {
            GameObject.Destroy(n.gameObject);
        }
        foreach (Transform n in resources.ssImageTransform)
        {
            GameObject.Destroy(n.gameObject);
        }

        resources.mainBackImage.sprite = null;
        resources.titleBackImage.sprite = null;
        resources.titleBackImage.enabled = false;

        resources.gameTitleText.text = "ここにタイトルが表示されるよ";
        resources.description.text = "こっちにはゲーム説明文が表示されます\n気になるゲームのバナーをクリックしてね！";
    }

    public void OnSSButtonClick(int index)
    {
        resources.ssImageScrollRect.ScrollTo(index, 0);
    }

    private void VideoEndProcess(VideoPlayer vp)
    {
        isVideoEnd = true;
    }
}
