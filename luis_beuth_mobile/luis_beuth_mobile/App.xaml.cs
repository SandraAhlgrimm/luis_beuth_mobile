﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace luis_beuth_mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Debug.WriteLine("LOG: Entry Point");
            if (Application.Current.Properties.ContainsKey("studentId"))
            {
                var studentId = Application.Current.Properties["studentId"] as string;
                Debug.WriteLine("LOG: Read " + studentId + " as Login");
            } else
            {
                // ToDo: Call QR Scanner to Login
                Debug.WriteLine("LOG: No Student ID");
            }
            

            MainPage =  new luis_beuth_mobile.Views.MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
