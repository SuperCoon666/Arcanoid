using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MatildaWinLib;
using System.IO;

namespace Орканоид
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        SuperImage desk; //доска
        SuperImage brick; // кирпич
        int ball_count = 1; //количество шаров
        public List<SuperBall> balls = new List<SuperBall>();
        List<SuperImage> plus = new List<SuperImage>();
        public int bullet_count; //кол-во пуль
        List<SuperImage> bullets = new List<SuperImage>(); //массив пуль
        int drop = 0; //кол-во шаров, которые упали. Нужно для вывода в файл      
        bool color = false; //переменная для замены цвета доски
        int breek=48; // колво кирпичей

        void PaintDesk(SuperImage s, SuperImagePainter b)
        {
            if (color == true)
            {
                b.FillRectangle(0, 0, s.Width, s.Height, Color.Red);
            }
            else
            {
                b.FillRectangle(0, 0, s.Width, s.Height, Color.LightPink); 
            }
        }

        private void Brick_OnPaint(SuperImage s, SuperImagePainter p)
        {
            p.FillRectangle(0, 0, s.Width, s.Height, Color.LightGoldenrodYellow);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < balls.Count; i++)
            {
                balls[i].Top += balls[i].topstep;
                balls[i].Left += balls[i].leftstep;

                if (balls[i].Top <= 0)
                {
                    balls[i].topstep *= -1;
                }
                if (balls[i].Left + balls[i].Width >= ClientRectangle.Width)
                {
                    balls[i].leftstep *= -1;
                }
                if (balls[i].Left <= 0)
                {
                    balls[i].leftstep *= -1;
                }
                if (balls[i].Top >= ClientRectangle.Height)
                {
                    drop++;
                    //Directory.CreateDirectory(@"E:\Группа1\Test"); 
                    //File.AppendAllText(@"E:\Группа1\Test\aaa.txt", "\nGame over" + drop.ToString());
                    //File.WriteAllText(@"E:\Группа1\Test\aaa2.txt", "Game over");
                    //File.WriteAllText(@"E:\Группа1\Test\aaa3.txt", "Game over");

                    balls[i].Dispose();
                    balls.RemoveAt(i);

                    if (drop == ball_count)
                    {
                        
                        
                         label1.Text = "Game Over";
                        timer1.Stop();
                        matilda1.ClearAllImages();
                        timer1.Stop();
                    }
                }
            }

            foreach (SuperImage f in plus)
            {
                f.Top += 5;
            }
            
            //Вот этого цикла у тебя не было. Он нужен для того, чтобы двигать пули
            foreach (SuperImage b in bullets)
            {
                b.Top -= 15;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //Таким образом запрещаем доске вылазить за правую границу экрана
            if (desk != null)
            {
                desk.Left = Math.Min(e.X, ClientRectangle.Width - desk.Width);
            }
        }

        private void matilda1_OnConnection(SuperImage a, SuperImage b)
        {
           if (a.Name == "desk" && b.Name == "ball")
            {
                //if (b.Top + 60 > a.Top)
                //{
                //    ((SuperBall)b).leftstep *= -1;
                //}
                //else
                //{
                //    ((SuperBall)b).topstep *= -1;
                //}


                //Здесь решается задача отталкивания метеорита от доски в правильном направлении
                //Сам алгоритм оценки координат немного сложноват
                //Он зашит внутри метода VerticalConnection из matils'ы
                SuperBall ball;
                ball = b as SuperBall;

                bool x = matilda1.IsVerticalConnection(ball, a);
                if (x == true) //Функция возвращает "правду", если метеорит ударился о верхнюю или нижнюю границы доски
                {
                    ball.topstep *= -1; //В этом случае "отталкиваем" метеорит вверх/вниз
                }
                else //Функция возвращает "ложь", если метеорит ударился о правую или левую границы доски
                {
                    ball.leftstep *= -1; //В этом случае "отталкиваем" метеорит вправо/влево
                }
            }
            if (b.Name == "desk" && a.Name == "ball")
            {
                //if (a.Top + 60 > b.Top)
                //{
                //    ((SuperBall)a).leftstep *= -1;
                //}
                //else
                //{
                //    ((SuperBall)a).topstep *= -1;
                //}

                //Смотри зеркальное условие
                SuperBall ball;
                ball = a as SuperBall;

                bool x = matilda1.IsVerticalConnection(ball, b);
                if (x == true)
                {
                    ball.topstep *= -1;
                }
                else
                {
                    ball.leftstep *= -1;
                }
            }

            if (a.Name == "rock" && b.Name == "ball")
            {
                //((SuperBall)b).topstep *= -1;

                SuperBall ball;
                ball = b as SuperBall;
                breek--;
                label2.Text = breek.ToString();

                //Смотри столкновение метеорита с доской
                bool x = matilda1.IsVerticalConnection(ball, a);
                if (x == true)
                {
                    ball.topstep *= -1;
                }
                else
                {
                    ball.leftstep *= -1;
                }
                a.Dispose();

                int ver = matilda1.RandomInt(1, 4);
                if (ver == 1)
                {
                    SuperImage bonus = matilda1.CreateSuperImage("4_ball.png", b.Left, b.Top, 30, 30, "plus");
                    plus.Add(bonus);
                }
                if (breek <= 0)
                {
                    label1.Text = "You Win";
                    timer1.Stop();
                    matilda1.ClearAllImages();
                    timer1.Stop();
                    matilda1.ClearAllImages();

                }

            }
            if (b.Name == "rock" && a.Name == "ball")
            {
                //((SuperBall)a).topstep *= -1;

                SuperBall ball;
                ball = a as SuperBall;
                breek--;
                label2.Text = breek.ToString();
                //Смотри столкновение метеорита с доской
                bool x = matilda1.IsVerticalConnection(ball, b);
                if (x == true)
                {
                    ball.topstep *= -1;
                }
                else
                {
                    ball.leftstep *= -1;
                }
                b.Dispose();

                int ver = matilda1.RandomInt(1, 4);
                if (ver == 1)
                {
                    SuperImage bonus = matilda1.CreateSuperImage("4_ball.png", b.Left, b.Top, 30, 30, "plus");
                    plus.Add(bonus);
                }
                if (breek <= 0)
                {
                    label1.Text = "You Win";
                    timer1.Stop();
                    matilda1.ClearAllImages();
                    timer1.Stop();
                    matilda1.ClearAllImages();

                }
            }

            if (a.Name == "plus" && b.Name == "desk")
            {
                a.Dispose();
                plus.Remove(a); //Тут ты забыл удалить бонус из массива. Из-за этого он так и остаётся в "мозгах" компьютера. Цикл будет продолжать двигать невидимый шарик вниз.
                color = true;
                bullet_count += 5;
            }
            if (b.Name == "plus" && a.Name == "desk")
            {
                b.Dispose();
                plus.Remove(b); //То же самое, что и в предыдущем "зеркальном" условии
                color = true;
                bullet_count += 5;
            }

            //Кирпичи не сбивались пулями, потому что вообще не было этого куска кода, который за это отвечает
            if (a.Name == "rock" && b.Name == "fire!")
            {
                a.Dispose();
                b.Dispose();
                bullets.Remove(b);
                breek--;
                label2.Text = breek.ToString();
                if (drop == ball_count)
                {


                    label1.Text = "Game Over";
                    timer1.Stop();
                    matilda1.ClearAllImages();
                    timer1.Stop();
                    matilda1.ClearAllImages();

                }
            }
            if (b.Name == "rock" && a.Name == "fire!")
            {
                b.Dispose();
                a.Dispose();
                bullets.Remove(a);
                breek--;
                label2.Text = breek.ToString();
                if (drop == ball_count)
                {


                    label1.Text = "Game Over";
                    timer1.Stop();
                    matilda1.ClearAllImages();
                    timer1.Stop();
                    matilda1.ClearAllImages();
                }
            }
        }

        private void стартToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Start();
            label2.Text = breek.ToString();

            label1.Text = ""; //Чтобы убрать надпись Game Over, когда начинаем игру не в первый раз
            for (int i = 0; i < ball_count; i++)
            {
                SuperBall ball;
                ball = new SuperBall(matilda1, "3_метеорит2.png", ClientRectangle.Width / 2 - 25, ClientRectangle.Height / 2 - 25, 50, 50, "ball");
                ball.topstep = matilda1.RandomInt(-30, 30);
                ball.leftstep = matilda1.RandomInt(-30, 30);
                balls.Add(ball);
            }

            desk = matilda1.CreateSuperImage(null, ClientRectangle.Width / 2 - 50, ClientRectangle.Height - 30 - 30, 100, 30, "desk");
            desk.OnPaint += PaintDesk;

            int brick_top = 33;
            for (int i = 0; i < 3; i++)
            {
                int brick_left = 35;
                for (int j = 0; j < 16; /*именно столько помещается*/ j++)
                {
                    brick = matilda1.CreateSuperImage(null, brick_left, brick_top, 100, 45, "rock");
                    brick.OnPaint += Brick_OnPaint;
                    brick_left += 100 + 20;
                }
                brick_top += 45 + 33;
            }
        }

        public void настройкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fOptions options;
            options = new fOptions();
            options.ShowDialog();
            if (options.ok==true)
            {
                ball_count= options.sar;
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (bullet_count > 0)
            {
                SuperImage fball = matilda1.CreateSuperImage("3_fireball.png", desk.Left + desk.Width / 2 - 30 / 2, desk.Top - 30, 30, 30, "fire!");
                bullets.Add(fball);
                bullet_count--;
            }
            //Тут ты забыл обратно возвращать цвет дощечки после того, как закончатся пули
            else
            {
                color = false;
            }
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Hide();
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Show();
        }
    }
}
 