using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms.VisualStyles;
using Wordki.Models;

namespace Wordki.Helpers {
  public class ConnectorDatabase {
    //tabele
    private const string usersTab = "T_USERS";
    private const string groupsTab = "T_GROUPS";
    private const string wordsTab = "T_WORDS";
    private const string resultsTab = "T_RESULTS";

    //pola users_tab
    private const string userId = "C_USER_ID";
    private const string login = "C_LOGIN";
    private const string password = "C_PASSWORD";
    private const string downloadDate = "C_DOWNLOAD_DATE";
    private const string translationDireciton = "C_TRANSLATION_DIRECTION";
    private const string allWords = "C_ALL_WORDS";
    private const string timeOut = "C_TIMEOUT";

    //pola groups_tab
    private const string groupId = "C_GROUP_ID";
    private const string groupName = "C_GROUP_NAME";
    private const string language1Type = "C_LANGUAGE_1_TYPE";
    private const string language2Type = "C_LANGUAGE_2_TYPE";
    private const string state = "C_STATE";

    //pola words_tab
    private const string wordId = "C_WORD_ID";
    private const string language1 = "C_LANGUAGE_1";
    private const string language2 = "C_LANGUAGE_2";
    private const string drawer = "C_DRAWER";
    private const string language1Comment = "C_LANGUAGE_1_COMMENT";
    private const string language2Comment = "C_LANGUAGE_2_COMMENT";
    private const string visibility = "C_VISIBILITY";

    //pola results_tab
    private const string resultId = "C_RESULT_ID";
    private const string datetime = "C_DATE_TIME";
    private const string correct = "C_CORRECT";
    private const string accepted = "C_ACCEPTED";
    private const string wrong = "C_WRONG";
    private const string invisibilities = "C_INVISIBILITIES";
    private const string timeCount = "C_TIME_COUNT";
    private const string direction = "C_DIRECTION";
    private const string lessonType = "C_LESSON_TYPE";


    private SqlConnection SqlConnection { get; set; }
    public bool Connected { get; private set; }
    public ConnectorDatabase() {

    }

    public void Disconnect() {
      SqlConnection.Close();
    }

    public bool Connect() {
      try {
        SqlConnection = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFileName=|DataDirectory|\WordkiDatabase.mdf;integrated security=true;database=WordkiDatabase;Integrated Security=True;");
        SqlConnection.Open();
      } catch (SqlException lException) {
        Logger.LogError("{0} - {1}", "ConnectorDatabase.Connect", lException.Message);
        Connected = false;
        return false;
      }
      Connected = true;
      return true;
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
        .Append((int)ElementState.Done);
      return ExecuteGroupsSelect(lBuilder.ToString());
    }

    public List<Group> SelectGroupList(long pUserId, bool pIsConstraint = false) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(groupsTab)
        .Append(" WHERE ")
        .Append(userId)
        .Append(" = ")
        .Append(pUserId);
      if (pIsConstraint) {
        lBuilder.Append(" AND ")
        .Append(state)
        .Append(" != ")
        .Append((int)ElementState.Delete);
      }
      return ExecuteGroupsSelect(lBuilder.ToString());
    }

