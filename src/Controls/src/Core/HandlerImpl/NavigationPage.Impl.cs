using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace Microsoft.Maui.Controls
{
	public partial class NavigationPage : INavigationView
	{
		Thickness IView.Margin => Thickness.Zero;

		partial void Init()
		{
			PushRequested += (_, args) =>
			{
				List<IView> newStack = new List<IView>((this as INavigationView).NavigationStack);
				var request = new MauiNavigationRequestedEventArgs(newStack, args.Animated);
				Handler?.Invoke(nameof(INavigationView.RequestNavigation), request);

			};

			PopRequested += (_, args) =>
			{
				List<IView> newStack = new List<IView>((this as INavigationView).NavigationStack);
				newStack.Remove(args.Page);
				var request = new MauiNavigationRequestedEventArgs(newStack, args.Animated);
				Handler?.Invoke(nameof(INavigationView.RequestNavigation), request);
			};

			RemovePageRequested += (_, args) =>
			{
				List<IView> newStack = new List<IView>((this as INavigationView).NavigationStack);
				newStack.Remove(args.Page);
				var request = new MauiNavigationRequestedEventArgs(newStack, args.Animated);
				Handler?.Invoke(nameof(INavigationView.RequestNavigation), request);
			};

			_insertPageBeforeRequested += (_, args) =>
			{
				// TODO MAUI why is this the only one where the stack insert is delayed?
				Device.BeginInvokeOnMainThread(() =>
				{
					List<IView> newStack = new List<IView>((this as INavigationView).NavigationStack);
					var request = new MauiNavigationRequestedEventArgs(newStack, args.Animated);
					Handler?.Invoke(nameof(INavigationView.RequestNavigation), request);
				});
			};

			PopToRootRequested += (_, args) =>
			{
				List<IView> newStack = new List<IView>((this as INavigationView).NavigationStack);
				var request = new MauiNavigationRequestedEventArgs(newStack, args.Animated);
				Handler?.Invoke(nameof(INavigationView.RequestNavigation), request);
			};
		}

		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
			if (Content is IView view)
			{
				view.Measure(widthConstraint, heightConstraint);
			}

			return new Size(widthConstraint, heightConstraint);
		}

		protected override Size ArrangeOverride(Rectangle bounds)
		{
			Frame = this.ComputeFrame(bounds);

			if (Content is IView view)
			{
				_ = view.Arrange(Frame);
			}

			return Frame.Size;
		}

		//void INavigationView.NavigationFinished()
		//{
		//	throw new NotImplementedException();
		//}

		void INavigationView.RequestNavigation(MauiNavigationRequestedEventArgs eventArgs)
		{
			Handler?.Invoke(nameof(INavigationView.RequestNavigation), eventArgs);
		}

		void INavigationView.NavigationFinished(IReadOnlyList<IView> newStack)
		{
			// TODO MAUI Create sync version of this since there's no animation
			RemoveAsyncInner(CurrentPage, false, true, true)
					.FireAndForget((e) =>
				{
					//Log.Warning(nameof(NavigationViewHandler), $"{e}");
				});

			// TODO MAUI calculate this out better
			//PopAsync()
			//	.FireAndForget((e) =>
			//	{
			//		//Log.Warning(nameof(NavigationViewHandler), $"{e}");
			//	});
		}

		//void INavigationView.InsertPageBefore(IView page, IView before)
		//{
		//	throw new NotImplementedException();
		//}

		//Task<IView> INavigationView.PopAsync() =>
		//	(this as INavigationView).PopAsync(true);

		//async Task<IView> INavigationView.PopAsync(bool animated)
		//{
		//	var thing = await this.PopAsync(animated);
		//	return thing;
		//}

		//Task<IView> INavigationView.PopModalAsync()
		//{
		//	throw new NotImplementedException();
		//}

		//Task<IView> INavigationView.PopModalAsync(bool animated)
		//{
		//	throw new NotImplementedException();
		//}

		//Task INavigationView.PushAsync(IView page) =>
		//	(this as INavigationView).PushAsync(page, true);

		//Task INavigationView.PushAsync(IView page, bool animated)
		//{
		//	return this.PushAsync((Page)page, animated);
		//}

		//Task INavigationView.PushModalAsync(IView page)
		//{
		//	throw new NotImplementedException();
		//}

		//Task INavigationView.PushModalAsync(IView page, bool animated)
		//{
		//	throw new NotImplementedException();
		//}

		//void INavigationView.RemovePage(IView page)
		//{
		//	throw new NotImplementedException();
		//}

		IView Content => this.CurrentPage;

		IReadOnlyList<IView> INavigationView.NavigationStack =>
			this.Navigation.NavigationStack;
	}

}
