namespace TourPlanner.Common.Logging
{
    public static class LoggerFactory
    {
        public static ILoggerWrapper GetLogger(Type type)
        {
            return Log4NetWrapper.CreateLogger("./log4net.config", type);
        }
    }
}
