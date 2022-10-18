using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Таблица_умножения_Forms
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            SubsEvent();   // Подписка на множество событий (используются лямба-выражения через делегирование)
            CreateQuestionArr();
            StatusGameUPDATE("Restart", 5);
        }
        private void SubsEvent() // Метод подписки на все нужные события 
        {
            timer1.Tick += (s, e) => LabelUpdate();

            panel1.Click += (s, e) => { this.Close(); }; // Нажатие кнопки закрытия

            textBox1.TextChanged += async (s, e) =>
            {
                /*if (Regex.IsMatch(textBox1.Text, "[^0-9]"))                                            // Реализация с regex 
                {
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
                    textBox1.SelectionStart = textBox1.TextLength;
                }*/


                if (Int32.TryParse(textBox1.Text, out int result) == false && textBox1.Text.Length > 0) // Реализация через TryParse
                {
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
                    textBox1.SelectionStart = textBox1.TextLength;
                    label1.Visible = true;
                    await Task.Delay(1200);
                    label1.Visible = false;
                }
            };  // Ввод текста

            textBox1.KeyDown += (s, e) =>
            {
                // УСЛОВИЯ: Есть вероятность, что три лэйбла будут иметь одинакое имя, например 24, т.е текст лэйбла будет сдержать любые строки из 6х4, 8х3, 4х6 и 3х8

                // ПРОБЛЕМА: Мы отправляем значение на проверку нажатием Enter и ожидаем, что самый верхний лэйбл на экране исчезнет, т.к мы ввели правильный ответ
                // однако, заместо этого может исчезнуть лэйбл уровня ниже, т.к есть вероятность, что ответ на второй или третий пример будет аналогичен первому.
                // В итоге, чтобы убрать первый лэйбл, нужно отправить ответ два раза - это противоречит всем принципам построения правильного геймплэя

                // РЕШЕНИЕ: Если введенное число удовлетвояет условию равенства на первый, второй или третий лэйбл (первый попавшися), помимо того, чтобы сразу скрыть его,
                // проверяем и другие на совпадение, через вложеные конструкции If().
                // Таким образом решается проблема, и теперь даже когда все три лэйбла имеют одинаковый ответ, будут исчезать все три

                // ПЛЮСЫ: Мы также можем дать ответ на решение примеров ниже, на любой из тех, что находится на экране и нам не засчитают ошибку
                // Ошибка засчитается, если ответ не подойдет ни к одному из лэйблов, находящихся на экране
                if (e.KeyCode == Keys.Enter)
                {
                    DoubleShot.Visible = false;
                    if (textBox1.Text == FirstLabel.Name && FirstLabel.Visible == true)
                    {
                        AddLog("", FirstLabel);
                        if (textBox1.Text == SecondLabel.Name && SecondLabel.Visible == true) // Есть возможность, что ответ совпадет на еще 1 или 2 примера, например 6х4=24, 8х3=24 и 4х6=24 
                        {
                            AddLog("", SecondLabel);
                            if (textBox1.Text == ThirdLabel.Name && ThirdLabel.Visible == true) // Также проверяем правильный ли это ответ на третий лэйбл
                            {
                                AddLog("", ThirdLabel);
                                correctAnswer++;
                                ThirdLabel.Visible = false;
                            }
                            correctAnswer++;
                            SecondLabel.Visible = false;

                            DoubleShot.Visible = true;
                        }
                        correctAnswer++;
                        FirstLabel.Visible = false; // на время перемещения выключаем видимость, так мы избавляемся от мерцания во время репоинта локации
                    }
                    if (textBox1.Text == SecondLabel.Name && SecondLabel.Visible == true)
                    {
                        AddLog("", SecondLabel);
                        if (textBox1.Text == FirstLabel.Name && FirstLabel.Visible == true) // Проверка первого лэйбла
                        {
                            AddLog("", FirstLabel);
                            if (textBox1.Text == ThirdLabel.Name && ThirdLabel.Visible == true) // Также проверяем правильный ли это ответ на третий лэйбл
                            {
                                AddLog("", ThirdLabel);
                                correctAnswer++;
                                ThirdLabel.Visible = false;
                            }
                            DoubleShot.Visible = true;

                            correctAnswer++;
                            FirstLabel.Visible = false;
                        }
                        correctAnswer++;
                        SecondLabel.Visible = false;
                    }
                    if (textBox1.Text == ThirdLabel.Name && ThirdLabel.Visible == true)
                    {
                        AddLog("", ThirdLabel);
                        if (textBox1.Text == FirstLabel.Name && FirstLabel.Visible == true) // Проверка первого лэйбла
                        {
                            AddLog("", FirstLabel);
                            if (textBox1.Text == SecondLabel.Name && SecondLabel.Visible == true)
                            {
                                AddLog("", SecondLabel);
                                correctAnswer++;
                                SecondLabel.Visible = false;
                            }
                            DoubleShot.Visible = true;

                            correctAnswer++;
                            FirstLabel.Visible = false;
                        }
                        correctAnswer++;
                        ThirdLabel.Visible = false;
                    }
                    if (textBox1.Text != ThirdLabel.Name && textBox1.Text != SecondLabel.Name && textBox1.Text != FirstLabel.Name)
                    {
                        mistakes++;
                        AddLog("mis+", label1);
                    }
                    e.SuppressKeyPress = true;
                    label10.Text = "Счет: " + correctAnswer;

                    textBox1.Text = "";

                    LevelUp();

                    CheckHealth();
                }
            };

            panel7.MouseMove += (s, e) =>
            {
                panel7.Size = new Size(120, 120);
                panel7.Location = new Point(893, 698);
            };
            panel7.MouseLeave += (s, e) =>
            {
                panel7.Size = new Size(110, 110);
                panel7.Location = new Point(898, 703);
            };
            panel7.MouseClick += (s, e) => { StatusGameUPDATE("Restart", 5); };
        }

        int correctAnswer = 0;
        int correctAnswerForLevelUp = 0;
        int level = 1;
        int complexity = 1; // Уровень сложности
        int mistakes = 0;
        int[] Answer = new int[64];
        string[] Questions = new string[64];
        int countQuestions = 63; // Считается от 0

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

        #region [ ГЕНЕРАЦИЯ РАНДОМНОГО МАССИВА ]
        public void CreateQuestionArr()
        {
            int count = 0;
            for (int i = 2; i <= 9; i++)
            {
                for (int j = 2; j <= 9; j++)
                {
                    Answer[count] = i * j;
                    count++;
                }
            }
            count = 0;
            for (int i = 2; i <= 9; i++)
            {
                for (int j = 2; j <= 9; j++)
                {
                    Questions[count] = $"{i}" + " x " + $"{j}";
                    count++;
                }
            }
        }

        public void RandomArr()
        {
            Random random = new Random();
            for (int i = Answer.Length - 1; i > 0; i--)
            {
                int j = random.Next(i);
                int temp = Answer[j];
                Answer[j] = Answer[i];
                Answer[i] = temp;

                string temp1 = Questions[j];
                Questions[j] = Questions[i];
                Questions[i] = temp1;
            }
        }
        #endregion

        #region [ ВЫПЛЫВАЮЩИЙ ТЕКСТ ]
        int count;
        int count2;
        int count3;

        float fontIterator = 70;
        float fontIterator2 = 70;
        float fontIterator3 = 70;
        bool firstLabelAchGoal;
        public void LabelUpdate()
        {
            if (FirstLabel.Location.Y == 800 - complexity && countQuestions >= 0) // Если панель в начальной позиции
            {
                FirstLabel.Text = Questions[countQuestions];
                FirstLabel.Name = Convert.ToString(Answer[countQuestions]);
                countQuestions--;
            }
            if (SecondLabel.Location.Y == 800 - complexity && countQuestions >= 0)
            {
                SecondLabel.Text = Questions[countQuestions];
                SecondLabel.Name = Convert.ToString(Answer[countQuestions]);
                countQuestions--;
            }
            if (ThirdLabel.Location.Y == 800 - complexity && countQuestions >= 0)
            {
                ThirdLabel.Text = Questions[countQuestions];
                ThirdLabel.Name = Convert.ToString(Answer[countQuestions]);
                countQuestions--;
            }

            // ПЕРВЫЙ ЛЭЙБЛ
            if (FirstLabel.Location.Y > 150) // Запускаем движение первого лэйбла, с условием, что он не доехал до финала (верхняя граница)
            {
                FirstLabel.Location = new Point(FirstLabel.Location.X, FirstLabel.Location.Y - complexity);
                count++;
                if (count > 6) // Каждые 6 тиков таймера увеличиваем шрифт на 0.5 и корректируем 
                {
                    FirstLabel.Location = new Point(FirstLabel.Location.X - 1, FirstLabel.Location.Y);
                    count = 0;
                    fontIterator += 0.5f;
                    FirstLabel.Font = new Font("Segoe UI Black", fontIterator, FontStyle.Bold);
                }
            }
            else // Если условие выше перестает быть true, то возвращаем лэйбл в начальное нижнее положение
            {
                firstLabelAchGoal = true;
                if (FirstLabel.Visible == true)
                {
                    mistakes++;
                    AddLog("mis", FirstLabel);
                }
                FirstLabel.Visible = false; // на время перемещения выключаем видимость, так мы избавляемся от мерцания во время репоинта локации
                count = 0;
                fontIterator = 81;
                ResetLabel(FirstLabel);
                CheckHealth();
            }

            // ВТОРОЙ ЛЭЙБЛ
            if (FirstLabel.Location.Y < 550 || firstLabelAchGoal == true) // Так мы проверяем на нужной ли координате первый лэйбл,
                                                                          // а после того, как он пропадет, у нас будет второй оператор, который также позволит запустить вторую панель
            {

                if (SecondLabel.Location.Y > 150) // Запускаем движение второго лэйбла, с условием, что он не доехал до финала
                {
                    SecondLabel.Location = new Point(SecondLabel.Location.X, SecondLabel.Location.Y - complexity);
                    count2++;
                    if (count2 > 6)
                    {
                        SecondLabel.Location = new Point(SecondLabel.Location.X - 1, SecondLabel.Location.Y);
                        count2 = 0;
                        fontIterator2 += 0.5f;
                        SecondLabel.Font = new Font("Segoe UI Black", fontIterator2, FontStyle.Bold);


                    }
                }
                else
                {

                    if (SecondLabel.Visible == true)
                    {
                        mistakes++;
                        AddLog("mis", SecondLabel);
                    }
                    count2 = 0;
                    fontIterator2 = 81;
                    ResetLabel(SecondLabel);
                    CheckHealth();
                }

            }

            // ТРЕТИЙ ЛЭЙБЛ
            if (FirstLabel.Location.Y < 352 || firstLabelAchGoal == true)
            {

                if (ThirdLabel.Location.Y > 150) // Запускаем движение третьего лэйбла, с условием, что он не доехал до финала
                {
                    ThirdLabel.Location = new Point(ThirdLabel.Location.X, ThirdLabel.Location.Y - complexity);
                    count3++;
                    if (count3 > 6)
                    {
                        ThirdLabel.Location = new Point(ThirdLabel.Location.X - 1, ThirdLabel.Location.Y);
                        count3 = 0;
                        fontIterator3 += 0.5f;
                        ThirdLabel.Font = new Font("Segoe UI Black", fontIterator3, FontStyle.Bold);
                    }
                }
                else
                {
                    if (ThirdLabel.Visible == true)
                    {
                        mistakes++;
                        AddLog("mis", ThirdLabel);
                    }
                    count3 = 0;
                    fontIterator3 = 81;
                    ResetLabel(ThirdLabel);
                    CheckHealth();
                }

            }
        }

        int yAdder = 3;
        bool firstColumnIsFull;
        public void AddLog(string str, Label l)
        {
            Panel panel = new Panel(); // Левая панель под значок
            if (firstColumnIsFull == false)
                panel.Location = new Point(3, yAdder);
            else
                panel.Location = new Point(142, yAdder);
            panel.Size = new Size(35, 35);
            panel.BackgroundImageLayout = ImageLayout.Stretch;
            if (str == "mis" || str == "mis+")
                panel.BackgroundImage = Properties.Resources.cancel;
            else
                panel.BackgroundImage = Properties.Resources.check_mark;
            panel9.Controls.Add(panel);

            Panel panel1 = new Panel(); // Правая панель под пример
            if (firstColumnIsFull == false)
                panel1.Location = new Point(42, yAdder);
            else
                panel1.Location = new Point(181, yAdder);
            panel1.Size = new Size(98, 35);
            panel9.Controls.Add(panel1);

            Label label = new Label();
            label.Location = new Point(0, 3);
            label.AutoSize = true;
            label.Font = new Font("Comic Sans MS", 15.6F, FontStyle.Bold);
            if (str == "mis+")
                label.Text = "ОШИБКА";
            else
                label.Text = l.Text.Replace(" ", "") + "=" + l.Name;
            panel1.Controls.Add(label);

            yAdder += 35;

            if (yAdder >= 900)
            {
                yAdder = 3;
                firstColumnIsFull = true;
            }
        }
        private void WriteRating()
        {
            int finalyRating = 0;
            if (correctAnswer <= 1)
                finalyRating = 0;
            else if (correctAnswer >= 2)
                finalyRating = 10 + Convert.ToInt32(correctAnswer * 1.7);
            if (correctAnswer >= 63)
                finalyRating = 100;
            using (StreamWriter writer = new StreamWriter("Rating.txt", true)) // Добавление в файл
            {
                writer.WriteLineAsync("[КОНВЕЙЕР] " + "уровень" + level + "        | " + Convert.ToString(finalyRating));
            }
        }

        public void LevelUp()
        {
            if (correctAnswer == correctAnswerForLevelUp)
            {
                level++;
                correctAnswerForLevelUp += 5;
                label6.Text = "УРОВЕНЬ " + level;
                timer1.Interval -= 2;
            }
        }

        public void ResetLabel(Label label)
        {
            label.Visible = false;
            label.Location = new Point(802, 800);
            label.Font = new Font("Segoe UI Black", 81, FontStyle.Bold);
            label.Visible = true;
        }

        #endregion

        #region [ СЛУЖЕБНЫЕ МЕТОДЫ ]

        public void CheckHealth()
        {
            if (mistakes == 1)
            {
                panel6.Visible = false;
            }
            if (mistakes == 2)
            {
                panel5.Visible = false;
            }
            if (mistakes == 3)
            {
                panel4.Visible = false;
            }
            if (mistakes == 4)
            {
                panel3.Visible = false;
                WriteRating();
                StatusGameUPDATE("Lost", 5);
            }
        }

        public void StatusGameUPDATE(string status, int countQuestions)
        {
            if (status == "Lost")
            {
                label9.Visible = true;
                label8.Visible = true;
                panel7.Visible = true;
                label3.Visible = false;
                timer1.Enabled = false;
                panel8.Visible = true;

            }
            if (status == "Restart")
            {
                label3.Visible = true;
                panel3.Visible = true;
                panel4.Visible = true;
                panel5.Visible = true;
                panel6.Visible = true;

                label9.Visible = false;
                label8.Visible = false;
                panel7.Visible = false;
                label6.Text = "УРОВЕНЬ 1";
                panel8.Visible = false;
                label10.Text = "Счет: 0";

                timer1.Enabled = true;
                panel9.Controls.Clear();
            }

            // Сброс различных счетчиков
            mistakes = 0;
            level = 1;
            correctAnswer = 0;
            timer1.Interval = 17;
            firstLabelAchGoal = false;
            DoubleShot.Visible = false;
            correctAnswerForLevelUp = 5;
            firstColumnIsFull = false;
            yAdder = 3;

            // Сброс трех лэйблов в стартовую позицию
            FirstLabel.Visible = false; // на время перемещения выключаем видимость, так мы избавляемся от мерцания во время репоинта локации
            count = 0;
            fontIterator = 81;
            ResetLabel(FirstLabel);

            count2 = 0;
            fontIterator2 = 81;
            ResetLabel(SecondLabel);

            count3 = 0;
            fontIterator3 = 81;
            ResetLabel(ThirdLabel);

            //Создание нового массива
            RandomArr();
        }
        #endregion
    }
}
