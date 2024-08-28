namespace Util
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object Lock = new();

        private static T instance;

        public static T Instance
        {
            get
            {
                lock (Lock)
                {
                    return instance ??= new T();
                }
            }
        }
    }
}