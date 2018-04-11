/*
    References: 
    * https://en.wikipedia.org/wiki/Affine_transformation
    * https://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_Transform2DEffect.htm
*/

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace MathAttack.Class
{
    class Scaling
    {
        public static void SetScale()
        {
            MainPage.boundaries = ApplicationView.GetForCurrentView().VisibleBounds;
            // Provide the values to multiply against when scaling anything
            MainPage.scaleWidth = (float)MainPage.boundaries.Width / MainPage.DesignWidth;
            MainPage.scaleHeight = (float)MainPage.boundaries.Height / MainPage.DesignHeight;

        }

        // Applies a 2D affine transformation to an image
        // Affine transformation is a linear mapping method 
        // that preserves points, straight lines, and planes.
        public static Transform2DEffect ScaleImage(CanvasBitmap source)
        { 
        
            // Declares transformation and specifies source
            Transform2DEffect image;
            image = new Transform2DEffect() { Source = source };

            // Takes the image and scales it to the defined width and height (x and y) using a matrix 
            image.TransformMatrix = Matrix3x2.CreateScale(MainPage.scaleWidth, MainPage.scaleHeight);
            
            return image;
        }
    }
}
