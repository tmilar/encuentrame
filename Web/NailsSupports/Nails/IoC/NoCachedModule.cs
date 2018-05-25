namespace NailsFramework.IoC
{
    public class NoSingletonLemmingAttribute : LemmingAttribute
    {
        public NoSingletonLemmingAttribute()
        {
            Singleton = false;
        }
    }
}