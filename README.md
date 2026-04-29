# Five Card Poker with Betting

## 專案簡介

本專案實作一個五張撲克牌遊戲，並加入下注（Betting）機制。
玩家可進行發牌、換牌，並依據最終牌型計算獎金，系統會自動更新資金狀態。

此專案著重於：

* 撲克牌資料結構設計
* 牌型判斷演算法
* 下注與獎金計算邏輯
* 視窗互動操作

---

## 功能說明

### 遊戲流程

1. 玩家輸入押注金額
2. 點擊下注開始遊戲
3. 系統發出五張牌
4. 玩家可選擇換牌
5. 系統判斷最終牌型並計算結果
6. 更新玩家總資金

---

### 核心功能

#### 1. 發牌系統

* 隨機從牌堆中抽取五張不重複的牌
* 支援重新發牌與換牌

#### 2. 牌型判斷

支援以下撲克牌型：

* Royal Flush
* Straight Flush
* Four of a Kind
* Full House
* Flush
* Straight
* Three of a Kind
* Two Pair
* One Pair

#### 3. 下注系統

* 玩家可自訂押注金額
* 系統檢查資金是否足夠
* 每局遊戲會扣除押注金額

#### 4. 獎金計算

根據牌型套用對應賠率：

| 牌型              | 賠率  |
| --------------- | --- |
| Royal Flush     | 250 |
| Straight Flush  | 50  |
| Four of a Kind  | 25  |
| Full House      | 9   |
| Flush           | 6   |
| Straight        | 4   |
| Three of a Kind | 3   |
| Two Pair        | 2   |
| One Pair        | 1   |

計算方式：

```id="calc01"
payout = bet × rate
```

---

## 程式設計說明

### 類別設計

#### Card

表示單一撲克牌：

* 花色（Suit）
* 點數（Rank）

#### Deck

* 管理整副牌（52 張）
* 提供洗牌與抽牌功能

#### PokerLogic

* 負責牌型判斷
* 回傳牌型與對應賠率

#### Game / Form

* 控制遊戲流程
* 管理資金與下注
* 處理 UI 事件

---

## 執行方式

### 使用 Visual Studio

1. 開啟 `.sln` 檔案
2. 按 F5 執行
3. 輸入押注金額後開始遊戲

---

## 專案結構

```id="struct01"
PokerProject/
│── Form1.cs        // UI 與事件控制
│── Program.cs      // 程式進入點
│── Card.cs         // 撲克牌結構
│── Deck.cs         // 牌堆管理
│── PokerLogic.cs   // 牌型判斷
│── Resources/      // 圖片資源
│── README.md
```

---



## 作者

田秉立
