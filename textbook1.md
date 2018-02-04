[< 前ページ](./readme.md) | [次ページ >](./textbook2.md)
# プロジェクトの新規作成
Xamarin.Android のプロジェクト(ソリューション)を新規作成します。  
プロジェクト名はお好きなもので構いませんが、今回は「XAMediaRecorder」で作ります。  

## Mac の場合
・Visual Studio for Mac を起動します。  
・メニューの ```ファイル > 新しいソリューション``` を選択します。  
・[新しいプロジェクト用のテンプレートを選択する] で ```Android > アプリ > (全般) の 空の Android アプリ (C#)``` を選択し、[次へ] をクリックします。  
・[アプリ名] に「```XAMediaRecorder```」を入力します。  
・[ターゲット プラットフォーム] で ```最新および最高``` を選択します。  
・[次へ] をクリックします。  
・[プロジェクト名] [ソリューション名] は特に変える必要はありません。[場所] は好みの場所があれば変更してください。  
・[作成] をクリックします。  
  
## Windows の場合
・Visual Studio (2017) を起動します。  
・メニューの ```ファイル > 新規作成 > プロジェクト``` を選択します。  
・[新しいプロジェクト] で ```Visual C# > Android > 空のアプリ(Android)``` を選択します。  
・[名前] に「```XAMediaRecorder```」を入力します。[場所] は好みの場所があれば変更してください。  
・[OK] をクリックします。  
  
# UI の移植
ネイティブの UI 定義が使えることが Xamarin の特徴です。  
Android で言えば、Android(Java) のプロジェクトで ```res``` フォルダー内にある ```.axml や .xml、画像ファイル``` などをそのまま使えます。  
  
## Mac の場合
```res``` フォルダーは Xamrin では ```Resources``` フォルダになります。  
・Visual Studio for Mac の [ソリューション] の ```Resources``` を右クリック(二本指タップ)します。  
・```追加 > フォルダーからファイルを追加``` を選択します。  
・Android(Java) プロジェクトの ```res``` フォルダーを選択し、[開く] をクリックします。  
・[res から追加するファイルを選ぶ] で [すべて含める] をクリックします。  
・全てチェック ON になっていることを確認し、[OK] をクリックします。  
・[ファイルをフォルダーに追加する] で [ファイルをディレクトリにコピーします] を選択し、[開く] をクリックします。  
・[ファイルXXXXX は既に存在します。置き換えますか？] と出た場合、[すべてに適用] を選択し [ファイルを上書き] をクリックします。  
  
## Windows の場合
```res``` フォルダーは Xamrin では ```Resources``` フォルダーになります。  
・(Win+e などで)エクスプローラーを開き、Android(Java) プロジェクトの ```res``` フォルダーの中身(フォルダー、ファイル)をすべて選択しドラッグ、Visual Studio のソリューションエクスプローラー上の ```Resources``` フォルダーにドロップします。  
・タイトル「ファイルを新しい場所に移動しますか？」メッセージ「１つ以上のファイルを "XXXXXXX\Resources" に移動します。この操作は長時間かかることがあります。」のダイアログで [OK] をクリックします。  
  
## 共通
・「ファイルが見つかりません」というエラーが出て、```Resources\layout\Main.axml``` 無いとされる場合、ソリューションエクスプローラーから ```Main.axml``` ファイルを削除します。  
※ Mac の場合は、ソリューションエクスプローラー上でファイル名が赤く表示されます。  
特にエラー等が発生しない場合はこの手順は不要です。  
  
# UI の適用
ネイティブの UI 定義が使えることが Xamarin の特徴です。  
```axml``` などのファイルを使えるばかりでなく、Activity も Activity クラスで定義します。  
また、先程コピーしたリソースファイルも、自動生成される ```Resource``` クラスに定数として定義されます。  
(Android(Java) では ```R``` でしたが Xamarin では ```Resource``` になります)  
また、Activity クラスのメソッドなども基本、Java と同じものが定義されており、同じ使い方になります。  
ただし、メソッド名などは C# の流儀に変わっているので少し名前が変更されています。  
例えば Java では ```onCreate``` メソッドでしたが、C# では ```OnCreate``` メソッドになっています。  
  
