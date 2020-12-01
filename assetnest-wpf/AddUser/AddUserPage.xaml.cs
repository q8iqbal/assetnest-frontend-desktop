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
using System.Windows.Shapes;
using Velacro.UIElements.Basic;
using Velacro.UIElements.Button;
using Velacro.UIElements.TextBlock;
using Velacro.UIElements.TextBox;
using Velacro.UIElements.ListBox;

namespace assetnest_wpf.AddUser
{
    /// <summary>
    /// Interaction logic for AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : MyPage
    {
        public AddUserPage()
        {
            InitializeComponent();
            this.KeepAlive = true;
            setController(new AddUserController(this));
            initUIBuilders();
            initUIElements();
        }

        private void setController(AddUserController addUserController)
        {
            throw new NotImplementedException();
        }

        private BuilderButton buttonBuilder;
        private BuilderTextBox txtBoxBuilder;
        private BuilderListBox listBoxBuilder;
        private BuilderTextBlock txtBlockBuilder;

        private void initUIBuilders()
        {
            buttonBuilder = new BuilderButton();
            txtBoxBuilder = new BuilderTextBox();
            listBoxBuilder = new BuilderListBox();
            txtBlockBuilder = new BuilderTextBlock();
        }

        private IMyButton addUserButton;
        private IMyTextBox emailTxtBox;
        private IMyButton saveButton;
        private IMyTextBox staffNameTxtBox;
        private IMyListBox roleListBox;
        private IMyTextBlock addUserStatusTxtBlock;

        public bool KeepAlive { get; }
        public object Dispatcher { get; private set; }

        private void initUIElements()
        {
            addUserButton = buttonBuilder.activate((IMyContainer)this, "addUser_btn")
                .addOnClick((Velacro.Basic.IMyController)this, "onAddUserButtonClick");
            staffNameTxtBox = txtBoxBuilder.activate((IMyContainer)this, "name_txt");
            emailTxtBox = txtBoxBuilder.activate((IMyContainer)this, "email_txt");
            roleListBox = listBoxBuilder.activate((IMyContainer)this, "role_lb");
            addUserStatusTxtBlock = txtBlockBuilder.activate((IMyContainer)this, "addUserStatus");
        }

        public void onAddUserButtonClick()
        {
            getController().callMethod("addUser",
                staffNameTxtBox.getText(),
                emailTxtBox.getText());
        }

        private object getController()
        {
            throw new NotImplementedException();
        }

        public void setAddUserStatus(string _status)
        {
            this.Dispatcher.Invoke(() => {
                addUserButton.setText(_status);
            });

        }
    }
}
