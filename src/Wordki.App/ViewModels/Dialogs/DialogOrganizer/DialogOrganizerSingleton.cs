namespace Wordki.ViewModels.Dialogs
{
    public class DialogOrganizerSingleton
    {

        private static IDialogOrganizer instance;
        private static object lockObj = new object();

        public static IDialogOrganizer Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new DialogOrganizer();
                    }
                    return instance;
                }
            }
        }

    }
}
