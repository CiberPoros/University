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

        public static (TResult, TimeSpan) Run<TResult, TInput1, TInput2>(TInput1 parameter1, TInput2 parameter2, Func<TInput1, TInput2, TResult> func)
        {
            var startTime = DateTime.Now;
            var result = func(parameter1, parameter2);
            var endTime = DateTime.Now;

            return (result, endTime - startTime);
        }
    }
}
