namespace TheLambClub.Models
{
    public class ChangingMoneyEvent(string name,int money ):EventArgs
    {
        public string? Name { get; set; } = name;
        public int? Money { get; set; } = money;
    }
}
