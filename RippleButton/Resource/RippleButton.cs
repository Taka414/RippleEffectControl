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
    /// リップル効果の付いたボタン
    /// </summary>
    public class RippleButton : Button
    {
        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        public Brush RippleColor
        {
            get { return (Brush)GetValue(RippleColorProperty); }
            set { SetValue(RippleColorProperty, value); }
        }

        //
        // Dependency props
        // - - - - - - - - - - - - - - - - - - - -

        public static readonly DependencyProperty RippleColorProperty =
            DependencyProperty.Register("RippleColor", typeof(Brush), typeof(RippleButton), new PropertyMetadata(Brushes.White));

        public RippleButton()
        {
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.AddHandler(MouseDownEvent, new RoutedEventHandler(this.OnMouseDown));
        }

        public void OnMouseDown(object sender, RoutedEventArgs e)
        {
            // クリック位置からRippleの中心を取る
            Point mousePos = (e as MouseButtonEventArgs).GetPosition(this);

            var ellipse = this.GetTemplateChild("CircleEffect") as Ellipse;

            ellipse.Margin = new Thickness(mousePos.X, mousePos.Y, 0, 0);

            // アニメーションの動作の指定
            Storyboard storyboard = (this.FindResource("RippleAnimation") as Storyboard).Clone();

            // 円の最大の大きさ -> コントロールの大きさの倍
            double effectMaxSize = Math.Max(this.ActualWidth, this.ActualHeight) * 3;

            (storyboard.Children[2] as ThicknessAnimation).From = new Thickness(mousePos.X, mousePos.Y, 0, 0);
            (storyboard.Children[2] as ThicknessAnimation).To = new Thickness(mousePos.X - effectMaxSize / 2, mousePos.Y - effectMaxSize / 2, 0, 0);
            (storyboard.Children[3] as DoubleAnimation).To = effectMaxSize;

            ellipse.BeginStoryboard(storyboard);
        }
    }
}
