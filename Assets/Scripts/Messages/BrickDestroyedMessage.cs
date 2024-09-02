namespace Messages
{
    public class BrickDestroyedMessage
    {
        public readonly int scoreContribution;

        public BrickDestroyedMessage(int scoreContribution)
        {
            this.scoreContribution = scoreContribution;
        }
    }
}