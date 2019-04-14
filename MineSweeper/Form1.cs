using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace MineSweeper
{
    public partial class Form1 : Form
    {
        Board gameBoard;
        Button[,] boardButton;
        Label[,] boardNumsLabel;

        public Form1()
        {
            InitializeComponent();
        }

        // 게임 초기화

        private void Init()
        {
            label2.Text = " ";
            gameBoard = new Board();
            boardButton = new Button[gameBoard.GetBoardWidth(), gameBoard.GetBoardHeight()];
            boardNumsLabel = new Label[gameBoard.GetBoardWidth(), gameBoard.GetBoardHeight()];

            for (int i = 0; i < gameBoard.GetBoardWidth(); ++i)
            {
                for (int j = 0; j < gameBoard.GetBoardHeight(); ++j)
                {
                    // 지뢰 찾기 버튼? 달기
                    boardButton[i, j] = new Button();
                    boardButton[i, j].BackColor = Color.White;
                    //boardButton[i, j].Text = gameBoard.GetBoardInfo()[i, j].ToString();
                    boardButton[i, j].Location = new Point(10 + i * 50, 100 + j * 50);
                    boardButton[i, j].Size = new Size(45, 45);

                    // 핸들러 장착
                    boardButton[i, j].MouseDown += new MouseEventHandler(button_MouseDown);


                    // 지뢰찾기 판
                    boardNumsLabel[i, j] = new Label();
                    if (gameBoard.GetBoardInfo()[i, j] == -1)
                    {
                        boardNumsLabel[i, j].Text = "*";
                    }
                    else if (gameBoard.GetBoardInfo()[i, j] == 0)
                    {
                        boardNumsLabel[i, j].Text = " ";
                    }
                    else
                    {
                        boardNumsLabel[i, j].Text = gameBoard.GetBoardInfo()[i, j].ToString();
                    }

                    boardNumsLabel[i, j].Location = new Point(10 + i * 50, 100 + j * 50);
                    boardNumsLabel[i, j].Size = new Size(45, 45);
                    boardNumsLabel[i, j].TextAlign = ContentAlignment.MiddleCenter;
                }
            }

            // 폼에 장착
            for (int i = 0; i < gameBoard.GetBoardWidth(); ++i)
            {
                for (int j = 0; j < gameBoard.GetBoardHeight(); ++j)
                {
                    this.Controls.Add(boardNumsLabel[i, j]);
                    this.Controls.SetChildIndex(boardNumsLabel[i, j], 3);
                    this.Controls.Add(boardButton[i, j]);
                    this.Controls.SetChildIndex(boardButton[i, j], 2);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }

        // 버튼 누를 때
        protected void button_MouseDown(object sender, MouseEventArgs e)
        {
            // identify which button was clicked and perform necessary actions
            Button button = sender as Button;
            
            // 왼쪽 버튼 클릭
            if (e.Button == MouseButtons.Left)
            {
                button.Visible = false;

                // 체크
                CheckGameClear();

                if (gameBoard.GetBoardInfo()[(button.Location.X - 10) / 50, (button.Location.Y - 100) / 50] == 0)
                {
                    int[,] direction = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };
                    int x = (button.Location.X - 10) / 50;
                    int y = (button.Location.Y - 100) / 50;

                    // BFS 
                    // 0을 누르면 근처의 0을 전부 다 오픈한다.
                    Queue<Point> que = new Queue<Point>();
                    bool[,] visited = new bool[gameBoard.GetBoardWidth(), gameBoard.GetBoardHeight()];

                    que.Enqueue(new Point(x, y));
                    visited[x, y] = true;

                    while (que.Count != 0)
                    {
                        Point current = que.Dequeue();
                        visited[current.X, current.Y] = true;

                        for (int i = 0; i < 8; ++i)
                        {
                            // 배열 범위 넘어가면 무시
                            if (current.X + direction[i, 0] < 0 || current.Y + direction[i, 1] < 0 ||
                                current.X + direction[i, 0] >= gameBoard.GetBoardWidth() || current.Y + direction[i, 1] >= gameBoard.GetBoardHeight())
                            {
                                continue;
                            }

                            // 버튼이 아직 안 없어졌으면 없애준다.
                            if (boardButton[current.X + direction[i, 0], current.Y + direction[i, 1]].Visible)
                            {
                                // 체크
                                CheckGameClear();
                                boardButton[current.X + direction[i, 0], current.Y + direction[i, 1]].Visible = false;
                            }

                            // 아직 방문 안한 점
                            if (!visited[current.X + direction[i, 0], current.Y + direction[i, 1]] &&
                                gameBoard.GetBoardInfo()[current.X + direction[i, 0], current.Y + direction[i, 1]] == 0)
                            {
                                visited[current.X + direction[i, 0], current.Y + direction[i, 1]] = true;
                                que.Enqueue(new Point(current.X + direction[i, 0], current.Y + direction[i, 1]));
                            }
                        }
                    }

                }
                // 지뢰 누르면 게임 오버
                else if (gameBoard.GetBoardInfo()[(button.Location.X - 10) / 50, (button.Location.Y - 100) / 50] == -1)
                {
                    label2.Text = "Game Over T^T";

                    OpenAllBoard();
                }
            }

            // 오른쪽 버튼 클릭
            else
            {
                if (button.Text == "")
                {
                    button.Text = "★";
                }
                else if (button.Text == "★")
                {
                    button.Text = "?";
                }
                else
                {
                    button.Text = "";
                }
            }
        }

        // 게임 클리어 체크 -> 지뢰빼고 칸을 다 열어봄
        private void CheckGameClear()
        {
            // 칸을 열어본다.
            gameBoard.decreasefindBoardCount();

            label2.Text = "남은 지뢰 수 : " + gameBoard.GetfindBoardCount().ToString();

            if (gameBoard.GetfindBoardCount() <= 0)
            {
                label2.Text = "Game Clear!!!!";
                OpenAllBoard();
            }
        }

        // 모든 게임 판을 열어본다.
        private void OpenAllBoard()
        {
            for (int i = 0; i < gameBoard.GetBoardWidth(); ++i)
            {
                for (int j = 0; j < gameBoard.GetBoardHeight(); ++j)
                {
                    boardButton[i, j].Visible = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Init();
        }
    }
}