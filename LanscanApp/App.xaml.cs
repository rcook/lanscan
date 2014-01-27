//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using System.Threading.Tasks;
    using Lanscan.Utilities;
    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.UI.ApplicationSettings;
    using Windows.UI.Popups;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using LanscanUnhandledExceptionEventArgs = Lanscan.Utilities.UnhandledExceptionEventArgs;
    using UnhandledExceptionEventArgs = Windows.UI.Xaml.UnhandledExceptionEventArgs;

    public sealed partial class App : Application, IDisposable
    {
        private StorageFileEventListener m_verboseListener;

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;

            // Handler only fires when task is garbage-collected. Sigh.
            // http://stackoverflow.com/questions/3973315/taskscheduler-unobservedtaskexception-never-gets-called
            //TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

            // http://www.markermetro.com/2013/01/technical/handling-unhandled-exceptions-with-asyncawait-on-windows-8-and-windows-phone-8/
            //UnhandledException += OnAppUnhandledException;
        }

        public void Dispose()
        {
            if (m_verboseListener != null)
            {
                m_verboseListener.Dispose();
            }
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Lanscan.Utilities.AppEventSource.Debug(System.String)")]
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            RegisterExceptionHandlingSynchronizationContext();

            var rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                m_verboseListener = new StorageFileEventListener("Verbose");
                m_verboseListener.EnableEvents(AppEventSource.Instance, EventLevel.Verbose);

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                var appViewModel = await CreateAppViewModel();
                appViewModel.NavigateToMainPage(args.Arguments);
            }

            AppEventSource.Instance.Debug("Activating");

            // Ensure the current window is active
            Window.Current.Activate();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            RegisterExceptionHandlingSynchronizationContext();
            base.OnActivated(args);
        }

        private static async Task<AppViewModel> CreateAppViewModel()
        {
            var serviceProvider = new AppServiceProvider();
            await serviceProvider.InitializeAsync();
            var result = new AppViewModel(serviceProvider);
            return result;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Lanscan.Utilities.AppEventSource.Debug(System.String)")]
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            AppEventSource.Instance.Debug("Suspending");

            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(
                new SettingsCommand(
                    "privacypolicy",
                    "Privacy policy",
                    command =>
                    {
                        ExternalAppHelper.LaunchUri(Constants.PrivacyPolicyUri);
                    }));
        }

        private void RegisterExceptionHandlingSynchronizationContext()
        {
            //ExceptionHandlingSynchronizationContext
            //    .Register()
            //    .UnhandledException += OnSynchronizationContextUnhandledException;
        }

        private async void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            await new MessageDialog("OnUnobservedTaskException:" + Environment.NewLine + e.Exception.Message).ShowAsync();
        }

        private async void OnAppUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            await new MessageDialog("OnAppUnhandledException:" + Environment.NewLine + e.Exception.Message).ShowAsync();
        }

        private async void OnSynchronizationContextUnhandledException(object sender, LanscanUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            await new MessageDialog("OnSynchronizationContextUnhandledException:" + Environment.NewLine + e.Exception.Message).ShowAsync();
        }
    }
}
