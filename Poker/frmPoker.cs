using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poker
{
    public partial class frmPoker : Form
    {
        #region 欄位
        /// <summary>
        /// 用來存放牌桌上五張牌的 PictureBox 陣列
        /// </summary>
        PictureBox[] pic = new PictureBox[5];

        /// <summary>
        /// 所有的牌的編號，從 0 到 51，對應到 52 張牌
        /// </summary>
        int[] allPoker = new int[52];

        /// <summary>
        /// 記錄玩家手牌的編號，從 0 到 51，對應到 52 張牌
        /// </summary>
        int[] playerPoker = new int[5];

        /// <summary>
        /// 玩家目前總資金
        /// </summary>
        int totalMoney = 1000000;

        /// <summary>
        /// 本局押注金額
        /// </summary>
        int currentBet = 0;

        /// <summary>
        /// 是否已完成本局下注
        /// </summary>
        bool hasBet = false;
        #endregion

        public frmPoker()
        {
            InitializeComponent();
            InitializePoker();
            UpdateMoneyText();
        }

        #region 自定義方法
        private void InitializePoker()
        {
            for (int i = 0; i < pic.Length; i++)
            {
                pic[i] = new PictureBox();
                pic[i].Image = GetImage("back");
                pic[i].Name = "pic" + i;
                pic[i].SizeMode = PictureBoxSizeMode.AutoSize;
                pic[i].Top = 30;
                pic[i].Left = 10 + ((pic[i].Width + 10) * i);
                pic[i].Enabled = false;
                pic[i].Tag = "back";
                pic[i].Visible = true;
                this.grpPoker.Controls.Add(pic[i]);
                pic[i].Click += Pic_Click;
            }
        }

        /// <summary>
        /// 將目前總資金顯示在畫面上
        /// </summary>
        private void UpdateMoneyText()
        {
            txtMoney.Text = totalMoney.ToString();
        }

        /// <summary>
        /// 開始新一局時，清除下注狀態並開放重新下注
        /// </summary>
        private void ResetBetForNextRound()
        {
            currentBet = 0;
            hasBet = false;
            txtBet.Enabled = true;
            btnBet.Enabled = true;
            btnDealCard.Enabled = false;
        }

        /// <summary>
        /// 顯示五張撲克牌到桌面上
        /// </summary>
        private void ShowCards()
        {
            for (int i = 0; i < playerPoker.Length; i++)
            {
                pic[i].Image = this.GetImage($"pic{playerPoker[i] + 1}");
            }
        }

        /// <summary>
        /// 取得圖片資源
        /// </summary>
        private Image GetImage(string name)
        {
            return Properties.Resources.ResourceManager.GetObject(name) as Image;
        }

        /// <summary>
        /// 取得圖片資源
        /// </summary>
        private Image GetImage(int num)
        {
            return GetImage($"pic{num}");
        }

        /// <summary>
        /// 將 allPoker 陣列中的牌隨機打亂，模擬洗牌的過程
        /// </summary>
        private void Shuffle()
        {
            Random rand = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int r = rand.Next(allPoker.Length);
                int temp = allPoker[r];
                allPoker[r] = allPoker[0];
                allPoker[0] = temp;
            }
        }

        /// <summary>
        /// 判斷牌型並回傳牌型名稱與賠率
        /// </summary>
        private string GetHandResult(out int odds)
        {
            string[] colorList = { "梅花", "方塊", "愛心", "黑桃" };
            string[] pointList = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

            int[] pokerColor = new int[5];
            int[] pokerPoint = new int[5];

            for (int i = 0; i < playerPoker.Length; i++)
            {
                pokerColor[i] = playerPoker[i] % 4;
                pokerPoint[i] = playerPoker[i] / 4;
            }

            int[] colorCount = new int[4];
            int[] pointCount = new int[13];

            for (int i = 0; i < pokerColor.Length; i++)
            {
                colorCount[pokerColor[i]]++;
                pointCount[pokerPoint[i]]++;
            }

            Array.Sort(colorCount, colorList);
            Array.Reverse(colorCount);
            Array.Reverse(colorList);

            Array.Sort(pointCount, pointList);
            Array.Reverse(pointCount);
            Array.Reverse(pointList);

            bool isFlush = (colorCount[0] == 5);
            bool isSingle = (pointCount[0] == 1 && pointCount[1] == 1 && pointCount[2] == 1 && pointCount[3] == 1 && pointCount[4] == 1);
            bool isDiffFour = (pokerPoint.Max() - pokerPoint.Min() == 4);
            bool isRoyal = pokerPoint.Contains(0) && pokerPoint.Contains(9) && pokerPoint.Contains(10) && pokerPoint.Contains(11) && pokerPoint.Contains(12);
            bool isRoyalFlush = isFlush && isRoyal;
            bool isStraightFlush = isFlush && isSingle && isDiffFour;
            bool isStraight = isSingle && (isDiffFour || isRoyal);
            bool isFourOfAKind = (pointCount[0] == 4);
            bool isFullHouse = (pointCount[0] == 3 && pointCount[1] == 2);
            bool isThreeOfAKind = (pointCount[0] == 3 && pointCount[1] == 1);
            bool isTwoPair = (pointCount[0] == 2 && pointCount[1] == 2);
            bool isOnePair = (pointCount[0] == 2 && pointCount[1] == 1);

            if (isRoyalFlush)
            {
                odds = 250;
                return $"{colorList[0]} 皇家同花順";
            }
            else if (isStraightFlush)
            {
                odds = 50;
                return $"{colorList[0]} 同花順";
            }
            else if (isFourOfAKind)
            {
                odds = 25;
                return $"{pointList[0]} 四條";
            }
            else if (isFullHouse)
            {
                odds = 9;
                return $"{pointList[0]}三張{pointList[1]}兩張 葫蘆";
            }
            else if (isFlush)
            {
                odds = 6;
                return $"{colorList[0]} 同花";
            }
            else if (isStraight)
            {
                odds = 4;
                return "順子";
            }
            else if (isThreeOfAKind)
            {
                odds = 3;
                return $"{pointList[0]} 三條";
            }
            else if (isTwoPair)
            {
                odds = 2;
                return $"{pointList[0]},{pointList[1]} 兩對";
            }
            else if (isOnePair)
            {
                odds = 1;
                return $"{pointList[0]} 一對";
            }
            else
            {
                odds = 0;
                return "雜牌";
            }
        }
        #endregion

        #region 事件處理程序
        private void btnBet_Click(object sender, EventArgs e)
        {
            int bet;
            if (!int.TryParse(txtBet.Text.Trim(), out bet))
            {
                MessageBox.Show("請輸入正確的押注金額。", "下注錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (bet <= 0)
            {
                MessageBox.Show("押注金額必須大於 0。", "下注錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (bet > totalMoney)
            {
                MessageBox.Show("押注金額不可超過目前總資金。", "下注錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            currentBet = bet;
            totalMoney -= currentBet;
            hasBet = true;
            UpdateMoneyText();

            txtBet.Enabled = false;
            btnBet.Enabled = false;
            btnDealCard.Enabled = true;
            lblResult.Text = $"已下注 {currentBet} 元，請按發牌";
        }

        private void Pic_Click(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            int index = int.Parse(pic.Name.Replace("pic", ""));
            int cardNum = playerPoker[index] + 1;

            if (pic.Tag.ToString() == "back")
            {
                pic.Tag = "front";
                pic.Image = GetImage(cardNum);
            }
            else
            {
                pic.Tag = "back";
                pic.Image = GetImage("back");
            }
        }

        private async void btnDealCard_Click(object sender, EventArgs e)
        {
            if (!hasBet)
            {
                MessageBox.Show("請先下注後再發牌。", "尚未下注", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.lblResult.Text = "";

            for (int i = 0; i < pic.Length; i++)
            {
                pic[i].Image = GetImage("back");
                pic[i].Tag = "back";
                pic[i].Enabled = false;
            }

            for (int i = 0; i < allPoker.Length; i++)
            {
                allPoker[i] = i;
            }

            this.Shuffle();
            await Task.Delay(500);

            for (int i = 0; i < playerPoker.Length; i++)
            {
                playerPoker[i] = allPoker[i];
            }

            this.ShowCards();

            for (int i = 0; i < pic.Length; i++)
            {
                pic[i].Enabled = true;
                pic[i].Tag = "front";
            }

            btnChangeCard.Enabled = true;
            btnDealCard.Enabled = false;
        }

        private void btnChangeCard_Click(object sender, EventArgs e)
        {
            int startIndex = 5;

            for (int i = 0; i < playerPoker.Length; i++)
            {
                if (pic[i].Tag.ToString() == "back")
                {
                    playerPoker[i] = allPoker[startIndex];
                    pic[i].Image = GetImage(playerPoker[i] + 1);
                    pic[i].Tag = "front";
                    startIndex++;
                }
            }

            for (int i = 0; i < pic.Length; i++)
            {
                pic[i].Enabled = false;
            }

            this.btnChangeCard.Enabled = false;
            this.btnCheck.Enabled = true;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            int odds;
            string handName = GetHandResult(out odds);
            int prize = currentBet * odds;
            totalMoney += prize;
            UpdateMoneyText();

            lblResult.Text = $"{handName}  賠率:{odds}  中獎:{prize}";

            btnChangeCard.Enabled = false;
            btnCheck.Enabled = false;

            if (totalMoney <= 0)
            {
                MessageBox.Show("總資金已用完，遊戲結束。", "遊戲結束", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnDealCard.Enabled = false;
                btnBet.Enabled = false;
                txtBet.Enabled = false;
            }
            else
            {
                ResetBetForNextRound();
            }
        }

        private void frmPoker_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.btnDealCard.Enabled == false && hasBet)
            {
                switch (e.KeyChar)
                {
                    case 'q':
                        // 皇家同花順
                        playerPoker[0] = 51;
                        playerPoker[1] = 47;
                        playerPoker[2] = 43;
                        playerPoker[3] = 39;
                        playerPoker[4] = 3;
                        break;
                    case 'w':
                        // 同花順
                        playerPoker[0] = 37;
                        playerPoker[1] = 33;
                        playerPoker[2] = 29;
                        playerPoker[3] = 25;
                        playerPoker[4] = 21;
                        break;
                    case 'e':
                        // 同花
                        playerPoker[0] = 50;
                        playerPoker[1] = 38;
                        playerPoker[2] = 34;
                        playerPoker[3] = 22;
                        playerPoker[4] = 18;
                        break;
                    case 'r':
                        // 四條
                        playerPoker[0] = 48;
                        playerPoker[1] = 39;
                        playerPoker[2] = 38;
                        playerPoker[3] = 37;
                        playerPoker[4] = 36;
                        break;
                    case 't':
                        // 葫蘆
                        playerPoker[0] = 30;
                        playerPoker[1] = 29;
                        playerPoker[2] = 6;
                        playerPoker[3] = 5;
                        playerPoker[4] = 4;
                        break;
                    case 'y':
                        // 三條
                        playerPoker[0] = 48;
                        playerPoker[1] = 39;
                        playerPoker[2] = 15;
                        playerPoker[3] = 14;
                        playerPoker[4] = 13;
                        break;
                }

                this.ShowCards();
            }
        }
        #endregion
    }
}
