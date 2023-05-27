namespace Models
{
    public class MoneyHolder
    {
        public MoneyHolder(float moneyPickupRadius)
        {
            MoneyPickupRadius = moneyPickupRadius;
        }

        public float MoneyPickupRadius { get; }
    }
}