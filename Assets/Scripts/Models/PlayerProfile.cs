namespace Models
{
    public class PlayerProfile
    {
        public MainPlayer CurrentPlayer { get; }

        public PlayerProfile(float playerSpeed, float rotateSmoothing)
        {
            CurrentPlayer = new MainPlayer(playerSpeed,rotateSmoothing);
        }
    }
}