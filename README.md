# UniTwitchClient
 - UniTwitchClientは[Twitch.tv](https://www.twitch.tv/)の様々なサービスへのUnity上からのアクセスをサポートするライブラリです。
 - Unity上でTwitchチャンネルのチャットコメントの取得や、フォローやサブスクライブ等の通知の取得が可能です。

# 対応機能
 - Twitch IRC（チャットコメント）のメッセージ取得
 - Twitch EventSubによるフォローやサブスクライブ、チアーといったイベント通知の取得

# 導入手順
1. UniRx, UniTask, Newtonsoft.Json-for-UnityをUPMでインストールする。
2. UniTwitchClientをadd package from git URLでインストール(https://github.com/mojopon/UniTwitchClient.git?path=Assets/UniTwitchClient)

# 依存ライブラリ
UniTwitchClient使用にあたって、次のライブラリの別途導入が必要です。
 - UniRx(https://github.com/neuecc/UniRx)
 - UniTask(https://github.com/Cysharp/UniTask)
 - Newtonsoft.Json-for-Unity(https://github.com/applejag/Newtonsoft.Json-for-Unity)

# 権利表記
UniRx Copyright (c) 2014 Yoshifumi Kawai https://github.com/neuecc/UniRx/blob/master/LICENSE

UniTask Copyright (c) 2019 Yoshifumi Kawai / Cysharp, Inc. https://github.com/Cysharp/UniTask/blob/master/LICENSE

Newtonsoft.Json Copyright (c) 2019 Kalle Jillheden (jilleJr) https://github.com/applejag/Newtonsoft.Json-for-Unity/blob/master/LICENSE.md

websocket-sharp Copyright (c) 2010-2018 sta.blockhead https://github.com/sta/websocket-sharp/blob/master/LICENSE.txt
