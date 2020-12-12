using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Velacro.DataStructures;
using Velacro.UIElements.Basic;
using Velacro.UIElements.Button;
using Velacro.UIElements.TextBlock;
using Velacro.UIElements.TextBox;

namespace assetnest_wpf.Employee
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : MyPage
    {
        public AddUser()
        {
            InitializeComponent();
            this.KeepAlive = true;
            setController(new AddUserController(this));
            initUIBuilders();
            initUIElements();
        }

        private BuilderButton buttonBuilder;
        private BuilderTextBox txtBoxBuilder;
        private ComboBox comboBox;
        private BuilderTextBlock txtBlockBuilder;

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
        private IMyTextBlock saveStatusTxtBlock;

        private void initUIElements()
        {
            saveButton = buttonBuilder.activate(this, "save_btn")
                .addOnClick(this, "onSaveButtonClick");
            cancelButton = buttonBuilder.activate(this, "cancel_btn")
                .addOnClick(this, "onCancelButtonClick");
            nameTxtBox = txtBoxBuilder.activate(this, "name_txt");
            emailTxtBox = txtBoxBuilder.activate(this, "email_txt");
            comboBox = this.FindName("role_cb") as ComboBox;
            saveStatusTxtBlock = txtBlockBuilder.activate(this, "saveStatus");
        }

        public void onSaveButtonClick()
        {
            getController().callMethod("save",
                nameTxtBox.getText(),
                emailTxtBox.getText(),
                comboBox.SelectedValue.ToString());
        }

        public void cancelButton_Click()
        {
           // this.NavigationService.Navigate(new ListUserPage());
        }

        public void setAddUserStatus(string _status)
        {
            this.Dispatcher.Invoke(() => {
                MessageBoxResult result = MessageBox.Show(_status, "Add User", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        this.NavigationService.Navigate(new AddUser());
                        break;
                }
            });

        }
    }
}
