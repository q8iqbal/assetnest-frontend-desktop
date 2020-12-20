using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Velacro.UIElements.Basic;
using Velacro.UIElements.TextBox;

using assetnest_wpf.Model;
using assetnest_wpf.Utils;
using assetnest_wpf.View.ListUser;

namespace assetnest_wpf.View.Staff
{
    /// <summary>
    /// Interaction logic for StaffPage.xaml
    /// </summary>
    public partial class StaffPage : MyPage
    {
        private int staffId;
        private string staffImage;
        private BuilderTextBox builderTextBox;
        private ComboBox roleComboBox;
        private Image staffImageTooltipImage;
        private ImageBrush staffImageImageBrush;
        private Label staffNameLabel;
        private Label staffRoleLabel;
        private ProgressBar loadingProgressBar;
        private StackPanel editStaffButtonsStackPanel;
        private StackPanel showStaffButtonsStackPanel;
        private TextBox fullNameTextBox;
        private TextBox roleTextBox;
        private IMyTextBox emailTextBox;

        public StaffPage(int id)
        {
            InitializeComponent();
            staffId = -1;
            setController(new StaffController(this));
            initUIBuilders();
            initUIElements();
            getController().callMethod("getStaff", id);
        }

        private void initUIBuilders()
        {
            builderTextBox = new BuilderTextBox();
        }

        private void initUIElements()
        {
            staffNameLabel = this.FindName("staffname_label") as Label;
            staffRoleLabel = this.FindName("staffrole_label") as Label;
            roleComboBox = this.FindName("role_combobox") as ComboBox;
            staffImageImageBrush = this.FindName("staffimage_imagebrush") as ImageBrush;
            staffImageTooltipImage = this.FindName("staffimage_tooltip_image") as Image;
            loadingProgressBar = this.FindName("loading_progressbar") as ProgressBar;
            editStaffButtonsStackPanel 
                = this.FindName("editstaffbuttons_stackpanel") as StackPanel;
            showStaffButtonsStackPanel 
                = this.FindName("showstaffbuttons_stackpanel") as StackPanel;
            fullNameTextBox = this.FindName("fullname_textbox") as TextBox;
            roleTextBox = this.FindName("role_textbox") as TextBox;
            emailTextBox = builderTextBox.activate(this, "email_textbox");
        }

        public void initStaff(User staff)
        {
            this.Dispatcher.Invoke(() =>
            {
                string role = char.ToUpper(staff.role[0]) + staff.role.Substring(1);

                staffId = staff.id;
                staffImage = staff.image;
                staffNameLabel.Content = staff.name;
                staffRoleLabel.Content = role;
                fullNameTextBox.Text = staff.name;
                roleTextBox.Text = role;
                roleComboBox.SelectedValue = role;
                emailTextBox.setText(staff.email);
                if (staff.image != null)
                {
                    Uri imageUri = new Uri(Constants.BASE_URL + staff.image);

                    staffImageImageBrush.ImageSource = new BitmapImage(imageUri);
                    staffImageTooltipImage.Source = new BitmapImage(imageUri);
                }
            });
        }

        public void changeToEditStaffPage()
        {
            this.Dispatcher.Invoke(() =>
            {
                editStaffButtonsStackPanel.Visibility = Visibility.Visible;
                showStaffButtonsStackPanel.Visibility = Visibility.Collapsed;
                fullNameTextBox.IsReadOnly = false;
                fullNameTextBox.Focusable = true;
                fullNameTextBox.BorderThickness = new Thickness(0, 0, 0, 1);
                roleComboBox.Visibility = Visibility.Visible;
                roleTextBox.Visibility = Visibility.Collapsed;
            });
        }

        public void changeToShowStaffPage()
        {
            this.Dispatcher.Invoke(() =>
            {
                fullNameTextBox.Text = staffNameLabel.Content.ToString();
                roleComboBox.SelectedValue = staffRoleLabel.Content.ToString();
                editStaffButtonsStackPanel.Visibility = Visibility.Collapsed;
                showStaffButtonsStackPanel.Visibility = Visibility.Visible;
                fullNameTextBox.IsReadOnly = true;
                fullNameTextBox.Focusable = false;
                fullNameTextBox.BorderThickness = new Thickness(0, 0, 0, 0);
                roleComboBox.Visibility = Visibility.Collapsed;
                roleTextBox.Visibility = Visibility.Visible;
            });
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (staffId == -1)
            {
                return;
            }

            changeToEditStaffPage();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            string confirmationMessage = "You're about to delete staff: " 
                + fullNameTextBox.Text + " (" + roleTextBox.Text + "). Proceed delete staff?";
            
            if (staffId == -1)
            {
                return;
            }

            switch (showConfirmationMessage(confirmationMessage))
            {
                case MessageBoxResult.OK:
                    getController().callMethod("deleteStaff", staffId);
                    break;
                default:
                    break;
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            changeToShowStaffPage();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = fullNameTextBox.Text;
            string email = emailTextBox.getText();
            string role = roleComboBox.SelectedValue?.ToString();

            if (name.Equals("") || role == null || role.Equals(""))
            {
                showErrorMessage("All fillable fields are required.");

                return;
            }

            role = Char.ToLower(role[0]) + role.Substring(1);
            switch (showConfirmationMessage("Proceed update staff?"))
            {
                case MessageBoxResult.OK:
                    getController()
                        .callMethod("updateStaff", staffId, name, email, role, staffImage);
                    break;
                default:
                    break;
            }
        }

        public void navigateToStaffListPage()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.NavigationService.Navigate(new ListUserPage(""));
            });
        }

        public void showErrorMessage(string message)
        {
            showMessage(message, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void showSuccessMessage(string message)
        {
            showMessage(message, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public MessageBoxResult showConfirmationMessage(string message)
        {
            return showMessage(message, MessageBoxButton.OKCancel, MessageBoxImage.Question);
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
