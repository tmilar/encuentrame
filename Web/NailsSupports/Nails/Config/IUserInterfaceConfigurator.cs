using System;
using NailsFramework.UserInterface;

namespace NailsFramework.Config
{
    public interface IUserInterfaceConfigurator : INailsConfigurator
    {
        INailsConfigurator Platform<TUIPlatform>(Action<TUIPlatform> ui=null) where TUIPlatform : UIPlatform, new();
        INailsConfigurator Platform(UIPlatform uiPlatform);
    }
}