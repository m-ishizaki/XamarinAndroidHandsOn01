[< 前ページ](./textbook1.md) | [次ページ >](./textbook3.md)  

# ロジックの移植
ここからロジックの移植を行って行きます。  
  
# Android サンプルの構成
サンプルアプリは次の3つのクラスで構成されています。  
・```com.example.android.common.media.CameraHelper``` クラス  
・```com.example.android.common.media.MediaCodecWrapper``` クラス  
・```com.example.android.mediarecorder.MainActivity``` クラス  
この3クラスを C# で書き換えます。  
  
## C# の クラスと Java のクラス
C# のクラスと Java のクラスは記述方法、機能ともに概ね変わりません。  
そのため、今回は C# でも3クラスを作成します。  

## ファイルの作成場所
Java ではクラスのソースファイルは、そのクラスのパッケージに合わせたフォルダーに作成しなければなりません。  
C# でも Java のパッケージに当たる名前空間という機能がありますが、この名前空間とフォルダーは合わせる必要はありません。  
合わせる必要はありませんが、名前空間とフォルダーは合わせて作成するのが一般的です。  
ただし、今回は手順の簡略化のため、ファイルはすべてプロジェクトの一番浅いフォルダーに置いてしまいます。  
  
# CameraHelper クラスの作成
```com.example.android.common.media.CameraHelper``` クラスを書き換えていきます。  
  
## Mac の場合
・ソリューションエクスプローラーの太字になっているプロジェクト名で右クリック (二本指タップ) し ```追加 > 新しいファイル```
を選択します。  
・[新しいファイル]ダイアログで ```General > 空のクラス``` を選択し、[名前] に 「CameraHelper」 を入力します。  
・[新規]ボタンをクリックします。  
  
## Windows の場合
・ソリューションエクスプローラーの太字になっているプロジェクト名で右クリック (二本指タップ) し ```追加 > 新しい項目``` を選択します。  
・[新しい項目の追加]ダイアログで ```Visual C# > クラス``` を選択し、[名前] に 「CameraHelper」 を入力します。  
・[新規]ボタンをクリックします。  

# CameraHelper クラスの移植
上記で作成したクラスのソースのクラス宣言は次のようになっているはずです。  
```cs
public class CameraHelper
{
    public CameraHelper()
    {
    }
}
```
クラスの宣言は Java と大きく変わらないことがわかります。  
このように C# と Java の構文はかなり似た部分が多くなっています。  
  
## ソースコードの コピー & ペースト
C# と Java の構文は似ているので、まずは Java のソースコードを C# へコピー & ペーストしてしまいます。  
Java の ```CameraHelper``` クラスの package、import を除いた
```java
public class CameraHelper {
...
中略
...
}
```
で C# の ```CameraHelper``` クラスの using、namespace を除いた
```cs
public class CameraHelper
{
    public CameraHelper()
    {
    }
}
```
を上書きします。  
大量のエラーが出ますが、これはコードの大枠が C# として認識できるものということでもあります。  
このエラーを潰して行くだけで、Xamarin.Android として成立するコードになる簡単な作業です。  

# エラーの修正
エラーは発生する順序が異なることがあります。次のエラー修正方法を参考に一つづつ修正してください。  
    
## static final の置き換え
大量のエラーの最初は
```java
public static final int MEDIA_TYPE_IMAGE = 1;
```
という一文のエラーです。これは Java で定数を定義している文ですが、C# では ```final``` が ```readonly``` になります。  
```cs
public static readonly int MEDIA_TYPE_IMAGE = 1;
```
すぐ下の ```MEDIA_TYPE_VIDEO``` の定義も同様に書き換えます。  
  
## using の追加
次のエラーは
```java
public static Camera.Size getOptimalVideoSize(List<Camera.Size> supportedVideoSizes,
```
で、```Camera``` が見つからないというエラーです。これは ```Camera``` クラスを使用するための、Java では ```import``` に当たる宣言を行っていないために発生しています。  
C# では ```using``` というキーワードになります。  
```using``` はエラーの発生している ```Camera``` にカーソルを合わせて、Mac の場合は ```option+enter```、Windows の場合は ```ctrl+.``` から自動で追加することもできます。  
```cs
using Android.Hardware;
```
を追加します。  

