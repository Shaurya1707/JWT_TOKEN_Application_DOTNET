using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;


namespace CommanBusinessLogic
{
    [Obsolete]
    public class BL
    {

        #region variable for database connection

       
        OracleConnection con;
        OracleConnection con2;
        OracleDataAdapter da;
        DataSet ds = new DataSet();
        OracleDataReader dr;
        OracleDataReader dr2;
        OracleCommand cmd;
        OracleCommand cmd2;
        OracleTransaction trans;
        #endregion
        public static bool Email_Send(string from, string empidto, string cc, string bcc, string msg, string subject)
        {
            //errorMsg = "";
            bool sent = false;
            string body;
            MailMessage mailObj = new MailMessage();
            SmtpClient SMTPServer = new SmtpClient();
            SMTPServer.EnableSsl = false;
            SMTPServer.Credentials = CredentialCache.DefaultNetworkCredentials;
            SMTPServer.UseDefaultCredentials = false;
            mailObj.From = new MailAddress(from);
            //empidto = "gailinter@gail.co.in";
            if (!string.IsNullOrEmpty(empidto))
            {
                string[] multiTo = (empidto).Split(',');
                foreach (string emailID in multiTo)
                {
                    mailObj.To.Add(emailID);
                }
            }
            if (!string.IsNullOrEmpty(cc))
            {
                string[] multiCC = (cc).Split(',');
                foreach (string emailIDCC in multiCC)
                { mailObj.CC.Add(emailIDCC); }
            }
            if (!string.IsNullOrEmpty(bcc))
            {
                string[] multiBCC = (bcc).Split(',');
                foreach (string emailIDBCC in multiBCC)
                { mailObj.Bcc.Add(emailIDBCC); }
            }
            //mailObj.Bcc.Add(bcc);
            mailObj.Subject = subject;
            // mailObj.Bcc.Add("gailinter@gail.co.in");
            //try
            //{
            //    if (!string.IsNullOrEmpty(attachment))
            //        mailObj.Attachments.Add(new Attachment(attachment));
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

            body = " <html>" +
            " <head>" +
            " </head>" +
            " <body>" +
            " <table>" +
            " <tr><td>Dear Sir/Madam,<br /></td>" +
            " </tr>" +
            " <tr><td></td>" +
            " </tr>" +
            " <tr>" +
            " <td colspan='2'> " + msg + "</td>" +
            " </tr>" +
            " <tr><td> <B></B> </td>" +
            " </tr>" +
            " <tr height='50px'><td></td>" +
            " </tr>" +
            " <tr><td>Thanks & Regards,</td>" +
            " </tr>" +
            " <tr><td></td>" +
            " </tr>" +
            " <tr><td>Company secretary,</td>" +
            " </tr>" +

            " <tr><td>GAIL (India) Limited.</td>" +
            " </tr>" +
            " <tr><td colspan='2'>*********************************************************************************************************************</td></tr>" +
            " <tr>" +
            " <td align=left>This is system generated mail, Please don't reply.</td></tr> " +
            " <tr><td align=left>**********************************************************************************************************************</td></tr>" +
            " </table>" +
            " </body>" +
            " </html>";

            mailObj.Body = body;
            mailObj.IsBodyHtml = true;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                SMTPServer.Send(mailObj);
                sent = true;
            }
            catch (Exception)
            {
                //errorMsg = ex.Message;
                //while ((ex = ex.InnerException) != null)
                //{
                //    errorMsg += " | " + ex.Message;
                //}
                //System.Windows.Forms.MessageBox.Show(msgEx);
                sent = false;
            }
            mailObj.Dispose();
            return sent;
        }
        #region Function for Open Connection
        public void OpenConn()
        
        {
            con = new OracleConnection(ConfigurationManager.ConnectionStrings["GailCnstr"].ConnectionString);
            // con = new OleDbConnection("Provider=MSDAORA;Data Source=intradev;User Id=gailinter;Password=intranet;");//ConfigurationManager.ConnectionStrings["constrCathode"].ToString()
            con.Open();
        }
        #endregion

