namespace tonkica.Localization
{
    public interface IIndex
    {
        string Title { get; }
        string TotalIncome { get; }
        string TotalExpenses { get; }
        string Summary { get; }
        string Issued { get; }
        string Income { get; }
        string Limit { get; }
        string NoLimit { get; }
        string TillLimit { get; }
        string Level { get; }
        string NextLevelIn { get; }
        string Profit { get; }
    }
    public class Index_en : IIndex
    {
        public string Title => "Hello World!";
        public string TotalIncome => "Total Income";
        public string TotalExpenses => "Total Expenses";
        public string Summary => "Summary";
        public string Issued => "Issued";
        public string Income => "Income";
        public string Limit => "Limit";
        public string NoLimit => "No";
        public string TillLimit => "Till limit";
        public string Level => "Level";
        public string NextLevelIn => "Next in";
        public string Profit => "Profit";
    }
    public class Index_hr : IIndex
    {
        public string Title => "Pozdrav svijete!";
        public string TotalIncome => "Ukupni prihodi";
        public string TotalExpenses => "Ukupni troškovi";
        public string Summary => "Sažetak";
        public string Issued => "Izdano";
        public string Income => "Prihod";
        public string Limit => "Limit";
        public string NoLimit => "Ne";
        public string TillLimit => "Do limita";
        public string Level => "Razred";
        public string NextLevelIn => "Idući za";
        public string Profit => "Profit";
    }
}