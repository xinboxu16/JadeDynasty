using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Assets.Scripts.Test
{
    class State
    {
        public object o = true;
        public object from = "";

        public State(object o, object from)
        {
            this.o = o;
            this.from = from;
        }
    }
    class Test1
    {
        private bool deadlocked = true;

        //这个方法用到了lock，我们希望lock的代码在同一时刻只能由一个线程访问
        public void LockMe(State st)
        {
            lock(this)
            {
                while(deadlocked)
                {
                    deadlocked = (bool)st.o;
                    Console.WriteLine("Foo: I am locked :(" + st.from);
                    Thread.Sleep(1000);
                    deadlocked = false;
                }
            }
        }

        //所有线程都可以同时访问的方法
        public void DoNotLockMe()
        {
            Console.WriteLine("I am not locked :)");
        }
    }

    class Test2
    {
        private bool deadlocked = true;
        private object locker = new object();
        //这个方法用到了lock，我们希望lock的代码在同一时刻只能由一个线程访问
        public void LockMe(object st)
        {
            lock (locker)
            {
                while (deadlocked)
                {
                    State s = (State)st;
                    deadlocked = (bool)s.o;
                    Console.WriteLine("Foo: I am locked :(" + s.from);
                    Thread.Sleep(2000);
                    deadlocked = false;
                }
            }
        }
        //所有线程都可以同时访问的方法
        public void DoNotLockMe()
        {
            Console.WriteLine("I am not locked :)");
        }
    }

    class ThreadTest
    {
        public static void Main(string[] args)
        {
            Test2 test1 = new Test2();
            //在t1线程中调用LockMe，并将deadlock设为true（将出现死锁）
            Thread t1 = new Thread(test1.LockMe);
            t1.Start(new State(true, "Thread"));
            Thread.Sleep(100);

            //在主线程中lock c1
            lock(test1)
            {
                //调用没有被lock的方法
                test1.DoNotLockMe();
                //调用被lock的方法，并试图将deadlock解除
                test1.LockMe(new State(true, "Main"));
            }
        }
    }
}
