using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Life
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int ct = 10;
        private int w;

        /// <summary>
        /// Количество итераций
        /// </summary>
        private int iterNum;

        int l;

        byte[,] map,m1;

        /// <summary>
        /// Массив карты
        /// </summary>
        Rectangle[,] mapRect;

        /// <summary>
        /// Изменение положения таймера
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            iter();
            this.Invalidate();
        }

        /// <summary>
        /// Перезагрузка карты
        /// </summary>
        private void refrBlM()
        {
            for (int i = 0; i < ct; i++)
                for (int j = 0; j < ct; j++)
                    m1[i, j] = getBl(i, j);
        }

        /// <summary>
        /// Загрузка формы
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "Speed : 1";
            label2.Text = "Size : " + ct;
            label3.Text = "Step: " + iterNum.ToString();

            this.Paint += Form1_Paint;
            this.MouseClick += Form1_MouseClick;
            init();
        }

        /// <summary>
        /// Расстановка точки
        /// </summary>
        void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            bool b = true;
            for (int i = 0; b & (i < ct); i++)
                for (int j = 0; b & (j < ct); j++)
                    if (mapRect[i, j].Contains(e.X, e.Y))
                    {
                        map[i, j] = 2;
                        b = false;
                    }
            refrBlM();
            this.Invalidate();
        }

        /// <summary>
        /// Инициализайия карты
        /// </summary>
        private void init()
        {
            iterNum = 0;
            l = (this.Width - this.Height) / 2;
            w = (this.Height - 60) / ct;
            mapRect = new Rectangle[ct, ct];

            map = new byte[ct, ct];
            m1 = new byte[ct, ct];
            for (int i = 0; i < ct; i++)
                for (int j = 0; j < ct; j++)
                {
                    m1[i, j] = 0;
                    map[i, j] = 0;

                    mapRect[i, j] = new Rectangle(l + i * w, 10 + j * w, w, w);
                }
        }

        /// <summary>
        /// Заполнить карту случайными элементами
        /// </summary>
        private void randmz()
        {
            Random r = new Random();
            if (checkBox1.Checked)
              for (int i = 0; i < ct; i++)
                  for (int j = 0; j < ct; j++)
                  {
                      map[i, j] = (byte)r.Next(0, 3);
                      if (map[i, j] == 1)
                          map[i, j] = 0;
                  }
            else
            for (int i = 0; i < ct; i++)
                for (int j = 0; j < ct; j++)                 
                    map[i, j] = (byte)r.Next(0, 3);
            refrBlM();
        }

        /// <summary>
        /// Получает значение для указанной ячейки
        /// </summary>
        private byte getBl(int i,int j)
        {
            byte a = 0;
            if (j > 0)
            {
                if (map[i, j - 1] == 2)
                    a++;
                if (i > 0)
                    if (map[i - 1, j - 1] == 2)
                        a++;
                if (i < ct - 1)
                    if (map[i + 1, j - 1] == 2)
                        a++;
            }

            if (j < ct - 1)
            {
                if (map[i, j + 1] == 2)
                    a++;
                if (i < ct - 1)
                    if (map[i + 1, j + 1] == 2)
                        a++;
                if (i > 0)
                    if (map[i - 1, j + 1] == 2)
                        a++;
            }
            if (i < ct - 1)
                if (map[i + 1, j] == 2)
                    a++;
            if (i > 0)
                if (map[i - 1, j] == 2)
                    a++;
            return a;
        }

        /// <summary>
        /// Главная функция
        /// </summary>
        private void iter()
        {

            refrBlM();

            for (int i = 0; i < ct; i++)
                for (int j = 0; j < ct; j++)
                {
                    if (checkBox1.Checked)
                    {
                        if ((map[i, j] == 0) & (m1[i, j] == 3))
                            map[i, j] = 2;
                        else if ((map[i, j] == 2) & ((m1[i, j] != 3) & (m1[i, j] != 2)))
                            map[i, j] = 0;
                    }
                    else
                    {
                        if (map[i, j] == 0)
                            if (m1[i, j] == 3)
                                map[i, j] = 1;
                            else { }
                        else if (map[i, j] == 1)
                            if ((m1[i, j] == 2) | (m1[i, j] == 3))
                                map[i, j] = 2;
                            else
                                map[i, j] = 0;
                        else if (map[i, j] == 2)
                            if ((m1[i, j] != 2) & (m1[i, j] != 3))
                                map[i, j] = 3;
                            else { }
                        else if (map[i, j] == 3)
                            if ((m1[i, j] != 2) & (m1[i, j] != 3))
                                map[i, j] = 0;
                            else
                                map[i, j] = 2;
                    }
                }

            iterNum++;
            label3.Text = "Step: " + iterNum.ToString();
        }

        /// <summary>
        /// Отрисовка карты
        /// </summary>
        void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < ct; i++)
                for (int j = 0; j < ct; j++)
                {
                    if (map[i, j] == 1)
                        e.Graphics.FillEllipse(Brushes.LightGreen, mapRect[i, j]);
                    else if (map[i, j] == 2)
                        e.Graphics.FillEllipse(Brushes.Green, mapRect[i, j]);
                    if (map[i, j] == 3)
                        e.Graphics.FillEllipse(Brushes.Red, mapRect[i, j]);
                    e.Graphics.DrawRectangle(Pens.White, mapRect[i, j]);

                    if (checkBox2.Checked)
                        e.Graphics.DrawString(m1[i, j].ToString(), SystemFonts.CaptionFont, Brushes.Red, mapRect[i, j]);
                }
        }

        /// <summary>
        /// Кнопка старт/стоп
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                timer1.Enabled = true;
                button1.Text = "Stop";
            }
            else
            {
                timer1.Enabled = false;
                button1.Text = "Start";
            }
        }

        /// <summary>
        /// Очистка поля
        /// </summary>
        private void clr()
        {
            for (int i = 0; i < ct; i++)
                for (int j = 0; j < ct; j++)
                {
                    map[i, j] = 0;
                    m1[i, j] = 0;
                }
        }

        /// <summary>
        /// Кнопка очистки поля
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            clr();
            this.Invalidate();
        }

        /// <summary>
        /// Кнопка Randomize
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            randmz();
            this.Invalidate();
        }

        /// <summary>
        /// Скорость
        /// </summary>
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = "Speed : " + trackBar1.Value;
            timer1.Interval = 1000 / trackBar1.Value;
        }

        /// <summary>
        /// Размер карты
        /// </summary>
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label2.Text = "Size : " + trackBar2.Value;

            timer1.Enabled = false;
            button1.Text = "Start";

            ct = trackBar2.Value;

            init();
            this.Invalidate();
        }

        /// <summary>
        /// Кнопка Шаг
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            iter();
            this.Invalidate();
        }

        /// <summary>
        /// Оригинальная версия
        /// </summary>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            button1.Text = "Start";
            clr();
            this.Invalidate();
        }

        /// <summary>
        /// Показывать числа
        /// </summary>
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
