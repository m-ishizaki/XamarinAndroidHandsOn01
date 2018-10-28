[< 前ページ](./textbook3.md)  

# MainActivity クラスの編集
```com.example.android.mediarecorder.MainActivity``` クラス書き換えていきます。  
  
## MainActivity クラス
MainActivity クラスは既に生成されているクラスを書き換えて行きます。  

## using の追加
今回も必要な ```using``` は最初に追加してしまいます。ファイルの先頭の ```using``` の部分が次のようになるようコピー&ペーストしてください。  
```cs
using System;
using Android.App;
using Android.Content.PM;
using Android.Hardware;
using Android.OS;
using Android.Views;
using Java.IO;
using Android.Media;
using Android.Widget;
using Java.Lang;
using Android.Util;
using System.Collections.Generic;
using String = System.String;
```

## データメンバー の移植
Java のクラスの先頭で定義されているデータメンバーを移植します。  
MainActivity.java の 46 行目からの次のコードを、C# の MainActivity.cs の同じ位置 (クラスの宣言と ```OnCreate``` メソッドの宣言の間) に追加します。  
下記の C# のコードをコピー&ペーストしてください。  
・Java のコード
```java
private Camera mCamera;
private TextureView mPreview;
private MediaRecorder mMediaRecorder;
private File mOutputFile;

private boolean isRecording = false;
private static final String TAG = "Recorder";
private Button captureButton;
```
・C# のコード
```cs
private Camera mCamera;
private TextureView mPreview;
private Android.Media.MediaRecorder mMediaRecorder;
private File mOutputFile;

private bool isRecording = false;
private static readonly string TAG = "Recorder";
private Button captureButton;
```
## コントロールのインスタンス取得の追加
```OnCreate``` での処理を追加します。```OnCreate``` は画面生成時の処理で、今回は画面上のコントロールの実態をデータメンバーに保持します。  
```cs
SetContentView(Resource.Layout.sample_main);
```
の下に下記の C# コードをコピー&ペーストします。これまで同様、Java では小文字始まりだったメソッド名が、大文字始まりになります。  
・Java のコード
```java
mPreview = (TextureView) findViewById(R.id.surface_view);
captureButton = (Button) findViewById(R.id.button_capture);
```
・C# のコード
```cs
mPreview = (TextureView)FindViewById(Resource.Id.surface_view);
captureButton = (Button)FindViewById(Resource.Id.button_capture);
```
  
```OnCreate``` は次のようになります。  
```cs
protected override void OnCreate(Bundle savedInstanceState)
{
    base.OnCreate(savedInstanceState);
 
    SetContentView(Resource.Layout.sample_main);
 
    mPreview = (TextureView)FindViewById(Resource.Id.surface_view);
    captureButton = (Button)FindViewById(Resource.Id.button_capture);
}
```

## OnCaptureClick メソッドのコピー&ペースト
既に作成済みの ```OnCaptureClick``` メソッドに実装を追加します。作成済みの ```OnCaptureClick``` メソッドは次のようになっているはずです。
```cs
[Java.Interop.Export("onCaptureClick")]
public void OnCaptureClick(View view)
{
}
``` 
このメソッドの中に、MainActivity.java の 71 行目 ```onCaptureClick``` メソッドの内容をコピー&ペーストします。  
いくらかのエラーが出ますが、構わず先へ進みます。  
  
## その他のメソッド等のコピー&ペースト
```OnCaptureClick``` の下に、MainActivity.java の 105 行目 ```setCaptureButtonText``` メソッド以降のコードを最後の ```}``` を除いてコピー&ペーストします。  

## メソッド名の変更
Java ではメソッド名は小文字で始まりますが、C# では大文字で始まります。  
小文字で始まっているメソッド名を、大文字始まりに一気に変更して行きます。  
  
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```mMediaRecorder.stop()``` → ```mMediaRecorder.Stop()```  
・```mOutputFile.delete()``` → ```mOutputFile.Delete()```  
・```mCamera.lock``` → ```mCamera.Lock```  
・```mMediaRecorder.reset()``` → ```mMediaRecorder.Reset()```  
・```mMediaRecorder.release()``` → ```mMediaRecorder.Release()```  
・```mCamera.release()``` → ```mCamera.Release()```  
・```mCamera.getParameters()``` → ```mCamera.GetParameters()```  
・```CamcorderProfile.get(``` → ```CamcorderProfile.Get(```  
・```parameters.setPreviewSize(``` → ```parameters.SetPreviewSize(```  
・```mCamera.setParameters(``` → ```mCamera.SetParameters(```  
・```mCamera.setPreviewTexture(``` → ```mCamera.SetPreviewTexture(```  
・```mCamera.unlock()``` → ```mCamera.Unlock()```  
・```mMediaRecorder.set``` → ```mMediaRecorder.Set```  
・```mMediaRecorder.prepare()``` → ```mMediaRecorder.Prepare()```  
・```mMediaRecorder.start()``` → ```mMediaRecorder.Start()```  
  
