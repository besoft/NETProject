using System;
using Zcu.StudentEvaluator.Domain.Test;

namespace TestApplication
{
     /// <summary>
    /// Tento program demonstruje naprosty zaklad hodnoceni studentu na predmetu.
    /// </summary>
    /// <remarks>
    ///Lze si to predstavit jako Excelovskou tabulku MxN, kde N je pocet studentu (pocet radek tabulky) a M je pocet sloupcu, 
    ///ktere mam k dispozici, pricemz v dobe prekladu jsou znamy jen 3 sloupce (osobni cislo, jmeno, prijmeni)
    ///a ostatni sloupce jsou stanoveny v dobe behu (napr. z konfiguracniho souboru), kazdy z techto sloupcu odpovida hodnoceni studentu v nejake kategorii.
    ///Protoze definice sloupcu (jmeno kategorie, min a max mozny pocet bodu) je spolecny pro kazdeho studenta, je vhodne definici udrzovat v pameti jen jednou, 
    ///jen jedna instance. Vlastni hodnoceni (pocet bodu a jejich zduvodneni) jsou uchovavany samozrejme pro kazdeho studenta.
    ///
    /// Tento prvni navrh je v mnohem nedokonaly:
    /// 1) Udaje o studentovi a udaje o jeho hodnoceni nejsou dostatecne oddeleny (metody pro soucty, apod. soucasti studenta)
    /// 2) Schema nelze snadno modifikovat (jedna se o pole)
    /// 3) Hodnoceni a schema jsou zcela nezavisle, tj. mohu specifikovat zcela odlisne schema a pole pro hodnoceni (viz TestRepository) 
    /// 4) Soucet vsech bodu nebere v uvahu Maxima
    /// 
    /// Ukazane principy: struct, property, interface, decimal, nullable type, override ToString, foreach
    /// </remarks>    
    class Program
    {

        static void Main(string[] args)
        {
            var repo = new TestRepository();

            Console.WriteLine("Number of students: " + repo.Students.Length);
            foreach (var st in repo.Students)
            {
                Console.WriteLine(st.ToString());
            }

            repo.Students[0].Evaluations[0].Points = 5.5m;
            repo.Students[0].Evaluations[0].Reason = "Bad design";

            repo.Students[0].Evaluations[1].Points = 4m;
            repo.Students[0].Evaluations[1].Reason = "Bad implementation";

            repo.Students[0].Evaluations[2].Points = 2m;

            repo.Students[1].Evaluations[0].Points = 15m;

            
            foreach (var st in repo.Students)
            {
                Console.WriteLine(st.ToString());
            }           

            Console.ReadLine();
        }
    }
}
