using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace luis_beuth_mobile.ViewModels
{
	//nuget not loading, temporary viewmodels
	class BaseViewModel : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
