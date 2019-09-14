using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPL.DataFlow.Extensions
{
    /// <summary>
    /// Logs the exception and events of dataflow pipeline
    /// </summary>
    public static class DataflowLogger
    {
        /// <summary>
        /// Logs the task is completed or faulted
        /// </summary>
        /// <param name="dataflowBlock">the dataflowBlock</param>
        public static void Log(this IDataflowBlock dataflowBlock)
        {
            dataflowBlock.Completion.ContinueWith(ex =>
                {
                    //Log exception
                },
                TaskContinuationOptions.OnlyOnFaulted);

            dataflowBlock.Completion.ContinueWith(result =>
                {
                   //Log on block completion
                },
                TaskContinuationOptions.None);
        }
    }
}
