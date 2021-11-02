using Microsoft.Maui.Platform;

namespace Microsoft.Maui.Handlers
{
	public partial class ApplicationHandler : ElementHandler<IApplication, UI.Xaml.Application>
	{
		public static void MapTerminate(ApplicationHandler handler, IApplication application, object? args)
		{
			handler.NativeView.Exit();
		}

		public static void MapOpenWindow(ApplicationHandler handler, IApplication application, object? args)
		{
			handler.NativeView?.CreateNativeWindow(application, args as OpenWindowRequest);
		}
	}
}