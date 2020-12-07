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

        public EditStaffPage(User staff)
        {
            InitializeComponent();
            setController(new EditStaffController(this));
            initUIBuilders();
            initUIElements();
            initStaffProfile(staff);
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

        private void initStaffProfile(User staff)
        {
            staff = new User()
            {
                id = 1,
                company_id = 1,
                name = "cinta",
                role = "admin",
                email = "cinta@gmail.com",
                image = "/upload/user/U-1606353684.jpg"
            };

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
                Uri imageUri = new Uri("http://api.assetnest.me/" + staff.image);

                staffImageImageBrush.ImageSource = new BitmapImage(imageUri);
                staffImageTooltipImage.Source = new BitmapImage(imageUri);
            }
        }

        public void saveButton_Click()
        {
            User newStaffData = new User()
            {
                id = staffId,
                name = fullNameTextBox.getText(),
                email = emailTextBox.getText(),
                role = roleComboBox.SelectedValue.ToString(),
                image = staffImage
            };

            getController().callMethod("updateStaff", staffId, newStaffData);
        }

        public void cancelButton_Click()
        {

        }
    }
}
