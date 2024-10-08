[![.NET Core Desktop](https://github.com/NaoakiUmedu/BookManager/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/NaoakiUmedu/BookManager/actions/workflows/dotnet-desktop.yml)
# 蔵書管理用デスクトップアプリケーション
## 画面イメージ
![using image](Doc/動いてるとこ.png)

## できること
+ 蔵書の<strong>「現在の配置場所」</strong>と<strong>「読み終わったら戻す段ボール」</strong>の両方を管理(この機能があるアプリが見つからなかったため作った)
+ 蔵書の題名・著者・ジャンルの情報を管理
+ 上記の情報を一覧表示
+ 題名で検索(他の列での検索は工事中...)
+ 各情報で蔵書をソート(WPFで表つくったら勝手に入ってるやつ)
+ TSVインポート・エクスポート機能(元々Excelで管理していたデータを取り込むため)

## 重視したこと
+ <strong>これが完成しないといつまでたっても本の整理ができない</strong>ため、開発速度を重視した
+ テスト駆動開発をやってみたかったため、やってみた
+ GitHub Actionsをやってみたかったため、やってみた
+ クリーンアーキテクチャ的な依存関係の整理をやってみたかったため、やってみた
+ SQLiteを試してみたかったため、やってみた

## 重視しなかったこと
+ メインPC上でのみ動けばよいと考えたためデスクトップアプリケーションとした<br>
  (後に別室のPCとデータ連携したくなったためWebアプリへの移行を計画中)
+ 書籍の件数は2024年9月現在で932冊程度のため、将来的に10000冊を超えるような事態は考えづらい<br>
  データ量を鑑み、処理効率はあまり考慮しなかった
+ 上記2点により、データベースアクセスは並行処理を想定せず作った
+ 想定利用者は作者1名のため、ユーザ等の概念はない

## 中身の構造
![class image](Doc/out/classes/classes.png)
