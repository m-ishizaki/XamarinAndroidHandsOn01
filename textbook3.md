[< 前ページ](./textbook2.md) | [次ページ >](./textbook4.md)  

# MediaCodecWrapper クラスの作成
```com.example.android.common.media.MediaCodecWrapper``` クラスを書き換えていきます。  
  
## Mac の場合
・ソリューションエクスプローラーの太字になっているプロジェクト名で右クリック (二本指タップ) し ```追加 > 新しいファイル```
を選択します。  
・[新しいファイル]ダイアログで ```General > 空のクラス``` を選択し、[名前] に 「MediaCodecWrapper」 を入力します。  
・[新規]ボタンをクリックします。  
  
## Windows の場合
・ソリューションエクスプローラーの太字になっているプロジェクト名で右クリック (二本指タップ) し ```追加 > 新しい項目``` を選択します。  
・[新しい項目の追加]ダイアログで ```Visual C# > クラス``` を選択し、[名前] に 「MediaCodecWrapper」 を入力します。  
・[新規]ボタンをクリックします。  

# MediaCodecWrapper クラスの移植
上記で作成したクラスのソースのクラス宣言は次のようになっているはずです。  
```cs
public class MediaCodecWrapper
{
    public MediaCodecWrapper()
    {
    }
}
```
  
## ソースコードの コピー & ペースト
Java の ```MediaCodecWrapper``` クラスの package、import を除いた
```java
public class MediaCodecWrapper {

...
中略
...

}
```
で C# の ```MediaCodecWrapper``` クラスの using、namespace を除いた
```cs
    public class MediaCodecWrapper
    {
        public MediaCodecWrapper()
        {
        }
    }
```
を上書きします。  


## using の追加
前回の ```CameraHelper``` クラスでは、1 つずつ確認しながら ```using``` を追加しました。  
今回は必要な ```using``` は最初に追加してしまいます。
```cs
using System.Linq;
using Android.OS;
using Android.Media;
using System.Collections.Generic;
using Java.Nio;
using Java.Lang;
using Java.Util;
using Android.Views;
```
を追加します。最初からある ```using System;``` と合わせて次のようになります。
```cs
using System;
using System.Linq;
using Android.OS;
using Android.Media;
using System.Collections.Generic;
using Java.Nio;
using Java.Lang;
using Java.Util;
using Android.Views;
```

## メソッド名の変更
Java ではメソッド名は小文字で始まりますが、C# では大文字で始まります。  
小文字で始まっているメソッド名を、大文字始まりに一気に変更して行きます。  
  
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```codec.start()``` → ```codec.Start()```  
・```mDecoder.stop()``` → ```mDecoder.Stop()```  
・```mDecoder.release()``` → ```mDecoder.Release()```  
・```codec.getInputBuffers()``` → ```codec.GetInputBuffers()```  
・```codec.getOutputBuffers()``` → ```codec.GetOutputBuffers()```  
・```Looper.myLooper()``` → ```Looper.MyLooper()```  
・```trackFormat.getString(``` → ```trackFormat.GetString(```  
・```mimeType.contains(``` → ```mimeType.Contains(```  
・```MediaCodec.createDecoderByType(``` → ```MediaCodec.CreateDecoderByType(```  
・```videoCodec.configure(``` → ```videoCodec.Configure(```  
・```input.remaining()``` → ```input.Remaining()```  
・```buffer.capacity()``` → ```buffer.Capacity()```  
・```buffer.clear()``` → ```buffer.Clear()```  
・```buffer.put(``` → ```buffer.Put(```  
・```mDecoder.queueInputBuffer(``` → ```mDecoder.QueueInputBuffer(```  
・```mDecoder.queueSecureInputBuffer(``` → ```mDecoder.QueueSecureInputBuffer(```  
・```extractor.readSampleData(``` → ```extractor.ReadSampleData(```  
・```extractor.getSampleCryptoInfo(``` → ```extractor.GetSampleCryptoInfo(```  
・```out_bufferInfo.set(``` → ```out_bufferInfo.Set(```  
・```mDecoder.releaseOutputBuffer(``` → ```mDecoder.ReleaseOutputBuffer(```  
・```mDecoder.dequeueInputBuffer(``` → ```mDecoder.DequeueInputBuffer(```  
・```mDecoder.dequeueOutputBuffer(``` → ```mDecoder.DequeueOutputBuffer(```  
・```mDecoder.getOutputBuffers()``` → ```mDecoder.GetOutputBuffers()```  
・```mHandler.post(``` → ```mHandler.Post(```  

## プロパティ名の変更
Java では公開データメンバは小文字で始まりますが、C# ではプロパティになり、大文字始まりになります。  
小文字で始まっているプロパティ名を、大文字始まりに一気に変更して行きます。 
  
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```mOutputBuffers.length``` → ```mOutputBuffers.Length```  
・```mInputBuffers.length``` → ```mInputBuffers.Length```  
・```info.offset``` → ```info.Offset```  
・```info.size``` → ```info.Size```  
・```info.presentationTimeUs``` → ```info.PresentationTimeUs```  
・```info.flags``` → ```info.Flags```  
・```Locale.US``` → ```Locale.Us```  

## 定数名の変更
Java では公開フォールドはすべて大文字・_区切りですが、C# では大文字始まり大文字区切りになります。  

置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```MediaFormat.KEY_MIME``` → ```MediaFormat.KeyMime```  

## 列挙体への変更
Java では公開フォールドはなるべく列挙体に変更されています。