## Activity の書き換え
・ソリューションエクスプローラー上で、```MainActivisy.cs``` をダブルクリックして開きます。  
・7 行目あたりに
```cs
[Activity(Label = "XamMediaRecorder", MainLauncher = true, ...
```
という記述があります。  
これが Activity に対する設定になります。Android では ```AndroidManifest.xml``` 内に記述していましたが、Xamarin では Activity クラスの属性として設定します。  
属性とは、Java で言うところのアノテーションに当たる機能です。  
この属性を先程追加したリソースを参照するよう、次のように変更します。  
```cs
[Activity(Label = "@string/app_name", MainLauncher = true, ScreenOrientation = ScreenOrientation.Landscape)]
```
・ここでビルドを行うと「ScreenOrientation が見つからない」という様なエラーになります。  
ScreenOrientation クラスの存在する名前空間(Java のパッケージに当たる機能)を ```using``` (Java でいう import に当たる機能) で宣言します。  
ファイルの先頭の ```using``` の塊に次を追加します。  
```cs
using Android.Content.PM;
``` 
この追加はエラーの発生している ```ScreenOrientation``` にカーソルを合わせて、Mac の場合は ```option+enter > クイック修正```、Windows の場合は ```ctrl+.``` から自動で追加することもできます。  
・画面レイアウトの定義も、先程追加したリソースを参照するよう、次のように変更します。  
変更箇所は15〜17行目あたりにあります。  
**変更前**
```cs
SetContentView(Resource.Layout.Main);
```
**変更後**
```cs
SetContentView(Resource.Layout.sample_main);
```
※ ```.sample_main``` がエラーになる場合、この手順書の少し上に書かれている「**UI の移植**」>「**共通**」の「ファイルが見つかりません」を参考に ```Main.axml``` を削除してください。  
※ OnCreate メソッド内に次のようなコードが書かれている場合があります。  
```cs
Button button = FindViewById<Button(Resource.Id.button_capture);

button.Click += delegate {button.Text = $"{count++} clicks!";
```
このコードは、不要なので削除します。  

## イベントハンドラメソッドの作成
サンプルの画面定義にはボタンクリックのイベントが設定されています。  
このイベントのハンドラとなるメソッドを MainActivity に作成します。  
・MainActivity クラスに次のメソッド定義を追加します。  
```Java.Interop.Export``` 属性で定義に設定されているメソッド名を設定することがポイントです。  
```csh
[Java.Interop.Export("onCaptureClick")]
public void OnCaptureClick(View view)
{
}
```
・ここでビルドを行うと「View が見つからない」という様なエラーになります。  
View クラスの存在する名前空間(Java のパッケージに当たる機能)を ```using``` (Java でいう import に当たる機能) で宣言します。  
ファイルの先頭の ```using``` の塊に次を追加します。  
```cs
using Android.Views;
``` 
この追加はエラーの発生している ```View``` にカーソルを合わせて、Mac の場合は ```option+enter > クイック修正```、Windows の場合は ```ctrl+.``` から自動で追加することもできます。  
・また、Export 属性を使用する場合、dll の参照を追加する必要があります。  
Mac の場合は、ソリューション上の ```[参照] を右クリック(二本指タップ) > [参照の編集]``` で開くダイアログで。  
Windows の場合は、```[参照] を右クリック > [参照の追加]``` で開くダイアログで [アセンブリ] を選択し。  
```Mono.Android.Export``` にチェックをつけて [OK] をクリックします。  
  
ここまでで、```MainActivity.cs``` は次の様になります。  
```cs
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Views;

namespace XamMediaRecorder
{
    [Activity(Label = "@string/app_name", MainLauncher = true, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.sample_main);
        }

        [Java.Interop.Export("onCaptureClick")]
        public void OnCaptureClick(View view)
        {
        }
    }
}
```

# UI の確認
これだけで、UI の移植が完了しました。  
F5 キーで実行し画面を確認しましょう。  
次の様な画面が表示されていれば成功です。  
<image src="./image1.jpg" alt="完成画像" title="完成画像" width=200/>  
  
# アプリケーションのアイコンの設定
アイコンは画像ファイルはこれまでの手順で移植しているリソースの中に含まれています。  
 
アプリケーションのアイコンはアプリケーションのプロパティから設定できますが、今回は設定のファイルを直接編集します。  
GUI は分かりやすいですが、使う IDE や IDE のバーションアップなどによって変わってしまいます。  
また、今回編集する設定ファイルは内容が簡単であり、またサンプルアプリがお手本としてあるため直接編集してしまう方が間違いがありません。  
お手本がない場合は、GUI から設定すると良いでしょう。  

## 設定ファイルの設定
編集する設定ファイルは、```Properties``` フォルダー内の ```AndroidManifest.xml``` ファイルです。  
・ソリューションエクスプローラー上で ```AndroidManifest.xml``` をダブルクリックします。  
・エディターで XML ファイルが開くので次のように編集します(GUI の設定画面が開く場合は、[ソース]タブを選択します)。  
・```<application>``` タグの中に ```android:icon``` 属性がある場合は次のように編集します。ない場合は書き足します。  
```xml
<application android:allowBackup="true" android:icon="@drawable/ic_launcher" android:label="@string/app_name">
</application>
```
これで、```Resources``` フォルダー内の ```drawable``` フォルダー内の ```ic_launcher.png``` ファイルがアイコンとして使用されます。  
  
## アイコン確認
これだけで、アイコンの移植が完了しました。  
F5 キーで実行し、画面を確認しましょう。  
アプリケーションのアイコンがビデオカメラ？のような画像になっていれば成功です。
  
[< 前ページ](./readme.md) | [次ページ >](./textbook2.md)  
