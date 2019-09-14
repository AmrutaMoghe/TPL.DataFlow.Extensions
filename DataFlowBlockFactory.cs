using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.DataFlow.Extensions
{
    /// <summary>
    /// Creates object of Dataflow block
    /// </summary>
    public static class DataFlowBlockFactory
    {
        /// <summary>
        /// Creates Instance of TransformBlock
        /// </summary>
        /// <typeparam name="TInput">Specifies the type of data received by the block</typeparam>
        /// <typeparam name="TOutput">Specifies the type of data output by the block</typeparam>
        /// <param name="transform">transform logic</param>
        /// <returns>TransformBlock</returns>
        public static TransformBlock<TInput, TOutput> CreateInstance<TInput, TOutput>(Func<TInput, TOutput> transform)
        {
            var transformBlock = new TransformBlock<TInput, TOutput>(transform);
            transformBlock.Log();
            return transformBlock;
        }

        /// <summary>
        /// Creates Instance of TransformBlock for async funcs
        /// </summary>
        /// <typeparam name="TInput">Specifies the type of data received by the block</typeparam>
        /// <typeparam name="TOutput">Specifies the type of data output by the block</typeparam>
        /// <param name="transform">transform logic</param>
        /// <returns>TransformBlock</returns>
        public static TransformBlock<TInput, TOutput> CreateInstance<TInput, TOutput>(Func<TInput, Task<TOutput>> transform)
        {
            var transformBlock = new TransformBlock<TInput, TOutput>(transform);
            transformBlock.Log();
            return transformBlock;
        }

        /// <summary>
        /// Creates Instance of Action block
        /// </summary>
        /// <typeparam name="TInput">Specifies the type of data received by the block</typeparam>
        /// <param name="action">action logic</param>
        /// <returns>ActionBlock</returns>
        public static ActionBlock<TInput> CreateInstance<TInput>(Action<TInput> action)
        {
            var actionBlock = new ActionBlock<TInput>(action);
            actionBlock.Log();
            return actionBlock;
        }
    }
}
