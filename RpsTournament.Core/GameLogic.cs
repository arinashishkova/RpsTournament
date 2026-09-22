using System;


namespace RpsTournament.Core
{
    public static class GameLogic
    {
        public static Move GetComputerMove()
        {
            Move[] moves = Enum.GetValues<Move>();

            return moves[Random.Shared.Next(moves.Length)];
        }

        public static RoundResult GetResult(Move player, Move computer)
        {
            if (player == computer)
            {
                return RoundResult.Draw;
            }

            return (player, computer) switch
            {
                (Move.Rock, Move.Scissors) => RoundResult.Win,
                (Move.Paper, Move.Rock) => RoundResult.Win,
                (Move.Scissors, Move.Paper) => RoundResult.Win,

                _ => RoundResult.Loss // other results
            };
        }

        public static int CountResults(
            GameRound[] rounds,
            int roundCount,
            RoundResult result)
        {
            int count = 0;

            for (int i = 0; i < roundCount; i++)
            {
                if (rounds[i].Result == result)
                {
                    count++;
                }
            }

            return count;
        }

        public static RoundResult GetTournamentResult(
            GameRound[] rounds,
            int roundCount)
        {
            int wins = CountResults(
                rounds,
                roundCount,
                RoundResult.Win);

            int losses = CountResults(
                rounds,
                roundCount,
                RoundResult.Loss);

            if (wins > losses)
            {
                return RoundResult.Win;
            }

            if (losses > wins)
            {
                return RoundResult.Loss;
            }

            return RoundResult.Draw;
        }
    }
}