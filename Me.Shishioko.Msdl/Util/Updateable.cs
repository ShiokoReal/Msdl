using System.Threading;
using System.Threading.Tasks;

namespace Me.Shishioko.Msdl.Util
{
    public sealed class Updateable<T>
    {
        private readonly SemaphoreSlim Sync;
        private T PostUpdate;
        private T PreUpdate;
        private bool Updated;
        public async Task Set(T data)
        {
            await Sync.WaitAsync();
            PostUpdate = data;
            Updated = true;
            Sync.Release();
        }
        public async Task<T> Get()
        {
            await Sync.WaitAsync();
            T data = PreUpdate;
            Sync.Release();
            return data;
        }
        public async Task<T> GetNext()
        {
            await Sync.WaitAsync();
            T data = PostUpdate;
            Sync.Release();
            return data;
        }
        public async Task<bool> Changed()
        {
            await Sync.WaitAsync();
            bool data = Updated;
            Sync.Release();
            return data;
        }
        internal Updateable(T value)
        {
            Sync = new(1, 1);
            PostUpdate = value;
            PreUpdate = value;
            Updated = true;
        }
        internal async Task<T> Update()
        {
            await Sync.WaitAsync();
            T data = PreUpdate = PostUpdate;
            Updated = false;
            Sync.Release();
            return data;
        }
    }
}
