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
            int wins = GameLogic.CountResults(
                rounds,
                roundCount,
                RoundResult.Win);

            int losses = GameLogic.CountResults(
                rounds,
                roundCount,
                RoundResult.Loss);

            int draws = GameLogic.CountResults(
                rounds,
                roundCount,
                RoundResult.Draw);

            ScoreTextBlock.Text =
                $"Võidud: {wins} | Kaotused: {losses} | Viigid: {draws}";
        }

        private void ShowTournamentResult(string playerName)
        {
            RoundResult tournamentResult =
                GameLogic.GetTournamentResult(rounds, roundCount);

            if (tournamentResult == RoundResult.Win)
            {
                string message = string.Format(
                    Properties.Resources.PlayerWonMessage,
                    playerName);

                MessageBox.Show(message);
            }
            else if (tournamentResult == RoundResult.Loss)
            {
                MessageBox.Show(
                    Properties.Resources.ComputerWonMessage);
            }
            else
            {
                MessageBox.Show(
                    Properties.Resources.TournamentDrawMessage);
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