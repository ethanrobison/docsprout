namespace Code.Session
{
    /// <summary>
    /// Handles cross-platform stuff.
    /// Lots of nasty stuff, but at least it's black-boxed.
    /// </summary>
    public class PlatformManager : ISessionManager
    {
        private enum Platform
        {
            OSX,
            Windows,
            Linux
        }

        private Platform _platform;

        public void Initialize () { }

        public void ShutDown () { }
    }
}