        #region Function for Open Connection
        public void OpenConn2()
        {
            con2 = new OracleConnection(ConfigurationManager.ConnectionStrings["GailCnstr"].ConnectionString);
            //  con2 = new OracleConnection("Provider=OracleClient;Data Source=intradev;User Id=gailinter;Password=intranet;");//ConfigurationManager.ConnectionStrings["constrCathode"].ToString()
            con2.Open();
        }
        #endregion

        public string GetBrowserName()
        {
            string browserName = ""; ;
            NameValueCollection coll = HttpContext.Current.Request.ServerVariables;
            string val = coll["HTTP_USER_AGENT"].ToString();
            if (val.Contains("MSIE"))
                browserName = "INTERNERT EXPLORER";
            else if (val.Contains("Chrome"))
                browserName = "GOOGLE CHROME";
            else if (val.Contains("Opera"))
                browserName = "OPERA";
            else if (val.Contains("Safari"))
                browserName = "SAFARI";
            else if (val.Contains("Mozilla"))
                browserName = "MOZILLA FIREFOX";
            return browserName;
        }

        public string GetIPAddress()
        {
            string IP = ""; ;
            IP = HttpContext.Current.Request.UserHostAddress == null ? "" : HttpContext.Current.Request.UserHostAddress;
            return IP;
        }
        public string GetConnectionString()
        {
            string conStr = ConfigurationManager.ConnectionStrings["oracleCon"].ConnectionString.ToString().Trim();

            return conStr;
        }

