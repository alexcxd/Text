using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SampleCode.DesignPattern.OtherPatterns
{
    /// <summary>
    /// 对象池模式
    /// 通过循环使用(重复使用)对象，减少资源在初始化和释放时的昂贵损耗。
    /// </summary>
    public class ObjectPoolPattern
    {
        /// <summary>
        /// 测试
        /// </summary>
        public void ObjectPoolPatternMain()
        {
            var pool = new DatabaseConnectionPool("aaaa");
            pool.Init();

            var databaseConnectionList = new List<DatabaseConnection>();
            for (int i = 0; i < 30; i++)
            {
                var databaseConnect = pool.CheckOut();
                if (databaseConnect == null)
                {
                    // 对象池内对象用完了
                }
                else
                {
                    // 提取对象成功
                    databaseConnectionList.Add(databaseConnect);
                }

                if (i >= 20)
                {
                    pool.CheckIn(databaseConnectionList.FirstOrDefault());
                }
            }

        }

        #region 对象池模式

        /// <summary>
        /// 抽象对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public abstract class ObjectPool<T>
        {
            private Dictionary<T, DateTime> _unlocked;
            private Dictionary<T, DateTime> _locked;
            private readonly object _sync = new object();

            /// <summary>
            /// 构造
            /// </summary>
            public ObjectPool()
            {
                _locked = new Dictionary<T, DateTime>();
                _unlocked = new Dictionary<T, DateTime>();
            }

            /// <summary>
            /// 初始化
            /// </summary>
            public void Init()
            {

                for (int i = 0; i < 10; i++)
                {
                    T objectValue = Create();
                    if (!_unlocked.ContainsKey(objectValue))
                    {
                        _unlocked.Add(objectValue, DateTime.Now);
                    }
                }
            }

            /// <summary>
            /// 创建对象
            /// </summary>
            /// <returns></returns>
            protected abstract T Create();

            /// <summary>
            /// 提取对象
            /// </summary>
            /// <returns></returns>
            public T CheckOut()
            {
                lock (_sync)
                {
                    if (_unlocked.Count > 0)
                    {
                        // 找到一个可以使用的对象
                        var item = _unlocked.FirstOrDefault();
                        _unlocked.Remove(item.Key);
                        _locked.Add(item.Key, DateTime.UtcNow);
                        return item.Key;
                    }

                    return default(T);
                }
            }

            /// <summary>
            /// 回收对象
            /// </summary>
            /// <param name="reusable"></param>
            public void CheckIn(T reusable)
            {
                lock (_sync)
                {
                    _locked.Remove(reusable);
                    _unlocked.Add(reusable, DateTime.UtcNow);
                }
            }
        }

        /// <summary>
        /// 具体对象池
        /// </summary>
        public class DatabaseConnectionPool : ObjectPool<DatabaseConnection>
        {
            private readonly string connectionString;

            public DatabaseConnectionPool(string connectionString)
            {
                this.connectionString = connectionString;
            }

            protected override DatabaseConnection Create()
            {
                return new DatabaseConnection(connectionString);
            }

        }

        /// <summary>
        /// 对象
        /// </summary>
        public class DatabaseConnection : IDisposable
        {
            // do some heavy works
            public DatabaseConnection(string connectionString)
            {
            }

            public bool IsOpen { get; set; }

            // release something
            public void Dispose()
            {
            }
        }


        #endregion

    }
}