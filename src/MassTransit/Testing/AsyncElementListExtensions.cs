namespace MassTransit.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;


    public static class AsyncElementListExtensions
    {
        public static async Task<TElement> First<TElement>(this IAsyncEnumerable<TElement> elements)
            where TElement : class
        {
            await foreach (var element in elements.ConfigureAwait(false))
                return element;

            throw new InvalidOperationException("Message List was empty, or timed out");
        }

        public static async Task<int> Count<TElement>(this IAsyncEnumerable<TElement> elements)
            where TElement : class
        {
            var count = 0;
            await foreach (var element in elements.ConfigureAwait(false))
                count++;

            return count;
        }

        public static int Count<TElement>(this IAsyncElementList<TElement> elements, CancellationToken cancellationToken = default)
            where TElement : class, IAsyncListElement
        {
            return elements.Select(x => true, cancellationToken).Count();
        }

#if NET10_0_OR_GREATER
        /// <summary>
        /// Takes the first <paramref name="quantity"/> elements from the async enumerable.
        /// On .NET 10+, delegates to System.Linq.AsyncEnumerable.Take.
        /// </summary>
        public static IAsyncEnumerable<TElement> TakeElements<TElement>(this IAsyncEnumerable<TElement> elements, int quantity)
            where TElement : class
        {
            return System.Linq.AsyncEnumerable.Take(elements, quantity);
        }
#else
        public static async IAsyncEnumerable<TElement> Take<TElement>(this IAsyncEnumerable<TElement> elements, int quantity)
            where TElement : class
        {
            var count = 0;
            await foreach (var element in elements.ConfigureAwait(false))
            {
                yield return element;

                count++;
                if (count == quantity)
                    yield break;
            }
        }

        /// <summary>
        /// Alias for Take, provided for source compatibility with .NET 10+
        /// </summary>
        public static IAsyncEnumerable<TElement> TakeElements<TElement>(this IAsyncEnumerable<TElement> elements, int quantity)
            where TElement : class
        {
            return elements.Take(quantity);
        }
#endif

        public static async Task<TElement> FirstOrDefault<TElement>(this IAsyncEnumerable<TElement> elements)
            where TElement : class
        {
            await foreach (var element in elements.ConfigureAwait(false))
                return element;

            return default;
        }

        public static async Task<bool> Any<TElement>(this IAsyncEnumerable<TElement> elements)
            where TElement : class
        {
            try
            {
                await foreach (var _ in elements.ConfigureAwait(false))
                    return true;
            }
            catch (OperationCanceledException)
            {
            }

            return false;
        }

        public static async IAsyncEnumerable<TResult> Select<TElement, TResult>(this IAsyncEnumerable<TElement> elements)
            where TElement : class
            where TResult : class
        {
            await foreach (var entry in elements.ConfigureAwait(false))
            {
                if (entry is TResult result)
                    yield return result;
            }
        }

        public static void Deconstruct(this ISentMessage sent, out object message, out SendContext context)
        {
            context = sent.Context;
            message = sent.MessageObject;
        }

        public static void Deconstruct<TMessage>(this ISentMessage<TMessage> sent, out TMessage message, out SendContext context)
            where TMessage : class
        {
            context = sent.Context;
            message = sent.Context.Message;
        }
    }
}
