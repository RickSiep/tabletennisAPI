namespace TableTennisAPI.Util
{
    public static class EloCalculator
    {
        public static (double winnerElo, double loserElo) CalculateEloForSinglesGame(double winnerCurrentElo, double loserCurrentElo)
        {
            double playerAChanceOfWinning = 1.00 / (1.00 + (Math.Pow(10.00, (loserCurrentElo - winnerCurrentElo) / 400)));
            double playerBChanceOfWinning = 1 - playerAChanceOfWinning;

            var newRatingPlayerA = winnerCurrentElo + 32 * (1 - playerAChanceOfWinning);
            var newRatingPlayerB = loserCurrentElo + 32 * (0 - playerBChanceOfWinning);

            return (newRatingPlayerA, newRatingPlayerB);
        }
    }
}
