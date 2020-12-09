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
        private Label staffNameLabel;
        private Label staffRoleLabel;
        private IMyTextBox fullNameTextBox;
        private ComboBox roleComboBox;
        private IMyTextBox emailTextBox;
        private IMyButton cancelButton;
        private IMyButton saveButton;
        private ImageBrush staffImageImageBrush;
        private Image staffImageTooltipImage;

        public EditStaffPage(int id)
        {
            InitializeComponent();
            setController(new EditStaffController(this));
            initUIBuilders();
            initUIElements();
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
            fullNameTextBox = builderTextBox.activate(this, "fullname_textbox");
            roleComboBox = this.FindName("role_combobox") as ComboBox;
            emailTextBox = builderTextBox.activate(this, "email_textbox");
            staffImageImageBrush = this.FindName("staffimage_imagebrush") as ImageBrush;
            staffImageTooltipImage = this.FindName("staffimage_tooltip_image") as Image;
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

                staffId = staff.id;
                staffImage = staff.image;
                staffNameLabel.Content = staff.name;
                staffRoleLabel.Content = role;
                fullNameTextBox.setText(staff.name);
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

        public void saveButton_Click()
        {
            string name = fullNameTextBox.getText();
            string email = emailTextBox.getText();
            string role = roleComboBox.SelectedValue.ToString();

            if (name.Equals("") || role.Equals(""))
            {
                showErrorMessage("All fillable fields are required.");

                return;
            }

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

        public void cancelButton_Click()
        {

        }

        public void navigateToStaffPage()
        {
//            this.NavigationService.Navigate(new StaffPage(staffId));
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
    }
}
