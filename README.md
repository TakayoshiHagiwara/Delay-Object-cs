# Delay-Object-cs
<img src="https://img.shields.io/badge/Unity-2017 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">


Unityで、あるGameObjectの動きを遅延させて、別のGameObjectに反映させるサンプルです。
アバターなどの階層構造を持つGameObjectも対応可能です。

# Environment
- Unity 2017 or Later
    - 2021での動作確認済み

# Installation
- 特に追加で必要なアセット、ライブラリはありません。

# Usage

1. Unityを開き、AssetsフォルダにDelayObject.csを読み込む
1. Hierarchyに空のGameobjectを作成し、DelayObject.csをアタッチする
1. InspectorのMaster Rootに動きの元になるオブジェクトを、Slave Rootに遅延させた動きを反映させたいオブジェクトをアタッチする (2つのオブジェクトの名前、階層構造は一致させる)
1. Delayに遅延時間 (秒) を入力する
1. 実行し、Masterに指定したオブジェクトを動かしたときに、遅れてSlaveに指定したオブジェクトが動く

![InspectorDelayObject](https://user-images.githubusercontent.com/85339315/163533874-e1c4d4fd-0661-4497-bf94-cf70940ed9db.png)

# Description
- Dictionaryを用いてオブジェクトのTransformを一時保存し、別のオブジェクトに適用するという処理を行なっています。
    - 実行時にMaster Rootに指定したオブジェクトの名前と階層構造をもとにDictionaryを初期化します。

## DelayObject.cs
### Parameters
- masterRoot: Transform
    - 動きのもとになるオブジェクトです。
- slaveRoot: Transform
    - 遅延した動きを反映するオブジェクトです。
- delay: float
    - 遅延時間です。単位は秒です。

### Methods
- RecordParameter
    - MasterのTransformをDictionaryに一時保存します。
- ApplyParameter
    - 記録されたTransformを指定したオブジェクトに反映させます。
- InitializeDictionary
    - Dictionaryを初期化します。
- CheckCurrentTime
    - 実行時から現在までの経過時間を確認し、delayを超えた場合、SlaveにTransformを反映させるフラグをtrueにします。
- ResetAll
    - Dictionaryと経過時間を初期化します。
    - 実行後に途中でdelayを変更した場合などに使用します。

### Note
- Master RootとSlave RootにアタッチするGameObjectは必ず同じ名前、同じ階層構造にしてください。
- 外部から操作する場合は、外部で経過時間やトリガーを設定し、RecordParameter、ApplyParameterを呼び出してください。


# Versions
- 1.0: 2019/8/27
- 1.1: 2022/4/15
    - Unity 2021にて動作確認

# Author
- Takayoshi Hagiwara
    - Graduate School of Media Design, Keio University
    - Toyohashi University of Technology


# License
- MIT License