using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MathAttack.Class;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MathAttack

{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Game level resources
        private CanvasBitmap BG, StartScreen, ScoreScreen, Level1, Blast, MinusMonster, PlusMonster, ENEMY_IMG, Weapon;

        // Boundaries of the application view
        public static Rect boundaries;

        // Width and Height of canvas and scale width and height
        public static float DesignWidth = 1920;
        public static float DesignHeight = 1080;
        public static float scaleWidth, scaleHeight, pointX, pointY;
        private float photonX;
        private float photonY;


        // Round Timer
        private DispatcherTimer RoundTimer = new DispatcherTimer();

        // Enemy Timer
        private DispatcherTimer EnemyTimer = new DispatcherTimer();


        // List of projectiles positions
        private List<float> blastXPos = new List<float>();
        private List<float> blastYPos = new List<float>();
        private List<float> percent = new List<float>();

        // List of Enemies
        private List<float> enemyXpos = new List<float>();
        private List<float> enemyYpos = new List<float>();
        private List<int> enemyType = new List<int>();

        // Random Number Generators
        private Random EnemyTypeRand = new Random(); // Enemy Type
        private Random EnemyGenIntervalRand = new Random(); // Generation Interval



        // Level of the game
        private int GameState = 0;

        // Timer starting value
        private int countdown = 10;

        // Controls when a round is over
        private bool RoundEnded = false;

        public MainPage()
        {
            this.InitializeComponent();
            // Fires when the window has changed its rendering size
            Window.Current.SizeChanged += Current_SizeChanged;
            // Set the scale on page load
            Scaling.SetScale();


            photonX = (float)boundaries.Width / 2;
            photonY = (float)boundaries.Height - (140f * scaleHeight);

            // Round Timer
            RoundTimer.Tick += RoundTimer_Tick;
            RoundTimer.Interval = new TimeSpan(0, 0, 1);

            // Enemy Timer
            EnemyTimer.Tick += EnemyTimer_Tick;
            // Controls intervals that spawn enemies
            EnemyTimer.Interval = new TimeSpan(0, 0, 0, 0, EnemyGenIntervalRand.Next(300, 3000));

        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            boundaries = ApplicationView.GetForCurrentView().VisibleBounds;

            // Everytime the window size changes reset the scale
             Scaling.SetScale();

            // Adjust projectiles for scaling
            photonX = (float)boundaries.Width / 2;
            photonY = (float)boundaries.Height - (140f * scaleHeight);
    }

        // Adapted from https://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_UI_Xaml_CanvasControl.htm
        private void GameCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            // Calls CreateResourcesAsync Task and ensures could will not execute until the task completes
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        async Task CreateResourcesAsync(CanvasControl sender)
        {
            // Loads the demo start screen
            StartScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/how-to-play.png"));

            // Loads Level 1 screen
            Level1 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/AVP.jpg"));

            // Loads the score screen
            ScoreScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/ALwallpaper.png"));

            // Loads a blast projectile
            Blast = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/blast.png"));

            // Load the subtraction symbol monster
            MinusMonster = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/minusmonster.png"));

            // Load the addition symbol monster
            PlusMonster = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/plusmonster.png"));

            //
            Weapon = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/weapon.png"));

        }

        private void GameCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            // Load initial Game State
            GSM();

            // Draw the start screen
            args.DrawingSession.DrawImage(Scaling.ScaleImage(BG));
            args.DrawingSession.DrawText(countdown.ToString(), 100, 100, Colors.Yellow);

            // Only draw enemies, weapons and projectiles if the start screen has been passed
            if(GameState > 0)
            {

                // Draw the weapon first
                args.DrawingSession.DrawImage(Scaling.ScaleImage(Weapon), (float)boundaries.Width / 2 - (50 * scaleWidth), (float)boundaries.Height - (150 * scaleHeight)); // Decrease by 30 to compensate for the offset of the mouse

                // Draw the enemies
                for (int j = 0; j < enemyXpos.Count; j++)
                {
                    if (enemyType[j] == 1) { ENEMY_IMG = MinusMonster; }

                    if (enemyType[j] == 2) { ENEMY_IMG = PlusMonster; }
                    enemyXpos[j] += 3;
                    args.DrawingSession.DrawImage(Scaling.ScaleImage(ENEMY_IMG), enemyXpos[j], enemyYpos[j]);
                }
                //Draw projectiles
                for (int i = 0; i < blastXPos.Count; i++)
                {

                    // Linear Interpolation for moving the projectiles
                    // Adapted from https://stackoverflow.com/questions/25276516/linear-interpolation-for-dummies
                    pointX = (photonX + (blastXPos[i] - photonX) * percent[i]);
                    pointY = (photonY + (blastYPos[i] - photonY) * percent[i]);

                    args.DrawingSession.DrawImage(Scaling.ScaleImage(Blast), pointX - (30 * scaleWidth), pointY - (30 * scaleHeight)); // Decrease by 30 to compensate for the offset of the mouse

                    // Increment the position of the projectile to give the appearance of movement
                    percent[i] += (0.040f);

                    // If the projectile goes off the screen
                    if (pointY < 0f)
                    {
                        // Remove any projectiles that go off the top of the screen
                        blastXPos.RemoveAt(i);
                        blastYPos.RemoveAt(i);
                        percent.RemoveAt(i);
                    }
                }
            }
            

            // Redraw everything in the draw method (roughly 60fps)
            GameCanvas.Invalidate();
        }

        // Handles touch screen taps
        private void GameCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Display the score screen if the round ends
            if (RoundEnded == true)
            {

                GameState = 0;
                // Reset the round
                RoundEnded = false;
                countdown = 60;

                // Stop the enemy timer
                EnemyTimer.Stop();
                enemyXpos.Clear();
                enemyYpos.Clear();
                enemyType.Clear();

            }
            else
            {
                // If the screen is tapped/clicked go up one level
                if (GameState == 0)
                {
                    GameState += 1;
                    RoundTimer.Start();
                    EnemyTimer.Start();
                   

                } else if (GameState > 0)
                {
                    // Add the xy coordinates of a blast projectile from user mouse position
                    blastXPos.Add((float)e.GetPosition(GameCanvas).X);
                    blastYPos.Add((float)e.GetPosition(GameCanvas).Y);
                    percent.Add(0f);
                }
            }
           
        }

        // The Game State Manager
        public void GSM()
        {
            // Shows the score screen if the round ends
            if (RoundEnded == true)
            {
                BG = ScoreScreen;
            }
            else
            {
                // Loads the Start Screen
                if (GameState == 0)
                {
                    BG = StartScreen;
                }

                // Loads Level 1
                else if (GameState == 1)
                {
                    BG = Level1;
                }
            }
            
        }

        // RoundTimer_Tick controls the decrementing round time
        private void RoundTimer_Tick(object sender, object e)
        {
            // Decrement the timer
            countdown -= 1;

            // Stops the timer once it reaches 0 and ends the round
            if (countdown < 1)
            {
                RoundTimer.Stop();
                RoundEnded = true;
            }
        }


        private void EnemyTimer_Tick(object sender, object e)
        {
            // Randomly choose what type of enemy to generate
            int eType = EnemyTypeRand.Next(1, 3);

            enemyXpos.Add(50 * scaleWidth);
            enemyYpos.Add(110 * scaleHeight);
            enemyType.Add(eType);

            // Regenerate a random number so individual enemies spawn differently
            EnemyTimer.Interval = new TimeSpan(0, 0, 0, 0, EnemyGenIntervalRand.Next(300, 3000));
        }
    }
}
