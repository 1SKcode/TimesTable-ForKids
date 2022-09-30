using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;

namespace Таблица_умножения_Forms
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            UpdateGame("Restart");
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

        int level = 1;
        int y = 4;
        int x = 4;
        int gameMode = 1; // 0 - режим ОТВЕТ-ПРИМЕР | 1 - режим ПРИМЕР-ОТВЕТ
        int sizeXY = 190;
        int addx = 0;
        int addy = 0;
        int numOfQuestion = 15; // Счетчик примеров ( от N до 0 )
        string[] buffquestions = new string[31];
        string[] buffq = new string[31];
        int mistakes = 0;

        #region [МЕТОДЫ РАБОТЫ С МАССИВАМИ]

        string[] Answer = new string[31];
        string[] Questions = new string[31];

        public void CreateQuestionArr()
        {
            int count = 0;
            int iShift = 2; // Переменная, чтобы не создавать повторяющихся по смыслу примеров (например 4х6 и 6х4) 
            for (int i = 2; i <= 9; i++)
            {
                for (int j = iShift; j <= 9; j++)
                {
                    if ((i == 2 && j == 9) || (i == 2 && j == 8) || (i == 2 && j == 6) || (i == 3 && j == 8) || (i == 4 && j == 9)) // Этим условием пропускаем некоторые элементы (нам не нужны)
                    {

                    }
                    else
                    {
                        Answer[count] = Convert.ToString(i * j);
                        count++;
                    }
                }
                iShift++;
            }
            count = 0;
            iShift = 2;
            for (int i = 2; i <= 9; i++)
            {
                for (int j = iShift; j <= 9; j++)
                {
                    if ((i == 2 && j == 9) || (i == 2 && j == 8) || (i == 2 && j == 6) || (i == 3 && j == 8) || (i == 4 && j == 9))
                    {

                    }
                    else
                    {
                        Questions[count] = $"{i}" + "x" + $"{j}";
                        count++;
                    }
                }
                iShift++;
            }
            RandomArr();
        }

        public void RandomArr()
        {
            Random random = new Random();
            for (int i = Answer.Length - 1; i > 0; i--)
            {
                int j = random.Next(i);
                string temp = Answer[j];
                Answer[j] = Answer[i];
                Answer[i] = temp;

                temp = Questions[j];
                Questions[j] = Questions[i];
                Questions[i] = temp;
            }
        }
        private void CreateNewMass(int y, int x)
        {

            Questions.CopyTo(buffq, 0);
            Answer.CopyTo(buffquestions, 0);

            Array.Resize(ref buffq, y * x);
            Array.Resize(ref buffquestions, y * x);
        }

        public void RandMass() // Рандомизируем массив ЧИСЛОВЫХ ПРИМЕРОВ
        {
            RandMass(buffquestions);
            RandMass(buffq);
        }

        public void RandMass(string[] array) // Рандомизируем массив ЧИСЛОВЫХ ПРИМЕРОВ
        {
            Random random = new Random();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(i);
                var temp = array[j];
                array[j] = array[i];
                array[i] = temp;
            }
        }
        #endregion

        #region [ ДОБАВЛЕНИЕ ПАНЕЛЕЙ, ОБНОВЛЕНИЕ СОСТОЯНИЯ ИГРЫ, СОБЫТИЕ КЛИКА НА ПАНЕЛЬ ]

        public void AddButtons(int level) // Добавляем кнопки на экран
        {
            if (level == 1)
            {
                label1.Text = "Уровень 1";
                panel1.Size = new Size(755, 755);

                panel1.BackgroundImage = Properties.Resources.WU0Sb46_e3DXAMOs5w62M1P7qfn3wZt0tmPd2kSuEphpWkIyaS1vSfsLEEkt9_9KiWy2R45U;
                y = 2;
                x = 2;
                sizeXY = 378;
                CreateNewMass(y, x);
                numOfQuestion = 3;
            }
            if (level == 2)
            {
                label1.Text = "Уровень 2";

                panel1.BackgroundImage = Properties.Resources.a875e0ff252fb4eb94207cb15b74cc8c;
                y = 3;
                x = 3;
                sizeXY = 252;
                panel1.Size = new Size(754, 754);
                CreateNewMass(y, x);
                numOfQuestion = 8;
            }
            else if (level == 3)
            {
                label1.Text = "Уровень 3";

                panel1.BackgroundImage = Properties.Resources.unnamed;
                y = 4;
                x = 4;
                sizeXY = 190;
                panel1.Size = new Size(757, 757);
                CreateNewMass(y, x);
                numOfQuestion = 15;
            }
            int indexArr = buffquestions.Length - 1;
            if (gameMode == 1)
            {
                GameModeArrAdder(buffquestions, indexArr);
            }
            else
            {
                GameModeArrAdder(buffq, indexArr);
            }
        }


        void GameModeArrAdder(string[] arr, int indexArr)
        {
            for (int i = 0; i < y; i++)     // Кол-во итераций по Y
            {
                for (int j = 0; j < x; j++) // Кол-во итераций по X
                {
                    Button button = new Button();

                    panel1.Controls.Add(button);
                    button.BackColor = Color.FromArgb(173, 125, 92);
                    button.BackgroundImageLayout = ImageLayout.Stretch;
                    button.FlatStyle = FlatStyle.Flat;
                    button.Font = new Font("Segoe UI Variable Text", 27.2F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                    button.ForeColor = Color.DarkSlateGray;
                    button.Text = Convert.ToString(arr[indexArr]);
                    button.TabStop = false;
                    button.Click += PanelNew_Click;
                    button.UseVisualStyleBackColor = false;
                    if (indexArr % 10 == 0 || indexArr % 10 == 3 || indexArr % 10 == 5)
                        button.BackgroundImage = Properties.Resources.PanelBack;
                    if (indexArr % 10 == 1 || indexArr % 10 == 4 || indexArr % 10 == 9)
                        button.BackgroundImage = Properties.Resources.PanelBack1;
                    if (indexArr % 10 == 2 || indexArr % 10 == 6)
                        button.BackgroundImage = Properties.Resources.PanelBack2;
                    if (indexArr % 10 == 7 || indexArr % 10 == 8)
                        button.BackgroundImage = Properties.Resources.PanelBack4;

                    button.Location = new Point(addx, addy);

                    if (gameMode == 0)
                        button.Name = buffquestions[indexArr];
                    else
                        button.Name = buffq[indexArr];

                    button.Size = new Size(sizeXY, sizeXY);

                    addx += sizeXY - 1;  // Итерируем значение Х координаты
                    indexArr--;
                }
                addy += sizeXY - 1;      // Итерируем значение Y координаты
                addx = 0;        // Сбрасываем X координаты
            }
            addy = 0;
            RandMass(buffquestions);
        }

        void PanelNew_Click(object sender, EventArgs e) // Метод события клик на панель
        {
            if (gameMode == 0)
            {
                if ((sender as Button).Name == buffquestions[numOfQuestion + 1])
                {
                    (sender as Button).Visible = false;
                    UpdateQuestion();
                }
                else
                {
                    (sender as Button).BackColor = Color.Red;
                    mistakes++;
                }
            }
            else
            {
                if ((sender as Button).Name == buffq[numOfQuestion + 1])
                {
                    (sender as Button).Visible = false;
                    UpdateQuestion();
                }
                else
                {
                    (sender as Button).BackColor = Color.Red;
                    mistakes++;
                }
            }
        }

        private void UpdateQuestion() // Обновление примера
        {

            if (numOfQuestion >= 0)
            {
                if (gameMode == 1)
                    label2.Text = buffq[numOfQuestion] + "= ";
                else
                    label2.Text = buffquestions[numOfQuestion];
                numOfQuestion--;
            }
            else
            {
                if (level >= 3)
                {
                    button1.Visible = false;
                    button3.Visible = true;
                }
                else button1.Visible = true;
                label2.Visible = false;
                label3.Visible = true;
                label4.Visible = true;
                button2.Visible = true;
                label10.Visible = true;

                label10.Text = "БАЛЛЫ: " + LogicOfRating();
                label4.Text = "ошибок: " + mistakes;

                // Запись баллов в лист с баллами
                WriteRating();
            }
        }



        private void UpdateGame(string status)
        {
            label2.Visible = true;
            label3.Visible = false;
            label4.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            label10.Visible = false;

            Controls.Clear();
            this.Refresh();
            InitializeComponent();
            mistakes = 0;

            Array.Resize(ref buffq, 64);
            Array.Resize(ref buffquestions, 64);
            CreateQuestionArr();
            if (status == "Next")
                level++;

            RandMass();
            AddButtons(level);
            RandMass(buffq);
            UpdateQuestion();

            if (gameMode == 1)
                label6.Text = "\"поиск по примеру\"";
            else
                label6.Text = "\"поиск по ответу\"";
        }

        #endregion

        #region [ КНОПКИ ]

        private void button3_Click(object sender, EventArgs e) // Кнопка начать с первого
        {
            button3.Visible = false;
            level = 0;
            UpdateGame("Next");
        }

        private void button2_Click(object sender, EventArgs e) // Повторить уровень
        {
            UpdateGame("Restart");
        }

        private void button1_Click(object sender, EventArgs e) // Следующий уровень
        {
            UpdateGame("Next");
        }

        private void panel2_Click(object sender, EventArgs e) // Кнопка закрытия
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e) // Кнопка смены режима
        {
            if (gameMode == 1)
            {
                label6.Text = "\"поиск по примеру\"";
                gameMode = 0;
                UpdateGame("Restart");
            }
            else
            {
                label6.Text = "\"поиск по ответу\"";
                gameMode = 1;
                UpdateGame("Restart");

            }

        }

        #endregion

        #region [ АНИМАЦИЯ ]

        bool animathionIsWorking;
        bool newEventIsStart;
        private async void button4_MouseMove(object sender, MouseEventArgs e)
        {
            while (animathionIsWorking == false && button4.Width <= 380 && newEventIsStart == false) // вылетание зеленой кнопки
            {
                animathionIsWorking = true;
                await Task.Delay(1);
                button4.Size = new Size(button4.Width + (500 - button4.Width) / 25, 59);
                button4.Size = new Size(button4.Width + 1, 59);
                animathionIsWorking = false;
            }
            if (button4.Width >= 380)
                panelAnimathionON(); // Вылетание белой панели с информацией
        }

        private async void panelAnimathionON()
        {
            while (animathionIsWorking == false && panel3.Height <= 240 && newEventIsStart == false)
            {
                animathionIsWorking = true;
                await Task.Delay(1);
                panel3.Size = new Size(367, panel3.Height + (300 - panel3.Height) / 5);
                animathionIsWorking = false;
            }

            while (animathionIsWorking == false && label9.Location.Y < 20 && newEventIsStart == false) // Вылетание лэйбла9 "Уровень сохранится"
            {
                animathionIsWorking = true;
                await Task.Delay(1);
                label9.Location = new Point(390, label9.Location.Y - label9.Location.Y / 2 + 11);
                label9.Location = new Point(390, label9.Location.Y - 1);

                animathionIsWorking = false;
            }
        }

        private async void button4_MouseLeave(object sender, EventArgs e) // Сворачивание 
        {
            // Сворачивание [сначала белой панели и лэйбла 9 - затем зеленой кнопки] 
            newEventIsStart = true;
            while (panel3.Height > 0 && newEventIsStart == true)
            {
                animathionIsWorking = true;
                await Task.Delay(1);
                panel3.Size = new Size(367, panel3.Height - panel3.Height / 5);
                panel3.Size = new Size(367, panel3.Height - 1);

                label9.Location = new Point(390, label9.Location.Y - (80 - label9.Location.Y) / 5);
                label9.Location = new Point(390, label9.Location.Y - 1);

                animathionIsWorking = false;
            }

            while (button4.Width > 200 && newEventIsStart == true)
            {
                animathionIsWorking = true;
                await Task.Delay(1);
                button4.Size = new Size(button4.Width - button4.Width / 20, 59);
                animathionIsWorking = false;
            }
            newEventIsStart = false;
        }

        #endregion

        #region [ ЛОГИКА ОЦЕНОК ]

        private string LogicOfRating()
        {
            int num = 100 - mistakes * 10 / level;
            if (num >= 0)
                return Convert.ToString(num);
            return "0";
        }

        private void WriteRating()
        {
            using (StreamWriter writer = new StreamWriter("Rating.txt", true)) // Добавление в файл
            {
                writer.WriteLineAsync("[ПЛИТКИ] " + "уровень " + level + " - " + LogicOfRating());
            }
        }

        #endregion
    }

}