## IList&lt;T&gt; への書き換え
次のエラーは
```java
public static Camera.Size getOptimalVideoSize(List<Camera.Size> supportedVideoSizes, 
    List<Camera.Size> previewSizes, int w, int h)
```
で、```List<>``` が見つからないというエラーです。```List<>``` は Java のインタフェースですが、C# ではインタフェースは頭に I が付いた ```IList<>``` という名前になります。  
```cs
public static Camera.Size getOptimalVideoSize(IList<Camera.Size> supportedVideoSizes,
    IList<Camera.Size> previewSizes, int w, int h)
```
```IList<>``` を使用するには次の ```using``` を追加します。
```cs
using System.Collections.Generic;
```
を追加します。  
  
さらに 8 行程下の行の
 ```List<>``` も同様に ```IList<>``` に書き換えます。
```java
List<Camera.Size> videoSizes;
```
を
```cs
IList<Camera.Size> videoSizes;
```
に書き換えます。  
  
## const ローカル変数への書き換え
次のエラーは
```java
final double ASPECT_TOLERANCE = 0.1;
```
という一文で、これはローカル定数を宣言しています。C# では ```final``` が ```const``` になります。
```cs
const double ASPECT_TOLERANCE = 0.1;
```
  
## Double.MaxValues への書き換え
次のエラーは
```java
double minDiff = Double.MAX_VALUE;
```
で ```double``` に ```MAX_VALUE``` が見つからないというエラーです。Java では定数はすべて大文字・_区切りで定義されていますが、C# では定数は大文字始まり・大文字区切りで定義されています。
```cs
double minDiff = Double.MaxValue;
```
C# では ```Double``` と ```double``` は別名の関係でどちらで書いても同じです。  
この書き換えは何度か出てくるので、覚えておくかここで一括置換してしまいましょう。  
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
## foreach への書き換え
次のエラーは
```java
for (Camera.Size size : videoSizes)
```
で、```:``` がおかしいですというエラーです。これは Java でコレクションの要素全てに対する繰り返しを行う記述です。
C# では ```foreach (... in ...)``` になります。
```cs
foreach (Camera.Size size in videoSizes)
```
この書き換えは何度か出てくるので、覚えておくかここで一括置換してしまいましょう。  
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
## size.Width、size.Height への書き換え
次のエラーは
```java
double ratio = (double)size.width / size.height;
```
で ```width``` が見つからないというエラーです。Java の public データメンバーは C# ではプロパティになります。public データメンバーとプロパティは利用する側のコードは同じですが、C# の流儀では、頭が大文字になります。  
```cs
double ratio = (double)size.Width / size.height;
```
同様に同じ文の ```size.height``` も ```size.Height``` に変更します。  
```cs
double ratio = (double)size.Width / size.Height;
```
  
