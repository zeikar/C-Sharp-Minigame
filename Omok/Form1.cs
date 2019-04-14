using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omok
{
    public partial class Form1 : Form
    {
        int CELL_NUMBER = 20;
        List<Point> dols = new List<Point>();
        bool isGameOver = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
                if (i % 2 == 0)
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
            if(isGameOver)
            {
                return;
            }

            Point mousePosition = e.Location;

            int x = (mousePosition.X + 10) / (Form1.ActiveForm.Size.Width / CELL_NUMBER);
            int y = (mousePosition.Y + 10) / (Form1.ActiveForm.Size.Width / CELL_NUMBER);

            if (x >= CELL_NUMBER || y >= CELL_NUMBER
                || isDuplicate(new Point(x, y)))
            {
                return;
            }

            dols.Add(new Point(x, y));

            // 새로고침
            this.Refresh();

            if(isGameClear())
            {      
                if (dols.Count%2==1)
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
            int[,] board = new int[CELL_NUMBER, CELL_NUMBER];

            for (int i = 0; i < dols.Count; ++i)
            {
                board[dols[i].X, dols[i].Y] = i % 2 + 1;
            }

            // 가로 체크
            for (int i = 0; i <= CELL_NUMBER - 5; ++i)
            {
                for(int j = 0; j < CELL_NUMBER; ++j)
                {
                    bool isSame = true;

                    for (int k = 1; k < 5; ++k)
                    {
                        if (board[i + k, j] == 0 || board[i, j] != board[i + k, j])
                        {
                            isSame = false;
                        }
                    }

                    if(isSame)
                    {
                        return true;
                    }
                }
            }

            // 세로 체크
            for (int i = 0; i < CELL_NUMBER; ++i)
            {
                for (int j = 0; j <= CELL_NUMBER - 5; ++j)
                {
                    bool isSame = true;

                    for (int k = 1; k < 5; ++k)
                    {
                        if (board[i, j + k] == 0 || board[i, j] != board[i, j + k])
                        {
                            isSame = false;
                        }
                    }

                    if (isSame)
                    {
                        return true;
                    }
                }
            }

            // 대각선 \ 체크
            for (int i = 0; i <= CELL_NUMBER - 5; ++i)
            {
                for (int j = 0; j <= CELL_NUMBER - 5; ++j)
                {
                    bool isSame = true;

                    for (int k = 1; k < 5; ++k)
                    {
                        if (board[i + k, j + k] == 0 || board[i, j] != board[i + k, j + k])
                        {
                            isSame = false;
                        }
                    }

                    if (isSame)
                    {
                        return true;
                    }
                }
            }

            // 대각선 / 체크
            for (int i = 0; i <= CELL_NUMBER - 5; ++i)
            {
                for (int j = 4; j < CELL_NUMBER; ++j)
                {
                    bool isSame = true;

                    for (int k = 1; k < 5; ++k)
                    {
                        if (board[i + k, j - k] == 0 || board[i, j] != board[i + k, j - k])
                        {
                            isSame = false;
                        }
                    }

                    if (isSame)
                    {
                        return true;
                    }
                }
            }


            return false;
        }

        private void ShowMsgbox(string message)
        {
            string caption = "Game Over";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;

            // Displays the MessageBox.

            result = MessageBox.Show(message, caption, buttons);
        }
    }
}
