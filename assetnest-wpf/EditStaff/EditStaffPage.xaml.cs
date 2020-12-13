using System;
using System.Collections.Generic;
using System.Diagnostics;
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

using Velacro.UIElements.Basic;
using Velacro.UIElements.Button;
using Velacro.UIElements.TextBox;

using assetnest_wpf.Model;
using assetnest_wpf.Utils;

namespace assetnest_wpf.EditStaff
{
    /// <summary>
    /// Interaction logic for EditStaffPage.xaml
    /// </summary>
    public partial class EditStaffPage : MyPage
    {
        private int staffId;
        private string staffImage;
        private BuilderButton builderButton;
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
        private IMyButton editButton;
        private IMyButton deleteButton;
        private IMyButton cancelButton;
        private IMyButton saveButton;
        private IMyTextBox emailTextBox;

        public EditStaffPage(int id)
        {
            InitializeComponent();
            setController(new EditStaffController(this));
            initUIBuilders();
            initUIElements();
            StorageUtil.Instance.token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hcGkuYXNzZXRuZXN0Lm1lXC9sb2dpblwvbW9iaWxlIiwiaWF0IjoxNjA3NzU2MjcyLCJuYmYiOjE2MDc3NTYyNzIsImp0aSI6IklGa3FOeTBoSU1FbFhtblAiLCJzdWIiOjEsInBydiI6IjIzYmQ1Yzg5NDlmNjAwYWRiMzllNzAxYzQwMDg3MmRiN2E1OTc2ZjcifQ.3JnBBj60Q_iz7GnGVM5TT9CawopKAdSwgN-rU3UVjHo";
            staffId = id;
            getController().callMethod("getStaff", id);
        }

        private void initUIBuilders()
        {
            builderButton = new BuilderButton();
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
            editButton = builderButton.activate(this, "edit_button")
                .addOnClick(this, "editButton_Click");
            deleteButton = builderButton.activate(this, "delete_button")
                .addOnClick(this, "deleteButton_Click");
            saveButton = builderButton.activate(this, "save_button")
                .addOnClick(this, "saveButton_Click");
            cancelButton = builderButton.activate(this, "cancel_button")
                .addOnClick(this, "cancelButton_Click");
        }

        public void initStaff(User staff)
        {
            this.Dispatcher.Invoke(() =>
            {
                string role = char.ToUpper(staff.role[0]) + staff.role.Substring(1);

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
                editStaffButtonsStackPanel.Visibility = Visibility.Collapsed;
                showStaffButtonsStackPanel.Visibility = Visibility.Visible;
                fullNameTextBox.IsReadOnly = true;
                fullNameTextBox.Focusable = false;
                fullNameTextBox.BorderThickness = new Thickness(0, 0, 0, 0);
                roleComboBox.Visibility = Visibility.Collapsed;
                roleTextBox.Visibility = Visibility.Visible;
            });
        }

        public void resetFields()
        {
            this.Dispatcher.Invoke(() =>
            {
                fullNameTextBox.Text = staffNameLabel.Content.ToString();
                roleComboBox.SelectedValue = staffRoleLabel.Content.ToString();
            });
        }

        public void editButton_Click()
        {
            changeToEditStaffPage();
        }

        public void deleteButton_Click()
        {
        }

        public void cancelButton_Click()
        {
            resetFields();
            changeToShowStaffPage();
        }

        public void saveButton_Click()
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
                    getController().callMethod("updateStaff", staffId, name, email, 
                                               role, staffImage);
                    break;
                case MessageBoxResult.Cancel:
                    break;
                default:
                    break;
            }
        }

        public void navigateToStaffListPage()
        {
            this.Dispatcher.Invoke(() =>
            {
//                MyPage page = assetnest_wpf.EditProfile.EditProfilePage();
//
//                this.NavigationService.Navigate(page);
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

        private MessageBoxResult showMessage(string message, MessageBoxButton buttons, MessageBoxImage icon)
        {
            MessageBoxResult messageResult = MessageBoxResult.OK;

            this.Dispatcher.Invoke(() =>
            {
                messageResult = MessageBox.Show(message, Application.Current.MainWindow.Title, buttons, icon);
            });

            return messageResult;
        }

        public void startLoading()
        {
            this.Dispatcher.Invoke(() => {
                loadingProgressBar.Visibility = Visibility.Visible;
            });
        }

        public void endLoading()
        {
            this.Dispatcher.Invoke(() => {
                loadingProgressBar.Visibility = Visibility.Hidden;
            });
        }
    }
}
