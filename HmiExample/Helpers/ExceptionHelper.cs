using System;
using System.Text;

namespace ProductionEquipmentControlSoftware.Helpers
{
    public static class ExceptionHelper
    {
        public static string GetRootMessage(this Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex.Message;
        }

        public static string GetAllExceptionInfo(this Exception ex)
        {
            StringBuilder sbexception = new StringBuilder();

            int i = 1;
            sbexception.Append(GetExceptionInfo(ex, i));

            while (ex.InnerException != null)
            {
                i++;
                ex = ex.InnerException;
                sbexception.Append(GetExceptionInfo(ex, i));
            }

            return sbexception.ToString();
        }

        private static string GetExceptionInfo(Exception ex, int count)
        {
            StringBuilder sbexception = new StringBuilder();
            sbexception.AppendLine(string.Format(""));
            sbexception.AppendLine(string.Format(""));
            sbexception.AppendLine(string.Format("************************************************"));
            sbexception.AppendLine(string.Format("************************************************"));
            sbexception.AppendLine(string.Format(" Inner Exception : No.{0} ", count));
            sbexception.AppendLine(string.Format("************************************************"));
            sbexception.AppendLine(string.Format("=================================================="));
            sbexception.AppendLine(string.Format(" Error Message : {0} ", ex.Message));
            sbexception.AppendLine(string.Format("=================================================="));
            #region Mine Thru data dictionary

            try
            {
                sbexception.AppendLine(string.Format("=================================================="));
                sbexception.AppendLine(string.Format(" Data parameters Count at Source :{0}", ex.Data.Count));
                sbexception.AppendLine(string.Format("=================================================="));

                string skey = string.Empty;
                foreach (object key in ex.Data.Keys)
                {
                    try
                    {
                        if (key != null)
                        {
                            skey = Convert.ToString(key);
                            sbexception.AppendLine(string.Format(" Key :{0} , Value:{1}", skey, Convert.ToString(ex.Data[key])));
                        }
                        else
                        {
                            sbexception.AppendLine(string.Format(" Key is null"));
                        }
                    }
                    catch (Exception e1)
                    {
                        sbexception.AppendLine(string.Format("**  Exception occurred when writting log *** [{0}] ", e1.Message));
                    }
                }
            }
            catch (Exception ex1)
            {
                sbexception.AppendLine(string.Format("**  Exception occurred when writting log *** [{0}] ", ex1.Message));
            }

            #endregion
            sbexception.AppendLine(string.Format("=================================================="));
            sbexception.AppendLine(string.Format(" Source : {0} ", ex.Source));
            sbexception.AppendLine(string.Format("=================================================="));
            sbexception.AppendLine(string.Format(" StackTrace : {0} ", ex.StackTrace));
            sbexception.AppendLine(string.Format("=================================================="));
            sbexception.AppendLine(string.Format(" TargetSite : {0} ", ex.TargetSite));
            sbexception.AppendLine(string.Format("************************************************"));
            sbexception.AppendLine(string.Format(" Finished Writting Exception info :{0} ", count));
            sbexception.AppendLine(string.Format("************************************************"));
            sbexception.AppendLine(string.Format("************************************************"));
            sbexception.AppendLine(string.Format(""));
            sbexception.AppendLine(string.Format(""));

            return sbexception.ToString();
        }
    }
}
