using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;


namespace NguyenVanDuy_XoSo
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private int currentRound = 0;
        private int tickCount = 0;
        private Random random;

        private Label[][] allLabels;
        private string historyFilePath = "database.txt"; // đổi tên 
        private List<string> lastResults = new List<string>();


        public Form1()
        {
            InitializeComponent();
            random = new Random();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += Timer_Tick;

            allLabels = new Label[][]
            {
                new Label[] { lbl_hcm_8, lbl_hcm_7, lbl_hcm_6_1, lbl_hcm_6_2, lbl_hcm_6_3, lbl_hcm_5, lbl_hcm_4_1, lbl_hcm_4_2, lbl_hcm_4_3, lbl_hcm_4_4, lbl_hcm_4_5, lbl_hcm_4_6, lbl_hcm_4_7, lbl_hcm_3_1, lbl_hcm_3_2, lbl_hcm_2, lbl_hcm_1, lbl_hcm_db },
                new Label[] { lbl_dt_8, lbl_dt_7, lbl_dt_6_1, lbl_dt_6_2, lbl_dt_6_3, lbl_dt_5, lbl_dt_4_1, lbl_dt_4_2, lbl_dt_4_3, lbl_dt_4_4, lbl_dt_4_5, lbl_dt_4_6, lbl_dt_4_7, lbl_dt_3_1, lbl_dt_3_2, lbl_dt_2, lbl_dt_1, lbl_dt_db },
                new Label[] { lbl_cm_8, lbl_cm_7, lbl_cm_6_1, lbl_cm_6_2, lbl_cm_6_3, lbl_cm_5, lbl_cm_4_1, lbl_cm_4_2, lbl_cm_4_3, lbl_cm_4_4, lbl_cm_4_5, lbl_cm_4_6, lbl_cm_4_7, lbl_cm_3_1, lbl_cm_3_2, lbl_cm_2, lbl_cm_1, lbl_cm_db }
            };

            DateTime currentDate = DateTime.Now;
            lblngay.Text = currentDate.ToString("dd/MM/yyyy HH:mm:ss");

            btnReset.Click += btnReset_Click;

        }

        private void playSimpleSound()
        {
            string staticPath = @"D:\University\Nam4_1\Lap_trinh_windowns\NguyenVanDuy_XoSo\Music\NhacSoXo.wav"; // Thay đổi đường dẫn
            string dynamicPath = System.IO.Path.Combine(Application.StartupPath, "Resources", "NhacSoXo.wav");

            string musicPath = System.IO.File.Exists(staticPath) ? staticPath : dynamicPath;

            try
            {
                SoundPlayer simpleSound = new SoundPlayer(musicPath);
                simpleSound.Load(); 
                simpleSound.PlayLooping(); 
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("File âm thanh không tìm thấy: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi phát âm thanh: " + ex.Message);
            }
        }


        private void btn_thucong_Click(object sender, EventArgs e)
        {
            string enteredSo = txtbdoso.Text.Trim();

            if (string.IsNullOrEmpty(enteredSo))
            {
                lblnhapso.Text = "Vui lòng nhập số cần dò";
                return;
            }

            if (enteredSo.Length < 2)
            {
                lblnhapso.Text = "Vui lòng nhập 2 số trở lên";
                return;
            }
            else if (enteredSo.Length > 6)
            {
                lblnhapso.Text = "Vui lòng nhập tối đa 6 số";
                return;
            }

            if (!long.TryParse(enteredSo, out _))
            {
                lblnhapso.Text = "Số cần dò chỉ được chứa ký tự số";
                return;
            }

            lblnhapso.Text = "";

            if (dtp_ngaydo.Value.Date > DateTime.Now.Date)
            {
                lblngaydo.Text = "Vui lòng chọn ngày dò hợp lệ";
                return;
            }

            lblngaydo.Text = "";

            if (cbb_giave.SelectedItem == null)
            {
                lblgiave.Text = "Vui lòng chọn giá vé";
                return;
            }

            lblgiave.Text = "";

            if (cbb_daiquay.SelectedItem == null)
            {
                lbldaiquay.Text = "Vui lòng chọn đài quay";
                return;
            }

            lbldaiquay.Text = "";

            currentRound = 0;
            tickCount = 0;

            string selectedDai = cbb_daiquay.SelectedItem.ToString();

            switch (selectedDai)
            {
                case "TP. HCM":
                    allLabels = new Label[][] { new Label[] { lbl_hcm_8, lbl_hcm_7, lbl_hcm_6_1, lbl_hcm_6_2, lbl_hcm_6_3, lbl_hcm_5, lbl_hcm_4_1, lbl_hcm_4_2, lbl_hcm_4_3, lbl_hcm_4_4, lbl_hcm_4_5, lbl_hcm_4_6, lbl_hcm_4_7, lbl_hcm_3_1, lbl_hcm_3_2, lbl_hcm_2, lbl_hcm_1, lbl_hcm_db } };
                    break;

                case "ĐỒNG THÁP":
                    allLabels = new Label[][] { new Label[] { lbl_dt_8, lbl_dt_7, lbl_dt_6_1, lbl_dt_6_2, lbl_dt_6_3, lbl_dt_5, lbl_dt_4_1, lbl_dt_4_2, lbl_dt_4_3, lbl_dt_4_4, lbl_dt_4_5, lbl_dt_4_6, lbl_dt_4_7, lbl_dt_3_1, lbl_dt_3_2, lbl_dt_2, lbl_dt_1, lbl_dt_db } };
                    break;

                case "CÀ MAU":
                    allLabels = new Label[][] { new Label[] { lbl_cm_8, lbl_cm_7, lbl_cm_6_1, lbl_cm_6_2, lbl_cm_6_3, lbl_cm_5, lbl_cm_4_1, lbl_cm_4_2, lbl_cm_4_3, lbl_cm_4_4, lbl_cm_4_5, lbl_cm_4_6, lbl_cm_4_7, lbl_cm_3_1, lbl_cm_3_2, lbl_cm_2, lbl_cm_1, lbl_cm_db } };
                    break;

                case "CẢ 3 ĐÀI":
                    allLabels = new Label[][]
                    {
                new Label[] { lbl_hcm_8, lbl_hcm_7, lbl_hcm_6_1, lbl_hcm_6_2, lbl_hcm_6_3, lbl_hcm_5, lbl_hcm_4_1, lbl_hcm_4_2, lbl_hcm_4_3, lbl_hcm_4_4, lbl_hcm_4_5, lbl_hcm_4_6, lbl_hcm_4_7, lbl_hcm_3_1, lbl_hcm_3_2, lbl_hcm_2, lbl_hcm_1, lbl_hcm_db },
                new Label[] { lbl_dt_8, lbl_dt_7, lbl_dt_6_1, lbl_dt_6_2, lbl_dt_6_3, lbl_dt_5, lbl_dt_4_1, lbl_dt_4_2, lbl_dt_4_3, lbl_dt_4_4, lbl_dt_4_5, lbl_dt_4_6, lbl_dt_4_7, lbl_dt_3_1, lbl_dt_3_2, lbl_dt_2, lbl_dt_1, lbl_dt_db },
                new Label[] { lbl_cm_8, lbl_cm_7, lbl_cm_6_1, lbl_cm_6_2, lbl_cm_6_3, lbl_cm_5, lbl_cm_4_1, lbl_cm_4_2, lbl_cm_4_3, lbl_cm_4_4, lbl_cm_4_5, lbl_cm_4_6, lbl_cm_4_7, lbl_cm_3_1, lbl_cm_3_2, lbl_cm_2, lbl_cm_1, lbl_cm_db }
                    };
                    break;
            }
            playSimpleSound(); 
            timer.Start();
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            if (currentRound < allLabels[0].Length)
            {
                for (int i = 0; i < allLabels.Length; i++)
                {
                    int randomValue = GenerateRandomNumber(currentRound);
                    allLabels[i][currentRound].Text = randomValue.ToString("D" + GetDigitCount(currentRound));
                }

                tickCount++;

                if (tickCount >= 10)
                {
                    tickCount = 0;
                    SaveResultsToHistory();
                    currentRound++;
                }
            }
            else
            {
                timer.Stop();
                CheckWinningNumbers();
            }
        }

        private void SaveResultsToHistory()
        {
            string selectedDai = cbb_daiquay.SelectedItem.ToString();
            DateTime currentDateTime = DateTime.Now;
            string dateTimeString = currentDateTime.ToString("dd/MM/yyyy HH:mm:ss");

            lastResults.Clear();

            using (StreamWriter writer = new StreamWriter(historyFilePath, true))
            {
                writer.WriteLine($"Ngày {dateTimeString} - Đài: {selectedDai}");
                for (int i = 0; i < allLabels.Length; i++)
                {
                    for (int j = 0; j < allLabels[i].Length; j++)
                    {
                        writer.WriteLine($"{allLabels[i][j].Text}");
                        lastResults.Add(allLabels[i][j].Text);
                    }
                }
                writer.WriteLine("-----");
            }
        }

        private void CheckWinningNumbers()
        {
            string enteredSo = txtbdoso.Text.Trim();
            bool isWinner = false;

            foreach (var winningNumber in lastResults)
            {
                if (CheckForWin(enteredSo, winningNumber))
                {
                    AnnounceWin(enteredSo, winningNumber);
                    isWinner = true;
                    break;
                }
            }


            if (!isWinner)
            {
                MessageBox.Show("Rất tiếc, bạn chưa trúng giải lần này. Đừng bỏ cuộc, hãy tiếp tục thử vận may ở lần quay tiếp theo! Chúc bạn may mắn và có những giây phút vui vẻ!", "Kết quả xổ số", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool CheckForWin(string enteredSo, string winningNumber)
        {
            return enteredSo == winningNumber;
        }

        private void AnnounceWin(string enteredSo, string winningNumber)
        {
            string selectedDai = cbb_daiquay.SelectedItem.ToString(); 
            string giaVe = cbb_giave.SelectedItem.ToString(); 

            MessageBox.Show($"🎉 CHÚC MỪNG! 🎉\n\n" +
                            $"Bạn đã trúng thưởng với số: {enteredSo}\n" +
                            $"💥 Kết quả trúng: {winningNumber}\n" +
                            $"🏆 Đài quay: {selectedDai}\n" +
                            $"💰 Giá vé: {giaVe}\n\n" +
                            $"Chúc mừng bạn đã may mắn!",
                            "Kết quả xổ số",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private string GetGiaiText(int roundIndex)
        {
            switch (roundIndex)
            {
                case 0: return "Giải 8";
                case 1: return "Giải 7";
                case 2: return "Giải 6";
                case 3: case 4: case 5: return "Giải 5";
                case 6: return "Giải 4";
                case 7: return "Giải 3";
                case 8: return "Giải 2";
                case 9: return "Giải 1";
                case 10: return "Giải đặc biệt";
                default: return "Giải đặc biệt";
            }
        }

        private int GenerateRandomNumber(int roundIndex)
        {
            if (roundIndex == 0)
            {
                return random.Next(0, 100);
            } 
            else if (roundIndex == 1)
            {
                return random.Next(0, 1000);
            }
            else if (roundIndex >= 2 && roundIndex <= 5)
            {
                return random.Next(0, 10000);
            }
            else if (roundIndex >= 6 && roundIndex <= 16)
            {
                return random.Next(0, 100000);
            }
            else
            {
                return random.Next(0, 1000000);
            }
        }

        private int GetDigitCount(int roundIndex)
        {
            if (roundIndex == 0) return 2;
            if (roundIndex == 1) return 3;
            if (roundIndex >= 2 && roundIndex <= 5) return 4;
            if (roundIndex >= 6 && roundIndex <= 16) return 5;
            return 6;
        }
        private void btnxuatketqua_Click(object sender, EventArgs e)
        {
            string selectedDai = cbb_daiquay.SelectedItem.ToString();
            string filePath = @"D:\University\Nam4_1\Lap_trinh_windowns\NguyenVanDuy_XoSo\Database\database.txt"; // Thay đổi đường dẫn của bạn

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"---------------------------------KQSX MIỀN NAM NGÀY {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}---------------------------------");
                writer.WriteLine($"TỈNH {selectedDai.ToUpper()}");

                if (selectedDai == "TP. HCM")
                {
                    WriteResultsToFile(writer, "hcm");
                }
                else if (selectedDai == "ĐỒNG THÁP")
                {
                    WriteResultsToFile(writer, "dt");
                }
                else if (selectedDai == "CÀ MAU")
                {
                    WriteResultsToFile(writer, "cm");
                }
                else if (selectedDai == "CẢ 3 ĐÀI")
                {
                    WriteResultsToFile(writer, "hcm");
                    WriteResultsToFile(writer, "dt");
                    WriteResultsToFile(writer, "cm");
                }
            }

            MessageBox.Show("Kết quả đã được lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void WriteResultsToFile(StreamWriter writer, string dai)
        {
            var labelG8 = this.Controls.Find($"lbl_{dai}_8", true).FirstOrDefault() as Label;
            if (labelG8 != null && !string.IsNullOrEmpty(labelG8.Text))
            {
                writer.WriteLine($"Giải tám: {labelG8.Text}");
            }

            var labelG7 = this.Controls.Find($"lbl_{dai}_7", true).FirstOrDefault() as Label;
            if (labelG7 != null && !string.IsNullOrEmpty(labelG7.Text))
            {
                writer.WriteLine($"Giải bảy: {labelG7.Text}");
            }

            List<string> resultsG6 = new List<string>();
            for (int j = 1; j <= 3; j++)
            {
                var labelG6 = this.Controls.Find($"lbl_{dai}_6_{j}", true).FirstOrDefault() as Label;
                if (labelG6 != null && !string.IsNullOrEmpty(labelG6.Text))
                {
                    resultsG6.Add(labelG6.Text);
                }
            }
            if (resultsG6.Count > 0)
            {
                writer.WriteLine($"Giải sáu: {string.Join(", ", resultsG6)}");
            }

            var labelG5 = this.Controls.Find($"lbl_{dai}_5", true).FirstOrDefault() as Label;
            if (labelG5 != null && !string.IsNullOrEmpty(labelG5.Text))
            {
                writer.WriteLine($"Giải năm: {labelG5.Text}");
            }

            List<string> resultsG4 = new List<string>();
            for (int j = 1; j <= 7; j++)
            {
                var labelG4 = this.Controls.Find($"lbl_{dai}_4_{j}", true).FirstOrDefault() as Label;
                if (labelG4 != null && !string.IsNullOrEmpty(labelG4.Text))
                {
                    resultsG4.Add(labelG4.Text);
                }
            }
            if (resultsG4.Count > 0)
            {
                writer.WriteLine($"Giải bốn: {string.Join(", ", resultsG4)}");
            }

            List<string> resultsG3 = new List<string>();
            for (int j = 1; j <= 2; j++)
            {
                var labelG3 = this.Controls.Find($"lbl_{dai}_3_{j}", true).FirstOrDefault() as Label;
                if (labelG3 != null && !string.IsNullOrEmpty(labelG3.Text))
                {
                    resultsG3.Add(labelG3.Text);
                }
            }
            if (resultsG3.Count > 0)
            {
                writer.WriteLine($"Giải ba: {string.Join(", ", resultsG3)}");
            }

            var labelG2 = this.Controls.Find($"lbl_{dai}_2", true).FirstOrDefault() as Label;
            if (labelG2 != null && !string.IsNullOrEmpty(labelG2.Text))
            {
                writer.WriteLine($"Giải hai: {labelG2.Text}");
            }


            var labelG1 = this.Controls.Find($"lbl_{dai}_1", true).FirstOrDefault() as Label;
            if (labelG1 != null && !string.IsNullOrEmpty(labelG1.Text))
            {
                writer.WriteLine($"Giải một: {labelG1.Text}");
            }

            var dbLabel = this.Controls.Find($"lbl_{dai}_db", true).FirstOrDefault() as Label;
            if (dbLabel != null && !string.IsNullOrEmpty(dbLabel.Text))
            {
                writer.WriteLine($"Giải đặc biệt: {dbLabel.Text}");
            }
        }

        private bool isResetMessageShown = false;

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (timer != null && timer.Enabled)
            {
                timer.Stop();
            }

            ResetLabels();

            txtbdoso.Clear();
            cbb_daiquay.SelectedIndex = -1;
            cbb_giave.SelectedIndex = -1;

            if (!isResetMessageShown)
            {
                MessageBox.Show("Đã đặt lại dữ liệu. Bạn có thể bắt đầu quay số mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isResetMessageShown = true;
            }
        }

        private void ResetLabels()
        {
            foreach (var labelGroup in allLabels)
            {
                foreach (var label in labelGroup)
                {
                    label.Text = "**";
                }
            }

            lbl_hcm_7.Text = "***";
            lbl_hcm_6_1.Text = "****";
            lbl_hcm_6_2.Text = "****";
            lbl_hcm_6_3.Text = "****";
            lbl_hcm_5.Text = "****";
            lbl_hcm_4_1.Text = "*****";
            lbl_hcm_4_2.Text = "*****";
            lbl_hcm_4_3.Text = "*****";
            lbl_hcm_4_4.Text = "*****";
            lbl_hcm_4_5.Text = "*****";
            lbl_hcm_4_6.Text = "*****";
            lbl_hcm_4_7.Text = "*****";
            lbl_hcm_3_1.Text = "*****";
            lbl_hcm_3_2.Text = "*****";
            lbl_hcm_2.Text = "*****";
            lbl_hcm_1.Text = "*****";
            lbl_hcm_db.Text = "******";

            lbl_dt_7.Text = "***";
            lbl_dt_6_1.Text = "****";
            lbl_dt_6_2.Text = "****";
            lbl_dt_6_3.Text = "****";
            lbl_dt_5.Text = "****";
            lbl_dt_4_1.Text = "*****";
            lbl_dt_4_2.Text = "*****";
            lbl_dt_4_3.Text = "*****";
            lbl_dt_4_4.Text = "*****";
            lbl_dt_4_5.Text = "*****";
            lbl_dt_4_6.Text = "*****";
            lbl_dt_4_7.Text = "*****";
            lbl_dt_3_1.Text = "*****";
            lbl_dt_3_2.Text = "*****";
            lbl_dt_2.Text = "*****";
            lbl_dt_1.Text = "*****";
            lbl_dt_db.Text = "******";

            lbl_cm_7.Text = "***";
            lbl_cm_6_1.Text = "****";
            lbl_cm_6_2.Text = "****";
            lbl_cm_6_3.Text = "****";
            lbl_cm_5.Text = "****";
            lbl_cm_4_1.Text = "*****";
            lbl_cm_4_2.Text = "*****";
            lbl_cm_4_3.Text = "*****";
            lbl_cm_4_4.Text = "*****";
            lbl_cm_4_5.Text = "*****";
            lbl_cm_4_6.Text = "*****";
            lbl_cm_4_7.Text = "*****";
            lbl_cm_3_1.Text = "*****";
            lbl_cm_3_2.Text = "*****";
            lbl_cm_2.Text = "*****";
            lbl_cm_1.Text = "*****";
            lbl_cm_db.Text = "******";
        }
    }
}
