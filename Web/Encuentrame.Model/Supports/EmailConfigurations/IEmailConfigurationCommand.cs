namespace Encuentrame.Model.Supports.EmailConfigurations
{
    public interface IEmailConfigurationCommand
    {
        EmailConfiguration Get();
        void Save(EmailConfigurationCommand.CreateOrEditParameters parameters);
    }
}