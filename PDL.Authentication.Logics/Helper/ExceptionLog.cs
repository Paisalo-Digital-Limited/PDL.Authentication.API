using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using PDL.Authentication.Entites.VM;

namespace PDL.Authentication.Logics.Helper
{
    public  class ExceptionLog
    {
        public  static  void InsertLogException(Exception exc, IConfiguration iConfig, string dbname, bool islive, string source = null)
        {
            string connection = string.Empty;
            if (!islive)
                connection = $"Data Source=192.168.10.2;Initial Catalog={dbname};User ID=sa;Password=Sasqlserver2022@10.2;Connection Timeout=120;Trusted_Connection=False;MultipleActiveResultSets=True;Encrypt=false";
            else
                connection = $"Data Source=192.168.10.2;Initial Catalog={dbname};User ID=sa;Password=Sasqlserver2022@10.2;Connection Timeout=120;Trusted_Connection=False;MultipleActiveResultSets=True;Encrypt=false";

            LosCodeExceptionLogVM losCodeExceptionLog = new LosCodeExceptionLogVM();
            if (exc.InnerException != null)
            {
                losCodeExceptionLog.InnerExeType = exc.InnerException.GetType().ToString().Replace("'", "_");
                losCodeExceptionLog.InnerExeMessage = exc.InnerException.Message.Replace("'", "_");
                losCodeExceptionLog.InnerExeSource = exc.InnerException.Source.Replace("'", "_");
                losCodeExceptionLog.InnerExeStackTrace = exc.InnerException.StackTrace.Replace("'", "_");
            }
            losCodeExceptionLog.ExeType = exc.GetType().ToString().Replace("'", "_");
            losCodeExceptionLog.ExeMessage = exc.Message.Replace("'", "_");
            losCodeExceptionLog.ExeStackTrace = exc.StackTrace != null ? exc.StackTrace.Replace("'", "_") : null;
            losCodeExceptionLog.ExeSource = source;

            string query = @"INSERT INTO LosCodeExceptionLogs (ExeSource,ExeType,ExeMessage,ExeStackTrace,InnerExeSource,InnerExeType,InnerExeMessage,InnerExeStackTrace,CreationDate)
                             VALUES  ('" + losCodeExceptionLog.ExeSource + "','" + losCodeExceptionLog.ExeType + "','" + losCodeExceptionLog.ExeMessage + "','" + losCodeExceptionLog.ExeStackTrace + "','" + losCodeExceptionLog.InnerExeSource + "','" + losCodeExceptionLog.InnerExeType + "','" + losCodeExceptionLog.InnerExeMessage + "','" + losCodeExceptionLog.InnerExeStackTrace + "',GETDATE())";
           
            using (var con = new SqlConnection(connection))
            {
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    if (con.State == ConnectionState.Closed) con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
}
