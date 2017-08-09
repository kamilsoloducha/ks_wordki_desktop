using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Repository.Models.Enums;
using Repository.Models.Language;
using Wordki.Helpers;
using Wordki.Models;

namespace Wordki.LocalDatabase {
  public class SqliteConnection {

    private const string DatabasePath = "Wordki.sqlite";
    private const string directory = "LocalDatabase";
    private const string creation_directory = "Sql";
    private const string creation_tables_file = "createTables";

    private const string usersTab = "users";
    private const string groupsTab = "groups";
    private const string wordsTab = "words";
    private const string resultsTab = "results";

    //pola users_tab
    private const string userId = "userId";
    private const string login = "userName";
    private const string password = "password";
    private const string downloadDate = "downloadDate";
    private const string translationDireciton = "translationDireciton";
    private const string allWords = "allWords";
    private const string timeOut = "timeOut";
    private const string apiKey = "apiKey";
    private const string isRegister = "isRegister";

    //pola groups_tab
    private const string groupId = "groupId";
    private const string groupName = "groupName";
    private const string language1Type = "language1Type";
    private const string language2Type = "language2Type";
    private const string state = "state";

    //pola words_tab
    private const string wordId = "wordId";
    private const string language1 = "language1";
    private const string language2 = "language2";
    private const string drawer = "drawer";
    private const string language1Comment = "language1Comment";
    private const string language2Comment = "language2Comment";
    private const string visible = "visible";

    //pola results_tab
    private const string resultId = "resultId";
    private const string correct = "correct";
    private const string accepted = "accepted";
    private const string wrong = "wrong";
    private const string invisibilities = "invisibilities";
    private const string timeCount = "timeCount";
    private const string direction = "direction";
    private const string lessonType = "lessonType";
    private const string datetime = "dateTime";

    private readonly SQLiteConnection _connection;

    public SqliteConnection() {
      _connection = new SQLiteConnection(String.Format("Data Source={0};Version=3;", DatabasePath));

    }

    public void CreateDatabase() {
      if (File.Exists(DatabasePath))
        return;
      SQLiteConnection.CreateFile(DatabasePath);

    }

    public void CreateTables() {
      if (!File.Exists(Path.Combine(directory, creation_directory, creation_tables_file))) {
        Logger.LogError("Brak pliku - {0}", Path.Combine(directory, creation_directory, creation_tables_file));
        return;
      }

      string lCreateQuery;
      using (StreamReader lReader = new StreamReader(Path.Combine(directory, creation_directory, creation_tables_file))) {
        lCreateQuery = lReader.ReadToEnd();
      }
      lCreateQuery = string.Format(lCreateQuery, Util.GetExeFilePath());
      SQLiteCommand lCommand = new SQLiteCommand(lCreateQuery, _connection);
      _connection.Open();
      lCommand.ExecuteNonQuery();
      _connection.Close();
    }

    public void OpenConnection() {
      _connection.Open();
    }

    public void CloseConnection() {
      _connection.Close();
    }

    public List<User> SelectUsers() {
      StringBuilder builder = new StringBuilder();
      builder.Append("SELECT * FROM ").Append(usersTab);

      return ExecuteUserSelect(builder.ToString());
    }

    private List<User> ExecuteUserSelect(string query) {
      List<User> users = new List<User>();
      try {
        using (SQLiteCommand command = new SQLiteCommand(query, _connection))
        using (var reader = command.ExecuteReader()) {
          while (reader.Read()) {
            int index = 0;
            User user = new User {
              UserId = reader.GetInt64(index++),
              Name = reader.GetString(index++),
              Password = reader.GetString(index),
            };
            users.Add(user);
          }
        }
      } catch (Exception e) {
        Logger.LogError(String.Format("{0} - Command: {1} - {2}"), "ConnectorDatabase.ExecuteUserSelect", query, e.Message);
      }
      return users;
    }

    public List<Group> SelectGroupsToSend(long pUserId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(groupsTab)
        .Append(" WHERE ")
        .Append(userId)
        .Append(" = ")
        .Append(pUserId)
        .Append(" AND ")
        .Append(state)
        .Append(" != ")
        .Append(0);
      return ExecuteGroupsSelect(lBuilder.ToString());
    }

    public List<Group> SelectGroupList(long pUserId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(groupsTab)
        .Append(" WHERE ")
        .Append(userId)
        .Append(" = ")
        .Append(pUserId)
        .Append(" AND ")
        .Append(state)
        .Append(" >= ")
        .Append(0);
      return ExecuteGroupsSelect(lBuilder.ToString());
    }

