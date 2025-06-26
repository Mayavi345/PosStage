using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace UIComponent
{
    public class TimerHelper
    {
        private const int INTERVALTIME = 10;
        private DispatcherTimer _timer;
        private int _elapsedTime = 0;  // 已經經過的時間（單位：毫秒）
        public Action timeFinishAction;

        private float _finishTime;
        public float FinishPercent => (_elapsedTime / (_finishTime * 100)) * 100;

        public bool isStop = false;

        public TimerHelper(float time, Action action)
        {
            _finishTime = time;
            timeFinishAction = action;
        }
        public void Start()
        {
            isStop = false;

            _elapsedTime = 0;
            _timer = new DispatcherTimer();

            _timer.Interval = TimeSpan.FromMilliseconds(INTERVALTIME);  // 設定計時器的觸發時間間隔
            _timer.Tick += Timer_Tick;  // 訂閱計時器的 Tick 事件

            _timer.Start();  // 啟動計時器
        }
        public bool IsFinish()
        {
            return isStop;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            _elapsedTime += INTERVALTIME;

            if (_elapsedTime >= (_finishTime * 100) - 1)
            {

                if (isStop == false)
                    timeFinishAction?.Invoke();
                _elapsedTime = 0;
                isStop = true;
                _timer.Stop();
            }
        }


        public void Stop()
        {
            if (_timer == null)
                return;

            _timer.Stop();  // 停止計時器
        }
    }
}
