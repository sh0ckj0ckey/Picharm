using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Picharm.Helpers
{
    // https://github.com/Microsoft/Windows-task-snippets/blob/master/tasks/UI-thread-access-from-background-thread.md
    internal class DispatcherHelper
    {
        public static async Task CallOnUiThreadAsync(CoreDispatcher dispatcher = null, DispatchedHandler handler = null)
        {
            try
            {
                if (dispatcher != null)
                {
                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, handler);
                }
                else
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, handler);
                }
            }
            catch (Exception e) { Trace.WriteLine(e.Message); }
        }

        //public static async Task CallOnMainViewUiThreadAsync(DispatchedHandler handler) =>
        //    await CallOnUiThreadAsync(CoreApplication.MainView.CoreWindow.Dispatcher, handler);
    }
}
