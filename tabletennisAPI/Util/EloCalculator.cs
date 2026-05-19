namespace TableTennisAPI.Util
{
    public static class EloCalculator
    {
        public static (int winnerElo, int loserElo) CalculateEloForSinglesGame(int winnerCurrentElo, int loserCurrentElo)
        {
            return (winnerCurrentElo + 15, loserCurrentElo + 15);
        }
    }
}
