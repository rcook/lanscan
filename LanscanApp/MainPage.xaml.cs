//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using System.Collections.Generic;
    using LanscanApp.Common;

    public sealed partial class MainPage : LayoutAwarePage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            if (navigationParameter == null)
            {
                throw new ArgumentNullException("navigationParameter");
            }

            if (pageState == null)
            {
                var args = (MainPageArgs)navigationParameter;
                DataContext = CreateViewModel(args);
            }
            else
            {
                DataContext = pageState["DataContext"];
            }
        }

        protected override void SaveState(Dictionary<string, object> pageState)
        {
            if (pageState == null)
            {
                throw new ArgumentNullException("pageState");
            }

            pageState["DataContext"] = DataContext;
        }

        private static MainPageViewModel CreateViewModel(MainPageArgs args)
        {
            var result = new MainPageViewModel(args.ServiceProvider, args.AppViewModel);
            return result;
        }
    }
}