置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```MediaCodec.BUFFER_FLAG_END_OF_STREAM``` → ```(int)MediaCodec.BufferFlagEndOfStream```  
・```, flags)``` → ```, (MediaCodecBufferFlags)flags)```  
・```MediaCodec.INFO_TRY_AGAIN_LATER``` → ```(int)MediaCodec.InfoTryAgainLater```  
・```MediaCodec.INFO_OUTPUT_BUFFERS_CHANGED``` → ```(int)MediaCodec.InfoOutputBuffersChanged```  
・```MediaCodec.INFO_OUTPUT_FORMAT_CHANGED``` → ```(int)MediaCodec.InfoOutputFormatChanged```  
  
## final ローカル変数の変更
Java では変更不可のローカル変数の定義に ```final``` キーワードを使用しますが、C# では ```const``` になります。  
さらに C# では文字列がリテラルでなければなりません。そのため、ここでは ```const``` が使用できません。```final``` の指定は削除します。
  
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```final String mimeType = ``` → ```string mimeType = ```

## final 引数の変更
Java ではメソッドの引数にはなるべく ```final``` を付けますが、C# では同等の機能はありません。  
引数の ```final``` を削除します。  
   
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```final ``` → ``` ```(空白)  

## Queue<T> への変更
Java の ```ArrayDeque<>``` クラスは Xamarin に移植されていません。代わりに C# の ```Queue<T>``` クラスを使用します。  
  
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```Queue<Integer>``` → ```Queue<int>```  
・```ArrayDeque<>``` → ```Queue<int>```  
  
また、クラスが変わりのでメソッドも変わります。```Queue<T>``` の同等のメソッドを使用するよう変更します。  
  
・```!mAvailableInputBuffers.isEmpty()``` → ```mAvailableInputBuffers.Any()```  
・```mAvailableInputBuffers.remove()``` → ```mAvailableInputBuffers.Dequeue()```  
・```!mAvailableOutputBuffers.isEmpty()``` → ```mAvailableOutputBuffers.Any()```  
・```mAvailableOutputBuffers.peek()``` → ```mAvailableOutputBuffers.Peek()```  
・```mAvailableOutputBuffers.remove()``` → ```mAvailableOutputBuffers.Dequeue()```  
・```mAvailableInputBuffers.add(``` → ```mAvailableInputBuffers.Enqueue(```  
・```mAvailableOutputBuffers.clear()``` → ```mAvailableOutputBuffers.Clear()```  
・```mAvailableOutputBuffers.add(``` → ```mAvailableOutputBuffers.Enqueue(```  
  
## throws の削除
Java のメソッドが返す場合、返す例外の種類を ```throws``` で宣言しますが、C# では同等の機能はありません。  
```throws``` の宣言を削除します。  
   
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```throws IOException``` → ``` ```(空白)  
・```throws MediaCodec.CryptoException, WriteException``` → ``` ```(空白)  

## bool への変更
Java では真偽値型は ```boolean``` キーワードですが、C# では ```bool``` になります。  
   
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```boolean``` → ```bool```  

## Java.Lang.String の使用
Xamarin にする場合、文字列は C# の ```string``` を使用するべきですが、Java の ```String``` も使用できます。  
今回は Java の ```String``` のメソッドが使用されている箇所は、手数を少なく書き換えのミスを減らすために、Java から移植されたクラスを使用します。  
  
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```String.format(``` → ```Java.Lang.String.Format(```  
  
## プロパティへの変更
Java の getter メソッドはなるべく、プロパティに変更されています。  
   
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```mDecoder.getOutputFormat()``` → ```mDecoder.OutputFormat```  

## ラムダ式への変更
Java でのイベントリスナーの匿名クラスは、C# ではラムダ式で実装します。  
ラムダ式を使用すると匿名メソッドを短い手数で記述することができます。  

Java の実装
```java
mHandler.post(new Runnable() {
    @Override
    public void run()
    {
        mOutputFormatChangedListener
        .outputFormatChanged(MediaCodecWrapper.this,
            mDecoder.getOutputFormat());

    }
});
```
C# での実装
```cs
mHandler.Post(new Runnable(() =>
{
    mOutputFormatChangedListener
    .outputFormatChanged(this,
        mDecoder.OutputFormat);

}));
```
 
## クラス継承のキーワードの変更
Java ではクラスの継承は ```extends``` キーワードですが、C# では ```:``` になります。  
   
置換は Mac の場合は ```option+command+F```、Windows の場合は ```ctrl+H``` で行えます。  
  
・```extends``` → ```:```  

## コンストラクタの変更
Java ではコンストラクタ内でスーパークラスのコンストラクタを呼ぶキーワードは ```super``` ですが、C# では ```base``` になります。  
また、記述する場所を変更になります。  

Java の実装
```java
private WriteException(String detailMessage)
{
    super(detailMessage);
}
```
C# での実装
```cs
public WriteException(string detailMessage) : base(detailMessage)
{
}
```
 アクセス修飾子が ```public``` になり、引数の型が ```string``` になっている点に注意してください。  

 # MediaCodecWrapper クラスの完成
これで ```MediaCodecWrapper``` クラスは完成です。  
ビルドを行いエラーが出ないことを確認してください。(警告は大量に出ますが、今回はそのままにします)  
エラーが出た場合は、エラーの内容と上記の書き換えを見比べて頑張ってエラーに対処してください。  
  
[< 前ページ](./textbook2.md) | [次ページ >](./textbook4.md)  
