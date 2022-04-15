//#define deb //other diagnostics
namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.SRM.AudioEssentials
{
    public class PreProcessor
        {
            private readonly Ema _ema1 = new Ema();
            public PreProcessor()
            {
                _ema1.Alpha = 0.9f;
            }

            public short[] ProcessTreshold(short[] inputshorts)
            {
                var start = 0; //Индекс подрезки
                var end = 0; //Индекс подрезки

                for (var i = 0; i < inputshorts.Length; i++)
                {
                    if (_ema1.Iterate(inputshorts[i]) < Configuration.Treshold) continue;
                    //Console.WriteLine("Max: "+inputshorts[i]);
                    start = i - Configuration.PreRecordBytes;
                    start = start > 0 ? start : 0;
                    break;
                } //прямой перебор для поиска восходящего фронта
                end = inputshorts.Length;

                var output = new short[end - start]; //вместилище результата
                var ctr2 = 0; //указатель для итеративной записи
                for (var i = start; i < end; i++) output[ctr2++] = inputshorts[i];
#if deb
                //диагностика
                Console.WriteLine($"Input: {inputshorts.Length}");
                Console.WriteLine($"Start: {start}");
                Console.WriteLine($"End: {end}");
                Console.WriteLine($"Output: {output.Length}");
#endif
                return output;
            }
        }
}
