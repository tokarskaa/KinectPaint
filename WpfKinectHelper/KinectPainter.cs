using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfKinectHelper
{
   public sealed class KinectPainter//singleton class for painting
    {
        private bool rightHandFalg { get; set; } = false;
        private bool leftHandFalg { get; set; } = false;
        private const float RenderWidth = 640.0f; // TO-DO: Move to constructor/initialization logic, allow user to set render size
        private const float RenderHeight = 480.0f;
        private const double DrawThickness = 6;
        private const double ClearThickness = 10;
        //private const double ClipBoundsThickness = 10;
        private Brush backgroundBrush = Brushes.Black;
        //private readonly Brush centerPointBrush = Brushes.Blue;
        private readonly Brush trackedJointBrush = Brushes.LightBlue;
        private readonly Brush CleanerBrush = Brushes.Green;
        //private readonly Brush inferredJointBrush = Brushes.Yellow;
        //private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);
        //private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        private static KinectPainter painter;
        private KinectPainter() {  }
        private RenderTargetBitmap bmp;
       public static KinectPainter getPainter
        {
            get
            {
                if (painter == null)
                    painter = new KinectPainter();
                return painter;
            }
            private set { }
        }
      public void Paint(IPainter painter,DrawingContext dc, JointCollection joints)//change dc to draw on screen
        {
            Joint rightHand = joints.Where(joint => joint.JointType == JointType.HandRight).FirstOrDefault();
            Joint leftHand = joints.Where(joint => joint.JointType == JointType.HandLeft).FirstOrDefault();
            Joint head = joints.Where(joint => joint.JointType == JointType.Head).FirstOrDefault();
            if (rightHand.Position.Y > head.Position.Y)
            {
                rightHandFalg = true;
                leftHandFalg = false;
            }
            if (leftHand.Position.Y > head.Position.Y)
            {
                leftHandFalg = true;
                rightHandFalg = false;
            }
            if (rightHandFalg)
                DrawInBmp(painter.SkeletonPointToScreen(rightHand.Position),DrawThickness,trackedJointBrush);
            if (leftHandFalg)
                DrawInBmp(painter.SkeletonPointToScreen(leftHand.Position),ClearThickness,backgroundBrush);
            dc.DrawImage(bmp, new System.Windows.Rect(0,0,RenderWidth,RenderHeight));
            if (leftHandFalg)
                dc.DrawRectangle(CleanerBrush, null, new Rect(leftHand.Position.X - ClearThickness, leftHand.Position.Y - ClearThickness / 2, ClearThickness * 2, ClearThickness));
        }
       private void DrawInBmp(Point point,double thickness,Brush color)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawEllipse(color, null,point,thickness,thickness);
            drawingContext.Close();
            if (bmp == null)
                bmp = new RenderTargetBitmap((int)RenderWidth, (int)RenderHeight,96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
        }
    }
}
