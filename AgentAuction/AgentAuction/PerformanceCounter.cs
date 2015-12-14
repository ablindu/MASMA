using System;
using System.Runtime.InteropServices;
using System.Security;

namespace AgentAuction
{
    /// <summary>
    /// Uses the high-precision counters to compute execution time
    /// </summary>
    public class PerformanceCounter
    {
        private static long _frecv;
        private static long _t1, _t2;


        static PerformanceCounter()
        {
            _t1 = 0;
            _t2 = 0;
            _frecv = 0;
            bool init = QueryPerformanceFrequency(out _frecv);
            if (!init)
                throw new Exception("Performance counters not initialized");
            Reset();
        }

        [DllImport("kernel32", EntryPoint = "QueryPerformanceFrequency")]
        [SuppressUnmanagedCodeSecurity]
        private static extern bool QueryPerformanceFrequency(out long lpPerformanceFreq);

        [DllImport("kernel32", EntryPoint = "QueryPerformanceCounter")]
        [SuppressUnmanagedCodeSecurity]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        /// <summary>
        /// Resets the counter
        /// </summary>
        public static void Reset()
        {
            QueryPerformanceCounter(out _t1);
        }

        /// <summary>
        /// Gets the current counter value in seconds
        /// </summary>
        public static double GetValue()
        {
            QueryPerformanceCounter(out _t2);
            long dif_counts = _t2 - _t1;
            double dif_secs = (double) dif_counts/(double) _frecv;
            return dif_secs;
        }
    }
}