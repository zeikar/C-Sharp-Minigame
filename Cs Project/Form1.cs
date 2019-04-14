using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cs_Project
{
    public partial class Form1 : Form
    {
        // 클릭한 좌표들 저장
        List<Point> clickPosition = new List<Point>();
        // 클릭한 좌표가 틀린 점이 맞는지 저장
        // 1 : O 맞음
        // 0 : X 틀림
        List<int> cilckPositionisCorrect = new List<int>();

        // 그림에서 틀린 좌표 저장
        Point[] wrongPosition = {   new Point(50, 238), 
                                    new Point(160, 90), 
                                    new Point(65, 117), 
                                    new Point(188, 159), 
                                    new Point(102, 288) };

        // 0이면 게임 중
        // 1이면 게임 종료
        int gameFinished = 0;

        public Form1()// 주함수
        {
            InitializeComponent();//Form1을 초기화 
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < clickPosition.Count; ++i)
            {
                // 클릭한 좌표가 틀린 곳이 맞으면
                if (cilckPositionisCorrect[i] == 1)
                {
                    // 원 그림
                    // DrawEllipse : 원 그리는 함수
                    e.Graphics.DrawEllipse(
                        new Pen(Color.Red, 4f),
                        clickPosition[i].X - pictureBox1.Location.X - 20,
                        clickPosition[i].Y - pictureBox1.Location.Y - 20,
                        40, 40);
                }
                // 틀림 : X 그림
                // DrawLine 함수 사용하세요
                else
                {
                    /* e.Graphics.DrawEllipse(
                        new Pen(Color.Aqua, 2f),
                        clickPosition[i].X - pictureBox1.Location.X - 20,
                        clickPosition[i].Y - pictureBox1.Location.Y - 20,
                        40, 40);
                      */
                    e.Graphics.DrawLine(
                        new Pen(Color.Red, 4f),
                         clickPosition[i].X - pictureBox1.Location.X - 20,
                       clickPosition[i].Y - pictureBox1.Location.Y - 20,
                         clickPosition[i].X - pictureBox1.Location.X + 20,
                       clickPosition[i].Y - pictureBox1.Location.Y + 20
                       );

                    e.Graphics.DrawLine(
                        new Pen(Color.Red, 4f),
                         clickPosition[i].X - pictureBox1.Location.X - 20,
                       clickPosition[i].Y - pictureBox1.Location.Y + 20,
                         clickPosition[i].X - pictureBox1.Location.X + 20,
                       clickPosition[i].Y - pictureBox1.Location.Y - 20
                       );

                }
            }
        }

        bool isRange(Point pos1, Point pos2)
        {
            return Math.Sqrt(Math.Pow(pos1.X - pos2.X, 2) + Math.Pow(pos1.Y - pos2.Y, 2)) <= 25f;
        }

        int getWrongPositionIndex(Point pos)
        {
            int index = -1;

            for (int i = 0; i < wrongPosition.Length; ++i)
            {
                // 범위에 들어가면 찾음.
                if (isRange(wrongPosition[i], pos))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (gameFinished == 1)
            {
                return;
            }

            int isCorrect = 0;


            // 이미 있으면 X
            for (int i = 0; i < clickPosition.Count; ++i)
            {
                if (getWrongPositionIndex(this.PointToClient(Cursor.Position)) != -1 &&
                    getWrongPositionIndex(this.PointToClient(Cursor.Position)) ==
                    getWrongPositionIndex(clickPosition[i]))
                {
                    return;
                }
            }


            // 클릭한 좌표 추가
            clickPosition.Add(this.PointToClient(Cursor.Position));


            for (int i = 0; i < wrongPosition.Length; ++i)
            {
                // 범위에 들어가면 맞다.
                if (isRange(wrongPosition[i], clickPosition[clickPosition.Count - 1]))
                {
                    isCorrect = 1;
                    break;
                }
            }

            cilckPositionisCorrect.Add(isCorrect);

            // 맞은 개수
            int correctCount = 0;

            // 맞은게 5개 이상이면 게임 클리어
            for (int i = 0; i < clickPosition.Count; ++i)
            {
                if (cilckPositionisCorrect[i] == 1)
                {
                    ++correctCount;
                }
            }           

            // 클릭 8번 이상이면 게임 오버
            if (clickPosition.Count == 8)
            {
                label2.Text = "Game Over";
                gameFinished = 1;
            }



            // 디버깅 용 클릭한 좌표 출력
           // label1.Text = clickPosition[clickPosition.Count - 1].ToString();

            // 새로고침
            this.Refresh();

            if (correctCount == 5)
            {
                label2.Text = "Game Clear!";
                gameFinished = 1;

                string message = "다음 스테이지로 가시겠습니까??";
                string caption = "Clear";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {

                    // Closes the parent form.


                    Form2 form2 = new Form2();
                    //this.Hide();
                    form2.Show();


                }
                else
                {
                    this.Close();
                }

            }
        }
    }
}
