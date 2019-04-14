using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 오델로
{
    public partial class Form1 : Form
    {
        const int BLACK = 1;
        const int WHITE = 2;
        const int CELL_NUMBER = 11;
        List<Point> dols = new List<Point>();
        int[,] dolsColor;
        bool isGameOver = false;

        public Form1()
        {
            dolsColor = new int[CELL_NUMBER, CELL_NUMBER];

            dolsColor[CELL_NUMBER / 2, CELL_NUMBER / 2] = BLACK;
            dolsColor[CELL_NUMBER / 2 + 1, CELL_NUMBER / 2 + 1] = BLACK;
            dolsColor[CELL_NUMBER / 2, CELL_NUMBER / 2 + 1] = WHITE;
            dolsColor[CELL_NUMBER / 2 + 1, CELL_NUMBER / 2] = WHITE;

            dols.Add(new Point(CELL_NUMBER / 2, CELL_NUMBER / 2));
            dols.Add(new Point(CELL_NUMBER / 2, CELL_NUMBER / 2 + 1));
            dols.Add(new Point(CELL_NUMBER / 2 + 1, CELL_NUMBER / 2));
            dols.Add(new Point(CELL_NUMBER / 2 + 1, CELL_NUMBER / 2 + 1));


            InitializeComponent();


            label1.Text = "BLACK : 2";
            label2.Text = "WHITE : 2";
            label4.Text = "BLACK's turn";
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // 판 그리기
            // 세로
            for (int i = 0; i < CELL_NUMBER; ++i)
            {
                g.DrawLine(new Pen(Color.Black),
                    new Point(Form1.ActiveForm.Size.Width * i / CELL_NUMBER, 0),
                    new Point(Form1.ActiveForm.Size.Width * i / CELL_NUMBER, Form1.ActiveForm.Size.Width - Form1.ActiveForm.Size.Width / CELL_NUMBER));
            }
            // 가로
            for (int i = 0; i < CELL_NUMBER; ++i)
            {
                g.DrawLine(new Pen(Color.Black),
                    new Point(0, Form1.ActiveForm.Size.Width * i / CELL_NUMBER),
                    new Point(Form1.ActiveForm.Size.Width - Form1.ActiveForm.Size.Width / CELL_NUMBER - 2, Form1.ActiveForm.Size.Width * i / CELL_NUMBER));
            }

            // 돌 그리기
            for (int i = 0; i < dols.Count; ++i)
            {
                SolidBrush dolColor;
                if (dolsColor[dols[i].X, dols[i].Y] == BLACK)
                {
                    dolColor = new SolidBrush(Color.Black);
                }
                else
                {
                    dolColor = new SolidBrush(Color.White);
                }

                g.FillEllipse(dolColor, new Rectangle(Form1.ActiveForm.Size.Width * dols[i].X / CELL_NUMBER - Form1.ActiveForm.Size.Width / CELL_NUMBER / 2,
                    Form1.ActiveForm.Size.Width * dols[i].Y / CELL_NUMBER - Form1.ActiveForm.Size.Width / CELL_NUMBER / 2,
                    Form1.ActiveForm.Size.Width / CELL_NUMBER,
                    Form1.ActiveForm.Size.Width / CELL_NUMBER));
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (isGameOver)
            {
                return;
            }

            Point mousePosition = e.Location;

            int x = (mousePosition.X + 10) / (Form1.ActiveForm.Size.Width / CELL_NUMBER);
            int y = (mousePosition.Y + 10) / (Form1.ActiveForm.Size.Width / CELL_NUMBER);

            if (x >= CELL_NUMBER || y >= CELL_NUMBER
                || isDuplicate(new Point(x, y)) || !isAvailableToMove(new Point(x, y), dols.Count % 2 + 1))
            {
                return;
            }

            dols.Add(new Point(x, y));

            if (dols.Count % 2 == 1)
            {
                dolsColor[x, y] = BLACK;
                Reverse(new Point(x, y), dolsColor[x, y]);
            }
            else
            {
                dolsColor[x, y] = WHITE;
                Reverse(new Point(x, y), dolsColor[x, y]);
            }

            CalculateScore();

            // 새로고침
            this.Refresh();

            if (isGameClear())
            {
                int blackCount = 0;
                int whiteCount = 0;

                for (int i = 0; i < CELL_NUMBER; i++)
                {
                    for (int j = 0; j < CELL_NUMBER; j++)
                    {
                        if (dolsColor[i, j] == WHITE)
                        {
                            ++whiteCount;

                        }
                        else if (dolsColor[i, j] == BLACK)
                        {
                            ++blackCount;
                        }
                    }
                }

                if (blackCount > whiteCount)
                {
                    //label1.Text = "BLACK IS WINNER";
                    ShowMsgbox("BLACK IS WINNER");
                }
                else
                {

                    //label1.Text = "WHITE IS WINNER";
                    ShowMsgbox("WHITE IS WINNER");
                }
                isGameOver = true;
            }

            if(dols.Count % 2 == 0)
            {
                label4.Text = "BLACK's turn";
            }
            else
            {
                label4.Text = "WHITE's turn";
            }

        }

        // 놓을 수 있는지 확인
        private bool isAvailableToMove(Point current, int color)
        {
            int[,] dir = new int[8, 2] { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };
            for (int d = 0; d < 8; ++d)
            {
                Boolean blackFlag = false;
                Boolean whiteFlag = false;
                int x = current.X + dir[d, 0];
                int y = current.Y + dir[d, 1];


                if (x >= 0 && x < CELL_NUMBER && y >= 0 && y < CELL_NUMBER && dols.Count % 2 == 0 && dolsColor[x, y] == BLACK)
                {
                    continue ;
                }
                else if(x >= 0 && x < CELL_NUMBER && y >= 0 && y < CELL_NUMBER && dols.Count % 2 == 1 && dolsColor[x, y] == WHITE)
                {
                    continue;
                }

                while (x >= 0 && x < CELL_NUMBER && y >= 0 && y < CELL_NUMBER &&
                    dolsColor[x, y] != 0)
                {

                    if (dolsColor[x, y] == BLACK)
                    {
                        blackFlag = true;
                    }
                    else if(dolsColor[x, y] == WHITE)
                    {
                        whiteFlag = true;
                    }
                    x += dir[d, 0];
                    y += dir[d, 1];
                }

                if(blackFlag && whiteFlag)
                {
                    return true;
                }
            }

            return false;
        }

        private bool isDuplicate(Point p)
        {
            for (int i = 0; i < dols.Count; ++i)
            {
                if (dols[i] == p)
                {
                    return true;
                }
            }
            return false;
        }

        private bool isGameClear()
        {
            // 놓을 데 없을 때 
            for (int x = 0; x < CELL_NUMBER; x++)
            {
                for (int y = 0; y < CELL_NUMBER; y++)
                {
                    if(dolsColor[x, y] != BLACK && dolsColor[x,y] != WHITE)
                    {
                        if (isAvailableToMove(new Point(x, y), dols.Count % 2 == 1 ? WHITE : BLACK))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void ShowMsgbox(string message)
        {
            string caption = "Game Over";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;

            // Displays the MessageBox.

            result = MessageBox.Show(message, caption, buttons);
        }

        private void Reverse(Point current, int color)
        {
            int[,] tempDolsColor = new int[CELL_NUMBER, CELL_NUMBER];

            for (int i = 0; i < CELL_NUMBER; ++i)
            {
                for (int j = 0; j < CELL_NUMBER; j++)
                {
                    tempDolsColor[i, j] = dolsColor[i, j];
                }
            }

            int[,] dir = new int[8, 2] { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };
            for (int d = 0; d < 8; ++d)
            {
                int x = current.X + dir[d, 0];
                int y = current.Y + dir[d, 1];

                while (x >= 0 && x < CELL_NUMBER && y >= 0 && y < CELL_NUMBER &&
                    dolsColor[x, y] != 0)
                {
                    int xx = x - dir[d, 0];
                    int yy = y - dir[d, 1];

                    if (dolsColor[x, y] == color)
                    {
                        while (xx >= 0 && xx < CELL_NUMBER && yy >= 0 && yy < CELL_NUMBER && dolsColor[xx, yy] != 0 && dolsColor[xx, yy] != color)
                        {
                            tempDolsColor[xx, yy] = color;
                            xx -= dir[d, 0];
                            yy -= dir[d, 1];
                        }
                    }

                    x += dir[d, 0];
                    y += dir[d, 1];
                }

            }

            for (int i = 0; i < CELL_NUMBER; ++i)
            {
                for (int j = 0; j < CELL_NUMBER; j++)
                {
                    dolsColor[i, j] = tempDolsColor[i, j];
                }
            }
        }

        private void CalculateScore()
        {
            int blackCount = 0;
            int whiteCount = 0;

            for (int i = 0; i < CELL_NUMBER; i++)
            {
                for (int j = 0; j < CELL_NUMBER; j++)
                {
                    if(dolsColor[i, j] == WHITE)
                    {
                        ++whiteCount;
              
                    }
                    else if(dolsColor[i, j] == BLACK)
                    {
                        ++blackCount;
                    }
                }
            }

            label2.Text = "WHITE : " + whiteCount;
            label1.Text = "BLACK : " + blackCount;
            //label text 수정
        }
    }
}
