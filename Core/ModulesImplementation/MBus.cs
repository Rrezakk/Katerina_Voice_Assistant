//#define deb
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace K3NA_Remastered_2.ModulesImplementation
{
    internal static class MBus
    {
        private static readonly object RequestsLocker = new object ();
        private static readonly object ModulesLocker = new object();
        private static readonly Dictionary<string, ModulesImplementation.IModule> Modules = new Dictionary<string, ModulesImplementation.IModule>();
        private static readonly Queue<ModulesImplementation.ModuleRequest> Requests = new Queue<ModulesImplementation.ModuleRequest>();
        public enum SpecialRequestType
        {
            Subscribe,
            Unsubscribe,
            Override
        }
        public static void AuthModule(ModulesImplementation.IModule module)
        {
#if deb
Console.WriteLine($"Auth module: {module.Name}");
#endif
            lock (ModulesLocker)
            {
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
Console.WriteLine($"Request in MBus: {request.From} {request.To} {request.Message}");
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
