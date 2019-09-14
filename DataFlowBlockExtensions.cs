using System;
using System.Threading.Tasks.Dataflow;

namespace TPL.DataFlow.Extensions
{
    /// <summary>
    /// Extension methods to help chain dataflow blocks.
    /// </summary>
    internal static class DataflowBlockExtensions
    {
        /// <summary>
        /// Adds condition to the next block
        /// </summary>
        /// <typeparam name="TInput">The input</typeparam>
        /// <param name="source">The source</param>
        /// <param name="condition">The condition</param>
        /// <returns>ISourceBlock</returns>
        public static ISourceBlock<TInput> If<TInput>(
            this ISourceBlock<TInput> source,
            Predicate<TInput> condition)
        {
            var broadcast = new BroadcastBlock<TInput>(null);
            source.LinkTo(broadcast, condition);
            return broadcast;
        }

        /// <summary>
        /// Links source to multiple targets 
        /// </summary>
        /// <typeparam name="TInput">The input</typeparam>
        /// <param name="source">The source</param>
        /// <param name="targets">he targets</param>
        public static void Next<TInput>(
            this ISourceBlock<TInput> source,
            params ITargetBlock<TInput>[] targets)
        {
            var broadcast = new BroadcastBlock<TInput>(null);

            source.LinkTo(broadcast);

            foreach (var target in targets)
            {
                broadcast.LinkTo(target);
            }
        }

        /// <summary>
        /// Links source to a target block
        /// </summary>
        /// <typeparam name="TInput">The input</typeparam>
        /// <typeparam name="TOutput">The output</typeparam>
        /// <param name="source">The source</param>
        /// <param name="target">The target</param>
        /// <returns>ISourceBlock</returns>
        public static ISourceBlock<TOutput> Next<TInput, TOutput>(
            this ISourceBlock<TInput> source,
            IPropagatorBlock<TInput, TOutput> target)
        {
            source.LinkTo(target);
            return target;
        }

        /// <summary>
        /// Links source to two targets
        /// </summary>
        /// <typeparam name="TInput">The input</typeparam>
        /// <typeparam name="TOutput">The output</typeparam>
        /// <param name="source">The source</param>
        /// <param name="first">The first target</param>
        /// <param name="second">The second target</param>
        /// <returns>ISourceBlock</returns>
        public static ISourceBlock<TOutput> Next<TInput, TOutput>(
            this ISourceBlock<TInput> source,
            IPropagatorBlock<TInput, TOutput> first,
            IPropagatorBlock<TInput, TOutput> second)
        {
            var broadcast = new BroadcastBlock<TInput>(null);
            source.LinkTo(broadcast);
            broadcast.LinkTo(first);
            broadcast.LinkTo(second);

            return Aggregate(first, second);
        }

        /// <summary>
        /// Links source to three targets
        /// </summary>
        /// <typeparam name="TInput">The input</typeparam>
        /// <typeparam name="TOutput">The output</typeparam>
        /// <param name="source">The source</param>
        /// <param name="first">The first target</param>
        /// <param name="second">The second target</param>
        /// <param name="third">The third target</param>
        /// <returns>ISourceBlock</returns>
        public static ISourceBlock<TOutput> Next<TInput, TOutput>(
            this ISourceBlock<TInput> source,
            IPropagatorBlock<TInput, TOutput> first,
            IPropagatorBlock<TInput, TOutput> second,
            IPropagatorBlock<TInput, TOutput> third)
        {
            var broadcast = new BroadcastBlock<TInput>(null);
            source.LinkTo(broadcast);
            broadcast.LinkTo(first);
            broadcast.LinkTo(second);
            broadcast.LinkTo(third);

            return Aggregate(first, second, third);
        }

        /// <summary>
        /// Aggregates the results of two blocks
        /// </summary>
        /// <typeparam name="T">Specifies the type of data accepted by the blocks</typeparam>
        /// <param name="target1">target1</param>
        /// <param name="target2">target2</param>
        /// <returns>JoinBlock</returns>
        private static ISourceBlock<T> Aggregate<T>(
            ISourceBlock<T> target1,
            ISourceBlock<T> target2)
        {
            var joinBlock = new JoinBlock<T, T>();
            target1.LinkTo(joinBlock.Target1);
            target2.LinkTo(joinBlock.Target2);

            var transformBlock = new TransformBlock<Tuple<T, T>, T>(data => data.Item1);
            joinBlock.LinkTo(transformBlock);
            return transformBlock;
        }

        /// <summary>
        /// Aggregates the results of three blocks
        /// </summary>
        /// <typeparam name="T">Specifies the type of data accepted by the block</typeparam>
        /// <param name="target1">target1</param>
        /// <param name="target2">target2</param>
        /// <param name="target3">target3</param>
        /// <returns>ISourceBlock</returns>
        private static ISourceBlock<T> Aggregate<T>(
            ISourceBlock<T> target1,
            ISourceBlock<T> target2,
            ISourceBlock<T> target3)
        {
            var joinBlock = new JoinBlock<T, T, T>();
            target1.LinkTo(joinBlock.Target1);
            target2.LinkTo(joinBlock.Target2);
            target3.LinkTo(joinBlock.Target3);

            var transformBlock = new TransformBlock<Tuple<T, T, T>, T>(data => data.Item1);
            joinBlock.LinkTo(transformBlock);
            return transformBlock;
        }
    }
}
