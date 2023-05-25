using Tools;

namespace Models
{
    public class PlayerProfile
    {
        public MainPlayer CurrentPlayer { get; }

        public PlayerProfile(float playerSpeed)
        {
            CurrentPlayer = new MainPlayer(playerSpeed);
        }
    }
}