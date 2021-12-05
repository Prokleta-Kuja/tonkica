using System.Collections.Generic;

namespace tonkica.Data
{
    public class TransactionCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string NameNormalized { get => Name.ToUpperInvariant(); private set { } }

        public ICollection<Transaction> Transactions { get; set; } = null!;
    }
}