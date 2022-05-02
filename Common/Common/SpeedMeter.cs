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

        public static (TResult, TimeSpan) Run<TResult, TInput1, TInput2, TInput3>(TInput1 parameter1, TInput2 parameter2, TInput3 parameter3, Func<TInput1, TInput2, TInput3, TResult> func)
        {
            var startTime = DateTime.Now;
            var result = func(parameter1, parameter2, parameter3);
            var endTime = DateTime.Now;

            return (result, endTime - startTime);
        }

        public static (TResult, TimeSpan) Run<TResult, TInput1, TInput2, TInput3, TInput4>
            (TInput1 parameter1, TInput2 parameter2, TInput3 parameter3, TInput4 parameter4, Func<TInput1, TInput2, TInput3, TInput4, TResult> func)
        {
            var startTime = DateTime.Now;
            var result = func(parameter1, parameter2, parameter3, parameter4);
            var endTime = DateTime.Now;

            return (result, endTime - startTime);
        }

        public static (TResult, TimeSpan) Run<TResult, TInput1, TInput2, TInput3, TInput4, TInput5>
            (TInput1 parameter1, TInput2 parameter2, TInput3 parameter3, TInput4 parameter4, TInput5 parameter5, Func<TInput1, TInput2, TInput3, TInput4, TInput5, TResult> func)
        {
            var startTime = DateTime.Now;
            var result = func(parameter1, parameter2, parameter3, parameter4, parameter5);
            var endTime = DateTime.Now;

            return (result, endTime - startTime);
        }
    }
}
