using Cores.Base.Interfaces;
using System.Collections.Generic;
using System.Linq;
using TSFramework.Core.Providers;

namespace Modules.Major.Providers
{
    public static class MajorProvider
    {
        public static List<INotify> LoadNotifications(string notificationLibrariesPathFolder)
        {
            return LibraryProvider<INotify>
                .LoadLibrary(notificationLibrariesPathFolder)
                .ToList();
        }
    }
}