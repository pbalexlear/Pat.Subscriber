﻿using System;
using System.Threading.Tasks;
using Pat.Subscriber.MessageProcessing;

namespace Pat.Subscriber.RateLimiterPolicy
{
    public class RateLimiterMessageProcessingBehaviour : IMessageProcessingBehaviour
    {
        private readonly RateLimiterBatchProcessingBehaviour _rateLimiterBatchProcessingBehaviour;

        public RateLimiterMessageProcessingBehaviour(RateLimiterBatchProcessingBehaviour rateLimiterBatchProcessingBehaviour)
        {
            _rateLimiterBatchProcessingBehaviour = rateLimiterBatchProcessingBehaviour;
        }

        public async Task Invoke(Func<MessageContext, Task> next, MessageContext messageContext)
        {
            try
            {
                await next(messageContext).ConfigureAwait(false);
                _rateLimiterBatchProcessingBehaviour.MessageCompleted();
            }
            catch (Exception ex)
            {
                _rateLimiterBatchProcessingBehaviour.MessageFailed(ex);
                throw;
            }
        }
    }
}