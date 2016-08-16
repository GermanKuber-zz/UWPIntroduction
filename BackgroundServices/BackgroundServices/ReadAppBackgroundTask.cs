﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Common.Repositorys;
using Common.ViewModels;

namespace BackgroundServices
{
    public sealed class ReadAppBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            var settings = new ConfigurationsViewModel();

            if (settings.NotificationsEnabled)
                await SendNotificationAsync();

 

            _deferral.Complete();
        }

        private async Task SendNotificationAsync()
        {
            var noticeInDay = await ReadResitory.GetNoticesInDay();
            if (noticeInDay == null)
                return;

            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
            XmlDocument content = ToastNotificationManager.GetTemplateContent(
                    ToastTemplateType.ToastText02);

            var texts = content.GetElementsByTagName("text");

            texts[0].InnerText = "Usted tienen algunas noticias para el dia de hoy!";

            texts[1].InnerText = $"Usted tiene un total de  {noticeInDay.Count} noticias!";

            notifier.Show(new ToastNotification(content));
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
