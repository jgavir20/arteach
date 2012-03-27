// http://stackoverflow.com/questions/1176910/finding-specific-pixel-colors-of-a-bitmapimage
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.XPath;
using System.Media;

namespace ReliefDrawingReaderWPF00
{
    struct FigureSoundSpot
    {
        public SoundPlayer s;
        public double x, y;
    }

    public partial class MainWindow : Window
    {
        XmlDocument doc;

        XmlTextReader reader;// = new XmlTextReader("cd.xml");

        List<FigureSoundSpot> soundSpots = new List<FigureSoundSpot>();

        public MainWindow()
        {
            InitializeComponent();
            doc = new XmlDocument();
            reader = new XmlTextReader("Annotations.xml");
            doc.Load(reader);
            //List<String> soundFiles = new List<String>();
            //List<int> xCoords = new List<int>();
            //List<int> yCoords = new List<int>();
            
            XmlNodeList annotationList = doc.SelectNodes("/Annotations/Annotation");
            foreach (XmlNode annotation in annotationList)
            {
                XmlNode id = annotation.SelectSingleNode("Id");
                FigureSoundSpot fss = new FigureSoundSpot();
                SoundPlayer sp = new SoundPlayer();
                fss.s = sp;
                fss.s.SoundLocation = "..\\..\\..\\ANNOTATED_PLANO_INCLINADO\\" + id.InnerText;
                String[] xy = annotation.SelectSingleNode("XY").InnerText.Split(',');
                fss.x = 640.0 * Convert.ToDouble(xy[0]);
                fss.y = 480.0 * Convert.ToDouble(xy[1]);
                soundSpots.Add(fss);
            }

            foreach (FigureSoundSpot fss in soundSpots)
            {
                Ellipse e = new Ellipse();
                canvas.Children.Add(e);
                e.Height = 20;
                e.Width = 20;
                e.Stroke = Brushes.Blue;
                TranslateTransform t = new TranslateTransform(fss.x, fss.y);
                e.RenderTransform = t;
                e.Visibility = System.Windows.Visibility.Visible;
            }

            //            reader.MoveToContent();
            //            while (reader.MoveToNextAttribute())
            //            {
            //                XmlNode a = doc.ReadNode(reader);
            ////                Console.WriteLine(a.Item[0]);
            //            }
        }

        private void mainImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // JFH ERROR??? WHY DO THESE VARIABLES HAVE THE SAME NAME???
            int closestDistanceIndex = 0;
            double closestDistance = 307200;
            for (int i = 0; i < soundSpots.Count; i++) //foreach (FigureSoundSpot fss in soundSpots)
            {
                double xImg = e.GetPosition(sender as Image).X;
                double yImg = e.GetPosition(sender as Image).Y;
                double x = soundSpots[i].x;
                double y = soundSpots[i].y;
                double d = Math.Sqrt((x - xImg) * (x - xImg) + (y - yImg) * (y - yImg));
                if (d < closestDistance)
                {
                    closestDistanceIndex = i;
                    closestDistance = d;
                }
            }
            if (closestDistance < 50)
            {
                soundSpots[closestDistanceIndex].s.Play();
            }

        }
    }
}
