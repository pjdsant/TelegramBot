namespace Telegram
{
    public class EnumStateMachine
    {
        public enum State
        {
            Initial = 0,
            ChangePassword,
            Unlock,
        }

        public enum Step
        {
            User,
            SecretePhraseOne,
            SecretePhraseTwo,
            Password,
            None
        }

    }
}
