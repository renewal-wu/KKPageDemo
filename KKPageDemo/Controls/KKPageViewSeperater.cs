using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace KKPageDemo.Controls
{
    public class KKPageViewSeperater : Control
    {
        public double TranslateXLeftLimit { get; set; } = 0;

        public double TranslateXRightLimit { get; set; } = 0;

        /// <summary>
        /// 回傳位移值
        /// </summary>
        public event EventHandler<double> SeperaterPositionChanged;

        private TranslateTransform RootContainerTranslateTransform { get; set; }

        public KKPageViewSeperater()
        {
            this.DefaultStyleKey = typeof(KKPageViewSeperater);
            this.ManipulationMode = ManipulationModes.TranslateX;
            this.ManipulationDelta += KKPageViewSeperater_ManipulationDelta;
            this.ManipulationCompleted += KKPageViewSeperater_ManipulationCompleted;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            RootContainerTranslateTransform = base.GetTemplateChild("RootContainerTranslateTransform") as TranslateTransform;
        }

        private void KKPageViewSeperater_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            if (RootContainerTranslateTransform != null)
            {
                double movedPosition = RootContainerTranslateTransform.X + e.Delta.Translation.X;
                if (movedPosition >= TranslateXLeftLimit && movedPosition <= TranslateXRightLimit)
                {
                    RootContainerTranslateTransform.X = movedPosition;
                }
            }
        }

        private void KKPageViewSeperater_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            if (RootContainerTranslateTransform != null && SeperaterPositionChanged != null)
            {
                SeperaterPositionChanged(this, RootContainerTranslateTransform.X);
                RootContainerTranslateTransform.X = 0;
            }
        }
    }
}
