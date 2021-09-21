using System;

namespace Common
{
    public static class SpeedMeter
    {
        public static (TResult, TimeSpan) Run<TResult, TInput>(TInput parameter, Func<TInput, TResult> func)
        {
            var startTime = DateTime.Now;
            var result = func(parameter);
            var endTime = DateTime.Now;

            return (result, endTime - startTime);
        }
    }
}
