using System;
using System.Linq;
using Windows.ApplicationModel.Background;

namespace BackgroundServices
{
    public sealed class ReadAppBackgroundTask : IBackgroundTask
    {

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            throw new NotImplementedException();
        }


        public static async void Register()
        {
            //Recorro todas las task y verifico si se encuentra registrada
            var isRegistered = BackgroundTaskRegistration.AllTasks.Values.Any(
                t => t.Name == nameof(ReadAppBackgroundTask));

            if (isRegistered)
                return;
            //Pedimos acceso para registrarla, si el usuario no acepta se retorna
            if (await BackgroundExecutionManager.RequestAccessAsync()
                == BackgroundAccessStatus.Denied)
                return;

            //Registramos
            var builder = new BackgroundTaskBuilder
            {
                Name = nameof(ReadAppBackgroundTask),
                TaskEntryPoint = $"{nameof(BackgroundServices)}.{nameof(ReadAppBackgroundTask)}"
            };

            builder.SetTrigger(new TimeTrigger(120, false));

            builder.Register();
        }
    }
}
