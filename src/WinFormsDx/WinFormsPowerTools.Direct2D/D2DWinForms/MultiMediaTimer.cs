using System.ComponentModel;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Media;

namespace System.Windows.Forms.Direct2D
{
    public class MultiMediaTimer
	{
		private uint _timerID;                  // ID of multi media timers
		private uint _user = 0;                 // user-defined Parameter - not used
		private TIMECAPS _timerCaps;            // Contains TimeCaps, describing timer resolution.
		private uint _mode = 1;                 // 0=once, 1=periodic - latter is default.

		private uint _periodInMs;               // How often should be triggered
		private uint _resolutionInMs;           // Resolution capabilities of timer.
		private bool _hasStarted;               // Is the timer running?

		private LPTIMECALLBACK _callBackTimeProc;   // callback when timer has ellapsed.

		public event EventHandler? Elapsed;

		/// <summary>
		/// Creates an instance of this class and sets the timer frequency and resolution in ms.
		/// </summary>
		/// <param name="periodInMs">Timer frequency</param>
		/// <param name="resolutionInMs">Timer resolution (default is 1 ms for most accuracy).</param>
		/// <remarks></remarks>
		public MultiMediaTimer(int periodInMs, int resolutionInMs = 1)
		{
			var ret = PInvoke.timeGetDevCaps(out _timerCaps, (uint)Marshal.SizeOf(_timerCaps));

			_periodInMs = (uint) periodInMs;
			_resolutionInMs = (uint) resolutionInMs;
			_callBackTimeProc = CallBackTimeProc;
		}

		/// <summary>
		/// Starts the timer.
		/// </summary>
		public void Start()
		{
			if (_hasStarted)
			{
				return;
			}

			var ret = PInvoke.timeSetEvent(_periodInMs, _resolutionInMs, _callBackTimeProc, _user, _mode);
			var hr = Marshal.GetHRForLastWin32Error;

			if (ret == 0)
			{
				throw new Win32Exception("Timer could not be started.");
			}
			else
			{
				_timerID = ret;
				_hasStarted = true;
			}
		}

		/// <summary>
		/// Stops the timer.
		/// </summary>
		/// <remarks></remarks>
		public void Stop()
		{
			if (!_hasStarted)
			{
				return;
			}

			PInvoke.timeKillEvent(_timerID);
		}

        private void CallBackTimeProc(uint id, uint msg, nuint user, nuint param1, nuint param2) 
			=> Elapsed?.Invoke(this, EventArgs.Empty);

        /// Retrieves if the timer is running.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool HasStarted => _hasStarted;
	}
}
