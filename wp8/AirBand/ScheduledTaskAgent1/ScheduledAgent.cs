using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;

using Microsoft.Phone.Shell;
using System;


namespace ScheduledTaskAgent1
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// ScheduledAgent 생성자는 UnhandledException 처리기를 초기화합니다.
        /// </remarks>
        static ScheduledAgent()
        {
            // 관리되는 예외 처리기에 알림을 신청합니다.
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// 처리되지 않은 예외에 대해 실행할 코드입니다.
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // 처리되지 않은 예외가 발생했습니다. 중단하고 디버거를 실행합니다.
                Debugger.Break();
            }
        }

        /// <summary>
        /// 예약된 작업을 실행하는 에이전트입니다.
        /// </summary>
        /// <param name="task">
        /// 호출한 작업입니다.
        /// </param>
        /// <remarks>
        /// 이 메서드는 정기적 작업 또는 리소스를 많이 사용하는 작업이 호출될 때 호출됩니다.
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            //TODO: 백그라운드에서 작업을 수행하는 코드를 추가합니다.

            if (task is PeriodicTask)
            {
                ShellToast toast = new ShellToast();
                toast.Title = "TimeAgent";
                toast.Content = DateTime.Now.ToString();
                toast.Show();
                ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(60));
                NotifyComplete();

            }
        }
    }
}