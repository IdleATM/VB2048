using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VB2048
{
    public partial class MainCode : Form
    {
        private Random rnd = new Random();
        private int[,] currentBoard = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        private int[,] cacheBoard = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        int score = 0;
        int highscore = 0;

        public MainCode()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "Good luck; have fun!";
            textBox2.Text = "0";
            randomizer(ref currentBoard);
            randomizer(ref currentBoard);
            update(ref currentBoard);
        }

        private void move(int direction, ref int[,] lastBoard, ref int[,] board) // 0 is left, 1 is right, 2 is up, 3 is down
        {
            direction %= 4;

            //if (!same(ref lastBoard, ref board))
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        for (int j = 0; j < 4; j++)
            //        {
            //            lastBoard[i, j] = board[i, j];

            //        }
            //    }
            //}

            // moves everything into one direction
            if (!boardFull(ref board))
            {
                for (int i = 0; i < 4; i++)
                {
                    do
                    {
                        for (int j = 1; j < 4; j++)
                        {
                            if (direction < 2)
                            {
                                int y;
                                int yNext;
                                if (direction == 0)
                                {
                                    y = j;
                                    yNext = j - 1;
                                }
                                else
                                {
                                    y = 3 - j;
                                    yNext = 4 - j;
                                }

                                // board moving
                                if (board[i, yNext] == 0)
                                {
                                    board[i, yNext] = board[i, y];
                                    board[i, y] = 0;
                                }
                            }
                            else
                            {
                                int x;
                                int xNext;
                                if (direction == 2)
                                {
                                    x = j;
                                    xNext = j - 1;
                                }
                                else
                                {
                                    x = 3 - j;
                                    xNext = 4 - j;
                                }
                                if (board[xNext, i] == 0)
                                {
                                    board[xNext, i] = board[x, i];
                                    board[x, i] = 0;
                                }
                            }
                        }
                    } while (!shaftHelper(direction, i, ref board));
                }
            }

            // combines everything into one
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    if (direction < 2)
                    {
                        int y;
                        int yNext;
                        if (direction == 0)
                        {
                            y = j;
                            yNext = j - 1;
                        }
                        else
                        {
                            y = 3 - j;
                            yNext = 4 - j;
                        }

                        // board moving
                        if (board[i, yNext] == board[i, y])
                        {
                            board[i, yNext] *= 2;
                            score += board[i, yNext];
                            board[i, y] = 0;
                        }
                    }
                    else
                    {
                        int x;
                        int xNext;
                        if (direction == 2)
                        {
                            x = j;
                            xNext = j - 1;
                        }
                        else
                        {
                            x = 3 - j;
                            xNext = 4 - j;
                        }

                        if (board[xNext, i] == board[x, i])
                        {
                            board[xNext, i] *= 2;
                            score += board[xNext, i];
                            board[x, i] = 0;
                        }
                    }
                }
            }

            // one final push
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    if (direction < 2)
                    {
                        int y;
                        int yNext;
                        if (direction == 0)
                        {
                            y = j;
                            yNext = j - 1;
                        }
                        else
                        {
                            y = 3 - j;
                            yNext = 4 - j;
                        }

                        // board moving
                        if (board[i, yNext] == 0)
                        {
                            board[i, yNext] = board[i, y];
                            board[i, y] = 0;
                        }
                    }
                    else
                    {
                        int x;
                        int xNext;
                        if (direction == 2)
                        {
                            x = j;
                            xNext = j - 1;
                        }
                        else
                        {
                            x = 3 - j;
                            xNext = 4 - j;
                        }
                        if (board[xNext, i] == 0)
                        {
                            board[xNext, i] = board[x, i];
                            board[x, i] = 0;
                        }
                    }
                }
            }
            if (!boardFull(ref board))
                randomizer(ref board);
        }

        private void afterMove(ref int[,] board)
        {
            try
            {
                update(ref board);
                advice();
                //stochasticProgramming();
                winner(ref board);
                if (score > highscore)
                {
                    highscore = score;
                    textBox3.Text = highscore.ToString();
                }
                if (lost(ref board) && boardFull(ref board))
                    textBox1.Text = "You lost.";
                textBox2.Text = score.ToString();
            }
            catch
            {
                textBox1.Text = "Overflow error!!!";
            }
        }

        private void update(ref int[,] board)
        {
            label1.Text = board[0, 0].ToString();
            label2.Text = board[0, 1].ToString();
            label3.Text = board[0, 2].ToString();
            label4.Text = board[0, 3].ToString();
            label5.Text = board[1, 0].ToString();
            label6.Text = board[1, 1].ToString();
            label7.Text = board[1, 2].ToString();
            label8.Text = board[1, 3].ToString();
            label9.Text = board[2, 0].ToString();
            label10.Text = board[2, 1].ToString();
            label11.Text = board[2, 2].ToString();
            label12.Text = board[2, 3].ToString();
            label13.Text = board[3, 0].ToString();
            label14.Text = board[3, 1].ToString();
            label15.Text = board[3, 2].ToString();
            label16.Text = board[3, 3].ToString();
        }

        // implement later
        private string labelConvert(int number)
        {
            if (number == 0)
                return "[   ]";
            else
                return "[" + number.ToString() + "]";
        }

        // checks to see if the board is full
        private bool boardFull(ref int[,] board)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board[i, j] == 0)
                        return false;
                }
            }
            return true;
        }



        // input to give advice
        private void advice()
        {
            int x = rnd.Next(4);
            string direction;
            if (x == 0)
                direction = "Left";
            else if (x == 1)
                direction = "Right";
            else if (x == 2)
                direction = "Up";
            else
                direction = "Down";
            textBox1.Text = direction;
        }


        // exhaust tree using stochastic method of checking up to 6 moves ahead, work in progress
        private void stochasticProgramming()
        {
            int[,] placeholder = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            int[][,] tree = new int[6][,] { returnBoard(), placeholder, placeholder, placeholder, placeholder, placeholder };
            int[] depthTree = new int[6] { 0, 0, 0, 0, 0, 0 };
            int depth = 1;
            int maxDepth = 0;
            int bestAdvice = -1;
            bool movementDepth = false;
            while (depth < 6 || depth > 0)
            {
                Console.WriteLine("{0} {1}", depth, depthTree[depth]);

                // making the assumption that the depth has gone up
                if (depth > 0 && movementDepth)
                {
                    tree[depth] = tree[depth - 1];
                    movementDepth = false;
                }

                if (depth >= 6)
                {
                    bestAdvice = depthTree[0];
                    break;
                }
                else if (depthTree[depth] < 4)
                {
                    Console.WriteLine("a");
                    move(depthTree[depth], ref tree[depth], ref tree[depth]);
                    printBoard(ref tree[depth]);
                    printBoard(ref tree[depth - 1]);
                    // checks to see if the previous inputs are the same
                    if (same(ref tree[depth], ref tree[depth - 1]))
                    {
                        Console.WriteLine("g");
                        if (depth > maxDepth)
                        {
                            bestAdvice = depthTree[0];
                            maxDepth = depth;
                        }
                        depth -= 1;
                        depthTree[depth] += 1;
                        movementDepth = true;
                    }
                    // checks to see if the move is a loss
                    else if (!lost(ref tree[depth]))
                    {
                        depth += 1;
                        movementDepth = true;
                        if (depth < 6)
                            depthTree[depth] = 0;
                        Console.WriteLine("b");
                    }
                    else
                    {
                        depthTree[depth] += 1;
                        Console.WriteLine("c");
                    }
                }
                else if (depth == 0)
                {
                    break;
                }
                else
                {
                    if (depth > maxDepth)
                    {
                        bestAdvice = depthTree[0];
                        maxDepth = depth;
                    }
                    depth -= 1;
                    depthTree[depth] += 1;
                    movementDepth = true;
                }

            }
            //advice(bestAdvice); // change advice function later
        }

        // checks to see if you win (if there exists 2048 on the board)
        private void winner(ref int[,] board)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board[i, j] == 2048)
                        textBox1.Text = "You win!";
                }
            }
        }

        // checks to see if the last board is the same as the current board
        private bool same(ref int[,] lastBoard, ref int[,] board)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board[i, j] != lastBoard[i, j])
                        return false;
                }
            }
            return true;
        }

        // checks to see if you lost (use in conjunction with boardFull())
        private bool lost(ref int[,] board)
        {
            for (int i = 1; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    if (!(board[i, j] != board[i, j - 1] && board[i - 1, j - 1] != board[i - 1, j] && board[i, j] != board[i - 1, j] && board[i - 1, j - 1] != board[i, j - 1]))
                        return false;
                }
            }
            return true;
        }

        // places a 2 (90%) or 4 (10%) randomly in an empty place if there exists somewhere 
        private void randomizer(ref int[,] board)
        {
            bool found = false;
            while (!found)
            {
                int twoOrFour = rnd.Next(10);
                int x = rnd.Next(4);
                int y = rnd.Next(4);
                if (board[x, y] == 0)
                {
                    if (twoOrFour == 6)
                    {
                        board[x, y] = 4;
                    }
                    else
                    {
                        board[x, y] = 2;
                    }
                    found = true;
                }
            }
        }

        // resets all stats except for high score
        private void reset(ref int[,] board)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    board[i, j] = 0;
                }
            }
            score = 0;
            textBox1.Text = "Good luck; have fun!";
            textBox2.Text = "0";
            randomizer(ref currentBoard);
            randomizer(ref currentBoard);
            update(ref currentBoard);
        }

        // helper to shaft
        private bool shaftHelper(int direction, int i, ref int[,] board)
        {
            if (direction == 0)
            {
                return shaft(board[i, 0], board[i, 1], board[i, 2], board[i, 3], ref board);
            }
            else if (direction == 1)
            {
                return shaft(board[i, 3], board[i, 2], board[i, 1], board[i, 0], ref board);
            }
            else if (direction == 2)
            {
                return shaft(board[0, i], board[1, i], board[2, i], board[3, i], ref board);
            }
            else
            {
                return shaft(board[3, i], board[2, i], board[1, i], board[0, i], ref board);
            }
        }

        // checks to see if everything is shafted in one corner
        private bool shaft(int corner, int second, int third, int last, ref int[,] board)
        {
            if (last == 0)
            {
                if (third == 0)
                {
                    if (second == 0)
                    {
                        return true;
                    }
                    else
                    {
                        if (corner == 0)
                            return false;
                    }
                }
                else
                {
                    if (second == 0)
                    {
                        return false;
                    }
                    else
                    {
                        if (corner == 0)
                            return false;
                    }

                }
            }
            else
            {
                if (third == 0)
                {
                    return false;
                }
                else
                {
                    if (second == 0)
                    {
                        return false;
                    }
                    else
                    {
                        if (corner == 0)
                            return false;
                    }

                }
            }
            return true;
        }

        // left
        private void button1_Click(object sender, EventArgs e)
        {
            move(0, ref cacheBoard, ref currentBoard);
            afterMove(ref currentBoard);
        }

        // right
        private void button2_Click(object sender, EventArgs e)
        {
            move(1, ref cacheBoard, ref currentBoard);
            afterMove(ref currentBoard);
        }

        // up
        private void button3_Click(object sender, EventArgs e)
        {
            move(2, ref cacheBoard, ref currentBoard);
            afterMove(ref currentBoard);
        }

        // down
        private void button4_Click(object sender, EventArgs e)
        {
            move(3, ref cacheBoard, ref currentBoard);
            afterMove(ref currentBoard);
        }

        // arrow keys on keyboard
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                move(0, ref cacheBoard, ref currentBoard);
                return true;
            }
            else if (keyData == Keys.Right)
            {
                move(1, ref cacheBoard, ref currentBoard);
                return true;
            }
            else if (keyData == Keys.Up)
            {
                move(2, ref cacheBoard, ref currentBoard);
                return true;
            }
            else if (keyData == Keys.Down)
            {
                move(3, ref cacheBoard, ref currentBoard);
                return true;
            }
            afterMove(ref currentBoard);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // returns the board
        public int[,] returnBoard()
        {
            return currentBoard;
        }

        public void printBoard(ref int[,] board)
        {
            Console.WriteLine("{0}{1}{2}{3}", board[0,0], board[0,1], board[0,2], board[0,3]);
            Console.WriteLine("{0}{1}{2}{3}", board[1,0], board[1,1], board[1,2], board[1,3]);
            Console.WriteLine("{0}{1}{2}{3}", board[2,0], board[2,1], board[2,2], board[2,3]);
            Console.WriteLine("{0}{1}{2}{3}", board[3,0], board[3,1], board[3,2], board[3,3]);
        }

        // in-takes a command
        public void executeCommand(int direction)
        {
            move(direction, ref cacheBoard, ref currentBoard);
            afterMove(ref currentBoard);
        }

        // reset button
        private void button5_Click(object sender, EventArgs e)
        {
            reset(ref currentBoard);
        }

    }
}
