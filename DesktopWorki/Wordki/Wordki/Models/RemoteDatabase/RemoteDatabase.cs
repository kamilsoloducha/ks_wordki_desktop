using System;
using System.Globalization;
using Wordki.Helpers.Command;
using Wordki.Models.Connector;

namespace Wordki.Models.RemoteDatabase
{
    public class RemoteDatabase : RemoteDatabaseBase
    {

        public override CommandQueue<ICommand> GetDownloadQueue()
        {
            DateTime downloadDateTime = new DateTime();
            CommandQueue<ICommand> lQueue = new CommandQueue<ICommand>();
            lQueue.MainQueue.AddLast(new CommandApiRequest(new ApiRequestGetDateTime(UserManager.GetInstance().User))
            {
                OnCompleteCommand = response =>
                {
                    if (response.IsError)
                    {
                        return;
                    }
                    if (response.Message == null)
                    {
                        return;
                    }
                    try
                    {
                        downloadDateTime = DateTime.ParseExact(response.Message,
                          "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB"));
                    }
                    catch (Exception)
                    {
                        string[] dateTimeElements = response.Message.Split(new[] { '|', ' ', ':' });
                        if (dateTimeElements.Length != 6)
                        {
                            return;
                        }
                        downloadDateTime = new DateTime(int.Parse(dateTimeElements[2]),
                          int.Parse(dateTimeElements[1]),
                          int.Parse(dateTimeElements[0]),
                          int.Parse(dateTimeElements[3]),
                          int.Parse(dateTimeElements[4]),
                          int.Parse(dateTimeElements[5]));
                    }
                }
            });
            lQueue.MainQueue.AddLast(new CommandApiRequest(new ApiRequestGetGroups(UserManager.GetInstance().User)) { OnCompleteCommand = Database.GetDatabase().OnReadGroups });
            lQueue.MainQueue.AddLast(new CommandApiRequest(new ApiRequestGetWords(UserManager.GetInstance().User)) { OnCompleteCommand = Database.GetDatabase().OnReadWords });
            lQueue.MainQueue.AddLast(new CommandApiRequest(new ApiRequestGetResults(UserManager.GetInstance().User)) { OnCompleteCommand = Database.GetDatabase().OnReadResults });
            lQueue.OnQueueComplete += success =>
            {
                if (success)
                {
                    User user = UserManager.GetInstance().User;
                    user.DownloadTime = downloadDateTime;
                    Database.GetDatabase().UpdateUser(user);
                    Database.GetDatabase().LoadDatabase();
                }
            };
            return lQueue;
        }

        public override CommandQueue<ICommand> GetUploadQueue()
        {
            CommandQueue<ICommand> lQueue = new CommandQueue<ICommand>();
            lQueue.MainQueue.AddLast(new CommandApiRequest(new ApiRequestPutGroups(Database.GetDatabase())));
            lQueue.MainQueue.AddLast(new CommandApiRequest(new ApiRequestPutWords(Database.GetDatabase())));
            lQueue.MainQueue.AddLast(new CommandApiRequest(new ApiRequestPutResults(Database.GetDatabase())));
            lQueue.OnQueueComplete += success =>
            {
                if (success)
                {
                    Database.GetDatabase().RefreshDatabase();
                    foreach (Group group in Database.GetDatabase().GroupsList)
                    {
                        group.State = 0;
                        foreach (Word word in group.WordsList)
                        {
                            word.State = 0;
                        }
                        foreach (Result result in group.ResultsList)
                        {
                            result.State = 0;
                        }
                    }
                }
            };
            return lQueue;
        }
    }



}
