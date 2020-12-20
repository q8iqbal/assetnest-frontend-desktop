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
using Velacro.UIElements.PasswordBox;
using Velacro.UIElements.TextBox;

using assetnest_wpf.Model;
using assetnest_wpf.View.Profile;
using assetnest_wpf.Utils;

namespace assetnest_wpf.View.EditProfile
{
    /// <summary>
    /// Interaction logic for EditProfilePage.xaml
    /// </summary>
    public partial class EditProfilePage : MyPage
    {
        private int userId;
        private string currentImagePath;
        private MyList<MyFile> newProfileImage;
        private BuilderButton builderButton;
        private BuilderPasswordBox builderPasswordBox;
        private BuilderTextBox builderTextBox;
        private Image profileImageTooltipImage;
        private ImageBrush profileImageImageBrush;
        private Label roleLabel;
        private Label nameLabel;
        private ProgressBar loadingProgressBar;
        private IMyButton cancelButton;
        private IMyButton saveButton;
        private IMyButton loadImageButton;
        private IMyPasswordBox passwordPasswordBox;
        private IMyTextBox fullNameTextBox;
        private IMyTextBox roleTextBox;
        private IMyTextBox emailTextBox;

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
            builderButton = new BuilderButton();
            builderPasswordBox = new BuilderPasswordBox();
            builderTextBox = new BuilderTextBox();
        }

        private void initUIElements()
        {
            profileImageTooltipImage = this.FindName("profileimage_tooltip_image") as Image;
            profileImageImageBrush = this.FindName("profileimage_imagebrush") as ImageBrush;
            roleLabel = this.FindName("role_label") as Label;
            nameLabel = this.FindName("name_label") as Label;
            loadingProgressBar = this.FindName("loading_progressbar") as ProgressBar;
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
            User user = StorageUtil.Instance.user;
            string role = Char.ToUpper(user.role[0]) + user.role.Substring(1);

            userId = user.id;
            newProfileImage = new MyList<MyFile>();
            newProfileImage.Add(null);
            currentImagePath = user.image;
            roleLabel.Content = role;
            nameLabel.Content = user.name;
            fullNameTextBox.setText(user.name);
            roleTextBox.setText(role);
            emailTextBox.setText(user.email);
            if (currentImagePath != null)
            {
                Uri imageUri = new Uri(Constants.BASE_URL + currentImagePath);

                profileImageImageBrush.ImageSource = new BitmapImage(imageUri);
                profileImageTooltipImage.Source = new BitmapImage(imageUri);
            }
        }

        public void cancelButton_Click() 
        {
            navigateToProfilePage();
        }

        public void saveButton_Click()
        {
            string name = fullNameTextBox.getText();
            string email = emailTextBox.getText();
            string password = passwordPasswordBox.getPassword();
            MyFile newImageFile = newProfileImage[0];

            if (name.Equals("") || email.Equals("") || password.Equals(""))
            {
                showErrorMessage("All fillable fields are required.");
                return;
            }

            switch (showConfirmationMessage("Proceed update profile?"))
            {
                case MessageBoxResult.OK:
                    getController().callMethod("updateUser", userId, name, email, 
                                               password, currentImagePath, newImageFile);
                    break;
                case MessageBoxResult.Cancel:
                    break;
                default:
                    break;
            }
        }

        public void loadImageButton_Click()
        {
            MyList<MyFile> chosenImage = new OpenFile().openFile(false);

            if (chosenImage[0] != null) 
            {
                string size = chosenImage[0].fileSize;
                if ((chosenImage[0].extension.ToUpper().Equals(".PNG") ||
                     chosenImage[0].extension.ToUpper().Equals(".JPEG") ||
                     chosenImage[0].extension.ToUpper().Equals(".JPG")) &&
                     size.Substring(size.Length - 2).ToUpper().Equals("KB"))
                {
                    newProfileImage.Clear();
                    newProfileImage.Add(chosenImage[0]);

                    Uri newImageUri = new Uri(newProfileImage[0].fullPath);

                    profileImageImageBrush.ImageSource = new BitmapImage(newImageUri);
                    profileImageTooltipImage.Source = new BitmapImage(newImageUri);
                } 
                else
                {
                    showErrorMessage("Image should be in .jpg, .jpeg, or .png and less than 1 MB!");
                }
            }
        }

        public void navigateToProfilePage()
        {
            this.Dispatcher.Invoke(() =>
            {

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