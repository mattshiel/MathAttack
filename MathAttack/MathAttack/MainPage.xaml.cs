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
    
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MathAttack

{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Game levels
        public CanvasBitmap BG, StartScreen, Level1;

        // Boundaries of the application view
        public static Rect boundaries = ApplicationView.GetForCurrentView().VisibleBounds;

        // Width and Height of canvas and scale width and height
        public static float DesignWidth = 1920;
        public static float DesignHeight = 1080;
        public static float scaleWidth, scaleHeight;

        public int GameState = 0; // Start screen

        public MainPage()
        {
            this.InitializeComponent();
            // Fires when the window has changed its rendering size
            Window.Current.SizeChanged += Current_SizeChanged;
            // Set the scale on page load
            Scaling.SetScale();
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            // Everytime the window size changes reset the scale
            Scaling.SetScale();
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
            // 'await' suspends the calling method and yields control back to its caller until the awaited task is complete.
            Level1 = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/AVP.jpg"));
            
        }

        private void GameCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            // Load initial Game State
            GSM();
            // Draw the start screen
            args.DrawingSession.DrawImage(Scaling.ScaleImage(BG));

            // Redraw everything in the draw method (roughly 60fps)
            GameCanvas.Invalidate();
        }

        // Handles touch screen taps
        private void GameCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // If the screen is tapped/clicked go up one level
            if(GameState == 0)
            {
                GameState += 1;
            }
        }

        // The Game State Manager
        public void GSM()
        {
            // Start Screen
            if (GameState == 0)
            {
                BG = StartScreen;
            }

            // Level 1
            else if (GameState == 1)
            {
                BG = Level1;
            }
        }             
    }
}
