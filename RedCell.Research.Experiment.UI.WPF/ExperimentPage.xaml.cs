﻿using System;
using System.Collections.Generic;
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

namespace RedCell.Research.Experiment.UI
{
    /// <summary>
    /// Interaction logic for ExperimentPage.xaml
    /// </summary>
    public partial class ExperimentPage : Page, IExperimentUI
    {
        public ExperimentPage()
        {
            InitializeComponent();
        }

        public void MessageBox(string message)
        {
            System.Windows.MessageBox.Show(message);
        }

        public IRegion AddRegion(float x, float y, float w, float h)
        {
            var r = new Region(x, y, w, h);
            Canvas.Children.Add(r);
            return r;
        }

        public void AddCameraFaceAnnotation(ICameraView view)
        {
            var c = new CameraFaceAnnotation(view);
            Canvas.Children.Add(c);
        }

        public void Wait(double seconds)
        {
            UISleepHelper.Wait(seconds);
        }
    }
}