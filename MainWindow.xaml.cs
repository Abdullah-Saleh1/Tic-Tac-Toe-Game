using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tic_Tac_Toe_WPF
{
    public partial class MainWindow : Window
    {
        private readonly BitmapImage ImageX = new BitmapImage(new Uri("pack://application:,,,/Resources/X.png"));
        private readonly BitmapImage ImageO = new BitmapImage(new Uri("pack://application:,,,/Resources/O.png"));
        private readonly BitmapImage ImageQM = new BitmapImage(new Uri("pack://application:,,,/Resources/question-mark-96.png"));
        public MainWindow()
        {
            InitializeComponent();
        }

        enum enGameWinner
        {
            Player1 = 'X',
            Player2 = 'O',
            Draw,
            InProgress
        }

        enum enPlayers
        {
            Player1,
            Player2
        }

        enPlayers CurrentTurn = enPlayers.Player1;

        Queue<Border> Player1Queue = new Queue<Border>();
        Queue<Border> Player2Queue = new Queue<Border>();

        void UpdateImageAndTag(object sender)
        {
            var cell = (Border)sender;
            var img = (Image)cell.Child;

            if (CurrentTurn == enPlayers.Player1)
            {
                img.Source = ImageX;
                cell.Tag = "X";
            }
            else
            {
                img.Source = ImageO;
                cell.Tag = "O";
            }
        }

        void SwapTurn()
        {
            CurrentTurn = CurrentTurn == enPlayers.Player1 ? enPlayers.Player2 : enPlayers.Player1;
        }

        void UpdateTurnLabel()
        {
            SwapTurn();
            lblTurn.Text = CurrentTurn == enPlayers.Player1 ? "Player 1: X" : "Player 2: O";
        }

        void ColorWinnerCells(Border cell1, Border cell2, Border cell3)
        {
            var winColor = new SolidColorBrush(Color.FromArgb(255, 35, 255, 0));
            cell1.Background = winColor;
            cell2.Background = winColor;
            cell3.Background = winColor;
        }

        enGameWinner CheckRows()
        {
            if (pb1.Tag.ToString() != "NULL" && pb1.Tag.ToString() == pb2.Tag.ToString() && pb2.Tag.ToString() == pb3.Tag.ToString())
            {
                ColorWinnerCells(pb1, pb2, pb3);
                return pb1.Tag.ToString() == "X" ? enGameWinner.Player1 : enGameWinner.Player2;
            }

            if (pb4.Tag.ToString() != "NULL" && pb4.Tag.ToString() == pb5.Tag.ToString() && pb5.Tag.ToString() == pb6.Tag.ToString())
            {
                ColorWinnerCells(pb4, pb5, pb6);
                return pb4.Tag.ToString() == "X" ? enGameWinner.Player1 : enGameWinner.Player2;
            }

            if (pb7.Tag.ToString() != "NULL" && pb7.Tag.ToString() == pb8.Tag.ToString() && pb8.Tag.ToString() == pb9.Tag.ToString())
            {
                ColorWinnerCells(pb7, pb8, pb9);
                return pb7.Tag.ToString() == "X" ? enGameWinner.Player1 : enGameWinner.Player2;
            }

            return enGameWinner.InProgress;
        }

        enGameWinner CheckColumns()
        {
            if (pb1.Tag.ToString() != "NULL" && pb1.Tag.ToString() == pb4.Tag.ToString() && pb4.Tag.ToString() == pb7.Tag.ToString())
            {
                ColorWinnerCells(pb1, pb4, pb7);
                return pb1.Tag.ToString() == "X" ? enGameWinner.Player1 : enGameWinner.Player2;
            }
            if (pb2.Tag.ToString() != "NULL" && pb2.Tag.ToString() == pb5.Tag.ToString() && pb5.Tag.ToString() == pb8.Tag.ToString())
            {
                ColorWinnerCells(pb2, pb5, pb8);
                return pb2.Tag.ToString() == "X" ? enGameWinner.Player1 : enGameWinner.Player2;
            }
            if (pb3.Tag.ToString() != "NULL" && pb3.Tag.ToString() == pb6.Tag.ToString() && pb6.Tag.ToString() == pb9.Tag.ToString())
            {
                ColorWinnerCells(pb3, pb6, pb9);
                return pb3.Tag.ToString() == "X" ? enGameWinner.Player1 : enGameWinner.Player2;
            }

            return enGameWinner.InProgress;
        }

        enGameWinner CheckDiagonals()
        {
            if (pb1.Tag.ToString() != "NULL" && pb1.Tag.ToString() == pb5.Tag.ToString() && pb5.Tag.ToString() == pb9.Tag.ToString())
            {
                ColorWinnerCells(pb1, pb5, pb9);
                return pb1.Tag.ToString() == "X" ? enGameWinner.Player1 : enGameWinner.Player2;
            }
            if (pb3.Tag.ToString() != "NULL" && pb3.Tag.ToString() == pb5.Tag.ToString() && pb5.Tag.ToString() == pb7.Tag.ToString())
            {
                ColorWinnerCells(pb3, pb5, pb7);
                return pb3.Tag.ToString() == "X" ? enGameWinner.Player1 : enGameWinner.Player2;
            }
            return enGameWinner.InProgress;
        }

        enGameWinner CheckDrawState()
        {
            if (pb1.Tag.ToString() != "NULL" && pb2.Tag.ToString() != "NULL" && pb3.Tag.ToString() != "NULL" &&
                pb4.Tag.ToString() != "NULL" && pb5.Tag.ToString() != "NULL" && pb6.Tag.ToString() != "NULL" &&
                pb7.Tag.ToString() != "NULL" && pb8.Tag.ToString() != "NULL" && pb9.Tag.ToString() != "NULL")
            {
                return enGameWinner.Draw;
            }

            return enGameWinner.InProgress;
        }

        enGameWinner CheckWinner()
        {
            enGameWinner RowWinner = CheckRows();
            if (RowWinner != enGameWinner.InProgress)
                return RowWinner;
            enGameWinner ColumnWinner = CheckColumns();
            if (ColumnWinner != enGameWinner.InProgress)
                return ColumnWinner;

            enGameWinner DiagonalWinner = CheckDiagonals();
            if (DiagonalWinner != enGameWinner.InProgress)
                return DiagonalWinner;

            enGameWinner IsDraw = CheckDrawState();

            return IsDraw;
        }

        void DisableCells()
        {
            pb1.IsHitTestVisible = false;
            pb2.IsHitTestVisible = false;
            pb3.IsHitTestVisible = false;
            pb4.IsHitTestVisible = false;
            pb5.IsHitTestVisible = false;
            pb6.IsHitTestVisible = false;
            pb7.IsHitTestVisible = false;
            pb8.IsHitTestVisible = false;
            pb9.IsHitTestVisible = false;
        }

        void RemoveFirstMove(Queue<Border> Queue)
        {
            Border firstMove = Queue.Dequeue();
            ResetPb(firstMove);
        }

        void FadeFirstMove(Queue<Border> Queue)
        {
            Border firstMove = Queue.Peek();
            UpdateImageOpacity(firstMove);
        }

        void LogMove(object sender)
        {
            var cell = (Border)sender;
            if (CurrentTurn == enPlayers.Player1)
            {
                Player1Queue.Enqueue(cell);

                if (Player1Queue.Count > 3)
                {
                    RemoveFirstMove(Player1Queue);
                }
            }
            else
            {
                Player2Queue.Enqueue(cell);

                if (Player2Queue.Count > 3)
                {
                    RemoveFirstMove(Player2Queue);
                }
            }

            if (Player1Queue.Count == 3 && Player2Queue.Count == 3)
                FadeFirstMove(CurrentTurn == enPlayers.Player1 ? Player2Queue : Player1Queue);
        }

        void UpdateImageOpacity(Border pb)
        {
            pb.Opacity = 0.4;
        }

        void ResetPb(Border pb)
        {
            pb.IsHitTestVisible = true;
            pb.Tag = "NULL";
            pb.Opacity = 1.0;

            var img = (Image)pb.Child;
            img.Source = ImageQM;
            pb.Background = new SolidColorBrush(Color.FromRgb(37, 37, 38));
        }

        void ResetGame()
        {
            ResetPb(pb1);
            ResetPb(pb2);
            ResetPb(pb3);
            ResetPb(pb4);
            ResetPb(pb5);
            ResetPb(pb6);
            ResetPb(pb7);
            ResetPb(pb8);
            ResetPb(pb9);

            CurrentTurn = enPlayers.Player1;
            lblTurn.Text = "Player 1: X";
            lblWinner.Text = "In Progress";

            Player1Queue.Clear();
            Player2Queue.Clear();
        }

        void EndGame(enGameWinner winner)
        {
            lblTurn.Text = "Game Over";
            DisableCells();
            if (winner == enGameWinner.Draw)
            {
                lblWinner.Text = "Match Draw";
                MessageBox.Show("Match Draw", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                lblWinner.Text = winner == enGameWinner.Player1 ? "Player 1" : "Player 2";
                MessageBox.Show(lblWinner.Text + " Is The Winner", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CellClicked(object sender, MouseButtonEventArgs e)
        {
            var cell = (Border)sender;
            if (cell.Tag.ToString() != "NULL")
            {
                MessageBox.Show("This Cell Is Already Filled, Please Select Another Cell", "Cell Already Filled", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LogMove(sender);
            UpdateImageAndTag(sender);
            UpdateTurnLabel();

            enGameWinner winner = CheckWinner();

            if (winner != enGameWinner.InProgress)
            {
                EndGame(winner);
            }
        }

        private void btnRestartGame_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }
    }
}