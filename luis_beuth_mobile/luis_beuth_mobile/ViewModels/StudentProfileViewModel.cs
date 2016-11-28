using Xamarin.Forms;

namespace luis_beuth_mobile.ViewModels
{
    class StudentProfileViewModel
    {
        public string Name { get { return Application.Current.Properties["name"] as string; } set { } }
        public string MatriculationNumber { get { return Application.Current.Properties["studentId"] as string; } set { } }
    }
}
