using application.App.Pages.View;
using Microsoft.Extensions.DependencyInjection;

namespace Application.App
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        private IServiceProvider _serviceProvider;
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            //return new Window(new AppShell());
            return new Window(_serviceProvider.GetRequiredService<MainPageView>());
        }
    }
}