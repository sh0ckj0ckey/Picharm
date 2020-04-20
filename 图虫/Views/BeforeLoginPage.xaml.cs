using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace 图虫
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BeforeLoginPage : Page
    {
        public BeforeLoginPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 点击登录，将跳转到登录网页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginHelper.SetAccount(AccountTextBox.Text.Trim());
            LoginHelper.SetPassword(PasswordPasswordBox.Password.Trim());
            LoginHelper.HaveInfo = false;
            this.Frame.Navigate(typeof(LoginPage));
        }

        private void AccountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AccountTextBox.Text.Trim().Length <= 0 || PasswordPasswordBox.Password.Trim().Length <= 0)
            {
                LoginButton.IsEnabled = false;
            }
            else
            {
                LoginButton.IsEnabled = true;
            }
        }

        private void PasswordPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (AccountTextBox.Text.Trim().Length <= 0 || PasswordPasswordBox.Password.Trim().Length <= 0)
            {
                LoginButton.IsEnabled = false;
            }
            else
            {
                LoginButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AccountTextBox.Focus(FocusState.Keyboard);
        }
    }
}
