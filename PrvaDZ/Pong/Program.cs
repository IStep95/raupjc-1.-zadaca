using System;

namespace Pong
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Game1 igra = new Game1();
            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}
