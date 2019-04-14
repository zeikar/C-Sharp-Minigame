using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class Board
    {
        int[,] board;
        int boardHeight, boardWidth;
        const int mineNum = 20;
        int findBoardCount;

        // 생성자, board 변수들 초기화
        public Board()
        {
            // 게임판 크기 설정 30 X 50
            boardHeight = 9;
            boardWidth = 15;
            board = new int[boardWidth, boardHeight];

            // 안 열어본 칸 수
            // 열어본 칸이 전체칸 - 지뢰개수면 게임 클리어!
            // -> 안 열어본 칸이 0이면 게임 클리어!
            findBoardCount = boardWidth * boardHeight - mineNum;

            MakeMines();
            MakeNums();
        }

        // 지뢰를 랜덤으로 생성
        void MakeMines()
        {
            Random rnd = new Random();
            for (int i = 0; i < mineNum; ++i)
            {
                int x = rnd.Next(0, boardWidth);
                int y = rnd.Next(0, boardHeight);

                if (board[x, y] == 0)
                {
                    board[x, y] = -1;
                }
                else
                {
                    --i;
                }
            }
        }

        // 게임판의 숫자를 생성 (주변의 지뢰 개수)
        void MakeNums()
        {
            int[,] direction = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

            for (int i = 0; i < boardWidth; ++i)
            {
                for (int j = 0; j < boardHeight; ++j)
                {
                    // 근처의 지뢰 개수 count
                    int mineCount = 0;

                    // 지뢰는 번호 표시 X
                    if (board[i, j] == -1)
                    {
                        continue;
                    }

                    for (int k = 0; k < 8; ++k)
                    {
                        // 배열 범위 넘어가면 무시
                        if (i + direction[k, 0] < 0 || j + direction[k, 1] < 0 ||
                            i + direction[k, 0] >= boardWidth || j + direction[k, 1] >= boardHeight)
                        {
                            continue;
                        }

                        // 지뢰가 근처에 있으면
                        if (board[i + direction[k, 0], j + direction[k, 1]] == -1)
                        {
                            ++mineCount;
                        }
                    }

                    board[i, j] = mineCount;
                }
            }
        }

        public int[,] GetBoardInfo()
        {
            return board;
        }

        public int GetBoardHeight()
        {
            return boardHeight;
        }

        public int GetBoardWidth()
        {
            return boardWidth;
        }

        public int GetfindBoardCount()
        {
            return findBoardCount;
        }

        public void decreasefindBoardCount()
        {
            --findBoardCount;
        }
    }
}
