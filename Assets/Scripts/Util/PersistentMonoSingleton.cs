public abstract class PersistentMonoSingleton<T> : MonoSingleton<T>, IPersistentMonoSingleton
    where T : PersistentMonoSingleton<T> { }