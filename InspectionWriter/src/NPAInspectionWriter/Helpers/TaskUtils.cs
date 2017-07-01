using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NPAInspectionWriter.Helpers
{
    /// <summary>
    /// Class TaskUtils.
    /// </summary>
    public static class TaskUtils
    {
        /// <summary>
        /// Tasks from result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result">The result.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public static Task<T> TaskFromResult<T>( T result )
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetResult( result );
            return tcs.Task;
        }

        /// <summary>
        /// Wait a certain amount of time and run an Action
        /// </summary>
        /// <param name="timespan">Timespan to wait</param>
        /// <param name="action">Action to perform</param>
        public static void WaitAndRun( TimeSpan timespan, Action action )
        {
            Xamarin.Forms.Device.StartTimer( timespan, () => true );
            action?.Invoke();
        }

        /// <summary>
        /// Wait a certain number of milliseconds and run an Action
        /// </summary>
        /// <param name="milliSeconds">Milliseconds to wait</param>
        /// <param name="action">Action to perform</param>
        public static void WaitAndRun( double milliSeconds, Action action ) => WaitAndRun( TimeSpan.FromMilliseconds( milliSeconds ), action );

        /// <summary>
        /// Wait a certain amount of time and run an Action with a specified input
        /// </summary>
        /// <typeparam name="T">Type of the input</typeparam>
        /// <param name="timespan">Timespan to wait</param>
        /// <param name="action">Action to perform</param>
        /// <param name="input">Input for the Action performed</param>
        public static void WaitAndRun<T>( TimeSpan timespan, Action<T> action, T input )
        {
            Xamarin.Forms.Device.StartTimer( timespan, () => true );
            action?.Invoke( input );
        }

        /// <summary>
        /// Wait a certain number of milliseconds and run an Action with a specified input
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="milliSeconds">Milliseconds to wait</param>
        /// <param name="action">Action to perform</param>
        /// <param name="input">Input for the Action performed</param>
        public static void WaitAndRun<T>( double milliSeconds, Action<T> action, T input ) => WaitAndRun( TimeSpan.FromMilliseconds( milliSeconds ), action, input );

        /// <summary>
        /// Wait a certain amound of time and perform an action on each item in an enumerable collection
        /// </summary>
        /// <typeparam name="T">Type in the collection</typeparam>
        /// <param name="timespan">Timespan to wait</param>
        /// <param name="action">Action to perform</param>
        /// <param name="collection">Collection to iterate over for the action</param>
        public static void WaitAndRun<T>( TimeSpan timespan, Action<T> action, IEnumerable<T> collection )
        {
            Xamarin.Forms.Device.StartTimer( timespan, () => true );
            foreach( var value in collection )
                action?.Invoke( value );
        }

        /// <summary>
        /// Wait a certain number of milliseconds and perform an action on each item in an enumberable collection.
        /// </summary>
        /// <typeparam name="T">Type in the collection</typeparam>
        /// <param name="milliSeconds">Milliseconds to wait</param>
        /// <param name="action">Action to perform</param>
        /// <param name="collection">Collection to iterate over for the action</param>
        public static void WaitAndRun<T>( double milliSeconds, Action<T> action, IEnumerable<T> collection ) => WaitAndRun( TimeSpan.FromMilliseconds( milliSeconds ), action, collection );

        /// <summary>
        /// Wait a certain timespan and return the result of a specified function
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="timespan">Timespan to wait</param>
        /// <param name="func">Function to perform</param>
        /// <returns></returns>
        public static T WaitAndRun<T>( TimeSpan timespan, Func<T> func )
        {
            Xamarin.Forms.Device.StartTimer( timespan, () => true );
            return func.Invoke();
        }

        /// <summary>
        /// Wait a specified number of milliseconds and return the result of a specified function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="milliSeconds"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T WaitAndRun<T>( double milliSeconds, Func<T> func ) => WaitAndRun( TimeSpan.FromMilliseconds( milliSeconds ), func );

        /// <summary>
        /// Wait a certain amount of time and return the result of a function with a specified input
        /// </summary>
        /// <typeparam name="T">Type of the input</typeparam>
        /// <typeparam name="TResult">Type to return</typeparam>
        /// <param name="timespan">Timespan to wait</param>
        /// <param name="func">Function to perform</param>
        /// <param name="input">Input for the function</param>
        /// <returns>Specified Return Type for the Function performed</returns>
        public static TResult WaitAndRun<T, TResult>( TimeSpan timespan, Func<T, TResult> func, T input )
        {
            Xamarin.Forms.Device.StartTimer( timespan, () => true );
            return func.Invoke( input );
        }

        /// <summary>
        /// Wait a specified number of milliseconds and return the result of a function with a specified input
        /// </summary>
        /// <typeparam name="T">Type of the input</typeparam>
        /// <typeparam name="TResult">Type to return</typeparam>
        /// <param name="milliSeconds">Milliseconds to wait</param>
        /// <param name="func">Function to perform</param>
        /// <param name="input">Input for the function</param>
        /// <returns>Specified Return Type for the Function performed</returns>
        public static TResult WaitAndRun<T, TResult>( double milliSeconds, Func<T, TResult> func, T input ) => WaitAndRun( TimeSpan.FromMilliseconds( milliSeconds ), func, input );
    }
}
