using RpsTournament.Core;
using System.Windows;

namespace RpsTournament.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameRound[] rounds = new GameRound[5];
        private int roundCount = 0;

        public MainWindow()
        {
            InitializeComponent();

            PlayerMoveComboBox.ItemsSource = Enum.GetValues<Move>();
        }

        private void PlayRoundButton_Click(object sender, RoutedEventArgs e)
        {
            StatusTextBlock.Text = "";

            string playerName = PlayerNameTextBox.Text.Trim();

            if (playerName.Length < 2 || playerName.Length > 30)
            {
                StatusTextBlock.Text = Properties.Resources.InvalidNameMessage;
                return;
            }

            if (PlayerMoveComboBox.SelectedItem == null)
            {
                StatusTextBlock.Text = Properties.Resources.SelectMoveMessage;
                return;
            }

            Move playerMove = (Move)PlayerMoveComboBox.SelectedItem;

            Move computerMove = GameLogic.GetComputerMove();

            RoundResult result = GameLogic.GetResult(playerMove, computerMove);

            rounds[roundCount] = new GameRound
            {
                Number = roundCount + 1,
                PlayerMove = playerMove,
                ComputerMove = computerMove,
                Result = result
            };

            roundCount++;

            UpdateRoundsDataGrid();
            UpdateScore();

            if (roundCount == 5)
            {
                PlayRoundButton.IsEnabled = false;
                ShowTournamentResult(playerName);
            }
        }

        private void UpdateRoundsDataGrid()
        {
            GameRound[] playedRounds = new GameRound[roundCount];

            for (int i = 0; i < roundCount; i++)
            {
                playedRounds[i] = rounds[i];
            }

            RoundsDataGrid.ItemsSource = playedRounds;
        }

        private void UpdateScore()
        {
            int wins = 0;
            int losses = 0;
            int draws = 0;

            for (int i = 0; i < roundCount; i++)
            {
                if (rounds[i].Result == RoundResult.Win)
                {
                    wins++;
                }
                else if (rounds[i].Result == RoundResult.Loss)
                {
                    losses++;
                }
                else if (rounds[i].Result == RoundResult.Draw)
                {
                    draws++;
                }
            }

            ScoreTextBlock.Text =
                $"Võidud: {wins} | Kaotused: {losses} | Viigid: {draws}";
        }

        private void ShowTournamentResult(string playerName)
        {
            int wins = 0;
            int losses = 0;

            for (int i = 0; i < roundCount; i++)
            {
                if (rounds[i].Result == RoundResult.Win)
                {
                    wins++;
                }
                else if (rounds[i].Result == RoundResult.Loss)
                {
                    losses++;
                }
            }

            if (wins > losses)
            {
                string message = string.Format(
                    Properties.Resources.PlayerWonMessage,
                    playerName);

                MessageBox.Show(message);
            }
            else if (losses > wins)
            {
                MessageBox.Show(Properties.Resources.ComputerWonMessage);
            }
            else
            {
                MessageBox.Show(Properties.Resources.TournamentDrawMessage);
            }
        }

        private void NewTournamentButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show(
                Properties.Resources.NewTournamentConfirmMessage,
                Properties.Resources.NewTournamentConfirmTitle,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (answer == MessageBoxResult.Yes)
            {
                rounds = new GameRound[5];
                roundCount = 0;

                UpdateRoundsDataGrid();
                UpdateScore();

                StatusTextBlock.Text = "";
                PlayRoundButton.IsEnabled = true;
            }
        }
    }
}