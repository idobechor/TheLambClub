using TheLambClub.ViewModel;

namespace TheLambClub
{
    public partial class MainPage : ContentPage
    {
        #region fields

        private readonly MainPageVM mpVM = new();

        #endregion

        #region constructors

        public MainPage()
        {
            InitializeComponent();
            BindingContext = mpVM;
        }

        #endregion

        #region protected methods

        protected override void OnAppearing()
        {
            base.OnAppearing();
            mpVM.AddSnapshotListener();
        }

        protected override void OnDisappearing()
        {
            mpVM.RemoveSnapshotListener();
            base.OnDisappearing();
        }

        #endregion
    }
}