        public int GetNextIdWithTrans(string tableName, string columnName, System.Data.OracleClient.OracleCommand cmd)
        {
            cmd.CommandText = "";
            cmd.CommandText = "SELECT NVL(MAX(" + columnName + "),0)+1 FROM " + tableName;
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        public int GetIntRecord(string query)
        {
            DataTable dt = new DataTable();
            int result = 0;
            OpenConn();
            OracleDataAdapter adapter = new OracleDataAdapter();
            try
            {
                adapter.SelectCommand = new OracleCommand(query, con);
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = Convert.ToInt32(dt.Rows[0][0].ToString());
                }
            }
            catch (Exception)
            {
               
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public void apputility(string empid, string appId, string type)
        {

            int count = 0;
            string location = getIntoString("select location from erp_hr_dat where emp_no=round(" + empid + ")");
            if (type == "REP")
            {
                count = GetIntRecord("select u_count from wbs_app_utilization where app_id = '" + appId + "' and type = 'R' and location='" + location + "'");
                //operation for
                if (count > 0)
                {
                    count++;
                    string query = "update wbs_app_utilization set u_count='" + count + "' , LAST_UPDATE_BY ='" + empid + "' ,LAST_UPDATE_ON=sysdate where app_id = '" + appId + "' and type = 'R' and location='" + location + "'";
                    ExecuteNQuery(query);
                }
                else
                {
                    string query = "insert into wbs_app_utilization values('" + appId + "','" + empid + "',sysdate,'1','R','" + location + "',sysdate)";
                    ExecuteNQuery(query);
                }

            }
            else
            {
                count = GetIntRecord("select u_count from wbs_app_utilization where app_id = '" + appId + "' and type = 'I' and location='" + location + "'");
                //operation for
                if (count > 0)
                {
                    count++;
                    string query = "update wbs_app_utilization set u_count='" + count + "' , LAST_UPDATE_BY ='" + empid + "' , LAST_UPDATE_ON = sysdate where app_id = '" + appId + "' and type = 'I' and location='" + location + "'";
                    int i = ExecuteNQueryNew(query);
                }
                else
                {
                    string query = "insert into wbs_app_utilization values('" + appId + "','" + empid + "',sysdate,'1','I','" + location + "',sysdate)";
                    int i = ExecuteNQueryNew(query);
                }
            }


        }

        #region Function for ExecuteNonQuery
        public void ExecuteNQuery(string str)
        {
            try
            {
                OpenConn();
                //trans = con.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                cmd = new OracleCommand(str, con);
                //cmd.Transaction = trans;
                cmd.ExecuteNonQuery();
                //trans.Commit();
                con.Close();
                // return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ExecuteNQueryNew(string str)
        {
            int Result = 0;
            try
            {
                OpenConn();
                //trans = con.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                cmd = new OracleCommand(str, con);
                //cmd.Transaction = trans;
                Result = cmd.ExecuteNonQuery();
                //trans.Commit();
                con.Close();
                // return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        #endregion
        #region Function for Execute Reader
        public OracleDataReader getIntoDataReader(string str)
        {
            OpenConn();
            cmd = new OracleCommand(str, con);
            dr = cmd.ExecuteReader();
            return dr;
        }
        #endregion
        #region Function for Execute Reader
        public OracleDataReader getIntoDataReader2(string str)
        {
            OpenConn2();
            cmd2 = new OracleCommand(str, con2);
            dr2 = cmd2.ExecuteReader();
            return dr2;
        }
        #endregion
        static public byte[] Encryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey); encryptedData = RSA.Encrypt(Data, DoOAEPPadding);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "G@il";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            if (cipherText == null)
            {
                cipherText = "";
            }
            string EncryptionKey = "G@il";
            cipherText = cipherText.Replace(" ", "+");
            //cipherText = HttpContext.Current.Server.UrlDecode(cipherText);
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        static public byte[] Decryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    decryptedData = RSA.Decrypt(Data, DoOAEPPadding);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        #region Function for Dataset
        public DataSet getIntoDataSet(string str)
        {
            DataSet ds1 = new DataSet();
            try
            {
                OpenConn();

                da = new OracleDataAdapter(str, con);
                da.Fill(ds1);
                con.Close();
            }
            catch (Exception ee)
            {

                HttpContext.Current.Response.Write(ee.Message + "</br>" + ee.Source + "</br>" + ee.TargetSite + "</br>" + ee.StackTrace + "</br>" + ee.InnerException + "</br>" + ee.HelpLink + HttpContext.Current.Session["EmpId"].ToString() + "</br>" + str + "<br>" + ConfigurationManager.ConnectionStrings["constrCathode"].ToString());

            }
            return ds1;
        }
        #endregion
        public int GetNextId(string tableName, string columnName)
        {
            OpenConn();

            string str = "SELECT NVL(MAX(to_number(" + columnName + ")),0)+1 FROM " + tableName + "";
            cmd2 = new OracleCommand(str, con);
            int result = Convert.ToInt32(cmd2.ExecuteScalar());
            con.Close();
            return result;

        }
        public int GetNextIdWithTransNew(string tableName, string columnName, System.Data.OracleClient.OracleCommand cmd)
        {
            cmd.CommandText = "";
            cmd.CommandText = "SELECT NVL(MAX(" + columnName + "),0)+1 FROM " + tableName;
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        public int GetNextIdWithTransNewApp(string tableName, string columnName, System.Data.OracleClient.OracleCommand cmd)
        {
            cmd.CommandText = "";
            cmd.CommandText = @"SELECT  nvl(to_number(max(SUBSTR(" + columnName + ", 4, 10))),0)+1 FROM " + tableName;
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public int GetNextIdWithTransNewAppWithArea(string area, System.Data.OracleClient.OracleCommand cmd)
        {
            cmd.CommandText = "";
            cmd.CommandText = @"SELECT  nvl(to_number(max(SUBSTR(APPLICATION_NO, 4, 10))),0)+1 FROM CGD_APPLICANT_MASTER

where   to_number(SUBSTR(APPLICATION_NO, 2, 2)) ='" + area + "'";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        public int GetNextIdCon(string tableName, string columnName, string cond)
        {
            OpenConn();

            string str = "SELECT NVL(MAX(" + columnName + "),0)+1 FROM " + tableName + " " + cond + "";
            cmd2 = new OracleCommand(str, con);
            int result = Convert.ToInt32(cmd2.ExecuteScalar());
            con.Close();
            return result;

        }
        public int getScalarValue(string str)
        {
            int result = 0;
            try
            {
                OpenConn();
                cmd2 = new OracleCommand(str, con);
                result = Convert.ToInt32(cmd2.ExecuteScalar());
                con.Close();
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        public int ExecuteScalar(string str)
        {
            int result = 0;
            OpenConn();
            cmd2 = new OracleCommand(str, con);
            var res = cmd2.ExecuteScalar();
            if (res == null)
            {
                result = 0;
            }
            else
            {
                result = 1;
            }
            con.Close();
            return result;
        }

        public object ExecuteScalarValue(string str)
        {
            OpenConn();
            cmd2 = new OracleCommand(str, con);
            var result = cmd2.ExecuteScalar();

            con.Close();
            return result;
        }

        #region Function for DataTable
        public DataTable getIntoDatatable(string str)
     {
            DataTable ds1 = new DataTable();
            try
            {
                OpenConn();


                da = new OracleDataAdapter(str, con);
                da.Fill(ds1);
                con.Close();

            }
            catch (Exception ex)
            {

                WriteToErrorLog(ex.Message, ex.StackTrace, "Error");

            }
            return ds1;
        }
        #endregion


        public static void WriteToErrorLog(string msg, string stkTrace, string title)
        {

            string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString();
            //
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            string sHour = DateTime.Now.Hour.ToString();
            string sMnt = DateTime.Now.Minute.ToString();
            string sSec = DateTime.Now.Second.ToString();
            string sErrorDate = sDay + "-" + sMonth + "-" + sYear + "-" + sHour + "-" + sMnt + "-" + sSec;

            string strPath = HttpContext.Current.Server.MapPath("~/ErrorLogs/");

            if (!System.IO.Directory.Exists(strPath))
            {
                System.IO.Directory.CreateDirectory(strPath);
            }

            strPath = strPath + sErrorDate + ".html";
            if (!System.IO.File.Exists(strPath))
            {
                System.IO.File.Create(strPath).Dispose();
            }

            //log it
            FileStream fs1 = new FileStream(strPath, FileMode.Append, FileAccess.Write);
            StreamWriter s1 = new StreamWriter(fs1);
            s1.WriteLine("Title: <b>" + title + "</b><br/>");
            s1.WriteLine("Message: " + msg + "<br/>");
            s1.WriteLine("StackTrace: <i>" + stkTrace + "</i><br/>");
            s1.WriteLine("Date/Time: " + sLogFormat);
            s1.WriteLine("<hr/>");
            s1.Close();
            fs1.Close();

        }

        #region Function for String
        public string getIntoString(string str)
        {
            OpenConn();

            DataTable dt = new DataTable();
            string result = string.Empty;
            OracleDataAdapter adapter = new OracleDataAdapter();
            try
            {
                adapter.SelectCommand = new OracleCommand(str, con);
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception)
            {

            }
            finally
            {

            }
            return result;
        }
        #endregion


        #region Function for Trace Logs
        public static void WriteTraceLog(string msg, string fileName, string title)
        {
            string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString();
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            string sHour = DateTime.Now.Hour.ToString();
            string sMnt = DateTime.Now.Minute.ToString();
            string sSec = DateTime.Now.Second.ToString();
            string sTraceDate = sDay + "-" + sMonth + "-" + sYear + "-" + sHour + "-" + sMnt + "-" + sSec;

            string strPath = HttpContext.Current.Server.MapPath("~/TraceLogs/");
            //string strPath = "C:\\Users\\varunchander96\\Documents\\Visual Studio 2013\\Projects\\PaperlessMeet\\PaperlessMeet\\TraceLogs\\";

            if (!System.IO.Directory.Exists(strPath))
            {
                System.IO.Directory.CreateDirectory(strPath);
            }

            strPath = strPath + fileName + ".html";
            if (!System.IO.File.Exists(strPath))
            {
                System.IO.File.Create(strPath).Dispose();
            }

            //log it
            FileStream fs1 = new FileStream(strPath, FileMode.Append, FileAccess.Write);
            StreamWriter s1 = new StreamWriter(fs1);
            s1.WriteLine("Title: <b>" + title + "</b><br/>");
            s1.WriteLine("Message: " + msg + "<br/>");
            s1.WriteLine("Date/Time: " + sLogFormat);
            s1.WriteLine("<hr/>");
            s1.Close();
            fs1.Close();

        }
        #endregion


    }
}