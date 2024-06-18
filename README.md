# UniTwitchClient
 - UniTwitchClientは[Twitch.tv](https://www.twitch.tv/)の様々なサービスへのUnity上からのアクセスをサポートするライブラリです。
 - Unity上でTwitchチャンネルのチャットコメントの取得や、フォローやサブスクライブ等の通知の取得が可能です。

# 対応機能
 - Twitch IRC（チャットコメント）のメッセージ取得
 - Twitch EventSubによるフォローやサブスクライブ、チアーといったイベント通知の取得

# 導入方法
UPMでインストールが可能です。add package by git URLで下記URLを入力してください。

```https://github.com/mojopon/UniTwitchClient.git?path=Assets/UniTwitchClient```

# 依存ライブラリ
UniTwitchClient使用にあたって、次のライブラリのUPMでのインストールが必要です。
 - UniRx(https://github.com/neuecc/UniRx)
 - UniTask(https://github.com/Cysharp/UniTask)
 - Newtonsoft.Json-for-Unity(https://github.com/applejag/Newtonsoft.Json-for-Unity)

# 使い方
## コメントを取得
```
string accessToken = "アクセストークンを入力";
string userName = "Twitchユーザーネームを入力";
string channelName = "コメントを取得するチャンネル名を入力";

// TwitchChatClientインスタンスを生成
var twitchChatClient = new TwitchChatClient(new TwitchIrcCredentials(accessToken, userName));

// TwitchChatMessageAsObservableでメッセージが通知される。
// ユーザーからのメッセージはCommandがPrivMsgとなる。
// Where句でフィルタリングしてユーザーからのメッセージのみ取得（入室時等のシステムメッセージを除外する）
twitchChatClient.TwitchChatMessageAsObservable
                .Where(x => x.Command == TwitchIrcCommand.PrivMsg)
                .Subscribe(x => Debug.Log($"Name:{x.DisplayName}({x.UserNickname}), Message:{x.Message}"));

// Connectで対象チャンネルに接続。
// TwitchChatMessageAsObservableにメッセージが流れる。
twitchChatClient.Connect(channelName);
```
使い終わったらClose()してDispose()してください。

# 権利表記
UniRx Copyright (c) 2014 Yoshifumi Kawai https://github.com/neuecc/UniRx/blob/master/LICENSE

UniTask Copyright (c) 2019 Yoshifumi Kawai / Cysharp, Inc. https://github.com/Cysharp/UniTask/blob/master/LICENSE

Newtonsoft.Json Copyright (c) 2019 Kalle Jillheden (jilleJr) https://github.com/applejag/Newtonsoft.Json-for-Unity/blob/master/LICENSE.md

websocket-sharp Copyright (c) 2010-2018 sta.blockhead https://github.com/sta/websocket-sharp/blob/master/LICENSE.txt
