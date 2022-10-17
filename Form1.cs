using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Таблица_умножения_Forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Rectangle screenSize = Screen.PrimaryScreen.Bounds;
            if (screenSize.Height < 1080 || screenSize.Width < 1920)
            {

                MessageBox.Show(String.Format("Разрешение данного дисплея {1} x {0}. " +
                    "Есть вероятность, что элементы отобразятся некорректно. " +
                    "Рекомендуемое разрешение экрана для использованния программы " +
                    "- 1920 х 1080" , screenSize.Size.Height, screenSize.Size.Width), "ПРЕДУПРЕЖДЕНИЕ!");
            }
            
            InitializeComponent();
            AddRatingTableOnPanel();
        }
        #region устраниение мерцания форм
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return handleParam;
            }
        }
        #endregion

        string[] ratingListArr;
        string[] scoreList;
        private void WriteRatingArr()
        {
            try
            {
                using (StreamReader reader = new StreamReader("Rating.txt"))
                {
                    int count = File.ReadAllLines("Rating.txt").Length;
                    ratingListArr = new string[count];
                    scoreList = new string[count];
                    for (int i = 0; i < count; i++)
                    {
                        ratingListArr[i] = reader.ReadLine();
                    }
                }
            }
            catch (Exception)
            {
                using (StreamWriter writer = new StreamWriter("Rating.txt")) // Добавление файла

                using (StreamReader reader = new StreamReader("Rating.txt"))
                {
                    int count = File.ReadAllLines("Rating.txt").Length;
                    ratingListArr = new string[count];
                    scoreList = new string[count];
                    for (int i = 0; i < count; i++)
                    {
                        ratingListArr[i] = reader.ReadLine();
                    }
                }
            }

            for (int i = 0; i < ratingListArr.Length; i++)
            {
                Regex regex = new Regex(@"\d+$");
                MatchCollection matches = regex.Matches(ratingListArr[i]);
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                        scoreList[i] = Convert.ToString(match.Value);
                }
            }
        }

        private void AddRatingTableOnPanel()
        {
            panel2.HorizontalScroll.Maximum = 0;
            panel2.AutoScroll = false;
            panel2.VerticalScroll.Visible = false;
            panel2.AutoScroll = true;
            WriteRatingArr();

            int xAdd = 0;

            for (int i = 0; i < ratingListArr.Length; i++)
            {
                Panel panel = new Panel();
                panel.Size = new Size(475, 50);
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.BackColor = Color.Transparent;
                panel.Location = new Point(0, xAdd);
                xAdd += 50;
                panel2.Controls.Add(panel);

                System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                label.Size = new Size(300, 33);
                label.Location = new Point(3, 9);
                label.Font = new Font("Microsoft YaHei", 17F, FontStyle.Bold);
                label.Text = ratingListArr[i];
                panel.Controls.Add(label);

                System.Windows.Forms.Label label1 = new System.Windows.Forms.Label();
                label1.Size = new Size(74, 40);
                label1.Location = new Point(391, 5);
                label1.Font = new Font("Microsoft YaHei", 17.8F, FontStyle.Bold);
                if (Convert.ToInt32(scoreList[i]) <= 20)
                    label1.ForeColor = Color.FromArgb(251, 37, 0);

                else if (Convert.ToInt32(scoreList[i]) <= 35)
                    label1.ForeColor = Color.FromArgb(243, 88, 2);

                else if (Convert.ToInt32(scoreList[i]) <= 45)
                    label1.ForeColor = Color.FromArgb(218, 147, 0);

                else if (Convert.ToInt32(scoreList[i]) <= 65)
                    label1.ForeColor = Color.FromArgb(197, 176, 0);

                else if (Convert.ToInt32(scoreList[i]) <= 80)
                    label1.ForeColor = Color.FromArgb(182, 193, 0);

                else if (Convert.ToInt32(scoreList[i]) <= 90)
                    label1.ForeColor = Color.FromArgb(145, 219, 1);

                else if (Convert.ToInt32(scoreList[i]) <= 95)
                    label1.ForeColor = Color.FromArgb(133, 228, 2);
                else
                    label1.ForeColor = Color.DarkOliveGreen;
                label1.Text = scoreList[i];
                panel.Controls.Add(label1);
            }
        }

        /// <summary>
        /// Событие закрытия формы. Активируется при нажатии
        /// </summary>
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(); // Форма игры "плитки"
            this.Hide();
            form2.ShowDialog();
            Controls.Clear();
            this.Refresh();
            InitializeComponent();
            AddRatingTableOnPanel();

            this.Show();
        }
        private void ExitApp(object sender, MouseEventArgs e)
        {
            Application.Exit(); // Завершение жизненного цикла главной формы
        }

        bool animathionIsWorking;
        bool newEventIsStart;
        private async void pictureBox4_MouseMove(object sender, MouseEventArgs e) // Анимация
        {
            while (animathionIsWorking == false && button4.Location.Y < 350 && newEventIsStart == false)
            {
                animathionIsWorking = true;
                await Task.Delay(1);
                button4.Location = new Point(button4.Location.X, button4.Location.Y + button4.Location.Y / 120);
                button4.Location = new Point(button4.Location.X, button4.Location.Y + 1);
                animathionIsWorking = false;
            }
        }

        private async void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            newEventIsStart = true;
            while (button4.Location.Y > 290 && newEventIsStart == true)
            {
                animathionIsWorking = true;
                await Task.Delay(1);
                button4.Location = new Point(button4.Location.X, button4.Location.Y + (100 - button4.Location.Y) / 100);
                /*button4.Location = new Point(button4.Location.X + 1, button4.Location.Y);*/
                animathionIsWorking = false;
            }
            newEventIsStart = false;
        }

        private void button1_Click(object sender, EventArgs e) // Очищение результатов из листа
        {
            using (StreamWriter writer = new StreamWriter("Rating.txt", false)) // Перезапись файла
            {
                writer.Write("");
                Controls.Clear();
                this.Refresh();

                InitializeComponent();
            }
        }
        bool animathionIsWorking1;
        bool newEventIsStart1;
        private async void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            while (animathionIsWorking1 == false && button2.Location.Y < 350 && newEventIsStart1 == false)
            {
                animathionIsWorking1 = true;
                await Task.Delay(1);
                button2.Location = new Point(button2.Location.X, button2.Location.Y + button2.Location.Y / 120);
                button2.Location = new Point(button2.Location.X, button2.Location.Y + 1);
                animathionIsWorking1 = false;
            }
        }

        private async void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            newEventIsStart1 = true;
            while (button2.Location.Y > 290 && newEventIsStart1 == true)
            {
                animathionIsWorking1 = true;
                await Task.Delay(1);
                button2.Location = new Point(button2.Location.X, button2.Location.Y + (100 - button2.Location.Y) / 100);
                animathionIsWorking1 = false;
            }
            newEventIsStart1 = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            this.Hide();
            form4.ShowDialog();
            Controls.Clear();
            this.Refresh();
            InitializeComponent();
            AddRatingTableOnPanel();

            this.Show();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("Rating.txt", false)) // Перезапись файла
            {
                writer.Write("");

                panel2.Controls.Clear();
            }
        }

        private void panel16_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            this.Hide();
            form3.ShowDialog();
            Controls.Clear();
            this.Refresh();
            InitializeComponent();
            AddRatingTableOnPanel();

            this.Show();
        }
    }
}
