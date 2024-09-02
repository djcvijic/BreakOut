using UnityEngine;

namespace Messages
{
    public class BrickDestroyedMessage
    {
        public readonly int scoreContribution;
        public readonly float powerUpProbability;
        public readonly Vector3 brickPosition;

        public BrickDestroyedMessage(int scoreContribution, float powerUpProbability, Vector3 brickPosition)
        {
            this.scoreContribution = scoreContribution;
            this.powerUpProbability = powerUpProbability;
            this.brickPosition = brickPosition;
        }
    }
}