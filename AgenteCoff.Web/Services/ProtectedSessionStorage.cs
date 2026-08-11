using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.JSInterop;

namespace AgenteCoff.Web.Services
{
    public sealed class ProtectedBrowserStorageResult<T>
    {
        public ProtectedBrowserStorageResult(T? value, bool success)
        {
            Value = value;
            Success = success;
        }

        public T? Value { get; }
        public bool Success { get; }
    }

    public class ProtectedSessionStorage
    {
        private readonly IJSRuntime jsRuntime;
        private readonly IDataProtector protector;
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, string> pendingWrites = new();

        public ProtectedSessionStorage(IJSRuntime jsRuntime, IDataProtectionProvider dp)
        {
            this.jsRuntime = jsRuntime;
            protector = dp.CreateProtector("AgenteCoff.ProtectedSessionStorage.v1");
        }

        public async ValueTask SetAsync<T>(string key, T value)
        {
            try
            {
                var json = JsonSerializer.Serialize(value);
                var bytes = Encoding.UTF8.GetBytes(json);
                var protectedBytes = protector.Protect(bytes);
                var base64 = Convert.ToBase64String(protectedBytes);
                await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", key, base64);
                try { await jsRuntime.InvokeVoidAsync("console.log", $"ProtectedSessionStorage: set {key}"); } catch { }
                Console.WriteLine($"ProtectedSessionStorage: Set key={key}");
                // if it previously existed as pending, remove
                pendingWrites.TryRemove(key, out _);
            }
            catch (Exception ex)
            {
                // If JS interop is not available (prerender), store pending value to flush later
                try
                {
                    var json = JsonSerializer.Serialize(value);
                    var bytes = Encoding.UTF8.GetBytes(json);
                    var protectedBytes = protector.Protect(bytes);
                    var base64 = Convert.ToBase64String(protectedBytes);
                    pendingWrites[key] = base64;
                    Console.WriteLine($"ProtectedSessionStorage: Set deferred for {key}");
                }
                catch (Exception inner)
                {
                    Console.WriteLine($"ProtectedSessionStorage: Set failed for {key}: {ex.Message}; deferred failed: {inner.Message}");
                }
            }
        }

        public async ValueTask<ProtectedBrowserStorageResult<T>> GetAsync<T>(string key)
        {
            try
            {
                var base64 = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", key);
                if (string.IsNullOrWhiteSpace(base64))
                {
                    Console.WriteLine($"ProtectedSessionStorage: Get {key} - not found");
                    // If there's a pending deferred value, return it
                    if (pendingWrites.TryGetValue(key, out var pendingBase64))
                    {
                        try
                        {
                            var protectedBytes2 = Convert.FromBase64String(pendingBase64);
                            var bytes2 = protector.Unprotect(protectedBytes2);
                            var json2 = Encoding.UTF8.GetString(bytes2);
                            var value2 = JsonSerializer.Deserialize<T>(json2);
                            Console.WriteLine($"ProtectedSessionStorage: Get {key} - returned deferred value");
                            return new ProtectedBrowserStorageResult<T>(value2, true);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"ProtectedSessionStorage: Get deferred failed for {key}: {ex.Message}");
                        }
                    }
                    return new ProtectedBrowserStorageResult<T>(default, false);
                }

                var protectedBytes = Convert.FromBase64String(base64);
                var bytes = protector.Unprotect(protectedBytes);
                var json = Encoding.UTF8.GetString(bytes);
                var value = JsonSerializer.Deserialize<T>(json);
                try { await jsRuntime.InvokeVoidAsync("console.log", $"ProtectedSessionStorage: get {key} success"); } catch { }
                Console.WriteLine($"ProtectedSessionStorage: Get key={key} success");
                return new ProtectedBrowserStorageResult<T>(value, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ProtectedSessionStorage: Get failed for {key}: {ex.Message}");
                return new ProtectedBrowserStorageResult<T>(default, false);
            }
        }

        public async ValueTask DeleteAsync(string key)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", key);
                try { await jsRuntime.InvokeVoidAsync("console.log", $"ProtectedSessionStorage: delete {key}"); } catch { }
                Console.WriteLine($"ProtectedSessionStorage: Delete key={key}");
                pendingWrites.TryRemove(key, out _);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ProtectedSessionStorage: Delete failed for {key}: {ex.Message}");
            }
        }

        public async Task FlushPendingAsync()
        {
            foreach (var kvp in pendingWrites.ToArray())
            {
                try
                {
                    await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", kvp.Key, kvp.Value);
                    pendingWrites.TryRemove(kvp.Key, out _);
                    try { await jsRuntime.InvokeVoidAsync("console.log", $"ProtectedSessionStorage: flushed {kvp.Key}"); } catch { }
                    Console.WriteLine($"ProtectedSessionStorage: Flushed key={kvp.Key}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ProtectedSessionStorage: Flush failed for {kvp.Key}: {ex.Message}");
                }
            }
        }

        public async Task<bool> IsJsRuntimeAvailableAsync()
        {
            try
            {
                // Try a benign JS call
                await jsRuntime.InvokeVoidAsync("console.debug", "ProtectedSessionStorage: ping");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
