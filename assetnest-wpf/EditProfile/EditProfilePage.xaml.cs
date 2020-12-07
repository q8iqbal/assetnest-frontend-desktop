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
using Velacro.Basic;
using Velacro.LocalFile;
using Velacro.UIElements.Basic;
using Velacro.UIElements.Button;
using Velacro.UIElements.TextBox;
using Velacro.UIElements.PasswordBox;
//using assetnest_wpf.Model;
using assetnest_wpf.Profile;
using assetnest_wpf.Model;
using Newtonsoft.Json.Linq;

namespace assetnest_wpf.EditProfile
{
    /// <summary>
    /// Interaction logic for EditProfile.xaml
    /// </summary>
    public partial class EditProfilePage : MyPage
    {
        private int userId;
        private MyList<MyFile> profileImage;
        private BuilderButton builderButton;
        private BuilderTextBox builderTextBox;
        private BuilderPasswordBox builderPasswordBox;
        private Label roleLabel;
        private Label nameLabel;
        private IMyTextBox fullNameTextBox;
        private IMyTextBox roleTextBox;
        private IMyTextBox emailTextBox;
        private IMyPasswordBox passwordPasswordBox;
        private IMyButton cancelButton;
        private IMyButton saveButton;
        private IMyButton loadImageButton;
        private ImageBrush profileImageImageBrush;
        private Image profileImageTooltipImage;

        public EditProfilePage()
        {
            InitializeComponent();
            setController(new EditProfileController(this));
            initUIBuilders();
            initUIElements();
            initProfile();
        }

        private void initUIBuilders()
        {
            builderTextBox = new BuilderTextBox();
            builderPasswordBox = new BuilderPasswordBox();
            builderButton = new BuilderButton();
        }

        private void initUIElements()
        {
            roleLabel = this.FindName("role_label") as Label;
            nameLabel = this.FindName("name_label") as Label;
            profileImageImageBrush = this.FindName("profileimage_imagebrush") as ImageBrush;
            profileImageTooltipImage = this.FindName("profileimage_tooltip_image") as Image;
            fullNameTextBox = builderTextBox.activate(this, "fullname_textbox");
            roleTextBox = builderTextBox.activate(this, "role_textbox");
            emailTextBox = builderTextBox.activate(this, "email_textbox");
            passwordPasswordBox = builderPasswordBox.activate(this, "password_passwordbox");
            cancelButton = builderButton.activate(this, "cancel_button")
                .addOnClick(this, "cancelButton_Click");
            saveButton = builderButton.activate(this, "save_button")
                .addOnClick(this, "saveButton_Click");
            loadImageButton = builderButton.activate(this, "loadimage_button")
                .addOnClick(this, "loadImageButton_Click");
        }

        private void initProfile()
        {
            profileImage = new MyList<MyFile>();
            profileImage.Add(null);
            
            User user = new User()
            {
                id = 1,
                name = "Andi Zain",
                company_id = 1,
                email = "andizain@gmail.com",
                image = null,
                role = "Owner"
            };

            userId = user.id;
            roleLabel.Content = user.role;
            nameLabel.Content = user.name;
            fullNameTextBox.setText(user.name);
            roleTextBox.setText(user.role);
            emailTextBox.setText(user.email);
        }

        public void cancelButton_Click() 
        {
            redirectToProfilePage();
        }

        public void saveButton_Click()
        {
            string confirmationMessage = "Proceed update profile?";
            string name = fullNameTextBox.getText();
            string email = emailTextBox.getText();
            string password = passwordPasswordBox.getPassword();
            MessageBoxResult confirmationResult = showConfirmationMessage(confirmationMessage);
            
            switch (confirmationResult)
            {
                case MessageBoxResult.Yes:
                    getController().callMethod("putUser", 1, name, email, password, profileImage[0]);
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
//            getController().callMethod("updateUserPassword", userId, newPassword);
//            this.NavigationService.Navigate(new ProfilePage()); 
        }

        public void loadImageButton_Click()
        {
            MyList<MyFile> chosenImage = new OpenFile().openFile(false);

            if (chosenImage[0] != null) 
            {
                if (chosenImage[0].extension.ToUpper().Equals(".PNG") ||
                    chosenImage[0].extension.ToUpper().Equals(".JPEG") ||
                    chosenImage[0].extension.ToUpper().Equals(".JPG"))
                {
                    profileImage.Clear();
                    profileImage.Add(chosenImage[0]);
                    
                    profileImageImageBrush.ImageSource = new BitmapImage(new Uri(profileImage[0].fullPath));
                    profileImageTooltipImage.Source = new BitmapImage(new Uri(profileImage[0].fullPath));
                }
            }
        }

        public void redirectToProfilePage()
        {
            this.NavigationService.Navigate(new ProfilePage());
        }

        public void redirectToLogin()
        {
            
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
            return showMessage(message, MessageBoxButton.YesNo, MessageBoxImage.Question);
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