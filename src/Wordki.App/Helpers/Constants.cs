using System.Configuration;

namespace Wordki.Helpers {
  public class Constants {
    public static string php_domen = ConfigurationManager.AppSettings["host"];
    public static string php_login = php_domen + "php_login_user.php";
    public static string php_register = php_domen + "php_register_user.php";
    public static string php_write_results = php_domen + "php_write_results.php";
    public static string php_write_words = php_domen + "php_write_words.php";
    public static string php_write_groups = php_domen + "php_write_groups.php";
    public static string php_read_results = php_domen + "php_read_results.php";
    public static string php_read_words = php_domen + "php_read_words.php";
    public static string php_read_groups = php_domen + "php_read_groups.php";
    public static string php_get_common_groups_name = php_domen + "php_get_groups_name.php";
    public static string php_get_common_group = php_domen + "php_get_new_group.php";
    public static string php_get_info = php_domen + "php_get_info.php";
    public static string php_write_update = php_domen + "php_write_update.php";
    //wordki.vipserv.org
    public static string php_test = "http://localhost:8080/Oazachaosu" + "/api/v1/groups";
    //==================================================================================
    public static string root_tag = "Root";
    public static string word_tag = "wd";
    public static string group_tag = "gr";
    public static string result_tag = "rs";
    public static string wordid_tag = "id";
    public static string native_tag = "na";
    public static string foreign_tag = "fo";
    //==================================================================================
    public static string groupid_tag = "gi";
    public static string comment_native_tag = "cn";
    public static string comment_foreign_tag = "cf";
    //==================================================================================
    public static string drawer_tag = "dr";
    public static string visible_tag = "vi";
    public static string resultid_tag = "ri";
    public static string groupname_tag = "gn";
    public static string date_tag = "dt";
    public static string correct_tag = "cr";
    public static string accepted_tag = "ac";
    public static string wrong_tag = "wr";
    public static string time_tag = "tm";
    public static string translation_direction_tag = "td";
    public static string lesson_type_tag = "lt";
    public static string unvisibilities_tag = "uv";
    //==================================================================================
    public static string beginMessage = "<#";
    public static string endMessage = "#>";
    public static string separator = "#";
    //==================================================================================
    public static string GroupsFileName = "Groups.dat";
    public static string WordsFileName = "Words.dat";
    public static string ResultsFileName = "Results.dat";
    public static string DataDictionary = "Data";
    //==================================================================================
    public static string DataTimeFormat = "yyyy-MM-dd HH:mm:ss";

    public static string[] ColorArray = new string[]
    {
      "#FFFF0000",
      "#FFFF8000",
      "#FFFFFF00",
      "#FF80FF00",
      "#FF00FF00"
    };
  }
}
