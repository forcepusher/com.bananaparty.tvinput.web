namespace BananaParty.Input.TVRemote
{
    public class ReleaseEvent
    {
        public float Time { get; private set; }

        public ReleaseEvent(float time)
        {
            Time = time;
        }
    }
}
