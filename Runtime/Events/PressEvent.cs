namespace BananaParty.Input.TVRemote
{
    public class PressEvent
    {
        public float Time { get; private set; }

        public PressEvent(float time)
        {
            Time = time;
        }
    }
}
