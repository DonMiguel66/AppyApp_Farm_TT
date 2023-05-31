namespace Models
{
    public class AviaryModel
    {
        private int _buildCost;
        private int _neededCarrotNumber;
        private int _currentCarrotNumber;
        public int BuildCost
        {
            get => _buildCost;
            set => _buildCost = value;
        }

        public int NeededCarrotNumber
        {
            get => _neededCarrotNumber;
            set => _neededCarrotNumber = value;
        }

        public int CurrentCarrotNumber
        {
            get => _currentCarrotNumber;
            set => _currentCarrotNumber = value;
        }
    }
}