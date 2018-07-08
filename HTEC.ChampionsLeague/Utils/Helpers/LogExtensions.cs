using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace HTEC.ChampionsLeague.Utils.Helpers
{
    public static class LogExtensions
    {
        /// <summary>
        /// Log exception into file
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="ex"></param>
        public static void LogException(this ILogger logger, Exception ex)
        {
            logger.LogCustomException(ex);
        }

        /// <summary>
        /// Format exception
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="ex"></param>
        private static void LogCustomException(this ILogger logger, Exception ex)
        {
            var errorMessage = new StringBuilder("Message: ");
            errorMessage.Append(ex.Message);
            errorMessage.Append(Environment.NewLine);
            errorMessage.Append("Exception Type: ");
            errorMessage.Append(ex.GetType());
            errorMessage.Append(Environment.NewLine);
            errorMessage.Append("Source: ");
            errorMessage.Append(ex.Source);
            errorMessage.Append(Environment.NewLine);
            errorMessage.Append("StacktTrace: ");
            errorMessage.Append(Environment.NewLine);
            errorMessage.Append(ex.StackTrace);
            errorMessage.Append(Environment.NewLine);

            if (ex.InnerException != null)
            {
                errorMessage.Append("InnerException Message: ");
                errorMessage.Append(ex.InnerException.Message);
                errorMessage.Append(Environment.NewLine);
                errorMessage.Append("InnerException Type: ");
                errorMessage.Append(ex.InnerException.GetType());
                errorMessage.Append(Environment.NewLine);
                errorMessage.Append("InnerException Source: ");
                errorMessage.Append(ex.InnerException.Source);
                errorMessage.Append(Environment.NewLine);
                errorMessage.Append("InnerException StackTrace: ");
                errorMessage.Append(Environment.NewLine);
                errorMessage.Append(ex.InnerException.StackTrace);
                errorMessage.Append(Environment.NewLine);

                if (ex.InnerException.InnerException != null)
                {
                    errorMessage.Append("InnerException InnerException Message: ");
                    errorMessage.Append(ex.InnerException.InnerException.Message);
                    errorMessage.Append(Environment.NewLine);
                    errorMessage.Append("InnerException InnerException Type: ");
                    errorMessage.Append(ex.InnerException.InnerException.GetType());
                    errorMessage.Append(Environment.NewLine);
                    errorMessage.Append("InnerException InnerException Source: ");
                    errorMessage.Append(ex.InnerException.InnerException.Source);
                    errorMessage.Append(Environment.NewLine);
                    errorMessage.Append("InnerException InnerException StackTrace: ");
                    errorMessage.Append(Environment.NewLine);
                    errorMessage.Append(ex.InnerException.InnerException.StackTrace);
                    errorMessage.Append(Environment.NewLine);
                }
            }

            logger.LogError(errorMessage.ToString());
        }
    }
}
