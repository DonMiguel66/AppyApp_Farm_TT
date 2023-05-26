namespace Models
{
    public class MainPlayer
    {
        public float Speed { get; }
        public float RotateSmoothing { get; }

        public MainPlayer(float speed, float rotateSmoothing)
        {
            Speed = speed;
            RotateSmoothing = rotateSmoothing;
        }
    }
}