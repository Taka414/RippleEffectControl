using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CustomControls
{
    /// <summary>
    /// Represents a button with a ripple effect.
    /// </summary>
    public class RippleButton : Button
    {
        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        public Brush RippleColor
        {
            get => (Brush)GetValue(RippleColorProperty);
            set => SetValue(RippleColorProperty, value);
        }

        //
        // Dependency props
        // - - - - - - - - - - - - - - - - - - - -

        public static readonly DependencyProperty RippleColorProperty =
            DependencyProperty.Register("RippleColor", typeof(Brush),
                typeof(RippleButton), new PropertyMetadata(Brushes.White));

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // React to all without specifying the use here
            this.AddHandler(MouseDownEvent, new RoutedEventHandler(this.OnMouseDown), true);
        }

        public void OnMouseDown(object sender, RoutedEventArgs e)
        {
            // Take the center of the Ripple from the click position
            Point mousePos = (e as MouseButtonEventArgs).GetPosition(this);

            var ellipse = GetTemplateChild("CircleEffect") as Ellipse;

            ellipse.Margin = new Thickness(mousePos.X, mousePos.Y, 0, 0);

            // Specify animation behavior
            Storyboard storyboard = (FindResource("RippleAnimation") as Storyboard).Clone();

            // Maximum size of the circle -> Double the size of the control
            double effectMaxSize = Math.Max(ActualWidth, ActualHeight) * 3;

            (storyboard.Children[2] as ThicknessAnimation).From = new Thickness(mousePos.X, mousePos.Y, 0, 0);
            (storyboard.Children[2] as ThicknessAnimation).To = new Thickness(mousePos.X - effectMaxSize / 2, mousePos.Y - effectMaxSize / 2, 0, 0);
            (storyboard.Children[3] as DoubleAnimation).To = effectMaxSize;

            ellipse.BeginStoryboard(storyboard);
        }
    }
}