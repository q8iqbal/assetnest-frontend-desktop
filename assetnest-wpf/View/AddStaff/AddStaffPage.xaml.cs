using System.Windows;
using System.Windows.Controls;

using Velacro.UIElements.Basic;
using Velacro.UIElements.Button;
using Velacro.UIElements.TextBlock;
using Velacro.UIElements.TextBox;

using assetnest_wpf.View.ListUser;
using assetnest_wpf.View.Staff;

namespace assetnest_wpf.View.AddStaff
{
    /// <summary>
    /// Interaction logic for AddStaffPage.xaml
    /// </summary>
    public partial class AddStaffPage : MyPage
    {
        public AddStaffPage()
        {
            InitializeComponent();
            this.KeepAlive = true;
            setController(new AddStaffController(this));
            initUIBuilders();
            initUIElements();
        }

        private BuilderButton buttonBuilder;
        private BuilderTextBox txtBoxBuilder;
        private ComboBox comboBox;
        private BuilderTextBlock txtBlockBuilder;
        private ProgressBar loadingProgressBar;

        private void initUIBuilders()
        {
            buttonBuilder = new BuilderButton();
            txtBoxBuilder = new BuilderTextBox();
            txtBlockBuilder = new BuilderTextBlock();
        }

        private IMyButton saveButton;
        private IMyButton cancelButton;
        private IMyTextBox emailTxtBox;
        private IMyTextBox nameTxtBox;

        private void initUIElements()
        {
            saveButton = buttonBuilder.activate(this, "save_btn")
                .addOnClick(this, "onSaveButtonClick");
            cancelButton = buttonBuilder.activate(this, "cancel_btn")
                .addOnClick(this, "onCancelButtonClick");
            nameTxtBox = txtBoxBuilder.activate(this, "name_txt");
            emailTxtBox = txtBoxBuilder.activate(this, "email_txt");
            comboBox = this.FindName("role_cb") as ComboBox;
            loadingProgressBar = this.FindName("loading_progressbar") as ProgressBar;
        }

        public void onSaveButtonClick()
        {
            string name = nameTxtBox.getText();
            string email = emailTxtBox.getText();
            string role = comboBox.SelectedValue.ToString();

            if (name.Equals("") || email.Equals("") || role.Equals(""))
            {
                showErrorMessage("All fillable fields are required");
                return;
            }

            getController().callMethod("save", name, email, role);
        }

        public void onCancelButtonClick()
        {
            navigateToListUserPage();
        }

        public void setAddUserStatus(string _status)
        {
            this.Dispatcher.Invoke(() => 
            {
                if (_status.Equals("OK"))
                {
                    showSuccessMessage("Staff added successfully.");
                }
                else
                {
                    showErrorMessage("Failed to add new staff. " + _status);
                }
            });

        }

        public void navigateToStaffPage(int staffId)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.NavigationService.Navigate(new StaffPage(staffId));
            });
        }

        public void navigateToListUserPage()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.NavigationService.Navigate(new ListUserPage(""));
            });
        }

        public MessageBoxResult showErrorMessage(string message)
        {
            return showMessage(message, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public MessageBoxResult showSuccessMessage(string message)
        {
            return showMessage(message, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private MessageBoxResult showMessage(string message, MessageBoxButton buttons,
                                             MessageBoxImage icon)
        {
            MessageBoxResult messageResult = MessageBoxResult.OK;

            this.Dispatcher.Invoke(() =>
            {
                string title = Application.Current.MainWindow.Title;

                messageResult = MessageBox.Show(message, title, buttons, icon);
            });

            return messageResult;
        }

        public void startLoading()
        {
            this.Dispatcher.Invoke(() =>
            {
                loadingProgressBar.Visibility = Visibility.Visible;
            });
        }

        public void endLoading()
        {
            this.Dispatcher.Invoke(() =>
            {
                loadingProgressBar.Visibility = Visibility.Hidden;
            });
        }
    }
}
