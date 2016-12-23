using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class PublishSubscribeSystem
    {
        private class ReceiptInfo
        {
            public string name_;
            public Delegate delegate_;
            public ReceiptInfo() { }
            public ReceiptInfo(string n, Delegate d)
            {
                name_ = n;
                delegate_ = d;
            }
        }

        private Dictionary<string, Delegate> subscribers_ = new Dictionary<string, Delegate>();

        private bool run_in_logic_thread_ = true;
        public bool RunInLogicThread
        {
            get { return run_in_logic_thread_; }
            set { run_in_logic_thread_ = value; }
        }

        //注册ui
        public object Subscribe(string ev_name, string group, MyAction subscriber) { return AddSubscriber(ev_name, group, subscriber); }
        public object Subscribe<T0, T1>(string ev_name, string group, MyAction<T0, T1> subscriber) { return AddSubscriber(ev_name, group, subscriber); }
        public object Subscribe<T0>(string ev_name, string group, MyAction<T0> subscriber) { return AddSubscriber(ev_name, group, subscriber); }

        private object AddSubscriber(string ev_name, string group, Delegate d)
        {
            Delegate source;
            string key = group + '#' + ev_name;
            if (subscribers_.TryGetValue(key, out source))
            {
                if (null != source)
                    source = Delegate.Combine(source, d);
                else
                    source = d;
            }
            else
            {
                source = d;
            }
            subscribers_[key] = source;
            return new ReceiptInfo(key, d);
        }

        public void Publish(string ev_name, string group, params object[] parameters)
        {
            try
            {
                if (RunInLogicThread)
                    LogSystem.Info("Publish {0} {1}", ev_name, group);
                else
                    LogicSystem.LogicLog("Publish {0} {1}", ev_name, group);

                Delegate d;
                string key = group + '#' + ev_name;
                if (subscribers_.TryGetValue(key, out d))
                {
                    if (null == d)
                    {
                        if (RunInLogicThread)
                            LogSystem.Error("Publish {0} {1}, Subscriber is null, Remove it", ev_name, group);
                        else
                            LogicSystem.LogicErrorLog("Publish {0} {1}, Subscriber is null, Remove it", ev_name, group);
                        subscribers_.Remove(key);
                    }
                    else
                    {
                        d.DynamicInvoke(parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                if (RunInLogicThread)
                    LogSystem.Error("PublishSubscribe.Publish({0},{1}) exception:{2}\n{3}", ev_name, group, ex.Message, ex.StackTrace);
                else
                    LogicSystem.LogicErrorLog("PublishSubscribe.Publish({0},{1}) exception:{2}\n{3}", ev_name, group, ex.Message, ex.StackTrace);
            }
        }

        public void Unsubscribe(object receipt)
        {
            ReceiptInfo r = receipt as ReceiptInfo;
            Delegate d;
            if (null != r && subscribers_.TryGetValue(r.name_, out d))
            {
                subscribers_[r.name_] = Delegate.Remove(d, r.delegate_);
            }
        }
    }
}
