using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Picharm.Controls
{
    public sealed class CommentRichTextBlock : Control
    {
        private static BitmapImage _guildMgrImage = null;
        private static BitmapImage _guardImage = null;
        private static SolidColorBrush _badgeForeground = null;
        private static Thickness _lowLevelBadgeMargin = new Thickness(0, 2, 10, -2);
        private static Thickness _highLevelBadgeMargin = new Thickness(0, 0, 16, 0);
        private static SolidColorBrush _myselfForeground = null;
        private static SolidColorBrush _nameForeground = new SolidColorBrush(Color.FromArgb(255, 60, 156, 254));

        private RichTextBlock _richTextBlock;

        public CommentRichTextBlock()
        {
            DefaultStyleKey = typeof(CommentRichTextBlock);
        }

        protected override void OnApplyTemplate()
        {
            _richTextBlock = GetTemplateChild("RichTextBlock") as RichTextBlock;
            // GenerateRichText();
            base.OnApplyTemplate();
        }

        //public PublicMessage Message
        //{
        //    get { return (PublicMessage)GetValue(MessageProperty); }
        //    set { SetValue(MessageProperty, value); }
        //}
        //public static readonly DependencyProperty MessageProperty =
        //    DependencyProperty.Register("Message", typeof(PublicMessage), typeof(CommentRichTextBlock), new PropertyMetadata(null, new PropertyChangedCallback(OnMessageChanged)));
        //private static void OnMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (e.NewValue != null && d is CommentRichTextBlock ertb)
        //    {
        //        ertb.GenerateRichText();
        //    }
        //}

        //private void GenerateRichText()
        //{
        //    try
        //    {
        //        if (_richTextBlock != null)
        //        {
        //            _richTextBlock.Blocks.Clear();
        //            Paragraph para = BuildContents(Message);
        //            if (para != null)
        //            {
        //                _richTextBlock.Blocks.Add(para);
        //            }
        //        }
        //    }
        //    catch (Exception ex) { Trace.WriteLine(ex.Message); }
        //}

        //private Paragraph BuildContents(PublicMessage message)
        //{
        //    try
        //    {
        //        if (message == null) return null;

        //        var para = new Paragraph();

        //        // 房管标识
        //        if (!message.bGuildMgr && message.iRoomMgrType > 0)
        //        {
        //            BitmapImage roomMgrImage = RoomMgrTypeToImageConverter.GetRoomMgrImageByType(message.iRoomMgrType);
        //            if (roomMgrImage != null)
        //            {
        //                var uiContainer = new InlineUIContainer();
        //                var img = new Windows.UI.Xaml.Controls.Image()
        //                {
        //                    Width = 20,
        //                    Stretch = Windows.UI.Xaml.Media.Stretch.None,
        //                    VerticalAlignment = VerticalAlignment.Center,
        //                    Margin = new Thickness(0, 0, 4, -4)
        //                };
        //                img.Source = roomMgrImage;
        //                uiContainer.Child = img;
        //                para.Inlines.Add(uiContainer);
        //            }
        //        }

        //        // 公会管理，若同时拥有公会管理和房管身份，则只显示公会管理不显示房管
        //        if (message.bGuildMgr)
        //        {
        //            if (_guildMgrImage == null) _guildMgrImage = new BitmapImage(new Uri("ms-appx:///Assets/Icons/Chat/icon_guildMgr.png"));
        //            if (_guildMgrImage != null)
        //            {
        //                var uiContainer = new InlineUIContainer();
        //                var img = new Windows.UI.Xaml.Controls.Image()
        //                {
        //                    Width = 20,
        //                    Stretch = Windows.UI.Xaml.Media.Stretch.None,
        //                    VerticalAlignment = VerticalAlignment.Center,
        //                    Margin = new Thickness(0, 0, 4, -4)
        //                };
        //                img.Source = _guildMgrImage;
        //                uiContainer.Child = img;
        //                para.Inlines.Add(uiContainer);
        //            }
        //        }

        //        // 守护
        //        if (message.bGuard)
        //        {
        //            if (_guardImage == null) _guardImage = new BitmapImage(new Uri("ms-appx:///Assets/Icons/Chat/icon_guard.png"));
        //            if (_guardImage != null)
        //            {
        //                var uiContainer = new InlineUIContainer();
        //                var img = new Windows.UI.Xaml.Controls.Image()
        //                {
        //                    Width = 20,
        //                    Stretch = Windows.UI.Xaml.Media.Stretch.None,
        //                    VerticalAlignment = VerticalAlignment.Center,
        //                    Margin = new Thickness(0, 0, 4, -4)
        //                };
        //                img.Source = _guardImage;
        //                uiContainer.Child = img;
        //                para.Inlines.Add(uiContainer);
        //            }
        //        }

        //        // 公会贵宾
        //        if (message.bGuildVip)
        //        {
        //            var uiContainer = new InlineUIContainer();
        //            var img = new Windows.UI.Xaml.Controls.Image()
        //            {
        //                Width = 20,
        //                Stretch = Windows.UI.Xaml.Media.Stretch.UniformToFill,
        //                VerticalAlignment = VerticalAlignment.Center,
        //                Margin = new Thickness(0, 0, 4, -4)
        //            };
        //            img.Source = _defaultGuildVipImageSource;
        //            message.LoadGuildVipImageAsync(img);
        //            uiContainer.Child = img;
        //            para.Inlines.Add(uiContainer);
        //        }

        //        // 粉丝
        //        if (message.bFans)
        //        {
        //            var grid = new Grid()
        //            {
        //                MaxHeight = 28,
        //                Margin = new Thickness(0, 0, 4, -4)
        //            };
        //            var img = new Windows.UI.Xaml.Controls.Image()
        //            {
        //                Height = 20,
        //                Stretch = Windows.UI.Xaml.Media.Stretch.None,
        //                VerticalAlignment = VerticalAlignment.Center
        //            };
        //            img.Source = BadgeService.Instance.GetFansBadges(message.tFans.iBadgeLevel);
        //            grid.Children.Add(img);

        //            if (_badgeForeground == null) _badgeForeground = new SolidColorBrush(Windows.UI.Colors.White);
        //            var tb = new TextBlock()
        //            {
        //                Foreground = _badgeForeground,
        //                FontSize = 12,
        //                HorizontalAlignment = HorizontalAlignment.Right,
        //                VerticalAlignment = VerticalAlignment.Center,
        //                Text = message.tFans.sBadgeName,
        //                Margin = (message.tFans.iBadgeLevel < 41) ? _lowLevelBadgeMargin : _highLevelBadgeMargin
        //            };
        //            grid.Children.Add(tb);

        //            var uiContainer = new InlineUIContainer();
        //            uiContainer.Child = grid;
        //            para.Inlines.Add(uiContainer);
        //        }

        //        // 超粉标志
        //        if (message.bFans)
        //        {
        //            BitmapImage superFansImage = SuperFansFlagToImageConverter.GetSuperFansImageByFlag(message.tFans.tSuperFansInfo.iSFFlag);
        //            if (superFansImage != null)
        //            {
        //                var uiContainer = new InlineUIContainer();
        //                var img = new Windows.UI.Xaml.Controls.Image()
        //                {
        //                    Width = 20,
        //                    Stretch = Windows.UI.Xaml.Media.Stretch.None,
        //                    VerticalAlignment = VerticalAlignment.Center,
        //                    Margin = new Thickness(-14, 0, 4, -4)
        //                };
        //                img.Source = superFansImage;
        //                uiContainer.Child = img;
        //                para.Inlines.Add(uiContainer);
        //            }
        //        }

        //        // 本人发言标志
        //        if (message.bMe)
        //        {
        //            if (_myselfForeground == null) _myselfForeground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 169, 0));
        //            var tb = new TextBlock()
        //            {
        //                Foreground = _myselfForeground,
        //                FontSize = 14,
        //                HorizontalAlignment = HorizontalAlignment.Center,
        //                VerticalAlignment = VerticalAlignment.Center,
        //                Text = "[我]",
        //                Margin = new Thickness(4, 0, 4, 0)
        //            };
        //            var uiContainer = new InlineUIContainer();
        //            uiContainer.Child = tb;
        //            para.Inlines.Add(uiContainer);
        //        }

        //        // 昵称
        //        if (!string.IsNullOrEmpty(message.sNick))
        //        {
        //            para.Inlines.Add(new Run { Text = message.sNick, CharacterSpacing = 40, Foreground = _nameForeground });
        //        }

        //        // 钻粉
        //        if (message.bVFlag)
        //        {
        //            var uiContainer = new InlineUIContainer();
        //            var img = new Windows.UI.Xaml.Controls.Image()
        //            {
        //                Width = 20,
        //                Stretch = Windows.UI.Xaml.Media.Stretch.UniformToFill,
        //                VerticalAlignment = VerticalAlignment.Center,
        //                Margin = new Thickness(2, 0, 4, -4)
        //            };
        //            img.Source = _defaultDiamondImageSource;
        //            message.LoadVLogoImageAsync(img);
        //            uiContainer.Child = img;
        //            para.Inlines.Add(uiContainer);
        //        }

        //        // 冒号
        //        if (!string.IsNullOrEmpty(message.sNick))
        //        {
        //            para.Inlines.Add(new Run { Text = "：", CharacterSpacing = 40 });
        //        }

        //        // 发言内容
        //        if (message.vContent != null && message.vContent.Count > 0)
        //        {
        //            foreach (var item in message.vContent)
        //            {
        //                if (item.bIsText)
        //                {
        //                    para.Inlines.Add(new Run { Text = item.sText, CharacterSpacing = 40 });
        //                }
        //                else
        //                {
        //                    string emojiFileName = LiveRoomViewModel.Instance.GetEmoji(item.sEmoji);
        //                    if (!string.IsNullOrWhiteSpace(emojiFileName))
        //                    {
        //                        try
        //                        {
        //                            var uiContainer = new InlineUIContainer();
        //                            var img = new Windows.UI.Xaml.Controls.Image()
        //                            {
        //                                Width = 20,
        //                                Stretch = Windows.UI.Xaml.Media.Stretch.Uniform,
        //                                VerticalAlignment = VerticalAlignment.Center,
        //                                Margin = new Thickness(2, 0, 2, -4)
        //                            };
        //                            var bitmap = EmojiToImageConverter.GetImageByEmojiFileName(emojiFileName);
        //                            img.Source = bitmap;
        //                            uiContainer.Child = img;
        //                            para.Inlines.Add(uiContainer);
        //                        }
        //                        catch (Exception e1)
        //                        {
        //                            Debug.WriteLine("BuildContents GetEmoji Error: " + e1.Message);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        para.Inlines.Add(new Run { Text = item.sText, CharacterSpacing = 40 });
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            para.Inlines.Add(new Run { Text = message.sOriginalContent, CharacterSpacing = 40 });
        //        }

        //        return para;
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("BuildContents Error: " + e.Message);
        //    }
        //    return null;
        //}
    }

}
