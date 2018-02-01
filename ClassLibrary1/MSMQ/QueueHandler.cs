using System;
using System.Messaging;

namespace LIB.MSMQ
{
    public static class MessageQueueHandler
    {
        //Peeks the message that matches the given label and waits until either a message with the specified label is available in the queue, or the time-out expires. 
        public static Message PeekByLabel(MessageQueue queue, string label, TimeSpan timeout)
        {
            queue.MessageReadPropertyFilter.Label = true;

            queue.MessageReadPropertyFilter.Body = false;
            queue.MessageReadPropertyFilter.Id = false;
            queue.MessageReadPropertyFilter.ArrivedTime = false;
            queue.MessageReadPropertyFilter.AdministrationQueue = false;
            queue.MessageReadPropertyFilter.AcknowledgeType = false;
            queue.MessageReadPropertyFilter.Acknowledgment = false;
            queue.MessageReadPropertyFilter.CorrelationId = false;

            //Returns and removes the message from the queue with a matching label or timeout
            MessageEnumerator enumerator = queue.GetMessageEnumerator2();
            DateTime maxtime = DateTime.Now.Add(timeout);

            while (DateTime.Now.CompareTo(maxtime) <= 0)
            {
                try
                {
                    while (enumerator.MoveNext(new TimeSpan(0, 0, 0)) && DateTime.Now.CompareTo(maxtime) <= 0)
                    {
                        if (enumerator.Current != null
                            && enumerator.Current.Label.Equals(label))
                        {
                            queue.MessageReadPropertyFilter.Body = true;
                            queue.MessageReadPropertyFilter.Id = true;
                            queue.MessageReadPropertyFilter.ArrivedTime = true;
                            queue.MessageReadPropertyFilter.AdministrationQueue = true;
                            queue.MessageReadPropertyFilter.AcknowledgeType = true;
                            queue.MessageReadPropertyFilter.Acknowledgment = false;
                            queue.MessageReadPropertyFilter.CorrelationId = false;

                            Message msg = enumerator.Current;
                            enumerator.Close();

                            return msg;
                        }
                    }

                    enumerator.Reset();
                }
                catch (MessageQueueException ex)
                {
                    if (ex.MessageQueueErrorCode.Equals(MessageQueueErrorCode.MessageAlreadyReceived)
                        || ex.MessageQueueErrorCode.Equals(MessageQueueErrorCode.MessageNotFound)
                        || ex.MessageQueueErrorCode.Equals(MessageQueueErrorCode.InvalidHandle))
                    {
                        enumerator.Reset();
                        continue;
                    }
                    else
                    {
                        enumerator.Close();
                        throw ex;
                    }
                }
            }

            return null;
        }
        //Receives the message that matches the given label and waits until either a message with the specified label is available in the queue, or the time-out expires. 
        public static Message ReceiveByLabel(MessageQueue queue, string label, TimeSpan timeout)
        {
            queue.MessageReadPropertyFilter.Label = true;

            queue.MessageReadPropertyFilter.Body = false;
            queue.MessageReadPropertyFilter.Id = false;
            queue.MessageReadPropertyFilter.ArrivedTime = false;
            queue.MessageReadPropertyFilter.AdministrationQueue = false;
            queue.MessageReadPropertyFilter.AcknowledgeType = false;
            queue.MessageReadPropertyFilter.Acknowledgment = false;
            queue.MessageReadPropertyFilter.CorrelationId = false;

            //Returns and removes the message from the queue with a matching label or timeout
            MessageEnumerator enumerator = queue.GetMessageEnumerator2();
            DateTime maxtime = DateTime.Now.Add(timeout);

            while (DateTime.Now.CompareTo(maxtime) <= 0)
            {
                try
                {
                    while (enumerator.MoveNext(new TimeSpan(0, 0, 0)) && DateTime.Now.CompareTo(maxtime) <= 0)
                    {
                        if (enumerator.Current != null
                            && enumerator.Current.Label.Equals(label))
                        {
                            queue.MessageReadPropertyFilter.Body = true;
                            queue.MessageReadPropertyFilter.Id = true;
                            queue.MessageReadPropertyFilter.ArrivedTime = true;
                            queue.MessageReadPropertyFilter.AdministrationQueue = true;
                            queue.MessageReadPropertyFilter.AcknowledgeType = true;
                            queue.MessageReadPropertyFilter.Acknowledgment = false;
                            queue.MessageReadPropertyFilter.CorrelationId = false;

                            Message msg = enumerator.RemoveCurrent();
                            enumerator.Close();

                            return msg;
                        }
                    }

                    enumerator.Reset();
                }
                catch (MessageQueueException ex)
                {
                    if (ex.MessageQueueErrorCode.Equals(MessageQueueErrorCode.MessageAlreadyReceived)
                        || ex.MessageQueueErrorCode.Equals(MessageQueueErrorCode.MessageNotFound)
                        || ex.MessageQueueErrorCode.Equals(MessageQueueErrorCode.InvalidHandle))
                    {
                        enumerator.Reset();
                        continue;
                    }
                    else
                    {
                        enumerator.Close();
                        throw ex;
                    }
                }
            }

            return null;
        }
    }
}