    public async Task<List<Group>> SelectGroupListAsync(long pUserId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(groupsTab)
        .Append(" WHERE ")
        .Append(userId)
        .Append(" = ")
        .Append(pUserId)
        .Append(" AND ")
        .Append(state)
        .Append(" >= ")
        .Append(0);
      return await ExecuteGroupsSelectAsync(lBuilder.ToString());
    }

    private async Task<List<Group>> ExecuteGroupsSelectAsync(string pQuery) {
      List<Group> lGroupList = new List<Group>();
      SQLiteCommand lCommand = new SQLiteCommand(pQuery, _connection);
      try {
        using (var lReader = await lCommand.ExecuteReaderAsync()) {
          while (await lReader.ReadAsync()) {
            Group lGroup = new Group();
            lGroup.Id = lReader.GetInt64(0);
            lGroup.Name = lReader.GetString(2);
            lGroup.Language1 = LanguageFactory.GetLanguage((LanguageType)lReader.GetInt32(3));
            lGroup.Language2 = LanguageFactory.GetLanguage((LanguageType)lReader.GetInt32(4));
            lGroup.State = lReader.GetInt32(5);
            lGroupList.Add(lGroup);
          }
        }
      } catch (SqlException lException) {
        Logger.LogError(String.Format("{0} - Command: {1} - {2}"), "ConnectorDatabase.SelectGroupList", pQuery, lException.Message);
      }
      return lGroupList;
    }

    private List<Group> ExecuteGroupsSelect(string pQuery) {
      List<Group> lGroupList = new List<Group>();
      SQLiteCommand lCommand = new SQLiteCommand(pQuery, _connection);
      try {
        using (var lReader = lCommand.ExecuteReader()) {
          while (lReader.Read()) {
            Group lGroup = new Group();
            lGroup.Id = lReader.GetInt64(0);
            lGroup.Name = lReader.GetString(2);
            lGroup.Language1 = LanguageFactory.GetLanguage((LanguageType)lReader.GetInt32(3));
            lGroup.Language2 = LanguageFactory.GetLanguage((LanguageType)lReader.GetInt32(4));
            lGroup.State = lReader.GetInt32(5);
            lGroupList.Add(lGroup);
          }
        }
      } catch (SqlException lException) {
        Logger.LogError(String.Format("{0} - Command: {1} - {2}"), "ConnectorDatabase.SelectGroupList", pQuery, lException.Message);
      }
      return lGroupList;
    }

    public List<Word> SelectWordToSend(long pUserId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(wordsTab)
        .Append(" WHERE ")
        .Append(userId)
        .Append(" = ")
        .Append(pUserId)
        .Append(" AND ")
        .Append(state)
        .Append(" != ")
        .Append(0);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      return ExecuteWordsSelect(lCommand);
    }

    public List<Word> SelectWordListByUserId(long pUserId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(wordsTab)
        .Append(" WHERE ")
        .Append(userId)
        .Append(" = ")
        .Append(pUserId)
        .Append(" AND ")
        .Append(state)
        .Append(" >= ")
        .Append(0);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      return ExecuteWordsSelect(lCommand);
    }

    public List<Word> SelectWordListByGroupId(long pGroupId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(wordsTab)
        .Append(" WHERE ")
        .Append(groupId)
        .Append(" = ")
        .Append(pGroupId)
        .Append(" AND ")
        .Append(state)
        .Append(" >= ")
        .Append(0);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      return ExecuteWordsSelect(lCommand);
    }

    public async Task<List<Word>> SelectWordListByGroupIdAsync(long pGroupId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(wordsTab)
        .Append(" WHERE ")
        .Append(groupId)
        .Append(" = ")
        .Append(pGroupId)
        .Append(" AND ")
        .Append(state)
        .Append(" >= ")
        .Append(0);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      return await ExecuteWordsSelectAsync(lCommand);
    }

    private List<Word> ExecuteWordsSelect(SQLiteCommand pCommand) {
      List<Word> lWordList = new List<Word>();
      try {
        using (SQLiteDataReader lReader = pCommand.ExecuteReader()) {
          while (lReader.Read()) {
            Word lWord = new Word();
            lWord.Id = lReader.GetInt64(0);
            lWord.GroupId = lReader.GetInt64(2);
            if (!lReader.IsDBNull(3))
              lWord.Language1 = lReader.GetString(3);
            if (!lReader.IsDBNull(4))
              lWord.Language2 = lReader.GetString(4);
            lWord.Drawer = lReader.GetByte(5);
            if (!lReader.IsDBNull(6))
              lWord.Language1Comment = lReader.GetString(6);
            if (!lReader.IsDBNull(7))
              lWord.Language2Comment = lReader.GetString(7);
            lWord.Visible = lReader.GetInt32(8) == 1;
            lWord.State = lReader.GetInt32(9);
            lWordList.Add(lWord);
          }
        }
      } catch (SqlException lException) {
        Logger.LogError(String.Format("{0} - Command: {1} - {2}"), "ConnectorDatabase.SelectGroupList", pCommand.CommandText, lException.Message);
      }
      return lWordList;
    }

