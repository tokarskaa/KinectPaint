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
   public interface IPainter
    {
        KinectPainter painter { get; set; }
        Point SkeletonPointToScreen(SkeletonPoint point);
        //void DrawJoints(Point point,DrawingContext dc, JointCollection joints);
        //void Clear(Point point, DrawingContext dc, JointCollection joints);
    }
}
