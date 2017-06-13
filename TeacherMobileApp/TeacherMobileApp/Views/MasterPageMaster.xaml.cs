using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TeacherMobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeacherMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPageMaster : ContentPage
    {
        public ListView ListView;
        MasterViewModel viewModel;
        public MasterPageMaster()
        {
            InitializeComponent();

            BindingContext = viewModel = new MasterViewModel();
            ListView = MenuItemsListView;

            SignOutText.GestureRecognizers.Add(new TapGestureRecognizer { Command = viewModel.SignOut });
        }
    }
}