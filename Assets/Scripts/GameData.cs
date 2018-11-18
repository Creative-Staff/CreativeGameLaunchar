using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 左のゲームリストボタンオブジェクトが持つクラス、自分たちのゲームの情報(gameDataParam)を持つ
/// </summary>
public class GameData : MonoBehaviour {

    [SerializeField]
    private GameDataParam gameDataParam;

    public void Initiate(GameDataParam param)
    {
        gameDataParam = param;
    }

    private void Update()
    {
        //カーソル表示処理
        if(LauncharManager.Instance.displayGameDataParam.gameID == gameDataParam.gameID)
            gameObject.GetComponent<Image>().enabled = true;
        else
            gameObject.GetComponent<Image>().enabled = false;
    }

    /// <summary>
    /// 自身が押された際(選択中になった際)に呼び出す関数 -> 自身の情報をマネージャーに渡す
    /// </summary>
    public void OnListGameButton()
    {
        LauncharManager.Instance.displayGameDataParam = gameDataParam;
        PanelDisplay.Instance.UpdatePanel();
    }
}
