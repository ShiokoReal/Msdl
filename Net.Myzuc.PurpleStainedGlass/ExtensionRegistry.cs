using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Net.Myzuc.PurpleStainedGlass
{
    public static class ExtensionRegistry
    {
        private static readonly SemaphoreSlim Sync = new(1, 1);
        private static readonly List<Extension> Extensions = [];
        public static async Task<bool> RegisterAsync(Extension extension)
        {
            await Sync.WaitAsync();
            bool success = !Extensions.Contains(extension);
            if (success) Extensions.Add(extension);
            Sync.Release();
            if (success) await extension.OnRegisterAsync();
            return success;
        }
        internal static async Task ExecuteParallelAsync(Func<Extension, Task> action)
        {
            await Sync.WaitAsync();
            Extension[] extensions = [..Extensions];
            Sync.Release();
            await Task.WhenAll(Extensions.Select(action));
        }
        internal static async Task ExecuteOrderlyAsync(Func<Extension, Task> action)
        {
            await Sync.WaitAsync();
            Extension[] extensions = [..Extensions];
            Sync.Release();
            await Task.WhenAll(Extensions.Select(action));
        }
    }
}
