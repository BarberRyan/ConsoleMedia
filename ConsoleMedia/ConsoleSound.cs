using System.Media;

namespace ConsoleSound
{
    internal class ConsoleSound
    {
        /// <summary>
        /// Creates a SoundPlayer instance for playing a specified sound
        /// </summary>
        /// <param name="fileName">filename of the sound to play</param>
        /// <returns>SoundPlayer instance</returns>
        public static SoundPlayer SoundMaker(string fileName)
        {
            string soundDir = System.IO.Directory.GetCurrentDirectory() + @"\sounds\" + fileName;
            SoundPlayer sound = new() { SoundLocation = soundDir };
            return sound;
        }
        /// <summary>
        /// Creates a SoundPlayer instance and plays the sound
        /// </summary>
        /// <param name="fileName">filename of the sound to play</param>
        public static void PlaySound(string fileName)
        {
            SoundPlayer sound = SoundMaker(fileName);
            sound.Play();
        }
    }
}
