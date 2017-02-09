using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform;
using ZendeskXamarinAndroid.Core.Services;
using MvvmCross.Binding.Bindings.Target.Construction;
using Android.Webkit;
using Android.Widget;
using ZendeskXamarinAndroid.Droid.Binding;
using System.Collections.Generic;
using Android.Renderscripts;

namespace ZendeskXamarinAndroid.Droid
{
	public class Setup : MvxAndroidSetup
	{
		public Setup(Context applicationContext) : base(applicationContext)
		{
		}

		protected override IMvxApplication CreateApp()
		{
			return new Core.App();
		}

		protected override IMvxTrace CreateDebugTrace()
		{
			return new DebugTrace();
		}


		protected override void InitializeFirstChance()
		{
			Mvx.ConstructAndRegisterSingleton<IDialogsService, DialogsService>();
			Mvx.ConstructAndRegisterSingleton<IMessageImageUploadService, MessageImageUploadService>();
			Mvx.ConstructAndRegisterSingleton<IFileSystem, FileSystemService>();

			base.InitializeFirstChance();
		}

		protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
		{
			base.FillTargetFactories(registry);

			registry.RegisterFactory(new MvxCustomBindingFactory<WebView>("HTMLArticleContent", (WebView) => new HtmlArticlesWebViewBinding(WebView)));
			registry.RegisterFactory(new MvxCustomBindingFactory<LinearLayout>("HtmlTicketsAttachments", (LinearLayout) => new HtmlTicketsAttachmentsBinding(LinearLayout)));
			registry.RegisterFactory(new MvxCustomBindingFactory<ImageView>("TicketsAttachmentPreview", (ImageView) => new TicketsAttachmentPreviewBinding(ImageView)));


		}

	}
}
