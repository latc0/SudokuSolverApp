using System;
using System.Drawing;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class Form1 : Form
    {
        const int MIN_VAL = 1;
        const int MAX_VAL = 9;
        const int EMPTY = 0;
        const int NUM_VALUES = 9;
        const int GRID_SIZE = 81;
        const int MAX_CELL = 80;

        public Form1()
        {
            InitializeComponent();
            addComponents();

        }

        private void addComponents()
        {
            int gap = 26;
            int x = 12, y = 12;
            Size s = new Size(20, 20);
            for (int i = 1; i <= 81; i++)
            {
                TextBox tb = new TextBox();
                tb.Size = s;
                tb.Location = new Point(x, y);
                this.Controls.Add(tb);
                if (i % 9 == 0)
                {
                    x = 12;
                    if (i % 27 == 0) y = y + gap + 10;
                    else y += gap;
                }
                else if (i % 3 == 0) x = x + gap + 10;
                else x += gap;
            }

            Button b = new Button();
            b.Size = new Size(100, 20);
            b.Location = new Point(80, y);
            b.Text = "Solve!";
            b.Click += B_Click;
            this.Controls.Add(b);
        }

        private void B_Click(object sender, EventArgs e)
        {
            int[] grid = new int[81];
            int i = 0;
            foreach (Control tb in this.Controls)
            {
                if (tb.GetType() == typeof(TextBox))
                {
                    if (tb.Text == string.Empty) grid[i] = 0;
                    else
                    {
                        int num = int.Parse(tb.Text);
                        grid[i] = num;
                    }
                    i++;
                }
            }

            if (hasSolution(grid)) showGame(grid);
        }

        private void showGame(int[] game)
        {
            int i = 0;
            foreach (Control tb in this.Controls)
            {
                if (tb.GetType() == typeof(TextBox))
                {
                    tb.Text = game[i].ToString();
                    i++;
                }
            }
        }

        private bool hasSolution(int[] game)
        {
            if (isFull(game))
            {
                return true;
            }
            else
            {
                int candidateCell = getEmptyCell(game);
                int trialValue = MIN_VAL;
                bool solved = false;

                while (!solved && (trialValue <= MAX_VAL))
                {
                    if (isLegal(game, candidateCell, trialValue))
                    {
                        setCell(game, candidateCell, trialValue);
                        if (hasSolution(game))
                        {
                            return true;
                        }
                        else
                        {
                            clearCell(game, candidateCell);
                        }
                    }
                    trialValue++;
                }
                return solved;
            }
        }

        private bool isLegal(int[] game, int candidateCell, int trialValue)
        {

            // check the column
            int column = candidateCell % 9;//remainder
            int rowAdd = 0;
            while (rowAdd <= MAX_CELL)
            {
                if (game[column + rowAdd] == trialValue)
                    return false;
                rowAdd += 9;
            }

            // check the row
            int rowStartIndex = candidateCell - column; //row start index

            // loop through each cell in the row to check whether value already exists
            for (int i = 0; i < NUM_VALUES; i++)
            {
                if (game[rowStartIndex + i] == trialValue)
                {
                    return false;
                }
            }

            //check box
            int horBox = column - (column % 3);//gives 0,3 or 6 - column index of left of box
            int row = rowStartIndex / 9;//get actual row index of candidate
            int vertBox = row - (row % 3);//0,3 or 6 - row index of top of box

            //get index of cell in top left of relevant box
            int startCell = (vertBox * 9) + horBox;

            //check digits in box
            for (int r = 0; r <= 2; r++)
            {
                for (int c = startCell; c <= startCell + 2; c++)
                {
                    if (game[c] == trialValue) return false;
                }
                startCell += 9;//go to next row
            }

            return true;
        }

        private bool isFull(int[] game)
        {
            for (int i = 0; i <= MAX_CELL; i++)
            {
                if (game[i] == 0)
                    return false;
            }
            return true;
        }

        private void clearCell(int[] game, int location)
        {
            game[location] = EMPTY;
        }

        private void setCell(int[] game, int location, int digit)
        {
            game[location] = digit;
        }

        private int getEmptyCell(int[] game)
        {
            for (int i = 0; i < GRID_SIZE; i++)
            {
                if (game[i] == EMPTY)
                    return i;
            }
            return 0;
        }
    }
}