これら書き換えは何度か出てくるので、覚えておくかここで一括置換してしまいましょう。  
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
## Math.Abs への書き換え
次のエラーは
```java
if (Math.abs(ratio - targetRatio) > ASPECT_TOLERANCE)
```
で ```abs`` が見つからないというエラーです。Java のメソッド名の流儀は頭を小文字で始めますが、C# では頭が大文字で始まります。  
```cs
if (Math.Abs(ratio - targetRatio) > ASPECT_TOLERANCE)
```
ここで、Math クラスは Java の標準ライブラリのクラスですが、今回書使用しているのは C# の標準ライブラリの Math クラスです。  
このように基本的なクラスには Java と C# で近いものがあることが珍しくありません。  
  
この書き換えは何度か出てくるので、覚えておくかここで一括置換してしまいましょう。  
```Math.abs``` を ```Math.Abs``` と置換します。  
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  

## previewSizes.Contains への書き換え
次のエラーは
```java
if (Math.Abs(size.Height - targetHeight) < minDiff && previewSizes.contains(size))
```
で ```contains``` が見つからないというエラーです。
Java のメソッド名の流儀は頭を小文字で始めますが、C# では頭が大文字で始まります。
```cs
if (Math.Abs(size.Height - targetHeight) < minDiff && previewSizes.contains(size))
```
  
この書き換えは何度か出てくるので、覚えておくかここで一括置換してしまいましょう。  
```previewSizes.contains``` を ```previewSizes.Contains``` と置換します。  
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  

## Camera.Open() への書き換え
次のエラーは
```java
return Camera.open();
```
で ```open``` が見つからないというエラーです。Java のメソッド名の流儀は頭を小文字で始めますが、C# では頭が大文字で始まります。
```cs
return Camera.Open();
```
  
この書き換えは何度か出てくるので、覚えておくかここで一括置換してしまいましょう。  
```Camera.open``` を ```Camera.Open``` と置換します。  
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
## (int)Camera.CameraInfo.CameraFacingBack への書き換え
次のエラーは
```java
return getDefaultCamera(Camera.CameraInfo.CAMERA_FACING_BACK);
```
で ```CAMERA_FACING_BACK``` が見つからないというエラーです。これは Java の定数ですが、C# では列挙体になります。  
Xamarin(C#) ではこのような種類を表す定数はなるべく列挙体にする方針になっています。  
また、列挙体のメンバーも定数なので、C# では大文字始まりの大文字区切りのスタイルになります。  
```cs
return getDefaultCamera(Camera.CameraInfo.CameraFacingBack);
```
ただし、ここでこの書き換えを行うと、またこの一文がエラーになります。  
```getDefaultCamera``` メソッドは今回作成している ```CameraHelper``` クラスで宣言しているメソッドで、引数が int になっています。  
正しく書き換えるのであればメソッドの定義を変えるべきですが、ここでは手数を少なく書き換えのミスを減らすために、列挙体メンバーを int にキャストします。
```cs
return getDefaultCamera((int)Camera.CameraInfo.CameraFacingBack);
```

同様に 9 行程下の
```java
return getDefaultCamera(Camera.CameraInfo.CAMERA_FACING_FRONT);
```
も
```cs
return getDefaultCamera((int)Camera.CameraInfo.CameraFacingFront);
```

## 属性への書き換え
次は
```java
@TargetApi(Build.VERSION_CODES.GINGERBREAD)
```
のエラーです。これは Java のアノテーションの宣言ですが、C# では属性になります。次のように書き換えます。  
```cs
[Android.Annotation.TargetApi(Value = (int)Android.OS.Build.VERSION_CODES.Gingerbread)]
```

## Camera.NumberOfCameras への書き換え
次は
```java
int mNumberOfCameras = Camera.getNumberOfCameras();
```
のエラーです。Xamarin(C#) では決まった値を取得するメソッドはプロパティになっています。  
また、プロパティは大文字で始まります。
```cs
int mNumberOfCameras = Camera.NumberOfCameras;
```
  
## Camera.GetCameraInfo への書き換え
次は
```java
Camera.getCameraInfo(i, cameraInfo);
```
のエラーです。C# ではメソッド名は大文字始まりになります。
```cs
Camera.GetCameraInfo(i, cameraInfo);
```
## cameraInfo.facing への書き換え
次は
```java
if (cameraInfo.facing == position)
```
のエラーです。C# 列挙体になっています。また、変数 ```position``` を int で宣言しているのでキャストします。  
本来であれば変数の型を変更するべきですが、ここでは手数を少なく書き換えのミスを減らすために、列挙体メンバーを int にキャストします。  
```cs
if ((int)cameraInfo.Facing == position)
```

## using Java.IO の追加
次は
```java
public static File getOutputMediaFile(int type)
```
の ```File``` が見つからないというエラーです。  
ファイルの先頭に
```cs
using Java.IO;
```
を追加します。
```using``` は Mac の場合は ```option+enter```、Windows の場合は ```ctrl+.``` から自動で追加することもできます。  
この時、```using System.IO``` と ```using Java.IO``` が選択肢に出てくるので注意してください。  
```System.IO``` 名前空間は .NET(C#) のクラスライブラリーのもので、Xamarin にするならば本来こちらを使用するべきですが、ここでは手数とミスを減らすために、Java から移植されたクラスを使用します。  
  
## Environment の using エイリアスの追加
次は
```java
if (!Environment.getExternalStorageState().equalsIgnoreCase(Environment.MEDIA_MOUNTED))
```
の ```getExternalStorageState``` が見つからないというエラーです。これは、C# と Android API に同名の ```Environment``` クラスが存在し、現在は C# のクラスを参照しています。これを Android API のクラスを参照するように、using エイリアスという機能で ```Environment``` を ```Android.OS.Environment``` の別名として定義します。  
ファイルの先頭に
```cs
using Environment = Android.OS.Environment;
```
を追加します。また、C# では getter メソッドはプロパティになるので、get と () を削除します。
また、```Environment.MEDIA_MOUNTED``` 定数も先頭大文字・大文字区切りに変更します。  
```cs
if (!Environment.ExternalStorageState.equalsIgnoreCase(Environment.MediaMounted))
```
これでも、まだ ```equalsIgnoreCase``` が見つからないというエラーになっています。  
```equalsIgnoreCase``` メソッドに相当する機能は、C# ではオプション引数付きの ```Equals``` メソッドになります。
```cs
if (!Environment.ExternalStorageState.Equals(Environment.MediaMounted, StringComparison.OrdinalIgnoreCase))
```

## Environment.GetExternalStoragePublicDirectory への書き換え
次は
```java
File mediaStorageDir = new File(Environment.getExternalStoragePublicDirectory(
    Environment.DIRECTORY_PICTURES), "CameraSample");
