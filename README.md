# JINS-MEME-Unity-Plugin

JINS MEME SDKのUnity Pluginです。
JINS MEME SDK 1.0.6とUnity5.3.1で開発しています。

# 利用方法

- JINS-MEME-Unity-Plugin/MEMEUnity/Assets/Editor/Files/　に　MEMELib.framework　を配置してください。
- AppMEMEProxy.csの先頭部分にAppClientIdとAppClientSecretを指定するconstがありますので取得済みの情報を設定してください。
- iOSで書き出します
- Xcodeで書き出したプロジェクトを開きビルドターゲット"Unity-iPhone"のBuild Phases-Copy FilesにXCodeプロジェクト内にあるMEMELib.frameworkをDestination"Frameworks"を指定して追加してください、

![xcodesetting](https://raw.githubusercontent.com/SystemFriend/JINS-MEME-Unity-Plugin/master/XcodeSetting.png "Xcodeプロジェクト設定")