    private List<Group> ExecuteGroupsSelect(string pQuery) {
      SqlCommand lCommand = new SqlCommand(pQuery, SqlConnection);
      List<Group> lGroupList = new List<Group>();
      try {
        using (SqlDataReader lReader = lCommand.ExecuteReader()) {
          while (lReader.Read()) {
            Group lGroup = new Group();
            lGroup.GroupId = lReader.GetInt64(0);
            lGroup.GroupName = lReader.GetString(2);
            lGroup.Language1 = (Languages)lReader.GetInt32(3);
            lGroup.Language2 = (Languages)lReader.GetInt32(4);
            lGroup.State = (ElementState)lReader.GetInt32(5);
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
        .Append(userId)
        .Append(" AND ")
        .Append(state)
        .Append(" != ")
        .Append((int)ElementState.Done);
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      return ExecuteWordsSelect(lCommand);
    }

    public List<Word> SelectWordListByUserId(long pUserId, bool pIsConstraint = false) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(wordsTab)
        .Append(" WHERE ")
        .Append(userId)
        .Append(" = ")
        .Append(userId);
      if (pIsConstraint) {
        lBuilder.Append(" AND ")
        .Append(state)
        .Append(" != ")
        .Append((int)ElementState.Delete);
      }
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      return ExecuteWordsSelect(lCommand);
    }

    public List<Word> SelectWordListByGroupId(long pGroupId, bool pIsConstraint = false) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(wordsTab)
        .Append(" WHERE ")
        .Append(groupId)
        .Append(" = ")
        .Append(pGroupId);
      if (pIsConstraint) {
        lBuilder.Append(" AND ")
        .Append(state)
        .Append(" != ")
        .Append((int)ElementState.Delete);
      }
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      return ExecuteWordsSelect(lCommand);
    }

    private List<Word> ExecuteWordsSelect(SqlCommand pCommand) {
      List<Word> lWordList = new List<Word>();
      try {
        using (SqlDataReader lReader = pCommand.ExecuteReader()) {
          while (lReader.Read()) {
            Word lWord = new Word();
            lWord.WordId = lReader.GetInt64(0);
            lWord.GroupId = lReader.GetInt64(2);
            lWord.Language1 = lReader.GetString(3);
            lWord.Language2 = lReader.GetString(4);
            lWord.Drawer = lReader.GetInt32(5);
            lWord.Language1Comment = lReader.GetString(6);
            lWord.Language2Comment = lReader.GetString(7);
            lWord.Visibility = lReader.GetInt32(8) == 1;
            lWord.State = (ElementState)lReader.GetInt32(9);
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
        .Append((int)ElementState.Done);
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      return ExecuteResultSelect(lCommand);
    }

    public List<Result> SelectResultsListByUserId(long pUserId, bool pIsConstraint = false) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(resultsTab)
        .Append(" WHERE ")
        .Append(userId)
        .Append(" = ")
        .Append(pUserId);
      if (pIsConstraint) {
        lBuilder.Append(" AND ")
        .Append(state)
        .Append(" != ")
        .Append((int)ElementState.Delete);
      }
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      return ExecuteResultSelect(lCommand);
    }


    public List<Result> SelectResultsListByGroupId(long pGroupId, bool pIsConstraint = false) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ")
        .Append(resultsTab)
        .Append(" WHERE ")
        .Append(groupId)
        .Append(" = ")
        .Append(pGroupId);
      if (pIsConstraint) {
        lBuilder.Append(" AND ")
        .Append(state)
        .Append(" != ")
        .Append((int)ElementState.Delete);
      }
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      return ExecuteResultSelect(lCommand);
    }

    private List<Result> ExecuteResultSelect(SqlCommand pCommand) {
      List<Result> lResultList = new List<Result>();
      try {
        using (SqlDataReader lReader = pCommand.ExecuteReader()) {
          while (lReader.Read()) {
            lResultList.Add(new Result(
              lReader.GetInt64(0),
              lReader.GetInt64(2),
              lReader.GetInt32(3),
              lReader.GetInt32(4),
              lReader.GetInt32(5),
              lReader.GetInt32(6),
              lReader.GetInt32(7),
              (TranslationDirection)lReader.GetInt32(8),
              (LessonType)lReader.GetInt32(9),
              lReader.GetDateTime(10),
              (ElementState)lReader.GetInt32(11)
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
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      List<long> lIdsList = new List<long>();
      using (SqlDataReader lReader = lCommand.ExecuteReader()) {
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
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
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
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      return ExecuteSelectUser(lCommand);
    }

    public User ExecuteSelectUser(SqlCommand pCommand) {
      User lUser = null;
      using (SqlDataReader lReader = pCommand.ExecuteReader()) {
        while (lReader.Read()) {
          lUser = new User();
          int index = 0;
          lUser.UserId = lReader.GetInt64(index++);
          lUser.UserName = lReader.GetString(index++);
          lUser.Password = lReader.GetString(index++);
          lUser.DownloadTime = lReader.GetDateTime(index++);
        }
      }
      return lUser;
    }

    public int InsertGroup(Group pGroup, User pUser) {
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
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      lCommand.Parameters.Add(new SqlParameter(groupId, pGroup.GroupId));
      lCommand.Parameters.Add(new SqlParameter(userId, pUser.UserId));
      lCommand.Parameters.Add(new SqlParameter(groupName, pGroup.GroupName));
      lCommand.Parameters.Add(new SqlParameter(language1Type, (int)pGroup.Language1));
      lCommand.Parameters.Add(new SqlParameter(language2Type, (int)pGroup.Language2));
      lCommand.Parameters.Add(new SqlParameter(state, pGroup.State));
      try {
        return lCommand.ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.InsertGroup", lException.Message));
        return -1;
      }
    }

    public int InsertWord(Word pWord, User pUser) {
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
        .Append("@").Append(visibility).Append(", ")
        .Append("@").Append(state)
        .Append(" ) ");
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      lCommand.Parameters.Add(new SqlParameter(wordId, pWord.WordId));
      lCommand.Parameters.Add(new SqlParameter(userId, pUser.UserId));
      lCommand.Parameters.Add(new SqlParameter(groupId, pWord.GroupId));
      lCommand.Parameters.Add(new SqlParameter(language1, pWord.Language1));
      lCommand.Parameters.Add(new SqlParameter(language2, pWord.Language2));
      lCommand.Parameters.Add(new SqlParameter(drawer, pWord.Drawer));
      lCommand.Parameters.Add(new SqlParameter(language1Comment, pWord.Language1Comment));
      lCommand.Parameters.Add(new SqlParameter(language2Comment, pWord.Language2Comment));
      lCommand.Parameters.Add(new SqlParameter(visibility, pWord.Visibility));
      lCommand.Parameters.Add(new SqlParameter(state, pWord.State));
      try {
        return lCommand.ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.InsertWord", lException.Message));
        return -1;
      }
    }

    public int InsertResult(Result pResult, User pUser) {
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
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      lCommand.Parameters.Add(new SqlParameter(resultId, pResult.ResultId));
      lCommand.Parameters.Add(new SqlParameter(userId, pUser.UserId));
      lCommand.Parameters.Add(new SqlParameter(groupId, pResult.GroupId));
      lCommand.Parameters.Add(new SqlParameter(correct, pResult.Correct));
      lCommand.Parameters.Add(new SqlParameter(accepted, pResult.Accepted));
      lCommand.Parameters.Add(new SqlParameter(wrong, pResult.Wrong));
      lCommand.Parameters.Add(new SqlParameter(invisibilities, pResult.Invisibilities));
      lCommand.Parameters.Add(new SqlParameter(timeCount, pResult.Time));
      lCommand.Parameters.Add(new SqlParameter(direction, (int)pResult.TranslationDirection));
      lCommand.Parameters.Add(new SqlParameter(lessonType, (int)pResult.LessonType));
      lCommand.Parameters.Add(new SqlParameter(datetime, pResult.Date));
      lCommand.Parameters.Add(new SqlParameter(state, pResult.State));
      try {
        return lCommand.ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.InsertResult", lException.Message));
        return -1;
      }
    }

    /// <summary>
    /// 1 jezeli udalo sie, -1 jezeli sie nie udalo
    /// </summary>
    /// <param name="pUser"></param>
    /// <returns></returns>
    public int InsertUser(User pUser) {
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
        .Append("@").Append(timeOut)
        .Append(" ) ");
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      lCommand.Parameters.Add(new SqlParameter(userId, pUser.UserId));
      lCommand.Parameters.Add(new SqlParameter(login, pUser.UserName));
      lCommand.Parameters.Add(new SqlParameter(password, pUser.Password));
      lCommand.Parameters.Add(new SqlParameter(downloadDate, pUser.DownloadTime));
      lCommand.Parameters.Add(new SqlParameter(translationDireciton, pUser.TranslationDirection));
      lCommand.Parameters.Add(new SqlParameter(allWords, pUser.AllWords));
      lCommand.Parameters.Add(new SqlParameter(timeOut, pUser.TimeOut));
      try {
        return lCommand.ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.InsertUser", lException.Message));
        return -1;
      }
    }

    /// <summary>
    /// ile udalo sie usunac
    /// </summary>
    /// <param name="pGroupId"></param>
    /// <returns></returns>
    public int DeleteGroup(long pGroupId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(groupsTab)
        .Append(" WHERE  ")
        .Append(groupId)
        .Append(" = ")
        .Append(pGroupId);
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      try {
        return lCommand.ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.DeleteGroup", lException.Message));
        return -1;
      }
    }

    public int DeleteWordByGroupId(long pGroupId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(wordsTab)
        .Append(" WHERE  ")
        .Append(groupId)
        .Append(" = ")
        .Append(pGroupId);
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      try {
        return lCommand.ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.DeleteWordByGroupId", lException.Message));
        return -1;
      }
    }

    public int DeleteWord(long pWordId) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(wordsTab)
        .Append(" WHERE  ")
        .Append(wordId)
        .Append(" = ")
        .Append(pWordId);
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      try {
        return lCommand.ExecuteNonQuery();
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
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
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
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
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
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      try {
        return lCommand.ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.DeleteUser", lException.Message));
        return -1;
      }
    }

    public int UpdateGroup(Group pGroup) {
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
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      lCommand.Parameters.Add(new SqlParameter(groupId, pGroup.GroupId));
      lCommand.Parameters.Add(new SqlParameter(groupName, pGroup.GroupName));
      lCommand.Parameters.Add(new SqlParameter(language1Type, (int)pGroup.Language1));
      lCommand.Parameters.Add(new SqlParameter(language2Type, (int)pGroup.Language2));
      lCommand.Parameters.Add(new SqlParameter(state, pGroup.State));
      try {
        return lCommand.ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.UpdateGroup", lException.Message));
        return -1;
      }
    }

    public int UpdateWord(Word pWord) {
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
        .Append(visibility).Append(" = ").Append("@").Append(visibility).Append(", ")
        .Append(state).Append(" = ").Append("@").Append(state)
        .Append(" WHERE ")
        .Append(wordId).Append(" = ").Append("@").Append(wordId);
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      lCommand.Parameters.Add(new SqlParameter(wordId, pWord.WordId));
      lCommand.Parameters.Add(new SqlParameter(groupId, pWord.GroupId));
      lCommand.Parameters.Add(new SqlParameter(language1, pWord.Language1));
      lCommand.Parameters.Add(new SqlParameter(language2, pWord.Language2));
      lCommand.Parameters.Add(new SqlParameter(drawer, pWord.Drawer));
      lCommand.Parameters.Add(new SqlParameter(language1Comment, pWord.Language1Comment));
      lCommand.Parameters.Add(new SqlParameter(language2Comment, pWord.Language2Comment));
      lCommand.Parameters.Add(new SqlParameter(visibility, pWord.Visibility));
      lCommand.Parameters.Add(new SqlParameter(state, pWord.State));
      try {
        return lCommand.ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.UpdateWord", lException.Message));
        return -1;
      }
    }

    public int UpdateResult(Result pResult) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("UPDATE ")
        .Append(resultsTab)
        .Append(" SET ")
        .Append(state).Append(" = ").Append("@").Append(state)
        .Append(" WHERE ")
        .Append(resultId).Append(" = ").Append("@").Append(resultId);
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      lCommand.Parameters.Add(new SqlParameter(resultId, pResult.ResultId));
      lCommand.Parameters.Add(new SqlParameter(state, pResult.State));
      try {
        return lCommand.ExecuteNonQuery();
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
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      lCommand.Parameters.Add(new SqlParameter(login, pUser.UserName));
      lCommand.Parameters.Add(new SqlParameter(password, pUser.Password));
      lCommand.Parameters.Add(new SqlParameter(downloadDate, pUser.DownloadTime));
      lCommand.Parameters.Add(new SqlParameter(userId, pUser.UserId));
      try {
        return lCommand.ExecuteNonQuery();
      } catch (Exception lException) {
        LogException(String.Format("{0} - {1}", "ConnectorDatabase.UpdateUser", lException.Message));
        return -1;
      }
    }

    public int InsertOrUpdateGroup(Group pGroup, User pUser) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT COUNT(*) FROM ")
        .Append(groupsTab)
        .Append(" WHERE ")
        .Append(groupId).Append(" = ").Append(pGroup.GroupId);
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      int lCount = (int)lCommand.ExecuteScalar();
      if (lCount > 0) {
        return UpdateGroup(pGroup);
      }
      return InsertGroup(pGroup, pUser);
    }

    public int InsertOrUpdateWord(Word pWord, User pUser) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT COUNT(*) FROM ")
        .Append(wordsTab)
        .Append(" WHERE ")
        .Append(wordId).Append(" = ").Append(pWord.WordId);
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      int lCount = (int)lCommand.ExecuteScalar();
      if (lCount > 0) {
        return UpdateWord(pWord);
      }
      return InsertWord(pWord, pUser);
    }

    /// <summary>
    /// 1 jezeli udalo sie -1 jezeli sie nie udalo
    /// </summary>
    /// <param name="pUser"></param>
    /// <returns></returns>
    public int InsertOrUpdateUser(User pUser) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT COUNT(*) FROM ")
        .Append(usersTab)
        .Append(" WHERE ")
        .Append(userId).Append(" = ").Append(pUser.UserId);
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      int lCount = (int)lCommand.ExecuteScalar();
      if (lCount > 0) {
        return UpdateUser(pUser);
      }
      return InsertUser(pUser);
    }

    public int InsertOrUpdateResult(Result pResult, User pUser) {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT COUNT(*) FROM ")
        .Append(resultsTab)
        .Append(" WHERE ")
        .Append(resultId).Append(" = ").Append(pResult.ResultId);
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      int lCount = (int)lCommand.ExecuteScalar();
      if (lCount > 0) {
        return UpdateResult(pResult);
      }
      return InsertResult(pResult, pUser);
    }

    public void RefreshDatabase(User pUser) {
      string lPreQuery = "UPDATE {0} SET {1} = {2} WHERE {3} != {4} AND {5} = {6}";
      SqlCommand lRefreshResultsCommand = new SqlCommand(string.Format(lPreQuery, resultsTab, state, (int)ElementState.Done, state, (int)ElementState.Delete, userId, pUser.UserId), SqlConnection);
      lRefreshResultsCommand.ExecuteNonQuery();
      SqlCommand lRefreshWordsCommand = new SqlCommand(string.Format(lPreQuery, wordsTab, state, (int)ElementState.Done, state, (int)ElementState.Delete, userId, pUser.UserId), SqlConnection);
      lRefreshWordsCommand.ExecuteNonQuery();
      SqlCommand lRefreshGroupsCommand = new SqlCommand(string.Format(lPreQuery, groupsTab, state, (int)ElementState.Done, state, (int)ElementState.Delete, userId, pUser.UserId), SqlConnection);
      lRefreshGroupsCommand.ExecuteNonQuery();

      lPreQuery = "DELETE FROM {0} WHERE {1} = {2} AND {3} = {4}";
      SqlCommand lDeleteResultsCommand = new SqlCommand(string.Format(lPreQuery, resultsTab, state, (int)ElementState.Delete, userId, pUser.UserId), SqlConnection);
      lDeleteResultsCommand.ExecuteNonQuery();
      SqlCommand lDeleteWordsCommand = new SqlCommand(string.Format(lPreQuery, wordsTab, state, (int)ElementState.Delete, userId, pUser.UserId), SqlConnection);
      lDeleteWordsCommand.ExecuteNonQuery();
      SqlCommand lDeleteGroupsCommand = new SqlCommand(string.Format(lPreQuery, groupsTab, state, (int)ElementState.Delete, userId, pUser.UserId), SqlConnection);
      lDeleteGroupsCommand.ExecuteNonQuery();
    }

    private void LogException(string pMessage) {
      Logger.LogError(pMessage);
    }

    public void PrintUser() {
      Logger.LogInfo("{0}", "Wypisanie User");
      Logger.LogInfo("{0} - {1} - {2}", "UserId", "UserName", "Password");
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("SELECT * FROM ").Append(usersTab);
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      using (SqlDataReader lReader = lCommand.ExecuteReader()) {
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
          Logger.LogInfo("{0} - {1} - {2} - {3} - {4}", lGroup.GroupId, lGroup.GroupName, lGroup.Language1, lGroup.Language2, lGroup.State);
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
          Logger.LogInfo("{0} - {1} - {2} - {3} - {4} - {5} - {6} - {7}", lWord.GroupId, lWord.Language1, lWord.Language2, lWord.Drawer, lWord.Language1Comment, lWord.Language2Comment, lWord.Visibility, lWord.State);
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
          Logger.LogInfo("{0} - {1} - {2} - {3} - {4} - {5} - {6} - {7} - {8} - {9}", lResults.GroupId, lResults.Correct, lResults.Accepted, lResults.Wrong, lResults.Invisibilities, lResults.Time, lResults.TranslationDirection, lResults.LessonType, lResults.Date, lResults.State);
        }
      }
    }

    public void PrintDatabase() {
      PrintUser();
      PrintGroups();
      PrintWord();
      PrintResult();
    }

    public void ConnectorTest() {
      const string tag = "SqlTag";
      User lUser = new User();

      Logger.LogInfo("{0} - {1}", tag, "insertUser");
      InsertUser(lUser);
      PrintUser();

      Logger.LogInfo("{0} - {1}", tag, "deleteUser");
      DeleteUser(lUser.UserId);
      PrintUser();

      Logger.LogInfo("{0} - {1}", tag, "insertUser");
      InsertUser(lUser);
      PrintUser();

      lUser.UserName = "TestLogin1";
      lUser.Password = "TestPassword1";

      Logger.LogInfo("{0} - {1}", tag, "updateUser");
      UpdateUser(lUser);
      PrintUser();

      Logger.LogInfo("");
      Group lGroup = new Group();

      Logger.LogInfo("{0} - {1}", tag, "insertGroup");
      InsertOrUpdateGroup(lGroup, lUser);
      PrintGroups();

      Logger.LogInfo("{0} - {1}", tag, "deleteGroup");
      DeleteGroup(lGroup.GroupId);
      PrintGroups();

      Logger.LogInfo("{0} - {1}", tag, "insertGroup");
      InsertOrUpdateGroup(lGroup, lUser);
      PrintGroups();

      lGroup.GroupName = "ółółółżźżźż";
      lGroup.Language1 = Languages.Portuaglese;
      lGroup.Language2 = Languages.French;
      lGroup.State = ElementState.Update;

      Logger.LogInfo("{0} - {1}", tag, "updateGroup");
      UpdateGroup(lGroup);
      PrintGroups();
      Logger.LogInfo("");
      Word lWord = new Word();

      Logger.LogInfo("{0} - {1}", tag, "insertWord");
      InsertWord(lWord, lUser);
      PrintWord();

      Logger.LogInfo("{0} - {1}", tag, "deleteWord");
      DeleteWord(lWord.WordId);
      PrintWord();

      Logger.LogInfo("{0} - {1}", tag, "insertWord");
      InsertWord(lWord, lUser);
      PrintWord();

      lWord.Language1 = "TestLanguage12";
      lWord.Language2 = "TestLanguage22";
      lWord.Drawer = 1;
      lWord.Language1Comment = "TestLanguage1Comment2";
      lWord.Language2Comment = "TestLanguage2Comment2";
      lWord.Visibility = false;
      lWord.State = ElementState.Update;

      Logger.LogInfo("{0} - {1}", tag, "updateWord");
      UpdateWord(lWord);
      PrintWord();
      Logger.LogInfo("");
      Result lResult = new Result(-1, lGroup.GroupId, 0, 0, 0, 0, 0, TranslationDirection.FromFirst, LessonType.Repeat, DateTime.Now, ElementState.Update);

      Logger.LogInfo("{0} - {1}", tag, "insertResult");
      InsertResult(lResult, lUser);
      PrintResult();

      Logger.LogInfo("{0} - {1}", tag, "deleteResult");
      DeleteResult(lResult.ResultId);
      PrintResult();

      Logger.LogInfo("{0} - {1}", tag, "insertResult");
      InsertResult(lResult, lUser);
      PrintResult();

      lResult.State = ElementState.Update;

      Logger.LogInfo("{0} - {1}", tag, "updateResult");
      UpdateResult(lResult);
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
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      lCommand.ExecuteNonQuery();
    }

    public void ClearWord() {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(wordsTab);
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      lCommand.ExecuteNonQuery();
    }

    public void ClearResult() {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(resultsTab);
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      lCommand.ExecuteNonQuery();
    }

    public void ClearGroup() {
      StringBuilder lBuilder = new StringBuilder();
      lBuilder.Append("DELETE FROM ")
        .Append(groupsTab);
      SqlCommand lCommand = new SqlCommand(lBuilder.ToString(), SqlConnection);
      lCommand.ExecuteNonQuery();
    }
  }
}