```
の ```getExternalStoragePublicDirectory``` が見つからないというエラーです。メソッド名を大文字始まりに変更します。  
また、```Environment.DIRECTORY_PICTURES``` 定数も先頭大文字・大文字区切りに変更します。  
```cs
File mediaStorageDir = new File(Environment.GetExternalStoragePublicDirectory(
    Environment.DirectoryPictures), "CameraSample");
```

## mediaStorageDir.Exists()、mediaStorageDir.Mkdirs() への書き換え
次は
```java
if (!mediaStorageDir.exists())
{
    if (!mediaStorageDir.mkdirs())
```
の ```exists``` が見つからないというエラーです。メソッド名を大文字始まりに変更します。  
```mkdirs``` も同じように変更します。
```cs
if (!mediaStorageDir.Exists())
{
    if (!mediaStorageDir.Mkdirs())
```

## using Android.Util の追加
次は
```java
Log.d("CameraSample", "failed to create directory");
```
の ```Log``` が見つからないというエラーです。  
ファイルの先頭に
```cs
using Android.Util;
```
を追加します。
```using``` は Mac の場合は ```option+enter```、Windows の場合は ```ctrl+.``` から自動で追加することもできます。  
また、```d``` メソッドは Xamarin では ```Debug``` になっているので変更します。
```cs
Log.Debug("CameraSample", "failed to create directory");
```
  
## using Java.Text、using Java.Util の追加
次は
```java
String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss", Locale.US).format(new Date());
```
の ```SimpleDateFormat```、```Locale```、```Date``` が見つからないというエラーです。  
ファイルの先頭に
```cs
using Java.Text;
using Java.Util;
```
を追加します。
```using``` は Mac の場合は ```option+enter```、Windows の場合は ```ctrl+.``` から自動で追加することもできます。  
本来であればここは C# の ```DateTime``` 構造体など変更するべきですが、ここでは手数を少なく書き換えのミスを減らすために、Java から移植されたクラスを使用します。  
また、```US``` 定数は Xamarin では ```Us```、```format``` メソッドは先頭が大文字になっているので変更します。
```cs
String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss", Locale.Us).Format(new Date());
```

## mediaStorageDir.Path、File.Separator への書き換え
次は
```java
mediaFile = new File(mediaStorageDir.getPath() + File.separator +
```
の ```getPath```、```separator``` が見つからないというエラーです。これまでのように、プロパティへの変更と、先頭大文字への変更を行います。
```cs
mediaFile = new File(mediaStorageDir.Path + File.Separator +
```
  
同じようにすぐ下の
```java
mediaFile = new File(mediaStorageDir.getPath() + File.separator +
```
も
```cs
mediaFile = new File(mediaStorageDir.Path + File.Separator +
```
と変更します。

# CameraHelper クラスの完成
これで ```CameraHelper``` クラスは完成です。  
ビルドを行いエラーが出ないことを確認してください。(警告は大量に出ますが、今回はそのままにします)  
エラーが出た場合は、エラーの内容と上記の書き換えを見比べて頑張ってエラーに対処してください。  
  
[< 前ページ](./textbook1.md) | [次ページ >](./textbook3.md)  
