using System;
using System.Collections.Generic;
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

namespace assetnest_wpf.EditProfile
{
    /// <summary>
    /// Interaction logic for EditProfile.xaml
    /// </summary>
    public partial class EditProfilePage : MyPage
    {
        int userId;
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
        {/*
            User user = new User()
            {
                id = 1,
                name = "Andi Zain",
                company_id = 1,
                email = "andizain@gmail.com",
                image = null,
                password = "andizain",
                role = "Owner"
            };

            userId = user.id;
            roleLabel.Content = user.role;
            nameLabel.Content = user.name;
            fullNameTextBox.setText(user.name);
            roleTextBox.setText(user.role);
            emailTextBox.setText(user.email);
            passwordPasswordBox.setPassword(user.password);*/
        }

        public void cancelButton_Click() 
        {/*
            this.NavigationService.Navigate(new ProfilePage()); */
        }

        public void saveButton_Click()
        {/*
            string newName = fullNameTextBox.getText();
            string newEmail = emailTextBox.getText();
            string newPassword = passwordPasswordBox.getPassword();
            string newImage = null;

            getController().callMethod("updateUser", userId, newName, newEmail, newImage);
            getController().callMethod("updateUserPassword", userId, newPassword);
            this.NavigationService.Navigate(new ProfilePage()); */
        }

        public void loadImageButton_Click()
        {
            MyList<BitmapImage> images = new OpenFile().openImageFile(false);

            if (images != null) { 
                profileImageImageBrush.ImageSource = images[0];
                profileImageTooltipImage.Source = images[0];
            }
        }
    }
}