using System;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using Piller.Services;

namespace Piller
{
    public class PillerApp : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.RegisterSingleton<IPermanentStorageService>(new PermanentStorageService());

            RegisterAppStart<ViewModels.RootViewModel>();
        }
    }
}
