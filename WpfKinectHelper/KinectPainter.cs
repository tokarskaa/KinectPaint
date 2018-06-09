using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WpfKinectHelper
{
   public sealed class KinectPainter
    {
        private bool rightHandFalg { get; set; } = false;
        private bool leftHandFalg { get; set; } = false;
        private const float RenderWidth = 640.0f; // TO-DO: Move to constructor/initialization logic, allow user to set render size
        private const float RenderHeight = 480.0f;
        private const double JointThickness = 3;
        private const double BodyCenterThickness = 10;
        private const double ClipBoundsThickness = 10;
        private Brush backgroundBrush = Brushes.White;
        private readonly Brush centerPointBrush = Brushes.Blue;
        private readonly Brush trackedJointBrush = Brushes.LightBlue;
        private readonly Brush inferredJointBrush = Brushes.Yellow;
        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        private static KinectPainter painter;
        private KinectPainter() {  }
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
      public void Paint(IPainter painter,DrawingContext dc, JointCollection joints)
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
                dc.DrawEllipse(this.trackedJointBrush, null, painter.SkeletonPointToScreen(rightHand.Position), JointThickness, JointThickness);
            if (leftHandFalg)
                dc.DrawEllipse(this.trackedJointBrush, null, painter.SkeletonPointToScreen(leftHand.Position), JointThickness, JointThickness);
        }
    }
}
