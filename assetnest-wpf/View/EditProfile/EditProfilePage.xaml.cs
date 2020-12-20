using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Velacro.Basic;
using Velacro.LocalFile;
using Velacro.UIElements.Basic;
using Velacro.UIElements.TextBox;

using assetnest_wpf.Model;
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
        private bool changePassword;
        private BuilderTextBox builderTextBox;
        private Button changePasswordButton;
        private Image profileImageTooltipImage;
        private ImageBrush profileImageImageBrush;
        private Label roleLabel;
        private Label nameLabel;
        private PasswordBox currentPasswordBox;
        private PasswordBox newPasswordBox;
        private PasswordBox confirmNewPasswordBox;
        private ProgressBar loadingProgressBar;
        private MyList<MyFile> newProfileImage;
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
            changePassword = false;
        }

        private void initUIBuilders()
        {
            builderTextBox = new BuilderTextBox();
        }

        private void initUIElements()
        {
            profileImageTooltipImage = this.FindName("profileimage_tooltip_image") as Image;
            profileImageImageBrush = this.FindName("profileimage_imagebrush") as ImageBrush;
            roleLabel = this.FindName("role_label") as Label;
            nameLabel = this.FindName("name_label") as Label;
            currentPasswordBox = this.FindName("current_passwordbox") as PasswordBox;
            newPasswordBox = this.FindName("new_passwordbox") as PasswordBox;
            confirmNewPasswordBox = this.FindName("confirmnew_passwordbox") as PasswordBox;
            loadingProgressBar = this.FindName("loading_progressbar") as ProgressBar;
            fullNameTextBox = builderTextBox.activate(this, "fullname_textbox");
            roleTextBox = builderTextBox.activate(this, "role_textbox");
            emailTextBox = builderTextBox.activate(this, "email_textbox");
            changePasswordButton = this.FindName("changepassword_button") as Button;
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

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            navigateToProfilePage();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = fullNameTextBox.getText();
            string email = emailTextBox.getText();
            string currentPassword = currentPasswordBox.Password;
            string newPassword = newPasswordBox.Password;
            string confirmNewPassword = confirmNewPasswordBox.Password;
            MyFile newImageFile = newProfileImage[0];

            if (name.Equals("") || email.Equals("") || 
                (changePassword && (currentPassword.Equals("") || 
                                    newPassword.Equals("") || 
                                    confirmNewPassword.Equals(""))))
            {
                showErrorMessage("All fillable fields are required.");
                return;
            }

            if (changePassword && !confirmNewPassword.Equals(newPassword))
            {
                showErrorMessage("New password and confirm new password should be equal.");
                return;
            }

            switch (showConfirmationMessage("Proceed update profile?"))
            {
                case MessageBoxResult.OK:
                    if (changePassword)
                    {
                        getController()
                            .callMethod("updateUser", userId, name, email, currentPassword, 
                                        newPassword, currentImagePath, newImageFile);
                    }
                    else
                    {
                        getController()
                            .callMethod("updateUser", userId, name, email, null, 
                                        null, currentImagePath, newImageFile);
                    }
                    break;
                default:
                    break;
            }
        }

        private void loadImageButton_Click(object sender, RoutedEventArgs e)
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
                    showErrorMessage("Image must be in .jpg, .jpeg, or .png and less than 1 MB!");
                }
            }
        }

        private void changePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            currentPasswordBox.Password = "";
            newPasswordBox.Password = "";
            confirmNewPasswordBox.Password = "";

            if (changePasswordButton.Content.ToString().Equals("Change Password"))
            {
                changePassword = true;
                currentPasswordBox.Visibility = Visibility.Visible;
                newPasswordBox.Visibility = Visibility.Visible;
                confirmNewPasswordBox.Visibility = Visibility.Visible;
                changePasswordButton.Content = "Cancel Change Password";
            } 
            else
            {
                changePassword = false;
                currentPasswordBox.Visibility = Visibility.Collapsed;
                newPasswordBox.Visibility = Visibility.Collapsed;
                confirmNewPasswordBox.Visibility = Visibility.Collapsed;
                changePasswordButton.Content = "Change Password";
            }
        }

        public void navigateToProfilePage()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.NavigationService.Navigate(new EditProfilePage());
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