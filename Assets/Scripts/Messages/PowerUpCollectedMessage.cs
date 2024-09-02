namespace Messages
{
    public class PowerUpCollectedMessage
    {
        public readonly PowerUpScriptable scriptable;

        public PowerUpCollectedMessage(PowerUpScriptable scriptable)
        {
            this.scriptable = scriptable;
        }
    }
}