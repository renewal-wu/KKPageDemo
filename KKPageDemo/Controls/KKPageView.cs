using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace KKPageDemo.Controls
{
    public sealed class KKPageView : Control
    {
        /// <summary>
        /// 一次最多顯示兩頁
        /// </summary>
        public readonly int ShowPageCounts = 2;

        private readonly double SeperaterWidth = 20;

        private readonly int PageAnimationDuration = 100;

        public int FrameCounts { get; set; } = 3;

        public int NavigationCache { get; set; } = 2;

        /// <summary>
        /// 最後一個人
        /// </summary>
        public KKPage PrimaryPage
        {
            get
            {
                KKPage result = null;

                if (Pages.Count >= 1)
                {
                    result = Pages.LastOrDefault();
                }

                return result;
            }
        }

        /// <summary>
        /// 倒數第二個人
        /// </summary>
        public KKPage SecondaryPage
        {
            get
            {
                KKPage result = null;

                if (Pages.Count >= 2)
                {
                    result = Pages.ElementAt(Pages.Count - 2);
                }

                return result;
            }
        }

        /// <summary>
        /// 倒數第三個人
        /// </summary>
        public KKPage ThirdPage
        {
            get
            {
                KKPage result = null;

                if (Pages.Count >= 3)
                {
                    result = Pages.ElementAt(Pages.Count - 3);
                }

                return result;
            }
        }

        public double SeperaterWidthHalf
        {
            get
            {
                return SeperaterWidth / 2;
            }
        }

        public double HalfActualWidth
        {
            get
            {
                return ActualWidth / 2;
            }
        }

        public bool IsSeperateMode
        {
            get
            {
                return this.ActualWidth > 700;
            }
        }

        public bool IsNeedShowSeperater
        {
            get
            {
                return IsSeperateMode && SecondaryPage != null;
            }
        }

        private double primaryPageWidth = 0;
        private double PrimaryPageWidth
        {
            get
            {
                if (IsSeperateMode == false || SecondaryPage == null)
                {
                    return ActualWidth;
                }

                return IsUseDefaultSeperateWidth ? HalfActualWidth - SeperaterWidthHalf : primaryPageWidth - SeperaterWidthHalf;
            }
            set
            {
                primaryPageWidth = value;
            }
        }

        private double secondaryPageWidth = 0;
        private double SecondaryPageWidth
        {
            get
            {
                if (IsSeperateMode == false)
                {
                    return ActualWidth;
                }

                return IsUseDefaultSeperateWidth ? HalfActualWidth - SeperaterWidthHalf : secondaryPageWidth - SeperaterWidthHalf;
            }
            set
            {
                secondaryPageWidth = value;
            }
        }

        private double PrimaryPageSeperatePosition
        {
            get
            {
                return IsSeperateMode && SecondaryPage != null ? SecondaryPageWidth + SeperaterWidth : 0;
            }
        }

        private double SecondaryPageSeperatePosition
        {
            get
            {
                return 0;
            }
        }

        private double ThirdPageSeperatePosition
        {
            get
            {
                return -1 * SecondaryPageWidth;
            }
        }

        private bool IsUseDefaultSeperateWidth { get; set; } = true;

        public bool CanNavigate { get; private set; } = true;

        public List<KKPage> Pages { get; private set; } = new List<KKPage>();

        private Canvas RootContainer { get; set; }

        private KKPageViewSeperater Seperater { get; set; }

        private KKPageViewSeperater SecondSeperater { get; set; }

        public KKPageView()
        {
            this.DefaultStyleKey = typeof(KKPageView);
            this.SizeChanged += KKPageView_SizeChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            RootContainer = base.GetTemplateChild("RootContainer") as Canvas;
            Seperater = GenerateSeperater();
        }

        private KKPageViewSeperater GenerateSeperater()
        {
            KKPageViewSeperater newSeperater = new KKPageViewSeperater()
            {
                Width = SeperaterWidth,
                Height = ActualHeight,
                Background = new SolidColorBrush(Colors.Black),
                Visibility = Windows.UI.Xaml.Visibility.Collapsed
            };

            newSeperater.SeperaterPositionChanged += NewSeperater_SeperaterPositionChanged;

            return newSeperater;
        }

        private KKPageViewSeperater GenerateSecondSeperater(KKNavigationMode mode)
        {
            KKPageViewSeperater newSeperater = GenerateSeperater();
            newSeperater.Visibility = Visibility;
            Canvas.SetTop(newSeperater, 0);
            Canvas.SetLeft(newSeperater, (mode == KKNavigationMode.Forward ? ActualWidth : -SeperaterWidth));
            Canvas.SetZIndex(newSeperater, 99);

            return newSeperater;
        }

        private void NewSeperater_SeperaterPositionChanged(object sender, double e)
        {
            SetPrimaryPageWidth(PrimaryPageWidth - e + SeperaterWidthHalf);
        }

        private void KKPageView_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            UpdatePageViewSize();
        }

        private void UpdatePageViewSize()
        {
            UpdateSeperateSize();
            UpdatePagesWidth();
            UpdatePagesHeight();
            UpdateSeperateVisibility();
        }

        private void UpdateSeperateSize()
        {
            // 注意，這邊是更動 "小寫" 的 field 而非 property
            if (IsSeperateMode && primaryPageWidth > 0 && secondaryPageWidth > 0)
            {
                double ratio = primaryPageWidth / (primaryPageWidth + secondaryPageWidth);

                double newPrimaryPageWidth = ActualWidth * ratio;
                double newSecondaryPageWidth = ActualWidth * (1 - ratio);

                if (newPrimaryPageWidth >= 320 && newSecondaryPageWidth >= 320)
                {
                    PrimaryPageWidth = newPrimaryPageWidth;
                    SecondaryPageWidth = newSecondaryPageWidth;
                }
                else if (newPrimaryPageWidth > newSecondaryPageWidth)
                {
                    SecondaryPageWidth = Math.Max(newSecondaryPageWidth, 320);
                    PrimaryPageWidth = ActualWidth - SeperaterWidthHalf - SecondaryPageWidth;
                }
                else
                {
                    PrimaryPageWidth = Math.Max(newPrimaryPageWidth, 320);
                    SecondaryPageWidth = ActualWidth - SeperaterWidthHalf - PrimaryPageWidth;
                }
            }
        }

        private void UpdatePagesWidth()
        {
            if (PrimaryPage != null)
            {
                Canvas.SetTop(PrimaryPage, 0);
                Canvas.SetLeft(PrimaryPage, PrimaryPageSeperatePosition);
                Canvas.SetZIndex(PrimaryPage, 2);

                PrimaryPage.Width = PrimaryPageWidth;
            }

            if (SecondaryPage != null)
            {
                Canvas.SetTop(SecondaryPage, 0);
                Canvas.SetLeft(SecondaryPage, SecondaryPageSeperatePosition);
                Canvas.SetZIndex(SecondaryPage, 1);

                SecondaryPage.Width = SecondaryPageWidth;
            }

            if (ThirdPage != null)
            {
                Canvas.SetTop(ThirdPage, 0);
                Canvas.SetLeft(ThirdPage, ThirdPageSeperatePosition);
                Canvas.SetZIndex(ThirdPage, 0);

                ThirdPage.Width = 0;
            }
        }

        private void UpdatePagesHeight()
        {
            if (PrimaryPage != null)
            {
                PrimaryPage.Height = ActualHeight;
            }

            if (SecondaryPage != null)
            {
                SecondaryPage.Height = ActualHeight;
            }

            if (Seperater != null)
            {
                Seperater.Height = ActualHeight;
            }
        }

        /// <summary>
        /// 開啟頁面
        /// </summary>
        /// <typeparam name="TKKPage">開啟哪個頁面?</typeparam>
        /// <param name="from">從哪個頁面開啟的?</param>
        /// <param name="parameter">頁面參數</param>
        /// <param name="isClearHistory">是否要清除歷史紀錄</param>
        /// <returns>true: 成功</returns>
        public async Task<bool> NavigateTo<TKKPage>(KKPage from = null, object parameter = null, bool isClearHistory = false)
            where TKKPage : KKPage, new()
        {
            bool result = false;

            if (CanNavigate)
            {
                if (isClearHistory)
                {
                    // 想要清除歷程重頭來過一次
                    ClearHistory();
                }

                if (PrimaryPage == null || SecondaryPage == null)
                {
                    result = await AddPage<TKKPage>(parameter);
                }
                else if (PrimaryPage != null && SecondaryPage != null)
                {
                    // 目前已經有 primary, Secondary
                    if (from == SecondaryPage)
                    {
                        // 此 Navigation 來自 Secondary
                        result = await UpdatePage<TKKPage>(parameter);
                    }
                    else
                    {
                        // 此 Navigation 來自 primary
                        result = await AddPage<TKKPage>(parameter);
                    }
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 清除目前 PageList
        /// </summary>
        public void ClearHistory()
        {
            RootContainer.Children.Clear();
            Pages.Clear();

            if (Seperater != null)
            {
                Seperater.SeperaterPositionChanged -= NewSeperater_SeperaterPositionChanged;
                Seperater = null;
            }

            if(SecondSeperater != null)
            {
                SecondSeperater.SeperaterPositionChanged -= NewSeperater_SeperaterPositionChanged;
                SecondSeperater = null;
            }

            IsUseDefaultSeperateWidth = true;
        }

        /// <summary>
        /// 新增頁面 (若是目前顯示頁面數已經滿了，則會跑位移動畫)
        /// </summary>
        /// <typeparam name="TKKPage"></typeparam>
        /// <param name="parameter"></param>
        /// <returns>true: 成功</returns>
        private async Task<bool> AddPage<TKKPage>(object parameter)
            where TKKPage : KKPage, new()
        {
            bool result = false;

            if (RootContainer != null)
            {
                CanNavigate = false;

                TKKPage newPage = new TKKPage();
                newPage.DataContext = parameter;
                RootContainer.Children.Add(newPage);
                Pages.Insert(Pages.Count, newPage);

                if (SecondaryPage == null)
                {
                    Canvas.SetTop(newPage, 0);
                    Canvas.SetLeft(newPage, PrimaryPageSeperatePosition);
                    Canvas.SetZIndex(newPage, 0);

                    newPage.Width = PrimaryPageWidth;
                    newPage.Height = ActualHeight;

                    result = true;
                }
                else if (SecondaryPage != null && ThirdPage == null)
                {
                    Canvas.SetTop(newPage, 0);
                    Canvas.SetLeft(newPage, PrimaryPageSeperatePosition);
                    Canvas.SetZIndex(newPage, 1);

                    newPage.Width = PrimaryPageWidth;
                    newPage.Height = ActualHeight;

                    SecondaryPage.Width = SecondaryPageWidth;
                    Canvas.SetTop(SecondaryPage, 0);
                    Canvas.SetLeft(SecondaryPage, SecondaryPageSeperatePosition);
                    Canvas.SetZIndex(SecondaryPage, 0);

                    result = true;
                }
                else if (SecondaryPage != null && ThirdPage != null)
                {
                    Canvas.SetTop(newPage, 0);
                    Canvas.SetLeft(newPage, ActualWidth);
                    Canvas.SetZIndex(newPage, 2);

                    newPage.Width = PrimaryPageWidth;
                    newPage.Height = ActualHeight;

                    Canvas.SetZIndex(SecondaryPage, 1);

                    Canvas.SetZIndex(ThirdPage, 0);

                    // 跑位移動畫

                    result = await PageViewAnimation(KKNavigationMode.Forward);
                    RootContainer.Children.Remove(ThirdPage);
                }

                UpdateSeperateVisibility();

                CanNavigate = true;
            }

            return result;
        }

        /// <summary>
        /// 更新 PrimaryPage
        /// </summary>
        /// <typeparam name="TKKPage"></typeparam>
        /// <param name="parameter"></param>
        /// <returns>true: 成功</returns>
        private async Task<bool> UpdatePage<TKKPage>(object parameter)
            where TKKPage : KKPage, new()
        {
            CanNavigate = false;
            bool result = false;

            if (RootContainer != null)
            {
                if (PrimaryPage != null)
                {
                    RootContainer.Children.Remove(PrimaryPage);
                    Pages.Remove(PrimaryPage);
                }

                result = await AddPage<TKKPage>(parameter);
            }

            CanNavigate = true;
            return result;
        }

        /// <summary>
        /// 跑 Page 位移動畫 (此動畫不包含新增/移除 Page)
        /// </summary>
        /// <param name="mode"></param>
        /// <returns>true: 成功</returns>
        private async Task<bool> PageViewAnimation(KKNavigationMode mode)
        {
            CanNavigate = false;
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            try
            {
                Storyboard storyboard = new Storyboard();
                EventHandler<object> storyboardCompleted = null;
                storyboardCompleted = (o, e) =>
                {
                    storyboard.Completed -= storyboardCompleted;
                    CanNavigate = true;
                    tcs.TrySetResult(true);
                };

                storyboard.Completed += storyboardCompleted;

                DoubleAnimation moveThirdPage = new DoubleAnimation()
                {
                    To = (mode == KKNavigationMode.Forward ? ThirdPageSeperatePosition : SecondaryPageSeperatePosition),
                    Duration = TimeSpan.FromMilliseconds(PageAnimationDuration)
                };
                Storyboard.SetTarget(moveThirdPage, ThirdPage);
                Storyboard.SetTargetProperty(moveThirdPage, "(Canvas.Left)");
                storyboard.Children.Add(moveThirdPage);

                if (mode == KKNavigationMode.Back)
                {
                    // 若是 forward ，則此頁將被捨棄，不做動畫
                    DoubleAnimation resizeThird = new DoubleAnimation()
                    {
                        To = SecondaryPageWidth,
                        Duration = TimeSpan.FromMilliseconds(PageAnimationDuration),
                        EnableDependentAnimation = true
                    };
                    Storyboard.SetTarget(resizeThird, ThirdPage);
                    Storyboard.SetTargetProperty(resizeThird, "Width");
                    storyboard.Children.Add(resizeThird);
                }

                DoubleAnimation moveSecondary = new DoubleAnimation()
                {
                    To = (mode == KKNavigationMode.Forward ? SecondaryPageSeperatePosition : PrimaryPageSeperatePosition),
                    Duration = TimeSpan.FromMilliseconds(PageAnimationDuration)
                };
                Storyboard.SetTarget(moveSecondary, SecondaryPage);
                Storyboard.SetTargetProperty(moveSecondary, "(Canvas.Left)");
                storyboard.Children.Add(moveSecondary);

                DoubleAnimation resizeSecondary = new DoubleAnimation()
                {
                    To = (mode == KKNavigationMode.Forward ? SecondaryPageWidth : PrimaryPageWidth),
                    Duration = TimeSpan.FromMilliseconds(PageAnimationDuration),
                    EnableDependentAnimation = true
                };
                Storyboard.SetTarget(resizeSecondary, SecondaryPage);
                Storyboard.SetTargetProperty(resizeSecondary, "Width");
                storyboard.Children.Add(resizeSecondary);

                DoubleAnimation movePrimary = new DoubleAnimation()
                {
                    To = (mode == KKNavigationMode.Forward ? PrimaryPageSeperatePosition : ActualWidth),
                    Duration = TimeSpan.FromMilliseconds(PageAnimationDuration)
                };
                Storyboard.SetTarget(movePrimary, PrimaryPage);
                Storyboard.SetTargetProperty(movePrimary, "(Canvas.Left)");
                storyboard.Children.Add(movePrimary);

                if (mode == KKNavigationMode.Forward)
                {
                    // 若是 back，則此頁將被捨棄，不做尺寸調整
                    DoubleAnimation resizePrimary = new DoubleAnimation()
                    {
                        To = PrimaryPageWidth,
                        Duration = TimeSpan.FromMilliseconds(PageAnimationDuration),
                        EnableDependentAnimation = true
                    };
                    Storyboard.SetTarget(resizePrimary, PrimaryPage);
                    Storyboard.SetTargetProperty(resizePrimary, "Width");
                    storyboard.Children.Add(resizePrimary);
                }

                if (IsNeedShowSeperater && Seperater != null && Seperater.Visibility == Windows.UI.Xaml.Visibility.Visible && RootContainer.Children.Contains(Seperater))
                {
                    DoubleAnimation moveSeperater = new DoubleAnimation()
                    {
                        To = (mode == KKNavigationMode.Forward ? -SeperaterWidth : ActualWidth),
                        Duration = TimeSpan.FromMilliseconds(PageAnimationDuration)
                    };
                    Storyboard.SetTarget(moveSeperater, Seperater);
                    Storyboard.SetTargetProperty(moveSeperater, "(Canvas.Left)");
                    storyboard.Children.Add(moveSeperater);

                    SecondSeperater = GenerateSecondSeperater(mode);
                    RootContainer.Children.Add(SecondSeperater);

                    DoubleAnimation moveSecondSeperater = new DoubleAnimation()
                    {
                        To = SecondaryPageWidth,
                        Duration = TimeSpan.FromMilliseconds(PageAnimationDuration)
                    };
                    Storyboard.SetTarget(moveSecondSeperater, SecondSeperater);
                    Storyboard.SetTargetProperty(moveSecondSeperater, "(Canvas.Left)");
                    storyboard.Children.Add(moveSecondSeperater);
                }

                storyboard.Begin();
            }
            catch (Exception)
            {
                CanNavigate = true;
                tcs.TrySetResult(false);
            }

            return await tcs.Task;
        }

        /// <summary>
        /// 回到上一頁
        /// </summary>
        /// <returns>true: 成功</returns>
        public async Task<bool> GoBack()
        {
            bool result = false;

            if (CanNavigate && PrimaryPage != null && SecondaryPage != null)
            {
                CanNavigate = false;

                if (ThirdPage == null)
                {
                    RootContainer.Children.Remove(PrimaryPage);
                    Pages.Remove(PrimaryPage);

                    Canvas.SetTop(PrimaryPage, 0);
                    Canvas.SetLeft(PrimaryPage, 0);
                    Canvas.SetZIndex(PrimaryPage, 0);

                    PrimaryPage.Width = PrimaryPageWidth;
                    PrimaryPage.Height = ActualHeight;

                    UpdateSeperateVisibility();

                    result = true;
                }
                else
                {
                    if (RootContainer.Children.Contains(ThirdPage) == false)
                    {
                        RootContainer.Children.Add(ThirdPage);
                    }

                    await PageViewAnimation(KKNavigationMode.Back);

                    RootContainer.Children.Remove(PrimaryPage);
                    Pages.Remove(PrimaryPage);
                    PrimaryPage.DataContext = null;
                    UpdateSeperateVisibility();
                    CanNavigate = true;

                    result = true;
                }
            }

            return result;
        }

        public bool SetPrimaryPageWidth(double targetWidth)
        {
            bool result = false;
            double diffWidth = ActualWidth - targetWidth;

            if (targetWidth >= (320 + SeperaterWidthHalf) && diffWidth >= (320 + SeperaterWidthHalf))
            {
                // 主畫面和副畫面都要大於 320
                IsUseDefaultSeperateWidth = false;
                PrimaryPageWidth = targetWidth;
                SecondaryPageWidth = diffWidth;
                UpdatePageViewSize();
            }

            return result;
        }

        private void UpdateSeperateVisibility()
        {
            if (Seperater != null && RootContainer != null)
            {
                if (SecondSeperater != null)
                {
                    Seperater.SeperaterPositionChanged -= NewSeperater_SeperaterPositionChanged;
                    RootContainer.Children.Remove(Seperater);
                    Seperater = SecondSeperater;
                }

                bool isSeperateContainsInRootContainer = RootContainer.Children.Contains(Seperater);
                bool needShowSeperater = IsNeedShowSeperater;
                Seperater.Visibility = needShowSeperater ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;

                if (needShowSeperater)
                {
                    Canvas.SetTop(Seperater, 0);
                    Canvas.SetLeft(Seperater, SecondaryPageWidth);
                    Canvas.SetZIndex(Seperater, 99);

                    Seperater.TranslateXLeftLimit = 320 - SecondaryPageWidth;
                    Seperater.TranslateXRightLimit = ActualWidth - SecondaryPageWidth - SeperaterWidth - 320;
                }

                if (needShowSeperater && isSeperateContainsInRootContainer == false)
                {
                    // 還沒加進視覺樹且應該要加進去
                    RootContainer.Children.Add(Seperater);
                }
                else if (needShowSeperater == false && isSeperateContainsInRootContainer)
                {
                    // 已經被加到視覺樹且應該要移除
                    Seperater.SeperaterPositionChanged -= NewSeperater_SeperaterPositionChanged;
                    RootContainer.Children.Remove(Seperater);
                }
            }
        }
    }
}
