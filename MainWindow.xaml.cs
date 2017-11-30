
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;

namespace DreieckeZählen
{
    public partial class MainWindow : Window
    {
        bool shown = false;
        bool shownP = false;
        bool shownS = false;
        List<cStrecken> meineStrecken = new List<cStrecken>();
        List<cPunkte> meinePunkte = new List<cPunkte>();
        List<cDreiecke> meineDreiecke = new List<cDreiecke>();
        public MainWindow()
        {
            InitializeComponent();
            lbCoord.MouseDoubleClick += new MouseButtonEventHandler(lbCoord_MouseDoubleClick);
            lbSp.MouseDoubleClick += new MouseButtonEventHandler(lbSp_MouseDoubleClick);
            lbDr.MouseDoubleClick += new MouseButtonEventHandler(lbDr_MouseDoubleClick);
        }

        private void miEinselen_Click(object sender, RoutedEventArgs e)
        {
            clearAll();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text Files|*.txt";
            openFileDialog1.Title = "Select a Text File";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] dateiinhalt = File.ReadAllLines(openFileDialog1.FileName);
                for (int i = 1; i < dateiinhalt.Length; i++)
                {
                    string[] tempCoord = dateiinhalt[i].Split(' ');
                    float tempEp1X = float.Parse(tempCoord[0], CultureInfo.InvariantCulture);
                    float tempEp1Y = float.Parse(tempCoord[1], CultureInfo.InvariantCulture);
                    float tempEp2X = float.Parse(tempCoord[2], CultureInfo.InvariantCulture);
                    float tempEp2Y = float.Parse(tempCoord[3], CultureInfo.InvariantCulture);
                    cStrecken tempStrecke = new cStrecken(tempEp1X, tempEp1Y, tempEp2X, tempEp2Y);
                    meineStrecken.Add(tempStrecke);
                }

                foreach (cStrecken Strecke in meineStrecken)
                {
                    lbCoord.Items.Add("ID: " + Strecke.MyId + " P1: (" + Strecke.Ep1X + "|" + Strecke.Ep1Y + ") P2: (" + Strecke.Ep2X + "|" + Strecke.Ep2Y + ")");
                    Line tempLine = new Line();
                    tempLine.Stroke = System.Windows.Media.Brushes.Black;
                    tempLine.StrokeThickness = 3;
                    tempLine.X1 = Strecke.Ep1X;
                    tempLine.Y1 = Strecke.Ep1Y;
                    tempLine.X2 = Strecke.Ep2X;
                    tempLine.Y2 = Strecke.Ep2Y;
                    CanvasEinlesen.Children.Add(tempLine);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Keine Datei gewählt");
            }
        }