## プロパティへの変更 (setter)
Java の setter メソッドはなるべく、プロパティに変更されています。  
   
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```captureButton.setText(title)``` → ```captureButton.Text = title```  

## メソッドの orverride の変更
Java ではスーパークラスのメソッドをサブクラスで orverride する場合はアノテーションを設定しました。C# では return 値の型の前に ```orverride``` キーワードを記述します。  
スーパークラスは C# に移植されたクラスのため、メソッド名の先頭が大文字になる点に注意してください。  
・Java のコード
```java
@Override
protected void onPause()
```
・C# のコード
```cs
protected override void OnPause()
```

## base への書き換え
Java ではスーパークラスのメソッドを呼び出す場合、```super``` キーワードを使用します。C# では同様の機能は ```base``` になります。  
スーパークラスは C# に移植されたクラスのため、メソッド名の先頭が大文字になる点に注意してください。  

置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```super.onPause()``` → ```base.OnPause()```  

## bool への変更
Java では真偽値型は ```boolean``` キーワードですが、C# では ```bool``` になります。  
  
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```boolean``` → ```bool```  
・```Boolean``` → ```bool```  

## IList<T> への書き換え
```List<>``` は Java のインタフェースですが、C# ではインタフェースは頭に I が付いた ```IList<>``` という名前になります。 
  
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```List<``` → ```IList<```  
 
## プロパティへの変更
Java の getter メソッドはなるべく、プロパティに変更されています。  
   
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  

・```parameters.getSupportedPreviewSizes()``` → ```parameters.SupportedPreviewSizes```  
・```parameters.getSupportedVideoSizes()``` → ```parameters.SupportedVideoSizes```  
・```mPreview.getWidth()``` → ```mPreview.Width```  
・```mPreview.getHeight()``` → ```mPreview.Height```  
・```mPreview.getSurfaceTexture()``` → ```mPreview.SurfaceTexture```  
・```e.getMessage()``` → ```e.Message```  
・```mOutputFile.getPath()``` → ```mOutputFile.Path```  

## プロパティ名の変更
Java では公開データメンバは小文字で始まりますが、C# ではプロパティになり、大文字始まりになります。  
  
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```profile.videoFrameWidth``` → ```profile.VideoFrameWidth```  
・```profile.videoFrameHeight``` → ```profile.VideoFrameHeight```  
・```optimalSize.width``` → ```optimalSize.Width```  
・```optimalSize.height``` → ```optimalSize.Height```  

## 列挙体への変更
Java では公開フォールドはなるべく列挙体に変更されています。

置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```CamcorderProfile.QUALITY_HIGH``` → ```CamcorderQuality.High```  
・```MediaRecorder.AudioSource.DEFAULT``` → ```AudioSource.Default```  
・```MediaRecorder.VideoSource.CAMERA``` → ```VideoSource.Camera```  

## デバッグ出力メソッドの変更
デバッグ出力メソッドのメソッド名 ```d``` は ```Debug``` になっているので変更します。  
また、エラー出力のメソッド名 ```e``` は ```Error``` になります。  
   
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```Log.d(``` → ```Log.Debug(```  
・```Log.e(``` → ```Log.Error(```  

## クラス継承のキーワードの変更
Java ではクラスの継承は ```extends``` キーワードですが、C# では ```:``` になります。  
   
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```extends``` → ```:```  

## 型引数 Void の変更
```AsyncTask<>``` の型引数 ```Void``` は ```Java.Lang.Void``` であることを明示します。  
   
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```AsyncTask<Void, Void, bool>``` → ```AsyncTask<Java.Lang.Void, Java.Lang.Void, bool>```  

## MediaPrepareTask クラスの変更
```MediaPrepareTask``` クラスは Android API のクラスで、非同期処理を行う ```AsyncTask``` クラスのサブクラスです。  
この ```MediaPrepareTask``` クラス内の変更も基本はこれまでのパターンで良いのですが、このクラスがインナークラスとして実装されているという大きな違いがあります。  
Java のインナークラスは親クラスのインスタンスを参照できますが、C# ではそのようなことができません。  
C# へ置き換える際に、```MediaPrepareTask``` クラスが親クラスである ```MainActivity``` クラスのインスタンスを明示的に持つよう変更します。  
  
