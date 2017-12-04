# Xamarin.Android ハンズオン
Android のサンプルアプリを Xamarin.Android に書き換えることで Xamarin.Android を体験します。  
  
# このハンズオンで体験できること
・簡単な作りのアプリであれば、Android (Java) のソースコードをほとんど変更せず Xamarin.Android へ書き換えられます。  
・書き換えを体験するとこで「API の薄いラッパー」「アプリ開発経験者は経験が活かせる」ことが理解できると思います。  
・書き換えを体験することで Xamarin への敷居を下げることを目的にしています。Xamarin でのベストプラクティスをご提案するものでははありません。  
  
# 事前準備
Xamarin.Android の開発環境を整えてください。  
Xamarin.Android は Windows でも Mac でも開発できます。  
**[余裕があれば]**  
Android のネイティブ開発環境もあるとベストです。Xamarin.Android への書き換え前に一度、サンプルを動作できるので理解の助けになると思います。  
  
# 今回作るアプリ
Android の公式サンプルアプリ「[MediaRecorder](https://github.com/googlesamples/android-MediaRecorder/)」を Xamarin に書き換えます。  
下記より、サンプルプロジェクトをダウンロードしてください。  
https://github.com/googlesamples/android-MediaRecorder/

# 手順書
１．[プロジェクトの新規作成・UI の移植・Activity の書き換え・アイコンの設定](./textbook1.md)  
２．[CameraHelper クラスの書き換え](./textbook2.md)  
３．[MediaCodecWrapper クラス書き換え](./textbook3.md)  
４．[MainActivity クラス書き換え・パーミッション設定](./textbook4.md)  
  
※もし手順書通りに作業してもうまくいかない場合、参考にしてください。  
[[書き換え済みプロジェクト例](./Sample/)]