    private async Task<List<Word>> ExecuteWordsSelectAsync(SQLiteCommand pCommand) {
      List<Word> lWordList = new List<Word>();
      try {
        using (DbDataReader lReader = await pCommand.ExecuteReaderAsync()) {
          while (lReader.Read()) {
            Word lWord = new Word();
            lWord.Id = lReader.GetInt64(0);
            lWord.GroupId = lReader.GetInt64(2);
            if (!lReader.IsDBNull(3))
              lWord.Language1 = lReader.GetString(3);
            if (!lReader.IsDBNull(4))
              lWord.Language2 = lReader.GetString(4);
            lWord.Drawer = lReader.GetByte(5);
            if (!lReader.IsDBNull(6))
              lWord.Language1Comment = lReader.GetString(6);
            if (!lReader.IsDBNull(7))
              lWord.Language2Comment = lReader.GetString(7);
            lWord.Visible = lReader.GetInt32(8) == 1;
            lWord.State = lReader.GetInt32(9);
            lWordList.Add(lWord);
          }
        }
      } catch (SqlException lException) {
        Logger.LogError(String.Format("{0} - Command: {1} - {2}"), "ConnectorDatabase.SelectGroupList", pCommand.CommandText, lException.Message);
      }
      return lWordList;
    }

    public List<Result> SelectResultToSend(long pUserId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(resultsTab)
        .Append(" WHERE ")
        .Append(userId)
        .Append(" = ")
        .Append(pUserId)
        .Append(" AND ")
        .Append(state)
        .Append(" != ")
        .Append(0);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      return ExecuteResultSelect(lCommand);
    }

    public List<Result> SelectResultsListByUserId(long pUserId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(resultsTab)
        .Append(" WHERE ")
        .Append(userId)
        .Append(" = ")
        .Append(pUserId)
        .Append(" AND ")
        .Append(state)
        .Append(" >= ")
        .Append(0);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      return ExecuteResultSelect(lCommand);
    }


    public List<Result> SelectResultsListByGroupId(long pGroupId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(resultsTab)
        .Append(" WHERE ")
        .Append(groupId)
        .Append(" = ")
        .Append(pGroupId)
        .Append(" AND ")
        .Append(state)
        .Append(" >= ")
        .Append(0);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      return ExecuteResultSelect(lCommand);
    }

    public async Task<List<Result>> SelectResultsListByGroupIdAsync(long pGroupId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(resultsTab)
        .Append(" WHERE ")
        .Append(groupId)
        .Append(" = ")
        .Append(pGroupId)
        .Append(" AND ")
        .Append(state)
        .Append(" >= ")
        .Append(0);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      return await ExecuteResultSelectAsync(lCommand);
    }

    private List<Result> ExecuteResultSelect(SQLiteCommand pCommand) {
      List<Result> lResultList = new List<Result>();
      try {
        using (SQLiteDataReader lReader = pCommand.ExecuteReader()) {
          while (lReader.Read()) {
            lResultList.Add(new Result(
              lReader.GetInt64(0),
              lReader.GetInt64(2),
              lReader.GetInt16(3),
              lReader.GetInt16(4),
              lReader.GetInt16(5),
              lReader.GetInt16(6),
              lReader.GetInt16(7),
              (TranslationDirection)lReader.GetInt32(8),
              (LessonType)lReader.GetInt32(9),
              lReader.GetDateTime(10),
              lReader.GetInt32(11)
              ));
          }
        }
      } catch (SqlException lException) {
        Logger.LogError(String.Format("{0} - Command: {1} - {2}"), "ConnectorDatabase.SelectGroupList", pCommand.CommandText, lException.Message);
      }
      return lResultList;
    }

    private async Task<List<Result>> ExecuteResultSelectAsync(SQLiteCommand pCommand) {
      List<Result> lResultList = new List<Result>();
      try {
        using (DbDataReader lReader = await pCommand.ExecuteReaderAsync()) {
          while (lReader.Read()) {
            lResultList.Add(new Result(
              lReader.GetInt64(0),
              lReader.GetInt64(2),
              lReader.GetInt16(3),
              lReader.GetInt16(4),
              lReader.GetInt16(5),
              lReader.GetInt16(6),
              lReader.GetInt16(7),
              (TranslationDirection)lReader.GetInt32(8),
              (LessonType)lReader.GetInt32(9),
              lReader.GetDateTime(10),
              lReader.GetInt32(11)
              ));
          }
        }
      } catch (SqlException lException) {
        Logger.LogError(String.Format("{0} - Command: {1} - {2}"), "ConnectorDatabase.SelectGroupList", pCommand.CommandText, lException.Message);
      }
      return lResultList;
    }

