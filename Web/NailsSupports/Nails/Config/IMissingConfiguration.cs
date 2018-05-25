namespace NailsFramework.Config
{
    public class MissingConfiguration
    {
        public MissingConfiguration(string description)
        {
            Description = description;
        }

        public string Description { get; private set; }
    }
}