using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using DeepMorphy;
using K3NA_Remastered_2.ModulesSystem.Modules;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Compairing;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Variables;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.Special;
using K3NA_Remastered_2.ModulesSystem.Submodules;

namespace K3NA_Remastered_2
{
    public static class Core
    {
        public static readonly MorphAnalyzer MorphAnalyzer = new MorphAnalyzer(true, true);
        public static readonly ProtocolsStorage ProtocolsStorage = new ProtocolsStorage();//automaticaly loades protocols
        public static readonly VariableStorage  GlobalVariables = new VariableStorage();//global variables storage
        public static readonly SubModulesContainer SubModules = new SubModulesContainer(new List<ISubModule> {new CommandsExecutor()});
        public static readonly ModulesContainer Modules = new ModulesContainer(/*new InnerDoubles<IModule>() { new SpeechModule(), new TestModule() }*/);
        public static void OnSpeech(string speech)
        {
            var speechPattern = new SSpeechPattern(speech);
            var proto = RelevanceAnalyzer.GetMaxRelevanceProtocol(speechPattern, ProtocolsStorage.Protocols);
            Console.WriteLine($"Protocol: {proto.Name}");
            var storage = new VariableStorage();
            storage.ExecuteRecognizedProtocol(proto, speechPattern);//конечная точка в обработке
        }
        private static void DebugProtocols()
        {
            foreach (var protocol in ProtocolsStorage.Protocols)
            {
                Console.WriteLine($"Loaded protocol: {protocol.Name}");
                Console.WriteLine("Commands: ");
                foreach (var c in protocol.GetCommands())
                {
                    Console.WriteLine($"    Command: {c.Name}");
                    var args = c.Arguments.Str();
                    if(args!="")
                        Console.WriteLine($"        Arguments: {args}");
                }
                Console.WriteLine("Pattern: ");
                foreach (var unit in protocol.GetPattern().Units/*.Where(unit => unit.Morph != null)*/)
                {
                    if (unit.IsVariable)
                    {
                        Console.WriteLine($"    Variable: {unit.VariableName} : {unit.TypeString}");
                        if(unit.Morph==null)continue;
                        foreach (var morph in unit.Morph)
                        {
                            Console.WriteLine($"        {morph?.Text} - {morph?.BestTag}");
                        }
                    }
                    else
                    {
                        if (unit.Morph == null) continue;
                        Console.WriteLine($"    Word [{unit.TypeString}]: ");
                        foreach (var morph in unit.Morph)
                        {
                            Console.WriteLine($"        {morph?.Text} - {morph?.BestTag}");
                        }
                        
                    }
                }
            }
        }
        private static void Test()
        {
            var phrases = new List<string>() { "Алёнка и другие и я"/*,"привет обыватель","найди","привет тебе цветок"*/};
            foreach (var phrase in phrases)
            {
                OnSpeech(phrase);
            }
            //double[,] array = new double[,]
            //{
            //    {100,100,100,100,100},
            //    {100,100,100,100,100},
            //    {100,100,100,100,100},
            //    {100,100,100,100,100},
            //    {100,100,100,100,100},
            //    {100,100,100,100,100},
            //    {100,100,100,100,100},
            //    {100,100,100,100,100},
            //};
            //var m1 = new Matrix(array);
            //var m2 = Matrix.CreateDiagonalic(Matrix.ExponentialRegression,5 + 3, 5);
            //var m3 = Matrix.CreateDiagonalic(Matrix.DefaultRegression, 5 + 3, 5);


            //Console.WriteLine(m1);
            //Console.WriteLine(m2*100);
            //Console.WriteLine(m1*m2);
            //Console.WriteLine(m3*100);
            //Console.WriteLine(m1 * m3);
            //Console.ReadKey();
        }
        private static string Str(this string[] a) => string.Join(';', a);
        private static void Main(string[] args)
        {
            Test();
            //DebugProtocols();
            SubModules.Init();//must be performed before accessing SubModules
            //Modules.Load(new InnerDoubles<IModule>(){new SpeechModule(), new TestModule()});

            //MBus.MakeRequest(new ModuleRequest("test","SRM","тестовое сообщение"));
            //MBus.MakeSpecialRequest("test SRM",MBus.SpecialRequestType.Subscribe);//subscribe test module to SRM messages
            //MBus.MakeSpecialRequest("test SRM", MBus.SpecialRequestType.Override);//subscribe test module to SRM messages for a once, but nobody will get this message



            //var result = MorphAnalyzer.Parse("слово", "привет","закрой","ибрагим","очаровательно","она");
            //foreach (var r in result)
            //{
            //    Console.WriteLine($"{r.Text}: {r.BestTag}");
            //}
            //DefaultProtocol proto = new DefaultProtocol();
            //proto.Construct(ProtocolsLoader.GetProtocols().First());

            //CommandsExecutor usage: SubModules.GetCommandsExecutor().
            Console.ReadKey();
        }
    }
}