    public List<long> SelectUserIdsList() {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ").Append(usersTab);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      List<long> lIdsList = new List<long>();
      using (SQLiteDataReader lReader = lCommand.ExecuteReader()) {
        while (lReader.Read()) {
          lIdsList.Add(lReader.GetInt64(0));
        }
      }
      return lIdsList;
    }

    public User SelectUser(string pUserName, string pPassword) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(usersTab)
        .Append(" WHERE ")
        .Append(login)
        .Append(" LIKE ")
        .Append("\'").Append(pUserName).Append("\'")
        .Append(" AND ")
        .Append(password)
        .Append(" LIKE ")
        .Append("\'").Append(pPassword).Append("\'");
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      return ExecuteSelectUser(lCommand);
    }

    public User SelectUser(long pUserId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(usersTab)
        .Append(" WHERE ")
        .Append(userId)
        .Append(" = ")
        .Append(pUserId);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      return ExecuteSelectUser(lCommand);
    }

    public User ExecuteSelectUser(SQLiteCommand pCommand) {
      User lUser = null;
      using (SQLiteDataReader lReader = pCommand.ExecuteReader()) {
        while (lReader.Read()) {
          lUser = new User();
          int index = 0;
          lUser.UserId = lReader.GetInt64(index++);
          lUser.Name = lReader.GetString(index++);
          lUser.Password = lReader.GetString(index++);
          lUser.DownloadTime = lReader.GetDateTime(index++);
          lUser.TranslationDirection = (TranslationDirection)lReader.GetInt32(index++);
          lUser.AllWords = lReader.GetBoolean(index++);
          lUser.Timeout = lReader.GetInt32(index++);
          lUser.ApiKey = lReader.GetString(index++);
          lUser.IsRegister = lReader.GetBoolean(index);
        }
      }
      return lUser;
    }

