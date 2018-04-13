using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System;

namespace MathAttack.Class
{
    class Storage
    {

        // Storage Folder for high scores
        public static StorageFolder StorageFolder = ApplicationData.Current.LocalFolder;

        // Creates a file to store high scores
        public static async void CreateFile()
        {
            try
            {
                // Create the file but open if it already exists
                // Creation Collision Option prevents player from losing high scores after the game closes
                await StorageFolder.CreateFileAsync("MathAttackScore.txt", CreationCollisionOption.OpenIfExists);
            }
            catch
            {
            }
        }

        // Reads the high score file
        public static async void ReadFile()
        {
            try
            {
                StorageFile DataFile = await StorageFolder.GetFileAsync("MathAttackScore.txt");
                MainPage.STRHighScore = await FileIO.ReadTextAsync(DataFile);
            }
            catch
            {

            }
        }

        // Updates the score in the high score file
        public static async void UpdateScore()
        {
            if (MainPage.gameScore > Convert.ToInt16(MainPage.STRHighScore))
            {
                try
                {
                    StorageFile DataFile = await StorageFolder.CreateFileAsync("MathAttackScore.txt");
                    await FileIO.WriteTextAsync(DataFile, MainPage.gameScore.ToString());
                    ReadFile(); // Update Score at the end of the round
                }
                catch
                {

                }
            }
        }
    }
}