少し複雑な書き換えを行うため ```MediaPrepareTask``` クラスは完成コードをまるごとコピー&ペーストします。  
次のコードへコピー&ペーストで置き換えてください。
```cs
class MediaPrepareTask : AsyncTask<Java.Lang.Void, Java.Lang.Void, bool>
{
    private readonly WeakReference<MainActivity> _activity;
    public MediaPrepareTask(MainActivity activity)
    {
        _activity = new WeakReference<MainActivity>(activity);
    }
    protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] voids)
    {
        MainActivity activity;
        _activity.TryGetTarget(out activity);
        if (activity.prepareVideoRecorder())
        {
            activity.mMediaRecorder.Start();
            activity.isRecording = true;
        }
        else
        {
            activity.releaseMediaRecorder();
            return false;
        }
        return true;
    }
    protected override void OnPostExecute(bool result)
    {
        MainActivity activity;
        _activity.TryGetTarget(out activity);
        if (!result)
        {
            activity.Finish();
        }
        activity.setCaptureButtonText("Stop");
    }
    protected override bool RunInBackground(params Java.Lang.Void[] @params)
    {
        throw new NotImplementedException();
    }
}
```

## MediaPrepareTask クラス使用の変更
```MediaPrepareTask``` クラスを ```MainActivity``` クラスのインスタンスを保持するよう変更しました。  
そのため、```MediaPrepareTask``` クラスの使用されている場所で、```MainActivity``` クラスのインスタンスを設定するよう変更します。  
   
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```new MediaPrepareTask().execute(null, null, null);``` → ```new MediaPrepareTask(this).Execute(null, null, null);```
  
# ■ MainActivity クラスの完成
これで ```MainActivity``` クラスは完成です。  
ビルドを行いエラーが出ないことを確認してください。(警告は大量に出ますが、今回はそのままにします)  
エラーが出た場合は、エラーの内容と上記の書き換えを見比べて頑張ってエラーに対処してください。    

# ■ 実行
ここまでで、一度アプリを実行してみましょう。  
Visual Studio (for Mac) のツールバーの 右三角 アイコンをクリックします。  
次の画面が表示されるはずです。  
**[完成画面]**  
<image src="./image1.jpg" alt="完成画像" title="完成画像" width=200/>  
  
ここで、[capture] ボタンをタップするとエラーが発生します。  
これはまだこのアプリがカメラ等を使うためのパーミッションを設定していないためです。  
パーミッションを正しく設定していない場合、該当する機能を使用しようとするとエラーが発生するのがスマートデバイスの特徴です。  
  
# ■ 設定ファイルの設定
編集する設定ファイルは、```Properties``` フォルダー内の ```AndroidManifest.xml``` ファイルです。  
・ソリューションエクスプローラー上で ```AndroidManifest.xml``` をダブルクリックします。  
・エディターで XML ファイルが開くので次のように編集します。ソースが開かない場合、[ソース]タブを選択してください。  
・```<manifest>``` タグの中に次の要素を追加します。  
```xml
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
<uses-permission android:name="android.permission.RECORD_VIDEO" />
<uses-permission android:name="android.permission.RECORD_AUDIO"/>
<uses-permission android:name="android.permission.CAMERA" />
<uses-feature android:name="android.hardware.camera" />
```
```AndroidManifest.xml``` 全体は例えば次のようになります。  
```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android" android:versionCode="1" android:versionName="1.0" package="com.example.android.XamMediaRecorder">
    <uses-sdk android:minSdkVersion="16" />
    <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
    <uses-permission android:name="android.permission.RECORD_VIDEO" />
    <uses-permission android:name="android.permission.RECORD_AUDIO"/>
    <uses-permission android:name="android.permission.CAMERA" />
    <uses-feature android:name="android.hardware.camera" />
    <application android:allowBackup="true" android:icon="@drawable/ic_launcher" android:label="@string/app_name"/>
</manifest>
```

# ■ 端末の設定
前述のパーミッションを設定してもエラーが発生する場合は、スマートフォンの設定からアプリへの許可を設定します。  
※手順は端末により異なる場合があります。  
・スマートフォンで [設定] アプリを開きます。  
・[アプリ] をタップします。  
・[MediaRecorder] をタップします。  
・[許可] をタップします。  
・[カメラ] [ストレージ] [マイク] のスイッチを ON にします。

# ■ 実行
これでアプリが正しく動作するはずです。  
もし、```activity.mMediaRecorder.Start();``` で例外が発生する場合は、```AndroidManifest.xml``` の ```<uses-sdk``` の設定値を確認してみてください。  
  
お疲れ様でした！ これで Android アプリの Xamarin.Android への移植ができました。  
この経験をもとにぜひ Xamarin で Android アプリを開発してください！

[< 前ページ](./textbook3.md)  