    private SQLiteCommand GetInsertGroupCommand(Group pGroup, User pUser) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("INSERT INTO ")
        .Append(groupsTab)
        .Append(" VALUES ( ")
        .Append("@").Append(groupId).Append(", ")
        .Append("@").Append(userId).Append(", ")
        .Append("@").Append(groupName).Append(", ")
        .Append("@").Append(language1Type).Append(", ")
        .Append("@").Append(language2Type).Append(", ")
        .Append("@").Append(state)
        .Append(" ) ");
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      lCommand.Parameters.Add(new SQLiteParameter(groupId, pGroup.Id));
      lCommand.Parameters.Add(new SQLiteParameter(userId, pUser.UserId));
      lCommand.Parameters.Add(new SQLiteParameter(groupName, pGroup.Name));
      lCommand.Parameters.Add(new SQLiteParameter(language1Type, (int)pGroup.Language1.Type));
      lCommand.Parameters.Add(new SQLiteParameter(language2Type, (int)pGroup.Language2.Type));
      lCommand.Parameters.Add(new SQLiteParameter(state, pGroup.State));
      return lCommand;
    }

    public int InsertGroup(Group pGroup, User pUser) {
      try {
        return GetInsertGroupCommand(pGroup, pUser).ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.InsertGroup", lException.Message));
        return -1;
      }
    }

    public async Task<int> InsertGroupAsync(Group pGroup, User pUser) {
      try {
        return await GetInsertGroupCommand(pGroup, pUser).ExecuteNonQueryAsync();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.InsertGroup", lException.Message));
        return -1;
      }
    }


    private SQLiteCommand GetInsertWordCommand(Word pWord, User pUser) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("INSERT INTO ")
        .Append(wordsTab)
        .Append(" VALUES ( ")
        .Append("@").Append(wordId).Append(", ")
        .Append("@").Append(userId).Append(", ")
        .Append("@").Append(groupId).Append(", ")
        .Append("@").Append(language1).Append(", ")
        .Append("@").Append(language2).Append(", ")
        .Append("@").Append(drawer).Append(", ")
        .Append("@").Append(language1Comment).Append(", ")
        .Append("@").Append(language2Comment).Append(", ")
        .Append("@").Append(visible).Append(", ")
        .Append("@").Append(state)
        .Append(" ) ");
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      lCommand.Parameters.Add(new SQLiteParameter(wordId, pWord.Id));
      lCommand.Parameters.Add(new SQLiteParameter(userId, pUser.UserId));
      lCommand.Parameters.Add(new SQLiteParameter(groupId, pWord.GroupId));
      lCommand.Parameters.Add(new SQLiteParameter(language1, pWord.Language1));
      lCommand.Parameters.Add(new SQLiteParameter(language2, pWord.Language2));
      lCommand.Parameters.Add(new SQLiteParameter(drawer, pWord.Drawer));
      lCommand.Parameters.Add(new SQLiteParameter(language1Comment, pWord.Language1Comment));
      lCommand.Parameters.Add(new SQLiteParameter(language2Comment, pWord.Language2Comment));
      lCommand.Parameters.Add(new SQLiteParameter(visible, pWord.Visible));
      lCommand.Parameters.Add(new SQLiteParameter(state, pWord.State));
      return lCommand;
    }

    public async Task<int> InsertWordAsync(Word pWord, User pUser) {
      try {
        return await GetInsertWordCommand(pWord, pUser).ExecuteNonQueryAsync();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.InsertWord", lException.Message));
        return -1;
      }
    }

    public int InsertWord(Word pWord, User pUser) {
      try {
        return GetInsertWordCommand(pWord, pUser).ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.InsertWord", lException.Message));
        return -1;
      }
    }

    private SQLiteCommand GetInsertResultCommand(Result pResult, User pUser) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("INSERT INTO ")
        .Append(resultsTab)
        .Append(" VALUES ( ")
        .Append("@").Append(resultId).Append(", ")
        .Append("@").Append(userId).Append(", ")
        .Append("@").Append(groupId).Append(", ")
        .Append("@").Append(correct).Append(", ")
        .Append("@").Append(accepted).Append(", ")
        .Append("@").Append(wrong).Append(", ")
        .Append("@").Append(invisibilities).Append(", ")
        .Append("@").Append(timeCount).Append(", ")
        .Append("@").Append(direction).Append(", ")
        .Append("@").Append(lessonType).Append(", ")
        .Append("@").Append(datetime).Append(", ")
        .Append("@").Append(state)
        .Append(" ) ");
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      lCommand.Parameters.Add(new SQLiteParameter(resultId, pResult.Id));
      lCommand.Parameters.Add(new SQLiteParameter(userId, pUser.UserId));
      lCommand.Parameters.Add(new SQLiteParameter(groupId, pResult.GroupId));
      lCommand.Parameters.Add(new SQLiteParameter(correct, pResult.Correct));
      lCommand.Parameters.Add(new SQLiteParameter(accepted, pResult.Accepted));
      lCommand.Parameters.Add(new SQLiteParameter(wrong, pResult.Wrong));
      lCommand.Parameters.Add(new SQLiteParameter(invisibilities, pResult.Invisibilities));
      lCommand.Parameters.Add(new SQLiteParameter(timeCount, pResult.TimeCount));
      lCommand.Parameters.Add(new SQLiteParameter(direction, (int)pResult.TranslationDirection));
      lCommand.Parameters.Add(new SQLiteParameter(lessonType, (int)pResult.LessonType));
      lCommand.Parameters.Add(new SQLiteParameter(datetime, pResult.DateTime));
      lCommand.Parameters.Add(new SQLiteParameter(state, pResult.State));
      return lCommand;
    }

    public int InsertResult(Result pResult, User pUser) {
      try {
        return GetInsertResultCommand(pResult, pUser).ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.InsertResult", lException.Message));
        return -1;
      }
    }

    public async Task<int> InsertResultAsync(Result pResult, User pUser) {
      try {
        return await GetInsertResultCommand(pResult, pUser).ExecuteNonQueryAsync();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.InsertResult", lException.Message));
        return -1;
      }
    }

    private SQLiteCommand GetInsertUserCommand(User pUser) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("INSERT INTO ")
        .Append(usersTab)
        .Append(" VALUES ( ")
        .Append("@").Append(userId).Append(", ")
        .Append("@").Append(login).Append(", ")
        .Append("@").Append(password).Append(", ")
        .Append("@").Append(downloadDate).Append(", ")
        .Append("@").Append(translationDireciton).Append(", ")
        .Append("@").Append(allWords).Append(", ")
        .Append("@").Append(timeOut).Append(", ")
        .Append("@").Append(apiKey).Append(", ")
        .Append("@").Append(isRegister)
        .Append(" ) ");
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      lCommand.Parameters.Add(new SQLiteParameter(userId, pUser.UserId));
      lCommand.Parameters.Add(new SQLiteParameter(login, pUser.Name));
      lCommand.Parameters.Add(new SQLiteParameter(password, pUser.Password));
      lCommand.Parameters.Add(new SQLiteParameter(downloadDate, pUser.DownloadTime));
      lCommand.Parameters.Add(new SQLiteParameter(translationDireciton, pUser.TranslationDirection));
      lCommand.Parameters.Add(new SQLiteParameter(allWords, pUser.AllWords));
      lCommand.Parameters.Add(new SQLiteParameter(timeOut, pUser.Timeout));
      lCommand.Parameters.Add(new SQLiteParameter(apiKey, pUser.ApiKey));
      lCommand.Parameters.Add(new SQLiteParameter(isRegister, pUser.IsRegister));
      return lCommand;
    }


    /// <summary>
    /// 1 jezeli udalo sie, -1 jezeli sie nie udalo
    /// </summary>
    /// <param name="pUser"></param>
    /// <returns></returns>
    public int InsertUser(User pUser) {
      try {
        return GetInsertUserCommand(pUser).ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.InsertUser", lException.Message));
        return -1;
      }
    }

    public async Task<int> InsertUserAsync(User pUser) {
      try {
        return await GetInsertUserCommand(pUser).ExecuteNonQueryAsync();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.InsertUser", lException.Message));
        return -1;
      }
    }

    private SQLiteCommand GetDeleteGroupCommand(long pGroupId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(groupsTab)
        .Append(" WHERE  ")
        .Append(groupId)
        .Append(" = ")
        .Append(pGroupId);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      return lCommand;
    }

    /// <summary>
    /// ile udalo sie usunac
    /// </summary>
    /// <param name="pGroupId"></param>
    /// <returns></returns>
    public int DeleteGroup(long pGroupId) {
      try {
        return GetDeleteGroupCommand(pGroupId).ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.DeleteGroup", lException.Message));
        return -1;
      }
    }

    public async Task<int> DeleteGroupAsync(long pGroupId) {
      try {
        return await GetDeleteGroupCommand(pGroupId).ExecuteNonQueryAsync();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.DeleteGroup", lException.Message));
        return -1;
      }
    }



    public async Task<int> DeleteWordByGroupId(long pGroupId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(wordsTab)
        .Append(" WHERE  ")
        .Append(groupId)
        .Append(" = ")
        .Append(pGroupId);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      try {
        return await lCommand.ExecuteNonQueryAsync();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.DeleteWordByGroupId", lException.Message));
        return -1;
      }
    }

    public async Task<int> DeleteWordAsync(long pWordId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(wordsTab)
        .Append(" WHERE  ")
        .Append(wordId)
        .Append(" = ")
        .Append(pWordId);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      try {
        return await lCommand.ExecuteNonQueryAsync();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.DeleteWord", lException.Message));
        return -1;
      }
    }

    public int DeleteResultByGroupId(long pGroupId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(resultsTab)
        .Append(" WHERE  ")
        .Append(groupId)
        .Append(" = ")
        .Append(pGroupId);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      try {
        return lCommand.ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.DeleteResultByGroupId", lException.Message));
        return -1;
      }
    }

    public int DeleteResult(long pResultId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(resultsTab)
        .Append(" WHERE  ")
        .Append(resultId)
        .Append(" = ")
        .Append(pResultId);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      try {
        return lCommand.ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.DeleteResult", lException.Message));
        return -1;
      }
    }

    public int DeleteUser(long pUserId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(usersTab)
        .Append(" WHERE  ")
        .Append(userId)
        .Append(" = ")
        .Append(pUserId);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      try {
        return lCommand.ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.DeleteUser", lException.Message));
        return -1;
      }
    }

    private SQLiteCommand GetUpdateGroupCommand(Group pGroup) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("UPDATE ")
        .Append(groupsTab)
        .Append(" SET ")
        .Append(groupName).Append(" = ").Append("@").Append(groupName).Append(", ")
        .Append(language1Type).Append(" = ").Append("@").Append(language1Type).Append(", ")
        .Append(language2Type).Append(" = ").Append("@").Append(language2Type).Append(", ")
        .Append(state).Append(" = ").Append("@").Append(state)
        .Append(" WHERE ")
        .Append(groupId).Append(" = ").Append("@").Append(groupId);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      lCommand.Parameters.Add(new SQLiteParameter(groupId, pGroup.Id));
      lCommand.Parameters.Add(new SQLiteParameter(groupName, pGroup.Name));
      lCommand.Parameters.Add(new SQLiteParameter(language1Type, (int)pGroup.Language1.Type));
      lCommand.Parameters.Add(new SQLiteParameter(language2Type, (int)pGroup.Language2.Type));
      lCommand.Parameters.Add(new SQLiteParameter(state, pGroup.State));
      return lCommand;
    }

    public int UpdateGroup(Group pGroup) {
      try {
        return GetUpdateGroupCommand(pGroup).ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.UpdateGroup", lException.Message));
        return -1;
      }
    }

    public async Task<int> UpdateGroupAsync(Group pGroup) {
      try {
        return await GetUpdateGroupCommand(pGroup).ExecuteNonQueryAsync();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.UpdateGroup", lException.Message));
        return -1;
      }
    }

    private SQLiteCommand GetUpdateWordCommand(Word pWord) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("UPDATE ")
        .Append(wordsTab)
        .Append(" SET ")
        .Append(groupId).Append(" = ").Append("@").Append(groupId).Append(", ")
        .Append(language1).Append(" = ").Append("@").Append(language1).Append(", ")
        .Append(language2).Append(" = ").Append("@").Append(language2).Append(", ")
        .Append(drawer).Append(" = ").Append("@").Append(drawer).Append(", ")
        .Append(language1Comment).Append(" = ").Append("@").Append(language1Comment).Append(", ")
        .Append(language2Comment).Append(" = ").Append("@").Append(language2Comment).Append(", ")
        .Append(visible).Append(" = ").Append("@").Append(visible).Append(", ")
        .Append(state).Append(" = ").Append("@").Append(state)
        .Append(" WHERE ")
        .Append(wordId).Append(" = ").Append("@").Append(wordId);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      lCommand.Parameters.Add(new SQLiteParameter(wordId, pWord.Id));
      lCommand.Parameters.Add(new SQLiteParameter(groupId, pWord.GroupId));
      lCommand.Parameters.Add(new SQLiteParameter(language1, pWord.Language1));
      lCommand.Parameters.Add(new SQLiteParameter(language2, pWord.Language2));
      lCommand.Parameters.Add(new SQLiteParameter(drawer, pWord.Drawer));
      lCommand.Parameters.Add(new SQLiteParameter(language1Comment, pWord.Language1Comment));
      lCommand.Parameters.Add(new SQLiteParameter(language2Comment, pWord.Language2Comment));
      lCommand.Parameters.Add(new SQLiteParameter(visible, pWord.Visible));
      lCommand.Parameters.Add(new SQLiteParameter(state, pWord.State));
      return lCommand;
    }

    public async Task<int> UpdateWordAsync(Word pWord) {
      try {
        return await GetUpdateWordCommand(pWord).ExecuteNonQueryAsync();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.UpdateWord", lException.Message));
        return -1;
      }
    }

    public int UpdateWord(Word pWord) {
      try {
        return GetUpdateWordCommand(pWord).ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.UpdateWord", lException.Message));
        return -1;
      }
    }

    private SQLiteCommand GetUpdateResultCommand(Result pResult) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("UPDATE ")
        .Append(resultsTab)
        .Append(" SET ")
        .Append(state).Append(" = ").Append("@").Append(state)
        .Append(" WHERE ")
        .Append(resultId).Append(" = ").Append("@").Append(resultId);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      lCommand.Parameters.Add(new SQLiteParameter(resultId, pResult.Id));
      lCommand.Parameters.Add(new SQLiteParameter(state, pResult.State));
      return lCommand;
    }

    public int UpdateResult(Result pResult) {
      try {
        return GetUpdateResultCommand(pResult).ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.UpdateWord", lException.Message));
        return -1;
      }
    }

    public async Task<int> UpdateResultAsync(Result pResult) {
      try {
        return await GetUpdateResultCommand(pResult).ExecuteNonQueryAsync();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.UpdateWord", lException.Message));
        return -1;
      }
    }

    public int UpdateUser(User pUser) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("UPDATE ")
        .Append(usersTab)
        .Append(" SET ")
        .Append(login).Append(" = ").Append("@").Append(login).Append(", ")
        .Append(password).Append(" = ").Append("@").Append(password).Append(", ")
        .Append(downloadDate).Append(" = ").Append("@").Append(downloadDate)
        .Append(" WHERE ")
        .Append(userId).Append(" = ").Append("@").Append(userId);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      lCommand.Parameters.Add(new SQLiteParameter(login, pUser.Name));
      lCommand.Parameters.Add(new SQLiteParameter(password, pUser.Password));
      lCommand.Parameters.Add(new SQLiteParameter(downloadDate, pUser.DownloadTime));
      lCommand.Parameters.Add(new SQLiteParameter(userId, pUser.UserId));
      try {
        return lCommand.ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.UpdateUser", lException.Message));
        return -1;
      }
    }

    public void RefreshDatabase(User pUser) {
      string lPreQuery = "UPDATE {0} SET {1} = {2} WHERE {3} > {4} AND {5} = {6}";
      SQLiteCommand lRefreshResultsCommand = new SQLiteCommand(string.Format(lPreQuery, resultsTab, state, 0, state, 0, userId, pUser.UserId), _connection);
      lRefreshResultsCommand.ExecuteNonQuery();
      SQLiteCommand lRefreshGroupsCommand = new SQLiteCommand(string.Format(lPreQuery, groupsTab, state, 0, state, 0, userId, pUser.UserId), _connection);
      lRefreshGroupsCommand.ExecuteNonQuery();
      SQLiteCommand lRefreshWordsCommand = new SQLiteCommand(string.Format(lPreQuery, wordsTab, state, 0, state, 0, userId, pUser.UserId), _connection);
      lRefreshWordsCommand.ExecuteNonQuery();

      lPreQuery = "DELETE FROM {0} WHERE {1} < {2} AND {3} = {4}";
      SQLiteCommand lDeleteResultsCommand = new SQLiteCommand(string.Format(lPreQuery, resultsTab, state, 0, userId, pUser.UserId), _connection);
      lDeleteResultsCommand.ExecuteNonQuery();
      SQLiteCommand lDeleteGroupsCommand = new SQLiteCommand(string.Format(lPreQuery, groupsTab, state, 0, userId, pUser.UserId), _connection);
      lDeleteGroupsCommand.ExecuteNonQuery();
      SQLiteCommand lDeleteWordsCommand = new SQLiteCommand(string.Format(lPreQuery, wordsTab, state, 0, userId, pUser.UserId), _connection);
      lDeleteWordsCommand.ExecuteNonQuery();
    }

    private void LogException(string pMessage) {
      Logger.LogError(pMessage);
    }

    public void PrintUser() {
      Logger.LogInfo("{0}", "Wypisanie User");
      Logger.LogInfo("{0} - {1} - {2}", "UserId", "UserName", "Password");
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ").Append(usersTab);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      using (SQLiteDataReader lReader = lCommand.ExecuteReader()) {
        while (lReader.Read()) {
          Logger.LogInfo("{0} - {1} - {2}", lReader.GetInt64(0), lReader.GetString(1), lReader.GetString(2));
        }
      }
    }

    public void PrintGroups() {
      Logger.LogInfo("{0}", "Wypisanie Group");
      Logger.LogInfo("{0} - {1} - {2} - {3} - {4}", "GroupId", "GroupName", "Language1", "Language2", "State");
      List<long> lIdsList = SelectUserIdsList();
      foreach (long lItem in lIdsList) {
        List<Group> lGroupsList = SelectGroupList(lItem);
        foreach (var lGroup in lGroupsList) {
          Logger.LogInfo("{0} - {1} - {2} - {3} - {4}", lGroup.Id, lGroup.Name, lGroup.Language1, lGroup.Language2, lGroup.State);
        }
      }
    }

    public void PrintWord() {
      Logger.LogInfo("{0}", "Wypisanie Word");
      Logger.LogInfo("{0} - {1} - {2} - {3} - {4} - {5} - {6} - {7}", "GroupId", "Language1", "Language2", "Drawer", "Language1Comment", "Language2Comment", "Visibility", "State");
      List<long> lIdsList = SelectUserIdsList();
      foreach (long lItem in lIdsList) {
        List<Word> lWordsList = SelectWordListByUserId(lItem);
        foreach (var lWord in lWordsList) {
          Logger.LogInfo("{0} - {1} - {2} - {3} - {4} - {5} - {6} - {7}", lWord.GroupId, lWord.Language1, lWord.Language2, lWord.Drawer, lWord.Language1Comment, lWord.Language2Comment, lWord.Visible, lWord.State);
        }
      }
    }

    public void PrintResult() {
      Logger.LogInfo("{0}", "Wypisanie Result");
      Logger.LogInfo("{0} - {1} - {2} - {3} - {4} - {5} - {6} - {7} - {8} - {9}", "GroupId", "Correct", "Accepted", "Wrong", "Invisibilities", "Time", "Direction", "Type", "Date", "State");
      List<long> lIdsList = SelectUserIdsList();
      foreach (long lItem in lIdsList) {
        List<Result> lResultsList = SelectResultsListByUserId(lItem);
        foreach (var lResults in lResultsList) {
          Logger.LogInfo("{0} - {1} - {2} - {3} - {4} - {5} - {6} - {7} - {8} - {9}", lResults.GroupId, lResults.Correct, lResults.Accepted, lResults.Wrong, lResults.Invisibilities, lResults.TimeCount, lResults.TranslationDirection, lResults.LessonType, lResults.DateTime, lResults.State);
        }
      }
    }

    public void PrintDatabase() {
      PrintUser();
      PrintGroups();
      PrintWord();
      PrintResult();
    }

    public void ClearDatabase() {
      ClearWord();
      ClearResult();
      ClearGroup();
      ClearUsers();
    }

    public void ClearUsers() {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(usersTab);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      lCommand.ExecuteNonQuery();
    }

    public void ClearWord() {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(wordsTab);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      lCommand.ExecuteNonQuery();
    }

    public void ClearResult() {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(resultsTab);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      lCommand.ExecuteNonQuery();
    }

    public void ClearGroup() {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(groupsTab);
      SQLiteCommand lCommand = new SQLiteCommand(lBuilder.ToString(), _connection);
      lCommand.ExecuteNonQuery();
    }
  }
}
