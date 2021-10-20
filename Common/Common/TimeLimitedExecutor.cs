﻿using System;
using System.Threading.Tasks;
using FluentResults;

namespace Common
{
    public class TimeLimitedExecutor
    {
        public int TimeLimitInSeconds { get; set; }

        public TimeLimitedExecutor(int timeLimitInSeconds)
        {
            TimeLimitInSeconds = timeLimitInSeconds;
        }

        public TimeLimitedExecutor()
        {
            TimeLimitInSeconds = 5;
        }

        public async Task<Result<T>> ExecuteAsync<T>(Func<T> func)
        {
            var task = Task.Run(() => func());
            return await Task.WhenAny(task, Task.Delay(TimeSpan.FromSeconds(TimeLimitInSeconds))) == task
                ? Result.Ok(task.Result)
                : Result.Fail($"The method did not complete in a limited time = {TimeLimitInSeconds} seconds");
        }
    }
}
