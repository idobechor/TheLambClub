namespace TheLambClub.Models
{
    public class ChangingMoneyEvent(string name, int money) : EventArgs
    {
        #region properties

        public string? Name { get; set; } = name;
        public int? Money { get; set; } = money;

        #endregion
    }
}
