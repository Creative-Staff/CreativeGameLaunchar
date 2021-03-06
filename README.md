![Top Image](https://github.com/Creative-Staff/CreativeGameLaunchar/blob/master/Assets/Sprites/Other/cgl.PNG)

# CreativeGameLaunchar
サークル展示用ゲーム起動ランチャー「CreativeGameLaunchar」

## 概要
exe(開発中はAssetsフォルダ)同列に格納されている「Games」フォルダからparam.csvを読み込み各ゲーム情報を基にバナーボタンオブジェクトを作成  
読み込んだ情報内の一つである起動ファイル名、起動ファイルが格納されているフォルダ名から  
起動するゲームファイルまでのパスを読み込みゲームを起動出来るゲーム起動ランチャーになります。  

各ゲームフォルダ内に「cgl」フォルダを作成し、その中に以下のファイルを置くとそれぞれランチャー中で反映されます  
・「bannar.png」：ゲームバナーの画像として参照されます png形式のみですがスクリプトの参照文字列を改変すればjpg等でも可能(多分)  
・「main.png」：スクリーンショットリストの先頭、バックグラウンド画像等として参照されます。  
・「video.mp4」スクリーンショットリストの動画像として参照される、スクリプトの参照文字列を改変すればmp4以外でも可能(多分)  
・それ以外のpngファイル：スクリーンショットリストの画像として参照される  

## Scene 
slMain -> メインシーン、csvで読み込んだゲームリストから一つゲームを選択して、ボタンを押すとゲームを起動出来る  
## Sprites
### CSLogo
サークルのロゴ画像等を格納  
### typeBannar
ゲームジャンル(RPG、シューティングなど)の画像を格納  
### Other
バックグラウンド画像やボタンマテリアル等を格納(追加する場合は適宜フォルダ分けとかをした方が良いかも)  
## Prefabs
主にテンプレ
### GameData
slMainシーン左のゲーム一覧リストのバナーオブジェクトのテンプレート  
### SSImage
スクリーンショットリストの画像用テンプレート  
### SSVideo
スクリーンショットリストの動画用テンプレート  
### TypeBannar
ゲームジャンル画像用オブジェクトのテンプレート  
## Scripts
### LauncharManager
ボタンを押した際にファイルパスから外部ファイルを起動させる処理など、内部処理を担うクラス  
### PanelDisplay
選択中ゲームのスクリーンショットリスト動作やゲーム概要の表示処理など、主に外観処理を担うクラス  
### CSVManager
param.csvを読み込んでslMainシーン左のゲームリストオブジェクトのテンプレートを作成していくクラス  
### GameData
左のゲームリストボタンオブジェクトが持つクラス、自分たちのゲームの情報(gameDataParam)を持つ  
### GameDataParam
ゲーム情報(ゲームタイトル、開くファイルまでのパス、説明文など)を格納するクラス  
### VideoPlayerOnUGui
SSVideo再生用クラス、Update毎にテクスチャ更新しているだけ  
### CSVReader
実際にCSVを読み込んでくれている外部ライブラリ、通常のCSVReaderだと改行が対応出来ない為使用
### GameImage
フォルダパスからSpriteを読み込む外部ライブラリ
### SnapScroll
スクリーンショットリストにスナップスクロールを対応させる為の外部ライブラリ、詳しくはスクリプト記載の作者様のページを参照
