using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Wordki.Helpers {
  public enum Level {
    Info,
    Alert,
    Error,
  }

  public class Logger {
    private static string _logDirName = Path.Combine(Directory.GetCurrentDirectory(), "Log");
    private static string _logFileName = "_log.log";
    private static Logger _instance;

    private Logger() {
      if (!Directory.Exists(_logDirName)) {
        Directory.CreateDirectory(_logDirName);
      }
    }

    private static Logger Instance {
      get {
        return _instance ?? (_instance = new Logger());
      }
    }

    private void LogData(Level pLevel, string pFileName, int pLineNumber, string pFormat, object[] pArgs) {
      StringBuilder lStringBuilder = new StringBuilder();
      lStringBuilder.Append(DateTime.Now.ToString("HH:mm:ss"));
      lStringBuilder.Append(" - ");
      lStringBuilder.Append(pLevel);
      lStringBuilder.Append(" - ");

      if(pArgs!=null)
        lStringBuilder.Append(String.Format(pFormat, pArgs));
      else
        lStringBuilder.Append(pFormat);

      lStringBuilder.Append(" - ");
      lStringBuilder.Append(pFileName);
      lStringBuilder.Append(" (");
      lStringBuilder.Append(pLineNumber);
      lStringBuilder.Append(")");
      lStringBuilder.AppendLine();
      string lPath = Path.Combine(_logDirName, String.Format("{0}{1}", DateTime.Now.ToString("yyyy-MM-dd"), _logFileName));
      Console.Write(lStringBuilder.ToString());
      using (StreamWriter lStreamWriter = new StreamWriter(lPath, true)) {
        lStreamWriter.Write(lStringBuilder.ToString());
      }
    }

    public static void LogInfo(string pFormat, params object[] pArgs) {
      StackTrace lStackTrace = new StackTrace(1, true);
      StackFrame lStackFrame = lStackTrace.GetFrame(0);
      Instance.LogData(Level.Info, Path.GetFileName(lStackFrame.GetFileName()), lStackFrame.GetFileLineNumber(), pFormat, pArgs);
    }

    public static void LogError(string pFormat, params object[] pArgs) {
      StackTrace lStackTrace = new StackTrace(1, true);
      StackFrame lStackFrame = lStackTrace.GetFrame(0);
      Instance.LogData(Level.Error, Path.GetFileName(lStackFrame.GetFileName()), lStackFrame.GetFileLineNumber(), pFormat, pArgs);
    }

    public static void LogAlert(string pFormat, params object[] pArgs) {
      StackTrace lStackTrace = new StackTrace(1, true);
      StackFrame lStackFrame = lStackTrace.GetFrame(0);
      Instance.LogData(Level.Alert, Path.GetFileName(lStackFrame.GetFileName()), lStackFrame.GetFileLineNumber(), pFormat, pArgs);
    }

  }
}
