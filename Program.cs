using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Spire.Doc;
using Spire.Doc.Documents;

namespace work.logic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // TODO: EJ, ERL, xls
            /*

            // EJ
            // Большие блоки разделены между собой 40+
            // В больших Транзакция разделение 10-
            // TODO: Надо определить какие последовательности это транзакции, какие сессии оператора
            // 1. Определить все разновидности и подсерии
            //  a) КЛИЕНТ NDC
            //  Клиентская сессия от +++ до ----
            //  Состоит из блоков разделенных 10- 
            //  строка с ошибкой выглядит так _903_CANNOT PROCESS AMOUNT, если в блоке встречается такая строка, значит блок с ошибкой

            //  ПРЕДВОРИТЕЛЬНО: Обрабатываем блок как Словарь: Берем слово до разделителя : и остаток строки в значение               
            //  Если в строке Больше одного : то это дата время? что делать?
            //  Если строка не соответствует условию то строки кладем в отдельный List<string> По каждому блоку

            //  В итоге должно получиться class client:transaction{List<sub>; time;cardholder?;} class sub{dict<string:string>;List<string>;}
            //  б) Сессия Оператора
            //  От С Е С С И Я   О П Е Р А Т О Р А До ++++ сессии оператора или  
            // 2. Найти характерные различия, по которым можно определеить ту или иную сущность
            // 3. Выделить характеристики в класс унаследованный от единого
            //    Базовый класс содержить время дату ... определить общие характеристики

            // 27.12.2022
            // TODO1: Правильно определить Типы транзакций по тексту и Заполнить списки соответственно.
            //if(+) while(!-) // Добавить обработку исключения, когда по ошибке прекращено
            //if(+1 == +)
            // Собрать List и обработать как Клиент
            //if(+1 == СЕССИЯ ОПЕРАТОРА) Собрать в лист и обработать как Оператор
            //if(+1 == ****************************************) Собрать в лист и обработать. Это перезагрузка 

            */

            string path = "D:\\temp\\work\\ej.txt";
            string path1 = "D:\\temp\\work\\exp.doc";
            var txt = File.ReadAllLines(path);

            // Разделитель string('+', 40); между транзакциями

            //TODO1:
            // Проходим построчно

            // Замечания:
            // 40* где бы не нашли просто определять в массив отдельный, так как эта хрень может быть в любом месте 
            // от 40* до 40*





            Document doc = new Document(path1);


            //Section sec = doc.AddSection();
            //Paragraph par = sec.AddParagraph();

            //par.AppendText("ajdkjdkad");

            //doc.SaveToFile(path1, Spire.Doc.FileFormat.Docx);
            //System.Diagnostics.Process.Start(path1);









            //EJournal ej = new EJournal(path);
            int t = 0;

        }
    }
}
