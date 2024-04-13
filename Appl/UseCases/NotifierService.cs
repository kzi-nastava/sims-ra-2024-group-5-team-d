using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace BookingApp.Appl.UseCases
{
    public class NotifierService
    {
        private Notifier notifier;
        public NotifierService() {
             notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive),
                    corner: Corner.BottomRight,
                    offsetX: 0,
                    offsetY: 0);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });
        }
        public void CreateInstance() {

        }

        public void ShowSuccess(string message)
        {
            notifier.ShowSuccess(message);
        }
        public void ShowError(string message)
        {
            notifier.ShowError(message);
        }
        public void ShowWarning(string message)
        {
            notifier.ShowWarning(message);
        }
        public void ShowInformation(string message)
        {
            notifier.ShowInformation(message);
        }

        /*
    var options = new MessageOptions
    {
        FontSize = 30, // set notification font size
        ShowCloseButton = false, // set the option to show or hide notification close button
        Tag = "Any object or value which might matter in callbacks",
        FreezeOnMouseEnter = true, // set the option to prevent notification dissapear automatically if user move cursor on it
        NotificationClickAction = n => // set the callback for notification click event
        {
            n.Close(); // call Close method to remove notification
            notifier.ShowSuccess("clicked!");
        },
    };
    notifier.ShowSuccess("Success message",options);*/
        // notifier.ShowSuccess("Message");
    }
}
