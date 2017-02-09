using System;
using Android.Webkit;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace ZendeskXamarinAndroid.Droid.Binding
{
	public class HtmlArticlesWebViewBinding : MvxConvertingTargetBinding
	{
		private readonly WebView _webView;

		public HtmlArticlesWebViewBinding(WebView view) : base(view)
		{
			_webView = view;
		}

		public override Type TargetType => typeof(string);

		protected override void SetValueImpl(object target, object value)
		{
			var html = (string)value;
			html= html.Replace("////", "http://");
			_webView.LoadDataWithBaseURL("", html, "text/html", "UTF-8", "");
		}

		public override MvxBindingMode DefaultMode => MvxBindingMode.Default;
	}
}
