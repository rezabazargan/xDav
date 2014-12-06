using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDav.Settings
{
    internal class XdavEvents
    {
        private static XdavEvents _current;
        internal static XdavEvents Current
        {
            get
            {
                if (_current == null)
                    _current = new XdavEvents();
                return _current;
            }
        }
        private List<Action<XdavOnProcessingEventArg>> XdavOnProcessing { get; set; }
        private List<Action<XdavOnProcessedEventArg>> XdavOnProcessed { get; set; }
        private List<Action<XdavOnExceptionEventArg>> XdavOnException { get; set; }


        internal void AddXdavOnProcessing(Action<XdavOnProcessingEventArg> handler)
        {
            if (XdavOnProcessing == null)
                XdavOnProcessing = new List<Action<XdavOnProcessingEventArg>>();
            XdavOnProcessing.Add(handler);
        }
        internal void AddXdavOnProcessed (Action<XdavOnProcessedEventArg> handler)
        {
            if (XdavOnProcessed == null)
                XdavOnProcessed = new List<Action<XdavOnProcessedEventArg>>();
            XdavOnProcessed.Add(handler);
        }
        internal void AddXdavOnException(Action<XdavOnExceptionEventArg> handler)
        {
            if (XdavOnException == null)
                XdavOnException = new List<Action<XdavOnExceptionEventArg>>();
            XdavOnException.Add(handler);
        }


        internal XdavOnProcessingEventArg XdavOnProcessingHandler(XdavOnProcessingEventArg e)
        {
            if (XdavOnProcessing != null)
                XdavOnProcessing.ForEach(p => p(e));
            return  e;
        }
        internal XdavOnProcessedEventArg XdavOnProcessedHandler(XdavOnProcessedEventArg e)
        {
            if (XdavOnProcessed != null)
                XdavOnProcessed.ForEach(p => p(e));
            return e;
        }

        internal XdavOnExceptionEventArg XdavOnExceptionHandler(XdavOnExceptionEventArg e)
        {
            if (XdavOnException != null)
                XdavOnException.ForEach(p => p(e));
            return e;
        }

    }

    public class XdavEventFactory
    {
        public XdavEventFactory  OnProcessing(Action<XdavOnProcessingEventArg> handler)
        {
            XdavEvents.Current.AddXdavOnProcessing(handler);
            return this;
        }
        public XdavEventFactory OnProcessed(Action<XdavOnProcessedEventArg> handler)
        {
            XdavEvents.Current.AddXdavOnProcessed(handler);
            return this;
        }
        public XdavEventFactory OnException(Action<XdavOnExceptionEventArg> handler)
        {
            XdavEvents.Current.AddXdavOnException(handler);
            return this;
        }
    }

}
