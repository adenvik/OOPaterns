namespace OOPatterns.Core.Utils.Log
{
    /// <summary>
    /// Logger
    /// </summary>
    public interface ILoggable
    {
        /// <summary>
        /// Path to file with information about program life cycle
        /// </summary>
        string PathToFile { get; set; }

        /// <summary>
        /// Print Message to the specified log
        /// </summary>
        /// <param name="Message"></param>
        void Log(string Message);
    }
}
