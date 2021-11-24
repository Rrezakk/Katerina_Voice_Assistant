//#define deb

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Implementation
{
    internal static class MBus
    {
        private static readonly object RequestsLocker = new object ();
        private static readonly object ModulesLocker = new object();
        private static readonly Dictionary<string, Module> Modules = new Dictionary<string, Module>();
        private static readonly Queue<ModuleRequest> Requests = new Queue<ModuleRequest>();
        public enum SpecialRequestType
        {
            Subscribe,
            Unsubscribe,
            Override
        }
        public static void AuthModule(Module module)
        {
#if deb
Console.WriteLine($"Auth module: {module.Name}");
#endif
            lock (ModulesLocker)
            {
                if (Modules.ContainsKey(module.Name))
                {
                    throw new Exception($"It's not allowed to auth module twice: {module.Name}");
                }
                Modules.Add(module.Name, module);
            }
        }

        public static bool MakeSpecialRequest(string request,SpecialRequestType requestType)
        {
#if deb
Console.WriteLine($"SpecialRequest in MBus: {request}");
#endif
            switch (requestType)
            {
                case SpecialRequestType.Subscribe:
                    try
                    {
                        var mas = request.Split(' ');
                        lock (ModulesLocker)
                        {
                            Modules[mas[1]].Subscribe(mas[0]);
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"subscribing module exception: {e}");
                        return false;
                    }
                case SpecialRequestType.Unsubscribe:
                    try
                    {
                        var mas = request.Split(' ');
                        lock (ModulesLocker)
                        {
                            Modules[mas[1]].UnSubscribe(mas[0]);
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"subscribing module exception: {e}");
                        return false;
                    }
                case SpecialRequestType.Override:
                    try
                    {
                        var mas = request.Split(' ');
                        lock (ModulesLocker)
                        {

                            Modules[mas[1]].Override(mas[0]);
                        }

                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"subscribing module exception: {e}");
                        return false;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(requestType), requestType, null);
            }
        }
        public static void MakeRequest(ModuleRequest request)
        {
#if deb
Console.WriteLine($"Enqueued request in MBus: {request}");
#endif
            lock (RequestsLocker)
            {
                Requests.Enqueue(request);
                
            }
        }
        public static void ListModules()
        {
            lock (ModulesLocker)
            {
#if deb
Console.WriteLine("Authenticated modules: ");
#endif
                foreach (var module in Modules)
                {
                    Console.WriteLine(module.Key);
                }
            }
        }
        public static async void Start()
        {
            await Task.Run(Cycle);
        }
        private static Task Cycle()
        {
            while (true)
            {
                lock (RequestsLocker)
                {
                    if (Requests.TryDequeue(out var request))
                    {
#if deb
Console.WriteLine($"Sending request to {request.To} : {request.Message}");
#endif
                        Modules[request.To].InBuffer.Put(request);
                    }
                }
                Thread.Sleep(50);
            }
        }
    }
}