        private void miAuswerten_Click(object sender, RoutedEventArgs e)
        {
            foreach (cStrecken strecke1 in meineStrecken)
            {
                foreach (cStrecken strecke2 in meineStrecken)
                {
                    if (strecke1.MyId != strecke2.MyId)
                    {
                        bool line_intersect;
                        bool segments_intersect;
                        PointF intersection;
                        PointF close_p1;
                        PointF close_p2;
                        FindIntersection(strecke1, strecke2, out line_intersect, out segments_intersect, out intersection, out close_p1, out close_p2);
                        if (segments_intersect == true)
                        {
                            cPunkte tempPoint = new cPunkte(intersection.X, intersection.Y, strecke1.MyId, strecke2.MyId);
                            bool dupe = false;
                            foreach (cPunkte point in meinePunkte)
                            {
                                if (point.SpX1 == tempPoint.SpX1 && point.SpY1 == tempPoint.SpY1)
                                {
                                    dupe = true;
                                }
                            }

                            if (dupe == false)
                            {
                                meinePunkte.Add(tempPoint);
                            }
                        }
                    }
                }
            }

            foreach (cPunkte punkt1 in meinePunkte)
            {
                foreach (cPunkte punkt2 in meinePunkte)
                {
                    foreach (cPunkte punkt3 in meinePunkte)
                    {
                        if (punkt1.MyId != punkt2.MyId && punkt1.MyId != punkt3.MyId && punkt2.MyId != punkt3.MyId)
                        {
                            bool dreick;
                            FindDreiecke(punkt1, punkt2, punkt3, out dreick);
                            if (dreick == true)
                            {
                                cDreiecke tempDreieck = new cDreiecke(punkt1.SpX1, punkt1.SpY1, punkt2.SpX1, punkt2.SpY1, punkt3.SpX1, punkt3.SpY1);
                                bool dupe = false;
                                PointF tempDreieckP1 = new PointF(tempDreieck.AX, tempDreieck.AY);
                                PointF tempDreieckP2 = new PointF(tempDreieck.BX, tempDreieck.BY);
                                PointF tempDreieckP3 = new PointF(tempDreieck.CX, tempDreieck.CY);
                                foreach (cDreiecke Dreieck in meineDreiecke)
                                {
                                    PointF dreieckP1 = new PointF(Dreieck.AX, Dreieck.AY);
                                    PointF dreieckP2 = new PointF(Dreieck.BX, Dreieck.BY);
                                    PointF dreieckP3 = new PointF(Dreieck.CX, Dreieck.CY);
                                    if (dreieckP1 == tempDreieckP1 || dreieckP1 == tempDreieckP2 || dreieckP1 == tempDreieckP3)
                                    {
                                        if (dreieckP2 == tempDreieckP2 || dreieckP2 == tempDreieckP1 || dreieckP2 == tempDreieckP3)
                                        {
                                            if (dreieckP3 == tempDreieckP3 || dreieckP3 == tempDreieckP2 || dreieckP3 == tempDreieckP1)
                                            {
                                                dupe = true;
                                            }
                                        }
                                    }
                                }

                                if (dupe == false)
                                {
                                    meineDreiecke.Add(tempDreieck);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void miPunkte_Click(object sender, RoutedEventArgs e)
        {
            foreach (cStrecken Strecke in meineStrecken)
            {
                Line tempLine = new Line();
                tempLine.Stroke = System.Windows.Media.Brushes.Black;
                tempLine.StrokeThickness = 3;
                tempLine.X1 = Strecke.Ep1X;
                tempLine.Y1 = Strecke.Ep1Y;
                tempLine.X2 = Strecke.Ep2X;
                tempLine.Y2 = Strecke.Ep2Y;
                CanvasSp.Children.Add(tempLine);
            }

            foreach (cPunkte Punkt in meinePunkte)
            {
                lbSp.Items.Add("StreckenID's: " + Punkt.StreckenID11 + '/' + Punkt.StreckenID21 + " Schnittpunkt: " + Punkt.SpX1 + '|' + Punkt.SpY1);
                Ellipse tempEllipse = new Ellipse();
                tempEllipse.Height = 3;
                tempEllipse.Width = 3;
                tempEllipse.Fill = System.Windows.Media.Brushes.Red;
                double left = Punkt.SpX1 - 1.5;
                double top = Punkt.SpY1 - 1.5;
                tempEllipse.Margin = new Thickness(left, top, 0, 0);
                CanvasSp.Children.Add(tempEllipse);
            }
        }

        private void miDreiecke_Click(object sender, RoutedEventArgs e)
        {
            foreach (cStrecken Strecke in meineStrecken)
            {
                Line tempLine = new Line();
                tempLine.Stroke = System.Windows.Media.Brushes.Black;
                tempLine.StrokeThickness = 3;
                tempLine.X1 = Strecke.Ep1X;
                tempLine.Y1 = Strecke.Ep1Y;
                tempLine.X2 = Strecke.Ep2X;
                tempLine.Y2 = Strecke.Ep2Y;
                //Canvas.SetZIndex(tempLine, 4);
                CanvasDr.Children.Add(tempLine);
            }

            foreach (cDreiecke Dreieck in meineDreiecke)
            {
                lbDr.Items.Add(Dreieck.AX + "/" + Dreieck.AY + " | " + Dreieck.BX + "/" + Dreieck.BY + " | " + Dreieck.CX + "/" + Dreieck.CY);

                Polygon tempPolygon = new Polygon();
                tempPolygon.StrokeThickness = 3;
                tempPolygon.Stroke = System.Windows.Media.Brushes.LightBlue;
                System.Windows.Point tempPoint1 = new System.Windows.Point(Dreieck.AX, Dreieck.AY);
                System.Windows.Point tempPoint2 = new System.Windows.Point(Dreieck.BX, Dreieck.BY);
                System.Windows.Point tempPoint3 = new System.Windows.Point(Dreieck.CX, Dreieck.CY);
                tempPolygon.Points = new PointCollection() { tempPoint1, tempPoint2, tempPoint3 };
                //Canvas.SetZIndex(tempPolygon,3);
                CanvasDr.Children.Add(tempPolygon);
            }


            foreach (cPunkte Punkt in meinePunkte)
            {
                Ellipse tempEllipse = new Ellipse();
                tempEllipse.Height = 3;
                tempEllipse.Width = 3;
                tempEllipse.Fill = System.Windows.Media.Brushes.Red;
                double left = Punkt.SpX1 - 1.5;
                double top = Punkt.SpY1 - 1.5;
                tempEllipse.Margin = new Thickness(left, top, 0, 0);
                //Canvas.SetZIndex(tempEllipse,1);
                CanvasDr.Children.Add(tempEllipse);
            }
        }

        private void lbDr_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (shown == true)
            {
                CanvasDr.Children.RemoveAt(CanvasDr.Children.Count - 1);
            }
            if (lbDr.SelectedIndex != -1)
            {
                shown = true;
                drawSingleDreieck(lbDr.SelectedIndex);
            }
        }

        private void lbSp_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (shownP == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    CanvasSp.Children.RemoveAt(CanvasSp.Children.Count - 1);
                }
            }

            if (lbSp.SelectedIndex != -1)
            {
                shownP = true;
                drawSingleSp(lbSp.SelectedIndex);
            }
        }

        private void lbCoord_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (shownS == true)
            {
                CanvasEinlesen.Children.RemoveAt(CanvasEinlesen.Children.Count - 1);
            }

            if (lbCoord.SelectedIndex != -1)
            {
                shownS = true;
                drawSingleStrecke(lbCoord.SelectedIndex);
            }
        }

        private void clearAll()
        {
            CanvasEinlesen.Children.Clear();
            CanvasSp.Children.Clear();
            CanvasDr.Children.Clear();
            meineStrecken.Clear();
            meinePunkte.Clear();
            meineDreiecke.Clear();
            lbCoord.Items.Clear();
            lbDr.Items.Clear();
            lbSp.Items.Clear();
            shown = false;
            shownP = false;
            shownS = false;
        }

        private void FindIntersection(cStrecken linie1, cStrecken linie2,
                                        out bool lines_intersect, out bool segments_intersect,
                                        out PointF intersection,
                                        out PointF close_p1, out PointF close_p2)
        {
            PointF p1 = new PointF(linie1.Ep1X, linie1.Ep1Y);
            PointF p2 = new PointF(linie1.Ep2X, linie1.Ep2Y);
            PointF p3 = new PointF(linie2.Ep1X, linie2.Ep1Y);
            PointF p4 = new PointF(linie2.Ep2X, linie2.Ep2Y);

            // Get the segments' parameters.
            float dx12 = p2.X - p1.X;
            float dy12 = p2.Y - p1.Y;
            float dx34 = p4.X - p3.X;
            float dy34 = p4.Y - p3.Y;

            // Solve for t1 and t2
            float denominator = (dy12 * dx34 - dx12 * dy34);

            float t1 =
                ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34)
                    / denominator;
            if (float.IsInfinity(t1))
            {
                // The lines are parallel (or close enough to it).
                lines_intersect = false;
                segments_intersect = false;
                intersection = new PointF(float.NaN, float.NaN);
                close_p1 = new PointF(float.NaN, float.NaN);
                close_p2 = new PointF(float.NaN, float.NaN);
                return;
            }
            lines_intersect = true;

            float t2 =
                ((p3.X - p1.X) * dy12 + (p1.Y - p3.Y) * dx12)
                    / -denominator;

            // Find the point of intersection.
            double tempX = p1.X + dx12 * t1;
            double tempY = p1.Y + dy12 * t1;

            intersection = new PointF(Convert.ToSingle(Math.Round(tempX, 2)), Convert.ToSingle(Math.Round(tempY, 2)));

            // The segments intersect if t1 and t2 are between 0 and 1.
            segments_intersect =
                ((t1 >= 0) && (t1 <= 1) &&
                 (t2 >= 0) && (t2 <= 1));

            // Find the closest points on the segments.
            if (t1 < 0)
            {
                t1 = 0;
            }
            else if (t1 > 1)
            {
                t1 = 1;
            }

            if (t2 < 0)
            {
                t2 = 0;
            }
            else if (t2 > 1)
            {
                t2 = 1;
            }

            close_p1 = new PointF(p1.X + dx12 * t1, p1.Y + dy12 * t1);
            close_p2 = new PointF(p3.X + dx34 * t2, p3.Y + dy34 * t2);
        }

        private void FindDreiecke(cPunkte punkt1, cPunkte punkt2, cPunkte punkt3,
                                    out bool dreieck)
        {
            dreieck = false;
            PointF p1 = new PointF(punkt1.SpX1, punkt1.SpY1);
            PointF p2 = new PointF(punkt2.SpX1, punkt2.SpY1);
            PointF p3 = new PointF(punkt3.SpX1, punkt3.SpY1);
            if (p1 != p2 && p1 != p3 && p2 != p3)
            {
                if (punkt1.StreckenID11 == punkt2.StreckenID11)
                {
                    if (punkt2.StreckenID21 == punkt3.StreckenID11)
                    {
                        if (punkt3.StreckenID21 == punkt1.StreckenID21)
                        {
                            dreieck = true;
                        }
                    }
                    if (punkt2.StreckenID21 == punkt3.StreckenID21)
                    {
                        if (punkt3.StreckenID11 == punkt1.StreckenID21)
                        {
                            dreieck = true;
                        }
                    }
                }
                if (punkt1.StreckenID11 == punkt2.StreckenID21)
                {
                    if (punkt2.StreckenID11 == punkt3.StreckenID11)
                    {
                        if (punkt3.StreckenID21 == punkt1.StreckenID21)
                        {
                            dreieck = true;
                        }
                    }
                    if (punkt2.StreckenID11 == punkt3.StreckenID21)
                    {
                        if (punkt3.StreckenID11 == punkt1.StreckenID21)
                        {
                            dreieck = true;
                        }
                    }
                }

                if (punkt1.StreckenID21 == punkt2.StreckenID11)
                {
                    if (punkt2.StreckenID21 == punkt3.StreckenID11)
                    {
                        if (punkt3.StreckenID21 == punkt1.StreckenID11)
                        {
                            dreieck = true;
                        }
                    }
                    if (punkt2.StreckenID21 == punkt3.StreckenID21)
                    {
                        if (punkt3.StreckenID11 == punkt1.StreckenID11)
                        {
                            dreieck = true;
                        }
                    }
                }
                if (punkt1.StreckenID21 == punkt2.StreckenID21)
                {
                    if (punkt2.StreckenID11 == punkt3.StreckenID11)
                    {
                        if (punkt3.StreckenID21 == punkt1.StreckenID11)
                        {
                            dreieck = true;
                        }
                    }
                    if (punkt2.StreckenID11 == punkt3.StreckenID21)
                    {
                        if (punkt3.StreckenID11 == punkt1.StreckenID11)
                        {
                            dreieck = true;
                        }
                    }
                }
            }
        }

        private void miClear_Click(object sender, RoutedEventArgs e)
        {
            clearAll();
        }

        private void drawSingleDreieck(int i)
        {
            Polygon polygon = new Polygon();
            polygon.StrokeThickness = 3;
            polygon.Stroke = System.Windows.Media.Brushes.Red;
            cDreiecke Dreieck = meineDreiecke[i];
            System.Windows.Point tempPoint1 = new System.Windows.Point(Dreieck.AX, Dreieck.AY);
            System.Windows.Point tempPoint2 = new System.Windows.Point(Dreieck.BX, Dreieck.BY);
            System.Windows.Point tempPoint3 = new System.Windows.Point(Dreieck.CX, Dreieck.CY);
            polygon.Points = new PointCollection() { tempPoint1, tempPoint2, tempPoint3 };
            //Canvas.SetZIndex(polygon, 2);
            CanvasDr.Children.Add(polygon);
        }

        private void drawSingleSp(int i)
        {
            cPunkte Punkt = meinePunkte[i];
            foreach (cStrecken Strecke in meineStrecken)
            {
                if (Strecke.MyId == Punkt.StreckenID11 || Strecke.MyId == Punkt.StreckenID21)
                {
                    Line tempLine = new Line();
                    tempLine.Stroke = System.Windows.Media.Brushes.LightBlue;
                    tempLine.StrokeThickness = 3;
                    tempLine.X1 = Strecke.Ep1X;
                    tempLine.Y1 = Strecke.Ep1Y;
                    tempLine.X2 = Strecke.Ep2X;
                    tempLine.Y2 = Strecke.Ep2Y;
                    CanvasSp.Children.Add(tempLine);
                }
            }

            Ellipse tempEllipse = new Ellipse();
            tempEllipse.Height = 5;
            tempEllipse.Width = 5;
            tempEllipse.Fill = System.Windows.Media.Brushes.DarkRed;
            double left = Punkt.SpX1 - 1.5;
            double top = Punkt.SpY1 - 1.5;
            tempEllipse.Margin = new Thickness(left, top, 0, 0);
            CanvasSp.Children.Add(tempEllipse);
        }

        private void drawSingleStrecke(int i)
        {
            Line tempLine = new Line();
            tempLine.Stroke = System.Windows.Media.Brushes.Red;
            tempLine.StrokeThickness = 3;
            cStrecken Strecke = meineStrecken[i];
            tempLine.X1 = Strecke.Ep1X;
            tempLine.Y1 = Strecke.Ep1Y;
            tempLine.X2 = Strecke.Ep2X;
            tempLine.Y2 = Strecke.Ep2Y;
            CanvasEinlesen.Children.Add(tempLine);
        }
    }
}

