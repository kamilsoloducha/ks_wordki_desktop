using System.Data.SqlClient;
using Wordki.Helpers;

namespace Wordki.LocalDatabase {
  public class SqlDatabaseCreator {
  
	public bool createDatabase(string pQuery, SqlConnection pConnection) {
      Logger.LogInfo("Tworzenie bazy danych");
      SqlCommand command = new SqlCommand(string.Format(pQuery, Wordki.Helpers.Util.GetExeFilePath()), pConnection);
      try {
        command.ExecuteNonQuery();
      } catch (SqlException lException) {
        Logger.LogInfo("Błąd tworzenia bazy danych - {0}", lException);
        return false;
      }
      return true;
    }

    public bool? IsDatabaseExist(string pDatabase, SqlConnection pConnection) {
      SqlCommand lCheckDbExict = new SqlCommand(string.Format("select count(*) from sys.databases where name='{0}' ", pDatabase), pConnection);
      try {
        return (lCheckDbExict.ExecuteScalar() as int?) == 1;
      } catch (SqlException e) {
        Logger.LogError("Exception - {0}", e.Message);
        return null;
      }
    }

    public bool dropDatabase(string pDatabase, SqlConnection pConnection) {
      Logger.LogInfo("Usuwanie bazy danych");
      SqlCommand lCheckDbExict = new SqlCommand(string.Format("DROP DATABASE {0}", pDatabase), pConnection);
      try {
        lCheckDbExict.ExecuteNonQuery();
      } catch (SqlException e) {
        Logger.LogError("Exception - {0}", e.Message);
        return false;
      }
      return true;
    }

    public void createTable(string pQuery, SqlConnection pConnection) {
      Logger.LogInfo("Tworzenie tabeli");
      SqlCommand command = new SqlCommand(pQuery, pConnection);
      try {
        command.ExecuteNonQuery();
      } catch (SqlException lException) {
        Logger.LogInfo("Błąd tworzenia bazy danych - {0}", lException);
      }
    }
  }
}